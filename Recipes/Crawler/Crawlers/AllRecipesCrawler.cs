using HtmlAgilityPack;
using PaJaMa.Common;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
    [RecipeSource("AllRecipes")]
    public class AllRecipesCrawler : CrawlerBase
    {
        protected override string baseURL
        {
            get { return "http://allrecipes.com"; }
        }

        protected override string allURL
        {
            get { return "/"; }
        }

        protected override string recipesXPath
        {
            get { return "//article[contains(@class, 'grid-col--fixed-tiles')]/a[contains(@href, '/recipe/')]"; }
        }

        protected override string imagesXPath
        {
            get { return "//td[@class='photodetail_td']//img"; }
        }

        protected override string ingredientAmountXPath
        {
            get { return "//*[@class='ingredient-amount']"; }
        }

        protected override string ingredientNameXPath
        {
            get { return "//*[@class='ingredient-name']"; }
        }

        protected override Tuple<string, string> getRecipeURL(HtmlNode node)
        {
            var baseUrl = base.getRecipeURL(node);
            return new Tuple<string, string>(baseUrl.Item1,
                baseUrl.Item2.Replace("Staff Pick", "").Replace("Recipe of the Day", "").Trim());
        }

        protected override string getPageURL(string keywordPage, int pageNum)
        {
            return keywordPage + (pageNum > 0 ? "?page=" + pageNum.ToString() : "");
        }

        //protected override List<RecipeImage> getRecipeImages(HtmlNode node)
        //{
        //    var imageNode = node.SelectSingleNode("//a[@title='See Recipe Pictures']");
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(getHTML(baseURL + imageNode.Attributes["href"].Value));
        //    var imageNodes = doc.DocumentNode.SelectNodes("//img[contains(@src, '/userphotos/')]");
        //    List<string> imageURLs = new List<string>();
        //    var images = new List<RecipeImage>();
        //    foreach (var imgNode in imageNodes)
        //    {
        //        var imageURL = getImageURL(imgNode);
        //        if (imageURL == null) continue;
        //        if (!imageURL.StartsWith(baseURL) && !imageURL.StartsWith("http://") && !imageURL.StartsWith("https://") && !imageURL.StartsWith("//"))
        //            imageURL = baseURL + imageURL;

        //        if (imageURLs.Contains(imageURL)) continue;
        //        imageURLs.Add(imageURL);

        //        RecipeImage img = new RecipeImage();
        //        img.ImageURL = imageURL;
        //        img.LocalImagePath = null;
        //        images.Add(img);
        //    }

        //    return images;
        //}

        //protected override List<HtmlNode> getRecipeImagesPageNodes(HtmlNode node)
        //{
        //    var imagesPageNode = node.SelectSingleNode("//a[@title='See Recipe Pictures']");
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(getHTML(baseURL + imagesPageNode.Attributes["href"].Value));
        //    List<HtmlNode> imageNodes = new List<HtmlNode>();
        //    var photoNodes = doc.DocumentNode.SelectNodes("//img[contains(@src, '/userphotos/')]");
        //    if (photoNodes == null) return new List<HtmlNode>();
        //    foreach (var imageNode in photoNodes)
        //    {
        //        var doc2 = new HtmlDocument();
        //        doc2.LoadHtml(getHTML(imageNode.Attributes["href"].Value));
        //        imageNodes.Add(doc2.DocumentNode);
        //    }

        //    return imageNodes;
        //}

        protected override void updateMaxPage(HtmlDocument doc, ref int maxPage)
        {
            base.updateMaxPage(doc, ref maxPage);
            maxPage++;
        }

        protected override float? getRating(HtmlAgilityPack.HtmlNode node)
        {
            var ratingNode = node.SelectSingleNode("//div[@class='rating-stars']");
            if (ratingNode != null)
            {
                float rating = 0;
                if (!float.TryParse(ratingNode.Attributes["data-ratingstars"].Value, out rating))
                {

                }
                return rating;
            }
            return null;
        }

        protected override List<string> getKeywordPages(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes("//ul[@class='browse-hubs__subcategories']/*/a").Select(n =>
                baseURL + n.Attributes["href"].Value)
                .ToList();
        }

        #region OLD
        //public static void Crawl(DataFactory df)
        //{
        //	WebClient wc = new WebClient();
        //	string html = wc.DownloadString("http://allrecipes.com/recipes/main.aspx");
        //	Match m2 = Regex.Match(html, "<p class=\"searchResultsCount results\">.*?<span class=\"emphasized\">(.*?)</span>", RegexOptions.Singleline);
        //	float total = float.Parse(m2.Groups[1].Value);
        //	double maxPages = Math.Ceiling(total / 20);
        //	for (int i = 1; i <= maxPages; i++)
        //	// for (int i = 1596; i <= 2459; i++)
        //	{
        //		html = wc.DownloadString(string.Format("http://allrecipes.com/recipes/main.aspx?vm=l&evt19=1&p34=HR_SortByTitle&st=t&Page={0}#recipes", i));
        //		MatchCollection mc = Regex.Matches(html, "<a id=\".*?_lnkTitle\" href=\"(.*?)\">(.*?)</a>");
        //		foreach (Match m in mc)
        //		{
        //			string recipeName = m.Groups[2].Value;
        //			string recipeURL = m.Groups[1].Value;
        //			string src = "allrecipes.com";
        //			Recipe existingRec = CrawlerHelper.GetRecipe(df, recipeName, src);
        //			if (existingRec != null)
        //			{
        //				Console.WriteLine(existingRec.RecipeSourceName + " - " + existingRec.RecipeName);
        //				if (existingRec.RecipeURL == null)
        //				{
        //					existingRec.RecipeURL = recipeURL;
        //					existingRec.Save();
        //				}
        //				continue;
        //			}

        //			html = wc.DownloadString(m.Groups[1].Value);
        //			m2 = Regex.Match(html, "<img id=\"imgPhoto\" class=\".*? photo\" itemprop=\"image\".*?src=\"(.*?)\".*?/></a>");
        //			if (!m2.Success) throw new NotImplementedException(i.ToString());

        //			Recipe rec = df.CreateDataItem<Recipe>();
        //			rec.RecipeName = recipeName;
        //			rec.RecipeURL = recipeURL;
        //			rec.RecipeSource = CrawlerHelper.GetRecipeSource(df, src);
        //			Console.WriteLine("Page: " + i.ToString() + " of " + maxPages.ToString() + " - Recipe: " + rec.RecipeName);

        //			// CrawlerHelper.AddImage(rec, m2.Groups[1].Value);
        //			SetImages(rec);

        //			SetRating(rec, html);

        //			MatchCollection mc2 = Regex.Matches(html, "<span class=\"plaincharacterwrap break\">(.*?)</span>");
        //			rec.Directions = "";
        //			bool firstIn = true;
        //			foreach (Match m3 in mc2)
        //			{
        //				rec.Directions += firstIn ? "" : "\n";
        //				rec.Directions += m3.Groups[1].Value;
        //				firstIn = false;
        //			}

        //			mc2 = Regex.Matches(html, "<p class=\"fl-ing\" itemprop=\"ingredients\">(.*?)</p>", RegexOptions.Singleline);

        //			foreach (Match m3 in mc2)
        //			{
        //				Match m4 = Regex.Match(m3.Groups[1].Value, "<span id=\"lblIngName\" class=\"ingredient-name\">(.*?)</span>");
        //				if (!m4.Success) continue;
        //				string ingredient = m4.Groups[1].Value;

        //				m4 = Regex.Match(m3.Groups[1].Value, "<span id=\"lblIngAmount\" class=\"ingredient-amount\">(.*?)</span>");
        //				double qty = 0;
        //				Measurement measurement = null;
        //				if (m4.Success)
        //				{
        //					string[] parts = Common.StripHTML(m4.Groups[1].Value).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //					double tempQty = 0;
        //					int index = 0;
        //					while (index < parts.Length && Common.TryParseFraction(parts[index], out tempQty))
        //					{
        //						qty += tempQty;
        //						index++;
        //					}

        //					string measurementString = string.Empty;
        //					for (int j = index; j < parts.Length; j++)
        //					{
        //						measurementString += " " + parts[j];
        //					}

        //					measurementString = measurementString.Trim();

        //					if (!string.IsNullOrEmpty(measurementString))
        //					{
        //						MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
        //						mf.MeasurementName = measurementString;
        //						measurement = mf.GetDataItems().FirstOrDefault();
        //						if (measurement == null)
        //						{
        //							measurement = df.CreateDataItem<Measurement>();
        //							measurement.MeasurementName = measurementString;
        //							measurement.Save();
        //						}
        //					}
        //				}

        //				CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
        //			}

        //			CrawlerHelper.SaveRecipe(rec);
        //			System.Threading.Thread.Sleep(100);
        //		}
        //	}
        //}

        //public static void SetRating(Recipe rec, string html)
        //{
        //	Match m2 = Regex.Match(html, "<meta itemprop=\"ratingValue\" content=\"(.*?)\" />");
        //	if (m2.Success)
        //		rec.Rating = Convert.ToSingle(m2.Groups[1].Value);
        //}

        //public static bool SetImages(Recipe rec)
        //{
        //	string url = rec.RecipeURL;
        //	int index = url.IndexOf("/detail");
        //	url = url.Substring(0, index) + "/photo-gallery.aspx";
        //	WebClient wc = new WebClient();
        //	string html = wc.DownloadString(url);
        //	int pages = 1;
        //	MatchCollection mc = Regex.Matches(html, "\\?Page=.*?\">(\\d*)</");
        //	if (mc.Count > 0)
        //		pages = mc.OfType<Match>().Select(m => int.Parse(m.Groups[1].Value)).Max();

        //	parseImages(rec, html);

        //	for (int i = 2; i <= pages; i++)
        //	{
        //		parseImages(rec, wc.DownloadString(url + "?Page=" + i.ToString()));
        //	}

        //	return rec.RecipeRecipeImages.Any();
        //}

        //private static void parseImages(Recipe rec, string html)
        //{
        //	MatchCollection mc = Regex.Matches(html, "<a .*? href=\"(.*?\\?photoID=\\d*)\"><img");
        //	foreach (var m in mc.OfType<Match>())
        //	{
        //		try
        //		{
        //			string html2 = new WebClient().DownloadString(m.Groups[1].Value);
        //			Match m2 = Regex.Match(html2, "(http://images.media-allrecipes.com/userphotos.*?)\"");
        //			CrawlerHelper.AddImage(rec, m2.Groups[1].Value);
        //		}
        //		catch
        //		{
        //			continue;
        //		}
        //	}
        //}
        #endregion

    }
}
