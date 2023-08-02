using Assets.Scripts.Tower;
using TMPro;
using UnityEngine;
using Assets.Scripts.Battle.Projectile;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentTowerInfo : AbsWindowContent<TowerInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc, textEffect, textTier;

        [SerializeField]
        private Image image;

        public override AbsWindowContent<TowerInfo> CloseExtra()
        {
            ServerData.InGame.LastTowerClicked = null;
            return this;
        }

        public override AbsWindowContent<TowerInfo> Init(TowerInfo parameter)
        {
            image.sprite = GlobalDictionary.Texture.Tower.data[parameter.Code];
            image.preserveAspect = true;
            textTitle.text = parameter.Name;
            textDesc.text = parameter.Desc;
            textTier.text = $"티어 {parameter.Tier}\n\n{CommonFunction.TranslateTowerType(parameter.towerType)}";
            ProjectileInfo p = parameter.projectileInfo[0];
            string eft = $"";

            // 기본 효과
            eft += $"{string.Format("{0:0.##}", p.effectInfo.Cooltime)}초마다 {p.Range * (1 + (ServerData.Saving.GoldUpgradeLevel[parameter.towerType][TowerUpgradeType.Range] * .05f))} 거리 내 ";
            if (p.effectInfo.tokens.Length > 0)
            {
                foreach (DamageEffectInfo.Token tk in p.effectInfo.tokens)
                {
                    switch (tk.damageEffectType)
                    {
                        case DamageEffectType.Damage:
                            eft += $"{(int)(tk.Amount * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[parameter.towerType])))} 데미지, ";
                            break;
                        case DamageEffectType.Speed:
                            eft += $"{(tk.Amount * 100)}% 이동속도 감소, ";
                            break;
                    }
                }
                eft = eft.Substring(0, eft.Length - 2);
                eft += "의 효과를 가지며, ";
            }
            switch (p.executeType)
            {
                case ProjectileExecuteType.Bullet:
                    eft += $"{p.CountPerOnce} 대상에게 부여하는 투사체를 발사합니다.";
                    break;
                case ProjectileExecuteType.Instant:
                    eft += $"{p.CountPerOnce} 대상을 즉시 타격합니다.";
                    break;
                case ProjectileExecuteType.Aura:
                    eft += "모든 대상을 타격하는 아우라를 발산합니다.";
                    break;
                case ProjectileExecuteType.Lazer:
                    eft += $"{p.CountPerOnce} 대상에게 경로상의 모든 적을 타격하는 레이저를 발사합니다.";
                    break;
            }

            eft += "\n";

            // 이후 효과 있을 시
            AfterHitInfo a = p.afterHitInfo;
            switch (a.afterHitType)
            {
                case AfterHitType.Explosive:
                    // 폭발 = 일회성 아우라
                    eft += $"피격 후 {a.range * 2} 지름의 원의 대상에게 즉시 ";
                    foreach (DamageEffectInfo.Token tk in a.damageEffects.tokens)
                    {
                        switch (tk.damageEffectType)
                        {
                            case DamageEffectType.Damage:
                                eft += $"{(int)(tk.Amount * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[parameter.towerType])))} 데미지, ";
                                break;
                            case DamageEffectType.Speed:
                                eft += $"{(tk.Amount * 100)}% 이동속도 감소, ";
                                break;
                        }
                    }
                    eft = eft.Substring(0, eft.Length - 2);
                    eft += "를 부여합니다.";
                    break;
                case AfterHitType.Area:
                    eft += $"피격 후 {a.duration}초 유지되는 {a.range * 2} 지름의 원의 대상에게 {string.Format("{0:0..##}", a.damageEffects.Cooltime)}초마다 ";
                    foreach (DamageEffectInfo.Token tk in a.damageEffects.tokens)
                    {
                        switch (tk.damageEffectType)
                        {
                            case DamageEffectType.Damage:
                                eft += $"{(int)(tk.Amount * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[parameter.towerType])))} 데미지, ";
                                break;
                            case DamageEffectType.Speed:
                                eft += $"{(tk.Amount * 100)}% 이동속도 감소, ";
                                break;
                        }
                    }
                    eft = eft.Substring(0, eft.Length - 2);
                    eft += "를 부여합니다.";
                    break;
            }

            textEffect.text = eft;
            gameObject.SetActive(true);
            return this;
        }
    }
}
