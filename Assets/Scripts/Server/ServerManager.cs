using Assets.Scripts.Common;
using Assets.Scripts.Monster;
using Assets.Scripts.Unit;
using Assets.Scripts.Server;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 서버 연결 관리용 매니저
/// </summary>
public class ServerManager : SingletonObject<ServerManager>
{
    [SerializeField]
    private bool isSingle;

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform);
        Application.targetFrameRate = 1000;
        GlobalStatus.IsSingle = isSingle;
        DataManager.Instance.LoadLocalDatas();
    }

    /// <summary>
    /// 던전 정보 받아오기 함수
    /// </summary>
    /// <param name="dungeonName"></param>
    public void LoadDungeonInfo()
    {
        GlobalStatus.MapInfo = ServerData.InGame.DungeonInfo;
        string basePath = $"Datas/Maps/{ServerData.InGame.DungeonInfo.radius}/{ServerData.InGame.DungeonInfo.Code}";
        for (int i = 0; i < ServerData.InGame.DungeonInfo.rounds; i++)
        {
            ServerData.InGame.MonsterInfo = Resources.LoadAll<MonsterToken>($"{basePath}/single/rounds/{i + 1}");
        }
    }

    /// <summary>
    /// 광고 보고 두배 수령하고 메인메뉴로 나가기
    /// </summary>
    public void ExitDouble()
    {
        ServerData.User.AmountArtifact += GlobalStatus.InGame.AccuArtifact * 2;
        ServerData.User.AmountGold += GlobalStatus.InGame.AccuGold * 2;
        GlobalStatus.NextScene = "Main";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 그냥 수령하고 메인메뉴로 나가기
    /// </summary>
    public void ExitNormal()
    {
        ServerData.User.AmountArtifact += GlobalStatus.InGame.AccuArtifact;
        ServerData.User.AmountGold += GlobalStatus.InGame.AccuGold;
        GlobalStatus.NextScene = "Main";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 신규 던전 입장
    /// </summary>
    /// <param name="dungeonCode"></param>
    public void EnterDungeon(string dungeonCode)
    {
        ServerData.InGame.DungeonInfo = ServerData.Dungeon.DungeonList[dungeonCode];
        GlobalStatus.NextScene = "InGame";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 기존 던전 이어서 진행
    /// </summary>
    public void ContinueDungeon()
    {
        Debug.Log($"이어서 드가자 ㅡ ! ");
    }

    /// <summary>
    /// 반복 실행 코루틴
    /// </summary>
    /// <param name="actionRepeat">반복할 람다</param>
    /// <param name="actionCondition">탈출 체크 람다 = true 반환 시 코루틴 강제 종료</param>
    /// <param name="actionEscape">탈출 시 실행되는 람다</param>
    /// <param name="time">반복 텀</param>
    public void ExecuteCrInRepeat(System.Action actionRepeat, System.Func<bool> actionCondition, System.Action actionEscape, float time)
    {
        StartCoroutine(CoroutineExecuteActionInRepeat(actionRepeat, actionCondition, actionEscape, time));
    }

    /// <summary>
    /// 람다 순차 실행 코루틴
    /// </summary>
    /// <param name="actionQueue"></param>
    /// <param name="time"></param>
    public void ExecuteCrInSequnce(Queue<System.Action> actionQueue, float time)
    {
        StartCoroutine(CoroutineExecuteActionsByTerm(actionQueue, time));
    }

    /// <summary>
    /// 반복 실행 코루틴
    /// </summary>
    /// <param name="actionRepeat"></param>
    /// <param name="actionCondition">true 반환 시 코루틴 강제 종료</param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator CoroutineExecuteActionInRepeat(System.Action actionRepeat, System.Func<bool> actionCondition, System.Action actionEscape, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (actionCondition?.Invoke() == true)
            {
                actionEscape?.Invoke();
                yield break;
            }
            actionRepeat?.Invoke();
        }
    }

    /// <summary>
    /// 전달된 람다식들을 time 텀을 두고 순차적으로 실행
    /// </summary>
    /// <param name="actionsToExecute"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator CoroutineExecuteActionsByTerm(Queue<System.Action> actionsToExecute, float time)
    {
        while (actionsToExecute.TryDequeue(out System.Action now))
        {
            yield return new WaitForSeconds(time);
            now?.Invoke();
        }
    }
}
