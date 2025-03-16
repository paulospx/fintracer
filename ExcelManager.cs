using OfficeOpenXml;


namespace FinTracer
{
    public class ExcelManager
    {
        private static ExcelWorksheet GetWorksheet(string filePath, string sheet)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at {filePath} does not exist.");

            var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[sheet];
            if (worksheet == null)
                throw new ArgumentException($"The sheet '{sheet}' does not exist in the Excel file {filePath}.");

            return worksheet;
        }

        public static List<string> ReadFirstRow(string filePath, string sheet = "AZ ZC YC")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var firstRowValues = new List<string>();
            var worksheet = GetWorksheet(filePath, sheet);

            int totalColumns = worksheet.Dimension.End.Column;
            for (int col = 1; col <= totalColumns; col++)
            {
                var value = worksheet.Cells[1, col].Text;
                firstRowValues.Add(value);
            }

            return firstRowValues;
        }

        public static ColumnValues GetColumnByHeader(string filePath, string columnName, string sheet = "AZ ZC YC")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var columnValues = new List<string>();
            var worksheet = GetWorksheet(filePath, sheet);

            int totalColumns = worksheet.Dimension.End.Column;
            int targetColumnIndex = -1;

            for (int col = 1; col <= totalColumns; col++)
            {
                if (worksheet.Cells[1, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    targetColumnIndex = col;
                    break;
                }
            }

            if (targetColumnIndex == -1)
                throw new ArgumentException($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");

            int totalRows = worksheet.Dimension.End.Row;
            for (int row = 2; row <= totalRows; row++)
            {
                columnValues.Add(worksheet.Cells[row, targetColumnIndex].Text);
            }

            return new ColumnValues
            {
                Name = columnName,
                Data = columnValues.ToArray()
            };
        }


        public static ColumnValues GetColumnByHeader2(string filePath, string columnName, string sheet = "AZ ZC YC", string currency = "EUR")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var columnValues = new List<string>();
            var worksheet = GetWorksheet(filePath, sheet);

            int totalColumns = worksheet.Dimension.End.Column;
            int targetColumnIndex = -1;
            int currencyColumnIndex = -1;

            for (int col = 1; col <= totalColumns; col++)
            {
                if (worksheet.Cells[1, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    targetColumnIndex = col;
                }
                if (worksheet.Cells[1, col].Text.Equals("Currency", StringComparison.OrdinalIgnoreCase))
                {
                    currencyColumnIndex = col;
                }
            }

            if (targetColumnIndex == -1)
                throw new ArgumentException($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");

            if (currencyColumnIndex == -1)
                throw new ArgumentException($"Column 'Currency' not found in the first row of the sheet {sheet} on the file {filePath}.");

            int totalRows = worksheet.Dimension.End.Row;
            for (int row = 2; row <= totalRows; row++)
            {
                if (worksheet.Cells[row, currencyColumnIndex].Text.Equals(currency, StringComparison.OrdinalIgnoreCase))
                {
                    columnValues.Add(worksheet.Cells[row, targetColumnIndex].Text);
                }
            }

            return new ColumnValues
            {
                Name = columnName,
                Data = columnValues.ToArray()
            };
        }


        public static List<string> GetFilesWithExtension(string directoryPath, string fileExtension)
        {
            var fileNames = new List<string>();

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory at {directoryPath} does not exist.");

            var files = Directory.GetFiles(directoryPath, $"*.{fileExtension}", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }

            return fileNames.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }


        public static List<ColumnValues> CalculateDeltas(List<ColumnValues> curves)
        {
            var deltas = new List<ColumnValues>();

            for (int i = 0; i < curves.Count - 1; i++)
            {
                var delta = new ColumnValues
                {
                    Name = $"Delta {i + 1}",
                    Data = new object[curves[i].Data.Length]
                };

                for (int j = 0; j < curves[i].Data.Length; j++)
                {
                    double value1 = Convert.ToDouble(curves[i].Data[j]);
                    double value2 = Convert.ToDouble(curves[i + 1].Data[j]);
                    delta.Data[j] = value2 - value1;
                }

                deltas.Add(delta);
            }
            return deltas;
        }


        public static List<ColumnValues> GenerateRandomCurves()
        {
            var random = new Random();
            var curves = new List<ColumnValues>();

            for (int i = 0; i < 5; i++)
            {
                var curve = new ColumnValues
                {
                    Name = $"Curve {i + 1}",
                    Data = new object[100]
                };

                double rate = random.NextDouble() * 0.1; // Different rate for each curve
                for (int j = 0; j < 100; j++)
                {
                    curve.Data[j] = Math.Sqrt(rate * j); // Exponential growth with different rates
                }

                curves.Add(curve);
            }

            return curves;
        }



        public static List<ColumnValues> GetColumnsByHeaders(string filePath, string columnNames, string sheet = "AZ ZC YC")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var resultData = new List<ColumnValues>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at {filePath} does not exist.");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheet];
                var columnNamesArray = columnNames.Split(',').Where(str => !string.IsNullOrEmpty(str)).ToList();

                var columnsData = new Dictionary<string, List<object>>();

                foreach (var colName in columnNamesArray)
                {
                    columnsData[colName.Trim()] = new List<object>();
                }

                int totalColumns = worksheet.Dimension.End.Column;
                var columnIndexes = new Dictionary<string, int>();

                for (int col = 1; col <= totalColumns; col++)
                {
                    var header = worksheet.Cells[1, col].Text;
                    foreach (var colName in columnNamesArray)
                    {
                        if (header.Equals(colName.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            columnIndexes[colName.Trim()] = col;
                        }
                    }
                }

                foreach (var colName in columnNamesArray)
                {
                    if (!columnIndexes.ContainsKey(colName.Trim()))
                        throw new ArgumentException($"Column '{colName.Trim()}' not found in the first row of the sheet.");
                }

                int totalRows = worksheet.Dimension.End.Row;
                for (int row = 2; row <= totalRows; row++) // Start from row 2 to skip the header
                {
                    foreach (var colName in columnNamesArray)
                    {
                        int columnIndex = columnIndexes[colName.Trim()];
                        string cellValue = worksheet.Cells[row, columnIndex].Text;

                        if (float.TryParse(cellValue, out float floatValue))
                        {
                            columnsData[colName.Trim()].Add(floatValue);
                        }
                        else
                        {
                            columnsData[colName.Trim()].Add(cellValue);
                        }
                    }
                }

                foreach (var colName in columnNamesArray)
                {
                    resultData.Add(new ColumnValues
                    {
                        Name = colName.Trim(),
                        Data = columnsData[colName.Trim()].ToArray()
                    });
                }
            }
            return resultData;
        }


        public static List<string> GetFileNames(string path, string pattern)
        {
            List<string> fileNames = new List<string>();

            try
            {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("The specified path does not exist.");
                    return fileNames;
                }

                fileNames.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return fileNames;
        }


        public static void UpdateFormula(string filePath, string cell = "A1", string formula = "SUM(B1:B10)")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[cell].Formula = "SUM(B1:B10)";
                worksheet.Calculate();
                package.Save();
            }
            Console.WriteLine("Formula updated successfully!");
        }
    }
}
