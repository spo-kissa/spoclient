namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    class CheckRebootRequiredPackages : ISimpleRecipe
    {
        public string Name => "再起動を必要とするパッケージを確認";

        public string Description => "アップデートによって再起動を必要とするパッケージのリストを取得します";

        public string Command => "cat /run/reboot-required.pkgs";

        public bool UseSudo => false;
    }
}
