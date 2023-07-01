using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Unit;

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
