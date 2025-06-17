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
    public int Attack = 10;
    public float Critical = 20;
    public int CriticalDmg = 150;
    public int BonusGold = 0;
    public float AutoAttackCooldown = 5f;

    [Header("Resource")]
    public int Gold;
    public int EnforceStone;

    [Header("Auto Attack")]
    public bool IsAutoAttackUnlocked = false;

    [Header("Equiment")]
    EquimentData equimentList = new EquimentData();

    public void RefreshData(PlayerData playerdata)  //임시코드입니다 실제로는 스텟 레벨을 가져올 예정
    {
        Debug.Log($"[RefreshData 내부 시작] 현재: Attack={Attack}, Critical={Critical}, CriticalDmg={CriticalDmg}, BonusGold={BonusGold}, AutoAttackCooldown={AutoAttackCooldown}");
        Debug.Log($"[RefreshData 내부 파라미터] 전달받은 값: Attack={playerdata.Attack}, Critical={playerdata.Critical}, CriticalDmg={playerdata.CriticalDmg}, BonusGold={playerdata.BonusGold}, AutoAttackCooldown={playerdata.AutoAttackCooldown}");
        
        SaveDataToJSON.LoadUsers();

        Stage = playerdata.Stage;
        Gold = playerdata.Gold;
        Attack = playerdata.Attack;
        Critical = playerdata.Critical;
        CriticalDmg = playerdata.CriticalDmg;
        BonusGold = playerdata.BonusGold;
        AutoAttackCooldown = playerdata.AutoAttackCooldown;
        IsAutoAttackUnlocked = playerdata.IsAutoAttackUnlocked;


        //statLevels[PlayerStatType.Attack] = playerdata.Attack;
        //statLevels[PlayerStatType.Critical] = playerdata.Critical;
        //statLevels[PlayerStatType.CriticalDmg] = playerdata.CriticalDmg;
        //statLevels[PlayerStatType.BonusGold] = playerdata.BonusGold;
        
        Debug.Log($"[RefreshData 내부 완료] 변경 후: Attack={Attack}, Critical={Critical}, CriticalDmg={CriticalDmg}, BonusGold={BonusGold}, AutoAttackCooldown={AutoAttackCooldown}");
        
        if (playerdata != null)
            SaveDataToJSON.SaveUsers(playerdata);
    }
}
