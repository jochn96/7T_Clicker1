using System;
using UnityEngine;

/// <summary>
/// 플레이어 능력치의 상태(레벨, 값, 업그레이드 비용 등)를 저장합니다.
/// </summary>
[Serializable]
public class PlayerStatData
{
    /// <summary>
    /// 능력치 종류
    /// </summary>
    public PlayerStatType statType;

    /// <summary>
    /// 현재 레벨
    /// </summary>
    public int level;

    /// <summary>
    /// 현재 값
    /// </summary>
    public float value;

    /// <summary>
    /// 다음 업그레이드 비용
    /// </summary>
    public int upgradeCost;
} 