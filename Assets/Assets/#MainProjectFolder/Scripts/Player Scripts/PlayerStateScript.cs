using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateScript : MonoBehaviour
{
    private static PlayerStateScript instance;

    [SerializeField] private Animator playerAnim;
    [SerializeField] private GameObject playerContainer;
    public bool isShooting;
    public bool playerTurn;
    public bool firstTurn;

    public GameObject enemyTarget;
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
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Touch input
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                RotateTowards(targetPosition);
            }
        }
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

    public void TriggerEnemyDeathAnimation()
    {
        enemyTarget.GetComponent<EnemyBrain>().TriggerDeathAnimation();
    }
    public void StabAnimationTrigger()
    {  
        playerAnim.SetTrigger("Stabbing");
    }

}
