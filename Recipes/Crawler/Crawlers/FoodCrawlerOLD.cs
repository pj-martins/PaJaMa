//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Food.com", IgnoreAuto = true)]
//    public class FoodCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.food.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipe-finder/all"; }
//        }

//        //protected override PageNumbers pageNumberURLRegex
//        //{
//        //    get
//        //    {
//        //        return new PageNumbers()
//        //        {
//        //            MaxPageRegexPattern = "\\?pn=(\\d*)",
//        //            URLFormat = "?pn={0}"
//        //        };
//        //    }
//        //}

//        protected override void updateMaxPage(string html, ref int maxPage)
//        {
//            base.updateMaxPage(html, ref maxPage);
//        }

//        /*

//        protected override string recipesRegexPattern
//        {
//            get { return "<a class=\"recipe-main-title\" href=\"(.*?)\" .*?>(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<span class=\"instructions\"  itemprop=\"recipeInstructions\">(.*?)</span>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<li class=\"current-rating\" style=\"width: (.*?)%;\"></li>"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<p>Servings Per Recipe: (.*?)</p>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\"  itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string quantityRegexPattern
//        {
//            get { return "<span class=\"value\">(.*?)<span class=\"name\">"; }
//        }

//        protected override string ingredientRegexPattern
//        {
//            get { return "<span class=\"name\">(.*?)</span>"; }
//        }

//        protected override string imageSectionRegexPattern
//        {
//            get
//            {
//                return "<div id=\"recipe-photo-gallery-list\" class=\"visuallyhidden\">(.*?)</div>";
//            }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "src=\"(.*?)\""; }
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return matchedRating / 20;
//        }
//        */

