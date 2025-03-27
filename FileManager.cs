using FinTracer.Models;
using System.Security.Cryptography;

namespace FinTracer
{
    public static class FileManager
    {
        public static async Task<string> CalculateMd5Async(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = await Task.Run(() => md5.ComputeHash(stream));
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        public static async Task<FileTransfer> CopyFileAsync(FileTransfer filetransfer)
        {
            try
            {
                if (!File.Exists(filetransfer.SourceFile))
                {
                    Console.WriteLine($"Source file not found: {filetransfer.SourceFile}");
                    filetransfer.Status = "Waiting";
                    return filetransfer;
                }

                // Capture source file metadata
                var sourceInfo = new FileInfo(filetransfer.SourceFile);
                filetransfer.SourceCreatedAt = sourceInfo.CreationTime;
                filetransfer.SourceSize = sourceInfo.Length;
                filetransfer.Status = "Validation";
                filetransfer.SourceMd5 = await CalculateMd5Async(filetransfer.SourceFile);

                // Copy the file
                filetransfer.Status = "Waiting";
                File.Copy(filetransfer.SourceFile, filetransfer.TargetFile, overwrite: true);

                // Capture target file metadata
                var targetInfo = new FileInfo(filetransfer.TargetFile);
                filetransfer.TargetCreatedAt = targetInfo.CreationTime;
                filetransfer.TargetSize = targetInfo.Length;
                filetransfer.TargetMd5 = await CalculateMd5Async(filetransfer.TargetFile);

                filetransfer.Status = "MD5 Check";
                if (filetransfer.SourceMd5 == filetransfer.TargetMd5)
                {
                    filetransfer.Status = "Transfer Successful";
                }

                // Update last copied time
                filetransfer.LastCopied = DateTime.Now;

                // Log the copy operation
                // LogCopyOperation(file);

                Console.WriteLine($"File copied successfully from {filetransfer.SourceFile} to {filetransfer.TargetFile}");
                filetransfer.Status = "Transfer Successful";
                return filetransfer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying file: {ex.Message}");
                filetransfer.Status = $"Transfer Error";
                return filetransfer;
            }
        }

        public static void HandleFileCopy(FileCopyTask task)
        {
            try
            {
                if (File.Exists(task.SourceFile))
                {
                    File.Copy(task.SourceFile, task.TargetFile, true);
                    task.CopiedAt = DateTime.Now;
                    task.Warning = null; // Clear warning on success
                    Console.WriteLine($"File copied successfully from {task.SourceFile} to {task.TargetFile} at {task.CopiedAt}");
                }
                else
                {
                    string message = $"File not found: {task.SourceFile}. We will retry every hour until the end of the day. \n" +
                                     $"Stakeholders: {task.Stakeholders}, please ensure the file is available.";
                    task.Warning = message;
                    Console.WriteLine(message);

                    // Retry logic: Retry every hour
                    DateTime retryEnd = DateTime.Today.AddDays(1).AddTicks(-1); // End of the day
                    for (DateTime retryTime = DateTime.Now.AddHours(1); retryTime <= retryEnd; retryTime = retryTime.AddHours(1))
                    {
                        Thread.Sleep(3600000); // Wait for an hour
                        if (File.Exists(task.SourceFile))
                        {
                            File.Copy(task.SourceFile, task.TargetFile, true);
                            task.CopiedAt = DateTime.Now;
                            task.Warning = null; // Clear warning on success
                            Console.WriteLine($"File copied successfully from {task.SourceFile} to {task.TargetFile} at {task.CopiedAt}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying file: {ex.Message}");
            }
        }

        public static DateTime GetNextScheduledTime(FileCopyTask task)
        {
            return task.Period switch
            {
                "Daily" => task.ScheduledAt.AddDays(1),
                "Weekly" => task.ScheduledAt.AddDays(7),
                "Monthly" => task.ScheduledAt.AddMonths(1),
                "Quarterly" => task.ScheduledAt.AddMonths(3),
                _ => task.ScheduledAt
            };
        }
    }
}
