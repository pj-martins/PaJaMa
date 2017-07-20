using PaJaMa.Common;

using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Crawler.Crawlers
{
	[RecipeSource("Cooks")]
	public class CooksCrawler : CrawlerBase
	{
		protected override string baseURL
		{
			get { return "http://www.cooks.com"; }
		}

		protected override PageNumbers pageNumberURLRegex
		{
			get
			{
				return new PageNumbers()
				{
					MaxPageRegexPattern = " of <b>(.*?)</b>"
				};
			}
		}

		protected override string recipesXPath
		{
			get { return "//a[@class='lnk b' and contains(@href, '/recipe/')]"; }
		}

		protected override string directionsXPath
		{
			get { return "//div[@class='instructions']"; }
		}

		protected override string ingredientsXPath
		{
			get { return "//span[@class='ingredient']"; }
		}

		protected override List<string> getKeywordPages(HtmlDocument document)
		{
			List<string> keywordPages = new List<string>();
			var nodes = document.DocumentNode.SelectNodes("//a[contains(@href, '/rec/ch/')]");
			foreach (var node in nodes)
			{
				var page = getHTML(baseURL + node.Attributes["href"].Value);
				Console.WriteLine("Getting keywords - " + node.Attributes["href"].Value);
				var doc2 = new HtmlDocument();
				doc2.LoadHtml(page);
				var nodes2 = doc2.DocumentNode.SelectNodes("//a[contains(@href, '/rec/search')]");
				foreach (HtmlNode node2 in nodes2)
				{
					keywordPages.Add(baseURL + node2.Attributes["href"].Value);
				}
			}
			return keywordPages.Distinct().OrderBy(k => k).ToList();
			//return Properties.Resources.Keywords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
			//	.Select(k => $"http://www.cooks.com/rec/search/0,1-00,{k.ToLower()},FF.html")
			//	.ToList();
		}

		protected override int getMaxPage(float maxPageMatch)
		{
			return (int)Math.Ceiling(maxPageMatch / 10);
		}

		protected override string getPageURL(string keywordPage, int pageNum)
		{
			if (keywordPage.Contains("0,1-00,"))
				keywordPage = keywordPage.Replace("0,1-00,", "0,1-0,");
			return keywordPage.Replace("0,1-0,", "0,1-" + ((pageNum - 1) * 10).ToString() + ",");
		}

		protected override float? getRating(HtmlNode node)
		{
			var rn = node.SelectSingleNode("//span[contains(@class, 'rating-static')]/following-sibling::b");
			if (rn == null) return null;
			var match = Regex.Match(rn.InnerText, "Rating: (.*?)&nbsp;");
			return Convert.ToSingle(match.Groups[1].Value);
		}

		protected override void updateMaxPage(HtmlDocument doc, ref int maxPage)
		{
			base.updateMaxPage(doc, ref maxPage);
			if (!doc.DocumentNode.InnerHtml.ToLower().Contains("<b>next</b>"))
				maxPage = 1;
		}
		#region OLD
		//        protected override string allURL
		//        {
		//            get { return string.Empty; }
		//        }

		//        protected override string recipesRegexPattern
		//        {
		//            get { return "<A CLASS=\"lnk b\" HREF=\"(/recipe.*?)\"><B>(.*?)</B></A>"; }
		//        }

		//        protected override PageNumbers pageNumberURLRegex
		//        {
		//            get
		//            {
		//                return new PageNumbers()
		//                {
		//                    MaxPageRegexPattern = "</B> of .*?<B>(.*?)</B>"
		//                };
		//            }
		//        }

		//        protected override string directionsRegexPattern
		//        {
		//            get { return "><DIV class=\"instructions\" STYLE=\".*?\">(.*?)</DIV>"; }
		//        }

		//        protected override string ratingRegexPattern
		//        {
		//            get { return "<B>Rating: (.*?)&nbsp;/"; }
		//        }

		//        protected override string ingredientsRegexPattern
		//        {
		//            get { return "<SPAN class=\"ingredient\">(.*?)</SPAN>"; }
		//        }

		//        protected override string imageRegexPattern
		//        {
		//            get { return "<IMG class=\"photo\" .*? SRC=\"(.*?)\">"; }
		//        }

		//        protected override List<string> getKeywordPages(string html)
		//        {
		//            var keywords = DbContext.Recipes.Select(r => r.RecipeName).Distinct().ToList();
		//            keywords.AddRange(CrawlerHelper.GetKeywords(myAttribute.StartKeyword));
		//            return (from k in keywords
		//                    select baseURL + "/rec/search?q=" + HttpUtility.UrlEncode(k)).ToList();
		//            //return (from k in CrawlerHelper.GetKeywords(myAttribute.StartKeyword)
		//            //        select baseURL + "/rec/search?q=" + HttpUtility.UrlEncode(k)).ToList();
		//        }

		//        protected override int getMaxPage(float maxPageMatch)
		//        {
		//            maxPageMatch = (int)Math.Ceiling(maxPageMatch / 10);
		//            return maxPageMatch > 85 ? 85 : (int)maxPageMatch;
		//        }

		//        protected override string getPageURL(string keywordPage, int pageNum)
		//        {
		//            string keyword = HttpUtility.UrlDecode(keywordPage).Replace(baseURL + "/rec/search?q=", "").ToLower();
		//            return baseURL + "/rec/doc/0,1-" + (pageNum * 10 + 1).ToString("00") + "," + keyword.ToString().Replace(" ", "_") + ",FF.html";
		//        }

		//        /*
		//        public static void Crawl(DataFactory df)
		//        {
		//            WebClient wc = new WebClient();

		//            string src = "cooks.com";
		//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;
		//            List<string> recURLs = df.GetDataTable("select RecipeURL from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
		//                .Select(r => r["RecipeURL"].ToString()).ToList();

		//            foreach (string keyword in CrawlerHelper.GetKeywords())
		//            {
		//                string baseURL = "http://www.cooks.com/rec/search?q=" + HttpUtility.UrlEncode(keyword);
		//                string html = wc.DownloadString(baseURL);
		//                Match m3 = Regex.Match(html, "</B> of (about )?<B>(.*?)</B>");
		//                int numRecipes = Convert.ToInt16(m3.Groups[2].Value.Replace(",", ""));
		//                int maxPage = (int)Math.Ceiling((decimal)numRecipes / 10);
		//                if (maxPage > 1)
		//                {
		//                    html = wc.DownloadString(baseURL);
		//                    Match m4 = Regex.Match(html, "<a class=\"lnk\" href=\"(.*?)\">2</a>", RegexOptions.IgnoreCase);
		//                    if (m4.Success)
		//                        baseURL = "http://www.cooks.com" + m4.Groups[1].Value.Replace("0,1-11,", "0,1-00,");
		//                }
		//                for (int i = 0; i < maxPage; i++)
		//                {
		//                    if (i > 85) break;
		//                    CrawlPage(df, srcID, baseURL, i, maxPage, wc, recURLs);
		//                }
		//            }
		//        }

		//        public static void CrawlPage(DataFactory df, int srcID, string baseURL, int pageNum, int maxPage, WebClient wc, List<string> recURLs)
		//        {
		//            string url = baseURL.Replace("0,1-00,", "0,1-" + (pageNum * 10 + 1).ToString("00") + ",");
		//            string html = wc.DownloadString(url);
		//            MatchCollection mc = Regex.Matches(html, "<A CLASS=\"lnk b\" HREF=\"(/recipe.*?)\"><B>(.*?)</B></A>");
		//            foreach (Match m in mc.OfType<Match>())
		//            {
		//                string recURL = "http://www.cooks.com" + m.Groups[1].Value;
		//                string recipeName = m.Groups[2].Value;
		//                if (recURLs.Contains(recURL))
		//                {
		//                    Console.WriteLine("Page: " + (pageNum + 1).ToString() + " of " + maxPage.ToString() + " Recipe: " + recipeName);
		//                    continue;
		//                }

		//                Console.WriteLine("* Page: " + (pageNum + 1).ToString() + " of " + maxPage.ToString() + " Recipe: " + recipeName);
		//                Recipe rec = df.CreateDataItem<Recipe>();
		//                rec.RecipeSourceID = srcID;
		//                rec.RecipeName = recipeName;
		//                rec.RecipeURL = recURL;

		//                int tries = 3;
		//                while (tries > 0)
		//                {
		//                    try
		//                    {
		//                        html = wc.DownloadString(rec.RecipeURL);
		//                        break;
		//                    }
		//                    catch
		//                    {
		//                        System.Threading.Thread.Sleep(1000);
		//                        tries--;
		//                    }
		//                }

		//                if (tries == 0)
		//                    throw new Exception("ERROR");

		//                MatchCollection mc3 = Regex.Matches(html, "><DIV class=\"instructions\" STYLE=\".*?\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

		//                if (mc3.Count > 0)
		//                {
		//                    rec.Directions = string.Join("\r\n\r\n",
		//                        (from m4 in mc3.OfType<Match>()
		//                         select Common.StripHTML(m4.Groups[1].Value.Replace("<P>", "\r\n"))).ToArray()).Trim();

		//                }
		//                Match m3 = Regex.Match(html, "<B>Rating: (.*?)&nbsp;/");

		//                if (m3.Success)
		//                    rec.Rating = Convert.ToSingle(m3.Groups[1].Value);

		//                MatchCollection mc2 = Regex.Matches(html, "<SPAN class=\"ingredient\">(.*?)</SPAN>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

		//                foreach (Match m2 in mc2)
		//                {
		//                    bool createMeasurement = false;
		//                    string ingredient = string.Empty;

		//                    List<string> parts = Common.StripHTML(m2.Groups[1].Value.Replace("-", " - ")).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

		//                    string measurementName = "";
		//                    string qtyString = "";

		//                    int index = 0;
		//                    double tempQty = 0;
		//                    while (index < parts.Count && (Common.TryParseFraction(parts[index], out tempQty) || parts[index] == "-")) //!(parts[index].Any(c => (int)c >= (int)'a' && (int)c <= (int)'z')))
		//                    {
		//                        index++;
		//                    }

		//                    for (int i = 0; i < index; i++)
		//                    {
		//                        qtyString += parts[i].Trim() + " ";
		//                    }

		//                    qtyString = qtyString.Trim();

		//                    float qty = 0;
		//                    if (!string.IsNullOrEmpty(qtyString))
		//                    {
		//                        if (qtyString.Contains("/"))
		//                        {
		//                            if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
		//                                throw new NotImplementedException();
		//                        }
		//                        else if (qtyString.Contains("-"))
		//                        {
		//                            string[] parts2 = qtyString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
		//                            if (parts2.Length != 2)
		//                                index = 0;
		//                            else
		//                            {
		//                                float first = 0;
		//                                float second = 0;

		//                                if (!PaJaMa.Common.Common.TryParseFraction(parts2[0], out first))
		//                                    throw new NotImplementedException();

		//                                if (!PaJaMa.Common.Common.TryParseFraction(parts2[1], out second))
		//                                    throw new NotImplementedException();

		//                                qty = (first + second) / 2;
		//                            }
		//                        }
		//                        else if (qtyString.Contains(" "))
		//                        {
		//                            string[] parts2 = qtyString.Split(' ');
		//                            qty = Convert.ToSingle(parts2[0]);
		//                            index -= parts2.Length - 1;
		//                        }
		//                        else
		//                            qty = Convert.ToSingle(qtyString);
		//                    }

		//                    bool firstIn = true;
		//                    for (int i = index; i < parts.Count; i++)
		//                    {
		//                        if (firstIn)
		//                            measurementName += parts[i].Trim() + " ";
		//                        else
		//                            ingredient = ingredient + " " + parts[i].Trim();
		//                        firstIn = false;
		//                    }

		//                    measurementName = measurementName.ToLower()
		//                        .Replace("c.", "cup")
		//                        .Replace("pkg", "package")
		//                        .Replace("lg.", "large")
		//                        .Replace(".", "").Trim();

		//                    if (measurementName == "sm") measurementName = "small";

		//                    ingredient = ingredient.Trim();

		//                    Measurement measurement = null;
		//                    if (!string.IsNullOrEmpty(measurementName))
		//                    {
		//                        MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
		//                        mf.MeasurementName = measurementName;
		//                        measurement = mf.GetDataItems().FirstOrDefault();
		//                        if (measurement == null)
		//                        {
		//                            if (createMeasurement)
		//                            {
		//                                measurement = df.CreateDataItem<Measurement>();
		//                                measurement.MeasurementName = mf.MeasurementName;
		//                                measurement.Save();
		//                            }
		//                            else
		//                            {
		//                                ingredient = measurementName + " " + ingredient;
		//                                measurementName = string.Empty;
		//                            }
		//                        }
		//                    }

		//                    if (!string.IsNullOrEmpty(ingredient))
		//                        CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
		//                }

		//                if (!rec.RecipeRecipeIngredientMeasurements.Any())
		//                    continue;

		//                m3 = Regex.Match(html, "<IMG class=\"photo\" .*? SRC=\"(.*?)\">");
		//                if (m3.Success)
		//                {
		//                    RecipeImage img = rec.RecipeRecipeImages.FirstOrDefault() ?? df.CreateDataItem<RecipeImage>();
		//                    img.ImageURL = m3.Groups[1].Value;
		//                    img.LocalImagePath = null;
		//                    rec.RecipeRecipeImages.Add(img);
		//                }

		//                CrawlerHelper.SaveRecipe(rec);
		//                recURLs.Add(rec.RecipeURL);
		//            }
		//        }
		//         * */

		//        protected override string servingsRegexPattern
		//        {
		//            get { return string.Empty; }
		//        }
		//    }
		//}
		#endregion
	}
}