//        protected override string getHTML(string url)
//        {
//            string html = string.Empty;
//            int tries = 3;
//            while (tries > 0)
//            {
//                try
//                {
//                    HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
//                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
//                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
//                    request.Headers.Add("Cache-Control", "max-age=0");
//                    request.Headers.Add("Cookie", "ft_id=c8edb51f-ed01-483f-ddcf-ba87430729e9; layout=desktop; s_nr=1410888695675; _gat=1; login_tab=google; role=0102zz!!14108957618720%07%00%05spersistZZyZZ%05%00%1AsrolesZZZZ1ZZZZMY%5FRECIPE%5FBOXZZZZ%07%00%0Esuser%5FidZZ1803142529ZZ%09%00%11screate%5FdtZZ1410895761872ZZ%00; value=0102zz!!14108957618720%04%00scity%07%00%05sconfirmZZ0ZZ%05%00%18semailZZpj%2Emartins%40gmail%2EcomZZ%0A%00%06sfirst%5FnameZZPJZZ%06%00%05sgenderZZFZZ%09%00%0Bslast%5FnameZZMartinsZZ%0B%00spostal%5Fcode%06%00%05sstatusZZ1ZZ%0D%00%04stranscompleteZZZZ%07%00%0Esuser%5FidZZ1803142529ZZ%09%00%14suser%5FnameZZChef%20%231803142529ZZ%09%00%05suser%5FtypeZZAZZ%08%00Ssmy%5FphotoZZhttp%3A%2F%2Fshare%2Efood%2Ecom%2Fskins%2Fmain%2Fimages%2Fprofilephotolookup%2F111%5Fmedthumb%5Fhor%2EgifZZ%00; userLoginCookie=user_id:1803142529|user_name:Chef #1803142529|email:pj.martins@gmail.com|birth_year:null|city:null|confirm:null|first_name:PJ|gender:F|last_name:Martins|persist:false|postal_code:null|status:1|; userRoleCookie=MY_RECIPE_BOX:1410895761872|; aimInfo=MTgwMzE0MjUyOXx8fHxDaGVmICMxODAzMTQyNTI5fHx8fGh0dHA6Ly9zaGFyZS5mb29kLmNvbS9za2lucy9tYWluL2ltYWdlcy9wcm9maWxlcGhvdG9sb29rdXAvMTExX21lZHRodW1iX2hvci5naWZ8fHx8aHR0cDovL2FpbS5mb29kLmNvbXx8fHxBSU1fU09DSUFMX1NJR05VUA==; s_fid=456B454154BD7961-2AA1397F72099A9C; gpv_pn=www.food.com%2Frecipe-finder%2Fall; s_cc=true; s_vi=[CS]v1|29C915F485013C18-60000111A000245B[CE]; __utma=105311892.1864486090.1402088423.1410890720.1410893201.13; __utmb=105311892.34.10.1410893201; __utmc=105311892; __utmz=105311892.1410893201.13.12.utmcsr=food.com|utmccn=(referral)|utmcmd=referral|utmcct=/recipe-finder/all; aam_dfp=aam%3D250578%2C804782%2C995819%2C312337%2C318090%2C550104%2C120911%2C260311%2C260308%2C807144%2C260313%2C311691%2C1135377%2C482238%2C690082%2C318100%2C929561%2C513250%2C513252%2C498422%2C933038%2C317646%2C807145%2C317779%2C242439%2C807141%2C260316%2C469562%2C245109%2C311695%2C807135%2C249531%2C807140%2C245110%2C120908%2C313978%2C313979%2C317794%2C317738%2C311688%2C923808%2C445063%2C492176%2C513256%2C513255%2C513267%2C311663%2C317734%2C317742%2C313963%2C313965%2C873347%2C1126453%2C513249%2C916072%2C685829%2C311697%2C375785%2C648004%2C684754%2C781062%2C120912%2C118661%2C513247%2C311423%2C405114%2C311692%2C311689%2C201984%2C886137%2C937638%2C260319%2C405118%2C1189786; aam_fw=scripps%3Dtest%3Baam%3D312337%3Baam%3D318090%3Baam%3D550104%3Baam%3D120911%3Baam%3D260311%3Baam%3D260308%3Baam%3D260313%3Baam%3D311691%3Baam%3D482238%3Baam%3D318100%3Baam%3D513250%3Baam%3D513252%3Baam%3D498422%3Baam%3D317646%3Baam%3D317779%3Baam%3D242439%3Baam%3D260316%3Baam%3D469562%3Baam%3D245109%3Baam%3D311695%3Baam%3D249531%3Baam%3D245110%3Baam%3D120908%3Baam%3D313978%3Baam%3D313979%3Baam%3D317794%3Baam%3D317738%3Baam%3D311688%3Baam%3D445063%3Baam%3D492176%3Baam%3D513256%3Baam%3D513255%3Baam%3D513267%3Baam%3D311663%3Baam%3D317734%3Baam%3D317742%3Baam%3D313963%3Baam%3D313965%3Baam%3D513249%3Baam%3D311697%3Baam%3D375785%3Baam%3D120912%3Baam%3D118661%3Baam%3D513247%3Baam%3D311423%3Baam%3D405114%3Baam%3D311692%3Baam%3D311689%3Baam%3D201984%3Baam%3D260319%3Baam%3D405118; aam_did=59069017482047825280291946385495963488; SMSESSION=MBH5BK2gpc6sbyMO2sC/QktXOLws7yLpfkm7rMZOhFpUXmcpl9lHFH86TUV+kYWH7Q15Lfl8/y7gxttr8gJ8qy+g6ikrDjrq/hdCzBZUqDw8uLnZLaC9SFcHJiEXjo+Ru7pkBNirBsnPC5IWmD+vEE+h8ICN1hkODEMsEs/a5rUJXLZbeqg2f8Cpy0HPUdGxzL2DhSkV5opEEe//SdvrDqZSagPqYUah9XPq2TZYn/i6y0uOTFhSqpncQN+MxAYEXP15o7s/bSe3aE6aPOlAmJUhnwBIBkn2hfYBr92+7lVaGjRVieIdYJUl+0xJ7QWfeuC1V271lGqfvQ8zechOCE73iDEXzK/fgD36ozGGz6hjTytyKOCx3rcmng3qsbVwIzlx6CflPeaUp3TJVUJEd8rgv3A8WN/8sG7mDFERUaqfDBJV3Ted9rzhwdso/PVcl+mLkC5wouyr7R/neEeZtMnmwROkTsXq/LuCXlnz/VTHltypJ2K44KlGlKOME6qIw/y9xz5Yz+w7z+k9kYmjUOo1Y/qOXMWQ8m6RnsKMc7ZrXJtp0dZ9QcSIgTC73obRG833uWzSYNfFzpP+kI9ls+YoUedStwhI8JgDLZKPIIm2SFpegzmW6SYKUmd0VSwNUrjvO3qndo2befvN18PYCkft/KQ+Xeshxea3kd/IWHd7S9QDVmuBGjtqVzUkIB99slQlXvVjQRDHFhRfq56QasEQmITxg6VHRyR4m07DIMe+n8hsxsDmYeiozA/GIamyzKyau0l/S/xK5WgRQVz0OBTRTrGmTuc269ljxKAdG2SVspSAge3WpfeDWLi9v32Jh7fFMh3KRrYgYxKrWkG2zCaLPmL0xZC+wxACffLZTiBre6jUBeSD25GeSByhLZxKPMKytF2ipgCgHK+6aR5kRX7u5JeHFnTcDt3JEEA9M8UzQAMqO70UtxNJ0NrUHL6O9IIWCMQvIFyGGkaBbkg1X9q1UgKC28wVmIqRV3JNUvbzvoahIOyjtGdZ1+T2kYW0AxsNOTOHCkww5uTc2qtmoH8e3fNjnLJm; s_ppv=31%7C0; QSI_HistorySession=http%3A%2F%2Fwww.food.com%2Frecipes~1410882199146%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers~1410882256278%7Chttp%3A%2F%2Fwww.food.com%2Frecipes~1410882436439%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Fpn%3D2~1410884133446%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Fpn%3D1~1410884142703%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Fpn%3D7~1410884265881%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Fpn%3D1~1410884313793%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers~1410884318342%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Flayout%3Ddesktop%26pn%3D1~1410884368155%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Fpn%3D20~1410884381430%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Flayout%3Ddesktop%26pn%3D1~1410884391236%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fappetizers%2F~1410884405738%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers%3Flayout%3Ddesktop%26pn%3D1%3Fpn%3D3~1410884418087%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fappetizers~1410884430760%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fappetizers~1410884443012%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fgerman~1410884468650%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Flayout%3Ddesktop~1410884481673%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Fpn%3D20~1410884499148%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Flayout%3Ddesktop%26pn%3D1~1410884514115%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Flayout%3Ddesktop~1410884541640%7Chttp%3A%2F%2Fwww.food.com%2Frecipes%2Fall~1410884584297%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Flayout%3Ddesktop~1410884589124%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall~1410884595106%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Fpn%3D5~1410885479654%7Chttp%3A%2F%2Fwww.food.com%2Frecipe%2Fbuffalo-chicken-dip-79116~1410885722840%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall~1410886889957%7Chttp%3A%2F%2Fwww.food.com%2Frecipe%2Fbourbon-chicken-45809~1410887050120%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall~1410888685976%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Fpn%3D2~1410888692799%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Fpn%3D1~1410888697254%7Chttp%3A%2F%2Fwww.food.com%2F~1410890734032%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall%3Fpn%3D21~1410890904298%7Chttp%3A%2F%2Fwww.food.com%2F~1410893925594%7Chttp%3A%2F%2Fwww.food.com%2Frecipes~1410894038573%7Chttp%3A%2F%2Fwww.food.com%2Frecipe-finder%2Fall~1410895815550");
//                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36";

