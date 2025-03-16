using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    class ListUpgradable : ISimpleRecipe
    {
        public string Name => "アップデートチェック";

        public string Description => "アップデート可能なパッケージの一覧を取得します";

        public string Command => "apt list --upgradable";
    }
}
