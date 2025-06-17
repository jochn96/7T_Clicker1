using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private EnemyCenter enemyLife;

    public EnemyCenter enemyCenter { set { enemyCenter = value; } }

    public void WatchEnemyInfo()
    {
        //Debug.Log("적 체력::" + EnemyCenter.EnemyLife);
    }
}
