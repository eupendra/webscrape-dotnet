using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace CODE

{
    class Program
    {

        static void Main(string[] args)
        {

            var bookLinks = GetBookLinks("http://books.toscrape.com/catalogue/category/books/mystery_3/index.html");
            var books = GetBookDetails(bookLinks);
            Console.WriteLine("Retrieved..");
            exportToCSV(books);
            Console.WriteLine("Exported!");

        }
        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
        static List<string> GetBookLinks(string url)
        {
            var bookLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            var linkNodes = doc.DocumentNode.SelectNodes("//h3/a");

            var baseUri = new Uri(url);
            foreach (var link in linkNodes)
            {
                if (!string.IsNullOrEmpty(link.Attributes["href"].Value))
                {
                    string href = link.Attributes["href"].Value;
                    bookLinks.Add(new Uri(baseUri, href).AbsoluteUri);
                }

            }
            //TODO: Paging
            return bookLinks;
        }

        static List<Book> GetBookDetails(List<string> urls)
        {
            var books = new List<Book>();
            foreach (var url in urls)
            {
                HtmlDocument document = GetDocument(url);
                var priceXPath = "//div[contains(@class,\"product_main\")]/p[@class=\"price_color\"]";
                var titleXPath = "//h1";
                var book = new Book();
                book.Title = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
                book.Price = document.DocumentNode.SelectSingleNode(priceXPath).InnerText;
                // Console.WriteLine(book.Title);
                books.Add(book);
            }

            return books;
        }

        static void exportToCSV(List<Book> books)
        {
            using (var writer = new StreamWriter("./books.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }


    }
}