using Assets.Scripts.Battle;
using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 광역으로 사용할 Resources에서 로드한 데이터 관리용 클래스
/// </summary>
public static class GlobalDictionary
{
    /// <summary>
    /// 맵에서 스크린 법선 벡터
    /// </summary>
    public static Vector3 VectorToScreen = new Vector3(0, Mathf.Sqrt(3f), -1);

    /// <summary>
    /// 레이어
    /// </summary>
    public static class Layer
    {
        public static int
            UI = 1 << 5,
            Map = 1 << 6,
            Unit = 1 << 7
            ;
    }
    /// <summary>
    /// 프리펩
    /// </summary>
    public static class Prefab
    {
        public static string rootPath = $"Prefabs";
        public static class Unit
        {
            public static string rootPath = $"{GlobalDictionary.Prefab.rootPath}/Unit";
            public static UnitController Prefab;
            public static Dictionary<string, UnitController> data = new Dictionary<string, UnitController>();
        }
        public static class UI
        {
            public static string rootPath = $"{GlobalDictionary.Prefab.rootPath}/UI";
            public static Dictionary<string, Transform> data = new Dictionary<string, Transform>();
        }
        public static class Battle
        {
            public static string rootPath = $"{GlobalDictionary.Prefab.rootPath}/Battle";
            public static ProjectileController Projectile;
        }
        public static class Effect
        {
            public static string rootPath = $"{GlobalDictionary.Prefab.rootPath}/Effect";
            public static Dictionary<string, EffectController> data = new Dictionary<string, EffectController>();
        }
    }

    /// <summary>
    /// 메테리얼
    /// </summary>
    public static class Materials
    {
        public static string rootPath = $"Materials";
        public static Dictionary<string, Material> data = new Dictionary<string, Material>();
        public static class Unit
        {
            public static string rootPath = $"{Materials.rootPath}Materials";
            public static Dictionary<string, Material> data = new Dictionary<string, Material>();
        }
    }

    /// <summary>
    /// 메쉬
    /// </summary>
    public static class Mesh
    {
        public static string rootPath = $"Meshs";
        public static Dictionary<string, UnityEngine.Mesh> data = new Dictionary<string, UnityEngine.Mesh>();
    }

    /// <summary>
    /// 텍스처, 스프라이트
    /// </summary>
    public static class Texture
    {
        public static string rootPath = $"Textures";
        public static class Unit
        {
            public static string rootPath = $"{Texture.rootPath}/Unit";
            public static Dictionary<string, Sprite> data = new Dictionary<string, Sprite>();
        }
    }
}
