using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Resources 관리용 매니저
    /// </summary>
    public class DataManager : SingletonObject<DataManager>
    {
        private new void Awake()
        {
            base.Awake();
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
            // 기물 이미지
            foreach (Sprite unit in Resources.LoadAll<Sprite>($"{GlobalDictionary.Texture.Unit.rootPath}"))
            {
                GlobalDictionary.Texture.Unit.data[unit.name] = unit;
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
                ServerData.Unit.data[unit.name] = unit;
            }
        }
    }
}
