namespace SpoClient.Plugin.Recipe.V1.SystemUpdate
{
    class CheckRebootRequired : ISimpleRecipe
    {
        public string Name => "再起動の要否チェック";

        public string Description => "アップデートによって再起動が必要かどうかを判定します";

        public string Command => "cat /run/reboot-required";

        public bool UseSudo => false;
    }
}
