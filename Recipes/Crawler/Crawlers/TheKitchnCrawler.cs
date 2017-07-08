using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PaJaMa.Common;

namespace Crawler.Crawlers
{
	[RecipeSource("The Kitchn")]
	public class TheKitchnCrawler : CrawlerBase
	{
		protected override string baseURL
		{
			get { return "http://www.thekitchn.com"; }
		}

		protected override string allURL
		{
			get { return "/categories"; }
		}

		protected override List<string> getKeywordPages(HtmlDocument document)
		{
			return document.DocumentNode.SelectNodes("//a[@class='level-3']").Select(n => n.Attributes["href"].Value).ToList();
		}

		protected override PageNumbers pageNumberURLRegex
		{
			get { return new PageNumbers() { URLFormat = "?page={0}", UnknownTotalPages = true }; }
		}

		protected override string recipesXPath
		{
			get { return "//a[@class='SimpleTeaser']"; }
		}

		protected override string directionsXPath
		{
			get { return "//p/span[@itemprop='ingredients']/parent::p/following-sibling::p"; }
		}
	}
}
#region OLD
//        protected override string allURL
//        {
//            get { return "/recipes"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<li><a class=\"brand-link\" href=\"(http://www.thekitchn.com/.*?)\">(.*?)</a></li>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<div id=\"recipe\">.*?<p>.*?</p>.*?<p>.*?</p>.*?<p>(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "NONE"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<div itemprop=\"recipeYield\">(.*?)</div>"; }
//        }

//        protected override string ingredientSectionRegexPattern
//        {
//            get { return "<div id=\"recipe\">.*?<p>.*?</p>.*?<p>(.*?)</p>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return string.Empty; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<span data-src=\"(.*?)\"></span>"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "<li><a class=\"level-2\" href=\"(.*?)\">.*?</a>");
//            return mc.OfType<Match>().Select(m => m.Groups[1].Value).ToList();
//        }

//        protected override List<string> getIngredientLines(string html)
//        {
//            return html.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).Select(l => PaJaMa.Common.Common.StripHTML(l)).ToList();
//        }
//    }
//}
#endregion
