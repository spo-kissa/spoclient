using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Prism.Events;
using spoclient.Events;

namespace spoclient.Views
{
    public static class ScrollViewerBehavior
    {
        public static readonly AttachedProperty<IEventAggregator?> EventAggregatorProperty =
            AvaloniaProperty.RegisterAttached<ScrollViewer, IEventAggregator?>(
                "EventAggregator",
                typeof(ScrollViewerBehavior));


        static ScrollViewerBehavior()
        {
            EventAggregatorProperty.Changed.AddClassHandler<ScrollViewer>((scrollViewer, args) =>
            {
                OnEventAggregatorChanged(scrollViewer, args.NewValue as IEventAggregator);
            });
        }


        public static void SetEventAggregator(AvaloniaObject element, IEventAggregator? value)
        {
            element.SetValue(EventAggregatorProperty, value);
        }


        public static IEventAggregator? GetEventAggregator(AvaloniaObject element)
        {
            return element.GetValue(EventAggregatorProperty);
        }


        private static void OnEventAggregatorChanged(ScrollViewer scrollViewer, IEventAggregator? eventAggregator)
        {
            eventAggregator?.GetEvent<ScrollEvent>().Subscribe(() =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        scrollViewer.ScrollToEnd();
                    });
                });
        }
    }
}
