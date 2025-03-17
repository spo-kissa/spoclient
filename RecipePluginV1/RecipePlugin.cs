using System.Diagnostics;
using System.Reflection;

namespace SpoClient.Plugin.Recipe.V1
{
    public abstract class RecipePlugin : IRecipePlugin
    {
        public abstract string Name { get; }


        public abstract string Description { get; }


        public abstract string AuthorName { get; }


        public abstract Version Version { get; }


        public static Version GetVersion(Assembly assembly)
        {
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart, fvi.ProductPrivatePart);
        }


        public abstract IReadOnlyList<Type> GetRecipes();
    }
}
