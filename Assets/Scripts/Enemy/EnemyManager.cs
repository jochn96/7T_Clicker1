using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   [SerializeField] private List<GameObject> enemyPrefab;
   [SerializeField] private List<GameObject> sqwnMonster;
   
   GameManager gameManager;
   private bool EnemySpawn;

   public void Init(GameManager gameManager)
   {
      this.gameManager = gameManager;
      
      enemyPrefab = new List<GameObject>();
      foreach (var VARIABLE in enemyPrefab)
      {
        
      }
   }
   private void enemySpawn(string prefabName)
   {
      // GameObject enemySqawn = Instantiate(enemyPrefabs[0], transform.position, Quaternion.identity);
     /* if (enemyPrefabs.Count == 0)
      {
         Debug.Log("적이 처치되었습니다.");
         return;
      }

      GameObject enemyPrefab;
      if (prefabName == null)
      {
         enemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Count)];
         
      } */
      
   }
}
