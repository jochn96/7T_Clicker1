
[System.Serializable]
public  class  PlayerData //임시입니다 스텟레벨을 가져올예정
{
    public int Stage;
    public int Gold;
    public int Attack;
    public int Critical;
    public int CriticalDmg;
    public int BonusGold;

    public void RefreshData(PlayerData playerdata)  //임시코드입니다 실제로는 스텟 레벨을 가져올 예정
    {
        Stage = playerdata.Stage;
        Gold = playerdata.Gold;
        Attack = playerdata.Attack;
        Critical = playerdata.Critical;
        CriticalDmg = playerdata.Critical;
        BonusGold = playerdata.BonusGold;

        if (playerdata != null)
            SaveDataToJSON.SaveUsers(playerdata);
    }
}
