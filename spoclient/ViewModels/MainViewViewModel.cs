using Avalonia.Controls;
using DryIoc;
using Prism.Commands;
using Prism.Services.Dialogs;
using spoclient.Models;
using spoclient.Plugins.Recipe;
using spoclient.ViewModels.MainView;
using spoclient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        ///     選択されているタブのインデックス
        /// </summary>
        private int? selectedIndex;


        /// <summary>
        ///     ダイアログサービス
        /// </summary>
        private readonly IDialogService dialogService;


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
        public MainViewViewModel(IDialogService dialogService, IContainer container)
        {
            this.dialogService = dialogService;
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
