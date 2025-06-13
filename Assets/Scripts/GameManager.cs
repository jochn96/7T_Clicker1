using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public int musicNumber;

    [Header("UI")]
    public TextMeshProUGUI warningText;
    private Coroutine warningCoroutine;
    public Image lodingDisplay;
    private Coroutine lodingCoroutine;
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

    private void Start()
    {
        lodingDisplay.gameObject.SetActive(false);
        warningText.gameObject.SetActive(false);
        soundManager.ChangeBackGroundMusic(musicNumber);  //기본 로비음악 재생
        ShowWarning("StartGame");
    }

    public void TestLoding()
    {
        ShowLoding();
    }

    public void TestWarningSign()
    {
        ShowWarning($"this Music is MusicTrack Name is\n{soundManager.musicClips[musicNumber].name}");
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
        if (useGold <= 0)
        {
            ShowWarning("잘못된 호출입니다");
            return false;
        }

        if (playerData.Gold >= useGold)
        {
            playerData.Gold -= useGold;
            return true;
        }
        else
        {
            ShowWarning("골드가 부족합니다");
            return false;
        }
    }

    public bool UseEnforceStone(int enforceStone)
    { 
        if (enforceStone <= 0)
        {
            ShowWarning("잘못된 호출입니다");
            return false;
        }

        if (playerData.EnforceStone >= enforceStone)
        {
            playerData.EnforceStone -= enforceStone;
            return true;
        }
        else
        {
            ShowWarning("강화석이 부족합니다");
            return false;
        }
    }

    public void GetGold(int dropGold, int enforceStone)  //몬스터가 죽으면 GetGold를 호출
    {
        //finalGetGold = dropGold + (dropGold * ((보너스골드 스텟 * 5) /100 보너스골드 스텟당 몇퍼센트로할지 상의))
        playerData.Gold += Mathf.RoundToInt(finalGetGold);
        playerData.EnforceStone += enforceStone;
    }

    public void ShowLoding()
    {
        if (lodingCoroutine != null)
        {
            StopCoroutine(lodingCoroutine);
        }
        lodingCoroutine = StartCoroutine(LodingSign());
    }
    private IEnumerator LodingSign()
    {
        Color color = lodingDisplay.color;
        color.a = 0;
        lodingDisplay.color = color;

        float effecttime = 0;
        float duration = 0.5f;  //연출시간
        lodingDisplay.gameObject.SetActive(true);

        while (effecttime < duration/2)  //연출시간 / 2만큼 시간동안 알파값이 1로증가
        {
            effecttime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, effecttime / (duration/2f));
            lodingDisplay.color = color;
            yield return null;
        }
        color.a = 1;
        lodingDisplay.color = color;
        yield return new WaitForSeconds(0.25f); //증가된채로 0.25초 대기
        effecttime = 0;

        while (effecttime < duration / 2)  //위와 동일코드
        {
            effecttime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, effecttime / (duration/2f));
            lodingDisplay.color = color;
            yield return null;
        }
        color.a = 0;
        lodingDisplay.color= color;  //총 지속시간 + 대기시간동안 작동 0.75초
        lodingDisplay.gameObject.SetActive(false);  //종료
    }

    public void ShowWarning(string mesege)
    {
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }
        warningCoroutine = StartCoroutine(WarningSign(mesege));
    }

    private IEnumerator WarningSign(string message)
    {
        warningText.alpha = 1;  //알파값 1로 초기화
        warningText.text = message;  //메세지를 미리 바꾸고
        warningText.gameObject.SetActive(true);  //게임오브젝트 활성화

        yield return new WaitForSeconds(0.5f);

        float duration = 1.5f;  //지속시간 1.5초
        float endEffect = 0f;  //임팩트시간

        while (endEffect < duration)  //임팩트 시간이 지속시간보다 짧을 때 까지
        {
            endEffect += Time.deltaTime;  //임팩트 시간변수에 시간마다 ++
            warningText.alpha = Mathf.Lerp(1f,0f,endEffect/duration);  //투명도가 1f에서 0f로 가는 간격의 비율 임팩트시간/지속시간
            yield return null;  //결론 투명도는 임팩트시간/지속시간
        }
        warningText.gameObject.SetActive(false);  //투명도가 0이될쯤 종료
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

    public int FinalAttack(bool isCritical)
    {//공격시 bool isCritical()을 실행시켜 (공격에서 임팩트를 주기위해서 이 함수가 필요) 크리티컬 여부판단
        //finalAttack = 전체 데미지 + (보너스 데미지 퍼센트)
        if (isCritical)//크리티컬이 발동되면
        {
            //finalCritDmg = finalAttack * 크리티컬 데미지 보너스 퍼센트
            damage = finalAttack + finalCritDmg; //데미지는 기존데미지 + 크리티컬로 발동된 추가데미지
            return damage; //데미지값을 반환
        }
        return damage;  //크리티컬이 안뜨면 그대로 데미지값 반환
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
