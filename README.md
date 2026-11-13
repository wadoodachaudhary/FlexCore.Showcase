# FlexCore Showcase

The official interactive demonstration suite and benchmark harness for the **FlexCore** enterprise Blazor UI component library.

Live demo: **[https://flexcoreui.com](https://flexcoreui.com)**  
Component Catalog: **[https://flexcoreui.com/components](https://flexcoreui.com/components)**  
GitHub Repository: **[https://github.com/wadoodachaudhary/FlexCore.Showcase](https://github.com/wadoodachaudhary/FlexCore.Showcase)**

---

## AI Atlas Report Studio

Open `/demo/reports/ai-atlas` (also available at `/demo/reports/crystal-bench`).
The studio includes a searchable report library, file browser, RPT/XML upload,
native C# RPT conversion, positioned viewer, visual designer and data explorer.
It requires no Java, IKVM or SAP Crystal runtime.

The bundled [AI Atlas pack](wwwroot/reports/ai-atlas/README.md) contains six
editable report stories and a read-only SQLite database with 12 related tables.
Historical sources are linked; model, adoption, infrastructure and economic
metrics are explicitly illustrative. No proprietary RPT samples are published.
The app does not create or migrate its database at startup.

Uploaded business reports start in **Layout only** mode until their fields are
mapped to the sample database. Conversion does not consult validation XML.
Edits and uploads live in a private temporary workspace; download report XML to
retain an edited definition. Original bundled reports are never overwritten.

Regression tools in the sibling `JavaToCSharp` workspace:

```sh
dotnet run --project tools/ReportAtlas.Tests
node tools/ReportAtlas.Tests/browser.cjs
```

The browser check targets a locally running Showcase on port 5088. The seed
tool and sample-pack README document how to rebuild the public database and
report definitions offline.

## Quick Start for Developers

For complete instructions on cloning, developing locally, running regression tests, connecting to Azure, and deploying updates to [https://flexcoreui.com](https://flexcoreui.com), read the complete guide:

📘 **[Full Deployment & Local Setup Guide](docs/DEPLOYMENT_AND_SETUP_GUIDE.md)**

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git & [GitHub CLI](https://cli.github.com/)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)

### Side-by-Side Clone
```bash
mkdir -p ~/projects && cd ~/projects
git clone https://github.com/wadoodachaudhary/FlexCore.git
git clone https://github.com/wadoodachaudhary/FlexCore.Showcase.git
```

### Run Locally
```bash
cd ~/projects/FlexCore.Showcase
dotnet run
```
Open [http://localhost:5002](http://localhost:5002) in your browser.

### Deploy to Azure (Production)
```bash
# macOS / Linux:
./deploy-to-azure.sh

# Windows (PowerShell):
.\deploy-to-azure.ps1
```
