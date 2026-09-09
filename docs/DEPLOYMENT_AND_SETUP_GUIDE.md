# FlexCore & FlexCore.Showcase: Local Setup, Development & Azure Deployment Guide

This document is the complete end-to-end reference for setting up, developing, testing, and deploying **FlexCore** and **FlexCore.Showcase** on a local development machine (macOS, Windows, or Linux) and publishing updates to the production web application at **[https://flexcoreui.com](https://flexcoreui.com)**.

---

## 1. Architectural Overview

The FlexCore solution is composed of two primary repositories:

1. **`FlexCore`**: The core component library (Razor Class Library / `.dll`).
   - **Characteristics**: 100% pure C# and SVG, zero third-party JavaScript dependencies.
   - **Key Features**: High-throughput virtualized Data Grid (sub-millisecond scrolling, 60 FPS), TreeGrid, Pivot Table, COVID-19 Sunburst Chart, SVG Charts gallery, PDF Viewer, Report Writer, Dialogs, and 70+ desktop-grade UI controls.
   - **GitHub**: [https://github.com/wadoodachaudhary/FlexCore](https://github.com/wadoodachaudhary/FlexCore)

2. **`FlexCore.Showcase`**: The interactive Blazor Server Web Application.
   - **Characteristics**: Targets .NET 10 (`net10.0`), provides real-time benchmarks (e.g. 50K/100K row virtualization, AG Grid 4-in-1 suite, in-grid fast typing, ClosedXML Excel exports, Syncfusion-style component catalog).
   - **Project Dependency**: References `FlexCore` directly via local project references:
     ```xml
     <ProjectReference Include="..\FlexCore\FlexCore.csproj" />
     <ProjectReference Include="..\FlexCore\FlexCore.Documents\FlexCore.Documents.csproj" />
     ```
   - **GitHub**: [https://github.com/wadoodachaudhary/FlexCore.Showcase](https://github.com/wadoodachaudhary/FlexCore.Showcase)
   - **Production Endpoints**:
     - **Primary Custom Domain**: [https://flexcoreui.com](https://flexcoreui.com) (mapped to Azure Web App `flexcore`)
     - **Direct Azure Hostname**: [https://flexcore.azurewebsites.net](https://flexcore.azurewebsites.net)
     - **Showcase Staging Hostname**: [https://flexcore-showcase.azurewebsites.net](https://flexcore-showcase.azurewebsites.net)

---

## 2. Directory Structure Requirement (Side-by-Side)

Because `FlexCore.Showcase.csproj` references `..\FlexCore\FlexCore.csproj` and `..\FlexCore\FlexCore.Documents\FlexCore.Documents.csproj`, both repositories **must be cloned as sibling folders inside the same parent directory**.

```text
your-projects-root/             <-- Any directory (e.g., ~/projects or C:\projects)
├── FlexCore/                   <-- Cloned from wadoodachaudhary/FlexCore
│   ├── Charts/
│   ├── Grid/
│   ├── Reports/
│   ├── tests/
│   ├── FlexCore.Documents/     <-- Document viewer / PDF / Spreadsheet project
│   │   └── FlexCore.Documents.csproj
│   ├── FlexCore.csproj
│   └── ...
└── FlexCore.Showcase/          <-- Cloned from wadoodachaudhary/FlexCore.Showcase
    ├── Components/
    │   ├── Layout/
    │   ├── Pages/
    │   └── Shared/
    ├── wwwroot/
    ├── FlexCore.Showcase.csproj
    ├── deploy-to-azure.sh
    ├── deploy-to-azure.ps1
    └── ...
```

---

## 3. Prerequisites & Environment Setup

Install the following tools on the new machine:

### 3.1 .NET 10 SDK
Verify that .NET 10 SDK is installed:
```bash
dotnet --version
# Expected: 10.0.x
```
Download installer: [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0)

### 3.2 Git & GitHub CLI
Install Git and GitHub CLI:
- **macOS**: `brew install git gh`
- **Windows**: `winget install Git.Git GitHub.cli`
- **Linux**: `sudo apt-get install git gh`

Authenticate with GitHub:
```bash
gh auth login
```
*(Select `GitHub.com`, `HTTPS`, and authenticate with your browser or personal access token. Ensure your GitHub account has collaborator access to `wadoodachaudhary/FlexCore` and `wadoodachaudhary/FlexCore.Showcase`.)*

### 3.3 Azure CLI
Install the Azure CLI:
- **macOS**: `brew install azure-cli`
- **Windows**: `winget install Microsoft.AzureCLI`
- **Linux**: `curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash`

Verify installation:
```bash
az version
```

---

## 4. Initial Setup & Cloning

Choose a directory to contain both projects and clone them side-by-side:

```bash
# 1. Create and enter your workspace root
mkdir -p ~/projects
cd ~/projects

# 2. Clone FlexCore
git clone https://github.com/wadoodachaudhary/FlexCore.git

# 3. Clone FlexCore.Showcase
git clone https://github.com/wadoodachaudhary/FlexCore.Showcase.git
```

### 4.1 Git Buffer Configuration (Recommended)
`FlexCore` contains vector assets, WASM fonts, and standalone PDF decoders. Increase Git's HTTP post buffer to avoid HTTP 400 errors during large pushes:

```bash
cd ~/projects/FlexCore
git config http.postBuffer 524288000
git config http.version HTTP/1.1

cd ~/projects/FlexCore.Showcase
git config http.postBuffer 524288000
git config http.version HTTP/1.1
```

---

## 5. Local Development & Testing Workflow

### 5.1 Step 1: Build FlexCore
From `~/projects/FlexCore`:
```bash
cd ~/projects/FlexCore
dotnet restore
dotnet build FlexCore.csproj
```
Verify that the build succeeds with `0 Warning(s), 0 Error(s)`.

### 5.2 Step 2: Run Regression Checks
FlexCore includes a dedicated regression suite verifying 270+ checks (AG Grid parity, windowing, date filters, tree hierarchy, formula evaluation, time pickers, and chart math):
```bash
dotnet run --project tests/FlexCore.RegressionTests/FlexCore.RegressionTests.csproj
```
Ensure all regression checks pass before committing changes.

### 5.3 Step 3: Run FlexCore.Showcase Locally
From `~/projects/FlexCore.Showcase`:
```bash
cd ~/projects/FlexCore.Showcase
dotnet run
```
Output will display the active listening URLs:
```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5002
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7011
```
Open [http://localhost:5002](http://localhost:5002) in your browser:
- **Homepage (`/`)**: Displays the interactive Data Grid with 1,000 rows, fast in-grid typing, DropDownGrid, sparkline charts, and ClosedXML export, followed by the Popular UI Components and Component Directory.
- **Component Catalog (`/components`)**: Displays the complete 13-category directory with real-time reactive search filtering and miniature vector icons (`MiniIcon`).
- **AG Grid 4-in-1 Suite (`/demo/grid/ag-grid-showcase`)**: 100K performance benchmark, live financial tickers, HR directory tree, and inventory master-detail.
- **COVID-19 Sunburst (`/demo/charts/sunburst`)**: Interactive drill-down Sunburst chart with country search and continent filters.

### 5.4 Hot Reload & Live Editing
You can make edits to components in `FlexCore` or pages in `FlexCore.Showcase`. Use `dotnet watch`:
```bash
cd ~/projects/FlexCore.Showcase
dotnet watch
```
Any modifications to `.razor`, `.razor.cs`, or `.css` files will hot-reload automatically in your browser.

---

## 6. Guidelines for Updating Components & Catalog

When adding a new control or modifying an existing showcase demo:

1. **Shared Single Source of Truth**:
   The component catalog rendered on both the Home page (`/`) and the All Components page (`/components`) is defined in:
   - [`FlexCore.Showcase/Components/Shared/ComponentCatalogDirectory.razor`](../Components/Shared/ComponentCatalogDirectory.razor)
   - [`FlexCore.Showcase/Components/Shared/ComponentCatalogDirectory.razor.css`](../Components/Shared/ComponentCatalogDirectory.razor.css)

2. **Miniature Vector Icons ($\le 16 \times 16$)**:
   Each component in the catalog displays a lightweight, crisp vector icon:
   - `<MiniIcon Name="grid" />`
   - If adding a new icon, register its SVG path inside [`FlexCore.Showcase/Components/Shared/MiniIcon.razor`](../Components/Shared/MiniIcon.razor).

3. **Guaranteed Routing Parity**:
   When registering or moving a component demo, update its entry in `ComponentCatalogDirectory.razor` so that both the homepage directory and `/components` navigate to the same destination.

---

## 7. Connecting to Azure & Deploying

### 7.1 Azure Infrastructure Configuration
Both targets are hosted in Microsoft Azure App Service:
- **Resource Group**: `ghostwriter-prod-rg`
- **App Service Plan**: `ghostwriter-prod-plan` (East US 2, Linux/Windows container runtime)
- **Target App Services**:
  1. `flexcore` — Serves production traffic for **[https://flexcoreui.com](https://flexcoreui.com)** (and `https://flexcore.azurewebsites.net`).
  2. `flexcore-showcase` — Serves showcase traffic for **[https://flexcore-showcase.azurewebsites.net](https://flexcore-showcase.azurewebsites.net)**.

### 7.2 Authenticate with Azure CLI
Run the following command once to sign in to your Azure account:
```bash
az login
```
A browser window will open. Sign in using the Azure account with permissions to the `ghostwriter-prod-rg` resource group (e.g. `wadoodchaudhary@hotmail.com` or your invited organizational ID).

Verify your subscription and target web apps:
```bash
# Check logged-in account
az account show --output table

# Verify the web apps are reachable
az webapp list --resource-group ghostwriter-prod-rg --output table
```
You should see `flexcore` and `flexcore-showcase` listed with state `Running`.

---

### 7.3 Automated Deployment (Single Command)

#### On macOS or Linux:
Run the included deployment script:
```bash
cd ~/projects/FlexCore.Showcase
chmod +x deploy-to-azure.sh
./deploy-to-azure.sh
```

#### On Windows (PowerShell):
Run the PowerShell deployment script:
```powershell
cd C:\projects\FlexCore.Showcase
.\deploy-to-azure.ps1
```

---

### 7.4 What the Deployment Script Does Under the Hood

If you ever need to deploy manually or understand the exact sequence, the script executes 3 steps:

1. **Clean Release Build & Publish**:
   ```bash
   rm -rf bin/Release/net10.0/publish
   dotnet publish FlexCore.Showcase.csproj -c Release -o bin/Release/net10.0/publish
   ```

2. **Package Zip Artifact**:
   ```bash
   (cd bin/Release/net10.0/publish && zip -qr /tmp/flexcore-site.zip .)
   ```

3. **Deploy to Azure via OneDeploy API**:
   ```bash
   # Deploy to primary webapp (flexcoreui.com)
   az webapp deploy \
       --resource-group ghostwriter-prod-rg \
       --name flexcore \
       --src-path /tmp/flexcore-site.zip \
       --type zip \
       --clean true \
       --restart true

   # Deploy to showcase webapp
   az webapp deploy \
       --resource-group ghostwriter-prod-rg \
       --name flexcore-showcase \
       --src-path /tmp/flexcore-site.zip \
       --type zip \
       --clean true \
       --restart true
   ```

---

### 7.5 Verifying the Live Deployment

Once the script outputs `All deployments completed successfully!`, verify HTTP 200 responses:

```bash
# Check primary custom domain
curl -s -o /dev/null -w "flexcoreui.com: %{http_code}\n" https://flexcoreui.com

# Check component catalog route
curl -s -o /dev/null -w "flexcoreui.com/components: %{http_code}\n" https://flexcoreui.com/components

# Check direct azure domain
curl -s -o /dev/null -w "flexcore-showcase: %{http_code}\n" https://flexcore-showcase.azurewebsites.net
```
All checks will return `HTTP 200`.

---

## 8. Synchronizing Git Repositories

After verifying your changes locally and in Azure, push your code back to GitHub:

### 8.1 Push FlexCore
```bash
cd ~/projects/FlexCore
git add -A
git commit -m "feat: <describe your component changes>"
git push origin main
```

### 8.2 Push FlexCore.Showcase
```bash
cd ~/projects/FlexCore.Showcase
git add -A
git commit -m "feat: <describe your showcase changes>"
git push origin main
```

---

## 9. Troubleshooting & FAQ

| Symptom | Probable Cause | Resolution |
| :--- | :--- | :--- |
| `Cannot find project ../FlexCore/FlexCore.csproj` | Folders were not cloned side-by-side | Ensure `FlexCore` and `FlexCore.Showcase` are siblings in the same folder. |
| `error: RPC failed; HTTP 400 curl 22` on git push | Default Git buffer is smaller than bundled WASM/PDF assets | Run `git config http.postBuffer 524288000 && git config http.version HTTP/1.1`. |
| `ResourceGroupNotFound` or Azure permission error | Logged into wrong Azure tenant/subscription | Run `az login` and `az account set --subscription "<Subscription Name>"`. |
| `HEAD` request to Azure returns `405 Method Not Allowed` | Blazor Server default endpoints disallow `HEAD` requests | Use standard `GET` requests (e.g. `curl -sL` or test in browser). |
| Component missing or mismatched between `/` and `/components` | Outdated individual page markup | Ensure changes are added to `ComponentCatalogDirectory.razor`, which powers both views. |

---

## 10. Anti-Gravity Collaboration Tips

When collaborating using Google Anti-Gravity:
1. **Workspace Setup**: Open `FlexCore.Showcase` as the primary active workspace. The assistant can view and edit `../FlexCore` via relative paths seamlessly.
2. **Background Commands**: Long-running commands (such as `./deploy-to-azure.sh` or extensive builds) can run in the background. Anti-Gravity will automatically resume when the task completes.
3. **Tests First**: Instruct Anti-Gravity to run `dotnet run --project ../FlexCore/tests/FlexCore.RegressionTests/FlexCore.RegressionTests.csproj` before any pull request or deployment.
