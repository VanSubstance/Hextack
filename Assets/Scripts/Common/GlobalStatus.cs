using Assets.Scripts.Battle;
using Assets.Scripts.UI;
using System.Collections.Generic;

/// <summary>
/// 현재 인게임에서 사용중인 데이터 관리 클래스
/// </summary>
public static class GlobalStatus
{
    /// <summary>
    /// 인게임 상수들
    /// </summary>
    public static class InGame
    {
        /// <summary>
        /// 기본 크리티컬 확률
        /// </summary>
        public static float RateCritical = .1f;
    }

    /// <summary>
    /// 현재 씬 이름, 다음 씬 이름
    /// </summary>
    public static string CurScene, NextScene;

    /// <summary>
    /// 체력게이지 풀
    /// </summary>
    public static Queue<GageController> HpGagePool = new Queue<GageController>();

    /// <summary>
    /// 이펙트 풀
    /// </summary>
    public static Dictionary<string, Queue<EffectController>> effectPool = new Dictionary<string, Queue<EffectController>>();

    /// <summary>
    /// 체력 게이지가 풀에 존재 -> 꺼내줌
    /// </summary>
    /// <returns></returns>
    public static GageController GetHpGageController()
    {
        if (HpGagePool.TryDequeue(out GageController res))
            return res;
        else
            return null;
    }
}