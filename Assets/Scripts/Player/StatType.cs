using UnityEngine;

/// <summary>
/// 플레이어 능력치 유형을 정의하는 이넘
/// </summary>
public enum StatType
{
    AttackPower,       // 공격력
    CriticalChance,    // 치명타 확률
    CriticalDamage,    // 치명타 대미지
    GoldGain,          // 골드획득량 증가(%)
    AutoAttackCooldown // 자동공격 쿨타임 감소
} 