using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Managers")]
    public static UIManager Instance;
    private GameManager gameManager;
    private SoundManager soundManager;

    [Header("WarningSign")]
    public GameObject warningSignPrefab;
    private Coroutine warningCoroutine;
    private List<GameObject> warningList = new List<GameObject>();
    private const int MAX_WARNING_SIGNS = 3;

    [Header("LodingDisplay")]
    public Image lodingDisplay;
    private Coroutine lodingCoroutine;
    private bool isLoding = false;

    [Header("System")]
    public const int MAX_VALUE = 1000000000;
    public const string MAX_VALUE_TEXT = "10억\n최대수치입니다";
    public TextMeshProUGUI goldText;
    public const int TITLE_MUSICNUM = 0;

    [Header("Sounds")]
    public GameObject soundUI;

    [Header("UI")]
    public Transform uiContainer;
    public GameObject titleUI;
    public Animator titleAnimator;
    public GameObject mainUI;
    

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
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        
        // UI 초기 설정
        titleUI.gameObject.SetActive(true);
        mainUI.gameObject.SetActive(false);
        lodingDisplay.gameObject.SetActive(false);
        
        // GameManager가 초기화된 경우에만 골드 표시
        if (gameManager != null)
        {
            ShowGoldText();
        }
        else
        {
            Debug.LogWarning("UIManager: GameManager.Instance is null");
            // GameManager가 초기화될 때까지 잠시 대기
            Invoke("TryShowGoldText", 0.2f);
        }
    }
    

    private void TryShowGoldText()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            ShowGoldText();
        }
        else
        {
            Debug.LogError("UIManager: GameManager.Instance is still null after delay");
        }
    }

    public void ShowGoldText()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
        
        if (gameManager != null && goldText != null)
        {
            goldText.text = $"{NumberText(gameManager.playerData.Gold)}";
        }
        else
        {
            Debug.LogWarning("ShowGoldText: GameManager is null or goldText is null");
        }
    }

    public void StartGame()
    {
        if(isLoding)
            return;

        isLoding = true;
        titleAnimator.SetBool("IsStart", true);
        if (lodingCoroutine != null)
        {
            StopCoroutine(lodingCoroutine);
        }
        lodingCoroutine = StartCoroutine(LodingSign(titleUI, mainUI));

        soundManager.ChangeBackGroundMusic(gameManager.StageMusic());
    }

    public void ReturnToTitle()
    {
        if (lodingCoroutine != null)
        {
            StopCoroutine(lodingCoroutine);
        }
        lodingCoroutine = StartCoroutine(LodingSign(mainUI, titleUI));
        
        soundManager.ChangeBackGroundMusic(TITLE_MUSICNUM);
    }

    private IEnumerator LodingSign(GameObject defaltObject, GameObject NextObject)
    {

        Color color = lodingDisplay.color;
        color.a = 0;
        lodingDisplay.color = color;

        float effecttime = 0;
        float duration = 1f;  //연출시간
        lodingDisplay.gameObject.SetActive(true);

        while (effecttime < duration)  //연출시간만큼 시간동안 알파값이 1로증가
        {
            effecttime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, effecttime / (duration / 2f));
            lodingDisplay.color = color;
            yield return null;
        }
        color.a = 1;
        lodingDisplay.color = color;

        defaltObject.SetActive(false);
        NextObject.SetActive(true);
        yield return new WaitForSeconds(0.25f); //증가된채로 0.25초 대기
        effecttime = 0;

        while (effecttime < duration / 2)  //연출시간 / 2 만큼 시간동안 알파값이 1로증가
        {
            effecttime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, effecttime / (duration / 2f));
            lodingDisplay.color = color;
            yield return null;
        }
        color.a = 0;
        lodingDisplay.color = color;  //총 지속시간 + 대기시간동안 작동 1.75초
        isLoding = false;
        lodingDisplay.gameObject.SetActive(false);  //종료
    }

    //워닝사인 동적 생성 및 배열선언으로 최대 워닝사인 갯수보다 많으면 삭제
    public void ShowWarning(string mesege)
    {
        if (warningList.Count >= MAX_WARNING_SIGNS)  
        {
            GameObject oldSign = warningList[0];
            warningList.RemoveAt(0);
            Destroy(oldSign);
        }

        GameObject warningUI = Instantiate(warningSignPrefab, uiContainer);
        warningList.Add(warningUI);

        var text = warningUI.GetComponentInChildren<TextMeshProUGUI>();
        text.text = mesege;

        warningCoroutine = StartCoroutine(WarningSign(warningUI));
    }

    private IEnumerator WarningSign(GameObject warningUI)
    {
        var text = warningUI.GetComponent<TextMeshProUGUI>();
        text.alpha = 1;  //알파값 1로 초기화

        yield return new WaitForSeconds(0.5f);

        float duration = 1.5f;  //지속시간 1.5초
        float endEffect = 0f;  //임팩트시간

        while (endEffect < duration)  //임팩트 시간이 지속시간보다 짧을 때 까지
        {
            endEffect += Time.deltaTime;  //임팩트 시간변수에 시간마다 ++
            text.alpha = Mathf.Lerp(1f, 0f, endEffect / duration);  //투명도가 1f에서 0f로 가는 간격의 비율 임팩트시간/지속시간
            yield return null;  //결론 투명도는 임팩트시간/지속시간
        }
        warningList.Remove(warningUI);
        Destroy(warningUI);
    }

    public string NumberText(int value) //예시 10억 1000만 1000 이란 숫자가 들어오면
    {
        if (value <= 0)  //0이면 0출력(0을 나누면 오류남)
            return "0";

        if (value >= MAX_VALUE)
            return MAX_VALUE_TEXT;

        string[] units = { "", "만", "억" }; //문자열 배열 선언
        List<string> parts = new List<string>();  //문자열 리스트선언

        int unitIndex = 0; //10000으로 몇번 나눴는지 카운팅

        while (value > 0 && unitIndex < units.Length)
        {
            int part = value % 10000; //10000으로 나누고 나서 나머지 값을 파츠에 저장
            if (part > 0)  //파츠가 남아있으면
            {
                parts.Insert(0, $"{part}{units[unitIndex]} "); //맨뒤에 있던 0000이 저장 다음 1000(), 10을저장
            }
            value /= 10000;
            unitIndex++;  //몇번 셌는지 카운팅
        }
        if (parts.Count >= 2)  //최상위 두개만 출력
            return $"{parts[0]}\n{parts[1]}";
        else  //그렇지않으면 하위파츠만 출력 예시 1456면 1456출력
            return parts[0];
    }

    public void OnClickSetting()
    {
        soundUI.gameObject.SetActive(true);
    }

    public void OnClickSetCancle()
    {
        soundUI.gameObject.SetActive(false);
    }
}
