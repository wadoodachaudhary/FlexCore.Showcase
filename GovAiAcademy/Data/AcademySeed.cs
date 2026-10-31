using GovAiAcademy.Models;

namespace GovAiAcademy.Data;

public sealed class AcademySeedBundle
{
    public required List<Person> People { get; init; }
    public required List<Course> Courses { get; init; }
    public required List<Cohort> Cohorts { get; init; }
    public required List<Enrollment> Enrollments { get; init; }
    public required List<AttendanceRecord> Attendance { get; init; }
    public required List<ExerciseProgress> ExerciseProgress { get; init; }
}

/// <summary>
/// Original sample curriculum. Titles, scenarios, and lab briefs are written for this app.
/// They are not taken from any vendor catalog.
/// </summary>
public static class AcademySeed
{
    public static AcademySeedBundle Create()
    {
        var people = People();
        var courses = Courses();
        var cohorts = Cohorts();
        var enrollments = new List<Enrollment>();
        var attendance = new List<AttendanceRecord>();
        var exercises = new List<ExerciseProgress>();

        Seat(enrollments, attendance, "p-jordan", "coh-brief", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-mei", "coh-brief", ["Present", "Excused", "NotMarked"]);
        Seat(enrollments, attendance, "p-luis", "coh-brief", ["Present", "Absent", "NotMarked"]);
        Seat(enrollments, attendance, "p-helen", "coh-brief", ["Present", "Present", "NotMarked"]);

        Seat(enrollments, attendance, "p-andre", "coh-acq", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-sofia", "coh-acq", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-renee", "coh-acq", ["Excused", "Present", "NotMarked"]);

        Seat(enrollments, attendance, "p-omar", "coh-cyber", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-luis", "coh-cyber", ["Present", "Absent", "NotMarked"]);
        Seat(enrollments, attendance, "p-andre", "coh-cyber", ["Present", "Present", "NotMarked"]);

        Seat(enrollments, attendance, "p-jordan", "coh-status", ["Present", "NotMarked", "NotMarked"]);
        Seat(enrollments, attendance, "p-helen", "coh-status", ["Present", "NotMarked", "NotMarked"]);
        Seat(enrollments, attendance, "p-mei", "coh-status", ["Absent", "NotMarked", "NotMarked"]);

        Seat(enrollments, attendance, "p-sofia", "coh-table", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-renee", "coh-table", ["Present", "Excused", "NotMarked"]);
        Seat(enrollments, attendance, "p-omar", "coh-table", ["Present", "Present", "NotMarked"]);

        Seat(enrollments, attendance, "p-mei", "coh-oversight", ["Present", "NotMarked", "NotMarked"]);
        Seat(enrollments, attendance, "p-helen", "coh-oversight", ["Present", "NotMarked", "NotMarked"]);

        Seat(enrollments, attendance, "p-omar", "coh-pilot", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-sofia", "coh-pilot", ["Present", "Absent", "NotMarked"]);

        Seat(enrollments, attendance, "p-renee", "coh-coach", ["Present", "Present", "NotMarked"]);
        Seat(enrollments, attendance, "p-andre", "coh-coach", ["Present", "Present", "NotMarked"]);

        exercises.Add(new ExerciseProgress
        {
            PersonId = "p-jordan",
            ExerciseId = "briefing-without-the-model-ex1",
            Completed = true,
            Notes = "Compared the model outline with the division's two-page memo template."
        });

        return new AcademySeedBundle
        {
            People = people,
            Courses = courses,
            Cohorts = cohorts,
            Enrollments = enrollments,
            Attendance = attendance,
            ExerciseProgress = exercises
        };
    }

    private static void Seat(
        List<Enrollment> enrollments,
        List<AttendanceRecord> attendance,
        string personId,
        string cohortId,
        IReadOnlyList<string> statuses)
    {
        enrollments.Add(new Enrollment
        {
            Id = $"enr-{personId}-{cohortId}",
            PersonId = personId,
            CohortId = cohortId,
            EnrolledOn = new DateTime(2026, 9, 2)
        });

        for (var i = 0; i < statuses.Count; i++)
        {
            var status = Enum.Parse<AttendanceStatus>(statuses[i]);
            attendance.Add(new AttendanceRecord
            {
                Id = $"{personId}:{cohortId}-s{i + 1}",
                PersonId = personId,
                CohortId = cohortId,
                SessionId = $"{cohortId}-s{i + 1}",
                Status = status,
                MarkedBy = status == AttendanceStatus.NotMarked ? null : "p-amira",
                MarkedAt = status == AttendanceStatus.NotMarked ? null : new DateTimeOffset(2026, 9, 24, 16, 0, 0, TimeSpan.Zero)
            });
        }
    }

    private static List<Person> People() =>
    [
        Person("p-jordan", "Jordan Hale", "Policy analyst", "Sample State Workforce Office", AcademyRole.Learner, true),
        Person("p-amira", "Amira Solano", "Learning lead", "Sample State Workforce Office", AcademyRole.Instructor, true),
        Person("p-casey", "Casey Okonkwo", "Academy administrator", "Sample State Workforce Office", AcademyRole.Administrator, true),
        Person("p-luis", "Luis Ortega", "Contract specialist", "Sample City Procurement Office", AcademyRole.Learner, false),
        Person("p-mei", "Mei Chen", "Program analyst", "Sample Benefits Program Office", AcademyRole.Learner, false),
        Person("p-andre", "Andre Brooks", "Supervisory analyst", "Sample City Procurement Office", AcademyRole.Learner, false),
        Person("p-sofia", "Sofia Rahman", "Performance analyst", "Sample Oversight and Evaluation Unit", AcademyRole.Learner, false),
        Person("p-renee", "Renee Patel", "Team supervisor", "Sample Benefits Program Office", AcademyRole.Learner, false),
        Person("p-omar", "Omar Diallo", "Security operations analyst", "Sample County Security Operations", AcademyRole.Learner, false),
        Person("p-helen", "Helen Cho", "Oversight officer", "Sample Oversight and Evaluation Unit", AcademyRole.Learner, false)
    ];

    private static Person Person(string id, string name, string title, string agency, AcademyRole role, bool persona) =>
        new()
        {
            Id = id,
            DisplayName = name,
            JobTitle = title,
            Agency = agency,
            Role = role,
            Email = $"{id}@example.com",
            DevPersona = persona
        };

    private static List<Cohort> Cohorts() =>
    [
        Cohort("coh-brief", "briefing-without-the-model", "Briefing cohort A", DeliveryMode.OnlineLive,
            "Three Thursday mornings, live online. Bring a redacted issue note from your own desk."),
        Cohort("coh-acq", "market-research-notes", "Market research cohort B", DeliveryMode.Hybrid,
            "Two live online labs and one in-person file review. Use only public sources in the lab."),
        Cohort("coh-cyber", "duty-officer-reading", "Duty officer cohort C", DeliveryMode.InPerson,
            "In the training room. Source logs stay on the agency network."),
        Cohort("coh-status", "status-reports-leaders-trust", "Status note cohort D", DeliveryMode.OnlineLive,
            "Three live online meetings. Bring the milestone sheet you already send upward."),
        Cohort("coh-table", "summary-against-the-table", "Source table cohort E", DeliveryMode.Hybrid,
            "Hybrid. The practice file is a public performance table, not a production extract."),
        Cohort("coh-oversight", "oversight-questions", "Oversight cohort F", DeliveryMode.InPerson,
            "In person. The vendor write-up used in class is fictional."),
        Cohort("coh-pilot", "evaluating-a-pilot", "Pilot evaluation cohort G", DeliveryMode.OnlineLive,
            "Live online. Learners design the review sample. They do not connect a production model."),
        Cohort("coh-coach", "coaching-the-team", "Supervisor cohort H", DeliveryMode.Hybrid,
            "Hybrid huddles for people who supervise staff using an approved assistant.")
    ];

    private static Cohort Cohort(string id, string courseId, string name, DeliveryMode delivery, string note)
    {
        var place = delivery switch
        {
            DeliveryMode.InPerson => "Training room, sample agency",
            DeliveryMode.Hybrid => "Live online, then training room",
            _ => "Live online"
        };
        var starts = new[]
        {
            new DateTime(2026, 9, 10, 9, 0, 0),
            new DateTime(2026, 9, 24, 9, 0, 0),
            new DateTime(2026, 10, 8, 9, 0, 0)
        };
        var titles = new[] { "Opening lab", "Applied practice", "Attendance and close-out" };
        return new Cohort
        {
            Id = id,
            CourseId = courseId,
            Name = name,
            InstructorId = "p-amira",
            Delivery = delivery,
            Capacity = 16,
            ScheduleNote = note,
            Sessions = starts.Select((when, index) => new Session
            {
                Id = $"{id}-s{index + 1}",
                CohortId = id,
                Title = titles[index],
                Starts = when,
                DurationMinutes = 180,
                Place = place,
                RequiredForCompletion = true
            }).ToList()
        };
    }

    private static List<Course> Courses() =>
    [
        Course(
            id: "briefing-without-the-model",
            title: "Briefing a Decision Without Handing Over the Pen",
            summary: "Policy staff practice a live class that turns a redacted issue note into a two-page options brief. The model may draft. The officer still owns every sentence that goes to a director.",
            level: CourseLevel.Foundation,
            role: RolePath.Policy,
            delivery: DeliveryMode.OnlineLive,
            hours: 9,
            audience: "Non-technical policy analysts, legislative aides, and writers who already produce decision memos.",
            scenario: "A division director needs a two-page options brief on a proposed workforce rule. The analyst may use the office's approved assistant. Non-public deliberations stay out of any tool the security office has not approved.",
            tools: "The agency-approved assistant, the division's memo template, and a redacted issue note from the learner's own docket.",
            customize: "In the first meeting the instructor replaces the sample memo headings with the host agency's template, citation style, and list of approved tools.",
            above: "Baseline literacy stops at 'this tool can draft text.' This class asks the officer to judge whether a draft is fit for a director, which claims lack a source, and which lines must stay in the officer's voice.",
            policy: "Completion requires instructor-recorded attendance at every live meeting, or an excused absence the instructor approves, plus the required exercise checklist. Watching a later recording does not replace attendance.",
            packet: "Sample packet: a one-page public issue note about extending office hours at a workforce center. Task: produce a two-page options brief with two options, one tradeoff, and a line that says what the model changed. Do not invent public comments.",
            outcomes:
            [
                "Produce an options brief on the office template that a supervisor could edit.",
                "Mark each claim that still needs a source before the brief leaves the desk.",
                "State, in writing, what the officer accepted, changed, or rejected from the draft."
            ],
            prerequisites:
            [
                "Comfort with a web browser and the office word processor.",
                "Access to the agency's approved assistant, or the classroom stand-in the instructor provides.",
                "A redacted writing sample from the learner's own work."
            ],
            modules:
            [
                ("Bring your own memo", "See where a model helps and where it invents.", "Learners paste only the redacted public facts the instructor cleared, then compare the draft with last quarter's real brief."),
                ("Sources and voice", "Keep the officer's judgment in the text.", "Side-by-side markup: highlight unsupported sentences and rewrite them in plain language."),
                ("Director read-back", "Practice the handoff.", "A partner plays the director and asks three questions the brief must already answer.")
            ],
            exercises:
            [
                ("ex1", "Outline against the template", "Ask the approved assistant for an outline. Check it against the division template. List headings you kept and headings you refused, and why."),
                ("ex2", "Claim check", "Take ten sentences from the draft. Mark each one as sourced, needs a source, or should be cut. Do this on paper or in the lab editor."),
                ("ex3", "Director version", "Submit the two-page brief in your own words, plus a short note of what you changed after the model draft.")
            ],
            areas: [DolFramework.DirectEffectively, DolFramework.EvaluateOutputs, DolFramework.UseResponsibly, DolFramework.ExploreUses],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.Prerequisites, DolFramework.Pathways]),

        Course(
            id: "market-research-notes",
            title: "Market Research Notes a Contracting Team Can Defend",
            summary: "Contract specialists practice writing a market-research summary from public sources. A human reviewer checks every citation before anything is treated as part of the contract file.",
            level: CourseLevel.Practitioner,
            role: RolePath.Acquisition,
            delivery: DeliveryMode.Hybrid,
            hours: 12,
            audience: "Contract specialists and contracting officer's representatives who draft market research. This class is not legal advice and does not interpret the Federal Acquisition Regulation.",
            scenario: "A team must summarize the public market for a facilities help-desk service. The specialist may use an approved assistant to cluster public vendor pages. Prices, past performance, and source-selection material stay out of the tool.",
            tools: "The office's approved assistant, a public-source log, and the host agency's market-research template.",
            customize: "The host contracting office swaps in its template, its list of blocked data types, and one closed (finished) public synopsis the team already published.",
            above: "Officers already know a chatbot can summarize a web page. This class trains the file discipline: what may be asked, what must stay out, and how a reviewer signs the citation check.",
            policy: "The instructor records attendance at the live online labs and the in-person file review. An unexcused absence blocks completion even if the exercises are done. Attendance is the completion record.",
            packet: "Sample packet: three fictional public vendor blurbs for a help-desk service, plus one planted claim that no blurb supports. Task: draft a one-page summary and a citation log. Flag the planted claim.",
            outcomes:
            [
                "Draft a market-research summary that separates public facts from staff judgment.",
                "Keep a citation log a teammate can re-check without opening the assistant.",
                "Refuse prompts that would place sensitive file material into an unapproved tool."
            ],
            prerequisites:
            [
                "Experience reading a solicitation or a market-research template.",
                "Ability to open the agency browser and save a document.",
                "A supervisor's ok to bring a redacted, already-public synopsis."
            ],
            modules:
            [
                ("What stays out of the prompt", "Name the data the file must not send to a tool.", "Learners sort sample sentences into 'public' and 'do not paste' before any drafting starts."),
                ("Public-source summary", "Cluster sources without inventing vendors.", "Guided practice on the sample blurbs. Every vendor name must trace to a blurb."),
                ("Reviewer pass", "A second person signs the citation check.", "Pairs swap summaries. The reviewer must find the planted unsupported claim.")
            ],
            exercises:
            [
                ("ex1", "Blocked-data list", "Write the five kinds of acquisition information you will not place in the assistant for this purchase, using your office's examples."),
                ("ex2", "Citation log", "Produce a summary of the sample vendors and a log that quotes the supporting line from each blurb."),
                ("ex3", "Reviewer note", "Review a partner's summary. Record the unsupported claim and the sentence you would send back.")
            ],
            areas: [DolFramework.ExploreUses, DolFramework.DirectEffectively, DolFramework.EvaluateOutputs, DolFramework.UseResponsibly],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.Agility]),

        Course(
            id: "duty-officer-reading",
            title: "Reading Model Output the Way a Duty Officer Reads a Ticket",
            summary: "Security analysts practice using an approved assistant to group alert narratives, then confirm each group against source logs. The class does not teach attack techniques.",
            level: CourseLevel.AdvancedTechnical,
            role: RolePath.Cybersecurity,
            delivery: DeliveryMode.InPerson,
            hours: 16,
            audience: "SOC analysts, incident coordinators, and security engineers who already triage tickets.",
            scenario: "A morning queue has twenty fictional alert narratives. The duty officer may ask an on-network assistant to group similar stories. Closure still requires the log line that supports the group.",
            tools: "A standalone lab tenant with fictional alerts, the agency's approved on-network assistant, and the ticket template the host SOC already uses.",
            customize: "The host security office replaces the sample ticket fields with its own, and restates which telemetry may be pasted into which tool.",
            above: "The class assumes analysts can already describe what a generative tool is. The work is confirmation discipline, logging of prompts that touched operational data, and knowing when to stop and escalate.",
            policy: "In-person attendance is taken at each lab block. Completion requires attendance (or an excused absence) and the exercise checklist. Remote viewing of slides is not a substitute.",
            packet: "Sample packet: twenty fictional alert titles about failed sign-ins and a noisy scanner. One group the model proposes mixes two unrelated assets. Task: break that group and cite the log field that separates them.",
            outcomes:
            [
                "Group alert narratives and then break any group the source log does not support.",
                "Record which prompts were used and which data types they included.",
                "Write the escalation note for a group the analyst will not close from model output alone."
            ],
            prerequisites:
            [
                "Current duty-officer or ticket-triage experience.",
                "Ability to read a log timestamp and an asset name.",
                "Completion of the agency's basic handling rules for operational data."
            ],
            modules:
            [
                ("Queue grouping", "Use the assistant only to cluster, not to close.", "Learners compare the model's groups with a manual pass on five alerts."),
                ("Source confirmation", "A group is a hypothesis.", "Each learner attaches the log field that confirms or splits a group."),
                ("Prompt log and escalation", "Leave a record another shift can audit.", "Write the prompt log and one escalation that refuses an unsupported closure.")
            ],
            exercises:
            [
                ("ex1", "Group and split", "Accept or split each proposed group. For every split, name the field that forced it."),
                ("ex2", "Prompt log", "Record the prompts you used and label each data element as fictional lab data or something you would not have typed."),
                ("ex3", "Escalation note", "Draft the note for the mixed-asset group. State what a human still has to check.")
            ],
            areas: [DolFramework.EvaluateOutputs, DolFramework.UseResponsibly, DolFramework.DirectEffectively, DolFramework.UnderstandPrinciples],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.EnablingRoles]),

        Course(
            id: "status-reports-leaders-trust",
            title: "Status Reports Leaders Can Trust",
            summary: "Program staff practice turning a milestone sheet into a plain-language status note. The lab rule is simple: if the sheet does not contain a number, the note does not invent one.",
            level: CourseLevel.Foundation,
            role: RolePath.ProgramManagement,
            delivery: DeliveryMode.OnlineLive,
            hours: 8,
            audience: "Non-technical program analysts and coordinators who write weekly status notes.",
            scenario: "A director asks for a half-page status on a benefits-portal rollout. The analyst has a milestone sheet. An approved assistant may propose wording. It may not invent percent complete.",
            tools: "The learner's real milestone sheet with names removed, the office status template, and the approved assistant.",
            customize: "Hosts paste their status headings and their rule for what 'blocked' means into the first-session packet.",
            above: "This is past a general introduction to drafting tools. Learners defend every figure and every date against the sheet they already use at work.",
            policy: "The instructor marks attendance for each live meeting. Completion is attendance plus the exercise checklist. A missed meeting without an excuse is recorded as absent and blocks completion.",
            packet: "Sample packet: eight milestones, three dates, and no percents. The model draft claims the rollout is 'about 70 percent complete.' Task: remove that claim and write the status from the sheet only.",
            outcomes:
            [
                "Write a half-page status note a director can read in two minutes.",
                "Remove any figure that is not on the source sheet.",
                "Label blockers in the words the office already uses."
            ],
            prerequisites:
            [
                "The learner currently writes or contributes to a status note.",
                "Basic spreadsheet or document skills.",
                "Permission to bring a milestone list with personal data removed."
            ],
            modules:
            [
                ("Sheet first", "Read the source before prompting.", "Learners circle the only facts the note is allowed to use."),
                ("Draft and strike", "Compare the model draft with the sheet.", "Strike invented percents, dates, and causes."),
                ("Read-aloud", "Check plain language with a partner.", "Partners flag jargon and any sentence that sounds more certain than the sheet.")
            ],
            exercises:
            [
                ("ex1", "Allowed facts", "List the facts from your sheet that may appear in the note. List one temptation the model is likely to invent."),
                ("ex2", "Strike the invention", "Paste the model draft into the lab editor and delete every claim the sheet does not support. Keep the deleted lines in a second paragraph labeled 'removed.'"),
                ("ex3", "Director note", "Submit the half-page note and one sentence on what you refused to include.")
            ],
            areas: [DolFramework.EvaluateOutputs, DolFramework.UseResponsibly, DolFramework.DirectEffectively, DolFramework.ExploreUses],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.Prerequisites]),

        Course(
            id: "summary-against-the-table",
            title: "Checking an AI Summary Against the Source Table",
            summary: "Analysts practice marking an AI summary of a public performance table. Unsupported claims are tagged before the summary can be shown to leadership.",
            level: CourseLevel.Practitioner,
            role: RolePath.DataAnalytics,
            delivery: DeliveryMode.Hybrid,
            hours: 12,
            audience: "Analysts who summarize tables for managers and who are not building models.",
            scenario: "Leadership wants a short read of a public quarterly performance table. An assistant drafted five bullets. Two bullets overstate a change. The analyst's job is the check, not a new chart for its own sake.",
            tools: "A spreadsheet the learner can open, the host office's briefing bullets, and the approved assistant.",
            customize: "Hosts may replace the sample table with a public table from their own program, provided no row contains personal data.",
            above: "Learners are expected to go beyond recognizing a chart. They tie each bullet to a cell, name the comparison period, and state when the table cannot support the sentence.",
            policy: "Hybrid attendance is recorded at both the online working session and the in-person review. Completion depends on that attendance record and the required checks.",
            packet: "Sample packet: a 12-row public-style table of monthly applications received and completed. The draft says completions 'doubled.' The table shows a smaller change. Task: rewrite the bullet and cite the cells.",
            outcomes:
            [
                "Link each summary bullet to the cells that support it.",
                "Rewrite or cut bullets the table does not support.",
                "State the comparison period in words a non-analyst can check."
            ],
            prerequisites:
            [
                "Comfort opening a spreadsheet and reading a row label.",
                "Experience explaining a table to a non-analyst.",
                "No personal data in any file brought to class."
            ],
            modules:
            [
                ("Bullet to cell", "Every sentence needs an address in the table.", "Learners annotate five bullets with cell references."),
                ("What changed, exactly", "Name the period and the unit.", "Rewrite a doubled-claim so it matches the arithmetic."),
                ("Leadership version", "Keep the caution in the note.", "Add one sentence on what the table cannot say.")
            ],
            exercises:
            [
                ("ex1", "Cell map", "For each draft bullet, write the cell or cells you used, or 'no cell supports this.'"),
                ("ex2", "Rewrite the overstatement", "Replace the 'doubled' claim with a sentence that matches the table and names the months."),
                ("ex3", "Limit line", "Add a limit line: what a leader should not conclude from this table.")
            ],
            areas: [DolFramework.EvaluateOutputs, DolFramework.UseResponsibly, DolFramework.UnderstandPrinciples, DolFramework.ExploreUses],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.Pathways]),

