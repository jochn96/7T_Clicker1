using UnityEngine;
using UnityEngine.UI;

public class ScrollTitle : MonoBehaviour
{
    [SerializeField] private RawImage backgroundImage;
    [SerializeField] private float scrollSpeedY = 0.1f;

    void Update()
    {
        // uvRect의 y값을 증가시켜 위로 스크롤
        Rect uvRect = backgroundImage.uvRect;
        uvRect.y += scrollSpeedY * Time.deltaTime;
        backgroundImage.uvRect = uvRect;
    }
}