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
                    return filetransfer;
                }

                // Capture source file metadata
                var sourceInfo = new FileInfo(filetransfer.SourceFile);
                filetransfer.SourceCreatedAt = sourceInfo.CreationTime;
                filetransfer.SourceSize = sourceInfo.Length;
                filetransfer.SourceMd5 = await CalculateMd5Async(filetransfer.SourceFile);

                // Copy the file
                File.Copy(filetransfer.SourceFile, filetransfer.TargetFile, overwrite: true);

                // Capture target file metadata
                var targetInfo = new FileInfo(filetransfer.TargetFile);
                filetransfer.TargetCreatedAt = targetInfo.CreationTime;
                filetransfer.TargetSize = targetInfo.Length;
                filetransfer.TargetMd5 = await CalculateMd5Async(filetransfer.TargetFile);

                // Update last copied time
                filetransfer.LastCopied = DateTime.Now;

                // Log the copy operation
                // LogCopyOperation(file);

                Console.WriteLine($"File copied successfully from {filetransfer.SourceFile} to {filetransfer.TargetFile}");
                filetransfer.Status = "Transferred";
                return filetransfer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying file: {ex.Message}");
                filetransfer.Status = $"Error";
                return filetransfer;
            }
        }
    }
}
