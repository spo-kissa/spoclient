namespace spoclient.Views
{
    public interface IMainTabViewItem
    {
        int TabItemIndex { get; }

        bool IsStartupTab { get; }
    }
}
