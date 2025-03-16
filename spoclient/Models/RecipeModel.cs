using SpoClient.Plugin.Recipe.V1;

namespace spoclient.Models
{
    public class RecipeModel
    {
        public int Index { get; private set; }


        public string Name { get; private set; }


        public string Description { get; private set; }


        public string Command { get; private set; }



        public RecipeModel(int index, IRecipe recipe)
        {
            Index = index;
            Name = recipe.Name;
            Description = recipe.Description;
            Command = recipe.Command;
        }
    }
}
