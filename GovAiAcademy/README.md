# Gov AI Academy

Instructor-led AI classes for U.S. public-sector officers, from non-technical staff through technical evaluators. This is a separate Blazor Server app inside the FlexCore.Showcase repository. It does not replace the Showcase demo and it does not deploy to flexcoreui.com.

The running sample is **not** an official government website. People, offices, and class packets are fictional.

## What you can do in the sample

- Read the mission, the security posture, and who the classes are for.
- Filter the catalog by level, role path, delivery, and approval.
- Open a course for outcomes, prerequisites, labs, the cohort schedule, and the attendance policy.
- Sign in as a sample learner, instructor, or administrator (development only).
- See enrollments, meetings, and an exercise checklist. Check off an exercise.
- On the instructor desk, mark attendance in a FlexCore grid and add a meeting.

Approved courses are live (online, in person, or hybrid), applied to the officer’s own tools and role, and above the baseline in the [U.S. Department of Labor AI Literacy Framework](https://www.dol.gov/agencies/eta/advisories/ten-07-25) (TEN 07-25). Completion is instructor-recorded attendance plus the required exercises. A self-paced recording is listed only as an example of what is **not** approved.

## Layout next to FlexCore

Showcase already references FlexCore as a sibling folder. The Academy project sits one level down, so its reference goes up twice:

```text
your-projects-root/
├── FlexCore/                          FlexCore.csproj and FlexCore.Documents/
└── FlexCore.Showcase/                 this repository
    ├── FlexCore.Showcase.csproj       references ../FlexCore
    ├── GovAiAcademy/
    │   └── GovAiAcademy.csproj        references ../../FlexCore
    └── GovAiAcademy.Tests/
```

```bash
mkdir -p ~/projects && cd ~/projects
git clone https://github.com/wadoodachaudhary/FlexCore.git
git clone https://github.com/wadoodachaudhary/FlexCore.Showcase.git
```

## Run locally

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
cd ~/projects/FlexCore.Showcase
dotnet test GovAiAcademy.Tests/GovAiAcademy.Tests.csproj
dotnet run --project GovAiAcademy/GovAiAcademy.csproj --launch-profile http
```

Open [http://localhost:5002](http://localhost:5002) for Showcase (unchanged) and [http://localhost:5080](http://localhost:5080) for the Academy. The two apps use different ports.

HTTPS profile (trust the local dev cert first with `dotnet dev-certs https --trust`):

```bash
dotnet run --project GovAiAcademy/GovAiAcademy.csproj --launch-profile https
```

That listens on `https://localhost:7080` and `http://localhost:5080`.

### Sample accounts

Development sign-in has no password. These accounts exist only when `Authentication:Mode` is `DevelopmentMock`.

| Account | Role | Use it for |
| --- | --- | --- |
| Jordan Hale | Learner | Two enrollments, a partial exercise checklist |
| Amira Solano | Instructor | Roster and attendance |
| Casey Okonkwo | Administrator | Same desk as the instructor |

`appsettings.Production.json` sets the mode to `EntraId`, which hides the sample accounts.

## Security

- No secrets are committed. Tenant id and client id are empty strings. Do not add a client secret to any json file.
- Outside Development the app sends HSTS, redirects to HTTPS, and adds nosniff, frame denial, and a content security policy. `script-src` still allows inline and eval because Blazor Server’s circuit host needs them. Tighten that only after a production circuit test.
- The lab editor stores text in memory. Do not type controlled unclassified information, personal data, non-public procurement files, or live security logs into the demo.
- Attendance is in memory. Restarting the process clears marks made during the session. A production host should replace `AcademyStore` with a database.

## Entra ID (not wired in this build)

The shell is ready for a government tenant. The `Microsoft.Identity.Web` package is **not** referenced, so the app builds and runs without a tenant. When the agency is ready:

1. Confirm the cloud with the tenant administrator. Azure Government uses `https://login.microsoftonline.us/`. Commercial tenants use `https://login.microsoftonline.com/`. Do not guess.
2. Register a single-tenant web application.
   - Redirect URI: `https://<academy-host>/signin-oidc`
   - Signed-out callback: `https://<academy-host>/signout-callback-oidc`
3. Create app roles `Learner`, `Instructor`, and `Administrator`.
4. Put the client secret or certificate in Key Vault. Surface it as an App Service setting named `Authentication__Entra__ClientSecret`. Never commit it.
5. Set `Authentication__Entra__TenantId`, `Authentication__Entra__ClientId`, and `Authentication__Entra__Instance`.
6. Add the package and middleware:

```bash
dotnet add GovAiAcademy/GovAiAcademy.csproj package Microsoft.Identity.Web
```

```csharp
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("Authentication:Entra"));
builder.Services.AddAuthorization();
// after UseForwardedHeaders, before the Blazor endpoints:
app.UseAuthentication();
app.UseAuthorization();
```

Map the Entra app-role claim onto `AcademyRole`. Delete the development persona list for that environment. Delegated Graph permissions to start: `openid`, `profile`, `email`, `User.Read`.

## Azure deploy (separate app)

Scripts refuse `flexcore`, `flexcore-showcase`, and the resource group `ghostwriter-prod-rg`.

Permissions you need, on a **new** resource group only:

- Permission to create a resource group and an App Service plan (Contributor on that group is enough).
- A user who can create the Entra app registration, or an admin who will hand you the client id.
- Key Vault access later, for the client secret. Not required to publish the sample.

```bash
az login
cd ~/projects/FlexCore.Showcase

# optional: pick a globally unique name
export APP_NAME="gov-ai-academy-<your-suffix>"
export RESOURCE_GROUP="gov-ai-academy-rg"
export LOCATION="eastus2"

chmod +x GovAiAcademy/deploy/*.sh
./GovAiAcademy/deploy/provision-azure.sh
./GovAiAcademy/deploy/deploy-to-azure.sh
```

Windows:

```powershell
$env:APP_NAME = "gov-ai-academy-<your-suffix>"
$env:RESOURCE_GROUP = "gov-ai-academy-rg"
.\GovAiAcademy\deploy\provision-azure.ps1
.\GovAiAcademy\deploy\deploy-to-azure.ps1
```

Preview the template without deploying:

```bash
az deployment group create \
  --resource-group gov-ai-academy-rg \
  --template-file GovAiAcademy/deploy/main.bicep \
  --parameters appName="$APP_NAME" \
  --what-if
```

The Bicep template creates a Linux App Service plan and a site with HTTPS only, TLS 1.2, FTP disabled, always on, a health check at `/health`, and client affinity. Blazor Server keeps each circuit on one instance. Affinity matters until you add a SignalR backplane. The template does not create a private endpoint or a Key Vault.

If `DOTNETCORE|10.0` is not offered in the subscription yet, set the .NET 10 stack in the portal after the site exists, then run the zip deploy again.

Check:

```bash
curl -sS "https://${APP_NAME}.azurewebsites.net/health"
```

## What is left for a live agency deployment

- Entra app registration, role assignments, and a Key Vault secret.
- A database behind `AcademyStore` (attendance must survive restarts and more than one instance).
- Azure SignalR Service if you turn off client affinity or run more than one worker.
- Private networking and the agency’s logging and retention rules.
- A counsel and security review before any real case file is used in a lab. The seeded packets are fictional on purpose.

## Project map

| Path | Role |
| --- | --- |
| `GovAiAcademy/` | Blazor Server UI. FlexCore controls only. |
| `GovAiAcademy/Data/AcademySeed.cs` | Original courses, cohorts, and attendance. |
| `GovAiAcademy/Services/PedagogyRules.cs` | Approval gate. |
| `GovAiAcademy/deploy/` | Bicep plus bash and PowerShell deploy scripts. |
| `GovAiAcademy.Tests/` | Catalog rules and attendance behavior. |

Showcase pages, `deploy-to-azure.sh` at the repository root, and the flexcoreui.com targets are unchanged.
