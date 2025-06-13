using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Connection")]
    public SoundManager soundManager;

    [Header("Info")]
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

    public void PlayEffect(AudioClip clip)
    {
        soundManager.PlayClip(clip);
    }

    public void playerDataLoad()
    {
        playerData = SaveDataToJSON.LoadUsers();

        if (playerData != null) //실제로는 스텟을 가져올것 
        {   //임시코드입니다
            gold = playerData.Gold; //플레이어 총 골드 가져올 예정
            finalAttack = playerData.Attack;  //플레이어 공격력 레벨 가져올예정
            finalCritical = playerData.Critical;  //플레이어 크리티컬 레벨 가져올예정
            finalCritDmg = playerData.CriticalDmg;  //플레이어 크리티컬 데미지 레벨 가져올 예정
            finalGetGold = playerData.BonusGold;  //플레이어 골드 보너스 가져올 예정
            stage = playerData.Stage;  //스테이지 인덱스를 가져올 예정

            updateData();  //가져온 값을 게임이 실행되면 넣어주기
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
    { //나중에 강화석이랑 분할을 하든 업그레이드 타입에 맞춰서 변수를 변경하던 할 것
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

    public string NumberText(int value) //예시 10조 1000억 1000만 이란 숫자가 들어오면
    {
        string[] units = { "", "만", "억", "조" }; //문자열 배열 선언
        List<string> parts = new List<string>();  //문자열 리스트선언

        int unitIndex = 0; //10000으로 몇번 나눴는지 카운팅
        if (value < 0)  //0보다 작으면 0을 출력
            value = 0;
        if (value > int.MaxValue) //int 최댓값을 넘기지 못하게 하는 방어코드
            value = int.MaxValue;

        if (value == 0)  //0이면 0출력(0을 나누면 오류남)
            return "0";

        while (value > 0 && unitIndex < units.Length)
        {
            int part = value % 10000; //10000으로 나누고 나서 나머지 값을 파츠에 저장
            if (part > 0)  //파츠가 남아있으면
            {
                parts.Insert(0, $"{part}{units[unitIndex]} "); //맨뒤에 있던 0000이 저장 다음 1000(), 1000, 10을저장
            }
            value /= 10000; 
            unitIndex++;  //몇번 셌는지 카운팅
        }
        if (parts.Count >= 2)  //최상위 두개만 출력
            return $"{parts[0]}\n{parts[1]}";
        else  //그렇지않으면 하위파츠만 출력 예시 1456면 1456출력
            return parts[0];
    }
}
