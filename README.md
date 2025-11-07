# Studentska Služba (WPF + C#)

This is my desktop application for student service administration. I built it as a classic WPF app with a structured shell, custom message box UX, localization hooks, and a set of converters that make the XAML binding layer both expressive and safe. The solution also contains a CLI project for command‑line interactions (batch tasks, smoke checks), sitting alongside the GUI application.

The goal: a fast, native Windows client that demonstrates solid WPF patterns (resource dictionaries, localization indirection, value converters, centralized message dialogs), with clean separation between XAML and code‑behind where it makes sense.

---

## Tech Stack

- Runtime: .NET (C#), WPF (Windows Presentation Foundation)
- UI: XAML + code‑behind (MainWindow shell, resource dictionaries, localization indirection)
- Utilities:
  - Custom MessageBox manager (consistent titles, buttons, and localization)
  - Value converters (null/visibility, boolean/visibility, formatting, etc.)
- Solution: Visual Studio solution (`StudentskaSluzba.sln`) with
  - GUI (WPF app) → `GUI/`
  - CLI (console app) → `CLI/`
- Packaging: WPF application via `GUI.csproj`

---

## Solution Layout

```
StudentskaSluzba.sln
├─ GUI/                      # WPF application
│  ├─ App.xaml               # Application resources + StartupUri
│  ├─ App.xaml.cs            # Startup logic, global event plumbing
│  ├─ GUI.csproj             # WPF project file
│  ├─ MainWindow.xaml        # Main application shell (menus, content regions, resources)
│  ├─ MainWindow.xaml.cs     # Shell event handlers, startup wiring, runtime UX rules
│  ├─ MessageBoxManager.cs   # Centralized dialog API (OK/Cancel/Yes/No with localized text)
│  ├─ Converters.cs          # Common WPF IValueConverters bound in XAML
│  ├─ L10N.cs                # Lightweight localization helper (resource lookup)
│  ├─ AssemblyInfo.cs        # Assembly metadata
│  ├─ Converters/            # (Optional) per‑converter files, if split out later
│  ├─ Resources/             # Resource dictionaries (styles, brushes, templates), resx files, images
│  └─ View/                  # Additional views/user controls loaded into the shell
└─ CLI/                      # Console utilities for headless ops (batch import, tests, etc.)
```

---

## What I Implemented

- WPF Shell
  - MainWindow as the central UI container with menus/toolbars, content regions, and resource scoping
  - Resource dictionaries for styles and shared UI assets (in `Resources/`)
  - Centralized event handling in `MainWindow.xaml.cs` for window lifecycle, menu commands, and message prompts
- Dialogs
  - `MessageBoxManager` as a single entry point for showing message boxes
  - Consistent default titles, standardized button sets (OK/Cancel/Yes/No) across the app
  - Localized button captions (e.g., “Da/Ne”, “U redu/Otkaži”) by indirection through resource lookups
  - Optional default icon selection based on severity (Info, Warning, Error)
- Localization (L10N)
  - `L10N` helper for retrieving localized strings at runtime (wrapping a ResourceManager or equivalent)
  - XAML bindings to dynamic resources enable swapping language dictionaries
  - String keys centralized in `Resources` for text reuse
- Converters
  - A single `Converters.cs` file collecting common `IValueConverter` utilities I use repeatedly in XAML:
    - BooleanToVisibility (and collapsed variant)
    - NullToVisibility (and inverted variants)
    - StringNullOrEmptyToVisibility
    - Equality / InEquality checks to booleans or visibility
    - Numeric formatting / string formatting helpers
  - Converters declared in App or Page resources and used declaratively in XAML bindings
- App Startup
  - `App.xaml` wires top‑level resources (styles, converter instances) and sets `StartupUri` to `MainWindow.xaml`
  - `App.xaml.cs` handles application‑level exceptions (if needed) and config bootstrap
- Views
  - `View/` contains specialized pages or user controls that the shell hosts (students, courses, professors, schedules, etc., as the domain evolves)
  - Data bindings, converters, and localized captions applied consistently
- CLI
  - Companion console project (in `CLI/`) used for headless checks or batch tasks (e.g., quick data integrity runs, offline utilities)

---

## Build & Run

1. Open `StudentskaSluzba.sln` in Visual Studio.
2. Set `GUI` as the startup project.
3. Build and Run. The main shell (`MainWindow`) bootstraps with the application resources defined in `App.xaml`.

For console utilities:
- Set `CLI` as startup project and run from Visual Studio, or build and invoke from a console.

---

## Code Walkthrough (Key Files)

### App.xaml (Application Resources + Startup)

- Declares application‑scoped resources:
  - Converter instances (keyed for XAML usage)
  - Styles/brushes/templates merged from `Resources/`
  - Localization resource dictionaries
- Sets `StartupUri` → `MainWindow.xaml`, ensuring the shell loads with resources ready.

Typical patterns:
```xml
<Application ... StartupUri="MainWindow.xaml">
  <Application.Resources>
    <!-- Resource dictionaries, converter instances -->
  </Application.Resources>
</Application>
```

### App.xaml.cs (Process Initialization)

- Application entry event hooks (`OnStartup`, `OnExit`)
- Global exception handling path (if desired) to present a friendly message via `MessageBoxManager`
- Environment initialization (load culture, prepare localization provider)

### MainWindow.xaml (Shell)

- Defines the top‑level layout:
  - Menu/Toolbar region with commands (File/Edit/View/Help)
  - Left/Right panels (e.g., navigation tree, detail forms)
  - Central content area (Frame/ContentPresenter) for hosting views from `View/`
  - Status bar for app state and messages
- Uses bindings and converters for enabling/disabling menu items based on state
- Pulls strings via `StaticResource`/`DynamicResource` keys for localization

Example binding uses:
```xml
<MenuItem Header="{DynamicResource Menu_File}">
  <MenuItem Header="{DynamicResource Menu_File_Open}"
            IsEnabled="{Binding CanOpen, Converter={StaticResource BoolToVisibility?}}">
  </MenuItem>
</MenuItem>
```

### MainWindow.xaml.cs (Shell Behavior)

- Wires `Click` handlers for menu items that aren’t command‑bound
- Coordinates navigation: sets `Content` to appropriate views created under `View/`
- Uses `MessageBoxManager` for any confirmations:
  - On “Exit” → confirm changes
  - On “Delete” → ask Yes/No
- Applies runtime language switches by reloading resource dictionaries via `L10N`

Typical flow:
```csharp
// Pseudocode style (not the exact code):
private void MenuExit_Click(object sender, RoutedEventArgs e)
{
    if (MessageBoxManager.Confirm(Resources["ConfirmExit"]) == true)
    {
        Close();
    }
}
```

### MessageBoxManager.cs (Consistent Dialogs)

- Central static API:
  - `Info(string message, string? title = null)`
  - `Warn(string message, string? title = null)`
  - `Error(string message, string? title = null)`
  - `Confirm(string message, string? title = null)` → returns bool?
- Localizes button text (e.g., Yes/No → Da/Ne) by wrapping the standard `MessageBox` or by using a custom dialog
- Ensures consistent icons for severity and a default owner window for modality

Example intention:
```csharp
public static bool Confirm(string msg, string? title = null)
{
    var caption = title ?? L10N.Text("Dialog_Confirm_Title");
    var buttons = MessageBoxButton.YesNo;
    var result  = MessageBox.Show(msg, caption, buttons, MessageBoxImage.Question);
    return result == MessageBoxResult.Yes;
}
```

### L10N.cs (Localization Helper)

- A small helper to retrieve localized text by key:
  - `public static string Text(string key) => ResourceManager.GetString(key) ?? key;`
- Indirection point used throughout the app to avoid hardcoding strings
- Works with resource dictionaries or `.resx` files under `Resources/`

### Converters.cs (WPF Binding Utilities)

- Collected `IValueConverter` implementations I often rely on:
  - `BoolToVisibilityConverter` (and `InverseBoolToVisibilityConverter`)
  - `NullToVisibilityConverter` (and inverse)
  - `StringNullOrEmptyToVisibilityConverter`
  - `EqualsConverter` / `NotEqualsConverter` (compare binding value to parameter)
  - `FormatConverter` (wraps `string.Format` or culture‑sensitive formatting)
- Typical XAML usage:
```xml
<TextBlock Visibility="{Binding SelectedItem, Converter={StaticResource NullToVisibility}}" />
```

### Resources/ (Theming & Strings)

- Resource dictionaries for:
  - Palette (brushes, colors)
  - Typography (fonts, sizes)
  - Control styles/templates for consistent look
- String dictionaries (by culture) for UI captions, menu text, dialog titles
- Image assets (icons) for menus/toolbars/status bar

### View/

- Dedicated pages or user controls that the shell hosts:
  - Example: StudentsView, ProfessorsView, CoursesView (domain‑specific pages)
  - Bindings + converters for visibility/enabling logic
  - Uses `L10N` resource keys for labels and tooltips

---

## UX Conventions

- No ad‑hoc `MessageBox.Show` scattered everywhere—everything goes through `MessageBoxManager` for consistency and localization
- Localized captions for menus/toolbars configured in XAML via resource keys
- Visibility/Enablement toggled via converters and boolean properties (cleaner in XAML than imperative code)
- Keyboard navigation and focus handling at the shell level (MainWindow) for a predictable feel

---

## CLI (Companion Console App)

- Lives in `CLI/` as a separate project within the solution
- Useful for:
  - Basic domain validations
  - Batch transformations or exports
  - Quick smoke tests independent of the WPF shell
- Shares domain logic via a common library or repeated minimal code (depending on how far I modularize)

---

## How I Run and Debug

- Debug start: WPF `GUI` project
- Watch the status bar and the log window (if present) for quick signal of operations
- Use localized resource keys for all visible strings to keep future translations straightforward
- Use the CLI project to validate new resource keys, run batch edits, or verify data files without spinning up the UI

---

## Notes on Extensibility

- The shell was designed to host additional views under `View/`—binding to the shell content presenter is easy
- New converters can be dropped into `Converters.cs` or split into the `GUI/Converters/` folder and then registered into `App.xaml` resources
- New dialog types can be added into `MessageBoxManager` if a specialized modal UX is needed (e.g., input dialogs)

---

## Why WPF Here

- Native Windows look and speed, strong binding system, and resource theming
- Fine‑grained control over UX without external dependencies
- Easy to keep the code approachable for education/demos (student service domain) while retaining professional UX touches

---

## Build Targets

- Build with Visual Studio (any recent version that supports WPF)
- AnyCPU/x86/x64 depending on deployment target
- Single‑user desktop runtime; no server dependency required

---

## Summary

This codebase is a compact but complete WPF application: localized UI text, consistent dialogs, a main window shell that hosts feature views, and a set of converters that keep XAML concise and robust. The CLI sibling project rounds it out for fast headless tasks. The structure is intentionally simple to read and extend while showing practical WPF patterns that I reach for in real desktop apps.
