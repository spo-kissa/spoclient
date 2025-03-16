using System.Diagnostics;
using System.Reflection;

namespace SpoClient.Plugin.Recipe.V1
{
    public abstract class RecipePlugin : IRecipePlugin
    {
        public abstract string GetName();


        public abstract string GetDescription();


        public abstract string GetAuthorName();


        public abstract Version GetVersion();


        public static Version GetVersion(Assembly assembly)
        {
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart, fvi.ProductPrivatePart);
        }


        public abstract IReadOnlyList<Type> GetRecipes();
    }
}
