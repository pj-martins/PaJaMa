using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
    [RecipeSource("MyRecipes", StartPage = 0)]
    public class MyRecipesCrawler : CrawlerBase
    {
        protected override string baseURL
        {
            get { return "http://www.myrecipes.com"; }
        }

        protected override string allURL
        {
            get { return "/search/site?f[0]=bundle%3Arecipe"; }
        }

        protected override string recipesXPath
        {
            get { return "//a[contains(@href, 'http://www.myrecipes.com/recipe/')]"; }
        }

        protected override string ingredientsXPath
        {
            get { return "//*[@itemprop='ingredient']"; }
        }

        protected override string imagesXPath
        {
            get { return "//img[@typeof='foaf:Image' and @width='300']"; }
        }

        protected override PageNumbers pageNumberURLRegex
        {
            get
            {
                return new PageNumbers()
                {
                    MaxPageRegexPattern = "\\?page=(\\d*)",
                    URLFormat = "&page={0}"
                };
            }
        }

        protected override string getImageURL(HtmlAgilityPack.HtmlNode node)
        {
            var imageURL = base.getImageURL(node);
            if (imageURL == "http://cdn-image.myrecipes.com/sites/all/themes/myrecipes/images/legacy/img_noPhoto150.gif") return null;
            return imageURL;
        }
    }
}
