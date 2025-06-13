/// <summary>
/// 플레이어 능력치 종류를 정의합니다.
/// </summary>
public enum PlayerStatType
{
    /// <summary>
    /// 공격력
    /// </summary>
    AttackPower,
    /// <summary>
    /// 치명타 확률(%)
    /// </summary>
    CriticalChance,
    /// <summary>
    /// 치명타 대미지(배율)
    /// </summary>
    CriticalDamage,
    /// <summary>
    /// 골드 획득량 증가(%)
    /// </summary>
    GoldGainPercent,
    /// <summary>
    /// 자동공격 쿨타임 감소(초)
    /// </summary>
    AutoAttackCooldownReduce
} 