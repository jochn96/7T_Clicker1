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
    
    [Header("자동 공격 설정")]
    public bool isAutoAttackUnlocked = false; // 자동 공격 기능 잠금 해제 여부
    private bool isAutoAttackActive = false; // 자동 공격 활성화 여부
    private float autoAttackTimer = 0f; // 자동 공격 타이머
    public int autoAttackUnlockCost = 5000; // 자동 공격 해금 비용

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
        
        // 저장된 데이터에서 자동 공격 잠금 해제 상태 불러오기
        if (gameManager != null && gameManager.playerData != null)
        {
            isAutoAttackUnlocked = gameManager.playerData.IsAutoAttackUnlocked;
            
            // 자동 공격이 해금되어 있다면 활성화
            if (isAutoAttackUnlocked)
            {
                ActivateAutoAttack();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 자동 공격 기능이 활성화되어 있다면 타이머 업데이트
        if (isAutoAttackActive)
        {
            autoAttackTimer += Time.deltaTime;
            
            // 자동 공격 쿨다운 시간이 지났다면 공격 실행
            float autoAttackCooldown = GetAutoAttackCooldown();
            if (autoAttackTimer >= autoAttackCooldown)
            {
                autoAttackTimer = 0f;
                AutoAttack();
            }
        }
    }

    /// <summary>
    /// 자동 공격 기능 활성화
    /// </summary>
    public void ActivateAutoAttack()
    {
        if (!isAutoAttackUnlocked) return;
        
        isAutoAttackActive = true;
        autoAttackTimer = 0f;
        Debug.Log("자동 공격 기능이 활성화되었습니다.");
    }
    
    /// <summary>
    /// 자동 공격 기능 비활성화
    /// </summary>
    public void DeactivateAutoAttack()
    {
        isAutoAttackActive = false;
        Debug.Log("자동 공격 기능이 비활성화되었습니다.");
    }
    
    /// <summary>
    /// 자동 공격 기능 잠금 해제 시도
    /// </summary>
    public bool TryUnlockAutoAttack()
    {
        if (isAutoAttackUnlocked) return true; // 이미 해금된 경우
        
        Debug.Log($"자동 공격 구매 시도: 비용={autoAttackUnlockCost}G, 현재 골드={gameManager.playerData.Gold}G");
        
        if (gameManager != null && gameManager.UseGold(autoAttackUnlockCost))
        {
            Debug.Log($"자동 공격 구매 성공: 남은 골드={gameManager.playerData.Gold}G");
            isAutoAttackUnlocked = true;
            gameManager.playerData.IsAutoAttackUnlocked = true;
            gameManager.updateData(); // 데이터 저장
            ActivateAutoAttack(); // 즉시 활성화
            return true;
        }
        else
        {
            Debug.Log("자동 공격 구매 실패: 골드 부족");
            return false;
        }
    }
    
    /// <summary>
    /// 자동 공격 실행
    /// </summary>
    private void AutoAttack()
    {
        // 실제 공격 로직 실행
        Attack();
        Debug.Log("자동 공격 실행!");
    }
    
    /// <summary>
    /// 현재 자동 공격 쿨다운 시간 반환
    /// </summary>
    private float GetAutoAttackCooldown()
    {
        // 기본 쿨다운 - 업그레이드로 감소된 시간
        float baseCooldown = gameManager.playerData.AutoAttackCooldown;
        float cooldownReduction = statManager.GetStatValue(PlayerStatType.AutoAttackCooldownReduce);
        
        // 최소 0.5초, 최대 기본값
        return Mathf.Max(0.5f, baseCooldown - cooldownReduction);
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
