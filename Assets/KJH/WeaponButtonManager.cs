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
        
        W1EquipButton.onClick.AddListener(() => EquipWeapon(0));
        W2EquipButton.onClick.AddListener(() => EquipWeapon(1));
        W3EquipButton.onClick.AddListener(() => EquipWeapon(2));
        W4EquipButton.onClick.AddListener(() => EquipWeapon(3));
        
        W2BuyButton.onClick.AddListener(VW2Buy);
        W3BuyButton.onClick.AddListener(VW3Buy);
        W4BuyButton.onClick.AddListener(VW4Buy);
        
        W1UpgradeButton.onClick.AddListener(VW1Upgrade);
        W2UpgradeButton.onClick.AddListener(VW2Upgrade);
        W3UpgradeButton.onClick.AddListener(VW3Upgrade);
        W4UpgradeButton.onClick.AddListener(VW4Upgrade);
    }

    void VWOpen() //창 열기 코드
    {
        WeaponInven.SetActive(true);
        Debug.Log("열기");
    }

    void VWClose() //창 닫기 코드
    {
        WeaponInven.SetActive(false);
        Debug.Log("닫기");
    }

    void EquipWeapon(int index) //장착 버튼 UI 코드
    {
        TMP_Text[] texts = { W1EquipText, W2EquipText, W3EquipText, W4EquipText };
        Image[] images = { W1EquipImage, W2EquipImage, W3EquipImage, W4EquipImage };

        for (int i = 0; i < 4; i++)
        {
            texts[i].text = (i == index) ? "장착중" : "장착";
            images[i].color = (i == index) ? EquipColor : UnEquipColor;
        }

        Debug.Log($"{index + 1}장착");
    }
    
    void VW2Buy() //2번무기 구매 코드
    {
        W2Equip.SetActive(true);
        W2Buy.SetActive(false);
        W2Shadow.SetActive(false);
        Debug.Log("2구매");
    }
    void VW3Buy() //3번무기 구매 코드
    {
        W3Equip.SetActive(true);
        W3Buy.SetActive(false);
        W3Shadow.SetActive(false);
        Debug.Log("3구매");
    }
    void VW4Buy() //4번무기 구매 코드
    {
        W4Equip.SetActive(true);
        W4Buy.SetActive(false);
        W4Shadow.SetActive(false);
        Debug.Log("4구매");
    }
    
    void VW1Upgrade() //1번무기 업그레이드 코드
    {
        Debug.Log("1업그레이드");
    }
    void VW2Upgrade() //2번무기 업그레이드 코드
    {
        Debug.Log("2업그레이드");
    }
    void VW3Upgrade() //3번무기 업그레이드 코드
    {
        Debug.Log("3업그레이드");
    }
    void VW4Upgrade() //4번무기 업그레이드 코드
    {
        Debug.Log("4업그레이드");
    }
    
    
}
