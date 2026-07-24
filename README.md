# AlphaX.Sheets - High-Performance WPF Spreadsheet & Calculation Engine

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/kartikdeepsagar/AlphaX.WPF.Sheets)
[![Platform](https://img.shields.io/badge/platform-WPF-blue.svg)](https://dotnet.microsoft.com/)
[![Target Framework](https://img.shields.io/badge/.NET-4.7.2%20%7C%20Standard%202.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

**AlphaX.Sheets** is a modular, high-performance, Excel-like spreadsheet component for WPF applications. It combines a platform-agnostic core spreadsheet data engine, a multi-sheet calculation engine (`AlphaX.CalcEngine`), and a modern WPF view control (`AlphaXSpread`) featuring an Excel-inspired Material 3 aesthetic.

![AlphaX Spread Explorer Preview](docs/alphax_spread_preview.jpg)

---

## ✨ Features at a Glance

- 🚀 **High Performance Grid**: Virtualized rendering supporting smooth navigation and virtual scrolling across 50,000+ data rows.
- 🧮 **Multi-Sheet Calculation Engine**: Cross-worksheet formula dependencies with real-time recalculation engine powered by `AlphaX.CalcEngine`.
- 🎨 **Materialist & Modern Theme**: Excel Green (`#107C41`) accent styling, light-slate surface palette, customizable gridlines, headers, and row striping.
- 📊 **Two-Way Data Binding**: Native binding to C# POCO collections (`List<T>`) and ADO.NET `DataTable` objects.
- 🔘 **Rich Custom Cell Renderers**: Built-in renderers for Checkbox, Button, ComboBox, Hyperlink, and Text cells.
- 🔃 **Range Sorting Engine**: Multi-column ascending and descending sorting algorithms.
- 📜 **Configurable Scroll Modes**: Support for **Item**, **Pixel**, and **Deferred** scroll modes.

---

## 🏗️ Architecture & Project Structure

The project is architected with strict separation of concerns into independent modular assemblies:

```
src/
├── AlphaX.Sheets/              # Core platform-agnostic spreadsheet data engine
├── AlphaX.CalcEngine/          # Expression parser & multi-sheet formula engine
├── AlphaX.WPF.Sheets/          # WPF UI view control (AlphaXSpread) & cell renderers
└── Samples/                    # Modern Samples Explorer application
```

### Core Libraries
- **AlphaX.Sheets**: Platform-agnostic core library providing workbook, worksheet, cell data store, row/column headers, and event structures.
- **AlphaX.CalcEngine**: Independent formula evaluation engine providing expression parsing, dependency tracking, and formula evaluation across interlinked worksheets.

### WPF Control
- **AlphaX.WPF.Sheets**: WPF control library containing `AlphaXSpread`, `AlphaXFormulaTextBox`, interaction layers, selection management, and WPF drawing renderers.

### Showcase Application
- **AlphaXSpreadSamplesExplorer**: Interactive showcase application featuring categorized live demonstrations, real-time theme customization, search filtering, and dataset benchmarks.

---

## 🚀 Getting Started

### 1. Adding `AlphaXSpread` to XAML

```xaml
<Window x:Class="SpreadDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:sheets="http://schemas.gcspreadsheet.com/2022/wpf"
        Title="AlphaX Spreadsheet Demo" Height="600" Width="900">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <!-- Formula Bar -->
        <sheets:AlphaXFormulaTextBox Margin="8" Spread="{Binding ElementName=spreadControl}"/>

        <!-- Main Spreadsheet Control -->
        <sheets:AlphaXSpread x:Name="spreadControl" Grid.Row="1"/>
    </Grid>
</Window>
```

### 2. Data Binding Example

```csharp
using AlphaX.Sheets.Data;

// Bind a List<Customer> to the active worksheet
var customers = GetCustomerList();
var worksheet = spreadControl.WorkBook.WorkSheets.GetSheet(0);

worksheet.DataSource = customers;
worksheet.Columns[0].DataMap = new PropertyDataMap("Id");
worksheet.Columns[1].DataMap = new PropertyDataMap("FirstName");
worksheet.Columns[2].DataMap = new PropertyDataMap("LastName");
worksheet.Columns[3].DataMap = new PropertyDataMap("Email");
```

---

## 💻 Samples Explorer

Run `AlphaXSpreadSamplesExplorer.csproj` to explore interactive feature demonstrations:

- **Formula Bar & Editor**: Real-time formula editing linked to spreadsheet cell selection.
- **Multi-Sheet Formulas**: Cross-sheet formula evaluation with real-time dependency recalculations.
- **Data Binding**: Compare POCO `List<T>` vs. ADO.NET `DataTable` two-way bindings.
- **Grid Styling & Themes**: Live theme switcher (Slate, Excel Classic Green, Emerald, Indigo, Corporate) and 4-quadrant grid showcase.
- **Scroll Modes**: Benchmark performance under 50,000+ data rows with Item, Pixel, and Deferred scroll modes.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open issues, submit pull requests, or propose new spreadsheet features and calculation engine capabilities.

## 📄 License

This project is licensed under the [MIT License](LICENSE).
