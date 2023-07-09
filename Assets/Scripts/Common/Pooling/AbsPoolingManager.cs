using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링 매니저 기본 컨트롤러
/// </summary>
public abstract class AbsPoolingManager<T, TContentInfo> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    [SerializeField]
    private AbsPoolingContent<TContentInfo> componentPrefab;
    private Queue<AbsPoolingContent<TContentInfo>> q;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        q = new Queue<AbsPoolingContent<TContentInfo>>();
        // 풀링: 10개만
        int ii = 0;
        AbsPoolingContent<TContentInfo> temp;
        while (ii++ < 10)
        {
            temp = Instantiate(componentPrefab, GetParent());
            temp.ConnectWithParent((content) =>
            {
                q.Enqueue(content);
            });
            temp.gameObject.SetActive(false);
            q.Enqueue(temp);
        }
    }

    /// <summary>
    /// 신규 컨텐츠 컴포넌트 반환
    /// </summary>
    /// <returns></returns>
    public AbsPoolingContent<TContentInfo> GetNewContent(TContentInfo _info)
    {
        if (!q.TryDequeue(out AbsPoolingContent<TContentInfo> res))
        {
            res = Instantiate(componentPrefab, GetParent());
        }
        res.Init(_info);
        res.ConnectWithParent((content) =>
        {
            q.Enqueue(content);
        });
        return res;
    }

    /// <summary>
    /// 오브젝트 풀링 부모 반환
    /// </summary>
    /// <returns></returns>
    public abstract Transform GetParent();
}