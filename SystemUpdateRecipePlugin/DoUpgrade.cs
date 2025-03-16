namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    public class DoUpgrade : ISimpleRecipe
    {
        public string Name => "アップデート";

        public string Description => "アップデートを実行します";

        public string Command => "sudo apt upgrade -y";

        public bool UseSudo => true;
    }
}
