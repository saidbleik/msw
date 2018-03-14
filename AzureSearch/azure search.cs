using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{

    public class Doc
    {
        public string Content { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //URL: https://.....search.windows.net
            //Key: 
            //Index: 


            string outFile = "";
            string searchServiceName = "";
            string queryApiKey = "";
            string indexName = "";

            SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, indexName, new SearchCredentials(queryApiKey));

            var parameters = new Microsoft.Azure.Search.Models.SearchParameters()
            {
                Select = new[] { "Content" },
                Top = 10000
            };

            var results = indexClient.Documents.Search<Doc>("well");

            int c = 0;
            using (var sw = new StreamWriter(outFile))
            {
                foreach (SearchResult<Doc> r in results.Results)
                {
                    var text = r.Document.Content;
                    if (text.Length > 100)
                    {
                        text = Regex.Replace(text, "[^A-Za-z.]", " ");
                        text = Regex.Replace(text, "\\s+", " ");
                        sw.WriteLine(text.ToLower());
                        Console.WriteLine(++c);
                    }
                }
            }
            Console.ReadLine();

        }
    }
}
