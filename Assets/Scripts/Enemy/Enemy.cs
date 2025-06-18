using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;



public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private EnemyCenter enemyCenter;
    
    public Image HPBar; // 적 체력 UI 컴포넌트
    
    GameManager gameManager;

    [SerializeField]
    private int currentHP;
    
    public Clicker clicker;
    public Transform HitEffectPivot;
    
    // 마우스 왼쪽 클릭 시 적 체력 감소 구현
    private void Start()
    {
        currentHP = enemyCenter.MaxEnemyLife;
        gameManager = GameManager.Instance;
    }
    

    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);

        // HP 바 업데이트
        if (HPBar != null)
        {
            float healthPercentage = (float)currentHP / enemyCenter.MaxEnemyLife;
            HPBar.fillAmount = healthPercentage;
        }

        // HP가 0 이하면 사망 처리
        if (currentHP <= 0)
        {
            Die();
        }
    }

        
        
    private void Die()  // Die 처리문
        {
            Debug.Log("죽었습니다.");
            if (gameManager == null)
            {
                gameManager = GameManager.Instance;
            }
    
            // 리소스 획득
            if (gameManager != null)
            {
                gameManager.GetResource(100, 100);
            }
            else 
            {
                Debug.Log("GameManager가 없습니다!");
            }
    
            // 오브젝트 제거
            Destroy(gameObject);
        }
    }
