using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;
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
            // 유닛
            foreach (UnitController unit in Resources.LoadAll<UnitController>($"{GlobalDictionary.Prefab.Unit.rootPath}"))
            {
                GlobalDictionary.Prefab.Unit.data[unit.name] = unit;
            }

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
            foreach (Mesh unit in Resources.LoadAll<Mesh>($"{GlobalDictionary.Mesh.rootPath}"))
            {
                GlobalDictionary.Mesh.data[unit.name] = unit;
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
            // 기물 정보
            foreach (UnitInfo unit in Resources.LoadAll<UnitInfo>($"{ServerData.Unit.rootPath}"))
            {
                ServerData.Unit.data[unit.Code] = unit;
            }

            // 던전 정보
            foreach (DungeonInfo dg in Resources.LoadAll<DungeonInfo>($"{ServerData.Dungeon.rootPath}"))
            {
                ServerData.Dungeon.DungeonList[dg.Code] = dg;
            }
        }
    }
}
