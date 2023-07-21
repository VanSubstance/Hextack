using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 광역으로 존재;
/// 대상을 받아 물리효과를 부여한다
/// ServerManager의 코루틴 함수를 활용
/// </summary>
[RequireComponent(typeof(ServerManager))]
public class ServerAddOnPhysics : SingletonObject<ServerAddOnPhysics>
{
    private Dictionary<int, Coroutine> CrTracker;
    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform);
        CrTracker = new Dictionary<int, Coroutine>();
    }

    /// <summary>
    /// 대상 트랜스폽을 진동시킨다
    /// </summary>
    /// <param name="target"></param>
    /// <param name="weight">1 : 약함, 2: 강함</param>
    public void Vibrate(Transform target, int weight, bool isTargetFixed)
    {
        if (CrTracker.ContainsKey(target.GetInstanceID()) && CrTracker[target.GetInstanceID()] != null)
        {
            // 지금 작동중인 코루틴 존재
            return;
        }
        Vector3 vibVec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * (weight == 1 ? .2f : 1f), originPos = target.position;
        CrTracker[target.GetInstanceID()] = ServerManager.Instance.ExecuteCrInRepeat(() =>
       {
           target.position += vibVec;
           vibVec *= -.5f;
       }, () => vibVec.magnitude < .005f, () =>
       {
           if (isTargetFixed)
           {
               target.position = originPos;
           }
           CrTracker[target.GetInstanceID()] = null;
       }, Time.deltaTime);
    }
}