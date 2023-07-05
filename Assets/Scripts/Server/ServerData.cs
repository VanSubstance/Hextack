using Assets.Scripts.Unit;
using Assets.Scripts.Map;
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

    /// <summary>
    /// 유저 정보
    /// </summary>
    public static class User
    {
        /// <summary>
        /// 닉네임
        /// </summary>
        public static string nickName = "서버수신닉네임";

        /// <summary>
        /// 인게임에 들고 들어간 덱
        /// </summary>
        public static UnitInfo[] Deck;

        /// <summary>
        /// 소유중인 기물들
        /// </summary>
        public static UnitInfo[] Storages;

        /// <summary>
        /// 보유 재화량
        /// </summary>
        public static int AmountGold, AmountArtifact;
    }

    /// <summary>
    /// 던전 정보
    /// </summary>
    public static class Dungeon
    {
        /// <summary>
        /// 기본 맵 정보
        /// </summary>
        public static MapInfo Info;
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