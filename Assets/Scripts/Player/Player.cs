using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 행동 및 능력치 사용을 담당합니다.
/// </summary>
public class Player : MonoBehaviour
{
    private PlayerStatManager statManager;
    private GameManager gameManager;

    private void Awake()
    {
        // StatManager 컴포넌트 캐싱
        statManager = GetComponent<PlayerStatManager>();
        Debug.Log(statManager == null ? "PlayerStatManager is NULL!" : "PlayerStatManager is OK!");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 공격 시 능력치(공격력, 치명타 등) 사용 예시
    /// </summary>
    public void Attack()
    {
        float attackPower = statManager.GetStatValue(PlayerStatType.AttackPower);
        float critChance = statManager.GetStatValue(PlayerStatType.CriticalChance);
        float critDamage = statManager.GetStatValue(PlayerStatType.CriticalDamage);
        // 실제 공격 로직 구현
    }

    /// <summary>
    /// 골드 획득 시 능력치(골드 획득량 %) 적용 예시
    /// </summary>
    public void GainGold(int baseGold)
    {
        float goldBonus = statManager.GetStatValue(PlayerStatType.GoldGainPercent);
        int totalGold = Mathf.RoundToInt(baseGold * (1f + goldBonus * 0.01f));
        // 골드 지급 로직
    }

    /// <summary>
    /// 능력치 업그레이드 (골드 체크는 이미 UI에서 수행됨)
    /// </summary>
    public bool UpgradeStat(PlayerStatType statType, int currentGold)
    {
        return statManager.TryUpgradeStat(statType, currentGold, out _);
    }

    /// <summary>
    /// 특정 능력치의 현재 값을 반환합니다. (PlayerData의 값 + 업그레이드 누적값, 최대치 적용)
    /// </summary>
    public float GetStatValue(PlayerStatType statType)
    {
        float upgradeValue = statManager.GetStatValue(statType);
        
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
        
        switch (statType)
        {
            case PlayerStatType.AttackPower:
                return gameManager.playerData.Attack + upgradeValue;
            case PlayerStatType.CriticalChance:
                // 최대 100%
                return Mathf.Min(gameManager.playerData.Critical + upgradeValue, 100f);
            case PlayerStatType.CriticalDamage:
                // 최대 250%
                
                float critDmg = Mathf.Min(gameManager.playerData.CriticalDmg + upgradeValue, 250f);
                
                return critDmg;
            case PlayerStatType.GoldGainPercent:
                // 최대 100%
                return Mathf.Min(gameManager.playerData.BonusGold + upgradeValue, 100f);
            case PlayerStatType.AutoAttackCooldownReduce:
                return gameManager.playerData.AutoAttackCooldown + upgradeValue;
            default:
                return upgradeValue;
        }
    }

    /// <summary>
    /// 특정 능력치의 현재 업그레이드 비용을 반환합니다.
    /// </summary>
    public int GetUpgradeCost(PlayerStatType statType)
    {
        return statManager.GetUpgradeCost(statType);
    }
}
