using Prism.Commands;
using Prism.Mvvm;

namespace spoclient.ViewModels
{
    /// <summary>
    ///     メインタブビューモデルの基底抽象クラス。
    ///     メインタブに表示するビューのビューモデルはこのクラスを継承してください。
    /// </summary>
    public abstract class MainTabViewModel : BindableBase
    {
        /// <summary>
        ///     タブを閉じる時のデリゲート。
        /// </summary>
        /// <param name="source"></param>
        public delegate void RequestCloseEventHandler(MainTabViewModel source);

        /// <summary>
        ///     このタブを閉じようとする時に発生するイベント
        /// </summary>
        public abstract event RequestCloseEventHandler RequestClose;


        /// <summary>
        ///     タブのタイトルを取得します。
        /// </summary>
        public abstract string Header { get; }


        /// <summary>
        ///     タブの内容を取得または設定します。
        /// </summary>
        public object? Content { get; set; } = null;


        /// <summary>
        ///     タブを閉じるコマンド。
        /// </summary>
        public abstract DelegateCommand CloseCommand { get; }
    }
}
