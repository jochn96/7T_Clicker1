using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public GameObject critChanceMaxText; // 최대 업그레이드 시 표시할 MAX 텍스트

    [Header("치명타 대미지 UI")]
    public TextMeshProUGUI critDamageValueText;
    public TextMeshProUGUI critDamageCostText;
    public Button critDamageUpgradeButton;
    public GameObject critDamageMaxText; // 최대 업그레이드 시 표시할 MAX 텍스트

    [Header("골드 획득량 UI")]
    public TextMeshProUGUI goldGainValueText;
    public TextMeshProUGUI goldGainCostText;
    public Button goldGainUpgradeButton;
    public GameObject goldGainMaxText; // 최대 업그레이드 시 표시할 MAX 텍스트

    [Header("자동공격 쿨타임 UI")]
    public TextMeshProUGUI autoAttackCooldownValueText;
    public TextMeshProUGUI autoAttackCooldownCostText;
    public Button autoAttackCooldownUpgradeButton;
    public GameObject autoAttackCooldownMaxText; // 최대 업그레이드 시 표시할 MAX 텍스트
    public GameObject autoAttackCooldownUpgradePanel; // 자동공격 쿨타임 업그레이드 패널
    
    [Header("자동공격 구매 UI")]
    public GameObject autoAttackPurchasePanel;
    public TextMeshProUGUI autoAttackPurchaseCostText;
    public Button autoAttackPurchaseButton;
    
    [Header("골드 부족 UI")]
    public Image notEnoughGoldPanel; // 골드 부족 알림 패널
    public TextMeshProUGUI notEnoughGoldText; // 골드 부족 알림 텍스트
    
    // 최대 업그레이드 값 상수
    private const float MAX_CRIT_CHANCE = 100f;
    private const float MAX_CRIT_DAMAGE = 250f;
    private const float MAX_GOLD_GAIN = 100f;
    private const float MAX_AUTO_ATTACK_COOLDOWN = 1.0f;

    private Player player;
    private GameManager gameManager;
    private int currentGold; // 실제로는 GameManager 등에서 받아와야 함

    private void Awake()
    {
        // Player 및 GameManager 초기화 - 여러 번 시도
        StartCoroutine(InitializeManagers());
        
        // 골드 부족 패널 초기 설정
        if (notEnoughGoldPanel != null)
        {
            notEnoughGoldPanel.gameObject.SetActive(false);
        }
        
        // MAX 텍스트 초기 설정
        InitializeMaxTexts();
    }
    
    /// <summary>
    /// Player와 GameManager를 초기화하는 코루틴
    /// </summary>
    private System.Collections.IEnumerator InitializeManagers()
    {
        // 최대 5번 시도
        int attempts = 0;
        while (attempts < 5)
        {
            player = FindObjectOfType<Player>();
            gameManager = GameManager.Instance;
            
            if (player != null && gameManager != null)
            {
                Debug.Log("PlayerStatUI: Player와 GameManager 초기화 성공");
                break;
            }
            
            attempts++;
            yield return new WaitForSeconds(0.1f);
        }
        
        if (player == null || gameManager == null)
        {
        
        }
    }

    /// <summary>
    /// MAX 텍스트 초기화
    /// </summary>
    private void InitializeMaxTexts()
    {
        if (critChanceMaxText != null) critChanceMaxText.SetActive(false);
        if (critDamageMaxText != null) critDamageMaxText.SetActive(false);
        if (goldGainMaxText != null) goldGainMaxText.SetActive(false);
        if (autoAttackCooldownMaxText != null) autoAttackCooldownMaxText.SetActive(false);
    }

    private void Start()
    {
        // GameManager 초기화 확인
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                Debug.LogError("GameManager.Instance is null! PlayerStatUI 초기화 실패");
                return;
            }
        }
        
        // Player 초기화 확인
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            
            if (player == null)
            {
                Debug.LogError("Player를 찾을 수 없습니다! PlayerStatUI 초기화 실패");
                return;
            }
        }
        
        // 시작 시 UI 갱신
        RefreshUI();
        
        // 약간의 지연 후 버튼 리스너 등록 (모든 매니저가 초기화된 후)
        Invoke("SetupButtonListeners", 0.1f);
        
        // 디버그 로그
        Debug.Log($"PlayerStatUI 초기화 완료. 현재 골드: {gameManager.playerData.Gold}");
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
        
        // 마우스 클릭 감지하여 골드 부족 패널 닫기
        if (Input.GetMouseButtonDown(0))
        {
            // 골드 부족 패널이 활성화되어 있다면 닫기
            if (notEnoughGoldPanel != null && notEnoughGoldPanel.gameObject.activeSelf)
            {
                CloseNotEnoughGoldPanel();
            }
        }
    }

    /// <summary>
    /// 현재 골드 UI를 갱신합니다.
    /// </summary>
    private void UpdateGoldUI()
    {
        // GameManager 체크 및 재시도
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogWarning("UpdateGoldUI: GameManager is null");
                return;
            }
        }
        
        if (currentGoldText != null)
        {
            try
            {
                // GameManager에서 현재 골드 정보 가져오기
                currentGold = gameManager.playerData.Gold;
                
                // 골드 표시 (천 단위 콤마 포함)
                if (currentGold <= 0)
                {
                    currentGoldText.text = "0 G";
                }
                else if (currentGold >= 1000000000) // 10억 이상
                {
                    currentGoldText.text = "10억 G";
                }
                else
                {
                    currentGoldText.text = string.Format("{0:#,##0} G", currentGold);
                }
                
                // 디버그 로그
                
            }
            catch (System.Exception e)
            {
               
            }
        }
    }

    //  public void ShowGoldText()
    // {
    //     if (gameManager == null)
    //     {
    //         gameManager = GameManager.Instance;
    //     }
        
    //     if (gameManager != null && goldText != null)
    //     {
    //         goldText.text = $"{NumberText(gameManager.playerData.Gold)}";
    //     }
    //     else
    //     {
    //         Debug.LogWarning("ShowGoldText: GameManager is null or goldText is null");
    //     }
    // }

    /// <summary>
    /// UI를 갱신합니다.
    /// </summary>
    public void RefreshUI()
    {
        // 골드 UI 갱신
        UpdateGoldUI();

        // 공격력 UI 갱신
        attackValueText.text = player.GetStatValue(PlayerStatType.AttackPower).ToString();
        attackCostText.text = player.GetUpgradeCost(PlayerStatType.AttackPower) + "G";

        // 치명타 확률 UI 갱신
        float critChance = player.GetStatValue(PlayerStatType.CriticalChance);
        critChanceValueText.text = critChance + "%";
        critChanceCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalChance) + "G";
        
        // 치명타 확률 최대치 확인 및 UI 업데이트
        bool isCritChanceMax = critChance >= MAX_CRIT_CHANCE;
        if (critChanceUpgradeButton != null) critChanceUpgradeButton.interactable = !isCritChanceMax;
        if (critChanceMaxText != null) critChanceMaxText.SetActive(isCritChanceMax);
        if (isCritChanceMax) critChanceCostText.text = "MAX";

        // 치명타 데미지 UI 갱신
        float critDamage = player.GetStatValue(PlayerStatType.CriticalDamage);
        critDamageValueText.text = critDamage + "%";
        critDamageCostText.text = player.GetUpgradeCost(PlayerStatType.CriticalDamage) + "G";
        
        // 치명타 데미지 최대치 확인 및 UI 업데이트
        bool isCritDamageMax = critDamage >= MAX_CRIT_DAMAGE;
        if (critDamageUpgradeButton != null) critDamageUpgradeButton.interactable = !isCritDamageMax;
        if (critDamageMaxText != null) critDamageMaxText.SetActive(isCritDamageMax);
        if (isCritDamageMax) critDamageCostText.text = "MAX";

        // 골드 획득량 UI 갱신
        float goldGain = player.GetStatValue(PlayerStatType.GoldGainPercent);
        goldGainValueText.text = goldGain + "%";
        goldGainCostText.text = player.GetUpgradeCost(PlayerStatType.GoldGainPercent) + "G";
        
        // 골드 획득량 최대치 확인 및 UI 업데이트
        bool isGoldGainMax = goldGain >= MAX_GOLD_GAIN;
        if (goldGainUpgradeButton != null) goldGainUpgradeButton.interactable = !isGoldGainMax;
        if (goldGainMaxText != null) goldGainMaxText.SetActive(isGoldGainMax);
        if (isGoldGainMax) goldGainCostText.text = "MAX";

        // 자동공격 쿨타임 UI 갱신
        // 실제 쿨다운 시간을 표시 (5초 - 감소량)
        float autoAttackCooldownReduction = player.GetStatValue(PlayerStatType.AutoAttackCooldownReduce);
        float actualCooldown = Mathf.Max(1.0f, 5.0f - Mathf.Abs(autoAttackCooldownReduction));
        autoAttackCooldownValueText.text = actualCooldown.ToString("F1") + "초";
        autoAttackCooldownCostText.text = player.GetUpgradeCost(PlayerStatType.AutoAttackCooldownReduce) + "G";
        
        // 자동공격 쿨타임 최대치 확인 및 UI 업데이트
        bool isAutoAttackCooldownMax = actualCooldown <= 1.0f;
        if (autoAttackCooldownUpgradeButton != null) autoAttackCooldownUpgradeButton.interactable = !isAutoAttackCooldownMax;
        if (autoAttackCooldownMaxText != null) autoAttackCooldownMaxText.SetActive(isAutoAttackCooldownMax);
        if (isAutoAttackCooldownMax) autoAttackCooldownCostText.text = "MAX";
        
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
            ShowNotEnoughGoldPanel(cost);
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
            ShowNotEnoughGoldPanel(player.autoAttackUnlockCost);
        }
    }
    
    /// <summary>
    /// 골드 부족 패널을 표시합니다.
    /// </summary>
    private void ShowNotEnoughGoldPanel(int requiredGold)
    {
        if (notEnoughGoldPanel == null) return;
        
        // 골드 부족 패널 활성화
        notEnoughGoldPanel.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 골드 부족 패널을 닫습니다.
    /// </summary>
    public void CloseNotEnoughGoldPanel()
    {
        if (notEnoughGoldPanel != null)
        {
            notEnoughGoldPanel.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 화면의 아무 곳이나 클릭했을 때 호출되는 메서드 (인스펙터에서 연결)
    /// </summary>
    public void OnClickAnywhere()
    {
        // 골드 부족 패널이 활성화되어 있다면 닫기
        if (notEnoughGoldPanel != null && notEnoughGoldPanel.gameObject.activeSelf)
        {
            CloseNotEnoughGoldPanel();
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