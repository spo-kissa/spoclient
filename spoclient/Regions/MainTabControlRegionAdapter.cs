using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using Prism.Regions;
using spoclient.Views;
using System;
using System.Collections.Specialized;

namespace spoclient.Regions
{
    public class MainTabControlRegionAdapter : RegionAdapterBase<TabView>
    {
        private IMainTabViewItem? startupTab = null;


        public MainTabControlRegionAdapter(IRegionBehaviorFactory factory)
            : base(factory)
        {
        }


        protected override void Adapt(IRegion region, TabView regionTarget)
        {
            region.Views.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    var items = regionTarget.TabItems;

                    if (args.NewItems is null)
                    {
                        return;
                    }
                    foreach (IMainTabViewItem tab in args.NewItems)
                    {
                        if (tab.TabItemIndex > items.Count())
                        {
                            //regionTarget.TabItems..Add(tab);
                        }
                        else
                        {
                            //items.Insert(tab.TabItemIndex, tab);
                        }

                        if (tab.IsStartupTab)
                        {
                            if (tab != startupTab && startupTab == null)
                            {
                                throw new InvalidOperationException("More than one tab is the startup tab.");
                            }

                            startupTab = tab;

                            regionTarget.SelectedItem = tab;
                        }
                    }
                }
            };
        }


        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
