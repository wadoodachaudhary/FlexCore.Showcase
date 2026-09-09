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
