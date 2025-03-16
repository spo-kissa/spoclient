using System.Reflection;

namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    /// <summary>
    ///     プラグインの定義
    /// </summary>
    public class SystemUpdateRecipePlugin : RecipePlugin
    {
        /// <summary>
        ///     プラグイン名を取得します。
        /// </summary>
        /// <returns></returns>
        public override string GetName() => "System Update Plugin";


        /// <summary>
        ///     プラグインの概要を取得します。
        /// </summary>
        /// <returns></returns>
        public override string GetDescription() => "Ubuntu Operating System Update Packages.";


        /// <summary>
        ///     プラグインの作成者を取得します。
        /// </summary>
        /// <returns></returns>
        public override string GetAuthorName() => "Cardano SPO Kissa (DAISUKE)";


        /// <summary>
        ///     プラグインのバージョンを取得します。
        /// </summary>
        /// <returns></returns>
        public override Version GetVersion()
        {
            return GetVersion(Assembly.GetExecutingAssembly());
        }


        /// <summary>
        ///     プラグインに含まれるレシピのリストを取得します。
        /// </summary>
        /// <returns></returns>
        public override IReadOnlyList<Type> GetRecipes()
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
