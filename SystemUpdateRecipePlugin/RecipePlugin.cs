
using System.Diagnostics;
using System.Reflection;

namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    /// <summary>
    ///     プラグインの定義
    /// </summary>
    public class RecipePlugin : IRecipePlugin
    {
        /// <summary>
        ///     プラグイン名を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetName() => "System Update Plugin";


        /// <summary>
        ///     プラグインの概要を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetDescription() => "Ubuntu Operating System Update Packages.";


        /// <summary>
        ///     プラグインの作成者を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetAuthorName() => "Cardano SPO Kissa (DAISUKE)";


        /// <summary>
        ///     プラグインのバージョンを取得します。
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
        }


        /// <summary>
        ///     プラグインに含まれるレシピのリストを取得します。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<Type> GetRecipes()
        {
            var list = new List<Type>
            {
                typeof(CheckUpdate),
                typeof(ListUpgradable),
                typeof(DoUpgrade),
                typeof(CheckRebootRequired),
                typeof(CheckRebootRequiredPackages),
                typeof(Reboot),
            };

            return list.AsReadOnly();
        }
    }
}
