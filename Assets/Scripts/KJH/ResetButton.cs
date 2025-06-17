using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResetButton : MonoBehaviour
{
    public Button resetButton;
    public Weapon Weapon1;
    public Weapon Weapon2;
    public Weapon Weapon3;
    public Weapon Weapon4;
    
    private void Start()
    {
        resetButton.onClick.AddListener(reset);
    }

    void reset()
    {
        Weapon1.WeaponLevel = 1;
        Weapon2.WeaponLevel = 1;
        Weapon3.WeaponLevel = 1;
        Weapon4.WeaponLevel = 1;
    }
}
