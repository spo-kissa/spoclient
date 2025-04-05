﻿using Avalonia.Controls;
using DryIoc;
using LocalizationManager;
using Prism.Commands;
using Prism.Services.Dialogs;
using spoclient.Models;
using spoclient.ViewModels.MainView;
using spoclient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LocalizationManager.Avalonia;
using System.Globalization;

namespace spoclient.ViewModels
{
    public class MainViewViewModel : ViewModelBase
    {
        /// <summary>
        ///     ナビゲーションファクトリ
        /// </summary>
        public NavigationFactory NavigationFactory { get; }


        /// <summary>
        ///     接続の一覧
        /// </summary>
        public ObservableCollection<MainTabViewModel> Connections { get; } = [];


        /// <summary>
        ///     選択されているタブのインデックス
        /// </summary>
        public int? SelectedIndex
        {
            get => selectedIndex;
            set => SetProperty(ref selectedIndex, value);
        }


        /// <summary>
        ///    ファイルメニューのヘッダー
        /// </summary>
        public string FileMenuHeader => localization["File", "UI"];


        public string EditMenuHeader => localization["Edit", "UI"];


        public string ViewMenuHeader => localization["View", "UI"];


        public string HelpMenuHeader => localization["Help", "UI"];


        public string LanguageMenuHeader => localization["LanguageMenuHeader", "UI"];


        public string SystemLanguageMenuHeader => localization["SystemLanguageMenuHeader", "UI"];


        /// <summary>
        ///     選択されているタブのインデックス
        /// </summary>
        private int? selectedIndex;


        /// <summary>
        ///     ダイアログサービス
        /// </summary>
        private readonly IDialogService dialogService;


        /// <summary>
        ///     ローカライズマネージャ
        /// </summary>
        private readonly ILocalizationManager localization;


        /// <summary>
        ///     コンテナサービス
        /// </summary>
        private readonly IContainer container;


        /// <summary>
        ///     タブとビューモデルのマッチャー
        /// </summary>
        private Dictionary<Type, Type> TabViewModelMatcher { get; } = [];


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <param name="container">コンテナサービス</param>
        public MainViewViewModel(IDialogService dialogService, ILocalizationManager localization, IContainer container)
        {
            this.dialogService = dialogService;
            this.localization = localization;
            this.container = container;

            NavigationFactory = new NavigationFactory(this);

            // タブとビューモデルのマッチャーを設定
            TabViewModelMatcher.Add(typeof(ShellTabView), typeof(ShellTabViewViewModel));
        }


        /// <summary>
        ///     接続コマンド
        /// </summary>
        public DelegateCommand ConnectCommand => new(() =>
        {
            dialogService.ShowDialog(nameof(ServersDialog), dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>("ServerInfo");

                    var tuple = CreateTabView<ShellTabViewViewModel, ShellTabView>();
                    
                    if (tuple is not null && serverInfo is not null)
                    {
                        Connections.Add(tuple!.Value.ViewModel);
                        tuple!.Value.ViewModel.Connect(serverInfo);

                        tuple!.Value.ViewModel.RequestClose += TabViewRequestClose;

                        SelectedIndex = Connections.Count - 1;
                    }
                }
            });
        });


        public DelegateCommand ChangeLanguageEnglish => new(() =>
        {
            localization.CurrentCulture = new CultureInfo("en-US");
        });


        /// <summary>
        ///     タブを閉じる
        /// </summary>
        /// <param name="mainTabViewModel"></param>
        private void TabViewRequestClose(MainTabViewModel mainTabViewModel)
        {
            mainTabViewModel.RequestClose -= TabViewRequestClose;
            var index = Connections.IndexOf(mainTabViewModel);

            Connections.RemoveAt(index);
        }


        /// <summary>
        ///     バージョン情報ダイアログ
        /// </summary>
        public DelegateCommand ShowAboutDialogCommand => new(() =>
        {
            dialogService.ShowDialog(nameof(AboutDialog));
        });


        /// <summary>
        ///     タブビューを作成
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <returns></returns>
        private (TView View, TViewModel ViewModel)? CreateTabView<TViewModel, TView>()
            where TView : UserControl, IMainTabViewItem, new()
            where TViewModel : MainTabViewModel
        {
            var result = TabViewModelMatcher.ContainsKey(typeof(TView));
            if (result)
            {
                var viewModelType = TabViewModelMatcher[typeof(TView)];
                var viewModel = container.Resolve<TViewModel>(); // (TViewModel)Activator.CreateInstance(viewModelType)!;
                var view = Activator.CreateInstance<TView>();
                viewModel.Content = view;
                view.DataContext = viewModel;

                return (view, viewModel);
            }

            return null;
        }
    }
}
