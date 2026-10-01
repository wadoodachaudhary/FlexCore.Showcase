// GovAI Academy catalog seed for GA-PE-210.
// Unclassified instructor-led course. Drop-in snippet for the Blazor catalog.
//
// If the academy project already defines CatalogCourse, CourseLevel, CourseExercise,
// and AttendancePolicy, delete the contract types at the bottom of this file and
// keep PromptEngineeringCourseSeed. Field names match the catalog pattern:
// title, levels, exercises, attendance.

using System;
using System.Collections.Generic;
using GovAiAcademy.Catalog;

namespace GovAiAcademy.Catalog.Seed
{

public static class PromptEngineeringCourseSeed
{
    public const string CourseCode = "GA-PE-210";

    public static CatalogCourse Create() => new()
    {
        Code = CourseCode,
        Title = "Prompt Engineering for Government Officers",
        Summary =
            "Instructor-led applied practice in directing generative AI for unclassified government office work. " +
            "Above the baseline in the U.S. Department of Labor AI Literacy Framework (TEN 07-25, February 13, 2026). " +
            "Not a Department of Labor publication. No classified or controlled data in the exercises.",
        Delivery = DeliveryMode.InstructorLed,
        Approval = ApprovalState.Approved,
        PrerequisiteLevel = ProficiencyLevel.BaselineLiteracy,
        DurationMinutes = 130,
        BriefingMinutes = 30,
        RequiresAttendance = true,
        Attendance = new AttendancePolicy
        {
            Mode = AttendanceMode.InPersonOrVirtualSection,
            Checkpoints = new[] { "Opening", "Start of Lab 1", "Dismissal" },
            VideoOnlyCompletes = false,
            RecordRoster = true,
            Notes = "Completion requires presence at each checkpoint and all three worksheets."
        },
        Levels = new List<CourseLevel>
        {
            new()
            {
                Level = ProficiencyLevel.Applied,
                IsPrimary = true,
                Audience = "Non-technical to semi-technical government officers",
                Outcomes = new[]
                {
                    "Build a prompt with role, context, task, constraints, and output.",
                    "Bind the model to a supplied source and label gaps as not in source.",
                    "Revise in passes and reject an unfit or unsafe draft.",
                    "Distinguish a draft from an official work product."
                }
            }
        },
        Exercises = new List<CourseExercise>
        {
            new()
            {
                Code = "GA-PE-210-L1",
                Title = "Rebuild a thin prompt",
                Minutes = 20,
                Required = true,
                Kind = ExerciseKind.Worksheet,
                PromptBoundary = "Fictional Harbor Town park flyer only. No live cases.",
                PassStandard = "All five prompt parts present; one job; source binding; no outside facts."
            },
            new()
            {
                Code = "GA-PE-210-L2",
                Title = "Two passes on a public reply",
                Minutes = 25,
                Required = true,
                Kind = ExerciseKind.Worksheet,
                PromptBoundary = "Same fictional fact sheet. Approved training tool or paper exchange.",
                PassStandard = "Pass 2 and pass 3 written; invented rain policy would be rejected."
            },
            new()
            {
                Code = "GA-PE-210-L3",
                Title = "Strip an oversharing prompt",
                Minutes = 25,
                Required = true,
                Kind = ExerciseKind.Worksheet,
                PromptBoundary = "Fictional overshare is given only so it can be removed. Do not add real personal data.",
                PassStandard = "Original thread marked Reject; rewritten prompt contains none of the prohibited details."
            }
        },
        Security = new SecurityProfile
        {
            ClassifiedDataAllowed = false,
            ControlledUnclassifiedAllowed = false,
            PersonalDataAllowed = false,
            ApprovedToolRequiredForLiveGeneration = true,
            PaperLabsAllowedWhenNoToolApproved = true,
            RecordsNote = "A chat is not the official record. Adopted language is copied into the office system of record."
        },
        Materials = new[]
        {
            "slides/Prompt-Engineering-for-Government-Officers.pptx",
            "video/GA-PE-210-prompt-engineering.mp4",
            "narration/narration-script.md",
            "outline/course-outline.md"
        }
    };
}

}

// --- Catalog contract (remove if the Blazor project already has these types) ---

namespace GovAiAcademy.Catalog
{

public enum DeliveryMode { SelfPaced, InstructorLed, Cohort }

public enum ApprovalState { Draft, InReview, Approved, Retired }

public enum ProficiencyLevel { BaselineLiteracy, Applied, Specialist }

public enum AttendanceMode { InPersonOrVirtualSection, SelfReport, None }

public enum ExerciseKind { Worksheet, Lab, Discussion, Quiz }

public sealed class AttendancePolicy
{
    public AttendanceMode Mode { get; set; }
    public string[] Checkpoints { get; set; } = Array.Empty<string>();
    public bool VideoOnlyCompletes { get; set; }
    public bool RecordRoster { get; set; }
    public string Notes { get; set; } = "";
}

public sealed class CourseLevel
{
    public ProficiencyLevel Level { get; set; }
    public bool IsPrimary { get; set; }
    public string Audience { get; set; } = "";
    public string[] Outcomes { get; set; } = Array.Empty<string>();
}

public sealed class CourseExercise
{
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public int Minutes { get; set; }
    public bool Required { get; set; }
    public ExerciseKind Kind { get; set; }
    public string PromptBoundary { get; set; } = "";
    public string PassStandard { get; set; } = "";
}

public sealed class SecurityProfile
{
    public bool ClassifiedDataAllowed { get; set; }
    public bool ControlledUnclassifiedAllowed { get; set; }
    public bool PersonalDataAllowed { get; set; }
    public bool ApprovedToolRequiredForLiveGeneration { get; set; }
    public bool PaperLabsAllowedWhenNoToolApproved { get; set; }
    public string RecordsNote { get; set; } = "";
}

public sealed class CatalogCourse
{
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public DeliveryMode Delivery { get; set; }
    public ApprovalState Approval { get; set; }
    public ProficiencyLevel PrerequisiteLevel { get; set; }
    public int DurationMinutes { get; set; }
    public int BriefingMinutes { get; set; }
    public bool RequiresAttendance { get; set; }
    public AttendancePolicy Attendance { get; set; } = new();
    public List<CourseLevel> Levels { get; set; } = new();
    public List<CourseExercise> Exercises { get; set; } = new();
    public SecurityProfile Security { get; set; } = new();
    public string[] Materials { get; set; } = Array.Empty<string>();
}
}
