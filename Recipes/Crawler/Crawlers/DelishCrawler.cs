using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
    [RecipeSource("Delish")]
    public class DelishCrawler : CrawlerBase
    {
        protected override string baseURL
        {
            get { return "http://www.delish.com/recipes/"; }
        }

        protected override string recipesXPath
        {
            get { return "//div[@class='landing-feed--special-container recipe']//a"; }
        }
    }
}
