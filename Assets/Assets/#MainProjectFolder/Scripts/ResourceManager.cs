using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<TerrainType> TerrainTypes = new List<TerrainType>();
    [SerializeField] private int tokenCount = 0;

    [SerializeField] private List<GameObject> enemyObjects = new List<GameObject>();
    [SerializeField] EnemyBrain enemyState;

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

        if (enemyObjects.Count > 0)
        {
            enemyObjects[tokenCount].GetComponent<EnemyBrain>().turnToken = 1;
        }
    }

    public void GiveToken()
    {
        PlayerStateScript.Instance.playerTurn = false;

        if (enemyObjects[tokenCount] != null)
        {
            enemyObjects[tokenCount].GetComponent<EnemyBrain>().turnToken = 1;
        }

        enemyState.turnToken--;
        tokenCount++;

        if (tokenCount >= enemyObjects.Count)
        {
            tokenCount = 0;
            PlayersTurn();
        }
    }

    public void PlayersTurn()
    {
        PlayerStateScript.Instance.playerTurn = true;
    }
}
