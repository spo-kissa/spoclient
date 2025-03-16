using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Plugin.Recipe.V1
{
    public interface IRecipe
    {
        string Name { get; }


        string Description { get; }


        string Command { get; }


        bool UseSudo { get; }
    }
}
