# GA-PE-210 Prompt Engineering for Government Officers

Instructor-led course outline for GovAI Academy.

Unclassified training. Original academy material. This course is **not** a U.S. Department of Labor publication and does not authorize any commercial tool.

The illustrated instructor and synthetic narration in the briefing video are production choices so every section can replay the same lesson. A live facilitator still owns attendance, questions, the labs, and the completion record.

## 1. Who the course is for

Officers and professional staff whose daily work is non-technical or only partly technical: correspondence, front counter, program support, training, analysis, and briefing preparation.

No coding. No model-building. Participants already meet a **baseline of AI literacy** and are ready for supervised practice above that floor.

## 2. Where it sits above the Department of Labor baseline

Training and Employment Notice 07-25, *The U.S. Department of Labor’s Artificial Intelligence Literacy Framework* (February 13, 2026), is voluntary guidance. It defines AI literacy as a foundational set of competencies for using and evaluating generative AI, and it says plainly that literacy is a baseline. Many roles need greater depth.

The framework’s five content areas are the floor this course assumes:

1. Understand AI principles, including that outputs are probabilistic and can be fluent and false.
2. Explore workplace uses.
3. Direct AI with clear instructions, context, and iteration.
4. Evaluate outputs for accuracy, completeness, and fitness.
5. Use AI responsibly, including protection of sensitive information.

Course 210 does not reteach that floor. It drills **applied direction** for ordinary unclassified government office work:

- A repeatable five-part prompt (role, context, task, constraints, output).
- Source binding, so the model may use only text the officer supplies.
- A four-pass revision sequence.
- A written accept / revise / reject decision before any text is reused.
- Government boundaries: classified information, controlled unclassified information, personal data, records, and the line between a draft and an official word.

Delivery matches the framework’s preference for hands-on practice. The session is instructor-led. Attendance is required. Three exercises are required. Watching the video is not completion.

## 3. Gov-safe rules

Read these aloud before anyone opens a tool.

- This session is unclassified. Do not bring classified information in any form, including paraphrase.
- Do not bring controlled unclassified information, or drafts your office treats as controlled, unless that exact tool is authorized for that information. This course does not grant that authorization.
- Do not bring personal data, personnel matters, health or financial details, investigative material, source-selection material, attorney work product, or credentials.
- Use only a tool the host agency has already approved **for this training**. If no tool is approved, run every lab on paper. The grade is the prompt and the judgment, not a live reply.
- Examples and worksheets are fictional. Harbor Town is not a real jurisdiction in this course. Do not “improve” a worksheet with a real case.
- Assume prompts and replies can be logged, reviewed, and retained.
- A chat is not the official record. Cleared language moves into the system your office already uses.
- The officer who adopts the words owns them. The vendor does not.

Printer test, said as often as needed: if you would not leave the text on a shared printer, do not put it in a prompt.

## 4. Time

| Block | Minutes | What happens |
| --- | ---: | --- |
| Welcome, attendance, rules | 15 | Sign-in. Read the gov-safe rules. Confirm no live cases. |
| Briefing | 30 | Play the video, or teach the same deck live from the notes. |
| Lab 1 | 20 | Rebuild a thin prompt. |
| Lab 2 | 25 | Write pass two and pass three. |
| Lab 3 | 25 | Strip an oversharing prompt and choose a quality-gate door. |
| Debrief and completion | 15 | Pairs report one fence they will keep. Collect worksheets. |
| **Facilitated session** | **130** | Attendance plus three worksheets. |

The video is the briefing only. It is about 30 minutes. Do not describe a shorter cut as the full briefing.

## 5. Outcomes

By the end of the session, a participant can:

1. Build a prompt with role, context, task, constraints, and output.
2. Bind the model to a source that is actually in the prompt, and require the phrase “not in source” for holes.
3. Revise in passes instead of accepting the first draft.
4. Reject an output that is the wrong job, invents a material fact, or sits in a thread that already contains sensitive information.
5. State that “accept as a draft” is not clearance, signature, or public release.

## 6. Attendance and completion

Record attendance at three moments: opening, the start of Lab 1, and dismissal.

Completion requires all of the following:

- Present for the briefing and for each lab check.
- Worksheet 1, Worksheet 2, and Worksheet 3 turned in.
- Each worksheet names a quality-gate door in one sentence (Lab 1 may say “not run — prompt only”).

Video-only viewing does not complete GA-PE-210. There is no score on a commercial model’s style. Credit follows the rubric in each lab.

Suggested roster columns: name, office, time in, present at Lab 1, worksheets received (1 / 2 / 3), facilitator initials. Do not collect personal data beyond what your training office already requires for a class roster.

## 7. Materials

- Slide deck: `slides/Prompt-Engineering-for-Government-Officers.pptx` (speaker notes are the narration).
- Briefing video: `video/GA-PE-210-prompt-engineering.mp4` (1080p, instructor in the background, slides in front).
- Narration script with timestamps: `narration/narration-script.md`.
- Captions: `narration/captions.vtt`.
- This outline, including the three worksheets.
- An approved training tool, or paper and a partner.

Facilitators may teach the deck live instead of playing the video. Do not skip the labs in either case.

## 8. Briefing map

Timestamps below are the teaching order. Exact clock times are in the narration script, which is generated from the finished soundtrack.

