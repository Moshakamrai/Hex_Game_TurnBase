using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static ToonyColorsPro.ShaderGenerator.Enums;

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
    public bool possibleKillPlayer;

    public bool barrelExploded;
    public bool barrelExist;
    public bool obstacleExist;
    //public int cellToken;
    MeshRenderer thisMesh;

    

    public GameObject currentEnemyObject;
    public GameObject currentPlayerObject;
    public GameObject barrelObject;
    public GameObject obstableObject;
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
        //if (enemyExist && playerExist)
        //{
        //     Destroy(currentEnemyObject);
        //}
        if (enemyExist && playerExist)
        {
            currentEnemyObject.GetComponent<EnemyBrain>().TriggerStabbingAnimation();
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

    public void MesherPlayer()
    {
        thisMesh.enabled = true;
        thisMesh.material = InteractableMat;
        //canActionPlayer = true;
        possibleKillPlayer = true;
    }

    public void BarrelMesher()
    {
        thisMesh.enabled = true;
        thisMesh.material = InteractableMat;
        canAction = true;
        canMoveEnemy = false;
    }

    public void UnMesher()
    {
        thisMesh.enabled = false;
        thisMesh.material = null;
        //canAction = false;
        //possibleKill = false;
        // Debug.LogError("UNMESHER");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            enemyExist = false;
            possibleKill = false;
            //canAction = false;
            Destroy(collision.gameObject);
            Debug.LogError("Enemy died here");

        }
        else if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            currentEnemyObject = collision.gameObject;
            currentEnemyObject.transform.parent = transform.parent;
            //cellToken = collision.gameObject.GetComponent<EnemyBrain>().turnToken;
            possibleKill = true;
            enemyExist = true;

            //canAction = true;
        }
        else if (collision.gameObject.CompareTag("Barrel"))
        {
            barrelObject = collision.gameObject;
           // Debug.LogError("Bomb barrel is here");
            barrelExist = true;
            canAction = true;
            possibleKill = false;
            canWalk =false;
            canMoveEnemy = false;
            enemyExist = false;
            playerExist = false;
        }

        else if (collision.gameObject.CompareTag("Block"))
        {
            //Debug.LogError("Block is here");
            obstableObject = collision.gameObject;
            obstacleExist = true;
            canMoveEnemy = false;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyBrain>().death)
        {
            
            currentEnemyObject = collision.gameObject;
            
            enemyExist = true;       
           // possibleKill= false;
            onTriggerEnemy?.Invoke();
        } 
        if (collision.gameObject.CompareTag("Barrel"))
        {
            BarrelMesher();
            barrelObject = collision.gameObject;
           // Debug.LogError("Bomb barrel is here");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            currentPlayerObject = other.gameObject;
            possibleKillPlayer = true;
            playerExist = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            currentPlayerObject = other.gameObject;
            possibleKillPlayer = true;
            playerExist = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayerObject = null;
            playerExist = false;
            possibleKillPlayer = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            //cellToken = 0 ;
            currentEnemyObject = null;
            enemyExist = false;
            
            possibleKill = false;
           
        }
       
    }

    public void Onfire()
    {
        Debug.LogError("ON FIRE");

        // PlayerStateScript.Instance.ShootBarrelTrigger(barrelObject);
        

       if (currentEnemyObject != null)
       {
            canAction = false;
            
            canMoveEnemy = false;
            enemyExist = false;
           
            currentEnemyObject.GetComponent<EnemyBrain>().TriggerDeathAnimation();
       }
       if (currentPlayerObject != null)
       {
            playerExist = false;
            canWalk = false;
            PlayerStateScript.Instance.DeathAnimationTrigger();
       }

        canAction = false;
        canWalk = false;
        canMoveEnemy = false;
        enemyExist = false;
        playerExist = false;


    }

}
