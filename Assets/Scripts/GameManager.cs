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
    
    
    
    public int gold;
    public int finalAttack;
    public float finalCritical;
    public int finalCritDmg;
    public float finalGetGold;
    public int stage;
    public int damage;
    public float finalAutoAttackCooldown;

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
        GetResource(addGold, 100000);
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
            stage = playerData.Stage;  //스테이지 인덱스를 가져올 예정

            // // 테스트용 골드 추가 (골드가 0이면 10000 추가)
            if (playerData.Gold <= 0)
            {
                playerData.Gold = 10000;
                gold = playerData.Gold;
                Debug.Log("테스트용 골드 10000 추가됨");
            }

            updateData();  //가져온 값을 게임이 실행되면 넣어주기
        }
    }

    public void updateData()
    {
        //Stage = 현 스테이지 인덱스? 데이터? 가져오기
        Debug.Log($"[updateData 시작] Attack={playerData.Attack}, Critical={playerData.Critical}, CriticalDmg={playerData.CriticalDmg}, BonusGold={playerData.BonusGold}, AutoAttackCooldown={playerData.AutoAttackCooldown}");

        // playerData.Attack을 기본 공격력으로 사용하고 장착무기 공격력 추가
        int equippedWeaponAttack = GetEquippedWeaponAttack();
        finalAttack = playerData.Attack + equippedWeaponAttack;
        finalGetGold = playerData.BonusGold; 
        finalCritical = playerData.Critical; 
        int equippedWeaponCritDmg = GetEquippedWeaponCritDmg();
        finalCritDmg = playerData.CriticalDmg + equippedWeaponCritDmg;
        
        
        finalAutoAttackCooldown = playerData.AutoAttackCooldown;

        //저장될때마다 혹은 UI창을 열어볼때마다 등등 각종 상황에서 갱신해줄것
        Debug.Log($"[RefreshData 호출 전] Attack={playerData.Attack}, Critical={playerData.Critical}, CriticalDmg={playerData.CriticalDmg}, BonusGold={playerData.BonusGold}, AutoAttackCooldown={playerData.AutoAttackCooldown}");
        playerData.RefreshData(playerData); 
        Debug.Log($"[RefreshData 호출 후] Attack={playerData.Attack}, Critical={playerData.Critical}, CriticalDmg={playerData.CriticalDmg}, BonusGold={playerData.BonusGold}, AutoAttackCooldown={playerData.AutoAttackCooldown}");
    }

    /// <summary>
    /// 현재 장착된 무기의 공격력을 반환합니다.
    /// </summary>
    /// <returns>장착된 무기의 공격력</returns>
    private int GetEquippedWeaponAttack()
    {
        // TODO: 장착된 무기 정보를 가져와서 공격력 반환 로직 구현
        // 현재는 임시로 0 반환
        return 0;
    }
    
    /// <summary>
    /// 현재 장착된 무기의 크리티컬 데미지를 반환합니다.
    /// </summary>
    /// <returns>장착된 무기의 크리티컬 데미지</returns>
    private int GetEquippedWeaponCritDmg()
    {
        // TODO: 장착된 무기의 크리티컬 데미지 반환 로직 구현
        // 현재는 임시로 0 반환
        return 0;
    }

    public bool UseGold(int useGold) //재화를 사용해야되면 UseGold 함수를 호출
    { //나중에 강화석이랑 분할을 하든 업그레이드 타입에 맞춰서 변수를 변경하던 할 것
        // uiManager가 null인 경우에도 골드 차감은 정상 처리
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
        }
        
        if (useGold < 0)
        {
            if (uiManager != null) uiManager.ShowWarning("잘못된 호출입니다");
            else Debug.LogWarning("잘못된 호출입니다 (UIManager is null)");
            return false;
        }

        if (playerData.Gold >= useGold)
        {
            playerData.Gold -= useGold;
            updateData();
            if (uiManager != null) uiManager.ShowGoldText();
            return true;
        }
        else
        {
            if (uiManager != null) uiManager.ShowWarning("골드가 부족합니다\n" + uiManager.NumberText(playerData.Gold));
            else Debug.LogWarning($"골드가 부족합니다 (필요: {useGold}G, 보유: {playerData.Gold}G)");
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
        return ((playerData.Stage - 1) % (soundManager.musicClips.Length - 1) + 1);
    }

    public int FinalAttack(bool isCritical)
    {//공격시 bool isCritical()을 실행시켜 (공격에서 임팩트를 주기위해서 이 함수가 필요) 크리티컬 여부판단
        if (isCritical)//크리티컬이 발동되면
        {
            // 크리티컬 데미지는 기본 공격력 + (기본 공격력 * 크리티컬 데미지 / 100)
            damage = finalAttack + (int)(finalAttack * (finalCritDmg / 100f));
            return damage; //데미지값을 반환
        }
        return finalAttack;  //크리티컬이 안뜨면 그대로 finalAttack 반환
    }

    public bool isCritical()
    {
        float isCritical = Random.Range(0f, 100f); //float값으로 랜덤을 돌려서
        if (isCritical <= finalCritical) //나온숫자가 크리티컬 수치보다 작거나 같다면
        {
            return true;  //크리티컬 발동을위해 true반환
        }
        else
        {
            return false;  //아니라면 false반환
        }
    }
}
