using Assets.Scripts.Map;
using Assets.Scripts.Monster;
using Assets.Scripts.Tower;
using System.Collections.Generic;

/// <summary>
/// 서버에서 넘겨받는 데이터들을 가정한 로컬 데이터
/// </summary>
public static class ServerData
{
    public static string rootPath = $"Datas";

    /// <summary>
    /// 기물 별 정보
    /// </summary>
    public static class Tower
    {
        public static string rootPath = $"{ServerData.rootPath}/Tower";
        public static Dictionary<string, TowerInfo> data = new Dictionary<string, TowerInfo>();
    }

    public static class Monster
    {
        public static string rootPath = $"{ServerData.rootPath}/Monster";
        public static Dictionary<string, MonsterInfo> data = new Dictionary<string, MonsterInfo>();
    }

    public static class User
    {
        public static int AmountGold, AmountArtifact;
    }

    /// <summary>
    /// 던전 정보
    /// </summary>
    public static class Dungeon
    {
        public static string rootPath = $"{ServerData.rootPath}/Maps";

        /// <summary>
        /// 접근 가능한 던전 리스트
        /// </summary>
        public static Dictionary<string, DungeonInfo> DungeonList = new Dictionary<string, DungeonInfo>();
    }

    /// <summary>
    /// 인게임에서 사용되는 정보
    /// </summary>
    public static class InGame
    {
        /// <summary>
        /// 전투에서 사용하는 맵 정보
        /// </summary>
        public static DungeonInfo DungeonInfo;

        /// <summary>
        /// 몬스터 정보
        /// </summary>
        public static MonsterToken[] MonsterInfo;
    }
}