using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerStatManager statManager;
    private UIManager uiManager;

    [Header("Connection")]
    private SoundManager soundManager;
    public PlayerData playerData = new PlayerData();

    [Header("Info")]
    public const int MAX_VALUE = 1000000000;
    private int baseAttack = 10;
    private float baseCritDmgPercent = 70;

    public int gold;
    public int finalAttack;
    public float finalCritical;
    public int finalCritDmg;
    public float finalGetGold;
    public int stage;
    public int damage;


    public int musicNumber;

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

    private void Start()
    {
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        soundManager.ChangeBackGroundMusic(musicNumber);  //기본 로비음악 재생
        uiManager.ShowWarning("StartGame");
    }


    public void TestWarningSign()
    {
        uiManager.ShowWarning($"this Music is MusicTrack Name is\n{soundManager.musicClips[musicNumber].name}");
    }

    public void TestMusicButton()
    {
        if (musicNumber < soundManager.musicClips.Length - 1)
        {
            musicNumber++;
        }
        else 
        {
            musicNumber = 0; 
        }
        soundManager.ChangeBackGroundMusic(musicNumber);
    }

    public void TestUseGold(int useGold)
    {
        if(UseGold(useGold))
        {
            uiManager.ShowWarning("골드사용 성공");
        }
    }

    public void TestAddGold(int addGold)
    {
        GetResource(addGold, 0);
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
            stage = playerData.StageInfo;  //스테이지 인덱스를 가져올 예정

            updateData();  //가져온 값을 게임이 실행되면 넣어주기
        }
    }

    public void updateData()
    {
        //Stage = 현 스테이지 인덱스? 데이터? 가져오기

        finalAttack = (int)Mathf.Round(baseAttack * (Mathf.Pow(1.2f, playerData.Attack)));  //*장착무기스텟 퍼뎀
        finalGetGold = (playerData.BonusGold * 5) / 100;  //장착무기스텟 보너스골드?
        finalCritical = 0.5f * playerData.Critical; //+장착무기스텟 크리
        finalCritDmg = finalAttack + (int)Mathf.Round(finalAttack * (baseCritDmgPercent + (playerData.CriticalDmg * 2))/100);

        //저장될때마다 혹은 UI창을 열어볼때마다 등등 각종 상황에서 갱신해줄것
        playerData.RefreshData(playerData);
    }

    public bool UseGold(int useGold) //재화를 사용해야되면 UseGold 함수를 호출
    { //나중에 강화석이랑 분할을 하든 업그레이드 타입에 맞춰서 변수를 변경하던 할 것
        if (useGold < 0)
        {
            uiManager.ShowWarning("잘못된 호출입니다");
            return false;
        }

        if (playerData.Gold >= useGold)
        {
            playerData.Gold -= useGold;
            updateData();
            uiManager.ShowGoldText();
            return true;
        }
        else
        {
            uiManager.ShowWarning("골드가 부족합니다\n" + uiManager.NumberText(playerData.Gold));
            return false;
        }
    }

    public bool UseEnforceStone(int enforceStone)
    { 
        if (enforceStone <= 0)
        {
            uiManager.ShowWarning("잘못된 호출입니다");
            return false;
        }

        if (playerData.EnforceStone >= enforceStone)
        {
            playerData.EnforceStone -= enforceStone;
            updateData();
            return true;
        }
        else
        {
            uiManager.ShowWarning("강화석이 부족합니다");
            return false;
        }
    }

    public void GetResource(int dropGold, int enforceStone)  //몬스터가 죽으면 GetGold를 호출
    {
        finalGetGold = dropGold + (int)Mathf.Round(dropGold * (playerData.BonusGold * 5) / 100);
        playerData.Gold += Mathf.RoundToInt(finalGetGold);
        playerData.EnforceStone += enforceStone;

        if (playerData.Gold >= MAX_VALUE)
        {
            playerData.Gold = MAX_VALUE;
        }
        if (playerData.EnforceStone <= MAX_VALUE)
        {
            playerData.EnforceStone = MAX_VALUE;
        }

        uiManager.ShowWarning($"골드 획득 {dropGold} + {(int)Mathf.Round(dropGold * (playerData.BonusGold * 5) / 100)}");
        updateData();
        uiManager.ShowGoldText();
    }

    public int StageMusic()
    {
        return ((playerData.StageInfo - 1) % (soundManager.musicClips.Length - 1) + 1);
    }
}
