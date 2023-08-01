using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 풀링 컨텐츠 기본형
/// </summary>
public abstract class AbsPoolingContent<TInfo> : MonoBehaviour
{
    protected System.Action<AbsPoolingContent<TInfo>> ActionReturnToPool;
    private bool isConnected = false;
    public void ConnectWithParent(System.Action<AbsPoolingContent<TInfo>> _ActionReturnToPool)
    {
        ActionReturnToPool = _ActionReturnToPool;
        isConnected = true;
    }

    /// <summary>
    /// 사용 시작 함수
    /// </summary>
    public AbsPoolingContent<TInfo> Init(TInfo info)
    {
        if (!isConnected || !InitExtra(info))
        {
            ReturnToPool();
            return null;
        }
        gameObject.SetActive(true);
        return this;
    }


    /// <summary>
    /// 사용 시작 추가 실행
    /// </summary>
    protected abstract bool InitExtra(TInfo _info);

    /// <summary>
    /// 풀에 반납
    /// </summary>
    public void ReturnToPool()
    {
        try
        {
            Clear();
            ActionReturnToPool?.Invoke(this);
            gameObject.SetActive(false);
        } catch (MissingReferenceException)
        {

        }
    }

    /// <summary>
    /// 풀에 반납할 때 추가 실행
    /// </summary>
    public abstract void Clear();
}