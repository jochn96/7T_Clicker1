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
    private GameManager gameManager;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gameManager = GameManager.Instance;

        // 버튼에 리스너 등록
        attackUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AttackPower));
        critChanceUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalChance));
        critDamageUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalDamage));
        goldGainUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.GoldGainPercent));
        autoAttackCooldownUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AutoAttackCooldownReduce));
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
        // 현재 골드 표시
        currentGoldText.text = gameManager.NumberText(gameManager.gold) + "G";

        // 공격력
        attackValueText.text = gameManager.finalAttack.ToString();
        int attackCost = player.GetUpgradeCost(PlayerStatType.AttackPower);
        attackCostText.text = gameManager.NumberText(attackCost) + "G";
        attackUpgradeButton.interactable = gameManager.gold >= attackCost;

        // 치명타 확률
        critChanceValueText.text = gameManager.finalCritical + "%";
        int critChanceCost = player.GetUpgradeCost(PlayerStatType.CriticalChance);
        critChanceCostText.text = gameManager.NumberText(critChanceCost) + "G";
        critChanceUpgradeButton.interactable = gameManager.gold >= critChanceCost;

        // 치명타 데미지
        critDamageValueText.text = gameManager.finalCritDmg + "%";
        int critDamageCost = player.GetUpgradeCost(PlayerStatType.CriticalDamage);
        critDamageCostText.text = gameManager.NumberText(critDamageCost) + "G";
        critDamageUpgradeButton.interactable = gameManager.gold >= critDamageCost;

        // 골드 획득량
        goldGainValueText.text = gameManager.finalGetGold + "%";
        int goldGainCost = player.GetUpgradeCost(PlayerStatType.GoldGainPercent);
        goldGainCostText.text = gameManager.NumberText(goldGainCost) + "G";
        goldGainUpgradeButton.interactable = gameManager.gold >= goldGainCost;

        // 자동공격 쿨타임
        float cooldownValue = player.GetStatValue(PlayerStatType.AutoAttackCooldownReduce);
        autoAttackCooldownValueText.text = cooldownValue.ToString("F1") + "s";
        int cooldownCost = player.GetUpgradeCost(PlayerStatType.AutoAttackCooldownReduce);
        autoAttackCooldownCostText.text = gameManager.NumberText(cooldownCost) + "G";
        autoAttackCooldownUpgradeButton.interactable = gameManager.gold >= cooldownCost;
    }

    /// <summary>
    /// +버튼 클릭 시 업그레이드 시도 및 UI 갱신
    /// </summary>
    private void OnUpgrade(PlayerStatType statType)
    {
        int cost = player.GetUpgradeCost(statType);
        if (gameManager.UseGold(cost))
        {
            if (player.UpgradeStat(statType, cost))
            {
                gameManager.updateData(); // 게임매니저 데이터 갱신
                RefreshUI();
            }
            else
            {
                gameManager.gold += cost; // 업그레이드 실패시 골드 환불
                Debug.Log($"업그레이드 실패: {statType}");
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
} 