using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int gold;

    public int finalAttack;
    public int finalCritical;
    public int finalCritDmg;
    public int finalGetGold;

    public int stage;

    public int damage;
    
    private Player player;
    private PlayerStatUI playerStatUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //테스트용 골드
        gold += 100000;
        player = FindObjectOfType<Player>();
        playerStatUI = FindObjectOfType<PlayerStatUI>();
        if (playerStatUI != null)
        {
            playerStatUI.RefreshUI();
        }
    }

    public void Update()
    {
        // 필요한 업데이트 로직
    }
    
    public void playerDataLoad()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (player != null && player.playerData != null)
        {
            gold = player.playerData.Gold;
            finalAttack = player.playerData.Attack;
            finalCritical = player.playerData.Critical;
            finalCritDmg = player.playerData.CriticalDmg;
            finalGetGold = player.playerData.BonusGold;
            stage = player.playerData.Stage;
            if (playerStatUI != null)
            {
                playerStatUI.RefreshUI();
            }
        }
    }

    public void updateData()
    {
        
        if (player == null) player = FindObjectOfType<Player>();
        if (player == null || player.playerData == null) return;
        
        //todo : 장비스탯 여기에 추가
        int equipmentAttack = 0;
        int equipmentCritical = 0;
        int equipmentCritDmg = 0;
        int equipmentBonusGold = 0;
        // 최종 스탯 계산 (업그레이드 누적값 + 기본값 + 장비)
        finalAttack = player.playerData.Attack + equipmentAttack;
        finalCritical = player.playerData.Critical + equipmentCritical;
        finalCritDmg = player.playerData.CriticalDmg + equipmentCritDmg;
        finalGetGold = player.playerData.BonusGold + equipmentBonusGold;
        SaveDataToJSON.SaveUsers(player.playerData);
        if (playerStatUI != null)
            playerStatUI.RefreshUI();
    }
    

    public bool UseGold(int useGold) //재화를 사용해야되면 UseGold 함수를 호출
    { 
        if (gold >= useGold)
        {
            gold -= useGold;
            if (playerStatUI != null)
            {
                playerStatUI.RefreshUI();
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    public void GetGold()  //몬스터가 죽으면 GetGold를 호출
    {
        //finalGetGold = 획득골드 + (획득골드 * 보너스 골드)
        gold += finalGetGold;
        if (playerStatUI != null)
        {
            playerStatUI.RefreshUI();
        }
    }

    public int FinalAttack(bool isCritical)
    {
        //finalAttack = 전체 데미지 + (보너스 데미지 퍼센트)
        if (isCritical)
        {
            //finalCritDmg = finalAttack * 크리티컬 데미지 보너스 퍼센트
            damage = finalAttack + finalCritDmg;
            return damage;
        }
        return finalAttack;
    }

    public bool isCritical()
    {
        float isCritical = Random.Range(0,100);
        if (isCritical <= finalCritical)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string NumberText(int value)
    {
        if (value == 0) return "0";

        int eok = value / 100000000;
        int man = (value % 100000000) / 10000;
        int rest = value % 10000;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (eok > 0)
            sb.Append(eok).Append("B ");
        if (man > 0)
            sb.Append(man).Append("M ");
        if (rest > 0 || sb.Length == 0)
            sb.Append(rest);

        return sb.ToString().Trim();
    }
}
