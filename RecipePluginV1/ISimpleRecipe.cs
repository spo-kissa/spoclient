using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Plugin.Recipe.V1
{
    public interface ISimpleRecipe : IRecipe
    {
        string Command { get; }
    }
}
