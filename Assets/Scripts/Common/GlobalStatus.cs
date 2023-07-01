using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Unit;

/// <summary>
/// 현재 인게임에서 사용중인 데이터 관리 클래스
/// </summary>
public static class GlobalStatus
{
    /// <summary>
    /// 인게임에 들고 들어간 덱
    /// </summary>
    public static UnitInfo[] deck;
}