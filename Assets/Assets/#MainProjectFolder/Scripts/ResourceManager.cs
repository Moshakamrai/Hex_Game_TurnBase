using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<TerrainType> TerrainTypes = new List<TerrainType>();
    [SerializeField] private int tokenCount = 0;

    [SerializeField] private List<GameObject> enemyObjects = new List<GameObject>();
    [SerializeField] EnemyBrain enemyState;
    [SerializeField] private HexGrid allCells;

    private void Start()
    {
        
        StartCoroutine(SetEnemyObjects());
        PlayerStateScript.Instance.playerTurn = true;
    }

    IEnumerator SetEnemyObjects()
    {
        yield return new WaitForSeconds(1.2f);
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in foundObjects)
        {
            enemyObjects.Add(obj);
        }
        GiveToken();
        
    }

    public void GiveToken()
    {
        allCells.SetActiveCells();
        PlayerStateScript.Instance.playerTurn = false;

        for(int i = 0; i <= enemyObjects.Count; i++)
        {
            if (enemyObjects[i] != null)
            {
                //Debug.LogError("giving tokens" + i);
                enemyObjects[i].GetComponent<EnemyBrain>().turnToken = 1;
            }
            
        }
        
    }
    

    public void PlayersTurn()
    {
        PlayerStateScript.Instance.playerTurn = true;
    }
}
