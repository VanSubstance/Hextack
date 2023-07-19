using Assets.Scripts.Tower;

public static class CommonFunction
{
    /// <summary>
    /// 타워 타임 -> 한글로 젼환 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string TranslateTowerType(TowerType type)
    {
        switch (type)
        {
            case TowerType.Machine:
                return "기계";
            case TowerType.Bio:
                return "생체";
            case TowerType.Magic:
                return "마법";
        }
        return string.Empty;
    }
}