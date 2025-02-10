using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace spoclient.Service
{
    public class NavigationService
    {
        public static NavigationService Instance { get; } = new NavigationService();


        public Control? PreviousPage { get; set; }


        private Frame? frame;

        private Panel overlayHost;



        public void SetFrame(Frame frame)
        {
            this.frame = frame;
        }


        public void SetOverlayHost(Panel panel)
        {
            this.overlayHost = panel;
        }


        public void Navigate(Type type)
        {
            this.frame!.Navigate(type);
        }


        public void NavigateFronContext(object dataContext, NavigationTransitionInfo? transitionInfo = null)
        {
            this.frame!.NavigateFromObject(dataContext,
                new FluentAvalonia.UI.Navigation.FrameNavigationOptions
                {
                    IsNavigationStackEnabled = true,
                    TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
                });
        }


        public void ShowControlDefinitionOverlay(Type targetType)
        {
        }
    }
}
