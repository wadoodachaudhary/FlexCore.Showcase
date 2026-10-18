using GovAiAcademy.Models;

namespace GovAiAcademy.Services;

/// <summary>
/// Approval gate for instructor-led courses. DOL TEN 07-25 treats AI literacy as the baseline
/// every worker should have. Approved Academy courses have to sit above that baseline:
/// live instruction, applied practice in the officer's own work, and attendance.
/// </summary>
public static class PedagogyRules
{
    public static IReadOnlyList<string> Violations(Course course)
    {
        var gaps = new List<string>();

        if (!course.InstructorLed)
            gaps.Add("An instructor must lead the class. A recording library does not qualify.");
        if (course.IsPassiveRecording)
            gaps.Add("The offering is a passive recording. Approved courses are applied class meetings.");
        if (course.Delivery == DeliveryMode.SelfPacedRecording)
            gaps.Add("Delivery must be online live, in person, or hybrid.");
        if (!course.AppliedPractice || !course.Exercises.Any(e => e.RequiredForCompletion && e.Brief.Trim().Length >= 40))
            gaps.Add("Approved courses need required hands-on exercises with a written brief.");
        if (!course.ContextualizedToRole || string.IsNullOrWhiteSpace(course.Scenario) || string.IsNullOrWhiteSpace(course.ToolsContext))
            gaps.Add("Practice must be tied to the officer's role, tools, and operations.");
        if (string.IsNullOrWhiteSpace(course.EmployerCustomization))
            gaps.Add("An agency training lead must be able to swap in local tools, templates, and policy.");
        if (!course.AboveBaselineLiteracy || string.IsNullOrWhiteSpace(course.AboveBaselineNote))
            gaps.Add("The course must be pitched above baseline AI literacy, toward role proficiency.");
        if (!course.StructuredParticipation || course.Modules.Count < 2)
            gaps.Add("Learners need structured participation across more than one module.");
        if (!course.AttendanceVerified || course.AttendancePolicy.Contains("attendance", StringComparison.OrdinalIgnoreCase) == false)
            gaps.Add("Completion must be verified by instructor-recorded attendance.");
        if (course.ContactHours < 6)
            gaps.Add("Approved courses are class meetings of at least 6 contact hours, not a short video.");
        if (course.Outcomes.Count < 3)
            gaps.Add("State at least three observable outcomes.");
        if (course.Prerequisites.Count == 0)
            gaps.Add("Name the digital-skill or workplace prerequisites so people can start ready.");
        if (!course.DolContentAreas.Contains(DolFramework.EvaluateOutputs) || !course.DolContentAreas.Contains(DolFramework.UseResponsibly))
            gaps.Add("Map the course to DOL content areas for evaluating outputs and using AI responsibly.");
        if (!course.DolDeliveryPrinciples.Contains(DolFramework.Experiential) || !course.DolDeliveryPrinciples.Contains(DolFramework.InContext))
            gaps.Add("Map the course to experiential learning embedded in the worker's context.");

        return gaps;
    }

    public static bool IsEligibleForApproval(Course course) => Violations(course).Count == 0;

    public static void EnsureCatalogIsConsistent(IEnumerable<Course> courses)
    {
        foreach (var course in courses)
        {
            var gaps = Violations(course);
            if (course.Approval == ApprovalStatus.Approved && gaps.Count > 0)
            {
                throw new InvalidOperationException(
                    $"Course '{course.Id}' is marked approved but fails pedagogy rules: {string.Join(" ", gaps)}");
            }

            if (course.Approval == ApprovalStatus.InReview && gaps.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Course '{course.Id}' is in review but already meets every approval rule.");
            }
        }
    }
}
