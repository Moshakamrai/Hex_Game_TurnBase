using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToonyColorsPro.ShaderGenerator.Enums;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class HexTerrain : MonoBehaviour
{
    public event Action OnMouseEnterAction;
    public event Action OnMouseExitAction;
    public event Action onTriggerEnemy;
    [SerializeField] private Material activeMatColor;
    [SerializeField] private Material InteractableMat;
    [SerializeField] private Material enemyPOVMat;
    private Collider parentCollider;
    public bool possibleKill;
    public bool canAction;
    public bool canWalk;
    public bool canMoveEnemy;
    public bool enemyExist;
    public bool playerExist;
    //public int cellToken;
    MeshRenderer thisMesh;

    public GameObject currentEnemyObject;
    public GameObject currentPlayerObject;
    private void Start()
    {
        parentCollider = GetComponent<Collider>(); 
        // Disable collisions between the parent collider and all child colliders
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider childCollider in childColliders)
        {
            childCollider.enabled = false;
        }
        parentCollider.enabled = true;
        thisMesh = GetComponent<MeshRenderer>();
        //cellToken = 1;
        enemyExist = false;
        playerExist = false;
    }
    private void Update()
    {
       //if (enemyExist)
       //{
       //     onTriggerEnemy?.Invoke();
       //}
       if (enemyExist && playerExist)
       {
            Destroy(currentEnemyObject);
       }
        
    }
    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        OnMouseEnterAction?.Invoke();
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        OnMouseExitAction?.Invoke();
    }

    public void Mesher()
    {
        thisMesh.enabled = true;
        thisMesh.material = activeMatColor;
        possibleKill = false;
    }
    public void EnemyViewMesher()
    {
        thisMesh.enabled = true;
        thisMesh.material = enemyPOVMat;
    }
    public void EnemyKillPlayerMesher()
    {
        thisMesh.enabled = true;
        thisMesh.material = InteractableMat;
    }
    public void MesherEnemy()
    {
        thisMesh.enabled = true;
        thisMesh.material = InteractableMat;
        canAction = true;
        possibleKill = true;
    }

    public void UnMesher()
    {
        thisMesh.enabled = false;
        thisMesh.material = null;
        canAction = false;
        possibleKill = false;
        // Debug.LogError("UNMESHER");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            enemyExist = false;
            canAction = false;
            Destroy(collision.gameObject);
            Debug.LogError("Enemy died here");
            
        }
        else if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            currentEnemyObject = collision.gameObject;
            //cellToken = collision.gameObject.GetComponent<EnemyBrain>().turnToken;
            enemyExist = true;
        }
        if (collision.gameObject.CompareTag("Barrel"))
        {
            Debug.LogError("Bomb barrel is here");
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            //cellToken = collision.gameObject.GetComponent<EnemyBrain>().turnToken;
            //Debug.LogError("enemyexist");
            currentEnemyObject = collision.gameObject;
            canAction = true;
            enemyExist = true;       
            onTriggerEnemy?.Invoke();
        } 
        if (collision.gameObject.CompareTag("Barrel"))
        {
            Debug.LogError("Bomb barrel is here");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            currentPlayerObject = other.gameObject;
            playerExist = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //cellToken = 0 ;
            currentEnemyObject = null;
            enemyExist = false;
            canAction = false;
            
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //cellToken = collision.gameObject.GetComponent<EnemyBrain>().turnToken;
            currentPlayerObject = null;
            playerExist = false;
            
        }
    }

}