//                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
//                    if (response.StatusCode == HttpStatusCode.OK)
//                    {
//                        StreamReader reader = new StreamReader(response.GetResponseStream());
//                        html = reader.ReadToEnd();
//                    }

//                    response.Close();
//                    break;
//                }
//                catch
//                {
//                    System.Threading.Thread.Sleep(60000);
//                    tries--;
//                }
//            }

//            if (string.IsNullOrEmpty(html) && tries == 0) throw new Exception("ERROR!");

//            return html;
//        }

//        /*
//        public static void Crawl(DataFactory df, int startPage = 1)
//        {
//            string html = getHTML("http://www.food.com/recipe-finder/all");

//            MatchCollection mc = Regex.Matches(html, "\\?pn=(\\d*)");
//            double maxPage = (from m in mc.OfType<Match>()
//                              select double.Parse(m.Groups[1].Value)).Max();

//            WebClient wc = new WebClient();
//            string src = "food.com";
//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;

//            List<string> recURLs = df.GetDataTable("select RecipeURL from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//                .Select(r => r["RecipeURL"].ToString()).ToList();

//            for (int i = startPage; i <= maxPage; i++)
//            {
//                html = getHTML("http://www.food.com/recipe-finder/all?pn=" + i.ToString());
//                mc = Regex.Matches(html, "<a class=\"recipe-main-title\" href=\"(.*?)\" .*?>(.*?)</a>");
//                foreach (Match m in mc.OfType<Match>())
//                {
//                    string recipeName = m.Groups[2].Value;


