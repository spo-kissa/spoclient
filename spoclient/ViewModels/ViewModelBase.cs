using Prism.Mvvm;

namespace spoclient.ViewModels
{
    /// <summary>
    ///    ビューモデルの基底クラス
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        /// <summary>
        ///     ビューのタイトル
        /// </summary>
        protected string title = string.Empty;


        /// <summary>
        ///     ビューのタイトルを取得または設定します。
        /// </summary>
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
