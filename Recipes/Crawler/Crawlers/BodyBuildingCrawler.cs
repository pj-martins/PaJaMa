using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
    [RecipeSource("BodyBuilding")]
    public class BodyBuildingCrawler : CrawlerBase
    {
        protected override string baseURL
        {
            get { return "http://www.bodybuilding.com/fun/healthy-recipe-database.html"; }
        }

        protected override string recipesXPath
        {
            get { return "//div[contains(@class, 'small-articles-container ')]/div[@class='small-article-header']/*/a"; }
        }

        protected override List<string> getKeywordPages(HtmlDocument doc)
        {
            var pages = new List<string>();
            var recipeNodes = doc.DocumentNode.SelectNodes("//a[contains(@href, 'recipes.html') and contains(@href, 'healthy')]");
            foreach (HtmlNode node in recipeNodes)
            {
                pages.Add(node.Attributes["href"].Value);
            }
            return pages;
        }

        protected override void crawlPage(string url, int pageNum, ref int maxPage)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(getHTML(url));
            var urls = getRecipeURLs(doc);
            List<string> urls1 = new List<string>();
            foreach (var kvp in urls)
            {
                if (urls1.Contains(kvp.Key))
                    continue;

                var html = getHTML(kvp.Key);

                string url1 = kvp.Key;
                if (url1.IndexOf("#") > 0)
                    url1 = url1.Substring(0, url1.IndexOf("#"));

                if (urls1.IndexOf(url) > 0)
                    continue;


                //var matches = Regex.Matches(html, "<a name=\"(.*?)\"></a>").OfType<Match>().ToList();
                //var recipeParts = new List<string>();
                //for (int i = 0; i < matches.Count; i++)
                //{
                //    var m = matches[i].Groups[1].Value;
                //    recipeParts.Add(html.Substring(i == 0 ? 0 : html.IndexOf(matches[i - 1].Groups[1].Value), html.IndexOf(m)));
                //}
                //var doc2 = new HtmlDocument();
                //doc2.LoadHtml(html);
                //List<string> recipeParts = new List<string>();
                //if (kvp.Key.IndexOf("#") <= 0)
                //{
                //    recipeParts.Add(html);
                //    urls1.Add(kvp.Key);
                //}
                //else
                //{
                //    var bookmark = kvp.Key.Substring(kvp.Key.LastIndexOf("#") + 1);
                //    urls1.Add(kvp.Key.Substring(0, kvp.Key.LastIndexOf("#")));
                //    var bookmarkOuter = doc2.DocumentNode.SelectNodes("//*[@id='" + bookmark + "']")[0].OuterHtml;
                //    var tag = bookmarkOuter.Substring(0, bookmarkOuter.IndexOf(bookmark));
                //    recipeParts.AddRange(html.Split(new string[] { tag }, StringSplitOptions.None).Skip(1));
                //}

                //foreach (var part in recipeParts)
                //{
                System.IO.File.WriteAllText("d:\\recipe_part_" + (Guid.NewGuid()).ToString() + ".txt", html);
                //}
            }
        }
    }
}
