//using PaJaMa.Common;

//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Saveur", StartPage = 0)]
//    public class SaveurCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.saveur.com"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "<span class=\"total-page\"> Page 1 of (.*?)</span>",
//                    URLFormat = "?page={0}"
//                };
//            }
//        }

//        protected override string allURL
//        {
//            get { return "/recipes"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<h2><a href=\"(/article/Recipes/.*?)\">(.*?)</a></h2>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<h4>.*?INSTRUCTIONS</h4>(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<span itemprop=\"rating\" style=\"text-indent: -9999px;display:block;position:absolute;\">(.*?)</span>"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "\\b((SERVES|SERVINGS|MAKES)\\b.*?)<h4>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<h4>INGREDIENTS</h4>(.*?)<h4>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "property=\"og:image\" content=\"(.*?)\" />"; }
//        }

//        protected override string getHTML(string url)
//        {
//            WebClient objWebClient = new WebClient();
//            objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//            return objWebClient.DownloadString(url);
//        }

//        protected override List<string> getIngredientLines(string html)
//        {
//            List<string> lines = base.getIngredientLines(html);
//            var newLines = from l in lines
//                           from sl in l.Split(new string[] { "<br>", "<br />" }, StringSplitOptions.RemoveEmptyEntries)
//                           select HttpUtility.HtmlDecode(sl);
//            return newLines.ToList();
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            List<string> keywords = new List<string>();
//            MatchCollection mc = Regex.Matches(html, "<span class=\"field-content\"><a href=\"(.*?)\">.*?</a></span>");
//            foreach (Match m in mc)
//            {
//                keywords.Add(baseURL + m.Groups[1].Value);
//            }
//            return keywords;
//        }

//        protected override string getDirections(string html)
//        {
//            Match m4 = Regex.Match(html, directionsRegexPattern, RegexOptions.Singleline);
//            if (m4.Success)
//            {
//                string directions = m4.Groups[1].Value.Replace("<br>", "\r\n");
//                return HttpUtility.HtmlDecode(Common.StripHTML(directions)).Trim();
//            }
//            return string.Empty;
//        }

//        //public static void Crawl(DataFactory df)
//        //{
//        //	MatchCollection mc = Regex.Matches(html, "<span class=\"field-content\"><a href=\"(.*?)\">.*?</a></span>");
//        //	foreach (Match m in mc)
//        //	{
//        //		objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//        //		html = objWebClient.DownloadString("http://www.saveur.com" + m.Groups[1].Value);
//        //		Match m2 = Regex.Match(html, "<span class=\"total-page\"> Page 1 of (.*?)</span>");
//        //		if (m2.Success)
//        //		{
//        //			for (int i = 0; i < int.Parse(m2.Groups[1].Value); i++)
//        //			{
//        //				objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//        //				string html2 = objWebClient.DownloadString("http://www.saveur.com" + m.Groups[1].Value + "?page=" + i.ToString());
//        //				parsePage(df, html2);
//        //			}
//        //		}
//        //		else
//        //			parsePage(df, html);
//        //	}
//        //}

//        //private static void parsePage(DataFactory df, string html)
//        //{
//        //	WebClient objWebClient = new WebClient();
//        //	MatchCollection mc = Regex.Matches(html, "<h2><a href=\"(.*?)\">(.*?)</a></h2>");

//        //	string src = "saveur.com";
//        //	foreach (Match m in mc)
//        //	{
//        //		Console.WriteLine(m.Groups[2].Value);
//        //		Recipe existingRec = CrawlerHelper.GetRecipe(df, m.Groups[2].Value, src);
//        //		if (existingRec != null) continue;
//        //		objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//        //		string html2 = objWebClient.DownloadString("http://www.saveur.com" + m.Groups[1].Value);
//        //		Match m2 = Regex.Match(html2, " <img .*? class=\"media-image media-element \" data-image_style=\".*?\" style=\"float:right;\" " +
//        //			"itemprop=\"photo\" typeof=\"foaf:Image\" data-src=\"(.*?)\" ");

//        //		if (!m2.Success)
//        //		{
//        //			m2 = Regex.Match(html2, "itemprop=\"photo\" typeof=\"foaf:Image\" src=\"(.*?)\" ");
//        //			if (!m2.Success)
//        //				throw new NotImplementedException();
//        //		}


//        //		Recipe rec = df.CreateDataItem<Recipe>();
//        //		rec.RecipeName = m.Groups[2].Value;
//        //		rec.RecipeURL = "http://www.saveur.com" + m.Groups[1].Value;
//        //		rec.RecipeSource = CrawlerHelper.GetRecipeSource(df, src);
//        //		CrawlerHelper.AddImage(rec, m2.Groups[1].Value);



//        //		m2 = Regex.Match(html2, "\\b(SERVES|SERVINGS|MAKES)\\b(.*?)<h4>INGREDIENTS</h4>(.*?)<h4>.*?INSTRUCTIONS</h4>(.*?)</div></div></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        //		if (!m2.Success)
//        //			throw new NotImplementedException();

//        //		rec.Directions = m2.Groups[4].Value.Replace("<br /><br />", "\r\n").Replace("<br />", "\r\n").Trim();

//        //		Match m3 = Regex.Match(m2.Groups[2].Value, @"(\d+)");
//        //		rec.NumberOfServings = int.Parse(m3.Groups[1].Value);

