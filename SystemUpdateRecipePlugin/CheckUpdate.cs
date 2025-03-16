namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    public class CheckUpdate : ISimpleRecipe
    {
        public string Name => "アップデートをチェック";

        public string Description => "最新のアップデートを確認します";

        public string Command => "sudo apt update -y";

        public bool UseSudo => true;
    }
}
