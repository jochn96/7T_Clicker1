using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 사운드 바 메뉴를 관리하는 클래스입니다.
/// </summary>
public class SoundBarMenu : MonoBehaviour
{
    [Header("메뉴 설정")]
    public GameObject soundBarMenuPanel; // 사운드 바 메뉴 패널
    
    [Header("버튼 설정")]
    public Button soundButton; // 사운드 버튼
    public Button quitButton; // 종료 버튼
    
    private void Start()
    {
        // 초기 상태에서 메뉴 숨기기
        if (soundBarMenuPanel != null)
        {
            soundBarMenuPanel.SetActive(false);
        }
        
        // 버튼 이벤트 등록
        if (soundButton != null)
        {
            soundButton.onClick.AddListener(ShowSoundBarMenu);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(HideSoundBarMenu);
        }
    }
    
    /// <summary>
    /// 사운드 바 메뉴를 표시합니다.
    /// </summary>
    public void ShowSoundBarMenu()
    {
        if (soundBarMenuPanel != null)
        {
            soundBarMenuPanel.SetActive(true);
        }
    }
    
    /// <summary>
    /// 사운드 바 메뉴를 숨깁니다.
    /// </summary>
    public void HideSoundBarMenu()
    {
        if (soundBarMenuPanel != null)
        {
            soundBarMenuPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 사운드 바 메뉴의 표시 상태를 토글합니다.
    /// </summary>
    public void ToggleSoundBarMenu()
    {
        if (soundBarMenuPanel != null)
        {
            soundBarMenuPanel.SetActive(!soundBarMenuPanel.activeSelf);
        }
    }
} 