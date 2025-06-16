using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clicker : MonoBehaviour
{
    public bool autoAttackUnlocked = false; //자동공격 구매전 비활성화
    public float autoAttackInterval = 5.0f; //자동공격 간격

    [Header("이펙트")]
    public GameObject nomalEffect;
    public GameObject criEffect;
    public Transform effectPivot;

    private Coroutine autoAttackRoutine;

    private Animator animator;
    private bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //UnlockAutoClick();
    }

    //클릭시 공격
    public void OnClickClickerButton()
    {
        bool isCri = GameManager.Instance.isCritical();
        int finalDamage = GameManager.Instance.FinalAttack(isCri);
        //몬스터 체력 - 플레이어 최종데미지
        AttackAnimation();
        Effect(isCri);
        //공격 이펙트 동작 그리고 크리티컬시 다른 이펙트 동작
        Debug.Log("클릭했습니다.");
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