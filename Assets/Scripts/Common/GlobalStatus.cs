using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Unit;
using Assets.Scripts.Map;

/// <summary>
/// 현재 인게임에서 사용중인 데이터 관리 클래스
/// </summary>
public static class GlobalStatus
{

    private static MapInfo mapInfo;
    public static MapInfo MapInfo
    {
        get
        {
            return mapInfo;
        }
        set
        {
            mapInfo = value;
        }
    }
    public static int Radius
    {
        get
        {
            return mapInfo.radius;
        }
    }
    public static int CntTileBan
    {
        get
        {
            return mapInfo.cntTileBan;
        }
    }
    public static int CntUnitBan
    {
        get
        {
            return mapInfo.cntUnitBan;
        }
    }
    public static int CntUnit
    {
        get
        {
            return mapInfo.cntUnit;
        }
    }
    private static bool isSingle;
    public static bool IsSingle
    {
        get
        {
            return isSingle;
        }
        set
        {
            isSingle = value;
        }
    }

    /// <summary>
    /// 인게임에 들고 들어간 덱
    /// </summary>
    public static UnitInfo[] Deck;
    public static HexTileController[][] Map;
    public static UnitController[][] Units;
    /// <summary>
    /// 기물 오브젝트 풀
    /// </summary>
    public static Queue<UnitController> UnitPool;

    /// <summary>
    /// 기물이 풀에 존재할 경우 -> 꺼내서 줌
    /// </summary>
    /// <returns></returns>
    public static UnitController GetUnitController()
    {
        if (UnitPool.Count > 0)
            return UnitPool.Dequeue();
        else
            return null;
    }
}