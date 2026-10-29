using GovAiAcademy.Models;
using GovAiAcademy.Services;
using Xunit;

namespace GovAiAcademy.Tests;

public class CatalogRulesTests
{
    private readonly AcademyStore _store = new();

    [Fact]
    public void Approved_courses_pass_the_pedagogy_gate()
    {
        var approved = _store.Courses.Where(c => c.Approval == ApprovalStatus.Approved).ToList();
        Assert.NotEmpty(approved);
        Assert.All(approved, course => Assert.Empty(PedagogyRules.Violations(course)));
    }

    [Fact]
    public void In_review_course_is_a_passive_recording_and_fails_the_gate()
    {
        var pending = Assert.Single(_store.Courses, c => c.Approval == ApprovalStatus.InReview);
        Assert.True(pending.IsPassiveRecording);
        Assert.False(pending.InstructorLed);
        Assert.NotEmpty(PedagogyRules.Violations(pending));
        Assert.Equal(EnrollmentAttempt.CourseMissing, _store.Enroll("p-jordan", pending.Id));
    }

    [Fact]
    public void Approved_catalog_covers_levels_roles_and_live_delivery()
    {
        var approved = _store.Courses.Where(c => c.Approval == ApprovalStatus.Approved).ToList();
        Assert.Contains(approved, c => c.Level == CourseLevel.Foundation);
        Assert.Contains(approved, c => c.Level == CourseLevel.Practitioner);
        Assert.Contains(approved, c => c.Level == CourseLevel.AdvancedTechnical);
        foreach (var role in Enum.GetValues<RolePath>())
            Assert.Contains(approved, c => c.Role == role);
        Assert.Contains(approved, c => c.Delivery == DeliveryMode.OnlineLive);
        Assert.Contains(approved, c => c.Delivery == DeliveryMode.InPerson);
        Assert.Contains(approved, c => c.Delivery == DeliveryMode.Hybrid);
        Assert.DoesNotContain(approved, c => c.Delivery == DeliveryMode.SelfPacedRecording);
    }

    [Fact]
    public void Every_approved_course_has_a_class_group_with_attendance_rows()
    {
        foreach (var course in _store.Courses.Where(c => c.Approval == ApprovalStatus.Approved))
        {
            var cohort = Assert.Single(_store.CohortsForCourse(course.Id));
            Assert.True(_store.EnrollmentCount(cohort.Id) >= 2);
            var roster = _store.Roster(cohort.Id, null);
            Assert.NotEmpty(roster);
            Assert.Contains(roster, row => row.Status == AcademyLabels.Attendance(AttendanceStatus.Present));
        }
    }

    [Fact]
    public void Sample_accounts_use_example_com_and_original_titles()
    {
        Assert.All(_store.People, person => Assert.EndsWith("@example.com", person.Email, StringComparison.Ordinal));
        var blob = string.Join("\n", _store.Courses.Select(c => c.Title + c.Summary + c.LabPacket));
        Assert.DoesNotContain("Analytics Vidhya", blob, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Educative", blob, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Instructor_can_mark_attendance_and_completion_follows_the_rule()
    {
        var progress = _store.Progress("p-jordan", "briefing-without-the-model");
        Assert.NotNull(progress);
        Assert.False(progress!.Complete);
        Assert.Equal(2, progress.SessionsCredited);

        var open = _store.Roster("coh-brief", "coh-brief-s3").Single(r => r.PersonId == "p-jordan");
        var marked = _store.MarkAttendance([open.AttendanceId], AttendanceStatus.Present, "p-amira", AcademyRole.Instructor);
        Assert.Equal(1, marked);

        _store.SetExerciseComplete("p-jordan", "briefing-without-the-model-ex2", true);
        _store.SetExerciseComplete("p-jordan", "briefing-without-the-model-ex3", true);

        progress = _store.Progress("p-jordan", "briefing-without-the-model");
        Assert.NotNull(progress);
        Assert.True(progress!.Complete);
        Assert.Equal(100, progress.Percent);
    }

    [Fact]
    public void Learner_cannot_mark_attendance()
    {
        var row = _store.Roster("coh-brief", "coh-brief-s3").First();
        Assert.Throws<InvalidOperationException>(() =>
            _store.MarkAttendance([row.AttendanceId], AttendanceStatus.Present, "p-jordan", AcademyRole.Learner));
    }

    [Fact]
    public void Adding_a_meeting_adds_unmarked_rows_for_the_roster()
    {
        var before = _store.EnrollmentCount("coh-status");
        var session = _store.AddSession("coh-status", "Office-hours clinic", new DateTime(2026, 10, 22), "Live online", AcademyRole.Administrator);
        Assert.NotNull(session);
        var rows = _store.Roster("coh-status", session!.Id);
        Assert.Equal(before, rows.Count);
        Assert.All(rows, row => Assert.Equal("Not marked", row.Status));
    }
}
