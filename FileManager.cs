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
    }
}
