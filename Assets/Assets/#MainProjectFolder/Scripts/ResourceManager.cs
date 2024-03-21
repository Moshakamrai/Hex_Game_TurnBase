using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<TerrainType> TerrainTypes = new List<TerrainType>();
    [SerializeField] private int tokenCount = 0;
    [SerializeField] private int enemyCount;
    [SerializeField] public int givenTurn;
    [SerializeField] private int enemyDead;
    [SerializeField] private List<GameObject> enemyObjects = new List<GameObject>();
    [SerializeField] EnemyBrain enemyState;
    [SerializeField] private HexGrid allCells;

    private void Start()
    {

        enemyDead = 0;
        StartCoroutine(SetEnemyObjects());
    }

    IEnumerator SetEnemyObjects()
    {
        yield return new WaitForSeconds(1.2f);
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in foundObjects)
        {
            enemyCount++;
            enemyObjects.Add(obj);
        }
        
        GiveToken();
    }

    public void GiveToken()
    {
        allCells.SetActiveCells();
        PlayerStateScript.Instance.playerTurn = false;
        givenTurn = enemyCount;
        for (int i = 0; i <= enemyCount; i++)
        {
            if (enemyObjects[i] != null)
            {
                givenTurn -= 1;
                //Debug.LogError("giving tokens" + i);
                enemyObjects[i].GetComponent<EnemyBrain>().turnToken = 1;
            }
            if (enemyObjects[i] == null)
            {
                givenTurn -= 1;
            }
        }
        
    }
    

    public void PlayersTurn()
    {
        //Debug.LogWarning("players turn but ");
        if (givenTurn == 0)
        {
            
            //Debug.LogWarning("enemy count is 0");
            PlayerStateScript.Instance.playerTurn = true;
        }
        
    }
}
