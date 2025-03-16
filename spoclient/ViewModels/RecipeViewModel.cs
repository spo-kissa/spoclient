using Prism.Commands;
using spoclient.Models;
using spoclient.Terminals;
using SpoClient.Plugin.Recipe.V1;
using System;

namespace spoclient.ViewModels
{
    public class RecipeViewModel : ViewModelBase
    {
        public int Index { get; private set; }


        public string Name { get; private set; }


        public string Description { get; private set; }


        public string Command { get; private set; }


        public DelegateCommand DelegateCommand => new(() =>
        {
            if (GetConnection is null)
            {
                return;
            }

            var connection = GetConnection();

            if (connection is not null && connection.IsConnected)
            {
                connection.SendCommand(Command);
            }
        });


        public Func<SshConnection?>? GetConnection;


        public RecipeViewModel(RecipeModel model)
        {
            Index = model.Index;
            Name = model.Name;
            Description = model.Description;
            Command = model.Command;
        }


        public RecipeViewModel(IRecipe recipe)
        {
            Index = 0;
            Name = recipe.Name;
            Description = recipe.Description;
            Command = recipe.Command;
        }
    }
}
