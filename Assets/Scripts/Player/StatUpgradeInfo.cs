using System;
using UnityEngine;

/// <summary>
/// 각 능력치의 업그레이드 정보를 담는 클래스
/// </summary>
[Serializable]
public class StatUpgradeInfo
{
    public StatType statType;            // 능력치 유형
    public float baseValue;              // 기본 값
    public float incrementPerUpgrade;    // 업그레이드당 증가량
    public int maxUpgradeLevel;          // 최대 업그레이드 단계
    public int baseCost;                 // 기본 비용
    public float costIncreaseRate;       // 비용 증가율
    
    /// <summary>
    /// 특정 업그레이드 단계에서의 능력치 값을 계산합니다.
    /// </summary>
    /// <param name="upgradeLevel">현재 업그레이드 단계</param>
    /// <returns>계산된 능력치 값</returns>
    public float GetValueAtUpgradeLevel(int upgradeLevel)
    {
        return baseValue + (incrementPerUpgrade * upgradeLevel);
    }
    
    /// <summary>
    /// 특정 업그레이드 단계에서 다음 업그레이드 비용을 계산합니다.
    /// </summary>
    /// <param name="upgradeLevel">현재 업그레이드 단계</param>
    /// <returns>다음 단계로 업그레이드하는 비용</returns>
    public int GetUpgradeCost(int upgradeLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costIncreaseRate, upgradeLevel));
    }
} 