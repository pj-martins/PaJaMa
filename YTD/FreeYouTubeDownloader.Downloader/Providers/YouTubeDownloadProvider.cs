// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Providers.YouTubeDownloadProvider
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Localization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FreeYouTubeDownloader.Downloader.Providers
{
    public class YouTubeDownloadProvider : DownloadProvider, IVideoInfoAnalyzer, IGetVideoId
    {
        private static readonly string[] TicketExpressions = new string[2]
        {
      "(?:^|&amp;|\")url_encoded_fmt_stream_map=([^&]+)",
      "(?:^|amp;|\")url_encoded_fmt_stream_map=([^(&|\\\\)]+)"
        };
        private readonly Regex _dashManifestEncryptedSignatureRegex = new Regex("/s/([\\w\\.]+)", RegexOptions.Singleline);
        private readonly Regex _thumbnailsRegex = new Regex("{\\\\\"thumbnails\\\\\":\\[.*?\\]}", RegexOptions.Singleline);
        private const string URL_PATTERN = "^(?:https?://)?(?:youtu\\.be/|(?:\\w+\\.)?(?:youtube\\.com/(?:watch|watch_popup|shared)?\\?(?:.+&)?(v|ci)=|youtube\\.com/(?:v|e|embed)/))([A-Za-z0-9_-]+)";
        private const string AGE_MESSAGE_EXPRESSIONS = "<div id=\"verify-age-actions\">(?<text>[\\w\\W]*?)</div>";
        private const string ProviderUrlFormat = "https://api.videoplex.com/getVideo?url={0}&token={1}&optimized=true&excludeFields=quality,videoType,bitrate,resolution,qualityDescription,videoOnly,audioOnly,duration,metaData,uploadDate,type,websiteLogos,isAdult,website";
        private const string GetVideoInfoUrlPattern = "https://www.youtube.com/get_video_info?video_id={0}&el=embedded&gl=US&hl=en&eurl={1}&asv=3&sts={2}";
        private const string RegexVideoDimensions = "((?<width>\\d{2,5})[x|X](?<height>\\d{2,5}))";
        private string _url;
        private string _html5PlayerPath;
        private static readonly Dictionary<int, Itag> Itags;

        public override string UrlPattern
        {
            get
            {
                return "^(?:https?://)?(?:youtu\\.be/|(?:\\w+\\.)?(?:youtube\\.com/(?:watch|watch_popup|shared)?\\?(?:.+&)?(v|ci)=|youtube\\.com/(?:v|e|embed)/))([A-Za-z0-9_-]+)";
            }
        }

        static YouTubeDownloadProvider()
        {
            Dictionary<int, Itag> dictionary = new Dictionary<int, Itag>();
            int key1 = 5;
            VideoItag videoItag1 = new VideoItag();
            videoItag1.StreamType = VideoStreamType.Flv;
            videoItag1.Quality = VideoQuality._240p;
            int num1 = 400;
            videoItag1.Width = num1;
            int num2 = 240;
            videoItag1.Height = num2;
            dictionary.Add(key1, (Itag)videoItag1);
            int key2 = 6;
            VideoItag videoItag2 = new VideoItag();
            videoItag2.StreamType = VideoStreamType.Flv;
            videoItag2.Quality = VideoQuality._270p;
            int num3 = 450;
            videoItag2.Width = num3;
            int num4 = 270;
            videoItag2.Height = num4;
            dictionary.Add(key2, (Itag)videoItag2);
            int key3 = 17;
            VideoItag videoItag3 = new VideoItag();
            videoItag3.StreamType = VideoStreamType.ThreeGp;
            videoItag3.Quality = VideoQuality._144p;
            int num5 = 176;
            videoItag3.Width = num5;
            int num6 = 144;
            videoItag3.Height = num6;
            dictionary.Add(key3, (Itag)videoItag3);
            int key4 = 18;
            VideoItag videoItag4 = new VideoItag();
            videoItag4.StreamType = VideoStreamType.Mp4;
            videoItag4.Quality = VideoQuality._360p;
            int num7 = 640;
            videoItag4.Width = num7;
            int num8 = 360;
            videoItag4.Height = num8;
            dictionary.Add(key4, (Itag)videoItag4);
            int key5 = 22;
            VideoItag videoItag5 = new VideoItag();
            videoItag5.StreamType = VideoStreamType.Mp4;
            videoItag5.Quality = VideoQuality._720p;
            int num9 = 1280;
            videoItag5.Width = num9;
            int num10 = 720;
            videoItag5.Height = num10;
            dictionary.Add(key5, (Itag)videoItag5);
            int key6 = 34;
            VideoItag videoItag6 = new VideoItag();
            videoItag6.StreamType = VideoStreamType.Flv;
            videoItag6.Quality = VideoQuality._360p;
            int num11 = 640;
            videoItag6.Width = num11;
            int num12 = 360;
            videoItag6.Height = num12;
            dictionary.Add(key6, (Itag)videoItag6);
            int key7 = 35;
            VideoItag videoItag7 = new VideoItag();
            videoItag7.StreamType = VideoStreamType.Flv;
            videoItag7.Quality = VideoQuality._480p;
            int num13 = 854;
            videoItag7.Width = num13;
            int num14 = 480;
            videoItag7.Height = num14;
            dictionary.Add(key7, (Itag)videoItag7);
            int key8 = 36;
            VideoItag videoItag8 = new VideoItag();
            videoItag8.StreamType = VideoStreamType.ThreeGp;
            videoItag8.Quality = VideoQuality._240p;
            int num15 = 320;
            videoItag8.Width = num15;
            int num16 = 240;
            videoItag8.Height = num16;
            dictionary.Add(key8, (Itag)videoItag8);
            int key9 = 37;
            VideoItag videoItag9 = new VideoItag();
            videoItag9.StreamType = VideoStreamType.Mp4;
            videoItag9.Quality = VideoQuality._1080p;
            int num17 = 1920;
            videoItag9.Width = num17;
            int num18 = 1080;
            videoItag9.Height = num18;
            dictionary.Add(key9, (Itag)videoItag9);
            int key10 = 38;
            VideoItag videoItag10 = new VideoItag();
            videoItag10.StreamType = VideoStreamType.Mp4;
            videoItag10.Quality = VideoQuality._3072p;
            int num19 = 4096;
            videoItag10.Width = num19;
            int num20 = 3072;
            videoItag10.Height = num20;
            dictionary.Add(key10, (Itag)videoItag10);
            int key11 = 43;
            VideoItag videoItag11 = new VideoItag();
            videoItag11.StreamType = VideoStreamType.WebM;
            videoItag11.Quality = VideoQuality._360p;
            int num21 = 640;
            videoItag11.Width = num21;
            int num22 = 360;
            videoItag11.Height = num22;
            dictionary.Add(key11, (Itag)videoItag11);
            int key12 = 44;
            VideoItag videoItag12 = new VideoItag();
            videoItag12.StreamType = VideoStreamType.WebM;
            videoItag12.Quality = VideoQuality._480p;
            int num23 = 854;
            videoItag12.Width = num23;
            int num24 = 480;
            videoItag12.Height = num24;
            dictionary.Add(key12, (Itag)videoItag12);
            int key13 = 45;
            VideoItag videoItag13 = new VideoItag();
            videoItag13.StreamType = VideoStreamType.WebM;
            videoItag13.Quality = VideoQuality._720p;
            int num25 = 1280;
            videoItag13.Width = num25;
            int num26 = 720;
            videoItag13.Height = num26;
            dictionary.Add(key13, (Itag)videoItag13);
            int key14 = 46;
            VideoItag videoItag14 = new VideoItag();
            videoItag14.StreamType = VideoStreamType.WebM;
            videoItag14.Quality = VideoQuality._1080p;
            int num27 = 1920;
            videoItag14.Width = num27;
            int num28 = 1080;
            videoItag14.Height = num28;
            dictionary.Add(key14, (Itag)videoItag14);
            int key15 = 59;
            VideoItag videoItag15 = new VideoItag();
            videoItag15.StreamType = VideoStreamType.Mp4;
            videoItag15.Quality = VideoQuality._480p;
            int num29 = 854;
            videoItag15.Width = num29;
            int num30 = 480;
            videoItag15.Height = num30;
            string str1 = "AVC";
            videoItag15.Codec = str1;
            dictionary.Add(key15, (Itag)videoItag15);
            int key16 = 78;
            VideoItag videoItag16 = new VideoItag();
            videoItag16.StreamType = VideoStreamType.Mp4;
            videoItag16.Quality = VideoQuality._360p;
            int num31 = 640;
            videoItag16.Width = num31;
            int num32 = 360;
            videoItag16.Height = num32;
            dictionary.Add(key16, (Itag)videoItag16);
            int key17 = 82;
            VideoItag videoItag17 = new VideoItag();
            videoItag17.StreamType = VideoStreamType.Mp4;
            videoItag17.Quality = VideoQuality._360p;
            int num33 = 640;
            videoItag17.Width = num33;
            int num34 = 360;
            videoItag17.Height = num34;
            int num35 = 1;
            videoItag17.Is3D = num35 != 0;
            dictionary.Add(key17, (Itag)videoItag17);
            int key18 = 83;
            VideoItag videoItag18 = new VideoItag();
            videoItag18.StreamType = VideoStreamType.Mp4;
            videoItag18.Quality = VideoQuality._480p;
            int num36 = 854;
            videoItag18.Width = num36;
            int num37 = 480;
            videoItag18.Height = num37;
            int num38 = 1;
            videoItag18.Is3D = num38 != 0;
            dictionary.Add(key18, (Itag)videoItag18);
            int key19 = 84;
            VideoItag videoItag19 = new VideoItag();
            videoItag19.StreamType = VideoStreamType.Mp4;
            videoItag19.Quality = VideoQuality._720p;
            int num39 = 1280;
            videoItag19.Width = num39;
            int num40 = 720;
            videoItag19.Height = num40;
            int num41 = 1;
            videoItag19.Is3D = num41 != 0;
            dictionary.Add(key19, (Itag)videoItag19);
            int key20 = 85;
            VideoItag videoItag20 = new VideoItag();
            videoItag20.StreamType = VideoStreamType.Mp4;
            videoItag20.Quality = VideoQuality._1080p;
            int num42 = 1920;
            videoItag20.Width = num42;
            int num43 = 1080;
            videoItag20.Height = num43;
            int num44 = 1;
            videoItag20.Is3D = num44 != 0;
            dictionary.Add(key20, (Itag)videoItag20);
            int key21 = 91;
            VideoItag videoItag21 = new VideoItag();
            videoItag21.StreamType = VideoStreamType.Mp4;
            videoItag21.Quality = VideoQuality._144p;
            int num45 = 144;
            videoItag21.Height = num45;
            int num46 = 1;
            videoItag21.IsLive = num46 != 0;
            dictionary.Add(key21, (Itag)videoItag21);
            int key22 = 92;
            VideoItag videoItag22 = new VideoItag();
            videoItag22.StreamType = VideoStreamType.Mp4;
            videoItag22.Quality = VideoQuality._240p;
            int num47 = 426;
            videoItag22.Width = num47;
            int num48 = 240;
            videoItag22.Height = num48;
            int num49 = 1;
            videoItag22.IsLive = num49 != 0;
            dictionary.Add(key22, (Itag)videoItag22);
            int key23 = 93;
            VideoItag videoItag23 = new VideoItag();
            videoItag23.StreamType = VideoStreamType.Mp4;
            videoItag23.Quality = VideoQuality._360p;
            int num50 = 640;
            videoItag23.Width = num50;
            int num51 = 360;
            videoItag23.Height = num51;
            int num52 = 1;
            videoItag23.IsLive = num52 != 0;
            dictionary.Add(key23, (Itag)videoItag23);
            int key24 = 94;
            VideoItag videoItag24 = new VideoItag();
            videoItag24.StreamType = VideoStreamType.Mp4;
            videoItag24.Quality = VideoQuality._480p;
            int num53 = 854;
            videoItag24.Width = num53;
            int num54 = 480;
            videoItag24.Height = num54;
            int num55 = 1;
            videoItag24.IsLive = num55 != 0;
            dictionary.Add(key24, (Itag)videoItag24);
            int key25 = 95;
            VideoItag videoItag25 = new VideoItag();
            videoItag25.StreamType = VideoStreamType.Mp4;
            videoItag25.Quality = VideoQuality._720p;
            int num56 = 1280;
            videoItag25.Width = num56;
            int num57 = 720;
            videoItag25.Height = num57;
            int num58 = 1;
            videoItag25.IsLive = num58 != 0;
            dictionary.Add(key25, (Itag)videoItag25);
            int key26 = 96;
            VideoItag videoItag26 = new VideoItag();
            videoItag26.StreamType = VideoStreamType.Mp4;
            videoItag26.Quality = VideoQuality._1080p;
            int num59 = 1920;
            videoItag26.Width = num59;
            int num60 = 1080;
            videoItag26.Height = num60;
            int num61 = 1;
            videoItag26.IsLive = num61 != 0;
            dictionary.Add(key26, (Itag)videoItag26);
            int key27 = 100;
            VideoItag videoItag27 = new VideoItag();
            videoItag27.StreamType = VideoStreamType.WebM;
            videoItag27.Quality = VideoQuality._360p;
            int num62 = 640;
            videoItag27.Width = num62;
            int num63 = 360;
            videoItag27.Height = num63;
            int num64 = 1;
            videoItag27.Is3D = num64 != 0;
            dictionary.Add(key27, (Itag)videoItag27);
            int key28 = 101;
            VideoItag videoItag28 = new VideoItag();
            videoItag28.StreamType = VideoStreamType.WebM;
            videoItag28.Quality = VideoQuality._480p;
            int num65 = 854;
            videoItag28.Width = num65;
            int num66 = 480;
            videoItag28.Height = num66;
            int num67 = 1;
            videoItag28.Is3D = num67 != 0;
            dictionary.Add(key28, (Itag)videoItag28);
            int key29 = 102;
            VideoItag videoItag29 = new VideoItag();
            videoItag29.StreamType = VideoStreamType.WebM;
            videoItag29.Quality = VideoQuality._720p;
            int num68 = 1280;
            videoItag29.Width = num68;
            int num69 = 720;
            videoItag29.Height = num69;
            int num70 = 1;
            videoItag29.Is3D = num70 != 0;
            dictionary.Add(key29, (Itag)videoItag29);
            int key30 = 132;
            VideoItag videoItag30 = new VideoItag();
            videoItag30.StreamType = VideoStreamType.Mp4;
            videoItag30.Quality = VideoQuality._240p;
            int num71 = 426;
            videoItag30.Width = num71;
            int num72 = 240;
            videoItag30.Height = num72;
            int num73 = 1;
            videoItag30.IsLive = num73 != 0;
            dictionary.Add(key30, (Itag)videoItag30);
            int key31 = 133;
            VideoItag videoItag31 = new VideoItag();
            videoItag31.StreamType = VideoStreamType.Mp4;
            videoItag31.Quality = VideoQuality._240p;
            int num74 = 426;
            videoItag31.Width = num74;
            int num75 = 240;
            videoItag31.Height = num75;
            int num76 = 1;
            videoItag31.IsDash = num76 != 0;
            dictionary.Add(key31, (Itag)videoItag31);
            int key32 = 134;
            VideoItag videoItag32 = new VideoItag();
            videoItag32.StreamType = VideoStreamType.Mp4;
            videoItag32.Quality = VideoQuality._360p;
            int num77 = 640;
            videoItag32.Width = num77;
            int num78 = 360;
            videoItag32.Height = num78;
            int num79 = 1;
            videoItag32.IsDash = num79 != 0;
            dictionary.Add(key32, (Itag)videoItag32);
            int key33 = 135;
            VideoItag videoItag33 = new VideoItag();
            videoItag33.StreamType = VideoStreamType.Mp4;
            videoItag33.Quality = VideoQuality._480p;
            int num80 = 854;
            videoItag33.Width = num80;
            int num81 = 480;
            videoItag33.Height = num81;
            int num82 = 1;
            videoItag33.IsDash = num82 != 0;
            dictionary.Add(key33, (Itag)videoItag33);
            int key34 = 136;
            VideoItag videoItag34 = new VideoItag();
            videoItag34.StreamType = VideoStreamType.Mp4;
            videoItag34.Quality = VideoQuality._720p;
            int num83 = 1280;
            videoItag34.Width = num83;
            int num84 = 720;
            videoItag34.Height = num84;
            int num85 = 1;
            videoItag34.IsDash = num85 != 0;
            dictionary.Add(key34, (Itag)videoItag34);
            int key35 = 137;
            VideoItag videoItag35 = new VideoItag();
            videoItag35.StreamType = VideoStreamType.Mp4;
            videoItag35.Quality = VideoQuality._1080p;
            int num86 = 1920;
            videoItag35.Width = num86;
            int num87 = 1080;
            videoItag35.Height = num87;
            int num88 = 1;
            videoItag35.IsDash = num88 != 0;
            dictionary.Add(key35, (Itag)videoItag35);
            int key36 = 138;
            dictionary.Add(key36, (Itag)new VideoItag()
            {
                StreamType = VideoStreamType.Mp4,
                Quality = VideoQuality._2160p,
                IsDash = true
            });
            int key37 = 139;
            AudioItag audioItag1 = new AudioItag();
            audioItag1.StreamType = AudioStreamType.M4A;
            audioItag1.Quality = AudioQuality._48kbps;
            string str2 = "aac";
            audioItag1.Codec = str2;
            dictionary.Add(key37, (Itag)audioItag1);
            int key38 = 140;
            AudioItag audioItag2 = new AudioItag();
            audioItag2.StreamType = AudioStreamType.M4A;
            audioItag2.Quality = AudioQuality._128kbps;
            string str3 = "aac";
            audioItag2.Codec = str3;
            dictionary.Add(key38, (Itag)audioItag2);
            int key39 = 141;
            AudioItag audioItag3 = new AudioItag();
            audioItag3.StreamType = AudioStreamType.M4A;
            audioItag3.Quality = AudioQuality._256kbps;
            string str4 = "aac";
            audioItag3.Codec = str4;
            dictionary.Add(key39, (Itag)audioItag3);
            int key40 = 151;
            VideoItag videoItag36 = new VideoItag();
            videoItag36.StreamType = VideoStreamType.Mp4;
            videoItag36.Quality = VideoQuality._72p;
            int num89 = 72;
            videoItag36.Height = num89;
            int num90 = 1;
            videoItag36.IsLive = num90 != 0;
            dictionary.Add(key40, (Itag)videoItag36);
            int key41 = 160;
            VideoItag videoItag37 = new VideoItag();
            videoItag37.StreamType = VideoStreamType.Mp4;
            videoItag37.Quality = VideoQuality._144p;
            int num91 = 256;
            videoItag37.Width = num91;
            int num92 = 144;
            videoItag37.Height = num92;
            int num93 = 1;
            videoItag37.IsDash = num93 != 0;
            dictionary.Add(key41, (Itag)videoItag37);
            int key42 = 167;
            VideoItag videoItag38 = new VideoItag();
            videoItag38.StreamType = VideoStreamType.WebM;
            videoItag38.Quality = VideoQuality._360p;
            int num94 = 640;
            videoItag38.Width = num94;
            int num95 = 360;
            videoItag38.Height = num95;
            int num96 = 1;
            videoItag38.IsDash = num96 != 0;
            string str5 = "VP8";
            videoItag38.Codec = str5;
            dictionary.Add(key42, (Itag)videoItag38);
            int key43 = 168;
            VideoItag videoItag39 = new VideoItag();
            videoItag39.StreamType = VideoStreamType.WebM;
            videoItag39.Quality = VideoQuality._480p;
            int num97 = 854;
            videoItag39.Width = num97;
            int num98 = 480;
            videoItag39.Height = num98;
            int num99 = 1;
            videoItag39.IsDash = num99 != 0;
            string str6 = "VP8";
            videoItag39.Codec = str6;
            dictionary.Add(key43, (Itag)videoItag39);
            int key44 = 169;
            VideoItag videoItag40 = new VideoItag();
            videoItag40.StreamType = VideoStreamType.WebM;
            videoItag40.Quality = VideoQuality._720p;
            int num100 = 1280;
            videoItag40.Width = num100;
            int num101 = 720;
            videoItag40.Height = num101;
            int num102 = 1;
            videoItag40.IsDash = num102 != 0;
            string str7 = "VP8";
            videoItag40.Codec = str7;
            dictionary.Add(key44, (Itag)videoItag40);
            int key45 = 170;
            VideoItag videoItag41 = new VideoItag();
            videoItag41.StreamType = VideoStreamType.WebM;
            videoItag41.Quality = VideoQuality._1080p;
            int num103 = 1920;
            videoItag41.Width = num103;
            int num104 = 1080;
            videoItag41.Height = num104;
            int num105 = 1;
            videoItag41.IsDash = num105 != 0;
            string str8 = "VP8";
            videoItag41.Codec = str8;
            dictionary.Add(key45, (Itag)videoItag41);
            int key46 = 171;
            dictionary.Add(key46, (Itag)new AudioItag()
            {
                StreamType = AudioStreamType.WebM,
                Quality = AudioQuality._128kbps
            });
            int key47 = 172;
            dictionary.Add(key47, (Itag)new AudioItag()
            {
                StreamType = AudioStreamType.WebM,
                Quality = AudioQuality._256kbps
            });
            int key48 = 212;
            VideoItag videoItag42 = new VideoItag();
            videoItag42.StreamType = VideoStreamType.Mp4;
            videoItag42.Quality = VideoQuality._480p;
            int num106 = 854;
            videoItag42.Width = num106;
            int num107 = 480;
            videoItag42.Height = num107;
            int num108 = 1;
            videoItag42.IsDash = num108 != 0;
            string str9 = "h264";
            videoItag42.Codec = str9;
            dictionary.Add(key48, (Itag)videoItag42);
            int key49 = 218;
            VideoItag videoItag43 = new VideoItag();
            videoItag43.StreamType = VideoStreamType.WebM;
            videoItag43.Quality = VideoQuality._480p;
            int num109 = 854;
            videoItag43.Width = num109;
            int num110 = 480;
            videoItag43.Height = num110;
            int num111 = 1;
            videoItag43.IsDash = num111 != 0;
            string str10 = "VP8";
            videoItag43.Codec = str10;
            dictionary.Add(key49, (Itag)videoItag43);
            int key50 = 219;
            VideoItag videoItag44 = new VideoItag();
            videoItag44.StreamType = VideoStreamType.WebM;
            videoItag44.Quality = VideoQuality._480p;
            int num112 = 854;
            videoItag44.Width = num112;
            int num113 = 480;
            videoItag44.Height = num113;
            int num114 = 1;
            videoItag44.IsDash = num114 != 0;
            string str11 = "VP8";
            videoItag44.Codec = str11;
            dictionary.Add(key50, (Itag)videoItag44);
            int key51 = 242;
            VideoItag videoItag45 = new VideoItag();
            videoItag45.StreamType = VideoStreamType.WebM;
            videoItag45.Quality = VideoQuality._240p;
            int num115 = 426;
            videoItag45.Width = num115;
            int num116 = 240;
            videoItag45.Height = num116;
            int num117 = 1;
            videoItag45.IsDash = num117 != 0;
            dictionary.Add(key51, (Itag)videoItag45);
            int key52 = 243;
            VideoItag videoItag46 = new VideoItag();
            videoItag46.StreamType = VideoStreamType.WebM;
            videoItag46.Quality = VideoQuality._360p;
            int num118 = 640;
            videoItag46.Width = num118;
            int num119 = 360;
            videoItag46.Height = num119;
            int num120 = 1;
            videoItag46.IsDash = num120 != 0;
            dictionary.Add(key52, (Itag)videoItag46);
            int key53 = 244;
            VideoItag videoItag47 = new VideoItag();
            videoItag47.StreamType = VideoStreamType.WebM;
            videoItag47.Quality = VideoQuality._480p;
            int num121 = 854;
            videoItag47.Width = num121;
            int num122 = 480;
            videoItag47.Height = num122;
            int num123 = 1;
            videoItag47.IsDash = num123 != 0;
            dictionary.Add(key53, (Itag)videoItag47);
            int key54 = 245;
            VideoItag videoItag48 = new VideoItag();
            videoItag48.StreamType = VideoStreamType.WebM;
            videoItag48.Quality = VideoQuality._480p;
            int num124 = 854;
            videoItag48.Width = num124;
            int num125 = 480;
            videoItag48.Height = num125;
            int num126 = 1;
            videoItag48.IsDash = num126 != 0;
            dictionary.Add(key54, (Itag)videoItag48);
            int key55 = 246;
            VideoItag videoItag49 = new VideoItag();
            videoItag49.StreamType = VideoStreamType.WebM;
            videoItag49.Quality = VideoQuality._480p;
            int num127 = 854;
            videoItag49.Width = num127;
            int num128 = 480;
            videoItag49.Height = num128;
            int num129 = 1;
            videoItag49.IsDash = num129 != 0;
            dictionary.Add(key55, (Itag)videoItag49);
            int key56 = 247;
            VideoItag videoItag50 = new VideoItag();
            videoItag50.StreamType = VideoStreamType.WebM;
            videoItag50.Quality = VideoQuality._720p;
            int num130 = 1280;
            videoItag50.Width = num130;
            int num131 = 720;
            videoItag50.Height = num131;
            int num132 = 1;
            videoItag50.IsDash = num132 != 0;
            dictionary.Add(key56, (Itag)videoItag50);
            int key57 = 248;
            VideoItag videoItag51 = new VideoItag();
            videoItag51.StreamType = VideoStreamType.WebM;
            videoItag51.Quality = VideoQuality._1080p;
            int num133 = 1920;
            videoItag51.Width = num133;
            int num134 = 1080;
            videoItag51.Height = num134;
            int num135 = 1;
            videoItag51.IsDash = num135 != 0;
            dictionary.Add(key57, (Itag)videoItag51);
            int key58 = 249;
            AudioItag audioItag4 = new AudioItag();
            audioItag4.StreamType = AudioStreamType.WebM;
            audioItag4.Quality = AudioQuality._50kbps;
            string str12 = "opus";
            audioItag4.Codec = str12;
            dictionary.Add(key58, (Itag)audioItag4);
            int key59 = 250;
            AudioItag audioItag5 = new AudioItag();
            audioItag5.StreamType = AudioStreamType.WebM;
            audioItag5.Quality = AudioQuality._70kbps;
            string str13 = "opus";
            audioItag5.Codec = str13;
            dictionary.Add(key59, (Itag)audioItag5);
            int key60 = 251;
            AudioItag audioItag6 = new AudioItag();
            audioItag6.StreamType = AudioStreamType.WebM;
            audioItag6.Quality = AudioQuality._160kbps;
            string str14 = "opus";
            audioItag6.Codec = str14;
            dictionary.Add(key60, (Itag)audioItag6);
            int key61 = 256;
            AudioItag audioItag7 = new AudioItag();
            audioItag7.StreamType = AudioStreamType.M4A;
            audioItag7.Quality = AudioQuality._192kbps;
            string str15 = "aac";
            audioItag7.Codec = str15;
            dictionary.Add(key61, (Itag)audioItag7);
            int key62 = 258;
            AudioItag audioItag8 = new AudioItag();
            audioItag8.StreamType = AudioStreamType.M4A;
            audioItag8.Quality = AudioQuality._384kbps;
            string str16 = "aac";
            audioItag8.Codec = str16;
            dictionary.Add(key62, (Itag)audioItag8);
            int key63 = 264;
            VideoItag videoItag52 = new VideoItag();
            videoItag52.StreamType = VideoStreamType.Mp4;
            videoItag52.Quality = VideoQuality._1440p;
            int num136 = 2560;
            videoItag52.Width = num136;
            int num137 = 1440;
            videoItag52.Height = num137;
            int num138 = 1;
            videoItag52.IsDash = num138 != 0;
            dictionary.Add(key63, (Itag)videoItag52);
            int key64 = 266;
            VideoItag videoItag53 = new VideoItag();
            videoItag53.StreamType = VideoStreamType.Mp4;
            videoItag53.Quality = VideoQuality._2160p;
            int num139 = 3840;
            videoItag53.Width = num139;
            int num140 = 2160;
            videoItag53.Height = num140;
            int num141 = 1;
            videoItag53.IsDash = num141 != 0;
            string str17 = "h264";
            videoItag53.Codec = str17;
            dictionary.Add(key64, (Itag)videoItag53);
            int key65 = 271;
            VideoItag videoItag54 = new VideoItag();
            videoItag54.StreamType = VideoStreamType.WebM;
            videoItag54.Quality = VideoQuality._1440p;
            int num142 = 2560;
            videoItag54.Width = num142;
            int num143 = 1440;
            videoItag54.Height = num143;
            int num144 = 1;
            videoItag54.IsDash = num144 != 0;
            dictionary.Add(key65, (Itag)videoItag54);
            int key66 = 272;
            VideoItag videoItag55 = new VideoItag();
            videoItag55.StreamType = VideoStreamType.WebM;
            videoItag55.Quality = VideoQuality._2160p;
            int num145 = 3840;
            videoItag55.Width = num145;
            int num146 = 2160;
            videoItag55.Height = num146;
            int num147 = 1;
            videoItag55.IsDash = num147 != 0;
            dictionary.Add(key66, (Itag)videoItag55);
            int key67 = 278;
            VideoItag videoItag56 = new VideoItag();
            videoItag56.StreamType = VideoStreamType.WebM;
            videoItag56.Quality = VideoQuality._144p;
            int num148 = 256;
            videoItag56.Width = num148;
            int num149 = 144;
            videoItag56.Height = num149;
            int num150 = 1;
            videoItag56.IsDash = num150 != 0;
            string str18 = "VP9";
            videoItag56.Codec = str18;
            dictionary.Add(key67, (Itag)videoItag56);
            int key68 = 298;
            VideoItag videoItag57 = new VideoItag();
            videoItag57.StreamType = VideoStreamType.Mp4;
            videoItag57.Quality = VideoQuality._720p60fps;
            int num151 = 1280;
            videoItag57.Width = num151;
            int num152 = 720;
            videoItag57.Height = num152;
            int num153 = 1;
            videoItag57.IsDash = num153 != 0;
            string str19 = "h264";
            videoItag57.Codec = str19;
            dictionary.Add(key68, (Itag)videoItag57);
            int key69 = 299;
            VideoItag videoItag58 = new VideoItag();
            videoItag58.StreamType = VideoStreamType.Mp4;
            videoItag58.Quality = VideoQuality._1080p60fps;
            int num154 = 1920;
            videoItag58.Width = num154;
            int num155 = 1080;
            videoItag58.Height = num155;
            int num156 = 1;
            videoItag58.IsDash = num156 != 0;
            string str20 = "h264";
            videoItag58.Codec = str20;
            dictionary.Add(key69, (Itag)videoItag58);
            int key70 = 302;
            VideoItag videoItag59 = new VideoItag();
            videoItag59.StreamType = VideoStreamType.WebM;
            videoItag59.Quality = VideoQuality._720p60fps;
            int num157 = 1280;
            videoItag59.Width = num157;
            int num158 = 720;
            videoItag59.Height = num158;
            int num159 = 1;
            videoItag59.IsDash = num159 != 0;
            string str21 = "VP9";
            videoItag59.Codec = str21;
            dictionary.Add(key70, (Itag)videoItag59);
            int key71 = 303;
            VideoItag videoItag60 = new VideoItag();
            videoItag60.StreamType = VideoStreamType.WebM;
            videoItag60.Quality = VideoQuality._1080p60fps;
            int num160 = 1920;
            videoItag60.Width = num160;
            int num161 = 1080;
            videoItag60.Height = num161;
            int num162 = 1;
            videoItag60.IsDash = num162 != 0;
            string str22 = "VP9";
            videoItag60.Codec = str22;
            dictionary.Add(key71, (Itag)videoItag60);
            int key72 = 308;
            VideoItag videoItag61 = new VideoItag();
            videoItag61.StreamType = VideoStreamType.WebM;
            videoItag61.Quality = VideoQuality._1440p60fps;
            int num163 = 1440;
            videoItag61.Height = num163;
            int num164 = 1;
            videoItag61.IsDash = num164 != 0;
            string str23 = "VP9";
            videoItag61.Codec = str23;
            dictionary.Add(key72, (Itag)videoItag61);
            int key73 = 313;
            VideoItag videoItag62 = new VideoItag();
            videoItag62.StreamType = VideoStreamType.WebM;
            videoItag62.Quality = VideoQuality._2160p;
            int num165 = 3840;
            videoItag62.Width = num165;
            int num166 = 2160;
            videoItag62.Height = num166;
            int num167 = 1;
            videoItag62.IsDash = num167 != 0;
            string str24 = "VP9";
            videoItag62.Codec = str24;
            dictionary.Add(key73, (Itag)videoItag62);
            int key74 = 315;
            VideoItag videoItag63 = new VideoItag();
            videoItag63.StreamType = VideoStreamType.WebM;
            videoItag63.Quality = VideoQuality._2160p60fps;
            int num168 = 2160;
            videoItag63.Height = num168;
            int num169 = 1;
            videoItag63.IsDash = num169 != 0;
            string str25 = "VP9";
            videoItag63.Codec = str25;
            dictionary.Add(key74, (Itag)videoItag63);
            int key75 = 325;
            AudioItag audioItag9 = new AudioItag();
            audioItag9.StreamType = AudioStreamType.M4A;
            string str26 = "dtse";
            audioItag9.Codec = str26;
            dictionary.Add(key75, (Itag)audioItag9);
            int key76 = 328;
            AudioItag audioItag10 = new AudioItag();
            audioItag10.StreamType = AudioStreamType.M4A;
            string str27 = "ec-3";
            audioItag10.Codec = str27;
            dictionary.Add(key76, (Itag)audioItag10);
            int key77 = 330;
            VideoItag videoItag64 = new VideoItag();
            videoItag64.StreamType = VideoStreamType.WebM;
            videoItag64.Quality = VideoQuality._144p60fps;
            int num170 = 256;
            videoItag64.Width = num170;
            int num171 = 144;
            videoItag64.Height = num171;
            int num172 = 1;
            videoItag64.IsDash = num172 != 0;
            string str28 = "VP9";
            videoItag64.Codec = str28;
            dictionary.Add(key77, (Itag)videoItag64);
            int key78 = 331;
            VideoItag videoItag65 = new VideoItag();
            videoItag65.StreamType = VideoStreamType.WebM;
            videoItag65.Quality = VideoQuality._240p60fps;
            int num173 = 426;
            videoItag65.Width = num173;
            int num174 = 240;
            videoItag65.Height = num174;
            int num175 = 1;
            videoItag65.IsDash = num175 != 0;
            string str29 = "VP9";
            videoItag65.Codec = str29;
            dictionary.Add(key78, (Itag)videoItag65);
            int key79 = 332;
            VideoItag videoItag66 = new VideoItag();
            videoItag66.StreamType = VideoStreamType.WebM;
            videoItag66.Quality = VideoQuality._360p60fps;
            int num176 = 640;
            videoItag66.Width = num176;
            int num177 = 360;
            videoItag66.Height = num177;
            int num178 = 1;
            videoItag66.IsDash = num178 != 0;
            string str30 = "VP9";
            videoItag66.Codec = str30;
            dictionary.Add(key79, (Itag)videoItag66);
            int key80 = 333;
            VideoItag videoItag67 = new VideoItag();
            videoItag67.StreamType = VideoStreamType.WebM;
            videoItag67.Quality = VideoQuality._480p60fps;
            int num179 = 854;
            videoItag67.Width = num179;
            int num180 = 480;
            videoItag67.Height = num180;
            int num181 = 1;
            videoItag67.IsDash = num181 != 0;
            string str31 = "VP9";
            videoItag67.Codec = str31;
            dictionary.Add(key80, (Itag)videoItag67);
            int key81 = 334;
            VideoItag videoItag68 = new VideoItag();
            videoItag68.StreamType = VideoStreamType.WebM;
            videoItag68.Quality = VideoQuality._720p60fps;
            int num182 = 1280;
            videoItag68.Width = num182;
            int num183 = 720;
            videoItag68.Height = num183;
            int num184 = 1;
            videoItag68.IsDash = num184 != 0;
            string str32 = "VP9";
            videoItag68.Codec = str32;
            dictionary.Add(key81, (Itag)videoItag68);
            int key82 = 335;
            VideoItag videoItag69 = new VideoItag();
            videoItag69.StreamType = VideoStreamType.WebM;
            videoItag69.Quality = VideoQuality._1080p60fps;
            int num185 = 1920;
            videoItag69.Width = num185;
            int num186 = 1080;
            videoItag69.Height = num186;
            int num187 = 1;
            videoItag69.IsDash = num187 != 0;
            string str33 = "VP9";
            videoItag69.Codec = str33;
            dictionary.Add(key82, (Itag)videoItag69);
            int key83 = 336;
            VideoItag videoItag70 = new VideoItag();
            videoItag70.StreamType = VideoStreamType.WebM;
            videoItag70.Quality = VideoQuality._1440p60fps;
            int num188 = 2560;
            videoItag70.Width = num188;
            int num189 = 1440;
            videoItag70.Height = num189;
            int num190 = 1;
            videoItag70.IsDash = num190 != 0;
            string str34 = "VP9";
            videoItag70.Codec = str34;
            dictionary.Add(key83, (Itag)videoItag70);
            int key84 = 337;
            VideoItag videoItag71 = new VideoItag();
            videoItag71.StreamType = VideoStreamType.WebM;
            videoItag71.Quality = VideoQuality._2160p60fps;
            int num191 = 3840;
            videoItag71.Width = num191;
            int num192 = 2160;
            videoItag71.Height = num192;
            int num193 = 1;
            videoItag71.IsDash = num193 != 0;
            string str35 = "VP9";
            videoItag71.Codec = str35;
            dictionary.Add(key84, (Itag)videoItag71);
            YouTubeDownloadProvider.Itags = dictionary;
        }

        public VideoInfo ParseVideoInfo(string videoInfoContent)
        {
            if (string.IsNullOrEmpty(videoInfoContent))
                throw new ProviderParsingException("Unable to get video info content", this._url);
            if (videoInfoContent.JustAfter("status=", "&") == "fail")
                throw new VideoServerException(MediaLink.Decode(videoInfoContent.JustAfter("reason=", "&")).HtmlToText().Replace('+', ' '), this._url);
            VideoInfo videoInfo1 = new VideoInfo();
            string url = this._url;
            videoInfo1.SourceUrl = url;
            VideoInfo videoInfo2 = videoInfo1;
            List<MediaLink> mediaLinkList = new List<MediaLink>();
            string stringToUnescape1 = videoInfoContent.JustAfter("url_encoded_fmt_stream_map=", "&");
            if (!string.IsNullOrEmpty(stringToUnescape1))
            {
                string[] strArray = Uri.UnescapeDataString(stringToUnescape1).Split(',');
                mediaLinkList.AddRange(((IEnumerable<string>)strArray).Select<string, MediaLink>((Func<string, MediaLink>)(fmtUrl => this.ParseFmtUrl(fmtUrl, false, false, this._html5PlayerPath))).Where<MediaLink>((Func<MediaLink, bool>)(l => l != null)));
            }
            string stringToUnescape2 = videoInfoContent.JustAfter("adaptive_fmts=", "&");
            if (!string.IsNullOrEmpty(stringToUnescape2))
            {
                string[] strArray = Uri.UnescapeDataString(stringToUnescape2).Split(',');
                mediaLinkList.AddRange(((IEnumerable<string>)strArray).Select<string, MediaLink>((Func<string, MediaLink>)(fmtUrl => this.ParseFmtUrl(fmtUrl, false, true, this._html5PlayerPath))).Where<MediaLink>((Func<MediaLink, bool>)(l => l != null)));
            }
            if (mediaLinkList.Count <= 0)
                throw new ProviderParsingException("Video info parsing error", this._url);
            videoInfo2.Links = (IMediaLink[])mediaLinkList.ToArray();
            videoInfo2.Title = MediaLink.Decode(videoInfoContent.JustAfter("title=", "&").Replace('+', ' '));
            return videoInfo2;
        }

        public void ParseVideoInfo(Uri videoInfoUrl, string pageData, Action<MediaInfo> callback)
        {
            throw new NotImplementedException();
        }

        public void ParseVideoInfo(Uri videoInfoUrl, string pageData, Action<MediaInfo> callback, object userState)
        {
            HttpUtil.Instance.GetData(videoInfoUrl, (Action<string>)(content => this.VideoInfoContentReceived(content, pageData, callback, userState)), "GET", (Dictionary<string, object>)null);
        }

        public void VideoInfoContentReceived(string videoInfoContent, string pageData, Action<MediaInfo> callback)
        {
            throw new NotImplementedException();
        }

        public void VideoInfoContentReceived(string videoInfoContent, string pageData, Action<MediaInfo> callback, object userState)
        {
            VideoInfo videoInfo1 = new VideoInfo();
            string url1 = this._url;
            videoInfo1.SourceUrl = url1;
            VideoInfo videoInfo2 = videoInfo1;
            VideoInfo videoInfo3;
            try
            {
                videoInfo3 = this.ParseVideoInfo(videoInfoContent);
            }
            catch (FreeYouTubeDownloader.Downloader.NotSupportedException ex)
            {
                videoInfo2.Exception = (Exception)ex;
                callback((MediaInfo)videoInfo2);
                return;
            }
            catch (Exception ex1)
            {
                Match match1 = new Regex(YouTubeDownloadProvider.TicketExpressions[0]).Match(pageData);
                if (string.IsNullOrEmpty(match1.Value))
                {
                    match1 = new Regex(YouTubeDownloadProvider.TicketExpressions[1]).Match(pageData);
                    if (string.IsNullOrEmpty(match1.Value))
                    {
                        Match match2 = new Regex("<div id=\"verify-age-actions\">(?<text>[\\w\\W]*?)</div>").Match(pageData);
                        if (match2.Success && !string.IsNullOrEmpty(match2.Groups["text"].Value))
                            videoInfo2.Exception = (Exception)new FreeYouTubeDownloader.Downloader.NotSupportedException(match2.Groups["text"].Value.HtmlToText(), this._url);
                        else if (pageData.Contains("verify-actions"))
                            videoInfo2.Exception = (Exception)new FreeYouTubeDownloader.Downloader.NotSupportedException(Strings.AgeProtectedWarning, this._url);
                        else
                            videoInfo2.Exception = ex1;
                        callback((MediaInfo)videoInfo2);
                        return;
                    }
                }
                string stringToUnescape = match1.Groups[1].Value;
                if (!string.IsNullOrEmpty(stringToUnescape))
                {
                    string[] strArray = Uri.UnescapeDataString(stringToUnescape).Split(',');
                    VideoInfo videoInfo4 = new VideoInfo();
                    VideoLink[] videoLinkArray = new VideoLink[strArray.Length];
                    videoInfo4.Links = (IMediaLink[])videoLinkArray;
                    string url2 = this._url;
                    videoInfo4.SourceUrl = url2;
                    videoInfo3 = videoInfo4;
                    int index = 0;
                    int num = 0;
                    for (; index < strArray.Length; ++index)
                    {
                        try
                        {
                            MediaLink fmtUrl = this.ParseFmtUrl(strArray[index], true, false, "");
                            if (fmtUrl != null)
                                videoInfo3.Links[num++] = (IMediaLink)fmtUrl;
                        }
                        catch (ProviderException ex2)
                        {
                            videoInfo3.Exception = (Exception)ex2;
                            callback((MediaInfo)videoInfo3);
                            return;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(pageData) && pageData.Contains("verify-actions"))
                    {
                        videoInfo2.Exception = (Exception)new FreeYouTubeDownloader.Downloader.NotSupportedException("Unable to parse video content. Please report", this._url);
                        callback((MediaInfo)videoInfo2);
                        return;
                    }
                    throw;
                }
            }
            if (string.IsNullOrEmpty(videoInfo3.Title) || videoInfo3.Title.Equals(Strings.PreparingForDownloading))
                videoInfo3.Title = this.GetMediaTitle(pageData);
            callback((MediaInfo)videoInfo3);
        }

        public string GetVideoId(string url)
        {
            return YouTubeDownloadProvider.GetVideoIdStatic(url);
        }

        public static string GetVideoIdStatic(string url)
        {
            Match match = Regex.Match(url, "^(?:https?://)?(?:youtu\\.be/|(?:\\w+\\.)?(?:youtube\\.com/(?:watch|watch_popup|shared)?\\?(?:.+&)?(v|ci)=|youtube\\.com/(?:v|e|embed)/))([A-Za-z0-9_-]+)");
            if (!match.Success)
                return (string)null;
            return match.Groups[2].Value;
        }

        public override void ReceiveMediaInfo(string url, string pageData, Action<MediaInfo> callback)
        {
            this._url = url;
            List<MediaLink> mediaLinkList = new List<MediaLink>();
            VideoInfo videoInfo1 = new VideoInfo();
            string str1 = url;
            videoInfo1.SourceUrl = str1;
            string mediaTitle = this.GetMediaTitle(pageData);
            videoInfo1.Title = mediaTitle;
            VideoInfo videoInfo2 = videoInfo1;
            if (pageData.Contains("meta property=\"og:video:tag\" content=\"YouTube Red\""))
            {
                videoInfo2.Exception = (Exception)new FreeYouTubeDownloader.Downloader.NotSupportedException(Strings.YouTubeRedNotSupported, this._url);
                callback((MediaInfo)videoInfo2);
            }
            else
            {
                this._html5PlayerPath = pageData.JustAfter("\"js\":\"", "\"");
                Match match1 = Regex.Match(pageData, "\"dashmpd\":\"(.*?)\"");
                string str2 = match1.Success ? match1.Groups[1].Value : (string)null;
                if (!string.IsNullOrEmpty(str2))
                {
                    string str3 = str2.Replace("\\/", "/");
                    Match match2 = this._dashManifestEncryptedSignatureRegex.Match(str3);
                    if (match2.Success)
                    {
                        string str4 = YouTubeSignatureDecipher.DecryptFromWebSide(match2.Groups[1].Value, this._html5PlayerPath, url);
                        str3 = str3.Replace(match2.Value, string.Format("/signature/{0}", (object)str4));
                    }
                    string dataSync = HttpUtil.Instance.GetDataSync(str3, "GET", (Dictionary<string, object>)null);
                    if (!string.IsNullOrEmpty(dataSync))
                        mediaLinkList.AddRange((IEnumerable<MediaLink>)this.GetLinksFromDashManifest(dataSync));
                }
                string stringToUnescape1 = pageData.JustAfter("\"adaptive_fmts\":\"", "\",");
                if (!string.IsNullOrEmpty(stringToUnescape1))
                {
                    try
                    {
                        List<string> list = ((IEnumerable<string>)Regex.Replace(Uri.UnescapeDataString(stringToUnescape1), "\\+codecs=\".*?\"", "").Split(',')).ToList<string>();
                        mediaLinkList.AddRange(list.Select<string, MediaLink>((Func<string, MediaLink>)(fmtUrl => this.ParseFmtUrl(fmtUrl, true, true, this._html5PlayerPath))).Where<MediaLink>((Func<MediaLink, bool>)(l => l != null)));
                    }
                    catch (Exception ex)
                    {
                        videoInfo2.Exception = ex;
                    }
                }
                string stringToUnescape2 = pageData.JustAfter("\"url_encoded_fmt_stream_map\":\"", "\",");
                if (!string.IsNullOrEmpty(stringToUnescape2))
                {
                    try
                    {
                        List<string> list = ((IEnumerable<string>)Regex.Replace(Uri.UnescapeDataString(stringToUnescape2), "\\+codecs=\".*?\"", "").Split(',')).ToList<string>();
                        mediaLinkList.AddRange(list.Select<string, MediaLink>((Func<string, MediaLink>)(fmtUrl => this.ParseFmtUrl(fmtUrl, true, false, this._html5PlayerPath))).Where<MediaLink>((Func<MediaLink, bool>)(l => l != null)));
                    }
                    catch
                    {
                    }
                }
                Match match3 = this._thumbnailsRegex.Match(pageData);
                if (match3.Success)
                {
                    foreach (JToken jtoken in (IEnumerable<JToken>)JObject.Parse(match3.Value.Replace("\\\"", "\""))["thumbnails"])
                        videoInfo2.Thumbnails.Add(new Thumbnail(jtoken[(object)"url"].Value<string>(), jtoken[(object)"width"].Value<int>(), jtoken[(object)"height"].Value<int>()));
                }
                if (mediaLinkList.Count > 0)
                {
                    videoInfo2.Links = (IMediaLink[])mediaLinkList.ToArray();
                    callback((MediaInfo)videoInfo2);
                }
                else
                {
                    string videoId = this.GetVideoId(url);
                    if (string.IsNullOrEmpty(videoId))
                        throw new ProviderParsingException("Unable to get video id", url);
                    string dataSync = HttpUtil.Instance.GetDataSync("https://www.youtube.com/embed/" + videoId, "GET", (Dictionary<string, object>)null);
                    this._html5PlayerPath = dataSync.JustAfter("\"js\":\"", "\"");
                    string str3 = Regex.Match(dataSync, "\"sts\":(\\d+)", RegexOptions.Singleline).Groups[1].Value;
                    string uriString = string.Format("https://www.youtube.com/get_video_info?video_id={0}&el=embedded&gl=US&hl=en&eurl={1}&asv=3&sts={2}", (object)videoId, (object)Uri.EscapeDataString(string.Format("https://youtube.googleapis.com/v/{0}", (object)videoId)), (object)str3);
                    YouTubeVideoObject youTubeVideoObject = new YouTubeVideoObject()
                    {
                        VideoId = videoId
                    };
                    this.ParseVideoInfo(new Uri(uriString), pageData, callback, (object)youTubeVideoObject);
                }
            }
        }

        public override void ReceiveMediaInfoFromServerSide(string url, Action<MediaInfo> callback)
        {
            string dataSync = HttpUtil.Instance.GetDataSync(string.Format("https://api.videoplex.com/getVideo?url={0}&token={1}&optimized=true&excludeFields=quality,videoType,bitrate,resolution,qualityDescription,videoOnly,audioOnly,duration,metaData,uploadDate,type,websiteLogos,isAdult,website", (object)url, (object)Authorization.GenerateToken()), "GET", (Dictionary<string, object>)null);
            if (string.IsNullOrEmpty(dataSync))
                throw new ProviderParsingException("Could not obtain media information from the remote source", url);
            JObject jobject = JObject.Parse(dataSync);
            VideoInfo videoInfo = new VideoInfo();
            if (jobject["error"] != null)
            {
                videoInfo.Exception = (Exception)new VideoServerException(jobject["error"].ToString(), url);
                callback((MediaInfo)videoInfo);
            }
            else
            {
                videoInfo.Title = jobject["title"].ToString();
                videoInfo.SourceUrl = url;
                List<MediaLink> mediaLinkList = new List<MediaLink>(jobject["mirrors"].Children().Count<JToken>());
                foreach (JToken child in jobject["mirrors"].Children())
                {
                    string str1 = "videoUrl";
                    string str2 = child[(object)str1].ToString();
                    string str3 = "itag";
                    int key = child[(object)str3].ToObject<int>();
                    long int64 = Regex.Match(str2, "expire=([^&]*)", RegexOptions.Singleline).Groups[1].Value.ToInt64();
                    string str4 = Regex.Match(str2, "clen=([^&]*)", RegexOptions.Singleline).Groups[1].Value;
                    long length = !string.IsNullOrEmpty(str4) ? str4.ToInt64() : 0L;
                    if (YouTubeDownloadProvider.Itags.ContainsKey(key))
                    {
                        Itag itag = YouTubeDownloadProvider.Itags[key];
                        if (itag is VideoItag)
                            mediaLinkList.Add((MediaLink)new VideoLink(str2, itag.AsVideoTag.Quality, itag.AsVideoTag.StreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime((double)int64)), itag.AsVideoTag.IsDash, length));
                        else
                            mediaLinkList.Add((MediaLink)new AudioLink(str2, itag.AsAudioTag.Quality, itag.AsAudioTag.StreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime((double)int64)), length));
                    }
                }
                videoInfo.Links = (IMediaLink[])mediaLinkList.ToArray();
                callback((MediaInfo)videoInfo);
            }
        }

        protected override string GetMediaTitle(string pageData)
        {
            string str = pageData.JustAfter("og:title\" content=\"", "\"");
            if (string.IsNullOrEmpty(str))
                str = base.GetMediaTitle(pageData);
            if (!string.IsNullOrEmpty(str))
                str = str.Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&#39;", "'");
            return str;
        }

        public MediaLink ParseFmtUrl(string fmtUrl, bool fromPageSource, bool adaptiveFmt, string jsPlayer = "")
        {
            int key = 0;
            string str1 = string.Empty;
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string empty3 = string.Empty;
            string empty4 = string.Empty;
            string empty5 = string.Empty;
            long length = 0;
            string[] strArray1;
            if (!fromPageSource)
                strArray1 = fmtUrl.Split('&');
            else
                strArray1 = Regex.Split(fmtUrl, "\\\\u0026");
            foreach (string input in strArray1)
            {
                string[] strArray2 = new Regex("=").Split(input, 2, 0);
                string s = strArray2[0];
                // ISSUE: reference to a compiler-generated method
                //uint stringHash = "\u003CPrivateImplementationDetails\u003E".ComputeStringHash(s);
                //if (stringHash <= 1361572173U)
                //{
                //  if (stringHash <= 597743964U)
                //  {
                //    if ((int) stringHash != 101885683)
                //    {
                //      if ((int) stringHash == 597743964 && s == "size")
                if (s == "size")
                    empty5 = strArray2[1];
                //}
                else if (s == "quality_label")
                    empty4 = strArray2[1];
                //}
                //else if ((int) stringHash != 848251934)
                //{
                //  if ((int) stringHash == 1361572173 && s == "type")
                else if (s == "type")
                    empty2 = strArray2[1];
                //}
                else if (s == "url")
                {
                    str1 = Uri.UnescapeDataString(strArray2[1]);
                    empty3 = Regex.Match(str1, "expire=([^&]*)", RegexOptions.Singleline).Groups[1].Value;
                    string str2 = Regex.Match(str1, "clen=([^&]*)", RegexOptions.Singleline).Groups[1].Value;
                    length = !string.IsNullOrEmpty(str2) ? str2.ToInt64() : 0L;
                }
                //}
                //else if (stringHash <= 3560979775U)
                //{
                //  if ((int) stringHash != -1792174388)
                //  {
                //    if ((int) stringHash == -733987521 && s == "conn" && strArray2[1].Contains("rtmp"))
                else if (s == "conn" && strArray2[1].Contains("rtmp"))
                    throw new FreeYouTubeDownloader.Downloader.NotSupportedException(Strings.RTMPUnsupported, this._url);
                //}
                else if (s == "itag")
                    key = Convert.ToInt32(strArray2[1]);
                //}
                //else if ((int) stringHash != -684712926)
                //{
                //  if ((int) stringHash == -166967934 && s == "s")
                else if (s == "s")
                    empty1 = strArray2[1];
                //}
                else if (s == "sig")
                    empty1 = strArray2[1];
            }
            if (str1.StartsWith("rtmp") || str1.Contains("&rtmpe=yes"))
                throw new FreeYouTubeDownloader.Downloader.NotSupportedException(Strings.RTMPUnsupported, this._url);
            if (!str1.Contains("ratebypass"))
                str1 += "&ratebypass=yes";
            if (YouTubeDownloadProvider.Itags.ContainsKey(key))
            {
                Itag itag = YouTubeDownloadProvider.Itags[key];
                int num1 = empty2.Contains("audio") ? 1 : 0;
                string str2 = fromPageSource ? empty2.JustAfter("/", ";") : empty2.JustAfter("%2F", "%3B");
                MediaLink mediaLink;
                if (num1 != 0)
                {
                    AudioStreamType audioStreamType = str2 == "mp4" ? AudioStreamType.Mp4 : (str2 == "webm" ? AudioStreamType.WebM : AudioStreamType.Unknown);
                    AudioLink audioLink = new AudioLink(str1, itag.AsAudioTag.Quality, audioStreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime(empty3)), length);
                    string url = this._url;
                    audioLink.SourceUrl = url;
                    int num2 = key;
                    audioLink.Itag = num2;
                    string str3 = empty1;
                    audioLink.S = str3;
                    string str4 = jsPlayer;
                    audioLink.JsPlayer = str4;
                    mediaLink = (MediaLink)audioLink;
                }
                else
                {
                    VideoStreamType videoStreamType = str2 == "mp4" ? VideoStreamType.Mp4 : (str2 == "web" ? VideoStreamType.WebM : (str2 == "webm" ? VideoStreamType.WebM : (str2 == "x-flv" ? VideoStreamType.Flv : (str2 == "3gpp" ? VideoStreamType.ThreeGp : VideoStreamType.Unknown))));
                    VideoLink videoLink = new VideoLink(str1, itag.AsVideoTag.Quality, videoStreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime(empty3)), adaptiveFmt, length);
                    string url = this._url;
                    videoLink.SourceUrl = url;
                    int num2 = key;
                    videoLink.Itag = num2;
                    int width = itag.AsVideoTag.Width;
                    videoLink.Width = width;
                    int height = itag.AsVideoTag.Height;
                    videoLink.Height = height;
                    string str3 = empty1;
                    videoLink.S = str3;
                    string str4 = jsPlayer;
                    videoLink.JsPlayer = str4;
                    mediaLink = (MediaLink)videoLink;
                    if (empty4 == "4320p")
                        mediaLink.ToVideoLink().VideoStreamQuality = VideoQuality._4320p;
                    else if (empty4 == "4320p60")
                        mediaLink.ToVideoLink().VideoStreamQuality = VideoQuality._4320p60fps;
                    if (mediaLink.ToVideoLink().VideoStreamQuality == VideoQuality._4320p || mediaLink.ToVideoLink().VideoStreamQuality == VideoQuality._4320p60fps)
                    {
                        Match match = Regex.Match(empty5, "((?<width>\\d{2,5})[x|X](?<height>\\d{2,5}))");
                        if (match.Success)
                        {
                            mediaLink.ToVideoLink().Width = match.Groups["width"].Value.ToInt32();
                            mediaLink.ToVideoLink().Height = match.Groups["height"].Value.ToInt32();
                        }
                    }
                }
                return mediaLink;
            }
            return (MediaLink)null;
        }

        private List<MediaLink> GetLinksFromDashManifest(string dashManifest)
        {
            XDocument xdocument = XDocument.Parse(dashManifest);
            XNamespace defaultNamespace = xdocument.Root.GetDefaultNamespace();
            xdocument.Root.GetNamespaceOfPrefix("yt");
            List<XElement> list = xdocument.Root.Descendants(defaultNamespace + "Representation").ToList<XElement>();
            List<MediaLink> mediaLinkList1 = new List<MediaLink>(list.Count);
            foreach (XElement xelement in list)
            {
                try
                {
                    int int32_1 = xelement.Attribute((XName)"id").Value.ToInt32();
                    string str1 = xelement.Element(defaultNamespace + "BaseURL").Value.Remove("amp;");
                    string str2 = Regex.Match(str1, "clen\\/(\\d*)", RegexOptions.Singleline).Groups[1].Value;
                    long length = !string.IsNullOrEmpty(str2) ? str2.ToInt64() : 0L;
                    if (length != 0L)
                    {
                        string unixTimeStamp = Regex.Match(str1, "expire\\/(\\d*)", RegexOptions.Singleline).Groups[1].Value;
                        if (!str1.Contains("ratebypass"))
                            str1 += "ratebypass/yes/";
                        Itag itag;
                        if (YouTubeDownloadProvider.Itags.ContainsKey(int32_1))
                        {
                            itag = YouTubeDownloadProvider.Itags[int32_1];
                        }
                        else
                        {
                            itag = YouTubeDownloadProvider.Itags.Values.First<Itag>();
                        }
                        if (itag.IsVideoTag)
                        {
                            int int32_2 = xelement.Attribute((XName)"width").Value.ToInt32();
                            int int32_3 = xelement.Attribute((XName)"height").Value.ToInt32();
                            List<MediaLink> mediaLinkList2 = mediaLinkList1;
                            VideoLink videoLink = new VideoLink(str1, itag.AsVideoTag.Quality, itag.AsVideoTag.StreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime(unixTimeStamp)), true, length);
                            string url = this._url;
                            videoLink.SourceUrl = url;
                            int num1 = int32_1;
                            videoLink.Itag = num1;
                            int num2 = int32_2;
                            videoLink.Width = num2;
                            int num3 = int32_3;
                            videoLink.Height = num3;
                            mediaLinkList2.Add((MediaLink)videoLink);
                        }
                        else
                        {
                            List<MediaLink> mediaLinkList2 = mediaLinkList1;
                            AudioLink audioLink = new AudioLink(str1, itag.AsAudioTag.Quality, itag.AsAudioTag.StreamType, new DateTime?(DateTimeUtil.UnixTimeStampToDateTime(unixTimeStamp)), length);
                            string url = this._url;
                            audioLink.SourceUrl = url;
                            int num = int32_1;
                            audioLink.Itag = num;
                            mediaLinkList2.Add((MediaLink)audioLink);
                        }
                    }
                }
                catch
                {
                }
            }
            return mediaLinkList1;
        }
    }
}
