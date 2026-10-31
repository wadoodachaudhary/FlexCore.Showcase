namespace GovAiAcademy.Models;

public enum CourseLevel
{
    Foundation,
    Practitioner,
    AdvancedTechnical
}

public enum RolePath
{
    Policy,
    Acquisition,
    Cybersecurity,
    DataAnalytics,
    ProgramManagement
}

public enum DeliveryMode
{
    OnlineLive,
    InPerson,
    Hybrid,
    SelfPacedRecording
}

public enum ApprovalStatus
{
    Approved,
    InReview
}

public enum AttendanceStatus
{
    NotMarked,
    Present,
    Excused,
    Absent
}

public enum AcademyRole
{
    Learner,
    Instructor,
    Administrator
}

/// <summary>
/// Names from U.S. DOL TEN 07-25. The Academy cites these areas; it does not reprint the notice.
/// </summary>
public static class DolFramework
{
    public const string UnderstandPrinciples = "Understand AI Principles";
    public const string ExploreUses = "Explore AI Uses";
    public const string DirectEffectively = "Direct AI Effectively";
    public const string EvaluateOutputs = "Evaluate AI Outputs";
    public const string UseResponsibly = "Use AI Responsibly";

    public const string Experiential = "Enable Experiential Learning";
    public const string InContext = "Embed Learning in Context";
    public const string HumanSkills = "Build Complementary Human Skills";
    public const string Prerequisites = "Address Prerequisites to AI Literacy";
    public const string Pathways = "Create Pathways for Continued Learning";
    public const string EnablingRoles = "Prepare Enabling Roles";
    public const string Agility = "Design for Agility";

    public static readonly string[] ContentAreas =
    [
        UnderstandPrinciples,
        ExploreUses,
        DirectEffectively,
        EvaluateOutputs,
        UseResponsibly
    ];

    public static readonly string[] DeliveryPrinciples =
    [
        Experiential,
        InContext,
        HumanSkills,
        Prerequisites,
        Pathways,
        EnablingRoles,
        Agility
    ];
}

public static class AcademyLabels
{
    public static string Level(CourseLevel level) => level switch
    {
        CourseLevel.Foundation => "Foundation (non-technical)",
        CourseLevel.Practitioner => "Practitioner",
        CourseLevel.AdvancedTechnical => "Advanced / technical",
        _ => level.ToString()
    };

    public static string Role(RolePath role) => role switch
    {
        RolePath.Policy => "Policy",
        RolePath.Acquisition => "Acquisition",
        RolePath.Cybersecurity => "Cybersecurity",
        RolePath.DataAnalytics => "Data and analytics",
        RolePath.ProgramManagement => "Program management",
        _ => role.ToString()
    };

    public static string Delivery(DeliveryMode mode) => mode switch
    {
        DeliveryMode.OnlineLive => "Online live",
        DeliveryMode.InPerson => "In person",
        DeliveryMode.Hybrid => "Hybrid",
        DeliveryMode.SelfPacedRecording => "Self-paced recording",
        _ => mode.ToString()
    };

    public static string Approval(ApprovalStatus status) => status switch
    {
        ApprovalStatus.Approved => "Approved",
        ApprovalStatus.InReview => "In review",
        _ => status.ToString()
    };

    public static string Attendance(AttendanceStatus status) => status switch
    {
        AttendanceStatus.NotMarked => "Not marked",
        AttendanceStatus.Present => "Present",
        AttendanceStatus.Excused => "Excused",
        AttendanceStatus.Absent => "Absent",
        _ => status.ToString()
    };

    public static string Persona(AcademyRole role) => role switch
    {
        AcademyRole.Learner => "Learner",
        AcademyRole.Instructor => "Instructor",
        AcademyRole.Administrator => "Administrator",
        _ => role.ToString()
    };
}

public sealed class Person
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string JobTitle { get; init; }
    public required string Agency { get; init; }
    public required AcademyRole Role { get; init; }
    public required string Email { get; init; }
    public bool DevPersona { get; init; }
}

public sealed class Module
{
    public required string Title { get; init; }
    public required string Purpose { get; init; }
    public required string Practice { get; init; }
}

public sealed class Exercise
{
    public required string Id { get; init; }
    public required string CourseId { get; init; }
    public required string Title { get; init; }
    public required string Brief { get; init; }
    public bool RequiredForCompletion { get; init; } = true;
}

