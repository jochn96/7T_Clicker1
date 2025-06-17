using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 플레이어 능력치 UI를 관리합니다.
/// </summary>
public class PlayerStatUI : MonoBehaviour
{
    [Header("골드 표시 UI")]
    public TextMeshProUGUI currentGoldText;

    [Header("공격력 UI")]
    public TextMeshProUGUI attackValueText;
    public TextMeshProUGUI attackCostText;
    public Button attackUpgradeButton;

    [Header("치명타 확률 UI")]
    public TextMeshProUGUI critChanceValueText;
    public TextMeshProUGUI critChanceCostText;
    public Button critChanceUpgradeButton;

    [Header("치명타 대미지 UI")]
    public TextMeshProUGUI critDamageValueText;
    public TextMeshProUGUI critDamageCostText;
    public Button critDamageUpgradeButton;

    [Header("골드 획득량 UI")]
    public TextMeshProUGUI goldGainValueText;
    public TextMeshProUGUI goldGainCostText;
    public Button goldGainUpgradeButton;

    [Header("자동공격 쿨타임 UI")]
    public TextMeshProUGUI autoAttackCooldownValueText;
    public TextMeshProUGUI autoAttackCooldownCostText;
    public Button autoAttackCooldownUpgradeButton;
    public GameObject autoAttackCooldownUpgradePanel; // 자동공격 쿨타임 업그레이드 패널
    
    [Header("자동공격 구매 UI")]
    public GameObject autoAttackPurchasePanel;
    public TextMeshProUGUI autoAttackPurchaseCostText;
    public Button autoAttackPurchaseButton;

    private Player player;
    private GameManager gameManager;
    private int currentGold; // 실제로는 GameManager 등에서 받아와야 함

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        // 시작 시 UI 갱신
        RefreshUI();
        
