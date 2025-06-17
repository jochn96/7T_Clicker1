using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponDisplayManager : MonoBehaviour
{
    public Weapon Weapon;
    public TMP_Text infoText;
    public TMP_Text BuyMaterialPriceText;
    public TMP_Text UpgradeMaterialPriceText;
    public TMP_Text EnforceStoneText;
    public PlayerData playerData;
    private void Update()
    {
        if (Weapon != null && infoText != null)
        {
            infoText.text =
                $"공격력: {Weapon.atk}\n" +
                $"치명타 확률: +{Weapon.criticalChance}%";
        }

        if (BuyMaterialPriceText != null)
        {
            BuyMaterialPriceText.text = Weapon.BuyPrice.ToString();
        }
        
        if (UpgradeMaterialPriceText != null)
        {
            switch (Weapon.WeaponLevel)
            {
                case 1:
                    UpgradeMaterialPriceText.text = Weapon.Lv1price.ToString();
                    break;
                case 2:
                    UpgradeMaterialPriceText.text = Weapon.Lv2price.ToString();
                    break;
                case 3:
                    UpgradeMaterialPriceText.text = Weapon.Lv3price.ToString();
                    break;
                case 4:
                    UpgradeMaterialPriceText.text = Weapon.Lv4price.ToString();
                    break;
                case 5:
                    UpgradeMaterialPriceText.text = "MAX!";
                    break;
                    
            }
            
        }

        if (EnforceStoneText != null)
        {
            EnforceStoneText.text = $"현재 강화석 수: {playerData.EnforceStone}";
        }
    }
}
