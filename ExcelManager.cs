using FinTracer.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using Serilog;

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
            {
                Log.Error($"The sheet '{sheet}' does not exist in the Excel file {filePath}.");
                throw new ArgumentException($"The sheet '{sheet}' does not exist in the Excel file {filePath}.");
            }
            return worksheet;
        }

        public static List<string> ReadFirstRow(string filePath, string sheet = "AC ZC YC")
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

        public static ColumnValues GetColumnByHeader(string filePath, string columnName, string sheet = "AC ZC YC")
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
            {
                Log.Error($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");
                throw new ArgumentException($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");
            }

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


        public static ColumnValues GetColumnByHeader2(string filePath, string columnName, string sheet = "AC ZC YC", string currency = "EUR")
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
            {
                Log.Error($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");
                throw new ArgumentException($"Column '{columnName}' not found in the first row of the sheet {sheet} on the file {filePath}.");
            }
                
            if (currencyColumnIndex == -1)
            {
                Log.Error($"Column 'Currency' not found in the first row of the sheet {sheet} on the file {filePath}.");
                throw new ArgumentException($"Column 'Currency' not found in the first row of the sheet {sheet} on the file {filePath}.");
            }

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
            {
                Log.Warning($"The directory at {directoryPath} does not exist.");
            }
               
            var files = Directory.GetFiles(directoryPath, $"*.{fileExtension}", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }

            return fileNames.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }


        public static List<CompareModel> CalculateDeltas(string title, string sourceFile, string targetFile, ColumnValues source, ColumnValues target, IEnumerable<string> maturities, string period)
        {
            var result = new List<CompareModel>();
            
            var delta = new ColumnValues
            {
                Name = $"Delta {source.Name} {target.Name}",
                Data = new object[source.Data.Length]
            };

            for (int j = 0; j < source.Data.Length; j++)
            {
                double value1 = Convert.ToDouble(source.Data[j]);
                double value2 = Convert.ToDouble(target.Data[j]);
                delta.Data[j] = value2 - value1;
            }

            result.Add(new CompareModel
            {
                Title = $"Delta {source.Name} {target.Name}",
                SourceFile = sourceFile,
                TargetFile = targetFile,
                SourceCurve = JsonConvert.SerializeObject(source.Data),
                TargetCurve = JsonConvert.SerializeObject(target.Data),
                Delta = JsonConvert.SerializeObject(delta.Data),
                CreatedBy = "Username",
                CreatedAt = DateTime.Now,
                Comments = "",
                Description = "",
                Maturities = string.Join(",",maturities.ToList()),
                Period = period
            });

            return result;
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



        public static List<ColumnValues> GetColumnsByHeaders(string filePath, string columnNames, string sheet = "AC ZC YC")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var resultData = new List<ColumnValues>();

            if (!File.Exists(filePath))
            {
                Log.Warning($"The file at {filePath} does not exist.");
            }

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


        public static ColumnValues CalculateCurveDifferences(ColumnValues curve1, ColumnValues curve2) {
            var result = new ColumnValues
            {
                Name = $"delta-{curve1.Name}-{curve2.Name}",
                Data = new object[curve1.Data.Length]
            };

            // if (curve1.Data.Length != curve2.Data.Length)
            //     throw new ArgumentException("The two curves must have the same number of data points.");

            double sum = 0;
            var differences = new List<object>();
            try
            {
                for (int i = 0; i < curve1.Data.Length; i++)
                {
                    double value1 = Convert.ToDouble(curve1.Data[i]);
                    double value2 = Convert.ToDouble(curve2.Data[i]);
                    sum += (value2 - value1);
                    differences.Add(value2 - value1);
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            result.Sum = sum;
            result.Data = differences.ToArray();
            return result;
        }


        public static List<ColumnValues> CompareFiles(string excelFile1, string excelFile2, string sheetName = "AC ZC YC")
        {
            var result = new List<ColumnValues>();
            if (!File.Exists(excelFile1) || !File.Exists(excelFile2))
            {
                return result;
            }

            var curvesNames1 = ReadFirstRow(excelFile1, sheetName);
            var curvesNames2 = ReadFirstRow(excelFile2, sheetName);

            var similarities = curvesNames1.Intersect(curvesNames2).ToList();
            
            similarities.Remove(string.Empty);
            similarities.Remove("Currency");
            
            foreach (var similarity in similarities)
            {
                var curves1 = GetColumnsByHeaders(excelFile1, string.Join(",", similarities), sheetName);
                var curves2 = GetColumnsByHeaders(excelFile2, string.Join(",", similarities), sheetName);
                foreach (var curve1 in curves1)
                {
                    foreach (var curve2 in curves2)
                    {
                        if (curve1.Name == curve2.Name)
                        {
                            var delta = CalculateCurveDifferences(curve1, curve2);
                            result.Add(delta);
                        }
                    }
                }
            }

            result = result.OrderByDescending(x => x.Sum).ToList();
            return result;
        }


        public static List<string> GetFileNames(string path, string pattern)
        {
            var fileNames = new List<string>();

            try
            {
                if (!Directory.Exists(path))
                {
                    Log.Warning($"The specified path does not exist: {path}");
                    return fileNames;
                }

                fileNames.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while getting file names: {ex.Message}");
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
            Log.Information("Formula updated successfully!");
        }

        public static double CalculateRMSE(double[] f, double[] g)
        {
            int n = f.Length;
            double sumSquaredErrors = 0;

            for (int i = 0; i < n; i++)
            {
                double difference = f[i] - g[i];
                sumSquaredErrors += Math.Pow(difference, 2);
            }

            return Math.Sqrt(sumSquaredErrors / n);
        }

        public static double CalculateMAD(double[] f, double[] g)
        {
            int n = f.Length;
            double sumAbsoluteDifferences = 0;

            for (int i = 0; i < n; i++)
            {
                sumAbsoluteDifferences += Math.Abs(f[i] - g[i]);
            }
            return sumAbsoluteDifferences / n;
        }


        public static double CalculateAreaDifference(double[] x, double[] f, double[] g)
        {
            int n = x.Length;
            double totalAreaDifference = 0;

            for (int i = 0; i < n - 1; i++)
            {
                // Calculate trapezoid areas for each segment
                double dx = x[i + 1] - x[i];
                double areaF = 0.5 * dx * (f[i] + f[i + 1]);
                double areaG = 0.5 * dx * (g[i] + g[i + 1]);

                // Add absolute area difference
                totalAreaDifference += Math.Abs(areaF - areaG);
            }
            return totalAreaDifference;
        }

        public static List<string> FilterStringsContainingText(List<string> inputList, string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || inputList == null || inputList.Count == 0)
                return new List<string>();
            return inputList.Where(s => s.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<string> CalculateCategories(string curvename) {
            var result = new List<string>();
            if (curvename.Contains("up", StringComparison.OrdinalIgnoreCase)) {
                result.Add("Up Curve");
            } else if (curvename.Contains("down", StringComparison.OrdinalIgnoreCase)) {
                result.Add("Down Curve");
            } else if (curvename.Contains("cra", StringComparison.OrdinalIgnoreCase)) {
                result.Add("CRA Curve");
            } else if (curvename.Contains("dl_", StringComparison.OrdinalIgnoreCase)) {
                result.Add("DL Curve");
            } else if (curvename.Contains("lip", StringComparison.OrdinalIgnoreCase)) {
                result.Add("LIP Curve");
            } else if (curvename.Contains("aegon", StringComparison.OrdinalIgnoreCase)) {
                result.Add("Aegon Curve");
            } else if (curvename.Contains("asr", StringComparison.OrdinalIgnoreCase)) {
                result.Add("ASR Curve");
            } else if (curvename.Contains("ft-", StringComparison.OrdinalIgnoreCase)) {
                result.Add("Flattener Curve");
            } else if (curvename.Contains("st-", StringComparison.OrdinalIgnoreCase)) {
                result.Add("Steepener Curve");
            }
            return result;
        }

    }
}
