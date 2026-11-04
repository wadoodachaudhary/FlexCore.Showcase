# Razor Sharp Getaways

A small travel agency's back office, written as a Blazor Server app on the [FlexCore](https://flexcoreui.com) controls. It is the
app you can try on the FlexCore home page, as a project of its own: open it, run it, read it, change it.

## Run it

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). Then, in this folder:

```bash
dotnet run
```

The first run restores the FlexCore package from NuGet and prints the address, `https://localhost:7150`. If the browser warns about
the certificate, trust the development certificate once with `dotnet dev-certs https --trust`, or open `http://localhost:5150`.

## The pages

| Page | What it shows | FlexCore controls |
|---|---|---|
| Dashboard | Four figures, trip requests by month, upcoming departures; drag the cards about | GridLayoutControl, CardControl, ChartControl, GridControl, SegmentedControl, CheckBoxControl |
| Trip Requests | 1,000 requests, typed quotes, an in-cell lookup, filter chips, search, a quote editor | GridControl, DropDownGridControl, ChipListControl, SearchBoxControl, ToolbarControl, StatusBarControl, DialogControl, DatePickerControl, NumericTextBoxControl |
| Itinerary | One booked trip, flight out to flight home | SchedulerControl |
| Invoices | The package cost sheet of a booked trip, a Crystal Reports XML export | ReportWriterControl |
| Settings | Colour scheme, text size, grid look, dashboard layout, with a live preview | ColorPaletteControl, SliderControl, SwitchControl, SegmentedControl, DropDownListControl |

The window itself is a vertical TabStripControl for the menu, an AppBarControl with ButtonControl menus and an AvatarControl, and a
FabMenuControl for the + button. Toasts come from NotificationService.

## Where things are

| Path | What is in it |
|---|---|
| `Components/Layout/MainLayout.razor` | The app window: menu, top bar, page area, + button |
| `Components/Pages/*.razor` | One routed page per menu entry; each is a thin wrapper around a page component |
| `Components/Getaways/*.razor` | The page components and the window's parts. The FlexCore site runs these same files |
| `Data/TripRequestStore.cs` | The 1,000 trip requests, generated once per visitor and kept in memory |
| `Data/GetawaysAppSettings.cs` | The Settings page's choices and the dashboard layout |
| `Data/TripPackages.cs`, `Data/PackageCostSheet.cs` | The packages the agency sells and the rows behind the cost sheet report |
| `Data/GetawaysReportServices.cs` | The report writer's data source and session context |
| `wwwroot/reports/Package Cost Sheet.xml` | The report, in the Crystal XML export shape |
| `wwwroot/getaways.css` | The look: colours, the four schemes, the grid theme, the editor |
| `Program.cs` | The FlexCore service registrations |

## How FlexCore is wired in

`RazorSharpGetaways.csproj` references the `FlexCore` package. `Components/_Imports.razor` opens the `Fx.ControlKit` namespaces.
`Program.cs` registers `NotificationService`, `DialogService` and the report writer's services, and `MainLayout.razor` mounts the
two hosts that draw toasts and dialogs, `NotificationDisplayControl` and `DialogHostControl`. The whole app renders interactively
(`App.razor`), which the controls need. The same steps for your own app are on [flexcoreui.com/get-started](https://flexcoreui.com/get-started).

## Changing things

- Your own data: replace the generator in `Data/TripRequestStore.cs`; the pages read only that class.
- Another look: the colours and sizes are in `wwwroot/getaways.css`, under `.gw-app` and the `scheme-*` classes.
- Another page: add a `Components/Pages/Something.razor` with an `@page` directive, and a `GetawaysPage` entry in
  `Components/Getaways/GetawaysPages.cs` so it appears in the menu.
- Your own report: drop a Crystal XML export into `wwwroot/reports`, answer its SQL in `Data/GetawaysReportServices.cs`, and call
  `ShowReportAsync` as `Components/Getaways/GetawaysInvoices.razor` does.

## Licence

The FlexCore package is released under the MIT licence; see its [LICENSE](https://github.com/wadoodachaudhary/FlexCore/blob/main/LICENSE).
This sample is yours to copy and change.