| Slides | Section | Facilitator cue |
| --- | --- | --- |
| 01–04 | Opening, outcomes, DOL floor | After slide 02, stop the video if anyone has live-work questions. Answer with the boundary, not with a workaround. |
| 05–09 | What a prompt is; three failures; workplace test; never-enter list | Security callout. Pause if the room needs the printer test repeated. |
| 10–18 | Anatomy and the Harbor Town rebuild | Have participants find the five labels on slide 18. |
| 19–24 | Best practices | If time is tight in a live teaching, do not cut slide 22 (source binding). |
| 25–30 | Getting more: passes, example, gaps, critique, chaining | This is the applied layer above baseline “try a clearer prompt.” |
| 31–36 | Workplace patterns | Remind the room the figures exist only inside the prompts. |
| 37–38 | Quality gate, records, ownership | Security and records callout. |
| 39–40 | Lab briefing and close | Stop. Take attendance again. Hand out worksheets. |

## 9. Lab 1 — Rebuild the prompt (20 minutes)

**Setup.** Individuals, then two minutes with a partner. Paper. No outside facts.

**Thin prompt (do not “fix” this by adding real events):**

> Write something nice about the volunteer day so people show up.

**Only source allowed:**

> Harbor Town Parks will host a riverside cleanup on Saturday, April 18, from 8:30 a.m. to 11:00 a.m. Volunteers meet at the public shelter near Pier 2. The city will supply gloves and bags. Children under 12 must come with an adult. No registration fee is stated in this fact sheet.

**Task for the participant.** Rewrite the prompt with all five parts. One job only: a short public announcement a supervisor could review. Require “not in source” for anything missing, including a rain plan, parking details, and a contact phone number. End with a Gaps line. Cap the requested announcement at 120 words. State that the draft is not an official post.

**Rubric (complete / incomplete).**

- Role names a staff job and a public reader.
- Context pastes or clearly fences the fact sheet as the only source.
- Task is one verb and one use.
- Constraints forbid invented weather policy, phone numbers, and quotations.
- Output names a shape and a Gaps line.
- No fact from outside the sheet.

## 10. Lab 2 — Two passes on a public reply (25 minutes)

**Resident question (fictional):**

> Is the cleanup canceled if it rains, and can my 10-year-old come alone?

**Same fact sheet as Lab 1.** Rain policy is not in the source. The sheet does say children under 12 must come with an adult.

**Pass 2.** Write the prompt that fills a four-sentence reply from the source only, with holes labeled “not in source.”

**Pass 3.** Write the follow-up that applies plain language and a calm tone without adding facts.

If an approved tool is available, participants may run those two prompts and mark any claim they cannot find in the sheet. If no tool is available, they exchange papers. The partner underlines any sentence that a model would have to invent (a rain rule, a phone number, permission for a child to attend alone).

**Rubric.**

- The reply prompt does not say the cleanup is canceled or confirmed in the rain.
- The reply prompt does not allow a 10-year-old to come alone.
- Pass 3 changes tone or length only. It does not open a new job.
- The participant names the quality-gate door they would use if a draft invented a rain policy: **Reject**.

## 11. Lab 3 — Strip the overshare (25 minutes)

**Prompt as received from a colleague (fictional; deliberately unfit):**

> Mrs. Alvarez at 418 West Cedar Street said her son Mateo, age 8, was bored at home. Also Dana in accounting is lazy and never orders the bags. Write a cheerful public post about the cleanup and mention them so it feels real.

**Participant tasks.**

1. Mark every piece that fails the never-enter list (home address, a child’s name and age, a personnel remark).
2. State the quality-gate door for this thread: **Reject**. Say why in one sentence. Include the point that asking a model to “forget” does not remove a log.
3. Rewrite a safe prompt for a public announcement using only the Lab 1 fact sheet. Do not carry the names, the street, the age, or the remark about Dana into the rewrite, even as a negative example (“do not mention Mateo”).

**Rubric.**

- Door is Reject, with a reason tied to personal data or a personnel remark, not to writing quality.
- The rewritten prompt contains none of the prohibited details.
- The rewritten prompt still has the five parts and source binding.

Adding a story from the participant’s real week fails the lab.

## 12. Debrief (15 minutes)

Ask three questions. Take two voices each. Do not collect live cases.

1. Which fence will you keep when you are rushed: one job, source binding, or the Gaps line?
2. Where did a thin prompt invite a fact that was not in the sheet?
3. What will you do when a colleague offers text that fails the printer test?

Close by restating ownership: the official word begins when a person adopts it through the office’s ordinary process.

## 13. Facilitator notes

- If a participant starts to describe a real case, stop the description. Offer to talk after class about the process, not the facts.
- Do not demonstrate a public commercial tool with anything typed by the room unless your agency has approved that exact action.
- Do not grade enthusiasm. Grade the fences.
- Slide speaker notes match the video narration. Teaching live, you may shorten an example, but keep slides 09, 14, 22, 37, and 38.
- Security callouts are part of the lesson, not an appendix. They appear in the opening, the never-enter list, source binding, the quality gate, and the records close.
- Store worksheets the way you store any other training product. Do not upload them to an unapproved tool “to summarize the class.”

## 14. What this course does not do

- It does not certify a participant to handle classified or controlled information in an AI tool.
- It does not replace counsel, records officers, or your agency AI use policy.
- It does not teach model training, fine-tuning, or bypassing a tool’s controls.
- It does not make a generated paragraph an official statement.
