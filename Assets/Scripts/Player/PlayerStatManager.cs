using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 능력치와 업그레이드 기능을 관리합니다.
/// </summary>
public class PlayerStatManager : MonoBehaviour
{
    /// <summary>
    /// 능력치별 업그레이드 테이블 리스트 (Inspector에서 할당)
    /// </summary>
    [SerializeField]
    private List<PlayerStatUpgradeTable> statUpgradeTables;

    /// <summary>
    /// 각 능력치의 현재 레벨(업그레이드 횟수)
    /// </summary>
    private Dictionary<PlayerStatType, int> statLevels = new();

    /// <summary>
    /// 각 능력치의 현재 값
    /// </summary>
    private Dictionary<PlayerStatType, float> statValues = new();

    private void Awake()
    {
        // 능력치별 초기화
        foreach (var table in statUpgradeTables)
        {
            statLevels[table.statType] = 0;
            statValues[table.statType] = 0f;
        }
    }

    /// <summary>
    /// 특정 능력치의 현재 값 반환
    /// </summary>
    public float GetStatValue(PlayerStatType statType)
    {
        return statValues.TryGetValue(statType, out var value) ? value : 0f;
    }

    /// <summary>
    /// 특정 능력치의 현재 업그레이드 비용 반환
    /// </summary>
    public int GetUpgradeCost(PlayerStatType statType)
    {
        var table = statUpgradeTables.Find(t => t.statType == statType);
        if (table == null) return 0;
        int level = statLevels[statType];
        return Mathf.RoundToInt(table.baseCost * Mathf.Pow(table.costMultiplier, level));
    }

    /// <summary>
    /// 능력치 업그레이드 시도(성공 시 true 반환)
    /// </summary>
    public bool TryUpgradeStat(PlayerStatType statType, int currentGold, out int newGold)
    {
        var table = statUpgradeTables.Find(t => t.statType == statType);
        if (table == null)
        {
            newGold = currentGold;
            return false;
        }
        int cost = GetUpgradeCost(statType);
        if (currentGold < cost)
        {
            newGold = currentGold;
            return false;
        }
        // 비용 차감
        newGold = currentGold - cost;
        // 능력치 증가
        statLevels[statType]++;
        statValues[statType] += table.upgradeValue;
        return true;
    }
} 