//        //		m3 = Regex.Match(html2, "<span itemprop=\"rating\" style=\"text-indent: -9999px;display:block;position:absolute;\">(.*?)</span>");
//        //		if (m3.Success)
//        //			rec.Rating = (int)Math.Round(decimal.Parse(m3.Groups[1].Value));

//        //		string[] ingredients = m2.Groups[3].Value.Split(new string[] { "<br />", "<br>" }, StringSplitOptions.RemoveEmptyEntries);
//        //		foreach (string ingrPart in ingredients)
//        //		{
//        //			if (string.IsNullOrEmpty(ingrPart.Trim())) continue;
//        //			string[] ingrParts = ingrPart.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//        //			float qty = 0;
//        //			if (ingrParts[0] == "¼")
//        //				qty = 0.25F;
//        //			else if (ingrParts[0] == "1½")
//        //				qty = 1.5F;
//        //			else if (ingrParts[0] == "½" || ingrParts[0] == "&#189;")
//        //				qty = 0.5F;
//        //			else if (ingrParts[0] == "¾")
//        //				qty = 0.75F;
//        //			else if (ingrParts[0] == "⅓")
//        //				qty = (float)1 / (float)3;
//        //			else if (ingrParts[0] == "2&#189;")
//        //				qty = 2.5F;
//        //			else if (ingrParts[0] == "2¼")
//        //				qty = 2.25F;
//        //			else if (ingrParts[0].ToLower() != "kosher" && ingrParts[0].ToLower() != "juice")
//        //				qty = float.Parse(ingrParts[0]);

//        //			string tempIngr = "";
//        //			Measurement measurement = null;
//        //			Match m4 = Regex.Match(ingrParts[1], @"^\b(whole|small|large|stick|cup|cups|lb|lb.|tsp|tbsp)\b");
//        //			if (m4.Success)
//        //			{
//        //				for (int j = 2; j < ingrParts.Length; j++)
//        //				{
//        //					tempIngr += " " + ingrParts[j];
//        //				}
//        //				var measurementFilt = df.CreateDataFilter<MeasurementFilter>();
//        //				measurementFilt.MeasurementName = m4.Groups[1].Value;
//        //				if (measurementFilt.MeasurementName.EndsWith("s"))
//        //					measurementFilt.MeasurementName = measurementFilt.MeasurementName.Substring(0, measurementFilt.MeasurementName.Length - 1);
//        //				measurement = measurementFilt.GetDataItems().First();
//        //			}
//        //			else
//        //			{
//        //				for (int j = 1; j < ingrParts.Length; j++)
//        //				{
//        //					tempIngr += " " + ingrParts[j];
//        //				}
//        //			}

//        //			tempIngr = PaJaMa.Common.Common.StripHTML(tempIngr).Trim();
//        //			var ingrFilt = df.CreateDataFilter<IngredientFilter>();
//        //			ingrFilt.IngredientName = tempIngr;
//        //			var ingredient = ingrFilt.GetDataItems().FirstOrDefault();

//        //			if (ingredient == null)
//        //			{
//        //				ingredient = df.CreateDataItem<Ingredient>();
//        //				ingredient.IngredientName = tempIngr;
//        //				ingredient.Save();
//        //			}

//        //			var imFilt = df.CreateDataFilter<IngredientMeasurementFilter>();
//        //			imFilt.IngredientID = ingredient.IngredientID;
//        //			if (measurement == null)
//        //				imFilt.Field_MeasurementID.FilterNull = true;
//        //			else
//        //				imFilt.MeasurementID = measurement.MeasurementID;

//        //			var im = imFilt.GetDataItems().FirstOrDefault();
//        //			if (im == null)
//        //			{
//        //				im = df.CreateDataItem<IngredientMeasurement>();
//        //				im.IngredientID = ingredient.IngredientID;
//        //				if (measurement != null) im.MeasurementID = measurement.MeasurementID;
//        //				im.Save();
//        //			}

//        //			var recIngrMeasurement = df.CreateDataItem<RecipeIngredientMeasurement>();
//        //			recIngrMeasurement.IngredientMeasurementID = im.IngredientMeasurementID;
//        //			if (qty != 0)
//        //				recIngrMeasurement.Quantity = qty;
//        //			rec.RecipeRecipeIngredientMeasurements.Add(recIngrMeasurement);
//        //		}

//        //		df.BeginTransaction();
//        //		try
//        //		{
//        //			rec.Save();
//        //			foreach (var ri in rec.RecipeRecipeIngredientMeasurements)
//        //			{
//        //				ri.RecipeID = rec.RecipeID;
//        //				ri.Save();
//        //			}
//        //			df.CommitTransaction();
//        //		}
//        //		catch
//        //		{
//        //			df.RollbackTransaction();
//        //			throw;
//        //		}


//        //		//string imgPath = @"\\pjserver\e\HTTP\RecipesAPI\Images\Recipes";
//        //		//string extension = Path.GetExtension(rec.ImageURL);
//        //		//string path = Path.Combine(imgPath, rec.RecipeID.ToString() + extension);
//        //		//if (!File.Exists(path))
//        //		//{
//        //		//	objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//        //		//	objWebClient.DownloadFile(rec.ImageURL, path);
//        //		//}


//        //	}
//        //}


//    }
//}
