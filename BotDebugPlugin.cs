using System;
using System.Reflection;
using Aki.Reflection.Patching;
using BepInEx;
using DrakiaXYZ.BotDebug.Components;
using DrakiaXYZ.BotDebug.Helpers;
using DrakiaXYZ.BotDebug.VersionChecker;
using EFT;
using HarmonyLib;
using UnityEngine;

namespace DrakiaXYZ.BotDebug
{
    [BepInPlugin("xyz.drakia.botdebug", "DrakiaXYZ-BotDebug", "1.1.0")]
    [BepInDependency("com.spt-aki.core", "3.6.0")]
    [BepInDependency("xyz.drakia.bigbrain", "0.2.0")]
    public class BotDebugPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            if (!TarkovVersion.CheckEftVersion(Logger, Info, Config))
            {
                throw new Exception($"Invalid EFT Version");
            }

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
