using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    public Weapon Weapon;
    public WeaponButtonManager weaponButtonManager;

    public void Upgrade()
    {
        switch (Weapon.WeaponLevel)
        {
            case 1:
                if (weaponButtonManager.TestUpgradeCoin >= Weapon.Lv1price)
                {
                    weaponButtonManager.TestUpgradeCoin -= Weapon.Lv1price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv2atk;
                    Weapon.criticalChance = Weapon.Lv2CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv2price;
                }
                break;
            case 2:
                if (weaponButtonManager.TestUpgradeCoin >= Weapon.Lv2price)
                {
                    weaponButtonManager.TestUpgradeCoin -= Weapon.Lv2price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv3atk;
                    Weapon.criticalChance = Weapon.Lv3CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv3price;
                }
                break;
            case 3:
                if (weaponButtonManager.TestUpgradeCoin >= Weapon.Lv3price)
                {
                    weaponButtonManager.TestUpgradeCoin -= Weapon.Lv3price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv4atk;
                    Weapon.criticalChance = Weapon.Lv4CriticalChance;
                    Weapon.UpgradePrice = Weapon.Lv4price;
                }
                break;
            case 4:
                if (weaponButtonManager.TestUpgradeCoin >= Weapon.Lv4price)
                {
                    weaponButtonManager.TestUpgradeCoin -= Weapon.Lv4price;
                    Weapon.WeaponLevel++;
                    Weapon.atk = Weapon.Lv5atk;
                    Weapon.criticalChance = Weapon.Lv5CriticalChance;
                }
                break;
            case 5:
                //최고레벨 UI 추가
                break;
            
        }
    }
}
