using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/Weapon")]
public class Weapon : ScriptableObject
{ 
    public int WeaponLevel;
    public int atk;
    public float criticalChance;
    public int Lv1atk; 
    public int Lv2atk;
    public int Lv3atk;
    public int Lv4atk;
    public int Lv5atk;
    public float Lv1CriticalChance;
    public float Lv2CriticalChance;
    public float Lv3CriticalChance;
    public float Lv4CriticalChance;
    public float Lv5CriticalChance;
    public int BuyPrice;
    public int UpgradePrice;
    public int Lv1price;
    public int Lv2price;
    public int Lv3price;
    public int Lv4price;
}
