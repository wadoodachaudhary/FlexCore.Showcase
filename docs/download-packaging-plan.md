# FlexCore Showcase — Downloadable Package & Distribution Specification
*Future Memory & Backlog Item*

## 1. Overview & Objective
This specification defines the packaging, distribution, and consumption architecture for making the **FlexCore Showcase** and its Syncfusion-style live benchmarks downloadable for external developers and evaluators.

Developers will be able to download, unpack, open in .NET (`dotnet run` or Visual Studio / Rider / VS Code), and immediately execute the benchmarks on their local machines.

---

## 2. Package Variants

### Package Option A: Full Self-Contained Showcase Package (`flexcore-showcase-full.zip`)
**Target Audience**: Developers evaluating FlexCore who want an immediate out-of-the-box running demo solution with full source code access to both the Showcase and the core control library.

#### Contents:
1. **FlexCore Library Source**:
   - `/FlexCore` (Grid, TreeGrid, TreeView, Charts, Layout, Reports, Counterparts).
   - Excludes binary outputs (`bin/`, `obj/`, `.vs/`).
2. **FlexCore Showcase Web Application**:
   - `/FlexCore.Showcase` (.NET 10 interactive server Blazor application).
   - Reusable `SyncfusionDemoLayout` component, CSS shells, and copy-to-clipboard scripts.
   - All 15+ live benchmark pages under `/Components/Pages/Demos/`.
3. **Data Assets**:
   - `/FlexCore.Showcase/wwwroot/data` or embedded `BenchDataStore.cs` with dual-mode data loading (loads disk JSON if available or auto-synthesizes realistic test records up to 100k+ rows).
4. **Solution & Config**:
   - `FlexCore.Showcase.sln` linking `FlexCore.csproj` and `FlexCore.Showcase.csproj`.
   - `.editorconfig`, `global.json` (optional target .NET 10/9/8 multi-target).
5. **Strict Exclusions**:
   - **MUST EXCLUDE**: Large AI/LLM modules (e.g., `FlexKitLLM`, Python virtual environments, model weights).
   - **MUST EXCLUDE**: `.git/`, `.github/`, transient cache folders.
   - **Target Zip Size**: < 15 MB.

#### Run Command:
```bash
unzip flexcore-showcase-full.zip -d flexcore-showcase
cd flexcore-showcase
dotnet run --project FlexCore.Showcase
```

---

### Package Option B: Minimal Standalone Showcase Snippets (`flexcore-showcase-minimal.zip`)
**Target Audience**: Developers who already have FlexCore installed via NuGet (`dotnet add package FlexCore`) and want to add the benchmark and documentation pages directly into their existing Blazor application.

#### Contents:
1. **Razor Components & Pages**:
   - `Components/Shared/SyncfusionDemoLayout.razor`
   - `Components/Pages/Demos/Grid/*.razor` (all 12 grid benches)
   - `Components/Pages/Demos/Tree/*.razor` (TreeGrid and TreeView benches)
   - `Components/Pages/Demos/Workspace/*.razor` (PageControl bench)
2. **C# Code & Services**:
   - `Data/BenchModels.cs` (LedgerRow, BenchTreeNode, SRow, EstimatingItemRecord, etc.)
   - `Services/BenchDataStore.cs` (Self-contained in-memory data generator)
3. **Styles & Assets**:
   - `wwwroot/css/syncfusion-demo-layout.css` (Clean, self-contained CSS styles for the demo shell, copy button, parameter table, and dark code snippets)
4. **Documentation & Integration Guide**:
   - `README.md` with step-by-step instructions on registering `BenchDataStore` and importing CSS.
5. **Strict Exclusions**:
   - Does NOT include the full FlexCore engine source.
   - Does NOT include any external backend databases.
   - **Target Zip Size**: < 200 KB.

#### Integration Steps:
```bash
# In your existing Blazor project:
dotnet add package FlexCore
# Copy Components/, Data/, and Services/ folders into your project
# Register service in Program.cs:
builder.Services.AddSingleton<BenchDataStore>();
```

---

## 3. Automated Packaging Script (`scripts/package-showcase.sh`)

A production packaging script will be maintained in the repository to automate generation of both distribution archives:

```bash
#!/usr/bin/env bash
set -euo pipefail

OUT_DIR="./dist"
mkdir -p "$OUT_DIR"

echo "=== Building Full Package ==="
git archive --format=tar.gz --prefix=flexcore-showcase-full/ HEAD \
    FlexCore/ FlexCore.Showcase/ FlexCore.Showcase.sln \
    ':!FlexKitLLM' ':!**/bin' ':!**/obj' \
    -o "$OUT_DIR/flexcore-showcase-full.tar.gz"

echo "=== Building Minimal Snippets Package ==="
zip -r "$OUT_DIR/flexcore-showcase-minimal.zip" \
    FlexCore.Showcase/Components/Shared/SyncfusionDemoLayout.razor \
    FlexCore.Showcase/Components/Pages/Demos/ \
    FlexCore.Showcase/Data/BenchModels.cs \
    FlexCore.Showcase/Services/BenchDataStore.cs \
    FlexCore.Showcase/docs/download-packaging-plan.md

echo "=== Packaging Complete! ==="
ls -lh "$OUT_DIR"
```

---

## 4. Quality & Compliance Checklist
- [x] All demo pages compile with 0 warnings and 0 errors under .NET 10.
- [x] In-memory fallbacks ensure demos run immediately without requiring local database installations or external web API keys.
- [x] Syntax-highlighted code tabs (`Index.razor` and C# models) match running controls with 1-click clipboard copying.
- [x] Strict exclusion of submodules (`FlexKitLLM`) guarantees fast download speeds and zero IP leakage.
- [x] Responsive layout functions seamlessly across desktop, tablet, and mobile browsers.
