using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private bool turnState;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private HexTerrain tileState;
    [SerializeField] private GameObject playerObject;
    public bool death;
    public int turnToken;
    private void Awake()
    {
        
    }
    private void Start()
    {
       
        turnToken = 1;
        enemyAnim = GetComponent<Animator>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HexTerrain>() != null)
        {
            tileState = collision.gameObject.GetComponent<HexTerrain>();
        }
        
    }

    public void StartCoroutineExternally(IEnumerator coroutine)
    {
        TriggerMovementAnimation();
        StartCoroutine(coroutine);
    }

    private void OnMouseDown()
    {
        // This method will be called when the object is clicked or tapped
        // Add your desired functionality here
        Debug.Log("Mouse click or tap detected on " + gameObject.name);
        if (tileState.canAction == true)
        {
            TriggerDeathAnimation();
            tileState.canAction = false;
        }
        
    }
    private void Update()
    {
        // Ensure playerObject is not null and the enemy is alive
        if (playerObject != null && !death)
        {
            // Continuously rotate towards the player object
            transform.LookAt(playerObject.transform);
        }
    }

    public void TriggerDeathAnimation()
    {
        enemyAnim.SetTrigger("Death");
    }

    public void TriggerMovementAnimation()
    {
        enemyAnim.SetTrigger("Jumping");
    }

    public void TakeTurn()
    {
        turnToken--;
    }
}