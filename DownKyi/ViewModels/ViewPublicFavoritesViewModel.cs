﻿using DownKyi.Core.Logging;
using DownKyi.Events;
using DownKyi.Images;
using DownKyi.Models;
using DownKyi.Services;
using DownKyi.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DownKyi.ViewModels
{
    public class ViewPublicFavoritesViewModel : BaseViewModel
    {
        public const string Tag = "PagePublicFavorites";

        #region 页面属性申明

        private VectorImage arrowBack;
        public VectorImage ArrowBack
        {
            get { return arrowBack; }
            set { SetProperty(ref arrowBack, value); }
        }

        private VectorImage play;
        public VectorImage Play
        {
            get { return play; }
            set { SetProperty(ref play, value); }
        }

        private VectorImage like;
        public VectorImage Like
        {
            get { return like; }
            set { SetProperty(ref like, value); }
        }

        private VectorImage favorite;
        public VectorImage Favorite
        {
            get { return favorite; }
            set { SetProperty(ref favorite, value); }
        }

        private VectorImage share;
        public VectorImage Share
        {
            get { return share; }
            set { SetProperty(ref share, value); }
        }

        private Favorites favorites;
        public Favorites Favorites
        {
            get { return favorites; }
            set { SetProperty(ref favorites, value); }
        }

        private List<FavoritesMedia> favoritesMedia;
        public List<FavoritesMedia> FavoritesMedia
        {
            get { return favoritesMedia; }
            set { SetProperty(ref favoritesMedia, value); }
        }

        private Visibility contentVisibility;
        public Visibility ContentVisibility
        {
            get { return contentVisibility; }
            set { SetProperty(ref contentVisibility, value); }
        }

        private Visibility noDataVisibility;
        public Visibility NoDataVisibility
        {
            get { return noDataVisibility; }
            set { SetProperty(ref noDataVisibility, value); }
        }

        #endregion

        public ViewPublicFavoritesViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            #region 属性初始化

            ArrowBack = NavigationIcon.Instance().ArrowBack;
            ArrowBack.Fill = DictionaryResource.GetColor("ColorTextDark");

            Play = NormalIcon.Instance().Play;
            Play.Fill = DictionaryResource.GetColor("ColorTextGrey2");

            Like = NormalIcon.Instance().Like;
            Like.Fill = DictionaryResource.GetColor("ColorTextGrey2");

            Favorite = NormalIcon.Instance().Favorite;
            Favorite.Fill = DictionaryResource.GetColor("ColorTextGrey2");

            Share = NormalIcon.Instance().Share;
            Share.Fill = DictionaryResource.GetColor("ColorTextGrey2");

            #endregion
        }

        #region 命令申明

        // 返回
        private DelegateCommand backSpaceCommand;
        public DelegateCommand BackSpaceCommand => backSpaceCommand ?? (backSpaceCommand = new DelegateCommand(ExecuteBackSpace));

        /// <summary>
        /// 返回
        /// </summary>
        private void ExecuteBackSpace()
        {
            NavigationParam parameter = new NavigationParam
            {
                ViewName = ParentView,
                ParentViewName = null,
                Parameter = null
            };
            eventAggregator.GetEvent<NavigationEvent>().Publish(parameter);
        }

        // 复制封面事件
        private DelegateCommand copyCoverCommand;
        public DelegateCommand CopyCoverCommand => copyCoverCommand ?? (copyCoverCommand = new DelegateCommand(ExecuteCopyCoverCommand));

        /// <summary>
        /// 复制封面事件
        /// </summary>
        private void ExecuteCopyCoverCommand()
        {
            // 复制封面图片到剪贴板
            Clipboard.SetImage(Favorites.Cover);
            LogManager.Info(Tag, "复制封面图片到剪贴板");
        }

        // 复制封面URL事件
        private DelegateCommand copyCoverUrlCommand;
        public DelegateCommand CopyCoverUrlCommand => copyCoverUrlCommand ?? (copyCoverUrlCommand = new DelegateCommand(ExecuteCopyCoverUrlCommand));

        /// <summary>
        /// 复制封面URL事件
        /// </summary>
        private void ExecuteCopyCoverUrlCommand()
        {
            // 复制封面url到剪贴板
            Clipboard.SetText(Favorites.CoverUrl);
            LogManager.Info(Tag, "复制封面url到剪贴板");
        }

        // 前往UP主页事件
        private DelegateCommand upperCommand;
        public DelegateCommand UpperCommand => upperCommand ?? (upperCommand = new DelegateCommand(ExecuteUpperCommand));

        /// <summary>
        /// 前往UP主页事件
        /// </summary>
        private void ExecuteUpperCommand()
        {
            NavigateToView.NavigateToViewUserSpace(eventAggregator, Tag, Favorites.UpperMid);
        }

        // 添加选中项到下载列表事件
        private DelegateCommand addToDownloadCommand;
        public DelegateCommand AddToDownloadCommand => addToDownloadCommand ?? (addToDownloadCommand = new DelegateCommand(ExecuteAddToDownloadCommand));

        /// <summary>
        /// 添加选中项到下载列表事件
        /// </summary>
        private void ExecuteAddToDownloadCommand()
        {
        }

        // 添加所有视频到下载列表事件
        private DelegateCommand addAllToDownloadCommand;
        public DelegateCommand AddAllToDownloadCommand => addAllToDownloadCommand ?? (addAllToDownloadCommand = new DelegateCommand(ExecuteAddAllToDownloadCommand));

        /// <summary>
        /// 添加所有视频到下载列表事件
        /// </summary>
        private void ExecuteAddAllToDownloadCommand()
        {
        }

        #endregion

        /// <summary>
        /// 初始化页面元素
        /// </summary>
        private void InitView()
        {
            LogManager.Debug(Tag, "初始化页面元素");

            ContentVisibility = Visibility.Collapsed;
            NoDataVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 更新页面
        /// </summary>
        private void UpdateView(IFavoritesService favoritesService, long favoritesId)
        {
            Favorites = favoritesService.GetFavorites(favoritesId);
            if (Favorites == null)
            {
                LogManager.Debug(Tag, "Favorites is null.");

                ContentVisibility = Visibility.Collapsed;
                NoDataVisibility = Visibility.Visible;
                return;
            }
            else
            {
                ContentVisibility = Visibility.Visible;
                NoDataVisibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 接收收藏夹id参数
        /// </summary>
        /// <param name="navigationContext"></param>
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            InitView();

            // 根据传入参数不同执行不同任务
            long parameter = navigationContext.Parameters.GetValue<long>("Parameter");
            if (parameter == 0)
            {
                return;
            }

            await Task.Run(new Action(() =>
            {
                UpdateView(new FavoritesService(), parameter);
            }));

        }
    }
}
