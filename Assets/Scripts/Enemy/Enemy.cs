using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour 
{
    [SerializeField] 
    private EnemyCenter enemyLife; // 적 체력

    private EnemyCenter MaxEnemyLife; // 적 최대 체력
    
    public Image HPBar; // 적 체력 UI 컴포넌트



    public EnemyCenter enemyCenter;

    // 적 현재 상태
    public void WatchEnemyInfo()
    {
        //Debug.Log("적 체력::" + EnemyCenter.MaxenemyLife);
    }

    // 마우스 왼쪽 클릭 시 적 체력 감소 구현
    public void Start()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("공격을 맞췄다!");
            if (enemyLife == null)
            {
                enemyLife = ScriptableObject.CreateInstance<EnemyCenter>();
            }

            enemyLife.EnemyLife -=  
                
            // HP 바 감소 구현 코드
           /* if (HPBar != null)
            {
                float healthPercentage = (float)enemyLife.EnemyLife / enemyLife.MaxEnemyLife;
                //HPBar.fillAmount = healthPercentage;
            }  */
        }
    }

    public void EnemyHP(int amount)
    {
        enemyLife -= PlayerData.Attack  * amount;
        
        if (enemyLife <= 0)
        {
            //isDie = true;
        }
    }
}