public sealed class Course
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required CourseLevel Level { get; init; }
    public required RolePath Role { get; init; }
    public required DeliveryMode Delivery { get; init; }
    public required ApprovalStatus Approval { get; init; }
    public required bool InstructorLed { get; init; }
    public required bool AppliedPractice { get; init; }
    public required bool ContextualizedToRole { get; init; }
    public required bool AboveBaselineLiteracy { get; init; }
    public required bool StructuredParticipation { get; init; }
    public required bool AttendanceVerified { get; init; }
    public required bool IsPassiveRecording { get; init; }
    public required int ContactHours { get; init; }
    public required string Audience { get; init; }
    public required string Scenario { get; init; }
    public required string ToolsContext { get; init; }
    public required string EmployerCustomization { get; init; }
    public required string AboveBaselineNote { get; init; }
    public required string AttendancePolicy { get; init; }
    public required string LabPacket { get; init; }
    public required IReadOnlyList<string> Outcomes { get; init; }
    public required IReadOnlyList<string> Prerequisites { get; init; }
    public required IReadOnlyList<Module> Modules { get; init; }
    public required IReadOnlyList<Exercise> Exercises { get; init; }
    public required IReadOnlyList<string> DolContentAreas { get; init; }
    public required IReadOnlyList<string> DolDeliveryPrinciples { get; init; }
}

public sealed class Session
{
    public required string Id { get; init; }
    public required string CohortId { get; init; }
    public required string Title { get; init; }
    public required DateTime Starts { get; init; }
    public required int DurationMinutes { get; init; }
    public required string Place { get; init; }
    public bool RequiredForCompletion { get; init; } = true;
}

public sealed class Cohort
{
    public required string Id { get; init; }
    public required string CourseId { get; init; }
    public required string Name { get; init; }
    public required string InstructorId { get; init; }
    public required DeliveryMode Delivery { get; init; }
    public required int Capacity { get; init; }
    public required string ScheduleNote { get; init; }
    public List<Session> Sessions { get; init; } = [];
}

public sealed class Enrollment
{
    public required string Id { get; init; }
    public required string PersonId { get; init; }
    public required string CohortId { get; init; }
    public required DateTime EnrolledOn { get; init; }
}

public sealed class AttendanceRecord
{
    public required string Id { get; init; }
    public required string PersonId { get; init; }
    public required string CohortId { get; init; }
    public required string SessionId { get; init; }
    public AttendanceStatus Status { get; set; }
    public string? MarkedBy { get; set; }
    public DateTimeOffset? MarkedAt { get; set; }
}

public sealed class ExerciseProgress
{
    public required string PersonId { get; init; }
    public required string ExerciseId { get; init; }
    public bool Completed { get; set; }
    public string Notes { get; set; } = "";
}

public sealed class CatalogFilter
{
    public CourseLevel? Level { get; set; }
    public RolePath? Role { get; set; }
    public DeliveryMode? Delivery { get; set; }
    public ApprovalStatus? Approval { get; set; }
    public string Text { get; set; } = "";
}

public sealed class NamedOption
{
    public required string Id { get; init; }
    public required string Label { get; init; }
}

public sealed class RosterRow
{
    public required string AttendanceId { get; init; }
    public required string PersonId { get; init; }
    public required string CourseId { get; init; }
    public required string LearnerName { get; init; }
    public required string Agency { get; init; }
    public required string JobTitle { get; init; }
    public required string SessionTitle { get; init; }
    public DateTime SessionDate { get; init; }
    public required string Status { get; init; }
}

public sealed class EnrollmentRow
{
    public required string CourseId { get; init; }
    public required string CourseTitle { get; init; }
    public required string CohortName { get; init; }
    public required string Level { get; init; }
    public required string Delivery { get; init; }
    public DateTime NextSession { get; init; }
    public required string NextSessionTitle { get; init; }
    public int ProgressPercent { get; init; }
    public required string ProgressNote { get; init; }
}

public sealed class LearnerCourseProgress
{
    public required string CourseId { get; init; }
    public required string CourseTitle { get; init; }
    public required string CohortId { get; init; }
    public required string CohortName { get; init; }
    public int RequiredSessions { get; init; }
    public int SessionsHeld { get; init; }
    public int SessionsCredited { get; init; }
    public int UpcomingSessions { get; init; }
    public int RequiredExercises { get; init; }
    public int ExercisesCompleted { get; init; }
    public bool Complete { get; init; }
    public int Percent { get; init; }
    public required string PlainSummary { get; init; }
}

public sealed class AttendanceChartPoint
{
    public required string CohortName { get; init; }
    public double PresentRate { get; init; }
}

public enum EnrollmentAttempt
{
    Enrolled,
    AlreadyEnrolled,
    Full,
    CourseMissing,
    NotOpen
}
