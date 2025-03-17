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
        public override string Name => "System Update Plugin";


        /// <summary>
        ///     プラグインの概要を取得します。
        /// </summary>
        /// <returns></returns>
        public override string Description => "Ubuntu Operating System Update Packages.";


        /// <summary>
        ///     プラグインの作成者を取得します。
        /// </summary>
        /// <returns></returns>
        public override string AuthorName => "Cardano SPO Kissa (DAISUKE)";


        /// <summary>
        ///     プラグインのバージョンを取得します。
        /// </summary>
        /// <returns></returns>
        public override Version Version => GetVersion(Assembly.GetExecutingAssembly());
        


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
                typeof(AutoRemovePackages),
                typeof(Reboot),
            };

            return list.AsReadOnly();
        }
    }
}
