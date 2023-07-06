

/// <summary>
/// 초기화 인터페이스
/// </summary>
public interface IInitiable
{
    /// <summary>
    /// 초기화
    /// </summary>
    public void Init();
}

/// <summary>
/// 초기화 인터페이스: Generic
/// </summary>
public interface IInitiable<T>
{
    /// <summary>
    /// 초기화
    /// </summary>
    public void Init(T param);
}

/// <summary>
/// 풀링 오브젝트
/// </summary>
public interface IPoolObject<TObject, TParameter>
{
    /// <summary>
    /// 초기화 후 해당 객체 반환
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public TObject Init(TParameter param, System.Action<TObject> actionReturnToPool);

    /// <summary>
    /// 풀링에 반납
    /// </summary>
    public void ReturnToPool();
}