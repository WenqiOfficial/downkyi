﻿using DownKyi.Models;
using DownKyi.Services.Download;
using DownKyi.Utils;
using DownKyi.ViewModels;
using DownKyi.ViewModels.Dialogs;
using DownKyi.ViewModels.DownloadManager;
using DownKyi.ViewModels.Settings;
using DownKyi.ViewModels.Toolbox;
using DownKyi.Views;
using DownKyi.Views.Dialogs;
using DownKyi.Views.DownloadManager;
using DownKyi.Views.Settings;
using DownKyi.Views.Toolbox;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DownKyi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static ObservableCollection<DownloadingItem> DownloadingList { get; set; }
        public static ObservableCollection<DownloadedItem> DownloadedList { get; set; }

        // 下载服务
        private IDownloadService downloadService;

        protected override Window CreateShell()
        {
            // 设置主题
            DictionaryResource.LoadTheme("ThemeDefault");
            //DictionaryResource.LoadTheme("ThemeDiy");

            // 切换语言
            DictionaryResource.LoadLanguage("Default");
            //DictionaryResource.LoadLanguage("en_US");

            // 初始化数据
            DownloadingList = new ObservableCollection<DownloadingItem>();
            DownloadedList = new ObservableCollection<DownloadedItem>();

            // test DownloadingList data
            DownloadingList.Add(new DownloadingItem
            {
                MainTitle = "Test1",
                Name = "name1=name1=name1=name1=name1=name1=name1=name1=name1=name1=name1",
                ZoneImage = (System.Windows.Media.DrawingImage)Current.Resources[Core.BiliApi.Zone.VideoZoneIcon.Instance().GetZoneImageKey(4)],
                Resolution = new Resolution
                {
                    Name = "1080P",
                    Id = 64
                },
                AudioCodecName = "194K",
            });
            DownloadingList.Add(new DownloadingItem
            {
                MainTitle = "Test2",
                Name = "name2",
                ZoneImage = (System.Windows.Media.DrawingImage)Current.Resources[Core.BiliApi.Zone.VideoZoneIcon.Instance().GetZoneImageKey(3)],
                Resolution = new Resolution
                {
                    Name = "1080P",
                    Id = 64
                },
                Duration = "1h23m50s",
                VideoCodecName = "HEVC",
                AudioCodecName = "194K",
            });
            DownloadingList.Add(new DownloadingItem
            {
                MainTitle = "Test3",
                Name = "name3",
                ZoneImage = (System.Windows.Media.DrawingImage)Current.Resources[Core.BiliApi.Zone.VideoZoneIcon.Instance().GetZoneImageKey(3)],
                Resolution = new Resolution
                {
                    Name = "720P",
                    Id = 64
                },
                Duration = "23m50s",
                VideoCodecName = "AVC",
                AudioCodecName = "194K",
            });
            DownloadingList.Add(new DownloadingItem
            {
                MainTitle = "Test4",
                Name = "name4",
                ZoneImage = (System.Windows.Media.DrawingImage)Current.Resources[Core.BiliApi.Zone.VideoZoneIcon.Instance().GetZoneImageKey(3)],
                Resolution = new Resolution
                {
                    Name = "720P",
                    Id = 64
                },
                Duration = "23m50s",
                VideoCodecName = "AVC",
                AudioCodecName = "194K",
            });
            DownloadingList.Add(new DownloadingItem
            {
                MainTitle = "Test5",
                Name = "name5",
                ZoneImage = (System.Windows.Media.DrawingImage)Current.Resources[Core.BiliApi.Zone.VideoZoneIcon.Instance().GetZoneImageKey(5)],
                Resolution = new Resolution
                {
                    Name = "720P",
                    Id = 64
                },
                Duration = "23m50s",
                VideoCodecName = "AVC",
                AudioCodecName = "194K",
            });

            // TODO 从数据库读取

            // 启动下载服务
            downloadService = new AriaDownloadService(DownloadingList, DownloadedList);
            downloadService.Start();

            return Container.Resolve<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 关闭下载服务
            downloadService.End();

            base.OnExit(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // pages
            containerRegistry.RegisterForNavigation<ViewIndex>(ViewIndexViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewLogin>(ViewLoginViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewVideoDetail>(ViewVideoDetailViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewSettings>(ViewSettingsViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewDownloadManager>(ViewDownloadManagerViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewToolbox>(ViewToolboxViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewUserSpace>(ViewUserSpaceViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewMySpace>(ViewMySpaceViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewPublicFavorites>(ViewPublicFavoritesViewModel.Tag);

            // downloadManager pages
            containerRegistry.RegisterForNavigation<ViewDownloading>(ViewDownloadingViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewDownloadFinished>(ViewDownloadFinishedViewModel.Tag);

            // settings pages
            containerRegistry.RegisterForNavigation<ViewBasic>(ViewBasicViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewNetwork>(ViewNetworkViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewVideo>(ViewVideoViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewDanmaku>(ViewDanmakuViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewAbout>(ViewAboutViewModel.Tag);

            // tools pages
            containerRegistry.RegisterForNavigation<ViewBiliHelper>(ViewBiliHelperViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewDelogo>(ViewDelogoViewModel.Tag);
            containerRegistry.RegisterForNavigation<ViewExtractMedia>(ViewExtractMediaViewModel.Tag);

            // dialogs
            containerRegistry.RegisterDialog<ViewDownloadSetter>(ViewDownloadSetterViewModel.Tag);
            containerRegistry.RegisterDialog<ViewParsingSelector>(ViewParsingSelectorViewModel.Tag);

        }

        /// <summary>
        /// 异步修改绑定到UI的属性
        /// </summary>
        /// <param name="callback"></param>
        public static void PropertyChangeAsync(Action callback)
        {
            Current.Dispatcher.Invoke(callback);
        }

    }
}
