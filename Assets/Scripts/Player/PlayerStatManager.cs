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
        if (statValues.TryGetValue(statType, out var value))
        {
            
            return value;
        }
        return 0f;
    }

    /// <summary>
    /// 특정 능력치의 현재 레벨(업그레이드 횟수) 반환
    /// </summary>
    public int GetStatLevel(PlayerStatType statType)
    {
        if (statLevels.TryGetValue(statType, out var level))
        {
            return level;
        }
        return 0;
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
        
        // 골드 체크 로직 제거 (이미 UI에서 체크했음)
        newGold = currentGold;
        
        // 능력치 증가
        statLevels[statType]++;
        statValues[statType] += table.upgradeValue;
        
        // GameManager.playerData 값도 직접 업데이트 (간결한 방식으로)
        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            switch (statType)
            {
                case PlayerStatType.AttackPower:
                    gameManager.playerData.Attack += Mathf.RoundToInt(table.upgradeValue);
                    break;
                case PlayerStatType.CriticalChance:
                    gameManager.playerData.Critical += table.upgradeValue;
                    break;
                case PlayerStatType.CriticalDamage:
                    gameManager.playerData.CriticalDmg += Mathf.RoundToInt(table.upgradeValue);
                    break;
                case PlayerStatType.GoldGainPercent:
                    gameManager.playerData.BonusGold += Mathf.RoundToInt(table.upgradeValue);
                    break;
                case PlayerStatType.AutoAttackCooldownReduce:
                    gameManager.playerData.AutoAttackCooldown += table.upgradeValue;
                    break;
            }
            
            // 값이 변경되었으므로 GameManager의 내부 값 갱신
            gameManager.UpdateData();
        }
        
        return true;
    }
} 