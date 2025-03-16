using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Plugin.Recipe.V1
{
    public interface IRecipePlugin
    {
        string GetName();


        string GetDescription();


        string GetAuthorName();


        Version GetVersion();


        IReadOnlyList<Type> GetRecipes();
    }
}
