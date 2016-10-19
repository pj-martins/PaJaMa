//using Newtonsoft.Json.Linq;
//using PaJaMa.Recipes.Model.Dto;
//using PaJaMa.Recipes.Model.Entities;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Web.Http;

//namespace PaJaMa.Recipes.Web.Api.Controllers
//{
//	[RoutePrefix("api/image")]
//#if DEBUG
//	[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
//#endif
//	public class ImageController : ApiController
//	{
//		public ImageController()
//		{
//		}

//		[Route("{id:int}")]
//		public HttpResponseMessage Get(int id)
//		{
//			string image = RecipeDtoHelper.GetRecipeImagePath(new RecipesContext(), id);
//			if (image.ToLower().StartsWith("http:"))
//			{
//				try
//				{
//					return getRemoteImage(image);
//				}
//				catch { }
//			}

//			return Get(image);
//		}

//		[Route("{img}")]
//		public HttpResponseMessage Get(string img)
//		{
//			string imagePath = Path.Combine(ConfigurationManager.AppSettings["LocalRecipeImagePath"], img);
//			return getImage(imagePath);
//		}

//		[Route]
//		[HttpPost]
//		public Base64ImageDto Post(Base64ImageDto img)
//		{
//			byte[] data = new WebClient().DownloadData(img.ImageURL);
//			Bitmap bmp = null;
//			Bitmap bmpClone = null;
//			try
//			{
//				bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(data));
//				if (bmp.Width != bmp.Height)
//				{
//					bmpClone = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
//					int marginLeftRight = (bmp.Width > bmp.Height ? 0 : (bmp.Height - bmp.Width) / 2);
//					int marginTopBottom = (bmp.Height > bmp.Width ? 0 : (bmp.Width - bmp.Height) / 2);

//					int max = Math.Max(bmp.Width, bmp.Height);
//					bmp = new Bitmap(max, max);

//					using (Graphics g = Graphics.FromImage(bmp))
//					{
//						g.Clear(Color.Black);
//						g.DrawImageUnscaled(bmpClone, marginLeftRight, marginTopBottom, bmpClone.Width, bmpClone.Height);
//					}

//					using (MemoryStream ms = new MemoryStream())
//					{
//						bmp.Save(ms, bmpClone.RawFormat);
//						data = ms.ToArray();
//					}
//				}
//			}
//			finally
//			{
//				if (bmp != null) bmp.Dispose();
//				if (bmpClone != null) bmpClone.Dispose();
//			}

//			img.Base64String = Convert.ToBase64String(data);
//			img.ContentType = "image/" + (img.ImageURL.EndsWith("png") ? "png" : "jpeg");
//			return img;
//		}

//		private static HttpResponseMessage getRemoteImage(string imageURL)
//		{
//			byte[] data = new WebClient().DownloadData(imageURL);
//			var response = new HttpResponseMessage();
//			response.Content = new ByteArrayContent(data);
//			response.Content.Headers.ContentType = new MediaTypeHeaderValue(imageURL.EndsWith("png") ? "image/png" : "image/jpeg");
//			return response;
//		}

//		private HttpResponseMessage getImage(string imagePath)
//		{
//			if (string.IsNullOrEmpty(imagePath))
//				imagePath = System.Web.HttpContext.Current.Server.MapPath(@"~/Images/food.png");

//			var response = new HttpResponseMessage();
//			using (MemoryStream stream = new MemoryStream())
//			{
//				using (Image img = Bitmap.FromFile(imagePath))
//				{
//					img.Save(stream, imagePath.EndsWith("png") ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
//					response.Content = new ByteArrayContent(stream.ToArray());
//					response.Content.Headers.ContentType = new MediaTypeHeaderValue(imagePath.EndsWith("png") ? "image/png" : "image/jpeg");
//				}
//			}
//			return response;
//		}
//	}
//}
