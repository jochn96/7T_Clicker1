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
    public int StageInfo;

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
              
        SaveDataToJSON.LoadUsers();

        StageInfo = playerdata.StageInfo;

        SaveDataToJSON.LoadUsers();
        
        Gold = playerdata.Gold;
        EnforceStone = playerdata.EnforceStone;
        Attack = playerdata.Attack;
        Critical = playerdata.Critical;
        CriticalDmg = playerdata.CriticalDmg;
        BonusGold = playerdata.BonusGold;
        AutoAttackCooldown = playerdata.AutoAttackCooldown;
        IsAutoAttackUnlocked = playerdata.IsAutoAttackUnlocked;
        
        if (playerdata != null)
            SaveDataToJSON.SaveUsers(playerdata);
    }
}