        // 약간의 지연 후 버튼 리스너 등록 (모든 매니저가 초기화된 후)
        Invoke("SetupButtonListeners", 0.1f);
    }

    /// <summary>
    /// 버튼 리스너를 설정합니다.
    /// </summary>
    private void SetupButtonListeners()
    {
        // 버튼에 리스너 등록
        attackUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AttackPower));
        critChanceUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalChance));
        critDamageUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalDamage));
        goldGainUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.GoldGainPercent));
        autoAttackCooldownUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AutoAttackCooldownReduce));
        
        // 자동 공격 구매 버튼 리스너 등록
        if (autoAttackPurchaseButton != null)
        {
            autoAttackPurchaseButton.onClick.AddListener(OnAutoAttackPurchase);
        }
    }

    private void Update()
    {
        // 골드 UI 업데이트 (매 프레임마다 갱신)
        UpdateGoldUI();
    }

    /// <summary>
    /// 현재 골드 UI를 갱신합니다.
    /// </summary>
    private void UpdateGoldUI()
    {
        if (gameManager != null && currentGoldText != null)
        {
            // GameManager에서 현재 골드 정보 가져오기
            currentGold = gameManager.playerData.Gold;
            
            // 골드 표시 (천 단위 콤마 포함)
            currentGoldText.text = string.Format("{0:#,0} G", currentGold);
        }
    }

    /// <summary>
    /// UI를 갱신합니다.
    /// </summary>
    public void RefreshUI()
    {
        // 골드 UI 갱신
        UpdateGoldUI();

        attackValueText.text = player.GetStatValue(PlayerStatType.AttackPower).ToString();
        attackCostText.text = player.GetUpgradeCost(PlayerStatType.AttackPower) + "G";

        critChanceValueText.text = player.GetStatValue(PlayerStatType.CriticalChance) + "%";
        critChanceCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalChance) + "G";

        // 치명타 데미지 %로 표시 - PlayerData의 값을 그대로 표시
        critDamageValueText.text = player.GetStatValue(PlayerStatType.CriticalDamage) + "%";
        critDamageCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalDamage) + "G";

        goldGainValueText.text = player.GetStatValue(PlayerStatType.GoldGainPercent) + "%";
        goldGainCostText.text = player.GetUpgradeCost(PlayerStatType.GoldGainPercent) + "G";

        // 자동공격 쿨타임 감소 값을 소수점 첫째자리까지만 표시
        autoAttackCooldownValueText.text = player.GetStatValue(PlayerStatType.AutoAttackCooldownReduce).ToString("F1");
        autoAttackCooldownCostText.text = player.GetUpgradeCost(PlayerStatType.AutoAttackCooldownReduce) + "G";
        
        // 자동 공격 UI 갱신
        UpdateAutoAttackUI();
    }
    
    /// <summary>
    /// 자동 공격 관련 UI를 갱신합니다.
    /// </summary>
    private void UpdateAutoAttackUI()
    {
        if (player == null) return;
        
        // 자동 공격 구매 비용 표시
        if (autoAttackPurchaseCostText != null)
        {
            autoAttackPurchaseCostText.text = player.autoAttackUnlockCost + "G";
        }
        
        // 자동 공격 잠금 해제 상태에 따라 UI 패널 표시/숨김
        if (autoAttackPurchasePanel != null && autoAttackCooldownUpgradePanel != null)
        {
            bool isUnlocked = player.isAutoAttackUnlocked;
            autoAttackPurchasePanel.SetActive(!isUnlocked);
            
            // 자동공격 기능이 잠금 해제되었을 때만 쿨타임 업그레이드 패널 표시
            if (autoAttackCooldownUpgradePanel != null)
            {
                autoAttackCooldownUpgradePanel.SetActive(isUnlocked);
            }
        }
    }

    /// <summary>
    /// +버튼 클릭 시 업그레이드 시도 및 UI 갱신
    /// </summary>
    private void OnUpgrade(PlayerStatType statType)
    {
        // null 체크
        if (gameManager == null) gameManager = GameManager.Instance;
        if (player == null) player = FindObjectOfType<Player>();
        
        if (gameManager == null || player == null)
        {
            Debug.LogWarning("GameManager 또는 Player가 null입니다. 약간 후에 다시 시도해주세요.");
            return;
        }
        
        // 업그레이드 비용 계산
        int cost = player.GetUpgradeCost(statType);
        
        // 골드 차감 및 스탯 업그레이드
        if (gameManager.UseGold(cost))
        {
            player.UpgradeStat(statType, gameManager.playerData.Gold);
            RefreshUI();
            Debug.Log($"{statType} 업그레이드 성공");
        }
        else
        {
            Debug.Log($"{statType} 업그레이드 실패: 골드 부족");
        }
    }
    
    /// <summary>
    /// 자동 공격 구매 버튼 클릭 핸들러
    /// </summary>
    private void OnAutoAttackPurchase()
    {
        if (player == null) return;
        
        // 자동 공격 기능 구매 시도
        if (player.TryUnlockAutoAttack())
        {
            // 구매 성공 시 UI 갱신
            RefreshUI();
            Debug.Log("자동 공격 기능 구매 성공!");
        }
        else
        {
            Debug.Log("자동 공격 기능 구매 실패: 골드 부족");
        }
    }

    /// <summary>
    /// PlayerStatUI 패널을 활성화(보이게) 합니다.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        RefreshUI();
    }

    /// <summary>
    /// PlayerStatUI 패널을 비활성화(숨김) 합니다.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 공격력 업그레이드 버튼 클릭 핸들러 (인스펙터에서 연결)
    /// </summary>
    public void OnAttackUpgrade()
    {
        OnUpgrade(PlayerStatType.AttackPower);
    }

    /// <summary>
    /// 치명타 확률 업그레이드 버튼 클릭 핸들러 (인스펙터에서 연결)
    /// </summary>
    public void OnCritChanceUpgrade()
    {
        OnUpgrade(PlayerStatType.CriticalChance);
    }

    /// <summary>
    /// 치명타 대미지 업그레이드 버튼 클릭 핸들러 (인스펙터에서 연결)
    /// </summary>
    public void OnCritDamageUpgrade()
    {
        OnUpgrade(PlayerStatType.CriticalDamage);
    }

    /// <summary>
    /// 골드 획득량 업그레이드 버튼 클릭 핸들러 (인스펙터에서 연결)
    /// </summary>
    public void OnGoldGainUpgrade()
    {
        OnUpgrade(PlayerStatType.GoldGainPercent);
    }

    /// <summary>
    /// 자동공격 쿨타임 업그레이드 버튼 클릭 핸들러 (인스펙터에서 연결)
    /// </summary>
    public void OnAutoAttackCooldownUpgrade()
    {
        OnUpgrade(PlayerStatType.AutoAttackCooldownReduce);
    }
} 