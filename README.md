# FlexCore Showcase

The official interactive demonstration suite and benchmark harness for the **FlexCore** enterprise Blazor UI component library.

Live demo: **[https://flexcoreui.com](https://flexcoreui.com)**  
Component Catalog: **[https://flexcoreui.com/components](https://flexcoreui.com/components)**  
GitHub Repository: **[https://github.com/wadoodachaudhary/FlexCore.Showcase](https://github.com/wadoodachaudhary/FlexCore.Showcase)**

---

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

## The Quick Start download (`quickstart/RazorSharpGetaways`)

The app in the home page's window, Razor Sharp Getaways, is a Blazor Server project of its own in `quickstart/RazorSharpGetaways`,
and the site's **Quick Start** button hands it out as a zip (`/downloads/razor-sharp-getaways.zip`, built on every build of this
project and embedded). The two are one code base: the page components (`Components/Getaways`) and data classes (`Data`) are compiled
into this site straight from that folder (see `FlexCore.Showcase.csproj`), and its `wwwroot/getaways.css` and
`wwwroot/reports/Package Cost Sheet.xml` are copied over this site's copies before each build. So:

- change the window's pages, data or look **in `quickstart/RazorSharpGetaways`**, never in this site's `wwwroot/getaways.css` or
  `wwwroot/reports` copies (a build overwrites them);
- the shared files must compile against the FlexCore version the quickstart pins on NuGet (`RazorSharpGetaways.csproj`), not only
  against the in-tree library;
- `dotnet run` inside `quickstart/RazorSharpGetaways` runs the app on its own; the site's `Components/Pages/Home.razor` only hosts it.
