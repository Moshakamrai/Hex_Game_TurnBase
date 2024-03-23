using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerStateScript : MonoBehaviour
{
    private static PlayerStateScript instance;

    [SerializeField] private Animator playerAnim;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private Transform shootDirection;
    [SerializeField] private GameObject bulletPrefab;
    public bool isShooting;
    public bool playerTurn;
    public bool firstTurn;

    public event Action onSelectAction;

    public GameObject enemyTarget;
    public GameObject environmentObject;
    //public bool playerRotation;

    public static PlayerStateScript Instance
    {
        
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStateScript>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PlayerStateScript).Name);
                    instance = singletonObject.AddComponent<PlayerStateScript>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        playerTurn = true;
        firstTurn = true;
    }
    void Update()
    {
        HandleRotationInput();
    }

    public void OnSelectTap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.LogError("Single tap - Select");
            onSelectAction?.Invoke();
        }

        else if (context.canceled)
        {
            
        }
    }

   
    void HandleRotationInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                RotateTowards(targetPosition);
            }
        }
       // && Input.GetTouch(0).phase == TouchPhase.Began
        //else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Touch input
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Vector3 targetPosition = hit.point;
        //        RotateTowards(targetPosition);
        //        //CameraController.Instance.onSelectAction?.Invoke();
        //        onSelectAction?.Invoke();
        //    }
        //}
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; // Ensure the rotation is in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = rotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            Debug.LogError("Player is here");
        }
    }

    public void IdleAnimationTrigger()
    {
        
        playerAnim.SetTrigger("Idle");
    }
    public void DeathAnimationTrigger()
    {

        playerAnim.SetTrigger("Death");
    }
    public void WalkAnimationTrigger()
    {
        
        playerAnim.SetTrigger("Walking");
    }

    public void ShootAnimationTrigger(GameObject target)
    {
        enemyTarget = target;
        playerAnim.SetTrigger("Shooting");  
    }

    public void ShootBarrelTrigger(GameObject barrel)
    {
        //enemyTarget = 
        enemyTarget = barrel;
        playerAnim.SetTrigger("Shooting2");
    }

    public void TriggerBarrelExplotion()
    {
        enemyTarget.GetComponent<BarrelScript>().TriggerDeathAnimation();
    }

    public void TriggerEnemyDeathAnimation()
    {
        enemyTarget.GetComponent<EnemyBrain>().TriggerDeathAnimation();
    }
    public void StabAnimationTrigger()
    {  
        playerAnim.SetTrigger("Stabbing");
    }

    public void BulletMovementTrigger()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootDirection.position, shootDirection.rotation);
        StartCoroutine(bulletTravel(bullet.transform, enemyTarget.transform));
    }

    private IEnumerator bulletTravel(Transform bulletPosition, Transform targetPosition)
    {

        ParticleManager.Instance.PlayParticle("PlayerBullet", bulletPosition.position, bulletPosition.rotation, bulletPosition.transform);
        while (targetPosition != null)
        {
            // Move the bullet towards the target
            bulletPosition.position = Vector3.MoveTowards(bulletPosition.position, targetPosition.position, 250f * Time.deltaTime);
            
            // Check if the bullet has reached the target
            if (transform.position == targetPosition.position)
            {
                // If the bullet has reached the target, destroy it
               
                
                // You can add damage logic or other effects here
                yield break; // Exit the coroutine
            }

            yield return null; // Wait for the next frame
        }
    }

    public void GiveTheTokens()
    {
        ResourceManager.Instance.GiveToken();
    }

}
