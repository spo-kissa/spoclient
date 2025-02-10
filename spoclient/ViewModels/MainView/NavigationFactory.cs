using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using spoclient.Pages;

namespace spoclient.ViewModels.MainView
{
    public class NavigationFactory(MainViewViewModel owner) : INavigationPageFactory
    {
        public MainViewViewModel Owner { get; } = owner;



        public Control? GetPage(Type srcType)
        {
            return null;
        }


        public Control? GetPageFromObject(object target)
        {
            if (target is HomePageViewModel)
            {
                return new HomePage
                {
                    DataContext = target
                };
            }
            return ResolvePage(target as PageBaseViewModel);
        }


        private Control? ResolvePage(PageBaseViewModel? pbvm)
        {
            if (pbvm is null)
            {
                return null;
            }

            return null;
        }
    }
}
