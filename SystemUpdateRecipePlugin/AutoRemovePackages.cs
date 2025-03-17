namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    class AutoRemovePackages : ISimpleRecipe
    {
        public string Name => "自動削除";

        public string Description => "不要になったパッケージを自動的に削除します";

        public string Command => "sudo apt autoremove";

        public bool UseSudo => true;
    }
}
