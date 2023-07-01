using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalDictionary
{
    public static class Prefab
    {
        public static string rootPath = $"Prefabs";
        public static class Unit
        {
            public static string rootPath = $"{Prefab.rootPath}/Unit";
            public static Dictionary<string, UnitController> data = new Dictionary<string, UnitController>();
        }
    }
    public static class Materials
    {
        public static string rootPath = $"Materials";
        public static Dictionary<string, Material> data = new Dictionary<string, Material>();
    }
}