        Course(
            id: "oversight-questions",
            title: "Questions to Ask Before an AI Feature Goes Live",
            summary: "Oversight staff practice writing the questions an agency should ask about a vendor's AI feature on a public-facing service. The class uses a fictional vendor note. It is not a legal opinion.",
            level: CourseLevel.Practitioner,
            role: RolePath.Policy,
            delivery: DeliveryMode.InPerson,
            hours: 10,
            audience: "Oversight officers, program counsel liaisons, and policy staff who review vendor proposals. Legal conclusions stay with the agency's counsel.",
            scenario: "A vendor says a new feature will 'automatically decide routine cases' on a public benefits portal. Oversight must produce the question set the program office will send back. The class does not score a real vendor.",
            tools: "The host office's question log or memo, the fictional vendor note, and an approved assistant used only to expand question wording after the officer lists the risks.",
            customize: "Hosts add their decision rights: who may accept a feature, who must review a denial, and which populations need extra care.",
            above: "Staff leave with a question set tied to a live service design, including human review, notice to the public, and what the agency will log. That is role work, not a literacy overview.",
            policy: "In-person attendance is the completion record, together with the question-set exercise. The instructor marks each day. Partial attendance is not rounded up to complete.",
            packet: "Sample packet: a one-page fictional vendor claim that the feature is 'fair because the model was trained on agency data.' Task: write questions that ask what data, what human review, and what a person can appeal. Do not treat the claim as true.",
            outcomes:
            [
                "Write a question set that covers purpose, data, human review, notice, and appeal.",
                "Separate questions the program office can answer from questions that need counsel or security.",
                "Describe one decision the feature must not make without a person."
            ],
            prerequisites:
            [
                "Familiarity with how the learner's program reviews a vendor change.",
                "Willingness to write in plain language for a public audience.",
                "No real vendor proposal in the classroom unless the host's counsel has cleared it."
            ],
            modules:
            [
                ("Claim versus question", "Turn marketing lines into questions.", "Learners underline claims in the fictional note and write the question each claim dodges."),
                ("Human review and notice", "Ask who acts when the feature is wrong.", "Draft the appeal and notice questions in words a resident could understand."),
                ("Routing the set", "Send each question to the right office.", "Mark each question for program, security, counsel, or vendor.")
            ],
            exercises:
            [
                ("ex1", "Claim list", "List the vendor claims that need a question, in the vendor's words and then in yours."),
                ("ex2", "Question set", "Write at least eight questions covering data, review, notice, and appeal."),
                ("ex3", "Do-not-automate line", "Name one decision on this service that must stay with a person, and the reason in one paragraph.")
            ],
            areas: [DolFramework.UseResponsibly, DolFramework.EvaluateOutputs, DolFramework.ExploreUses, DolFramework.UnderstandPrinciples],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.HumanSkills, DolFramework.EnablingRoles, DolFramework.Agility]),

        Course(
            id: "evaluating-a-pilot",
            title: "Evaluating an Agency Pilot Without Becoming the Vendor",
            summary: "Technical evaluators design a human-review sample and a logging checklist for a pilot that drafts public correspondence. They do not train a model and they do not connect production systems.",
            level: CourseLevel.AdvancedTechnical,
            role: RolePath.DataAnalytics,
            delivery: DeliveryMode.OnlineLive,
            hours: 14,
            audience: "Evaluators, data leads, and engineers who will judge a pilot. The class is about the evaluation plan, not model building.",
            scenario: "A program wants a pilot in which an assistant drafts replies to public questions about office hours. Evaluation must say how many drafts a person reviews, what is logged, and what ends the pilot.",
            tools: "The host's pilot charter template, a fictional mail sample with no personal data, and a spreadsheet for the sample plan.",
            customize: "Hosts insert their stop rules, their records schedule, and the office that owns the decision to end the pilot.",
            above: "Participants already understand that models can draft. Here they specify sampling, error categories, and logs an auditor could read later.",
            policy: "Live online attendance is recorded each session. The evaluation plan is required, and it is not accepted without attendance. Recordings, if the host makes them for absentees, do not grant completion.",
            packet: "Sample packet: forty fictional public questions about office hours. Task: choose a review sample, define three error types (wrong hours, invented service, wrong tone), and write the stop rule.",
            outcomes:
            [
                "Define a human-review sample and the reason for its size.",
                "Name error types a reviewer can apply the same way twice.",
                "Write the log fields and the stop rule for the pilot."
            ],
            prerequisites:
            [
                "Experience reviewing someone else's work product for errors.",
                "Spreadsheet skills sufficient to count a sample.",
                "No production mailbox connected during class."
            ],
            modules:
            [
                ("What the pilot is allowed to draft", "Bound the task before measuring it.", "Learners cut the fictional charter down to office-hours replies only."),
                ("Sample and error types", "Make review repeatable.", "Pairs define error types and test them on five letters."),
                ("Logs and stop rules", "Decide in advance what ends the pilot.", "Write the fields captured for each draft and the threshold that stops the pilot.")
            ],
            exercises:
            [
                ("ex1", "Task bound", "Rewrite the pilot purpose in four sentences. Name what the assistant must not answer."),
                ("ex2", "Sample plan", "Choose how many drafts a person will review in week one and why that number is enough to learn, not to prove the tool."),
                ("ex3", "Stop rule", "Write the stop rule and the five log fields you would keep for an auditor.")
            ],
            areas: [DolFramework.EvaluateOutputs, DolFramework.UseResponsibly, DolFramework.DirectEffectively, DolFramework.UnderstandPrinciples],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.Pathways, DolFramework.Agility, DolFramework.HumanSkills]),

        Course(
            id: "coaching-the-team",
            title: "Coaching a Team That Uses an Approved Assistant",
            summary: "Supervisors practice setting team rules, reviewing staff drafts, and coaching without taking over the pen. Built for people who enable others, not only for individual users.",
            level: CourseLevel.Practitioner,
            role: RolePath.ProgramManagement,
            delivery: DeliveryMode.Hybrid,
            hours: 9,
            audience: "Supervisors and team leads whose staff already have access to an agency-approved assistant.",
            scenario: "Three staff members used an assistant on this week's public FAQ update. One draft invented a fee. The supervisor must coach the team and set a written rule for next week.",
            tools: "The host's supervisory checklist, the approved-tool list, and redacted staff drafts.",
            customize: "Hosts bring their actual tool list and the name of the office that must approve a new tool. The instructor does not introduce unsanctioned products.",
            above: "The work is managerial: norms, review, and escalation. It is the DOL 'enabling roles' path, taught as practice rather than a slide on leadership.",
            policy: "Attendance at the live huddles is required and is marked by the instructor. The team-rule exercise is also required. Reading the huddle notes later is not completion.",
            packet: "Sample packet: three short FAQ drafts. One states a fee the source page does not list. Task: write the coaching note and a five-line team rule.",
            outcomes:
            [
                "Write a team rule that says what staff may ask an assistant to draft.",
                "Give feedback on a draft that invented a fact, without rewriting it for the employee.",
                "Name who on the team can approve an exception."
            ],
            prerequisites:
            [
                "The learner supervises at least one person, or is about to.",
                "The host has named at least one approved assistant.",
                "Basic document editing skills."
            ],
            modules:
            [
                ("The rule before the tool", "Staff need a boundary they can remember.", "Draft a five-line rule from the host's tool list."),
                ("Coaching the invented fee", "Correct the practice, not only the sentence.", "Role-play the conversation. The employee keeps the pen."),
                ("Exception path", "Say where a hard case goes.", "Write the one-step escalation the team will actually use.")
            ],
            exercises:
            [
                ("ex1", "Five-line rule", "Write the rule in words your team would follow on a busy Friday."),
                ("ex2", "Coaching note", "Comment on the invented fee. Point to the source page. Do not silently replace the employee's draft."),
                ("ex3", "Exception path", "Name the person or office that approves an exception, and what the ask must include.")
            ],
            areas: [DolFramework.UseResponsibly, DolFramework.EvaluateOutputs, DolFramework.ExploreUses, DolFramework.DirectEffectively],
            principles: [DolFramework.Experiential, DolFramework.InContext, DolFramework.EnablingRoles, DolFramework.HumanSkills, DolFramework.Pathways]),

        NotApprovedRecording()
    ];

    private static Course NotApprovedRecording() =>
        new()
        {
            Id = "recorded-office-tour",
            Title = "Recorded Tour of Common Office Assistants",
            Summary = "A one-hour recording that points at generic assistant menus. It is listed so staff can see what the Academy will not mark approved.",
            Level = CourseLevel.Foundation,
            Role = RolePath.ProgramManagement,
            Delivery = DeliveryMode.SelfPacedRecording,
            Approval = ApprovalStatus.InReview,
            InstructorLed = false,
            AppliedPractice = false,
            ContextualizedToRole = false,
            AboveBaselineLiteracy = false,
            StructuredParticipation = false,
            AttendanceVerified = false,
            IsPassiveRecording = true,
            ContactHours = 1,
            Audience = "Anyone who might be tempted to assign a video instead of a class.",
            Scenario = "None. The recording does not use the learner's cases, tools, or templates.",
            ToolsContext = "Whatever product happens to appear on screen.",
            EmployerCustomization = "",
            AboveBaselineNote = "",
            AttendancePolicy = "The recording does not take attendance and does not record completion.",
            LabPacket = "There is no lab.",
            Outcomes = ["Recognize a few buttons in a generic assistant."],
            Prerequisites = [],
            Modules = [new Module { Title = "Watch the tour", Purpose = "Passive viewing.", Practice = "None." }],
            Exercises = [],
            DolContentAreas = [DolFramework.UnderstandPrinciples],
            DolDeliveryPrinciples = [DolFramework.Prerequisites]
        };

    private static Course Course(
        string id,
        string title,
        string summary,
        CourseLevel level,
        RolePath role,
        DeliveryMode delivery,
        int hours,
        string audience,
        string scenario,
        string tools,
        string customize,
        string above,
        string policy,
        string packet,
        string[] outcomes,
        string[] prerequisites,
        (string Title, string Purpose, string Practice)[] modules,
        (string Id, string Title, string Brief)[] exercises,
        string[] areas,
        string[] principles) =>
        new()
        {
            Id = id,
            Title = title,
            Summary = summary,
            Level = level,
            Role = role,
            Delivery = delivery,
            Approval = ApprovalStatus.Approved,
            InstructorLed = true,
            AppliedPractice = true,
            ContextualizedToRole = true,
            AboveBaselineLiteracy = true,
            StructuredParticipation = true,
            AttendanceVerified = true,
            IsPassiveRecording = false,
            ContactHours = hours,
            Audience = audience,
            Scenario = scenario,
            ToolsContext = tools,
            EmployerCustomization = customize,
            AboveBaselineNote = above,
            AttendancePolicy = policy,
            LabPacket = packet,
            Outcomes = outcomes,
            Prerequisites = prerequisites,
            Modules = modules.Select(m => new Module { Title = m.Title, Purpose = m.Purpose, Practice = m.Practice }).ToList(),
            Exercises = exercises.Select(e => new Exercise
            {
                Id = $"{id}-{e.Id}",
                CourseId = id,
                Title = e.Title,
                Brief = e.Brief,
                RequiredForCompletion = true
            }).ToList(),
            DolContentAreas = areas,
            DolDeliveryPrinciples = principles
        };
}
