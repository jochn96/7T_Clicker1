using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCenter" , menuName = "ScriptableObject/Enemy", order = 1)]

public class EnemyCenter : ScriptableObject
{
    [Header("적 기초 체력")] 
    [Tooltip("값을 수정하여 적의 기초 체력량을 바꾼다")] 
    [SerializeField]

    private int enemyLife = 5;
    public int EnemyLife { get { return enemyLife; } }
   
    [Header("적 최대 체력")]
    [Tooltip("값을 수정하여 적의 최대 체력량을 바꾼다")]
    [SerializeField]
   
    private int maxenemyLife = 30;
    public int MaxenemyLife { get { return maxenemyLife; } }

    [Header("적 경험치")] 
    [Tooltip("적을 쓰러뜨릴 때 얻는 경험치")] 
    [SerializeField]

    private int enemyExp;
    public int EnemyExp { get { return enemyExp; } }

    [Header("적 종류")] 
    [Tooltip("적 종류의 갯수를 수정한다")] 
    [SerializeField]

    private int enenmyType = 10;
    public int EnenmyType { get { return enenmyType; } }

    [Header("스테이지 종류")] 
    [Tooltip("스테이지의 총 종류를 수정한다")] 
    [SerializeField]

    private int stageCount = 10;
    public int StageCount { get { return stageCount; } }
}
