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
    
    PlayerData playerData = new PlayerData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        playerDataLoad();
    }

    public void Update()
    {

    }
    
    public void playerDataLoad()
    {
        playerData = SaveDataToJSON.LoadUsers();

        if (playerData != null) //실제로는 스텟을 가져올것 
        {   //임시코드입니다
            gold = playerData.Gold;
            finalAttack = playerData.Attack;
            finalCritical = playerData.Critical;
            finalCritDmg = playerData.CriticalDmg;
            finalGetGold = playerData.BonusGold;
            stage = playerData.Stage;
        }
    }

    public void updateData()
    {
        //Stage = 현 스테이지 인덱스? 데이터? 가져오기

        //finalAttack = finalAttack = 전체 데미지 + (보너스 데미지 퍼센트)
        //finalGetGold = 획득골드 + (획득골드 * 보너스 골드)
        //finalCritDmg = finalAttack * 크리티컬 데미지 보너스 퍼센트
        //finalCritical = 크리티컬 확률 + 보너스 크리티컬 확률(무기보너스등)

        //저장될때마다 혹은 UI창을 열어볼때마다 등등 각종 상황에서 갱신해줄것

        SaveDataToJSON.SaveUsers(playerData);
    }

    public bool UseGold(int useGold) //재화를 사용해야되면 UseGold 함수를 호출
    { 
        if (gold >= useGold)
        {
            gold -= useGold;
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
        return damage;
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
        int eok = value / 100000000;
        int man = (value % 100000000) / 10000;
        int rest = value % 10000;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (eok > 0)
            sb.Append(eok).Append("억 ");
        if (man > 0)
            sb.Append(man).Append("만 ");
        if (rest > 0 || sb.Length == 0)
            sb.Append(rest);

        return sb.ToString().Trim();
    }
}
