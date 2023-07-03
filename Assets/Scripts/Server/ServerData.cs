using Assets.Scripts.Unit;
using System.Collections.Generic;

/// <summary>
/// 서버에서 넘겨받는 데이터들을 가정한 로컬 데이터
/// </summary>
public static class ServerData
{
    public static string rootPath = $"Datas";
    public static class Unit
    {
        public static string rootPath = $"{ServerData.rootPath}/Units";
        public static Dictionary<string, UnitInfo> data = new Dictionary<string, UnitInfo>();
    }
}