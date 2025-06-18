using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Resources 폴더에서 UI_Player_Stat 프리팹을 로드하고 인스턴스화
        GameObject playerStatUIPrefab = Resources.Load<GameObject>("UI/UI_Player_Canvas");
        if (playerStatUIPrefab != null)
        {
            Instantiate(playerStatUIPrefab);
        }
        else
        {
            Debug.LogError("UI_Player_Stat 프리팹을 찾을 수 없습니다. Resources/UI/ 경로를 확인하세요.");
        }
    }

  
}
