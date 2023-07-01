using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 광역으로 사용할 Resources에서 로드한 데이터 관리용 클래스
/// </summary>
public static class GlobalDictionary
{
    public static class Layer
    {
        public static int
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
    }

    /// <summary>
    /// 메테리얼
    /// </summary>
    public static class Materials
    {
        public static string rootPath = $"Materials";
        public static Dictionary<string, Material> data = new Dictionary<string, Material>();
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
