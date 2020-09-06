using System;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using EntityStates.Captain.Weapon;
using R2API.Utils;
using RoR2.UI;
using RoR2;
using On_ChargeCaptainShotgun = On.EntityStates.Captain.Weapon.ChargeCaptainShotgun;

namespace CaptainShotgunModes
{
    enum FireMode { Normal, Auto, AutoCharge, Count }

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("de.userstorm.captainshotgunmodes", "CaptainShotgunModes", "{VERSION}")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class CaptainShotgunModesPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<string> DefaultMode { get; set; }
        public static ConfigEntry<bool> EnableModeSelectionWithNumberKeys { get; set; }
        public static ConfigEntry<bool> EnableModeSelectionWithMouseWheel { get; set; }
        public static ConfigEntry<bool> EnableModeSelectionWithDPad { get; set; }

        private FireMode fireMode = FireMode.Normal;
        private float fixedAge = 0;

        private void SingleFireMode(On_ChargeCaptainShotgun.orig_FixedUpdate orig, ChargeCaptainShotgun self)
        {
            orig.Invoke(self);

            if (self.GetFieldValue<bool>("released"))
            {
                fixedAge = 0;
            }
        }

        private void AutoFireMode(On_ChargeCaptainShotgun.orig_FixedUpdate orig, ChargeCaptainShotgun self)
        {
            var didFire = false;
            var released = self.GetFieldValue<bool>("released");

            if (!released)
            {
                didFire = true;
                fixedAge = 0;
                self.SetFieldValue("released", true);
            }

            orig.Invoke(self);

            if (didFire)
            {
                self.SetFieldValue("released", false);
            }
        }

        private void AutoFireChargeMode(On_ChargeCaptainShotgun.orig_FixedUpdate orig, ChargeCaptainShotgun self)
        {
            var didFire = false;
            var released = self.GetFieldValue<bool>("released");
            var chargeDuration = self.GetFieldValue<float>("chargeDuration");

            if (!released && fixedAge >= chargeDuration)
            {
                didFire = true;
                fixedAge = 0;
                self.SetFieldValue("released", true);
            }

            orig.Invoke(self);

            if (didFire)
            {
                self.SetFieldValue("released", false);
            }
        }

        private void CycleFireMode (bool forward = true)
        {
            FireMode newFireMode = fireMode + (forward ? 1 : -1);

            if (newFireMode == FireMode.Count)
            {
                newFireMode = FireMode.Normal;
            }

            if ((int)newFireMode == -1)
            {
                newFireMode = FireMode.Count - 1;
            }

            fireMode = newFireMode;
        }

        private void InitConfig()
        {
            DefaultMode = Config.Bind<string>(
                "Settings",
                "DefaultMode",
                "Normal",
                "The mode that is selected on game start. Modes: Normal, Auto, AutoCharge"
            );

            EnableModeSelectionWithNumberKeys = Config.Bind<bool>(
               "Settings",
               "EnableModeSelectionWithNumberKeys",
               true,
               "When set to true modes can be selected using the number keys"
            );

            EnableModeSelectionWithMouseWheel = Config.Bind<bool>(
               "Settings",
               "EnableModeSelectionWithMouseWheel",
               true,
               "When set to true modes can be cycled through using the mouse wheel"
            );


            EnableModeSelectionWithDPad = Config.Bind<bool>(
               "Settings",
               "EnableModeSelectionWithDPad",
               true,
               "When set to true modes can be cycled through using the DPad (controller)"
            );
        }

        private void HandleConfig()
        {
            try
            {
                fireMode = (FireMode)Enum.Parse(typeof(FireMode), DefaultMode.Value);
            }
            catch (Exception)
            {
                fireMode = FireMode.Normal;
            }
        }

        private void SelectFireModeWithNumberKeys() {
            if (!EnableModeSelectionWithNumberKeys.Value) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                fireMode = FireMode.Normal;

                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                fireMode = FireMode.Auto;

                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                fireMode = FireMode.AutoCharge;
            }
        }

        private void SelectFireModeWithMouseWheel() {
            if (!EnableModeSelectionWithMouseWheel.Value) {
                return;
            }

            float wheel = Input.GetAxis("Mouse ScrollWheel");

            if (wheel == 0) {
                return;
            }

            // scroll down => forward; scroll up => backward
            CycleFireMode(wheel < 0f);
        }

        private void SelectFireModeWithDPad()
        {
            if (!EnableModeSelectionWithDPad.Value) {
                return;
            }

            if (DPad.GetInputDown(DPadInput.Right) || DPad.GetInputDown(DPadInput.Down))
            {
                CycleFireMode();

                return;
            }

            if (DPad.GetInputDown(DPadInput.Left) || DPad.GetInputDown(DPadInput.Up))
            {
                CycleFireMode(false);
            }
         }

        private void SelectFireMode()
        {
            SelectFireModeWithNumberKeys();
            SelectFireModeWithMouseWheel();
            SelectFireModeWithDPad();
        }

        public void FixedUpdateHook(On_ChargeCaptainShotgun.orig_FixedUpdate orig, ChargeCaptainShotgun self)
        {
            fixedAge += Time.fixedDeltaTime;

            switch (fireMode)
            {
                case FireMode.Normal:
                    SingleFireMode(orig, self);
                    break;
                case FireMode.Auto:
                    AutoFireMode(orig, self);
                    break;
                case FireMode.AutoCharge:
                    AutoFireChargeMode(orig, self);
                    break;
                default:
                    // fallback to single fire mode
                    SingleFireMode(orig, self);
                    break;
            }
        }

        public void UpdateHook(On.RoR2.UI.SkillIcon.orig_Update orig, SkillIcon self)
        {
            orig.Invoke(self);

            if (self.targetSkill && self.targetSkillSlot == SkillSlot.Primary)
            {
                int bodyIndex = self.targetSkill.characterBody.bodyIndex;
                SurvivorIndex survivorIndex = SurvivorCatalog.GetSurvivorIndexFromBodyIndex(bodyIndex);

                if (survivorIndex == SurvivorIndex.Captain)
                {
                    self.stockText.gameObject.SetActive(true);
                    self.stockText.fontSize = 12f;
                    self.stockText.SetText(fireMode.ToString());
                }
            }
        }

        public void Awake()
        {
            InitConfig();
            HandleConfig();

            On_ChargeCaptainShotgun.FixedUpdate += FixedUpdateHook;
            On.RoR2.UI.SkillIcon.Update += UpdateHook;
        }

        public void Update()
        {
            DPad.Update();

            SelectFireMode();
        }

        public void OnDestroy()
        {
            On_ChargeCaptainShotgun.FixedUpdate -= FixedUpdateHook;
            On.RoR2.UI.SkillIcon.Update -= UpdateHook;
        }
    }
}
