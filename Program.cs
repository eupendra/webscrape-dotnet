using System;
using HtmlAgilityPack;
namespace CODE
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = @"http://html-agility-pack.net/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
            Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);

        }
    }
}
