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

    private Player player;
    private int currentGold; // 실제로는 GameManager 등에서 받아와야 함

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        // 버튼에 리스너 등록
        // attackUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AttackPower));
        // critChanceUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalChance));
        // critDamageUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.CriticalDamage));
        // goldGainUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.GoldGainPercent));
        // autoAttackCooldownUpgradeButton.onClick.AddListener(() => OnUpgrade(PlayerStatType.AutoAttackCooldownReduce));
    }

    /// <summary>
    /// UI를 갱신합니다.
    /// </summary>
    public void RefreshUI()
    {
        attackValueText.text = player.GetStatValue(PlayerStatType.AttackPower).ToString();
        attackCostText.text = player.GetUpgradeCost(PlayerStatType.AttackPower) + "G";

        critChanceValueText.text = player.GetStatValue(PlayerStatType.CriticalChance) + "%";
        critChanceCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalChance) + "G";

        // 치명타 데미지 %로 표시
        critDamageValueText.text = (player.GetStatValue(PlayerStatType.CriticalDamage) * 100f).ToString("F0") + "%";
        critDamageCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalDamage) + "G";

        goldGainValueText.text = player.GetStatValue(PlayerStatType.GoldGainPercent) + "%";
        goldGainCostText.text = player.GetUpgradeCost(PlayerStatType.GoldGainPercent) + "G";

        autoAttackCooldownValueText.text = player.GetStatValue(PlayerStatType.AutoAttackCooldownReduce).ToString();
        autoAttackCooldownCostText.text = player.GetUpgradeCost(PlayerStatType.AutoAttackCooldownReduce) + "G";
    }

    /// <summary>
    /// +버튼 클릭 시 업그레이드 시도 및 UI 갱신
    /// </summary>
    // private void OnUpgrade(PlayerStatType statType)
    // {
    //     if (player.UpgradeStat(statType, currentGold))
    //     {
    //         RefreshUI();
    //     }
    //     else
    //     {
    //         Debug.Log("돈부족");
    //     }
    // }

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