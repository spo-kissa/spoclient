using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Plugin.Recipe.V1
{
    public interface IRecipePlugin
    {
        string Name { get; }


        string Description { get; }


        string AuthorName { get; }


        Version Version { get; }


        IReadOnlyList<Type> GetRecipes();
    }
}
