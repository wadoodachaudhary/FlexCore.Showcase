using GovAiAcademy.Data;
using GovAiAcademy.Models;

namespace GovAiAcademy.Services;

/// <summary>
/// In-memory catalog, rosters, and attendance for the sample academy.
/// A restart clears marks. Production should replace this with a database.
/// </summary>
public sealed class AcademyStore
{
    private readonly Lock _gate = new();
    private readonly List<Person> _people;
    private readonly List<Course> _courses;
    private readonly List<Cohort> _cohorts;
    private readonly List<Enrollment> _enrollments;
    private readonly List<AttendanceRecord> _attendance;
    private readonly List<ExerciseProgress> _exercises;
    private readonly Dictionary<string, string> _notes = new(StringComparer.Ordinal);

    public AcademyStore()
        : this(AcademySeed.Create())
    {
    }

    public AcademyStore(AcademySeedBundle seed)
    {
        PedagogyRules.EnsureCatalogIsConsistent(seed.Courses);
        _people = seed.People;
        _courses = seed.Courses;
        _cohorts = seed.Cohorts;
        _enrollments = seed.Enrollments;
        _attendance = seed.Attendance;
        _exercises = seed.ExerciseProgress;
    }

    public IReadOnlyList<Person> People
    {
        get { lock (_gate) return _people.ToList(); }
    }

    public IReadOnlyList<Course> Courses
    {
        get { lock (_gate) return _courses.ToList(); }
    }

    public IReadOnlyList<Cohort> Cohorts
    {
        get { lock (_gate) return _cohorts.ToList(); }
    }

    public IReadOnlyList<Person> DevPersonas =>
        People.Where(p => p.DevPersona).OrderBy(p => p.Role).ThenBy(p => p.DisplayName).ToList();

    public Person? FindPerson(string id)
    {
        lock (_gate) return _people.FirstOrDefault(p => p.Id == id);
    }

    public Course? FindCourse(string id)
    {
        lock (_gate) return _courses.FirstOrDefault(c => c.Id == id);
    }

    public Cohort? FindCohort(string id)
    {
        lock (_gate) return _cohorts.FirstOrDefault(c => c.Id == id);
    }