//                    if (recURLs.Contains(m.Groups[1].Value))
//                    {
//                        Console.WriteLine("Page: " + i.ToString() + " of " + maxPage.ToString() + " Recipe: " + recipeName);
//                        continue;
//                    }

//                    Console.WriteLine("* Page: " + i.ToString() + " of " + maxPage.ToString() + " Recipe: " + recipeName);
//                    int tries = 3;
//                    while (tries > 0)
//                    {
//                        try
//                        {
//                            html = wc.DownloadString(m.Groups[1].Value);
//                            break;
//                        }
//                        catch
//                        {
//                            System.Threading.Thread.Sleep(1000);
//                            tries--;
//                        }
//                    }

//                    if (tries == 0)
//                        continue;


//                    Recipe rec = df.CreateDataItem<Recipe>();
//                    rec.RecipeName = recipeName;
//                    rec.RecipeSourceID = srcID;
//                    rec.RecipeURL = m.Groups[1].Value;

//                    File.AppendAllText("Food.txt", "\r\nPage: " + i.ToString() + " of " + maxPage.ToString() + " Recipe: " + rec.RecipeName);

//                    SetImages(rec, html);

//                    Match m3 = Regex.Match(html, "<p>Servings Per Recipe: (.*?)</p>");
//                    if (m3.Success)
//                    {
//                        int servings = 0;
//                        if (int.TryParse(m3.Groups[1].Value.Trim(), out servings))
//                            rec.NumberOfServings = servings;
//                    }

//                    SetRating(rec, html);

//                    m3 = Regex.Match(html, "<span class=\"instructions\"  itemprop=\"recipeInstructions\">(.*?)</span>", RegexOptions.Singleline);
//                    rec.Directions = PaJaMa.Common.Common.StripHTML(m3.Groups[1].Value).Trim();

//                    MatchCollection mc2 = Regex.Matches(html, "<li class=\"ingredient\"  itemprop=\"ingredients\">(.*?)</li>", RegexOptions.Singleline);

//                    List<RecipeIngredientMeasurement> recIngs = new List<RecipeIngredientMeasurement>();
//                    foreach (Match m4 in mc2)
//                    {
//                        double qty = 0;
//                        m3 = Regex.Match(m4.Groups[1].Value, "<span class=\"value\">(.*?)</span>");
//                        if (m3.Success)
//                        {
//                            string qtyString = m3.Groups[1].Value.Trim();
//                            if (qtyString.Contains("-"))
//                            {
//                                string[] parts = qtyString.Split('-');
//                                double qty1 = 0;
//                                PaJaMa.Common.Common.TryParseFraction(parts[0], out qty1);
//                                double qty2 = 0;
//                                PaJaMa.Common.Common.TryParseFraction(parts[1], out qty2);
//                                qty = (qty2 + qty1) / 2;
//                            }
//                            else
//                                PaJaMa.Common.Common.TryParseFraction(qtyString, out qty);
//                        }

//                        Measurement measurement = null;
//                        m3 = Regex.Match(m4.Groups[1].Value, "<span class=\"type\">(.*?)</span>");
//                        if (m3.Success)
//                        {
//                            MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                            mf.MeasurementName = m3.Groups[1].Value.Trim();
//                            if (!string.IsNullOrEmpty(mf.MeasurementName))
//                            {
//                                measurement = mf.GetDataItems().FirstOrDefault();
//                                if (measurement == null)
//                                {
//                                    measurement = df.CreateDataItem<Measurement>();
//                                    measurement.MeasurementName = m3.Groups[1].Value;
//                                    measurement.Save();
//                                }
//                            }
//                        }

