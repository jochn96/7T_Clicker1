using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    public PlayerData playerData;
    public Weapon Weapon;

    public void Upgrade()
    {
        var playerData = GameManager.Instance.playerData;

        switch (Weapon.WeaponLevel)
        {
            case 1:
                if (playerData.EnforceStone >= Weapon.Lv1price)
                {
                    playerData.EnforceStone -= Weapon.Lv1price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv2atk;
                    Weapon.criticalChance = Weapon.Lv2CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv2price;
                    Debug.Log("무기1 업그레이드");
                }
                break;
            case 2:
                if (playerData.EnforceStone >= Weapon.Lv2price)
                {
                    playerData.EnforceStone -= Weapon.Lv2price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv3atk;
                    Weapon.criticalChance = Weapon.Lv3CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv3price;
                    Debug.Log("무기2 업그레이드");
                }
                break;
            case 3:
                if (playerData.EnforceStone >= Weapon.Lv3price)
                {
                    playerData.EnforceStone -= Weapon.Lv3price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv4atk;
                    Weapon.criticalChance = Weapon.Lv4CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv4price;
                    Debug.Log("무기3 업그레이드");
                }
                break;
            case 4:
                if (playerData.EnforceStone >= Weapon.Lv4price)
                {
                    playerData.EnforceStone -= Weapon.Lv4price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv5atk;
                    Weapon.criticalChance = Weapon.Lv5CriticalChance;
                    Debug.Log("무기4 업그레이드");
                }
                break;
            case 5:
                //최고레벨 UI 추가
                break;
            
        }
    }
}
