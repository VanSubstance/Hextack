using Assets.Scripts.Battle;
using Assets.Scripts.Dungeon;
using Assets.Scripts.Tower;
using Assets.Scripts.Monster;
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
            LoadPrefabs();
            LoadMaterials();
            LoadTextures();
            LoadMeshs();
            LoadScriptables();
        }

        /// <summary>
        /// 프리펩 로드 함수
        /// </summary>
        public void LoadPrefabs()
        {
            // UI
            foreach (Transform unit in Resources.LoadAll<Transform>($"{GlobalDictionary.Prefab.UI.rootPath}"))
            {
                GlobalDictionary.Prefab.UI.data[unit.name] = unit;
            }

            // Effect
            foreach (EffectController unit in Resources.LoadAll<EffectController>($"{GlobalDictionary.Prefab.Effect.rootPath}"))
            {
                GlobalDictionary.Prefab.Effect.data[unit.name] = unit;
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
            foreach (Material unit in Resources.LoadAll<Material>($"{GlobalDictionary.Materials.Unit.rootPath}"))
            {
                GlobalDictionary.Materials.Unit.data[unit.name] = unit;
            }
        }

        /// <summary>
        /// 메쉬 로드 함수
        /// </summary>
        public void LoadMeshs()
        {
            // 타워
            foreach (Mesh unit in Resources.LoadAll<Mesh>($"{GlobalDictionary.Mesh.Tower.rootPath}"))
            {
                GlobalDictionary.Mesh.Tower.data[unit.name] = unit;
            }
        }

        /// <summary>
        /// 텍스처 로드 함수
        /// </summary>
        public void LoadTextures()
        {
            // 기물 Sprite
            foreach (Sprite unit in Resources.LoadAll<Sprite>($"{GlobalDictionary.Texture.Unit.rootPath}"))
            {
                GlobalDictionary.Texture.Unit.data[unit.name] = unit;
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
