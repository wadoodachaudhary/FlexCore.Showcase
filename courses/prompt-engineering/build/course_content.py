"""Source content for GovAI Academy course GA-PE-210.

Original training material. Examples are fictional. No classified or controlled data.
"""

COURSE_CODE = "GA-PE-210"
COURSE_TITLE = "Prompt Engineering for Government Officers"
VOICE = "en-US-JennyNeural"
# Clear training pace. The build measures duration and may apply a small atempo
# only if the synthesized track falls outside 28–32 minutes.
VOICE_RATE = "+12%"

SLIDES = [
    {
        "id": "01",
        "layout": "title",
        "section": "Opening",
        "kicker": "GovAI Academy  ·  Instructor-led briefing",
        "title": "Prompt Engineering for Government Officers",
        "subtitle": "Applied practice above baseline AI literacy",
        "bullets": [
            "Course GA-PE-210  ·  Unclassified training",
            "About 30 minutes of briefing, then supervised labs",
            "Illustrated instructor and synthetic narration",
        ],
        "narration": (
            "Welcome to Gov A I Academy course 210, Prompt Engineering for Government Officers. "
            "This is an unclassified briefing for officers whose work is non-technical or only partly technical. "
            "You will not write code. You will learn to assign work to a generative model with the same care you use when you task a new colleague. "
            "The briefing runs about thirty minutes. In the facilitated session, your instructor takes attendance, plays or teaches this briefing, and then leads the labs. "
            "The figure beside the slides is an illustrated instructor, and this soundtrack is synthetic narration, so the lesson can be replayed when live faculty are with another section. "
            "Your facilitator still owns attendance, questions, and completion. "
            "Every example in the deck is fictional and safe to read aloud. Do not improve the examples by adding facts from your real workload."
        ),
    },
    {
        "id": "02",
        "layout": "alert",
        "section": "Opening",
        "kicker": "How this session runs",
        "title": "Attendance, exercises, and a hard boundary",
        "bullets": [
            "Sign in. Stay for the briefing and each lab check.",
            "Completion requires the three worksheets, not the video alone.",
            "Approved tool only. If none is approved, work the labs on paper.",
            "No classified information. No controlled unclassified information. No personal data.",
        ],
        "callout": "If you would not leave the text on a shared printer, do not put it in a prompt.",
        "narration": (
            "Treat this room, and any practice tool, as an official setting. "
            "Attendance is recorded at the start, at each lab check, and at dismissal. Watching the video alone does not complete the course. "
            "You will finish three short exercises. They use fictional public facts printed on the worksheets. "
            "Use only a tool your agency has already approved for training. If no tool is approved, write the prompts on paper. The skill is the writing and the judgment, not the brand of software. "
            "Here is the boundary that does not bend. Do not enter classified information. Do not enter controlled unclassified information. Do not enter personal data, personnel matters, investigative details, source-selection material, or attorney work product. "
            "Do not enter a draft decision that has not been cleared for that system. "
            "A useful test: if you would not leave the text on a shared printer in the hallway, do not put it in a prompt."
        ),
    },
    {
        "id": "03",
        "layout": "bullets",
        "section": "Opening",
        "kicker": "By the end of the facilitated session",
        "title": "What you will be able to do",
        "bullets": [
            "Build a prompt with role, context, task, constraints, and output.",
            "Bind the model to a source you actually provide.",
            "Revise in passes instead of accepting the first draft.",
            "Reject an output that is unfit for official use.",
        ],
        "narration": (
            "By the end of the facilitated session you should be able to do four things without a template in your lap. "
            "First, build a prompt that names the role, the context you are willing to share, the task, the constraints, and the output you want. "
            "Second, bind the model to a source you provide, so it does not fill gaps with confident invention. "
            "Third, revise in passes. You will ask for gaps, then for a tighter format, instead of hoping the first reply is finished work. "
            "Fourth, reject an output that is unfit for official use, and say why in a sentence a supervisor can follow. "
            "Those four skills sit on top of general familiarity with artificial intelligence. They are the applied standard for this course. "
            "You are not being asked to become a model designer. You are being asked to remain the accountable officer when a draft appears quickly."
        ),
    },
    {
        "id": "04",
        "layout": "bullets",
        "section": "Opening",
        "kicker": "Where this course sits",
        "title": "Above the Department of Labor baseline",
        "bullets": [
            "Baseline: DOL AI Literacy Framework, TEN 07-25 (February 13, 2026).",
            "Literacy covers principles, uses, basic direction, evaluation, and care.",
            "This course is applied direction for unclassified government work.",
            "Prerequisite: you already know outputs can be wrong, and data has rules.",
        ],
        "callout": "TEN 07-25 is the floor. This briefing is not a Department of Labor publication.",
        "narration": (
            "The Department of Labor published its Artificial Intelligence Literacy Framework in Training and Employment Notice 07-25, dated February 13, 2026. "
            "That notice is voluntary guidance. It defines A I literacy as a foundational set of competencies for using and evaluating artificial intelligence, with emphasis on generative tools. "
            "The framework names five content areas. Understand how these systems behave. Explore real uses. Direct them with clear instructions. Evaluate the output. Use them responsibly, including protection of sensitive information. "
            "The Department is explicit that literacy is a baseline, and that many roles need a greater depth of skill. "
            "This academy treats that framework as the floor you should already meet. You should already know that a model produces likely text, not a looked-up record, and that a fluent paragraph can still be false. "
            "Course 210 does not reteach that floor. It drills applied direction for ordinary government office work: a repeatable prompt structure, source binding, a fixed revision sequence, and a written accept-or-reject decision. "
            "Delivery is instructor-led, with attendance and exercises, which matches the framework's preference for hands-on practice over a lecture alone."
        ),
    },
    {
        "id": "05",
        "layout": "bullets",
        "section": "Prompts",
        "kicker": "Introducing prompts",
        "title": "A prompt is an assignment, not a search",
        "bullets": [
            "A prompt is the full instruction the model receives.",
            "Keywords are enough for a search box. They are thin as a tasking.",
            "Missing pieces are filled by guesswork.",
            "Write the assignment you would give a fast, literal new hire.",
        ],
        "narration": (
            "A prompt is the full instruction you give a generative model. It may be a sentence. It may be a short brief. Either way, it is the assignment. "
            "A search box does well with a few keywords because it retrieves pages that already exist. A generative model writes new text that resembles a useful answer. "
            "If your assignment is thin, the model still writes. It guesses the audience, the tone, the missing facts, and the format. "
            "That guess is the usual source of a draft that sounds polished and still misses the job. "
            "Write the prompt the way you would task a new hire who is quick, literal, and unable to see your files, your inbox, or the conversation you had in the hallway. "
            "If the new hire would have to stop and ask a question, the prompt needs that answer before you send it. "
            "You are not chatting for entertainment. You are commissioning a draft you may later have to defend."
        ),
    },
    {
        "id": "06",
        "layout": "bullets",
        "section": "Prompts",
        "kicker": "A working picture, not a engineering course",
        "title": "What the model does with your words",
        "bullets": [
            "It continues text in a way that fits patterns from training.",
            "The same prompt can yield different wording next time.",
            "Fluency is not evidence. Confidence is not a citation.",
            "Your prompt is the steering. It is not a guarantee.",
        ],
        "narration": (
            "You do not need the mathematics. You do need a picture that keeps you honest. "
            "During training, a model is adjusted on very large collections of text and, for some products, images or other media. "
            "When you submit a prompt, the system produces a continuation that fits those patterns, shaped by the product's safety rules and by any instructions the vendor or your agency added outside your view. "
            "That step is inference. The model is not opening your agency's system of record unless a separate, approved connection was built and you were told so. "
            "The same prompt can produce different wording on a second try. Treat that as normal, not as a malfunction. "
            "A smooth paragraph is not evidence. A confident date, quotation, or legal citation is not a citation until you check it against a source you trust. "
            "Your prompt steers the draft. It does not certify it. The officer who uses the text remains responsible for it."
        ),
    },
    {
        "id": "07",
        "layout": "bullets",
        "section": "Prompts",
        "kicker": "What officers notice first",
        "title": "Three failures that look like success",
        "bullets": [
            "Invented specifics: a date, a quote, a statute, a cost.",
            "The right tone for the wrong reader.",
            "A complete-looking answer that skipped a required step.",
        ],
        "narration": (
            "Most weak drafts fail in one of three ways, and all three can look successful at a glance. "
            "The first is invented specifics. The model supplies a meeting date, a quotation, a dollar figure, or a statute that was never in your prompt. The sentences are grammatical, so the invention is easy to miss. "
            "The second is the right tone for the wrong reader. A note written for the public lands on your desk when you needed a briefing for a division chief, or the reverse. Nothing is false, and the work is still unfit. "
            "The third is a skipped obligation. The draft answers a neighboring question and never reaches the decision you actually needed. It feels complete because it is long. "
            "When you review a draft, hunt those three failures before you edit style. Style is cheap to fix. A fabricated fact in a memo that leaves the office is not. "
            "Name the failure in plain language when you send the work back. That sentence becomes your next prompt."
        ),
    },
    {
        "id": "08",
        "layout": "bullets",
        "section": "Prompts",
        "kicker": "Before you type",
        "title": "The workplace test",
        "bullets": [
            "Is this task allowed on this tool, under our policy?",
            "Can I do the job with facts I am allowed to paste?",
            "Will a person review the result before anyone relies on it?",
            "If any answer is no, do not prompt. Use the ordinary process.",
        ],
        "narration": (
            "Before you write a clever prompt, pass a shorter test. "
            "Is this task allowed on this tool under your agency's current policy? Training approval is not the same as approval for casework. "
            "Can you do the job with facts you are allowed to paste? If the useful version of the task requires material you cannot enter, the safe version is a different task, or no task at all. "
            "Will a person review the result before anyone relies on it? A draft that will be forwarded untouched is not a draft. It is an official communication wearing a costume. "
            "If any answer is no, do not prompt. Use the ordinary process: the template, the specialist, the counsel, the system of record. "
            "Prompt engineering does not expand your authority. It only changes how fast a piece of text can appear. Speed is a reason for a stricter test, not a looser one."
        ),
    },
    {
        "id": "09",
        "layout": "alert",
        "section": "Prompts",
        "kicker": "Security and privacy",
        "title": "What never goes in a prompt",
        "bullets": [
            "Classified information, at any level, in any form.",
            "Controlled unclassified information and similarly marked drafts.",
            "Personal data, personnel actions, health, finance, or home contact details.",
            "Investigations, source selection, legal advice, and credentials or passwords.",
        ],
        "callout": "A prompt may be logged, reviewed, and retained. Write it that way.",
        "narration": (
            "Memorize the categories, not a vendor's marketing page. "
            "Never put classified information into a prompt, including a paraphrase that still conveys classified content. This course is not a classified channel. "
            "Never put controlled unclassified information, or drafts your office treats as controlled, into a tool that has not been authorized for that information. When you are unsure of the marking, leave it out and ask. "
            "Never put personal data. That includes Social Security numbers, home addresses, personal phone numbers, medical information, and details of a personnel action. "
            "Never put investigative details, source-selection or bid material, or attorney-client and deliberative legal advice. "
            "Never put passwords, tokens, or access badges into a prompt, even as an example. "
            "Assume the prompt and the reply can be logged, reviewed by administrators, and retained under a records rule you do not control. "
            "If a colleague hands you text to drop into a tool, you still own the decision to refuse it."
        ),
    },
    {
        "id": "10",
        "layout": "section",
        "section": "Anatomy",
        "kicker": "Part two",
        "title": "The anatomy of a working prompt",
        "subtitle": "Five parts you can rebuild under time pressure",
        "bullets": [
            "Role",
            "Context",
            "Task",
            "Constraints",
            "Output",
        ],
        "narration": (
            "We will use one structure for the rest of the course. Five parts: role, context, task, constraints, and output. "
            "The initials R C T C O are only a memory aid. The order on the page can change. The presence of each part should not. "
            "Officers who already write decent prompts usually have a clear task and a missing constraint, or a clear format and no source. "
            "The structure makes the missing part visible before you hit enter. "
            "You can keep the labels in the prompt itself. Models follow labeled instructions well, and a later reader, including you next week, can see what you asked. "
            "We will take the five parts one at a time, then assemble them."
        ),
    },
    {
        "id": "11",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Part 1 of 5",
        "title": "Role: who is doing the work",
        "bullets": [
            "Name the job, not a celebrity and not a vague expert.",
            "Say who the reader is.",
            "Keep the role inside the employee's real duties.",
            "A role sets tone and emphasis. It does not grant authority.",
        ],
        "narration": (
            "Role tells the model what kind of staff work to imitate. "
            "Use a job you would actually assign: staff assistant preparing a read-ahead, correspondence clerk answering a public question, training coordinator drafting practice items. "
            "Also name the reader. A division chief, a front-desk colleague, and a member of the public are three different pieces of writing. "
            "Skip theater. Asking the model to act as a famous person, a judge, or the head of your agency adds costume and not accountability. "
            "Keep the role inside duties an employee may perform. A role does not give the model authority to decide, to sign, or to speak for the government. "
            "A clean role line sounds like this. You are a staff assistant in a municipal library office. You write for the library board, which is a public body. You prepare drafts a human supervisor will edit."
        ),
    },
    {
        "id": "12",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Part 2 of 5",
        "title": "Task: the one job, in one verb",
        "bullets": [
            "Start with a verb: outline, compare, list, rewrite, check.",
            "State the finished use: read-ahead, public reply, training item.",
            "One job per prompt. Split a second job into a later pass.",
            "If you cannot say the task in a sentence, you are not ready to send it.",
        ],
        "narration": (
            "The task is the job, stated with a verb a supervisor could grade. "
            "Outline a one-page read-ahead. Compare two published options. Turn notes into an action list. Rewrite a paragraph for plain language. Draft five study questions. "
            "Add the finished use in the same breath. A comparison for a briefing book is different from a comparison for a press statement. "
            "Give the model one job. When you ask for a summary, a recommendation, and a public speech in the same prompt, you usually get a blend that serves none of them. "
            "The second job can be the next prompt, after you have checked the first result. "
            "If you cannot say the task in one sentence without the words and so on, you are not ready to send it. Finish the sentence on paper first. "
            "That pause is part of the craft. It is also where you notice you were about to paste material that does not belong in the tool."
        ),
    },
    {
        "id": "13",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Part 3 of 5",
        "title": "Context: only what you may hand over",
        "bullets": [
            "Paste the facts the model is allowed to see.",
            "Label them as the source.",
            "Say what you are withholding, in general terms.",
            "Do not ask the model to recall your office from memory.",
        ],
        "narration": (
            "Context is the material you deliberately hand over. It is not everything you know. "
            "Paste a short public fact, a sanitized paragraph, or a fictional scenario from the worksheet. Label it. Source follows, or Fact sheet. "
            "Then fence it. Tell the model that this text is the only factual source for the assignment. "
            "If something important must stay out, say so in general terms. Do not describe the sensitive material in order to say you are excluding it. A description can be a disclosure. "
            "A sentence such as, internal deliberations and personal data are not included, and you must not infer them, is enough. "
            "Do not write, based on what you know about our director, our pending case, or our budget. The model does not know your office. If it answers as if it does, it is improvising. "
            "Context you did not supply is not context. It is a risk."
        ),
    },
    {
        "id": "14",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Part 4 of 5",
        "title": "Constraints: the fences",
        "bullets": [
            "Use only the source. Mark gaps as not in source.",
            "Forbid invented quotations, citations, and numbers.",
            "Set length, reading level, and what to leave out.",
            "State that the draft is not an official position.",
        ],
        "narration": (
            "Constraints are the fences. Good officers already think this way when they edit. Put the fences in the prompt so the first draft trips over fewer of them. "
            "The most important fence in government work is source binding. Use only the source provided. If a fact, date, name, or figure is absent, write the words not in source. Do not estimate. "
            "Forbid invented quotations and invented legal or policy citations. If you want a place held for a citation a person will add, say, leave a bracket: citation needed. "
            "Set length. Two hundred words is a real instruction. Be brief is a mood. "
            "Set reading level when the reader is the public. Plain language, short sentences, no jargon unless the source uses it and the reader needs it. "
            "Say what to leave out. No recommendation unless you actually want one. No background history. No greeting. "
            "Add one line that protects the record. This draft is not an official position and must be reviewed by a person before use."
        ),
    },
    {
        "id": "15",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Part 5 of 5",
        "title": "Output: the shape you can check",
        "bullets": [
            "Name the headings, the table columns, or the numbered list.",
            "Say how many items, and how long each should be.",
            "Ask for a final line titled Gaps.",
            "A shape you can scan is a shape you can grade.",
        ],
        "narration": (
            "Output is the shape of the reply. Decide it before you send the prompt, the way you decide the sections of a memo before you draft. "
            "Name the headings. Purpose, facts from the source, decision needed, open questions. Or specify a table with three columns: option, what the source says, what is not in the source. "
            "Say how many items you want, and a ceiling for each. Five questions, each one sentence, each answerable from the fact sheet. "
            "Ask for a last line titled Gaps. That line is where the model must admit what the source did not contain. It is also the first place you will look. "
            "Avoid the request, write it nicely. Nice is not checkable. Five headings and a gaps line are checkable in under a minute. "
            "If the reply ignores the shape, do not silently repair it and move on. Send one follow-up that repeats the shape. You are training your own habit as much as you are correcting the draft."
        ),
    },
    {
        "id": "16",
        "layout": "bullets",
        "section": "Anatomy",
        "kicker": "Memory aid",
        "title": "R C T C O, then stop and read it back",
        "bullets": [
            "Role — the job and the reader.",
            "Context — the only source you provide.",
            "Task — one verb and the finished use.",
            "Constraints — source binding, length, prohibitions.",
            "Output — headings plus a Gaps line.",
        ],
        "callout": "Read the prompt once as if you were the person who must live with the draft.",
        "narration": (
            "Put the five parts together and then stop. Read the prompt once, aloud if you can, as if you were the colleague who must live with the draft. "
            "Role: the job and the reader. Context: the only source. Task: one verb and the use. Constraints: source binding, length, and prohibitions. Output: headings and a gaps line. "
            "Ask three questions on that read-through. Did I include a fact I am not allowed to share? Did I ask for two jobs? Did I leave a hole that will be filled by invention? "
            "If the prompt fails any of those, fix the prompt. Do not plan to fix the damage after a fluent answer arrives. "
            "You will see the labels written out in the workplace examples. In daily work you may shorten them. Do not shorten them so far that a part disappears. "
            "The card is done when a colleague who did not attend this briefing could follow it and produce a draft you can review."
        ),
    },
    {
        "id": "17",
        "layout": "split",
        "section": "Anatomy",
        "kicker": "The same job, two assignments",
        "title": "A weak prompt hides the missing parts",
        "left_title": "Too thin to steer",
        "left_body": "Write a smart memo about the library hours thing. Use what you know about our board and make us look good.",
        "right_title": "What is missing",
        "right_body": "No reader. No source. Two hidden jobs: invent facts, and advocate. Look good invites spin.",
        "bullets": [],
        "narration": (
            "Here is a prompt that feels normal and works badly. Write a smart memo about the library hours thing. Use what you know about our board and make us look good. "
            "Count the missing parts. There is no role and no reader. There is no source. The task is not a verb you can grade. Smart and look good are requests for costume. "
            "Use what you know about our board invites the model to invent a relationship to your organization. Make us look good invites spin. Spin is not a briefing. "
            "If this prompt were used with real board history pasted underneath, it could also become a disclosure problem. Even with nothing pasted, the reply is likely to manufacture dates, quotes, and a cheerful recommendation. "
            "Do not send this prompt and then hope to edit the damage. Rebuild it. The next slide is the same workplace situation, written so a person can check the result."
        ),
    },
    {
        "id": "18",
        "layout": "prompt",
        "section": "Anatomy",
        "kicker": "The same job, rebuilt",
        "title": "A prompt you can grade",
        "prompt": (
            "Role: Staff assistant writing for the Harbor Town Library Board.\n"
            "Context: Use only this public flyer fact. Harbor Town may trial Saturday hours at Riverside Library, 9 a.m. to 1 p.m., for three months. The board meets the first Tuesday. The flyer estimates about $420 a week for two part-time clerks.\n"
            "Task: Outline a one-page read-ahead.\n"
            "Constraints: Do not invent quotes, costs, or votes. If it is not above, write \"not in source.\" This is not an official position.\n"
            "Output: Headings — Purpose, Decision needed, Facts from the flyer, Options, Questions for the board. End with Gaps. Under 250 words."
        ),
        "bullets": [],
        "narration": (
            "Listen for the five parts in this rebuilt prompt. "
            "Role. You are a staff assistant writing for the Harbor Town Library Board, a fictional public body. "
            "Context. Use only this public flyer fact. Harbor Town may trial Saturday hours at Riverside Library, nine in the morning until one in the afternoon, for three months. The board meets on the first Tuesday. The flyer estimates about four hundred twenty dollars a week for two part-time clerks. "
            "Task. Outline a one-page read-ahead. "
            "Constraints. Do not invent quotations, costs, or votes. If a point is not in the flyer fact, write not in source. The draft is not an official position. "
            "Output. Use five headings: Purpose, Decision needed, Facts from the flyer, Options, and Questions for the board. End with Gaps. Stay under two hundred fifty words. "
            "Harbor Town and Riverside Library are invented for training. The numbers exist only inside the prompt. That is the point. The model is not being asked to know a real town. It is being asked to organize a source you supplied. "
            "You can grade the reply by checking each heading and by confirming that every figure traces to that flyer sentence."
        ),
    },
    {
        "id": "19",
        "layout": "section",
        "section": "Practices",
        "kicker": "Part three",
        "title": "Best practices that survive a busy week",
        "subtitle": "Five habits to keep when the inbox is the pressure",
        "bullets": [
            "One job",
            "A named reader and use",
            "A bounded source",
            "A checkable shape",
            "Tone, limits, and no borrowed authority",
        ],
        "narration": (
            "The five-part card is the method. These next practices are how the method survives a Tuesday afternoon when someone needs a draft before a meeting. "
            "We will stay with five habits. Give the model one job. Name the reader and the use. Bind the source. Demand a shape you can check. Set tone and limits so the draft cannot borrow the authority of the office. "
            "None of these habits requires a special product feature. They are writing habits. "
            "If you adopt only one after this course, adopt source binding. It prevents the failure that is hardest to see and easiest to forward."
        ),
    },
    {
        "id": "20",
        "layout": "bullets",
        "section": "Practices",
        "kicker": "Best practice",
        "title": "One job per prompt",
        "bullets": [
            "Ask for the outline. Stop. Review it.",
            "Then ask for the prose, or the table, or the questions.",
            "A combined prompt hides which job failed.",
            "Short prompts in series beat one crowded prompt.",
        ],
        "narration": (
            "One job per prompt feels slower. It is faster once you count the time you spend untangling a blended draft. "
            "Ask for the outline. Read it. Then ask for the prose of a single section, or for the table, or for the study questions. "
            "When a combined prompt fails, you often cannot tell which instruction was ignored. When a single-job prompt fails, the next message is obvious. "
            "Keep each prompt short enough to reread. A crowded prompt is where officers accidentally paste a second document, including one they did not mean to share. "
            "Series also match how offices already work. You would not ask a colleague for the outline, the final letter, and the hearing questions in one breath and then leave for lunch. "
            "You are allowed to be sequential. The tool will wait."
        ),
    },
    {
        "id": "21",
        "layout": "bullets",
        "section": "Practices",
        "kicker": "Best practice",
        "title": "Name the reader and the use",
        "bullets": [
            "Reader: board, division chief, front counter, or the public.",
            "Use: read-ahead, reply, training item, or personal notes.",
            "Say whether the text may be seen outside the office.",
            "A draft for internal notes must not sound like a decision.",
        ],
        "narration": (
            "Name the reader and the use in the role or the task, every time. "
            "A library board, a division chief, a colleague at the front counter, and a member of the public do not need the same words. "
            "The use matters just as much. A read-ahead can surface open questions. A public reply should not surface internal doubts as if they were policy. A training item can be playful and still accurate. Personal notes can be fragmentary, and they must not read like a decision. "
            "Add a line when the text might travel. This draft is for internal discussion and is not cleared for release outside the office. Or, this draft is written so it could be posted, and it uses only the public source. "
            "That line keeps the model from adding a false voice of authority, and it reminds you which review path the text must take if you decide to keep it."
        ),
    },
    {
        "id": "22",
        "layout": "bullets",
        "section": "Practices",
        "kicker": "Best practice",
        "title": "Bind the source, then make gaps visible",
        "bullets": [
            "Quote or paste the allowed source inside the prompt.",
            "Instruction: use only this source.",
            "Required phrase for holes: not in source.",
            "If the source is too sensitive to paste, do not run the prompt.",
        ],
        "narration": (
            "Source binding is the practice that separates a training exercise from a rumor generator. "
            "Put the allowed text in the prompt, or in an attachment your approved tool isolates for that purpose, and write, use only this source. "
            "Require the phrase not in source wherever a detail is missing. You want the holes labeled, not smoothed over. "
            "Then check. Pick two concrete claims in the reply and find them in the source. If you cannot find them, the draft fails, even if the prose is excellent. "
            "There is a hard companion rule. If the source is too sensitive to paste into the approved tool, you do not have a prompt problem. You have a task you should not run. "
            "Do not solve that by summarizing the sensitive source more cleverly. A careful paraphrase of controlled or personal information is still that information. "
            "Choose a smaller, public, or fully fictional task, or do the work without the model."
        ),
    },
    {
        "id": "23",
        "layout": "bullets",
        "section": "Practices",
        "kicker": "Best practice",
        "title": "Demand a shape you can check quickly",
        "bullets": [
            "Headings, a table, or a numbered list. Pick one.",
            "Cap the length in words or in rows.",
            "Always end with a Gaps line.",
            "If the shape is wrong, correct the shape before editing sentences.",
        ],
        "narration": (
            "A checkable shape is a kindness to your future self, who will be reading this ten minutes before a meeting. "
            "Pick one shape. Headings, a table, or a numbered list. Ask for it by name. "
            "Cap the size. Under two hundred fifty words. No more than six rows. Each bullet one sentence. "
            "Always end with Gaps. On a good day the gaps line says none. On a normal day it tells you what a person still has to look up. "
            "When the reply comes back as a single block of prose and you asked for a table, do not start editing the prose. Send a short correction. Put the answer in the table I specified. Keep every cell tied to the source. "
            "Editing a misshapen draft hides the fact that the prompt was ignored. Correcting the shape teaches you whether the model is following instructions at all."
        ),
    },
    {
        "id": "24",
        "layout": "bullets",
        "section": "Practices",
        "kicker": "Best practice",
        "title": "Set tone, and refuse borrowed authority",
        "bullets": [
            "Plain language for the public. Neutral tone for a briefing.",
            "No slogans, no praise of the agency, no pressure on the reader.",
            "The model does not speak for the office.",
            "Do not ask it to sign, decide, or predict a vote.",
        ],
        "narration": (
            "Tone is a constraint. Set it on purpose. "
            "For the public, ask for plain language and short sentences. For a briefing, ask for neutral wording and no adjectives that sell a side. "
            "Forbid slogans, praise of the agency, and any sentence that pressures the reader to agree. Those lines creep in when a prompt says make us look good or be persuasive. "
            "State the limit of authority in the prompt and respect it in your own use. The model does not speak for the office. Do not ask it to decide, to sign, to apologize on behalf of the agency, or to predict how a board will vote. "
            "You may ask it to list options that are present in the source, with the tradeoff the source actually states. "
            "A recommendation, if you want one at all, should be labeled as a staff option for a human to accept or discard. The official position begins only when a person with the authority adopts the words through your normal process."
        ),
    },
    {
        "id": "25",
        "layout": "section",
        "section": "More",
        "kicker": "Part four",
        "title": "Getting the most from a prompt",
        "subtitle": "Iteration is the job, not a rescue after a bad draft",
        "bullets": [
            "Work in passes",
            "Show a short example",
            "Ask for assumptions and gaps",
            "Critique, then revise",
            "Chain tasks. Do not dump the mission.",
        ],
        "narration": (
            "Getting more from a prompt rarely means writing a longer first message. It means running a short sequence on purpose. "
            "In this part you will practice five moves. Work in passes. Show one short example of the shape you want. Ask the model to list assumptions and gaps before it polishes. Ask it to critique a draft against your constraints, then revise. And chain tasks so each prompt starts from checked text, not from a pile of unfinished hopes. "
            "Officers who say the tool is hit or miss are often sending one prompt and grading the product as if it were final. "
            "Treat the first reply as a proof. The value is in the second and third messages, which are shorter and stricter."
        ),
    },
    {
        "id": "26",
        "layout": "bullets",
        "section": "More",
        "kicker": "Getting more",
        "title": "Work in four passes",
        "bullets": [
            "Pass 1 — structure only. Headings or a table. No prose flourish.",
            "Pass 2 — fill from the source. Mark every hole.",
            "Pass 3 — apply the reader, the length, and the tone.",
            "Pass 4 — you check claims. Only then ask for a clean copy.",
        ],
        "narration": (
            "Use four passes when the product matters enough to leave your desk. "
            "Pass one asks for structure only. Headings or a table. No introduction, no flourish. You are checking whether the assignment was understood. "
            "Pass two fills the structure from the source and marks every hole with not in source. You are checking fidelity. "
            "Pass three applies the reader, the length, and the tone. You are checking fitness for use. "
            "Pass four is yours. You check the concrete claims against the source. Only after that do you ask for a clean copy that incorporates corrections you state in writing. "
            "You can collapse pass one and pass two for a small task, such as a six-line action list. Do not collapse the human check. "
            "Write the pass number at the top of your follow-up message. Pass two. Fill the outline using only the flyer fact. It keeps you, and the model, from drifting back to a vague rewrite."
        ),
    },
    {
        "id": "27",
        "layout": "bullets",
        "section": "More",
        "kicker": "Getting more",
        "title": "Show one short example",
        "bullets": [
            "A pattern teaches faster than an adjective.",
            "Give one row, one bullet, or one question — then say, match this.",
            "Make the example fictional or drawn from the allowed source.",
            "Do not paste a real client's file as the example.",
        ],
        "narration": (
            "Adjectives wear out. Professional, concise, and high quality mean different things in every office. "
            "A short example teaches the pattern. Provide one finished row of a table, or one bullet, or one study question, and say, match this pattern. Do not copy its facts into the other rows. "
            "Here is a pattern for a gap-aware bullet. Fact: the flyer estimates about four hundred twenty dollars a week. Status: in source. "
            "And a second. Fact: the board has already voted. Status: not in source. "
            "The model can imitate that pair more reliably than it can imitate the word thoroughly. "
            "Build the example from the allowed source or from fiction written for the exercise. "
            "Do not paste a real resident's letter, a personnel file, or last year's closed case as the example, even if you plan to delete the name. Names are not the only identifiers."
        ),
    },
    {
        "id": "28",
        "layout": "bullets",
        "section": "More",
        "kicker": "Getting more",
        "title": "Ask for assumptions and gaps before polish",
        "bullets": [
            "First question: what are you assuming that I did not state?",
            "Second: what would change the outline if it were in the source?",
            "Read that list before you ask for smoother sentences.",
            "If an assumption is dangerous or unknowable, stop and rewrite.",
        ],
        "narration": (
            "Polish hides assumptions. Ask for them while the draft is still rough. "
            "A reliable follow-up is: List the assumptions you made that I did not state. Then list facts that would change this outline if they were in the source. Do not add those facts. Label them unknown. "
            "Read that list before you ask for smoother sentences. You will often find the model assumed a vote already happened, assumed a legal authority, or assumed the public supports the trial. "
            "If an assumption is wrong and harmless, correct it in the next prompt with a sentence of allowed fact. "
            "If an assumption is dangerous, or if the missing fact is something you are not allowed to type, stop. Rewrite the task so it can be finished with the source you have, or finish it without the model. "
            "This move is how you get more truth out of the tool. You are not asking it to know more. You are asking it to show where it papered over a hole."
        ),
    },
    {
        "id": "29",
        "layout": "bullets",
        "section": "More",
        "kicker": "Getting more",
        "title": "Critique against the constraints, then revise",
        "bullets": [
            "Paste your own constraint list back to the model.",
            "Ask: which sentences break a constraint, and how?",
            "Then: revise only those sentences.",
            "Keep the critique. It is your review note.",
        ],
        "narration": (
            "You can ask the model to audit its own draft, as long as you treat the audit as a lead and not as clearance. "
            "Paste the constraint list back. Then say: Quote each sentence that breaks a constraint. Name the constraint. Do not revise yet. "
            "Read the critique yourself. Add anything it missed, especially a fact that is not in the source. "
            "Only then say: Revise only the sentences you quoted and the ones I added. Leave the rest. Keep the Gaps line. "
            "Save the critique in your notes for the exercise. It shows a reviewer that you did not accept the first draft, and it becomes evidence of judgment if someone later asks why the text changed. "
            "Never ask, make this perfect, or make this legally sound. Those requests invite a confident stamp the model is not entitled to give. Ask it to compare text to the fences you wrote."
        ),
    },
    {
        "id": "30",
        "layout": "bullets",
        "section": "More",
        "kicker": "Getting more",
        "title": "Chain the work. Do not dump the mission.",
        "bullets": [
            "Each prompt should be able to stand on one page.",
            "Carry forward only the checked text, not the whole chat.",
            "Restate the source rule in every pass.",
            "Start a new chat when the task or the sensitivity changes.",
        ],
        "narration": (
            "A long chat feels powerful and becomes sloppy. The early messages scroll away. A later reply may follow a casual aside you typed twenty minutes ago. "
            "Chain instead of dumping. Each prompt should fit on one page. When you move to the next pass, paste the checked outline or the corrected paragraph, and restate the source rule in one sentence. "
            "Do not rely on the model remembering the fence. Repeat the fence. Use only the flyer fact already given. Do not add new facts. "
            "Start a new conversation when the task changes, and always start a new conversation when the sensitivity might change. Do not continue a practice chat into real work. Do not continue a public-information chat into a personnel question. "
            "If your tool keeps history, assume a later user of that account, or an administrator, can read the chain. That is another reason each message should be able to survive review on its own."
        ),
    },
    {
        "id": "31",
        "layout": "section",
        "section": "Workplace",
        "kicker": "Part five",
        "title": "Government workplace examples",
        "subtitle": "Fictional, public on their face, and written to be checked",
        "bullets": [
            "A board read-ahead",
            "A public inquiry",
            "A comparison table",
            "Notes turned into actions",
            "Study questions from a fact sheet",
        ],
        "narration": (
            "The next examples are patterns for ordinary office work. They are fictional. Harbor Town is not a real jurisdiction in this course, and the figures exist only in the text you are given. "
            "You will see a board read-ahead, a reply to a public question, a comparison of two options, a conversion of notes into actions, and a set of study questions for internal training. "
            "As you hear each one, listen for source binding and for the thing the prompt refuses to do. "
            "None of these prompts asks the model to decide for the board, to invent a law, or to sound like a final letter from the agency. "
            "You can reuse the structure tomorrow with a public source your supervisor has cleared for the tool. You should not reuse the structure as an excuse to paste a live case."
        ),
    },
    {
        "id": "32",
        "layout": "prompt",
        "section": "Workplace",
        "kicker": "Workplace example  ·  Briefing",
        "title": "Outline a read-ahead from a flyer",
        "prompt": (
            "Role: Staff assistant to the Harbor Town Library Board.\n"
            "Task: Outline a read-ahead the board can skim in three minutes.\n"
            "Source: Saturday hours trial, Riverside Library, 9 a.m.–1 p.m., three months. Board meets first Tuesday. Flyer estimate: about $420 a week for two part-time clerks.\n"
            "Constraints: Only this source. No recommendation. No invented public comment.\n"
            "Output: Purpose; Decision needed; Facts; Options stated only if supported; Questions. Then Gaps. Under 250 words."
        ),
        "bullets": [],
        "narration": (
            "The first workplace pattern is the read-ahead you already saw, used here as a model you can copy. "
            "The officer needs a board to skim the issue in three minutes. The prompt asks for an outline, not for a speech and not for a recommendation. "
            "The source is four facts from a public flyer. Saturday hours at Riverside Library, nine to one, a three-month trial, a meeting on the first Tuesday, and a weekly estimate of about four hundred twenty dollars for two part-time clerks. "
            "The constraints refuse invented public comment. Models like to add a sentence that residents have asked for this change. That sentence is exactly the kind of fluent fiction that must not reach a board packet. "
            "Options are allowed only if the source supports them. If the flyer does not list options, the heading should say not in source, or the prompt should tell the model to omit the heading. "
            "Your check takes one minute. Every number matches. No quote appears. The gaps line is present. The draft does not congratulate the board."
        ),
    },
    {
        "id": "33",
        "layout": "prompt",
        "section": "Workplace",
        "kicker": "Workplace example  ·  Public reply",
        "title": "Answer only what a fact sheet supports",
        "prompt": (
            "Role: Correspondence clerk drafting for a supervisor's review.\n"
            "Reader: A resident asking whether Saturday hours are already approved.\n"
            "Source: The board will discuss a three-month trial. No vote is recorded in this source.\n"
            "Task: Draft a short reply.\n"
            "Constraints: Plain language. Do not say the hours are approved. Do not guess the vote. Invite the resident to the public meeting without inventing a room number.\n"
            "Output: Four sentences maximum, then a Gaps line."
        ),
        "bullets": [],
        "narration": (
            "The second pattern is a public reply. A resident asks whether Saturday hours are already approved. "
            "The source says only that the board will discuss a three-month trial, and that no vote is recorded in the source. "
            "The prompt tells the clerk's draft to stay in plain language, to avoid saying the hours are approved, to avoid guessing the vote, and to invite the resident to the public meeting without inventing a room number or an address. "
            "Four sentences is the cap. Then a gaps line. "
            "This is a useful pattern whenever the true answer is narrower than the question. The model should say what is known and stop. "
            "Your review looks for sneaky certainty. Phrases such as the board is expected to approve, or the hours will begin next month, fail the prompt even if they sound helpful. "
            "Helpful and unsupported is still unsupported. The supervisor, not the model, decides whether a warmer sentence is justified by facts outside the tool."
        ),
    },
    {
        "id": "34",
        "layout": "prompt",
        "section": "Workplace",
        "kicker": "Workplace example  ·  Comparison",
        "title": "Compare two options without picking a winner",
        "prompt": (
            "Role: Analyst preparing a neutral table for a division chief.\n"
            "Source: Option A — three-month Saturday trial, flyer cost about $420 a week. Option B — no trial; library stays closed Saturdays; no new weekly cost stated.\n"
            "Task: Compare A and B.\n"
            "Constraints: Do not recommend. Do not add equity, safety, or traffic claims.\n"
            "Output: A table with columns Option, What the source says, Not in source. Then one sentence: Decision owner: the board, not this draft."
        ),
        "bullets": [],
        "narration": (
            "The third pattern is a comparison that refuses to pick a winner. "
            "Option A is the three-month Saturday trial at the flyer cost of about four hundred twenty dollars a week. Option B is no trial. The library stays closed on Saturdays, and the source states no new weekly cost. "
            "The analyst's role is a neutral table for a division chief. The constraints forbid a recommendation, and they forbid imported claims about equity, safety, or traffic. Those topics may matter in real life. They are not in this source, so they do not belong in this draft. "
            "The output is a table with three columns. Option. What the source says. Not in source. "
            "A closing sentence names the decision owner. The board, not this draft. "
            "That sentence is for the human reader as much as for the model. It keeps a tidy table from being mistaken for a decision memo that has already chosen."
        ),
    },
    {
        "id": "35",
        "layout": "prompt",
        "section": "Workplace",
        "kicker": "Workplace example  ·  Meetings",
        "title": "Turn sanitized notes into actions",
        "prompt": (
            "Role: Staff assistant clearing notes for the team.\n"
            "Source: Notes — Discuss Saturday trial. Clerk to confirm the flyer cost with finance. Supervisor to ask counsel whether the meeting notice is already scheduled. No names of the public.\n"
            "Task: Make an action list.\n"
            "Constraints: Do not add owners who are not named. Do not invent due dates.\n"
            "Output: A numbered list: Action, Owner, Due (or not in source). End with Gaps."
        ),
        "bullets": [],
        "narration": (
            "The fourth pattern turns meeting notes into actions. The notes used here are already sanitized. They contain roles, not personal histories. Discuss the Saturday trial. The clerk will confirm the flyer cost with finance. The supervisor will ask counsel whether the meeting notice is already scheduled. No names of members of the public. "
            "The prompt forbids new owners and invented due dates. Models love a deadline. A deadline you did not set is a fact you will have to walk back. "
            "The shape is a numbered list with action, owner, and due date or the phrase not in source. "
            "Bring this pattern back to your office only after the notes themselves are safe to paste. Raw notes often contain asides about a person, a complaint, or a legal risk. Those asides must be removed before the notes ever reach a prompt, and removal means delete, not anonymize with a single initial. "
            "If you cannot make the notes safe, write the action list yourself. It is a short list."
        ),
    },
    {
        "id": "36",
        "layout": "prompt",
        "section": "Workplace",
        "kicker": "Workplace example  ·  Training",
        "title": "Write study questions from a public sheet",
        "prompt": (
            "Role: Training coordinator for new front-desk staff.\n"
            "Source: The same flyer facts, plus: staff may say \"The board will discuss a trial. I cannot predict the vote.\"\n"
            "Task: Write practice questions for a 10-minute huddle.\n"
            "Constraints: Every answer must be in the source. No trick questions. No legal advice.\n"
            "Output: Five questions. After each, the answer in one sentence, then the source phrase it rests on."
        ),
        "bullets": [],
        "narration": (
            "The fifth pattern builds a ten-minute huddle for new front-desk staff. "
            "The source is the flyer facts, plus one approved sentence staff may say. The board will discuss a trial. I cannot predict the vote. "
            "The task is five practice questions. Every answer must be in the source. No trick questions. No legal advice. "
            "The output asks for the answer immediately after each question, plus the source phrase the answer rests on. That extra phrase is what you check. If the phrase is not really in the source, the item fails. "
            "This is a strong use of a model because the stakes of a practice question are real, and the creativity required is small. "
            "It is a poor use if you ask the model to invent policy. Front-desk staff deserve answers a supervisor has confirmed. "
            "In the lab, you will write this kind of prompt yourself, using a fact sheet printed on the worksheet, not a fact sheet from your agency."
        ),
    },
    {
        "id": "37",
        "layout": "bullets",
        "section": "Judgment",
        "kicker": "Before anyone relies on the text",
        "title": "A quality gate with three doors",
        "bullets": [
            "Reject — wrong job, invented fact, or sensitive material in the thread.",
            "Revise — right job, fixable shape, gaps labeled.",
            "Accept as a draft — checks out, and a named person will review it.",
            "Accept never means cleared, signed, or final.",
        ],
        "narration": (
            "Use a quality gate with three doors. Write the door you chose on the worksheet, in one sentence. "
            "Reject when the draft does the wrong job, when it invents a material fact, or when you notice that sensitive information entered the thread. If sensitive information entered, follow your agency's incident steps. Do not try to erase the mistake by asking the model to forget. Assume the text remains in a log. "
            "Revise when the job is right and the problems are fixable: shape, length, tone, or a gap that is labeled but messy. "
            "Accept as a draft only when the claims you checked match the source, the gaps are honest, and a named person will review the text before it is used. "
            "Accept does not mean cleared, signed, filed as the agency position, or ready for the public. Those are later acts, done by people, in the systems your office already uses. "
            "If you cannot explain the door you chose, you are not done reviewing. You are hoping."
        ),
    },
    {
        "id": "38",
        "layout": "alert",
        "section": "Judgment",
        "kicker": "Security, privacy, and records",
        "title": "The draft is not the record until a person makes it one",
        "bullets": [
            "Follow the agency A I use policy and the tool's terms as implemented by your office.",
            "A chat log may be a record. Do not keep the official copy only in the chat.",
            "Copy cleared text into the official system. Note that a tool assisted, if policy says to note it.",
            "You own the words you adopt. The vendor does not.",
        ],
        "callout": "No classified data. No controlled data the tool is not authorized to hold. No personal data.",
        "narration": (
            "Close the loop on security, privacy, and records. "
            "Your agency's artificial intelligence use policy governs, along with the way your office has implemented the tool's terms. This briefing cannot authorize a product, and it cannot waive a marking. "
            "A conversation in a tool may itself be a record under your schedule. Do not treat the chat as the place where the official copy lives. "
            "When a person clears language for use, copy that language into the official system: the correspondence log, the board packet folder, the training repository. If policy says to note that a tool assisted, note it in the way your office prescribes. Do not hide the assistance, and do not overclaim it. "
            "You own the words you adopt. The vendor does not sign your memo. A disclaimer in a prompt does not move responsibility to the software. "
            "Repeat the boundary one last time. No classified information. No controlled information the tool is not authorized to hold. No personal data. "
            "When a colleague treats those rules as fussiness, you may repeat the printer test. If it should not sit on the shared printer, it should not sit in the prompt."
        ),
    },
    {
        "id": "39",
        "layout": "bullets",
        "section": "Labs",
        "kicker": "Facilitated practice",
        "title": "Three exercises after this briefing",
        "bullets": [
            "Lab 1 — Rebuild a thin prompt with R C T C O. 20 minutes.",
            "Lab 2 — Write pass two and pass three for a public reply. 25 minutes.",
            "Lab 3 — Strip a prompt that overshares, then choose a quality-gate door. 25 minutes.",
            "Use the worksheets only. Do not import outside facts.",
        ],
        "narration": (
            "Your facilitator will pause the video here, or pause the live briefing, and move you into the labs. Attendance is checked again before Lab 1. "
            "Lab 1 takes about twenty minutes. You receive a thin prompt about a fictional park-volunteer flyer. You rebuild it with all five parts. You do not need a tool. You need a prompt a colleague could run. "
            "Lab 2 takes about twenty-five minutes. You write pass two and pass three for a public reply, using only the fact sheet on the worksheet. If an approved tool is available, you may run the prompts. If it is not, you will exchange papers and mark invented claims by hand. "
            "Lab 3 takes about twenty-five minutes. You are given a prompt that overshares: a home street, a child's first name, and a snide remark about a coworker. You strip it, you rewrite a safe task, and you choose a quality-gate door for the original thread. The correct first door is reject, because personal data already entered the picture. "
            "Use only the worksheets. Adding a real story from your week fails the course even if the writing is elegant."
        ),
    },
    {
        "id": "40",
        "layout": "close",
        "section": "Close",
        "kicker": "Course GA-PE-210",
        "title": "You assign the work. You answer for it.",
        "bullets": [
            "A prompt is an assignment: role, context, task, constraints, output.",
            "Bind the source. Label the gaps. Check the claims.",
            "The official word begins when a person adopts it.",
            "Completion: attendance plus the three worksheets.",
        ],
        "narration": (
            "You assign the work, and you answer for it. That is the whole course, stripped to one line. "
            "A prompt is an assignment with a role, a context you are allowed to share, one task, constraints that bind the source, and an output you can check. "
            "You get more from the tool by working in passes, by showing a short example, and by asking for gaps before you ask for polish. "
            "You protect the public and your colleagues by keeping classified information, unauthorized controlled information, and personal data out of the thread. "
            "The official word begins only when a person adopts the text through the process your office already trusts. "
            "To complete course 210, stay for the labs and turn in the three worksheets. Your facilitator records attendance and completion. "
            "Thank you for the care you bring to a tool that can write faster than it can be responsible. The responsibility stays with you, which is exactly where the public needs it to stay."
        ),
    },
]

from narration_tight import NARRATION as _TIGHT

_missing = [s["id"] for s in SLIDES if s["id"] not in _TIGHT]
if _missing:
    raise RuntimeError(f"Missing tight narration for {_missing}")
for _slide in SLIDES:
    _slide["narration"] = _TIGHT[_slide["id"]]
