// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.License.LicenseHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Search;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using wyDay.TurboActivate;

namespace FreeYouTubeDownloader.License
{
    internal static class LicenseHelper
    {
        private static readonly Regex NameEncodingRegex = new Regex("(?:\\d{1,5})");
        private const string GetPKeyIdUrlFomat = "https://wyday.com/limelm/api/rest/?method=limelm.pkey.getID&api_key=1jjnpq958716cfa419776.95091326&format=json&pkey={0}";
        private const string GetPKeyDetailsUrlFormat = "https://wyday.com/limelm/api/rest/?method=limelm.pkey.getDetails&api_key=1jjnpq958716cfa419776.95091326&format=json&pkey_id={0}";
        //internal static wyDay.TurboActivate.TurboActivate TurboActivate;
        internal static IsGenuineResult GenuineResult;
        internal static DateTime? KeyExpirationDate;

        internal static bool IsGenuine
        {
            get
            {
                //Log.Trace("GET LicenseHelper.IsGenuine", (Exception)null);
                //if (LicenseHelper.TurboActivate == null)
                //    return false;
                //bool flag = (LicenseHelper.GenuineResult == IsGenuineResult.Genuine || LicenseHelper.GenuineResult == IsGenuineResult.GenuineFeaturesChanged || LicenseHelper.GenuineResult == IsGenuineResult.InternetError) && LicenseHelper.TurboActivate.IsActivated();
                //if (flag && LicenseHelper.IsExpired)
                //    flag = false;
                //return flag;
                return true;
            }
        }

        internal static string LicensedTo { get; private set; }

        internal static bool IsExpired
        {
            get
            {
                //if (!LicenseHelper.KeyExpirationDate.HasValue)
                //    return false;
                //DateTime now = DateTime.Now;
                //DateTime? keyExpirationDate = LicenseHelper.KeyExpirationDate;
                //if (!keyExpirationDate.HasValue)
                //    return false;
                //return now > keyExpirationDate.GetValueOrDefault();
                return false;
            }
        }

        internal static void Init()
        {
            try
            {
                //LicenseHelper.TurboActivate = new wyDay.TurboActivate.TurboActivate("267a051258716db7f24653.29117985", (string)null);
                //LicenseHelper.CheckGenuineness();
            }
            catch (TurboActivateException ex)
            {
                Log.Warning("Failed to check if activated: " + ex.Message, (Exception)null);
            }
        }

        internal static void CheckGenuineness()
        {
            //LicenseHelper.GenuineResult = LicenseHelper.TurboActivate.IsGenuine();
            try
            {
                //LicenseHelper.LicensedTo = LicenseHelper.DecodeName(LicenseHelper.TurboActivate.GetFeatureValue("name"));
                //LicenseHelper.KeyExpirationDate = new DateTime?(DateTime.Parse(LicenseHelper.TurboActivate.GetFeatureValue("expires"), (IFormatProvider)CultureInfo.InvariantCulture));
            }
            catch (TurboActivateException ex)
            {
                Log.Error(ex.Message, (Exception)ex);
            }
        }

        internal static string GetAppTitle(bool includeLicenseHolder = false)
        {
            if (!LicenseHelper.IsGenuine)
                return "Free YouTube Downloader";
            string str = "Pro YouTube Downloader";
            if (includeLicenseHolder)
                return str + string.Format(" - {0} {1}", (object)Strings.RegisteredTo, (object)LicenseHelper.LicensedTo);
            return str;
        }

        private static string DecodeName(string encodedName)
        {
            MatchCollection matchCollection = LicenseHelper.NameEncodingRegex.Matches(encodedName);
            if (matchCollection.Count < 3)
                return encodedName;
            StringBuilder stringBuilder = new StringBuilder(encodedName.Length + matchCollection.Count * 3);
            foreach (object obj in matchCollection)
                stringBuilder.AppendFormat("&#{0};", obj);
            return WebUtility.HtmlDecode(stringBuilder.ToString());
        }

        public static string GetProductKeyId(string pKey)
        {
            string str = HttpClientEx.GetString(string.Format("https://wyday.com/limelm/api/rest/?method=limelm.pkey.getID&api_key=1jjnpq958716cfa419776.95091326&format=json&pkey={0}", (object)pKey));
            if (string.IsNullOrEmpty(str))
                throw new WebException("No data received");
            char[] chArray = new char[1] { ')' };
            return JObject.Parse(str.TrimEnd(chArray).TrimStart("jsonLimeLMApi(".ToCharArray()))["pkey"][(object)"id"].Value<string>();
        }

        private static string GetProductKeyDetails(string pKeyId)
        {
            string str = HttpClientEx.GetString(string.Format("https://wyday.com/limelm/api/rest/?method=limelm.pkey.getDetails&api_key=1jjnpq958716cfa419776.95091326&format=json&pkey_id={0}", (object)pKeyId));
            if (string.IsNullOrEmpty(str))
                throw new WebException("No data received");
            char[] chArray = new char[1] { ')' };
            return str.TrimEnd(chArray).TrimStart("jsonLimeLMApi(".ToCharArray());
        }

        public static bool IsProductKeyRevoked(string pKey)
        {
            JObject jobject = JObject.Parse(LicenseHelper.GetProductKeyDetails(LicenseHelper.GetProductKeyId(pKey)));
            if (jobject["pkey"][(object)"revoked"] != null)
                return jobject["pkey"][(object)"revoked"].Value<bool>();
            return false;
        }

        public static bool IsProductKeyRevoked()
        {
            //try
            //{
            //    return LicenseHelper.IsProductKeyRevoked(LicenseHelper.TurboActivate.GetPKey());
            //}
            //catch
            {
                return false;
            }
        }
    }
}
