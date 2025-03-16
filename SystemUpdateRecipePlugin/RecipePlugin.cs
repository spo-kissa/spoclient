
using System.Diagnostics;
using System.Reflection;

namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    public class RecipePlugin : IRecipePlugin
    {
        public string GetName() => "System Update Plugin";


        public string GetDescription() => "Ubuntu Operating System Update Packages.";


        public string GetAuthorName() => "Cardano SPO Kissa (DAISUKE)";


        public Version GetVersion()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
        }


        public IReadOnlyList<Type> GetRecipes()
        {
            var list = new List<Type>
            {
                typeof(ListUpgradable)
            };

            return list.AsReadOnly();
        }
    }
}
