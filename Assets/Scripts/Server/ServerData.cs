using Assets.Scripts.Dungeon;
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
        public static string rootPath = $"{ServerData.rootPath}/Dungeon";

        /// <summary>
        /// 접근 가능한 던전 리스트
        /// </summary>
        public static Dictionary<string, DungeonInfo> data = new Dictionary<string, DungeonInfo>();
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
        public static MonsterToken[] MonsterInfo
        {
            get
            {
                return DungeonInfo.MonsterCodeList;
            }
        }

        /// <summary>
        /// 현재 라운드
        /// </summary>
        public static int CurrentRound;

        /// <summary>
        /// 던전 총 라운드 수
        /// </summary>
        public static int MaxRound
        {
            get
            {
                return MonsterInfo.Length;
            }
        }
        
        /// <summary>
        /// 던전 라운드당 시간 (초0
        /// </summary>
        public static int TimeRound
        {
            get
            {
                return DungeonInfo.TimeRound;
            }
        }

        /// <summary>
        /// 현재 채굴 레벨
        /// </summary>
        public static int MiningLevel;

        /// <summary>
        /// 채굴 레벨업 비용
        /// </summary>
        public static int PriceMiningLvUp = 10;

        /// <summary>
        /// 현재 던전 누적 보상량 (끝날 때 획득)
        /// </summary>
        public static int AccuGold, AccuArtifact;

        /// <summary>
        /// 마지막으로 선택한 타워
        /// </summary>
        public static TowerController LastTowerClicked = null;
    }
}