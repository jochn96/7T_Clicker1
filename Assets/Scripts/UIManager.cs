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
    public TextMeshProUGUI warningText;
    private Coroutine warningCoroutine;

    [Header("LodingDisplay")]
    public Image lodingDisplay;
    private Coroutine lodingCoroutine;

    [Header("System")]
    public const int MAX_VALUE = 1000000000;
    public TextMeshProUGUI goldText;
    public const int TITLE_MUSICNUM = 0;

    [Header("UI")]
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
        titleUI.gameObject.SetActive(true);
        mainUI.gameObject.SetActive(false);
        lodingDisplay.gameObject.SetActive(false);
        warningText.gameObject.SetActive(false);
    }

    private void Update()
    {
        goldText.text = $"{NumberText(gameManager.playerData.Gold)}";
    }

    public void StartGame()
    {
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
            warningText.alpha = Mathf.Lerp(1f, 0f, endEffect / duration);  //투명도가 1f에서 0f로 가는 간격의 비율 임팩트시간/지속시간
            yield return null;  //결론 투명도는 임팩트시간/지속시간
        }
        warningText.gameObject.SetActive(false);  //투명도가 0이될쯤 종료
    }

    public string NumberText(int value) //예시 10조 1000억 1000만 이란 숫자가 들어오면
    {
        if (value <= 0)  //0이면 0출력(0을 나누면 오류남)
            return "0";

        if (value >= MAX_VALUE)
            return MAX_VALUE.ToString();

        string[] units = { "", "만", "억" }; //문자열 배열 선언
        List<string> parts = new List<string>();  //문자열 리스트선언

        int unitIndex = 0; //10000으로 몇번 나눴는지 카운팅

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
