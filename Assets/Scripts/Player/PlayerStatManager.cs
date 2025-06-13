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

    private PlayerData playerData;

    public void InitWithPlayerData(PlayerData data)
    {
        playerData = data;
        foreach (var table in statUpgradeTables)
        {
            statLevels[table.statType] = GetUpgradeLevelFromPlayerData(table.statType);
            statValues[table.statType] = GetUpgradeValueFromPlayerData(table.statType, table.upgradeValue);
        }
    }

    private int GetUpgradeLevelFromPlayerData(PlayerStatType statType)
    {
        if (playerData == null) return 0;
        return statType switch
        {
            PlayerStatType.AttackPower => playerData.Attack,
            PlayerStatType.CriticalChance => playerData.Critical,
            PlayerStatType.CriticalDamage => playerData.CriticalDmg,
            PlayerStatType.GoldGainPercent => playerData.BonusGold,
            // AutoAttack 등 추가 시 여기에
            _ => 0
        };
    }

    private float GetUpgradeValueFromPlayerData(PlayerStatType statType, float upgradeValue)
    {
        int level = GetUpgradeLevelFromPlayerData(statType);
        return level * upgradeValue;
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
        newGold = currentGold - cost;
        statLevels[statType]++;
        statValues[statType] += table.upgradeValue;
        // PlayerData에 업그레이드 값 반영
        if (playerData != null)
        {
            switch (statType)
            {
                case PlayerStatType.AttackPower:
                    playerData.Attack++;
                    break;
                case PlayerStatType.CriticalChance:
                    playerData.Critical++;
                    break;
                case PlayerStatType.CriticalDamage:
                    playerData.CriticalDmg++;
                    break;
                case PlayerStatType.GoldGainPercent:
                    playerData.BonusGold++;
                    break;
                // AutoAttack 등 추가 시 여기에
            }
            SaveDataToJSON.SaveUsers(playerData);
        }
        return true;
    }
} 