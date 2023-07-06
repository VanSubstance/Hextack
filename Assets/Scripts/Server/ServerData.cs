using Assets.Scripts.Unit;
using Assets.Scripts.Map;
using System.Collections.Generic;
using Assets.Scripts.Server;

/// <summary>
/// 서버에서 넘겨받는 데이터들을 가정한 로컬 데이터
/// </summary>
public static class ServerData
{
    public static string rootPath = $"Datas";

    /// <summary>
    /// 기물 별 정보
    /// </summary>
    public static class Unit
    {
        public static string rootPath = $"{ServerData.rootPath}/Units";
        public static Dictionary<string, UnitInfo> data = new Dictionary<string, UnitInfo>();
    }

    /// <summary>
    /// 유저 정보
    /// </summary>
    public static class User
    {
        /// <summary>
        /// 서버로부터 넘겨받은 기본 유저 정보
        /// </summary>
        public static UserBasicInfo Base;

        /// <summary>
        /// 소유중인 기물들
        /// </summary>
        public static UnitInfo[] Storages;

        /// <summary>
        /// 등록된 덱 리스트
        /// </summary>
        public static UnitInfo[][] Decks;

        /// <summary>
        /// 인게임에 들고 들어간 덱
        /// </summary>
        public static UnitInfo[] DeckLive;
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

        /// <summary>
        /// 이전 진행 기록 (라운드까지만 저장)
        /// </summary>
        public static DungeonInfo History;

        /// <summary>
        /// 기본 맵 정보
        /// </summary>
        public static DungeonInfo Info;

        /// <summary>
        /// 몬스터 정보
        /// </summary>
        public static UnitToken[][] MonsterInfo;

        /// <summary>
        /// 타일 정보
        /// </summary>
        public static HexCoordinate[] TilesInfo;

        /// <summary>
        /// 던전 덱
        /// </summary>
        public static UnitInfo[] Deck;
    }
}