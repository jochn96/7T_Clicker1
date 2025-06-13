using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponButtonManager : MonoBehaviour
{
    public Button WOpenButton;
    public Button WCloseButton;
    public Button W1EquipButton;
    public Button W2EquipButton;
    public Button W3EquipButton;
    public Button W4EquipButton;
    public Button W2BuyButton;
    public Button W3BuyButton;
    public Button W4BuyButton;
    public Button W1UpgradeButton;
    public Button W2UpgradeButton;
    public Button W3UpgradeButton;
    public Button W4UpgradeButton;

    public GameObject WeaponInven;
    public GameObject W2Shadow;
    public GameObject W3Shadow;
    public GameObject W4Shadow;
    public GameObject W2Buy;
    public GameObject W3Buy;
    public GameObject W4Buy;
    public GameObject W2Equip;
    public GameObject W3Equip;
    public GameObject W4Equip;

    public TMP_Text W1EquipText;
    public TMP_Text W2EquipText;
    public TMP_Text W3EquipText;
    public TMP_Text W4EquipText;
    
    public Image W1EquipImage;
    public Image W2EquipImage;
    public Image W3EquipImage;
    public Image W4EquipImage;
    
    Color EquipColor = new Color(229f / 255f, 229f / 255f, 229f / 255f, 1f);
    Color UnEquipColor = new Color(125f / 255f, 255f / 255f, 80f / 255f, 1f);
    void Start()
    {
        WOpenButton.onClick.AddListener(VWOpen);
        WCloseButton.onClick.AddListener(VWClose);
        
        W1EquipButton.onClick.AddListener(VW1Equip);
        W2EquipButton.onClick.AddListener(VW2Equip);
        W3EquipButton.onClick.AddListener(VW3Equip);
        W4EquipButton.onClick.AddListener(VW4Equip);
        
        W2BuyButton.onClick.AddListener(VW2Buy);
        W3BuyButton.onClick.AddListener(VW3Buy);
        W4BuyButton.onClick.AddListener(VW4Buy);
        
        W1UpgradeButton.onClick.AddListener(VW1Upgrade);
        W2UpgradeButton.onClick.AddListener(VW2Upgrade);
        W3UpgradeButton.onClick.AddListener(VW3Upgrade);
        W4UpgradeButton.onClick.AddListener(VW4Upgrade);
    }

    void VWOpen()
    {
        WeaponInven.SetActive(true);
        Debug.Log("열기");
    }

    void VWClose()
    {
        WeaponInven.SetActive(false);
        Debug.Log("닫기");
    }

    void VW1Equip()
    {
        W1EquipText.text = "장착중";
        W2EquipText.text = "장착";
        W3EquipText.text = "장착";
        W4EquipText.text = "장착";
        W1EquipImage.color = EquipColor;
        W2EquipImage.color = UnEquipColor;
        W3EquipImage.color = UnEquipColor;
        W4EquipImage.color = UnEquipColor;
        Debug.Log("1장착");
    }
    void VW2Equip()
    {
        W1EquipText.text = "장착";
        W2EquipText.text = "장착중";
        W3EquipText.text = "장착";
        W4EquipText.text = "장착";
        W1EquipImage.color = UnEquipColor;
        W2EquipImage.color = EquipColor;
        W3EquipImage.color = UnEquipColor;
        W4EquipImage.color = UnEquipColor;
        Debug.Log("2장착");
    }
    void VW3Equip()
    {
        W1EquipText.text = "장착";
        W2EquipText.text = "장착";
        W3EquipText.text = "장착중";
        W4EquipText.text = "장착";
        W1EquipImage.color = UnEquipColor;
        W2EquipImage.color = UnEquipColor;
        W3EquipImage.color = EquipColor;
        W4EquipImage.color = UnEquipColor;
        Debug.Log("3장착");
    }
    void VW4Equip()
    {
        W1EquipText.text = "장착";
        W2EquipText.text = "장착";
        W3EquipText.text = "장착";
        W4EquipText.text = "장착중";
        W1EquipImage.color = UnEquipColor;
        W2EquipImage.color = UnEquipColor;
        W3EquipImage.color = UnEquipColor;
        W4EquipImage.color = EquipColor;
        Debug.Log("4장착");
    }
    
    void VW2Buy()
    {
        W2Equip.SetActive(true);
        W2Buy.SetActive(false);
        W2Shadow.SetActive(false);
        Debug.Log("2구매");
    }
    void VW3Buy()
    {
        W3Equip.SetActive(true);
        W3Buy.SetActive(false);
        W3Shadow.SetActive(false);
        Debug.Log("3구매");
    }
    void VW4Buy()
    {
        W4Equip.SetActive(true);
        W4Buy.SetActive(false);
        W4Shadow.SetActive(false);
        Debug.Log("4구매");
    }
    
    void VW1Upgrade()
    {
        Debug.Log("1업그레이드");
    }
    void VW2Upgrade()
    {
        Debug.Log("2업그레이드");
    }
    void VW3Upgrade()
    {
        Debug.Log("3업그레이드");
    }
    void VW4Upgrade()
    {
        Debug.Log("4업그레이드");
    }
    
    
}
