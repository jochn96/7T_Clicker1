using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clicker : MonoBehaviour
{
    public bool autoAttackUnlocked = false; //자동공격 구매전 비활성화
    public float autoAttackInterval = 5.0f; //자동공격 간격
    public Enemy targetEnemy;

    [Header("이펙트")]
    public GameObject nomalEffect;
    public GameObject criEffect;
    public Transform effectPivot;

    private Coroutine autoAttackRoutine;

    private Animator animator;
    private bool isAttack;
    private GameManager gameManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        //UnlockAutoClick();
    }

    //클릭시 공격
    public void OnClickClickerButton()
    {
        bool isCri = isCritical();
        int Damage = FinalDamage(isCri);
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(Damage); //몬스터 체력 - Damage;
        }
        //공격 이펙트 동작 그리고 크리티컬시 다른 이펙트 동작
        Debug.Log("클릭했습니다.");
    }

    public int FinalDamage(bool isCri)
    {//공격시 bool isCritical()을 실행시켜 (공격에서 임팩트를 주기위해서 이 함수가 필요) 크리티컬 여부판단
        if (isCri)//크리티컬이 발동되면
        {
            AttackAnimation();
            Effect(isCri);
            // 크리티컬 발동 시 finalAttack 사용 (이미 크리티컬 데미지가 적용된 값)
            return gameManager.finalAttack;
        }
        AttackAnimation();
        Effect(isCri);
        // 일반 공격 시 damage 사용 (기본 공격력 + 장비 공격력)
        return gameManager.damage;
    }
    public bool isCritical()
    {
        float CriticalRange = Random.Range(0f, 100f); //float값으로 랜덤을 돌려서
        Debug.Log($"{gameManager.finalCritical},{gameManager},{gameManager.damage},{CriticalRange}");
        if (CriticalRange <= gameManager.finalCritical) //나온숫자가 크리티컬 수치보다 작거나 같다면
        {
            return true;  //크리티컬 발동을위해 true반환
        }
        else
        {
            return false;  //아니라면 false반환
        }
    }

    private void AttackAnimation()
    {
        if (animator == null) return;

        if (isAttack)
        {
            animator.SetTrigger("Attack1"); //어택 1 애니메이션 재생
        }
        else
        {
            animator.SetTrigger("Attack2"); //어택 2 애니메인션 재생
        }

        isAttack = !isAttack; // 1, 2 바꾸면서 재생
    }

    private void Effect(bool isCri)
    {
        GameObject effectPrefab = isCri ? criEffect : nomalEffect;
        if (effectPrefab == null || effectPivot == null) return;

        //이펙트 소환위치
        GameObject spawnedEffect = Instantiate(effectPrefab, effectPivot.position, Quaternion.identity);

        //좌우반전
        if (!isAttack)
        {
            Vector3 scale = spawnedEffect.transform.localScale;
            scale.x *= -1;
            spawnedEffect.transform.localScale = scale;
        }

        //약간의 랜덤 Z 회전 (예: -45도 ~ +45도)
        float randomZ = Random.Range(-45f, 45f);
        spawnedEffect.transform.Rotate(0f, 0f, randomZ);

        //0.5초후 삭제
        Destroy(spawnedEffect, 0.5f);
    }

    //애니메이션 초기화
    public void ResetAttack()
    {
        isAttack = true;
    }

    //자동 공격 시작
    public void AutoClick()
    {
        if (autoAttackUnlocked && autoAttackRoutine == null)
        {
                autoAttackRoutine = StartCoroutine(AutoClickRoutine());
        }
    }

    //일정시간마다 클릭 코루틴
    IEnumerator AutoClickRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoAttackInterval);
            OnClickClickerButton();
        }
    }

    // 상점에서 구매시 자동클릭해금
    public void UnlockAutoClick()
    {
        if (!autoAttackUnlocked)
        {
            autoAttackUnlocked = true;
            AutoClick();
        }
    }
}