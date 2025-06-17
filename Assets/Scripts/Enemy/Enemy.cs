using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private EnemyCenter enemyLife; // 적 체력

    private EnemyCenter maxenemyLife; // 적 최대 체력
    
    public Image HPBar; // 적 체력 UI 컴포넌트
    
    

    public EnemyCenter enemyCenter { set { enemyCenter = value; } }

    // 적 현재 상태
    public void WatchEnemyInfo()
    {
        //Debug.Log("적 체력::" + EnemyCenter.MaxenemyLife);
    }

    // 마우스 왼쪽 클릭 시 적 체력 감소 구현
    void Start()
    {
        if (Input.GetMouseButton(0))
        {
            //EnemyCenter.CreateInstance<int>(maxEnemyLife) - Player.Attack = enemyLife;
        }
    }
}
