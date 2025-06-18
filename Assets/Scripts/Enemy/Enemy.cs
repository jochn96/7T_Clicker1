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

    // 적 현재 상태
    public void WatchEnemyInfo()
    {
        //Debug.Log("적 체력::" + EnemyCenter.MaxenemyLife);
    }

    // 마우스 왼쪽 클릭 시 적 체력 감소 구현
    private void Start()
    {
        currentHP = enemyCenter.MaxEnemyLife - enemyCenter.EnemyLife;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // OnClickClickerButton()을 호출하여 데미지를 받아옵니다
            clicker.OnClickClickerButton();
            int damage = clicker.FinalDamage(clicker.isCritical());
        
            // TakeDamage 메서드를 통해 데미지를 적용합니다
            TakeDamage(damage);
        
            Debug.Log($"공격을 맞췄다! 데미지: {damage}");
        }
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
            if (gameManager == null)
            {
                gameManager = GameManager.Instance;
            }
    
            // 리소스 획득
            if (gameManager != null)
            {
                gameManager.GetResource(10, 10);
            }
            else 
            {
                Debug.Log("GameManager가 없습니다!");
            }
    
            // 오브젝트 제거
            Destroy(gameObject);
        }
    }
