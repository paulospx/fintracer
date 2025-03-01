using OfficeOpenXml;

namespace FinTracer
{
    public class ExcelManager
    {
        // private static string DefaultSheet = "AC ZC YC";
        public static List<string> ReadFirstRow(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var firstRowValues = new List<string>();
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at {filePath} does not exist.");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                // var worksheet = package.Workbook.Worksheets[DefaultSheet];
                if (worksheet == null)
                    throw new InvalidDataException("The workbook does not contain any worksheets.");

                int totalColumns = worksheet.Dimension.End.Column;

                for (int col = 1; col <= totalColumns; col++)
                {
                    var value = worksheet.Cells[1, col].Text;
                    firstRowValues.Add(value);
                }
            }

            return firstRowValues;
        }


        public static ColumnValues GetColumnByHeader(string filePath, string columnName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var columnValues = new List<string>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at {filePath} does not exist.");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // var worksheet = package.Workbook.Worksheets[DefaultSheet];
                var worksheet = package.Workbook.Worksheets[0];
                if (worksheet == null)
                    throw new ArgumentException($"The sheet '{0}' does not exist in the Excel file.");

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
                    throw new ArgumentException($"Column '{columnName}' not found in the first row of the sheet.");

                int totalRows = worksheet.Dimension.End.Row;
                for (int row = 2; row <= totalRows; row++) // Start from row 2 to skip the header
                {
                    columnValues.Add(worksheet.Cells[row, targetColumnIndex].Text);
                }
            }

            // Prepare the result object
            var result = new ColumnValues
            {
                Name = columnName,
                Data = columnValues.ToArray()
            };

            // Return the result as a JSON string
            return result;
        }


        public static List<string> GetFilesWithExtension(string directoryPath, string fileExtension)
        {
            var fileNames = new List<string>();

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory at {directoryPath} does not exist.");

            // Get all files with the specified extension (case-insensitive)
            var files = Directory.GetFiles(directoryPath, $"*.{fileExtension}", SearchOption.TopDirectoryOnly);

            // Extract only file names from full paths
            foreach (var file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }

            return fileNames.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }


        public static List<ColumnValues> GetColumnsByHeaders(string filePath, string columnNames)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var resultData = new List<ColumnValues>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at {filePath} does not exist.");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // var worksheet = package.Workbook.Worksheets[DefaultSheet];
                var worksheet = package.Workbook.Worksheets[0];
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

                // Check if any column name was not found
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
    }
}
