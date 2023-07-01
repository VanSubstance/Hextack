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
            LoadUnits();
            LoadMaterials();
            LoadTextures();
            LoadMeshs();
        }

        /// <summary>
        /// 유닛 프리펩 로드 함수
        /// </summary>
        public void LoadUnits()
        {
            foreach (UnitController unit in Resources.LoadAll<UnitController>($"{GlobalDictionary.Prefab.Unit.rootPath}"))
            {
                GlobalDictionary.Prefab.Unit.data[unit.name] = unit;
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
    }
}
