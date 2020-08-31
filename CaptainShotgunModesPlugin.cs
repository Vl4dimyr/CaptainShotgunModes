using BepInEx;
using UnityEngine;
using EntityStates.Captain.Weapon;
using R2API.Utils;
using RoR2.UI;
using RoR2;

namespace CaptainShotgunModes
{
    enum FireMode { Normal, Auto, AutoCharge }

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("de.userstorm.captainshotgunmodes", "CaptainShotgunModes", "{VERSION}")]
    public class CaptainShotgunModesPlugin : BaseUnityPlugin
    {
        private FireMode fireMode = FireMode.Normal;
        private float fixedAge = 0;

        public void FixedUpdateHook(
            On.EntityStates.Captain.Weapon.ChargeCaptainShotgun.orig_FixedUpdate orig,
            ChargeCaptainShotgun self
        )
        {
            fixedAge += Time.fixedDeltaTime;

            if (fireMode.Equals(FireMode.Normal))
            {
                orig.Invoke(self);

                if (self.GetFieldValue<bool>("released")) {
                    fixedAge = 0;
                }

                return;
            }

            bool didFire = false;
            bool charge = fireMode.Equals(FireMode.AutoCharge);
            bool released = self.GetFieldValue<bool>("released");
            float chargeDuration = self.GetFieldValue<float>("chargeDuration");

            if (!released && (!charge || (charge && fixedAge >= chargeDuration)))
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

        public void UpdateHook(On.RoR2.UI.SkillIcon.orig_Update orig, SkillIcon self)
        {
            orig.Invoke(self);

            if (self.targetSkill && self.targetSkillSlot.Equals(SkillSlot.Primary))
            {
                SurvivorIndex survivorIndex =
                    SurvivorCatalog.GetSurvivorIndexFromBodyIndex(self.targetSkill.characterBody.bodyIndex);

                if (survivorIndex.Equals(SurvivorIndex.Captain))
                {
                    self.stockText.gameObject.SetActive(true);
                    self.stockText.fontSize = 12f;
                    self.stockText.SetText(fireMode.ToString());
                }
            }
        }

        public void Awake()
        {
            On.EntityStates.Captain.Weapon.ChargeCaptainShotgun.FixedUpdate += FixedUpdateHook;
            On.RoR2.UI.SkillIcon.Update += UpdateHook;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                switch (fireMode)
                {
                    case FireMode.Normal:
                        fireMode = FireMode.Auto;
                        break;

                    case FireMode.Auto:
                        fireMode = FireMode.AutoCharge;
                        break;

                    case FireMode.AutoCharge:
                        fireMode = FireMode.Normal;
                        break;

                    default:
                        // this fire mode isn't implemented yet!
                        fireMode = FireMode.Normal;
                        break;
                }
            }

            // TODO: add gamepad button to cycle through fire modes
        }

        public void OnDestroy()
        {
            On.EntityStates.Captain.Weapon.ChargeCaptainShotgun.FixedUpdate -= FixedUpdateHook;
            On.RoR2.UI.SkillIcon.Update -= UpdateHook;
        }
    }
}
