// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Helpers.ReportingHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common;
using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Helpers
{
    public class ReportingHelper
    {
        private static readonly MigraDoc.DocumentObjectModel.Color TableBorder = new MigraDoc.DocumentObjectModel.Color(byte.MaxValue, (byte)127, (byte)127);
        private static readonly MigraDoc.DocumentObjectModel.Color TableHeader = new MigraDoc.DocumentObjectModel.Color(byte.MaxValue, (byte)110, (byte)85);
        private static Dictionary<string, string> _operatingSystemInfo;
        private static TextMeasurement _tm;
        private static Table _table;
        private const string BodyTemplate = "<html><head><meta charset='utf-8' /><title></title></head><body style='font-size: 15px;'><div><img src='https://d3szoh0f46td6t.cloudfront.net/public/40431162/small_bounded' alt='' /></div><br/><br/><div><span>Dear administrator,</span><p>We got the message about the new error that occurred in the <span style='font-weight: bold;'>{0} ver. {1}</span>.</p><br /><br /><table style='width: 100%; border: 1px solid grey;'><tr><td style='width: 200px; font-weight: bold;'>Application name</td><td>{2}</td></tr><tr><td style='width: 200px; font-weight: bold;'>Version</td><td>{3}</td></tr><tr><td style='width: 200px; font-weight: bold; vertical-align: top;'>Folder name</td><td>{4}</td></tr>{5}<tr><td style='width: 200px; font-weight: bold;'>Date</td><td>{6}</td></tr><tr><td style='width: 200px; font-weight: bold;'>Time</td><td>{7}</td></tr><tr><td style='width: 200px; font-weight: bold;'>Short description</td><td>{8}</td></tr>{9}{10}</table></div><br /><br /><br /><br /><div id='mail-footer'>Please don't reply to this email.<br /><br />*** Mail Delivery System  ***</div></body></html>";

        public static Dictionary<string, string> OperatingSystemInfo
        {
            get
            {
                if (ReportingHelper._operatingSystemInfo == null || ReportingHelper._operatingSystemInfo.Count == 0)
                    ReportingHelper.UpdateOperatingSystemInfo();
                return ReportingHelper._operatingSystemInfo;
            }
        }

        public static void UpdateOperatingSystemInfo()
        {
            if (ReportingHelper._operatingSystemInfo == null)
                ReportingHelper._operatingSystemInfo = new Dictionary<string, string>();
            ReportingHelper._operatingSystemInfo.Clear();
            try
            {
                int num = 0;
                ManagementScope scope = new ManagementScope();
                ObjectQuery query1 = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection objectCollection = new ManagementObjectSearcher(scope, query1).Get();
                string key1 = "User is an admin";
                string str1 = ReportingHelper.IsUserAdministrator() ? "Yes" : "No";
                ReportingHelper._operatingSystemInfo.Add(key1, str1);
                foreach (ManagementBaseObject managementBaseObject in objectCollection)
                {
                    string key2 = "Operating system";
                    string index1 = "Caption";
                    string str2 = managementBaseObject[index1].ToString();
                    ReportingHelper._operatingSystemInfo.Add(key2, str2);
                    string key3 = "Version";
                    string index2 = "Version";
                    string str3 = managementBaseObject[index2].ToString();
                    ReportingHelper._operatingSystemInfo.Add(key3, str3);
                    string key4 = "Windows directory";
                    string index3 = "WindowsDirectory";
                    string str4 = managementBaseObject[index3].ToString();
                    ReportingHelper._operatingSystemInfo.Add(key4, str4);
                }
                List<string> frameworkVersions = ReportingHelper.GetInstalledNetFrameworkVersions();
                string key5 = ".NET Framework";
                string str5 = string.Join("; ", (IEnumerable<string>)frameworkVersions);
                ReportingHelper._operatingSystemInfo.Add(key5, str5);
                string key6 = "CLR";
                string str6 = Environment.Version.ToString();
                ReportingHelper._operatingSystemInfo.Add(key6, str6);
                ObjectQuery query2 = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(scope, query2).Get())
                {
                    string key2 = "System type";
                    string index1 = "SystemType";
                    string str2 = managementBaseObject[index1].ToString();
                    ReportingHelper._operatingSystemInfo.Add(key2, str2);
                    string key3 = "Total physical memory";
                    string index2 = "totalphysicalmemory";
                    string fileSize = ((ulong)managementBaseObject[index2]).ToFileSize(true);
                    ReportingHelper._operatingSystemInfo.Add(key3, fileSize);
                }
                ObjectQuery query3 = new ObjectQuery("SELECT * FROM Win32_timezone");
                foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(scope, query3).Get())
                {
                    string key2 = "Time zone";
                    string index = "Caption";
                    string str2 = managementBaseObject[index].ToString();
                    ReportingHelper._operatingSystemInfo.Add(key2, str2);
                }
                foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("\\\\" + Environment.MachineName + "\\root\\SecurityCenter2", "SELECT * FROM AntivirusProduct").Get())
                {
                    string key2 = "AV Product " + (object)++num;
                    string propertyName = "displayName";
                    string str2 = managementBaseObject.GetPropertyValue(propertyName).ToString();
                    ReportingHelper._operatingSystemInfo.Add(key2, str2);
                }
                IPPacketInformation packetInformation = new IPPacketInformation();
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                string key7 = "Network interface";
                int index4 = packetInformation.Interface;
                string description = networkInterfaces[index4].Description;
                ReportingHelper._operatingSystemInfo.Add(key7, description);
                string key8 = "Network type";
                int index5 = packetInformation.Interface;
                string str7 = networkInterfaces[index5].NetworkInterfaceType.ToString();
                ReportingHelper._operatingSystemInfo.Add(key8, str7);
            }
            catch
            {
            }
        }

        private static bool IsUserAdministrator()
        {
            try
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static List<string> GetInstalledNetFrameworkVersions()
        {
            List<string> stringList = new List<string>();
            using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\"))
            {
                foreach (string subKeyName1 in registryKey1.GetSubKeyNames())
                {
                    if (subKeyName1.StartsWith("v"))
                    {
                        RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName1);
                        string str1 = (string)registryKey2.GetValue("Version", (object)string.Empty);
                        string str2 = registryKey2.GetValue("SP", (object)string.Empty).ToString();
                        string str3 = registryKey2.GetValue("Install", (object)"").ToString();
                        if (str3 == string.Empty)
                        {
                            string str4 = string.Format("{0} {1}", (object)subKeyName1, (object)str1);
                            stringList.Add(str4.Trim());
                        }
                        else if (str2 != string.Empty && str3 == "1")
                        {
                            string str4 = string.Format("{0} {1} SP{2}", (object)subKeyName1, (object)str1, (object)str2);
                            stringList.Add(str4.Trim());
                        }
                        if (!(str1 != string.Empty))
                        {
                            foreach (string subKeyName2 in registryKey2.GetSubKeyNames())
                            {
                                RegistryKey registryKey3 = registryKey2.OpenSubKey(subKeyName2);
                                string str4 = (string)registryKey3.GetValue("Version", (object)string.Empty);
                                if (str4 != string.Empty)
                                    str2 = registryKey3.GetValue("SP", (object)string.Empty).ToString();
                                string str5 = registryKey3.GetValue("Install", (object)string.Empty).ToString();
                                if (str5 == string.Empty)
                                {
                                    string str6 = string.Format("{0} {1}", (object)subKeyName1, (object)str4);
                                    stringList.Add(str6.Trim());
                                }
                                else if (str2 != string.Empty && str5 == "1")
                                {
                                    string str6 = string.Format(" {0} {1} SP{2}", (object)subKeyName2, (object)str4, (object)str2);
                                    stringList.Add(str6.Trim());
                                }
                                else if (str5 == "1")
                                {
                                    string str6 = string.Format(" {0} {1}", (object)subKeyName2, (object)str4);
                                    stringList.Add(str6.Trim());
                                }
                            }
                        }
                    }
                }
            }
            return stringList;
        }

        private static string GetApplicationDirectoryPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static string CreateHtmlRowsFromSettings()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string str1 = "<tr><td colspan='2' style='font-weight: bold; text_align: center;'>Settings</td></tr>";
            stringBuilder.AppendLine(str1);
            string str2 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"MaxSimultaneousDownloads", (object)Settings.Instance.MaxSimultaneousDownloads);
            stringBuilder.AppendLine(str2);
            string str3 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"LanguageCode", (object)Settings.Instance.LanguageCode);
            stringBuilder.AppendLine(str3);
            string str4 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"WindowWidth", (object)Settings.Instance.WindowWidth);
            stringBuilder.AppendLine(str4);
            string str5 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"WindowHeight", (object)Settings.Instance.WindowHeight);
            stringBuilder.AppendLine(str5);
            string str6 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"PreferedMediaFormat", (object)Settings.Instance.PreferedMediaFormat);
            stringBuilder.AppendLine(str6);
            string str7 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"AllowSimultaneousDownloads", (object)Settings.Instance.AllowSimultaneousDownloads);
            stringBuilder.AppendLine(str7);
            string str8 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"AlwaysOnTop", (object)Settings.Instance.AlwaysOnTop);
            stringBuilder.AppendLine(str8);
            string str9 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"DefaultVideosDownloadFolder", (object)Settings.Instance.DefaultVideosDownloadFolder);
            stringBuilder.AppendLine(str9);
            string str10 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"DefaultAudiosDownloadFolder", (object)Settings.Instance.DefaultAudiosDownloadFolder);
            stringBuilder.AppendLine(str10);
            string str11 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"FileLocationOption", (object)Settings.Instance.FileLocationOption);
            stringBuilder.AppendLine(str11);
            string str12 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"AllowSounds", (object)Settings.Instance.AllowSounds);
            stringBuilder.AppendLine(str12);
            string str13 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"AllowBalloons", (object)Settings.Instance.AllowBalloons);
            stringBuilder.AppendLine(str13);
            string str14 = string.Format("<tr><td style='width: 200px;'></td><td>{0}: {1}</td></tr>", (object)"RemoveAllFinishedFiles", (object)Settings.Instance.RemoveAllFinishedFiles);
            stringBuilder.AppendLine(str14);
            string str15 = "<tr><td colspan='2' style='font-weight: bold;'> </td></tr>";
            stringBuilder.AppendLine(str15);
            return stringBuilder.ToString();
        }

        private static string CreateHtmlRowsFromSystemInfo(Dictionary<string, string> systemInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in systemInfo)
                stringBuilder.AppendLine(string.Format("<tr><td style='width: 200px; font-weight: bold;'>{0}</td><td>{1}</td></tr>", (object)keyValuePair.Key, (object)keyValuePair.Value));
            return stringBuilder.ToString();
        }

        private static void DefineStyles(Document document)
        {
            document.DefaultPageSetup.TopMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.BottomMargin = Unit.FromInch(0.6);
            document.DefaultPageSetup.LeftMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.RightMargin = Unit.FromInch(0.5);
            document.Styles["Normal"].Font.Name = "Verdana";
            ReportingHelper._tm = new TextMeasurement(document.Styles["Normal"].Font.Clone());
            Style style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = (Unit)9;
        }

        private static void CreatePage(Document document)
        {
            ReportingHelper._table = document.AddSection().AddTable();
            ReportingHelper._table.Style = "Table";
            ReportingHelper._table.Borders.Color = ReportingHelper.TableBorder;
            ReportingHelper._table.Borders.Width = (Unit)0.25;
            ReportingHelper._table.Borders.Left.Width = (Unit)0.5;
            ReportingHelper._table.Borders.Right.Width = (Unit)0.5;
            ReportingHelper._table.Rows.LeftIndent = (Unit)0;
            ReportingHelper._table.AddColumn((Unit)"5cm").Format.Alignment = ParagraphAlignment.Left;
            ReportingHelper._table.AddColumn((Unit)"13.2cm").Format.Alignment = ParagraphAlignment.Left;
        }

        private static string AdjustIfTooWideToFitIn(Cell cell, string text)
        {
            Column column = cell.Column;
            Unit availableWidth = (Unit)((float)column.Width - (float)column.Table.Borders.Width - (float)cell.Borders.Width);
            IEnumerable<string> strings = ((IEnumerable<string>)text.Split(" ".ToCharArray())).Distinct<string>().Where<string>((Func<string, bool>)(s => ReportingHelper.TooWide(s, availableWidth)));
            StringBuilder stringBuilder = new StringBuilder(text);
            foreach (string str in strings)
            {
                string newValue = ReportingHelper.MakeFit(str, availableWidth);
                stringBuilder.Replace(str, newValue);
            }
            return stringBuilder.ToString();
        }

        private static bool TooWide(string word, Unit width)
        {
            return (double)ReportingHelper._tm.MeasureString(word, UnitType.Point).Width > width.Point;
        }

        private static string MakeFit(string word, Unit width)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string empty = string.Empty;
            foreach (char ch in word)
            {
                if (ReportingHelper.TooWide(empty + ch.ToString(), width))
                {
                    stringBuilder.Append(empty);
                    stringBuilder.Append('\r');
                    empty = ch.ToString();
                }
                else
                    empty += ch.ToString();
            }
            stringBuilder.Append(empty);
            return stringBuilder.ToString();
        }
    }
}
