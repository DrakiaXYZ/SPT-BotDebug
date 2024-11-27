using System;
using System.Reflection;
using SPT.Reflection.Patching;
using BepInEx;
using DrakiaXYZ.BotDebug.Components;
using DrakiaXYZ.BotDebug.Helpers;
using DrakiaXYZ.BotDebug.VersionChecker;
using EFT;

namespace DrakiaXYZ.BotDebug
{
    [BepInPlugin("xyz.drakia.botdebug", "DrakiaXYZ-BotDebug", "1.5.0")]
#if !STANDALONE
    [BepInDependency("com.SPT.core", "3.10.0")]
    [BepInDependency("xyz.drakia.bigbrain", "1.2.0")]
#endif
    public class BotDebugPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
#if !STANDALONE
            if (!TarkovVersion.CheckEftVersion(Logger, Info, Config))
            {
                throw new Exception($"Invalid EFT Version");
            }
#endif

            Settings.Init(Config);

            new NewGamePatch().Enable();
        }
    }

    // Add the debug component every time a match starts
    internal class NewGamePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));

        [PatchPrefix]
        public static void PatchPrefix()
        {
            BotDebugComponent.Enable();
        }
    }
}
