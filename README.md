# AlphaX.Sheets - A WPF Excel-like Component

AlphaX.Sheets is a modular, Excel-like component built for WPF, designed to be reusable, flexible, and extendable. The project consists of distinct libraries, ensuring separation of concerns and ease of maintenance. It is currently a Minimum Viable Product (MVP) with essential features like sorting, data binding, styling, and support for multiple cell types.

This project started as a personal exploration to learn how to build reusable components and has grown into a robust framework. It is now open source, inviting contributors to improve and expand it over time.

## Project Structure

### Core Libraries
- **AlphaX.Sheets**: A core library providing Excel-like APIs. This library is platform-agnostic and serves as the foundation for building spreadsheet components.
- **AlphaX.CalcEngine**: A calculation engine library integrated into the core library to provide formula support. Like the core library, it is not tied to WPF and can be used across different platforms.

### WPF-Specific Libraries
- **AlphaX.WPF.Sheets**: A WPF-specific user control library built on top of AlphaX.Sheets. It provides an Excel-like component tailored for WPF applications.

### Sample Application
- **AlphaXSamplesExplorer**: A WPF application showcasing the usage of the AlphaX.WPF.Sheets component, referred to as **AlphaXSpread**. It includes interactive examples to demonstrate the component’s capabilities and features.

## Features
- Sorting
- Data Binding
- Styling
- Multiple Cell Types

## Design Philosophy
- **Modular and Reusable**: The component is designed as a set of independent libraries to ensure flexibility and reusability across different platforms.
- **Extendable**: Developers can easily extend the functionality of the component to fit their requirements.
- **Open Source**: The project is open to contributions from the community to make it better and more feature-rich over time.

## Contribution
This project is open source, and contributions are highly encouraged! Whether you want to enhance the existing features, fix bugs, or add new capabilities, feel free to contribute and help grow this project.

## Acknowledgements
This project was initiated as a learning experiment to understand the fundamentals of building reusable, generic components. The journey has been rewarding, overcoming various challenges along the way. Now, it is an open-source initiative with the potential to become a comprehensive Excel-like component for WPF.

Let's make this project big together!
