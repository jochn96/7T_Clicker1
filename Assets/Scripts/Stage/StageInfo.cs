using UnityEngine;

public class StageInfo : MonoBehaviour
{
    public int StageKey;
    public WaveData[] Waves;

    public StageInfo(int stageKey, WaveData[] waves)
    {
        this.StageKey = stageKey;
        this.Waves = waves;
    }
}

[System.Serializable] 
public class WaveData
{
    public EnemySpawnData[] enemies;
    public bool hasboss;
    public string bosstype;

    public WaveData(EnemySpawnData[] enemies, bool hasboss, string bosstype)
    {
        this.enemies = enemies;
        this.hasboss = hasboss;
        this.bosstype = bosstype;
    }
}

[System.Serializable]
public class EnemySpawnData
{
    public string enemyType;
    public int spawnCount;

    public EnemySpawnData(string enemyType, int spawnCount)
    {
        this.enemyType = enemyType;
        this.spawnCount = spawnCount;
    }
}

public static class StageData
{
    public static readonly StageInfo[] Stage = new StageInfo[10];
    
}
