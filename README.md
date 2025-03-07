# FinTracer

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()

## Overview

**FinTracer** is a C# application designed to streamline the comparison and validation of financial curve data across multiple software versions or datasets. By offering robust regression analysis, the tool ensures consistency, accuracy, and reliability in financial modeling and forecasting processes. 

The project provides:

- **Version Comparison**: Analyze and compare financial curves from different versions to identify deviations or discrepancies.
- **Automated Validation**: Automatically validate curve data to ensure compliance with business and regulatory standards.
- **Trend Analysis**: Detect and highlight significant changes or trends in curve patterns.
- **Detailed Reporting**: Generate intuitive, visual, and data-rich reports for easy interpretation of results.
- **User-Friendly Interface**: Simplified workflows and customizable settings for ease of use by financial analysts and software developers.

### Use Cases:

- Ensuring consistent performance of financial algorithms in software updates.
- Auditing and verifying data integrity in financial curve datasets.
- Supporting decision-making by providing clear insights into curve changes over time.

This tool aims to become an indispensable asset for financial institutions, software developers, and analysts who require precise regression testing in dynamic and data-intensive environments.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- [Visual Studio 2019 or later](https://visualstudio.microsoft.com/)
- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Any additional dependencies or tools]

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/paulospx/fintracer.git
   ```

2. Navigate to the project directory:
   ```bash
   cd fintracer
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Build the project:
   ```bash
   dotnet build
   ```

## Usage

1. Run the application:
   ```bash
   dotnet run
   ```

2. [Provide instructions on how to use your project, e.g., command-line arguments, configuration steps, or examples.]

## Contributing

Contributions are welcome! Follow these steps to contribute:

1. Fork the repository.
2. Create a new branch:
   ```bash
   git checkout -b feature-name
   ```
3. Make your changes and commit them:
   ```bash
   git commit -m "Add feature name"
   ```
4. Push to the branch:
   ```bash
   git push origin feature-name
   ```
5. Open a pull request.

## Database 

Database generation
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For more information, please contact:

- [GitHub Repository](https://github.com/paulospx/fintracer)

## Acknowledgements

- [Library/Tool 1, e.g., EPPlus for Excel Handling](https://github.com/JanKallman/EPPlus)
- [Library/Tool 2, e.g., Newtonsoft.Json for JSON Serialization](https://www.newtonsoft.com/json)
- [Any other resources or inspirations]
