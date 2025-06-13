using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 능력치와 업그레이드 기능을 관리합니다.
/// </summary>
public class PlayerStat : MonoBehaviour
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
    private Player player;

    [Header("기본 능력치(초기값)")]
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float baseCriticalChance = 20f;      // %
    [SerializeField] private float baseCriticalDamage = 1.3f;    // 배율
    [SerializeField] private float baseGoldGainPercent = 0f;     // %
    [SerializeField] private float baseAutoAttackCooldownReduce = 0f; // 쿨타임 감소(초)


    private void Awake()
    {
        player = GetComponent<Player>();
        
        // 초기화: Dictionary 준비
        foreach (var table in statUpgradeTables)
        {
            statLevels[table.statType] = 0;
            statValues[table.statType] = 0f;
        }
    }

    /// <summary>
    /// PlayerData를 이용해 능력치를 초기화합니다.
    /// </summary>
    public void InitWithPlayerData(PlayerData data)
    {
        if (data == null) return;
        
        playerData = data;
        
        // 기본 능력치 값 할당 (신규 플레이어인 경우)
        if (playerData.Attack <= 0)
            playerData.Attack = (int)baseAttackPower;
        if (playerData.Critical <= 0)
            playerData.Critical = baseCriticalChance;
        if (playerData.CriticalDmg <= 0)
            playerData.CriticalDmg = (int)(baseCriticalDamage * 100); // 배율을 퍼센트로 변환
        if (playerData.BonusGold <= 0)
            playerData.BonusGold = (int)baseGoldGainPercent;
        
        // 각 능력치의 레벨과 값 초기화
        foreach (var table in statUpgradeTables)
        {
            statLevels[table.statType] = GetUpgradeLevelFromPlayerData(table.statType);
            statValues[table.statType] = GetUpgradeValueFromPlayerData(table.statType, table.upgradeValue);
        }
        
        // 값 저장
        SaveDataToJSON.SaveUsers(playerData);
    }

    /// <summary>
    /// PlayerData에서 해당 능력치의 레벨을 가져옵니다.
    /// </summary>
    private int GetUpgradeLevelFromPlayerData(PlayerStatType statType)
    {
        if (playerData == null) return 0;
        return statType switch
        {
            PlayerStatType.AttackPower => playerData.Attack,
            PlayerStatType.CriticalChance => (int)playerData.Critical,
            PlayerStatType.CriticalDamage => playerData.CriticalDmg,
            PlayerStatType.GoldGainPercent => playerData.BonusGold,
            PlayerStatType.AutoAttackCooldownReduce => (int)playerData.AutoAttackCooldown,
            _ => 0
        };
    }

    /// <summary>
    /// 능력치 타입과 기본 증가값을 기반으로 총 증가값을 계산합니다.
    /// </summary>
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
        
        int level = statLevels.TryGetValue(statType, out var value) ? value : 0;
        return Mathf.RoundToInt(table.baseCost * Mathf.Pow(table.costMultiplier, level));
    }

    /// <summary>
    /// 능력치 업그레이드 시도 (성공 시 true 반환)
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
        
        // 골드 차감
        newGold = currentGold - cost;
        
        // 레벨 및 값 증가
        if (!statLevels.ContainsKey(statType))
            statLevels[statType] = 0;
            
        if (!statValues.ContainsKey(statType))
            statValues[statType] = 0;
            
        statLevels[statType]++;
        statValues[statType] += table.upgradeValue;
        
        // PlayerData에 업그레이드 값 반영
        if (playerData != null)
        {
            switch (statType)
            {
                case PlayerStatType.AttackPower:
                    playerData.Attack += (int)table.upgradeValue;
                    break;
                case PlayerStatType.CriticalChance:
                    playerData.Critical += table.upgradeValue;
                    break;
                case PlayerStatType.CriticalDamage:
                    playerData.CriticalDmg += (int)table.upgradeValue;
                    break;
                case PlayerStatType.GoldGainPercent:
                    playerData.BonusGold += (int)table.upgradeValue;
                    break;
                case PlayerStatType.AutoAttackCooldownReduce:
                    playerData.AutoAttackCooldown += table.upgradeValue;
                    break;
            }
            
            // 변경된 데이터 저장
            SaveDataToJSON.SaveUsers(playerData);
            
            // GameManager에 데이터 반영
            GameManager.Instance.updateData();
        }
        
        return true;
    }
    
    /// <summary>
    /// 공격력 능력치 업그레이드
    /// </summary>
    public void UpgradeAttack()
    {
        if (GameManager.Instance == null) return;
        
        if (TryUpgradeStat(PlayerStatType.AttackPower, GameManager.Instance.gold, out int newGold))
        {
            GameManager.Instance.gold = newGold;
            GameManager.Instance.updateData();
        }
    }
    
    /// <summary>
    /// 치명타 확률 능력치 업그레이드
    /// </summary>
    public void UpgradeCriticalChance()
    {
        if (GameManager.Instance == null) return;
        
        if (TryUpgradeStat(PlayerStatType.CriticalChance, GameManager.Instance.gold, out int newGold))
        {
            GameManager.Instance.gold = newGold;
            GameManager.Instance.updateData();
        }
    }
    
    /// <summary>
    /// 치명타 데미지 능력치 업그레이드
    /// </summary>
    public void UpgradeCriticalDamage()
    {
        if (GameManager.Instance == null) return;
        
        if (TryUpgradeStat(PlayerStatType.CriticalDamage, GameManager.Instance.gold, out int newGold))
        {
            GameManager.Instance.gold = newGold;
            GameManager.Instance.updateData();
        }
    }
    
    /// <summary>
    /// 골드 획득량 능력치 업그레이드
    /// </summary>
    public void UpgradeGoldGain()
    {
        if (GameManager.Instance == null) return;
        
        if (TryUpgradeStat(PlayerStatType.GoldGainPercent, GameManager.Instance.gold, out int newGold))
        {
            GameManager.Instance.gold = newGold;
            GameManager.Instance.updateData();
        }
    }
    
    /// <summary>
    /// 자동 공격 쿨타임 능력치 업그레이드
    /// </summary>
    public void UpgradeAutoAttackCooldown()
    {
        if (GameManager.Instance == null) return;
        
        if (TryUpgradeStat(PlayerStatType.AutoAttackCooldownReduce, GameManager.Instance.gold, out int newGold))
        {
            GameManager.Instance.gold = newGold;
            GameManager.Instance.updateData();
        }
    }
} 