    public IReadOnlyList<Course> FilterCourses(CatalogFilter filter)
    {
        lock (_gate)
        {
            IEnumerable<Course> query = _courses;
            if (filter.Level is { } level)
                query = query.Where(c => c.Level == level);
            if (filter.Role is { } role)
                query = query.Where(c => c.Role == role);
            if (filter.Delivery is { } delivery)
                query = query.Where(c => c.Delivery == delivery);
            if (filter.Approval is { } approval)
                query = query.Where(c => c.Approval == approval);
            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                var text = filter.Text.Trim();
                query = query.Where(c =>
                    c.Title.Contains(text, StringComparison.OrdinalIgnoreCase)
                    || c.Summary.Contains(text, StringComparison.OrdinalIgnoreCase)
                    || c.Scenario.Contains(text, StringComparison.OrdinalIgnoreCase)
                    || AcademyLabels.Role(c.Role).Contains(text, StringComparison.OrdinalIgnoreCase));
            }

            return query.OrderBy(c => c.Approval).ThenBy(c => c.Level).ThenBy(c => c.Title).ToList();
        }
    }

    public IReadOnlyList<Cohort> CohortsForCourse(string courseId)
    {
        lock (_gate) return _cohorts.Where(c => c.CourseId == courseId).OrderBy(c => c.Name).ToList();
    }

    public int EnrollmentCount(string cohortId)
    {
        lock (_gate) return _enrollments.Count(e => e.CohortId == cohortId);
    }

    public EnrollmentAttempt Enroll(string personId, string cohortId)
    {
        lock (_gate)
        {
            var cohort = _cohorts.FirstOrDefault(c => c.Id == cohortId);
            if (cohort is null)
                return EnrollmentAttempt.CourseMissing;
            var course = _courses.FirstOrDefault(c => c.Id == cohort.CourseId);
            if (course is null || course.Approval != ApprovalStatus.Approved)
                return EnrollmentAttempt.NotOpen;
            if (_enrollments.Any(e => e.PersonId == personId && e.CohortId == cohortId))
                return EnrollmentAttempt.AlreadyEnrolled;
            if (_enrollments.Any(e => e.PersonId == personId && _cohorts.First(c => c.Id == e.CohortId).CourseId == course.Id))
                return EnrollmentAttempt.AlreadyEnrolled;
            if (_enrollments.Count(e => e.CohortId == cohortId) >= cohort.Capacity)
                return EnrollmentAttempt.Full;

            var enrollment = new Enrollment
            {
                Id = $"enr-{personId}-{cohortId}",
                PersonId = personId,
                CohortId = cohortId,
                EnrolledOn = DateTime.Today
            };
            _enrollments.Add(enrollment);
            foreach (var session in cohort.Sessions)
            {
                _attendance.Add(new AttendanceRecord
                {
                    Id = AttendanceId(personId, session.Id),
                    PersonId = personId,
                    CohortId = cohortId,
                    SessionId = session.Id,
                    Status = AttendanceStatus.NotMarked
                });
            }

            return EnrollmentAttempt.Enrolled;
        }
    }

    public IReadOnlyList<EnrollmentRow> EnrollmentsFor(string personId)
    {
        lock (_gate)
        {
            return _enrollments
                .Where(e => e.PersonId == personId)
                .Select(e =>
                {
                    var cohort = _cohorts.First(c => c.Id == e.CohortId);
                    var course = _courses.First(c => c.Id == cohort.CourseId);
                    var progress = ProgressUnlocked(personId, course.Id)
                        ?? throw new InvalidOperationException($"Missing progress for {personId} in {course.Id}.");
                    var next = cohort.Sessions
                        .Where(s => s.Starts.Date >= DateTime.Today)
                        .OrderBy(s => s.Starts)
                        .FirstOrDefault()
                        ?? cohort.Sessions.OrderByDescending(s => s.Starts).First();
                    return new EnrollmentRow
                    {
                        CourseId = course.Id,
                        CourseTitle = course.Title,
                        CohortName = cohort.Name,
                        Level = AcademyLabels.Level(course.Level),
                        Delivery = AcademyLabels.Delivery(cohort.Delivery),
                        NextSession = next.Starts,
                        NextSessionTitle = next.Title,
                        ProgressPercent = progress.Percent,
                        ProgressNote = progress.PlainSummary
                    };
                })
                .OrderBy(r => r.CourseTitle)
                .ToList();
        }
    }

    public LearnerCourseProgress? Progress(string personId, string courseId)
    {
        lock (_gate) return ProgressUnlocked(personId, courseId);
    }

    public IReadOnlyList<RosterRow> AttendanceFor(string personId)
    {
        lock (_gate)
        {
            return _attendance
                .Where(a => a.PersonId == personId)
                .Select(record =>
                {
                    var cohort = _cohorts.First(c => c.Id == record.CohortId);
                    var session = cohort.Sessions.First(s => s.Id == record.SessionId);
                    var person = _people.First(p => p.Id == personId);
                    return new RosterRow
                    {
                        AttendanceId = record.Id,
                        PersonId = personId,
                        CourseId = cohort.CourseId,
                        LearnerName = person.DisplayName,
                        Agency = person.Agency,
                        JobTitle = person.JobTitle,
                        SessionTitle = $"{cohort.Name}: {session.Title}",
                        SessionDate = session.Starts,
                        Status = AcademyLabels.Attendance(record.Status)
                    };
                })
                .OrderBy(r => r.SessionDate)
                .ToList();
        }
    }

    public IReadOnlyList<RosterRow> Roster(string cohortId, string? sessionId)
    {
        lock (_gate)
        {
            var cohort = _cohorts.FirstOrDefault(c => c.Id == cohortId);
            if (cohort is null)
                return [];

            var sessions = cohort.Sessions
                .Where(s => sessionId is null || s.Id == sessionId)
                .ToList();

            var rows = new List<RosterRow>();
            foreach (var record in _attendance.Where(a => a.CohortId == cohortId))
            {
                var session = sessions.FirstOrDefault(s => s.Id == record.SessionId);
                if (session is null)
                    continue;
                var person = _people.First(p => p.Id == record.PersonId);
                rows.Add(new RosterRow
                {
                    AttendanceId = record.Id,
                    PersonId = person.Id,
                    CourseId = cohort.CourseId,
                    LearnerName = person.DisplayName,
                    Agency = person.Agency,
                    JobTitle = person.JobTitle,
                    SessionTitle = session.Title,
                    SessionDate = session.Starts,
                    Status = AcademyLabels.Attendance(record.Status)
                });
            }

            return rows.OrderBy(r => r.SessionDate).ThenBy(r => r.LearnerName).ToList();
        }
    }

    public int MarkAttendance(IEnumerable<string> attendanceIds, AttendanceStatus status, string markedBy, AcademyRole actorRole)
    {
        if (actorRole is not (AcademyRole.Instructor or AcademyRole.Administrator))
            throw new InvalidOperationException("Only an instructor or an administrator can record attendance.");

        lock (_gate)
        {
            var ids = attendanceIds.ToHashSet(StringComparer.Ordinal);
            var changed = 0;
            foreach (var record in _attendance.Where(a => ids.Contains(a.Id)))
            {
                record.Status = status;
                record.MarkedBy = markedBy;
                record.MarkedAt = DateTimeOffset.Now;
                changed++;
            }

            return changed;
        }
    }

    public void SetExerciseComplete(string personId, string exerciseId, bool completed)
    {
        lock (_gate)
        {
            var row = _exercises.FirstOrDefault(e => e.PersonId == personId && e.ExerciseId == exerciseId);
            if (row is null)
            {
                row = new ExerciseProgress
                {
                    PersonId = personId,
                    ExerciseId = exerciseId,
                    Completed = completed
                };
                _exercises.Add(row);
            }
            else
            {
                row.Completed = completed;
            }
        }
    }

    public bool IsExerciseComplete(string personId, string exerciseId)
    {
        lock (_gate)
            return _exercises.Any(e => e.PersonId == personId && e.ExerciseId == exerciseId && e.Completed);
    }

    public void SaveNotes(string personId, string courseId, string notes)
    {
        lock (_gate)
            _notes[$"{personId}|{courseId}"] = notes ?? "";
    }

    public string NotesFor(string personId, string courseId)
    {
        lock (_gate)
            return _notes.TryGetValue($"{personId}|{courseId}", out var notes) ? notes : "";
    }

    public Session? AddSession(string cohortId, string title, DateTime starts, string place, AcademyRole actorRole)
    {
        if (actorRole is not (AcademyRole.Instructor or AcademyRole.Administrator))
            throw new InvalidOperationException("Only an instructor or an administrator can add a class meeting.");

        lock (_gate)
        {
            var cohort = _cohorts.FirstOrDefault(c => c.Id == cohortId);
            if (cohort is null || string.IsNullOrWhiteSpace(title))
                return null;

            var session = new Session
            {
                Id = $"{cohortId}-s{cohort.Sessions.Count + 1}-{starts:yyyyMMdd}",
                CohortId = cohortId,
                Title = title.Trim(),
                Starts = starts.Date.AddHours(9),
                DurationMinutes = 180,
                Place = string.IsNullOrWhiteSpace(place) ? "Live online" : place.Trim(),
                RequiredForCompletion = true
            };
            cohort.Sessions.Add(session);
            foreach (var enrollment in _enrollments.Where(e => e.CohortId == cohortId))
            {
                _attendance.Add(new AttendanceRecord
                {
                    Id = AttendanceId(enrollment.PersonId, session.Id),
                    PersonId = enrollment.PersonId,
                    CohortId = cohortId,
                    SessionId = session.Id,
                    Status = AttendanceStatus.NotMarked
                });
            }

            return session;
        }
    }

    public IReadOnlyList<AttendanceChartPoint> AttendanceRates()
    {
        lock (_gate)
        {
            return _cohorts
                .Where(c => _courses.Any(course => course.Id == c.CourseId && course.Approval == ApprovalStatus.Approved))
                .Select(c =>
                {
                    var held = c.Sessions.Where(s => s.Starts.Date <= DateTime.Today).Select(s => s.Id).ToHashSet();
                    var rows = _attendance.Where(a => a.CohortId == c.Id && held.Contains(a.SessionId)).ToList();
                    var credited = rows.Count(a => a.Status is AttendanceStatus.Present or AttendanceStatus.Excused);
                    var rate = rows.Count == 0 ? 0 : Math.Round(100.0 * credited / rows.Count, 1);
                    return new AttendanceChartPoint { CohortName = c.Name, PresentRate = rate };
                })
                .OrderBy(p => p.CohortName)
                .ToList();
        }
    }

    public int ApprovedCourseCount
    {
        get { lock (_gate) return _courses.Count(c => c.Approval == ApprovalStatus.Approved); }
    }

    private LearnerCourseProgress? ProgressUnlocked(string personId, string courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course is null)
            return null;
        var enrollment = _enrollments.FirstOrDefault(e =>
            e.PersonId == personId && _cohorts.First(c => c.Id == e.CohortId).CourseId == courseId);
        if (enrollment is null)
            return null;

        var cohort = _cohorts.First(c => c.Id == enrollment.CohortId);
        var requiredSessions = cohort.Sessions.Where(s => s.RequiredForCompletion).ToList();
        var held = requiredSessions.Where(s => s.Starts.Date <= DateTime.Today).ToList();
        var records = _attendance.Where(a => a.PersonId == personId && a.CohortId == cohort.Id).ToList();
        var credited = requiredSessions.Count(s =>
            records.Any(a => a.SessionId == s.Id && a.Status is AttendanceStatus.Present or AttendanceStatus.Excused));
        var requiredExercises = course.Exercises.Where(e => e.RequiredForCompletion).ToList();
        var done = requiredExercises.Count(e =>
            _exercises.Any(p => p.PersonId == personId && p.ExerciseId == e.Id && p.Completed));
        var complete = requiredSessions.Count > 0
            && credited == requiredSessions.Count
            && done == requiredExercises.Count;
        var denom = requiredSessions.Count + requiredExercises.Count;
        var percent = denom == 0 ? 0 : (int)Math.Round(100.0 * (credited + done) / denom);
        var upcoming = requiredSessions.Count(s => s.Starts.Date > DateTime.Today);

        return new LearnerCourseProgress
        {
            CourseId = course.Id,
            CourseTitle = course.Title,
            CohortId = cohort.Id,
            CohortName = cohort.Name,
            RequiredSessions = requiredSessions.Count,
            SessionsHeld = held.Count,
            SessionsCredited = credited,
            UpcomingSessions = upcoming,
            RequiredExercises = requiredExercises.Count,
            ExercisesCompleted = done,
            Complete = complete,
            Percent = percent,
            PlainSummary = complete
                ? "Complete. Attendance and the required exercises are both recorded."
                : $"{credited} of {requiredSessions.Count} required meetings are marked present or excused. {upcoming} meeting(s) are still ahead. {done} of {requiredExercises.Count} required exercises are checked."
        };
    }

    private static string AttendanceId(string personId, string sessionId) => $"{personId}:{sessionId}";
}
