using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 행동 및 능력치 사용을 담당합니다.
/// </summary>
public class Player : MonoBehaviour
{
    private PlayerStatManager statManager;

    [Header("기본 능력치(초기값)")]
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float baseCriticalChance = 20f;      // %
    [SerializeField] private float baseCriticalDamage = 1.3f;    // 배율
    [SerializeField] private float baseGoldGainPercent = 0f;     // %
    [SerializeField] private float baseAutoAttackCooldownReduce = 0f; // 쿨타임 감소(초)

    private void Awake()
    {
        // StatManager 컴포넌트 캐싱
        statManager = GetComponent<PlayerStatManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    /// 능력치 업그레이드 시도 예시
    /// </summary>
    public void UpgradeStat(PlayerStatType statType, int currentGold)
    {
        if (statManager.TryUpgradeStat(statType, currentGold, out int newGold))
        {
            // 업그레이드 성공, newGold로 갱신
        }
        else
        {
            // 업그레이드 실패(골드 부족 등)
        }
    }

    /// <summary>
    /// 특정 능력치의 현재 값을 반환합니다. (기본값 + 업그레이드 누적값, 최대치 적용)
    /// </summary>
    public float GetStatValue(PlayerStatType statType)
    {
        float upgradeValue = statManager.GetStatValue(statType);
        switch (statType)
        {
            case PlayerStatType.AttackPower:
                return baseAttackPower + upgradeValue;
            case PlayerStatType.CriticalChance:
                // 최대 100%
                return Mathf.Min(baseCriticalChance + upgradeValue, 100f);
            case PlayerStatType.CriticalDamage:
                // 최대 250%
                return Mathf.Min(baseCriticalDamage + upgradeValue, 250f);
            case PlayerStatType.GoldGainPercent:
                // 최대 100%
                return Mathf.Min(baseGoldGainPercent + upgradeValue, 100f);
            case PlayerStatType.AutoAttackCooldownReduce:
                return baseAutoAttackCooldownReduce + upgradeValue;
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
