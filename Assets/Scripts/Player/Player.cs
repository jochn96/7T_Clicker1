using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 행동 및 능력치 사용을 담당합니다.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 데이터 (레벨, 능력치 등)
    /// </summary>
    public PlayerData playerData { get; private set; }

    /// <summary>
    /// 플레이어의 능력치 관리 컴포넌트
    /// </summary>
    private PlayerStat playerStat;

    
    private void Awake()
    {
        // 저장된 데이터 로드
        playerData = SaveDataToJSON.LoadUsers();
        
        // PlayerData가 null이면 새로 생성
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        
        // PlayerStat 컴포넌트 캐싱
        playerStat = GetComponent<PlayerStat>();   
    }
    
    private void Start()
    {
        // PlayerStat 컴포넌트에 PlayerData 초기화
        if (playerStat != null)
        {
            playerStat.InitWithPlayerData(playerData);
            // GameManager에 데이터 반영
            GameManager.Instance.playerDataLoad();
        }
    }
}
