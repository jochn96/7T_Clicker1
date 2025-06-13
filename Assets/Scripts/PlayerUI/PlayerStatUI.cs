using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 플레이어 능력치 UI를 관리합니다.
/// </summary>
public class PlayerStatUI : MonoBehaviour
{
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

    [Header("현재 골드 UI")]
    public TextMeshProUGUI currentGoldText;

    private Player player;
    private PlayerStat playerStat;
    private GameManager gameManager;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerStat = player?.GetComponent<PlayerStat>();
        gameManager = GameManager.Instance;

        // 버튼에 리스너 등록
        attackUpgradeButton.onClick.AddListener(OnUpgradeAttack);
        critChanceUpgradeButton.onClick.AddListener(OnUpgradeCriticalChance);
        critDamageUpgradeButton.onClick.AddListener(OnUpgradeCriticalDamage);
        goldGainUpgradeButton.onClick.AddListener(OnUpgradeGoldGain);
        autoAttackCooldownUpgradeButton.onClick.AddListener(OnUpgradeAutoAttackCooldown);
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    /// <summary>
    /// UI를 갱신합니다.
    /// </summary>
    public void RefreshUI()
    {
        if (player == null || gameManager == null) return;

        // 현재 골드 표시
        currentGoldText.text = gameManager.NumberText(gameManager.gold) + "G";

        // 공격력
        attackValueText.text = gameManager.finalAttack.ToString();
        int attackCost = playerStat?.GetUpgradeCost(PlayerStatType.AttackPower) ?? 0;
        attackCostText.text = gameManager.NumberText(attackCost) + "G";
        attackUpgradeButton.interactable = gameManager.gold >= attackCost;

        // 치명타 확률
        critChanceValueText.text = gameManager.finalCritical + "%";
        int critChanceCost = playerStat?.GetUpgradeCost(PlayerStatType.CriticalChance) ?? 0;
        critChanceCostText.text = gameManager.NumberText(critChanceCost) + "G";
        critChanceUpgradeButton.interactable = gameManager.gold >= critChanceCost;

        // 치명타 데미지
        critDamageValueText.text = gameManager.finalCritDmg + "%";
        int critDamageCost = playerStat?.GetUpgradeCost(PlayerStatType.CriticalDamage) ?? 0;
        critDamageCostText.text = gameManager.NumberText(critDamageCost) + "G";
        critDamageUpgradeButton.interactable = gameManager.gold >= critDamageCost;

        // 골드 획득량
        goldGainValueText.text = gameManager.finalGetGold + "%";
        int goldGainCost = playerStat?.GetUpgradeCost(PlayerStatType.GoldGainPercent) ?? 0;
        goldGainCostText.text = gameManager.NumberText(goldGainCost) + "G";
        goldGainUpgradeButton.interactable = gameManager.gold >= goldGainCost;

        // 자동공격 쿨타임
        if (autoAttackCooldownValueText != null)
        {
            float autoAttackValue = playerStat?.GetStatValue(PlayerStatType.AutoAttackCooldownReduce) ?? 0f;
            autoAttackCooldownValueText.text = autoAttackValue.ToString("F2") + "초";
            
            int autoAttackCost = playerStat?.GetUpgradeCost(PlayerStatType.AutoAttackCooldownReduce) ?? 0;
            if (autoAttackCooldownCostText != null)
            {
                autoAttackCooldownCostText.text = gameManager.NumberText(autoAttackCost) + "G";
            }
            if (autoAttackCooldownUpgradeButton != null)
            {
                autoAttackCooldownUpgradeButton.interactable = gameManager.gold >= autoAttackCost;
            }
        }
    }

    /// <summary>
    /// 공격력 업그레이드 버튼 클릭 처리
    /// </summary>
    private void OnUpgradeAttack()
    {
        if (playerStat == null) return;
        playerStat.UpgradeAttack();
        RefreshUI();
    }

    /// <summary>
    /// 치명타 확률 업그레이드 버튼 클릭 처리
    /// </summary>
    private void OnUpgradeCriticalChance()
    {
        if (playerStat == null) return;
        playerStat.UpgradeCriticalChance();
        RefreshUI();
    }

    /// <summary>
    /// 치명타 데미지 업그레이드 버튼 클릭 처리
    /// </summary>
    private void OnUpgradeCriticalDamage()
    {
        if (playerStat == null) return;
        playerStat.UpgradeCriticalDamage();
        RefreshUI();
    }

    /// <summary>
    /// 골드 획득량 업그레이드 버튼 클릭 처리
    /// </summary>
    private void OnUpgradeGoldGain()
    {
        if (playerStat == null) return;
        playerStat.UpgradeGoldGain();
        RefreshUI();
    }

    /// <summary>
    /// 자동공격 쿨타임 업그레이드 버튼 클릭 처리
    /// </summary>
    private void OnUpgradeAutoAttackCooldown()
    {
        if (playerStat == null) return;
        playerStat.UpgradeAutoAttackCooldown();
        RefreshUI();
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
} 