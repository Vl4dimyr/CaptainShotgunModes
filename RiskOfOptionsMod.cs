using System;
using System.IO;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using UnityEngine;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;

namespace CaptainShotgunModes
{
    static class RiskOfOptionsMod
    {
        private static bool? _enabled;

        internal static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
                }

                return (bool)_enabled;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void Init (string description)
        {
            ModSettingsManager.SetModDescription(description);

            using var stream = GetType().Assembly.GetManifestResourceStream("icon");
            var texture = new Texture2D(0, 0);
            var imgData = new byte[stream.Length];

            stream.Read(imgData, 0, imgData.Length);

            if (ImageConversion.LoadImage(texture, imgData))
            {
                ModSettingsManager.SetModIcon(
                    Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0))
                );
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void AddCheckboxOption(ConfigEntry<bool> configEntry)
        {
            ModSettingsManager.AddOption(new CheckBoxOption(configEntry));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void AddStepSliderOption(ConfigEntry<float> configEntry, float min, float max, float step)
        {
            StepSliderConfig config = new StepSliderConfig();

            config.min = min;
            config.max = max;
            config.increment = step;

            ModSettingsManager.AddOption(new StepSliderOption(configEntry, config));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void AddChoiceOption<T>(ConfigEntry<T> configEntry) where T : Enum
        {
            ModSettingsManager.AddOption(new ChoiceOption(configEntry));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void AddKeyBindOption(ConfigEntry<KeyboardShortcut> configEntry)
        {
            ModSettingsManager.AddOption(new KeyBindOption(configEntry));
        }
    }
}
