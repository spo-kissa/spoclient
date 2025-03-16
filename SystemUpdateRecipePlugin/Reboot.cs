namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    class Reboot : ISimpleRecipe
    {
        public string Name => "再起動";

        public string Description => "再起動をおこないます";

        public string Command => "sudo reboot";

        public bool UseSudo => true;
    }
}
