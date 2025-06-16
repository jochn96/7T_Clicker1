using UnityEngine;

[System.Serializable]
public class EquimentData
{
    public Sprite itemImg;
    public string EquimentName;
    public string EquimentLevel;
}

[System.Serializable]
public  class  PlayerData //임시입니다 스텟레벨을 가져올예정
{
    public int Stage;

    [Header("StatLevel")]
    public int Attack;
    public int Critical;
    public int CriticalDmg;
    public int BonusGold;

    [Header("Resource")]
    public int Gold;
    public int EnforceStone;

    [Header("Equiment")]
    EquimentData equimentList = new EquimentData();

    public void RefreshData(PlayerData playerdata)  //임시코드입니다 실제로는 스텟 레벨을 가져올 예정
    {
        SaveDataToJSON.LoadUsers();

        int stageIndex = playerdata.Stage;
        int stageKey = -1;

        if (stageIndex >= 0 && stageIndex < StageData.Stage.Length && StageData.Stage[stageIndex] != null)
        {
            stageKey = StageData.Stage[stageIndex].StageKey;
        }

        Stage = playerdata.Stage;
        Gold = playerdata.Gold;
        Attack = playerdata.Attack;
        Critical = playerdata.Critical;
        CriticalDmg = playerdata.CriticalDmg;
        BonusGold = playerdata.BonusGold;

        //statLevels[PlayerStatType.Attack] = playerdata.Attack;
        //statLevels[PlayerStatType.Critical] = playerdata.Critical;
        //statLevels[PlayerStatType.CriticalDmg] = playerdata.CriticalDmg;
        //statLevels[PlayerStatType.BonusGold] = playerdata.BonusGold;
        
        if (playerdata != null)
            SaveDataToJSON.SaveUsers(playerdata);
    }
    
}
