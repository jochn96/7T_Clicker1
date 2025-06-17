using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private EnemyCenter enemyCenter;
    
    public Image HPBar; // 적 체력 UI 컴포넌트
    
    private GameManager gameManager;

    private int currentHP;
    
    

    // 적 현재 상태
    public void WatchEnemyInfo()
    {
        //Debug.Log("적 체력::" + EnemyCenter.MaxenemyLife);
    }

    // 마우스 왼쪽 클릭 시 적 체력 감소 구현
    public void Start()
    {
        currentHP = enemyCenter.MaxEnemyLife;
        gameManager = GameManager.Instance;
        if (Input.GetMouseButton(0))
        {
           // currentHP - Damage;
            Debug.Log("공격을 맞췄다!");
            
            // HP 바 감소 구현 코드
           /* if (HPBar != null)
            {
                float healthPercentage = (float)enemyLife.EnemyLife / enemyLife.MaxEnemyLife;
                //HPBar.fillAmount = healthPercentage;
            }  */
        }
    }

}

   /* public void EnemyHP(int amount)
    {
        currentHP -= amount;
        
        if (currentHP <= 0)
        {
            // isDie = true;
        }
    }

    public void isDie()
    {
        // 몬스터 죽었을 때 얻는 골드와 강화석 값
         gameManager.GetResource(10,10);
        
        Destroy(gameObject);
    } */

