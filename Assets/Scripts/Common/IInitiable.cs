

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