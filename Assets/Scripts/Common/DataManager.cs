using Assets.Scripts.Battle;
using Assets.Scripts.Dungeon;
using Assets.Scripts.Monster;
using Assets.Scripts.Server;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.Fragment.Section.GearUpgrade;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Resources 관리용 매니저
    /// </summary>
    public class DataManager : SingletonObject<DataManager>
    {

        public void LoadLocalDatas()
        {
            LoadSavingData();
            LoadPrefabs();
            LoadMaterials();
            LoadTextures();
            LoadMeshs();
            LoadScriptables();
        }

        /// <summary>
        /// 로컬 저장 정보 로드 함수
        /// </summary>
        public void LoadSavingData()
        {
            ServerData.Saving = Resources.Load<SavingData>($"{ServerData.rootPath}/Server/Base");
            ServerData.Saving.GoldUpgradeLevel = new Dictionary<TowerType, Dictionary<TowerUpgradeType, int>>()
                {
                    {TowerType.Machine, new Dictionary<TowerUpgradeType, int>()
                    {
                        { TowerUpgradeType.AttackSpeed, ServerData.Saving.GoldLvMachine[0] },
                        { TowerUpgradeType.Range, ServerData.Saving.GoldLvMachine[1] },
                        { TowerUpgradeType.Damage, ServerData.Saving.GoldLvMachine[2] },
                    } },
                    {TowerType.Magic, new Dictionary<TowerUpgradeType, int>()
                    {
                        { TowerUpgradeType.AttackSpeed, ServerData.Saving.GoldLvMagic[0] },
                        { TowerUpgradeType.Range, ServerData.Saving.GoldLvMagic[1] },
                        { TowerUpgradeType.Damage, ServerData.Saving.GoldLvMagic[2] },
                    } },
                    {TowerType.Bio, new Dictionary<TowerUpgradeType, int>()
                    {
                        { TowerUpgradeType.AttackSpeed, ServerData.Saving.GoldLvBio[0] },
                        { TowerUpgradeType.Range, ServerData.Saving.GoldLvBio[1] },
                        { TowerUpgradeType.Damage, ServerData.Saving.GoldLvBio[2] },
                    } },
                };
            ServerData.Saving.GearUpgradeLevel = new Dictionary<GearUpgradeType, int>()
                {
                    {GearUpgradeType.Stone, ServerData.Saving.GearLv[0] },
                    {GearUpgradeType.Mining, ServerData.Saving.GearLv[1] },
                };
        }

        /// <summary>
        /// 프리펩 로드 함수
        /// </summary>
        public void LoadPrefabs()
        {
            // 사용할 소리
            ServerData.Sound = Resources.Load<PresetSound>($"{ServerData.rootPath}/Server/Sound");

            // Icon
            foreach (Transform unit in Resources.LoadAll<Transform>($"{GlobalDictionary.Prefab.Icon.rootPath}"))
            {
                GlobalDictionary.Prefab.Icon.data[unit.name] = unit;
            }

            // Effect
            foreach (EffectController unit in Resources.LoadAll<EffectController>($"{GlobalDictionary.Prefab.Effect.rootPath}"))
            {
                GlobalDictionary.Prefab.Effect.data[unit.name] = unit;
            }

            // 타워
            foreach (GameObject unit in Resources.LoadAll<GameObject>($"{GlobalDictionary.Mesh.Tower.rootPath}"))
            {
                GlobalDictionary.Prefab.Tower.data[unit.name] = unit;
            }
        }

        /// <summary>
        /// 메테리얼 로드 함수
        /// </summary>
        public void LoadMaterials()
        {
            foreach (Material unit in Resources.LoadAll<Material>($"{GlobalDictionary.Materials.rootPath}"))
            {
                GlobalDictionary.Materials.data[unit.name] = unit;
            }
        }

        /// <summary>
        /// 메쉬 로드 함수
        /// </summary>
        public void LoadMeshs()
        {
            // 타워
            //foreach (Mesh unit in Resources.LoadAll<Mesh>($"{GlobalDictionary.Mesh.Tower.rootPath}"))
            //{
            //    GlobalDictionary.Mesh.Tower.data[unit.name] = unit;
            //}
        }

        /// <summary>
        /// 텍스처 로드 함수
        /// </summary>
        public void LoadTextures()
        {
            // 기물 Sprite
            foreach (Sprite unit in Resources.LoadAll<Sprite>($"{GlobalDictionary.Texture.Tower.rootPath}"))
            {
                GlobalDictionary.Texture.Tower.data[unit.name] = unit;
            }
            // 몬스터 Sprite
            foreach (Sprite unit in Resources.LoadAll<Sprite>($"{GlobalDictionary.Texture.Monster.rootPath}"))
            {
                GlobalDictionary.Texture.Monster.data[unit.name] = unit;
            }
            // 아이콘 Sprite
            foreach (Sprite unit in Resources.LoadAll<Sprite>($"{GlobalDictionary.Texture.Icon.rootPath}"))
            {
                GlobalDictionary.Texture.Icon.data[unit.name] = unit;
            }
        }

        /// <summary>
        /// ScriptableObject 로드 함수
        /// </summary>
        public void LoadScriptables()
        {
            // 타워 정보
            foreach (TowerInfo unit in Resources.LoadAll<TowerInfo>($"{ServerData.Tower.rootPath}"))
            {
                ServerData.Tower.data[unit.name] = unit.SetCode(unit.name);
            }
            foreach (MonsterInfo unit in Resources.LoadAll<MonsterInfo>($"{ServerData.Monster.rootPath}"))
            {
                ServerData.Monster.data[unit.name] = unit.SetCode(unit.name);
            }

            // 던전 정보
            foreach (DungeonInfo unit in Resources.LoadAll<DungeonInfo>($"{ServerData.Dungeon.rootPath}"))
            {
                ServerData.Dungeon.data[unit.name] = unit.SetCode(unit.name);
            }
        }
    }
}
