using FinTracer.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTracer
{
    public class FinTraceContext: DbContext
    {
        public DbSet<Timeline> Timelines { get; set; } = null!;

        public DbSet<FileTransfer> Filetransfers { get; set; } = null;
        public string DbPath { get; }
        public FinTraceContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "FinTracer.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