//                        m3 = Regex.Match(m4.Groups[1].Value, "<span class=\"name\">(.*?)</span>", RegexOptions.Singleline);
//                        string ingredient = PaJaMa.Common.Common.StripHTML(m3.Groups[1].Value).Trim();

//                        string[] lines = ingredient.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
//                        ingredient = string.Empty;
//                        foreach (string line in lines)
//                        {
//                            ingredient += line.Trim() + " ";
//                        }
//                        ingredient = ingredient.Trim();

//                        CrawlerHelper.FillIngredient(rec, ingredient, measurement, qty);
//                    }

//                    if (rec.RecipeRecipeIngredientMeasurements.Any())
//                        CrawlerHelper.SaveRecipe(rec);

//                    //if (!string.IsNullOrEmpty(rec.ImageURL))
//                    //{
//                    //	string imgPath = @"\\pjserver\e\HTTP\RecipesAPI\Images\Recipes";
//                    //	string extension = Path.GetExtension(rec.ImageURL);
//                    //	extension = extension.Replace("jpeg", "jpg");
//                    //	string path = Path.Combine(imgPath, rec.RecipeID.ToString() + extension);
//                    //	if (!File.Exists(path))
//                    //		wc.DownloadFile(rec.ImageURL, path);
//                    //}

//                    System.Threading.Thread.Sleep(10);
//                }
//            }
//        }

//        public static void SetRating(Recipe rec, string html)
//        {
//            Match m3 = Regex.Match(html, "<li class=\"current-rating\" style=\"width: (.*?)%;\"></li>");
//            if (m3.Success)
//                rec.Rating = (float)(double.Parse(m3.Groups[1].Value) / 20);
//        }

//        public static void SetImages(Recipe rec, string html)
//        {
//            if (html == null)
//                html = getHTML(rec.RecipeURL);
//            Match m3;
//            try
//            {
//                m3 = Regex.Match(html, "<div id=\"recipe-photo-gallery-list\"(.*?)</div>", RegexOptions.Singleline);
//                if (m3.Success)
//                {
//                    MatchCollection mc = Regex.Matches(m3.Groups[1].Value, "src=\"(.*?)\"");
//                    foreach (Match m in mc)
//                    {
//                        string imgURL = m.Groups[1].Value;
//                        if (imgURL.Contains("/w_98,h_73"))
//                        {
//                            imgURL = imgURL.Replace("/w_98,h_73", "");
//                        }
//                        else if (imgURL.Contains("&width=98&height=73"))
//                        {
//                            imgURL = imgURL.Replace("&width=98&height=73", "");
//                        }
//                        else
//                        {

//                        }

//                        int index = imgURL.IndexOf("/convert?");
//                        if (index != -1)
//                            imgURL = imgURL.Substring(0, index + 8);

//                        index = imgURL.IndexOf("?loc=");
//                        if (index != -1)
//                            imgURL = imgURL.Substring(0, index);

//                        CrawlerHelper.AddImage(rec, imgURL);
//                    }
//                }
//                else
//                {
//                    m3 = Regex.Match(html, "<img id=\".*?\" class=\".*?\" itemprop=\"image\" src=\"(.*?)\" title=");
//                    if (m3.Success)
//                    {
//                        RecipeImage rim = CrawlerHelper.AddImage(rec, m3.Groups[1].Value);
//                        if (rim != null && rim.ImageURL.Length > 255)
//                        {
//                            int index = rim.ImageURL.IndexOf("?loc=");
//                            if (index != -1)
//                                rim.ImageURL = rim.ImageURL.Substring(0, index) + "?width=266";
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                m3 = Regex.Match(html, "<img id=\".*?\" class=\".*?\" itemprop=\"image\" src=\"(.*?)\" title=");
//                if (m3.Success)
//                {
//                    RecipeImage rim = CrawlerHelper.AddImage(rec, m3.Groups[1].Value);
//                    if (rim != null && rim.ImageURL.Length > 255)
//                    {
//                        int index = rim.ImageURL.IndexOf("?loc=");
//                        if (index != -1)
//                            rim.ImageURL = rim.ImageURL.Substring(0, index) + "?width=266";
//                    }
//                }
//            }
//        }

//        */
//    }
//}
