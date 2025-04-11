using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System.Text.RegularExpressions;
using Lucene.Net.QueryParsers.Classic;

namespace FinTracer
{
    public class PathInfo
    {
        public required string Filename { get; set; }
        public required string FullPath { get; set; }
        public required string Extension { get; set; }
        public int Size { get; set; }
    }

    public class SearchManager
    {
        const string FILE_SEARCH_INDEX = "files-search-index";
        const string BASE_PATH = "C:\\temp\\"; 

        private static PathInfo ParseFilePaths(string input)
        {
            var result = new PathInfo { 
                Filename = string.Empty,
                Extension = string.Empty,
                FullPath = string.Empty,
                Size = 0
            };

            string pattern = @"^.*[\\](?<filename>[^\\]+?)(?:\.(?<extension>[^.]+))?$";
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            Match match = Regex.Match(input, pattern, options);
            result.FullPath = input;
            if (match.Success)
            {
                result.Filename = match.Groups["filename"].Value;
                result.Extension = match.Groups["extension"].Value;
            }
            return result; 
        }

        public static void IndexFile(string path)
        {
            CreateIndex(File.ReadAllLines(path));
        }


        public static void CreateIndex(string[] lines)
        {
            const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

            var indexPath = Path.Combine(BASE_PATH, FILE_SEARCH_INDEX);

            using var dir = FSDirectory.Open(indexPath);

            // Create an analyzer to process the text
            var analyzer = new StandardAnalyzer(AppLuceneVersion);

            // Create an index writer
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);


            foreach(var line in lines)
            {
                var info = ParseFilePaths(line);
                var doc = new Document
                {
                    new StringField("filename",
                        info.Filename,
                        Field.Store.YES),
                    new TextField("extension",
                        info.Extension,
                        Field.Store.YES),
                    new TextField("fullpath",
                        info.FullPath,
                        Field.Store.YES)
                };
                writer.AddDocument(doc);
                
            }
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
            writer.Commit();
        }

        public static List<PathInfo> ConstructQuery(string q = "awesome")
        {
            var result = new List<PathInfo>();
            if (string.IsNullOrEmpty(q))
            {
                return result;
            }

            var indexPath = Path.Combine(BASE_PATH, FILE_SEARCH_INDEX);

            using var dir = FSDirectory.Open(indexPath);

            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);

            using var reader = DirectoryReader.Open(dir);
            var searcher = new IndexSearcher(reader);
            var parser = new QueryParser(LuceneVersion.LUCENE_48, "fullpath", analyzer);
            Query query = parser.Parse(q);

            var topDocs = searcher.Search(query, 20).ScoreDocs;
             
            foreach (var hit in topDocs)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                result.Add(
                    new PathInfo
                    {
                        Filename = foundDoc.Get("filename"),
                        Extension = foundDoc.Get("extension"),
                        FullPath = foundDoc.Get("fullpath"),
                        Size = 0
                    });
            }
            return result;
        }
    }
}
