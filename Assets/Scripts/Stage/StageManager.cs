using UnityEngine;
using UnityEngine.UIElements;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageInfo[] stageInfos;
    [SerializeField] private int stageKey;
    [SerializeField] private EnemyCenter[] enemyCenters;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform target; // 적이 소환될 위치
    public int thisStageEnemy;
    public EnemyCenter enemyCenter;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;

        if (stageKey >= 0 && stageKey < stageInfos.Length)
        {
            StageInfo stageInfo = stageInfos[stageKey];
            StageData.Stage[stageKey] = stageInfo;

            // 적 초기화 또는 스폰 로직
            InitializeStage(stageInfo);
        }
        else
        {
            Debug.LogError($"잘못된 스테이지 키: {stageKey}");
        }
        thisStageEnemy = 0;
    }

    private void InitializeStage(StageInfo stageInfo)
    {
        if (stageInfo.Waves != null && stageInfo.Waves.Length > 0)
        {
            // 첫 번째 웨이브 시작
            StartWave(stageInfo.Waves[0]);
        }
    }

    private void StartWave(WaveData wave)
    {
        if (wave.enemies != null)
        {
            foreach (var enemyData in wave.enemies)
            {
                // 적 생성 로직
                SpawnEnemies();
            }

            if (wave.hasboss)
            {
                // 보스 생성 로직
                SpawnBoss();
            }
        }
    }

    // 적 스폰 로직
    private void SpawnEnemies()
    {
        if (enemyCenter.Length == 0)
            return;
        if (thisStageEnemy < stageInfos[gameManager.playerData.StageInfo].Waves.Length)
        {
            thisStageEnemy++;
            int randomEnemy = Random.Range(0, enemyCenter.Length);
            for (int i = 0; i <= 10; i++)
            {
                Instantiate(enemy, target);
            }
        }
        else if (thisStageEnemy == stageInfos[gameManager.playerData.StageInfo].Waves.Length)
        {
            StageClear();
        }
    }

    private void StageClear()
    {
        gameManager.playerData.StageInfo++;
        thisStageEnemy = 0;
        SpawnEnemies();
    }

    private void SpawnBoss()
    {

    }
}
