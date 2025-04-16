using McMaster.NETCore.Plugins;
using SpoClient.Plugin.Recipe.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace spoclient.Plugins.Recipe
{
    public class RecipeV1Loader
    {
        public static IEnumerable<IRecipePlugin> FindRecipePlugins()
        {
            var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Plugin");
            if (Directory.Exists(location))
            {
                var files = Directory.EnumerateFiles(location, "*.v1.dll", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    using var loader = PluginLoader.CreateFromAssemblyFile(file, false, [typeof(IRecipePlugin), typeof(IRecipe), typeof(ISimpleRecipe)]);
                    var plugin = loader.LoadDefaultAssembly();
                    foreach (var recipePluginType in plugin.GetTypes().Where(t => typeof(IRecipePlugin).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        var recipePlugin = Activator.CreateInstance(recipePluginType) as IRecipePlugin;
                        if (recipePlugin is not null)
                        {
                            Debug.WriteLine(recipePlugin.Name);
                            yield return recipePlugin;
                        }
                    }
                }
            }
        }


        public static IEnumerable<IRecipe> GetRecipes(IEnumerable<IRecipePlugin> recipePlugins)
        {
            foreach (var plugin in recipePlugins)
            {
                foreach (var recipeType in plugin.GetRecipes())
                {
                    var recipe = Activator.CreateInstance(recipeType) as IRecipe;
                    if (recipe is not null)
                    {
                        yield return recipe;
                    }
                }
            }
        }


        public static void RegisterRecipes(IEnumerable<IRecipe> recipes)
        {
            RecipeV1Loader.recipes.AddRange(recipes);
        }


        public static IRecipe[] Recipes => [.. recipes]; 


        private static readonly List<IRecipe> recipes = [];
    }
}
