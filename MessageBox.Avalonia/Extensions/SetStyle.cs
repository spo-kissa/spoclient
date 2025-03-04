using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

using MessageBoxSlim.Avalonia.Enums;

using System;

namespace MessageBoxSlim.Avalonia.Extensions
{
    internal static class SetStyleExtension
    {
        internal static void SetStyle(this Window window, BoxStyle style)
        {
            global::Avalonia.Styling.Styles styles = window.Styles;
            switch (style)
            {
                case BoxStyle.Windows:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/Windows/Windows.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/Windows/Windows.xaml")
                            });
                        break;
                    }
                case BoxStyle.MacOs:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/MacOs/MacOs.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/MacOs/MacOs.xaml")
                            });
                        break;
                    }
                case BoxStyle.UbuntuLinux:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/Ubuntu/Ubuntu.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/Ubuntu/Ubuntu.xaml")
                            });
                        break;
                    }
                case BoxStyle.MintLinux:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/Mint/Mint.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/Mint/Mint.xaml")
                            });
                        break;
                    }
                case BoxStyle.DarkMode:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/Dark/Dark.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/Dark/Dark.xaml")
                            });
                        break;
                    }
                case BoxStyle.RoundButtons:
                    {
                        styles
                            .Add(new StyleInclude(new Uri("avares://MessageBoxSlim.Avalonia/Styles/RoundButtons/RoundButtons.xaml"))
                            {
                                Source =
                                    new Uri("avares://MessageBoxSlim.Avalonia/Styles/RoundButtons/RoundButtons.xaml")
                            });
                        break;
                    }
            }
        }
    }
}
