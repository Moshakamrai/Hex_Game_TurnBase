using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private bool turnState;
    [SerializeField] private Animator enemyAnim;
    public HexTerrain tileState;
    [SerializeField] private GameObject playerObject;
    public bool death;
    public int turnToken;
    public bool canKillPlayer;

    public ParticleSystem bloodExplode;

    [SerializeField] private Transform shootDirection;
    [SerializeField] private GameObject bulletPrefab;

    public bool gunner;
    private void Awake()
    {
        
    }
    private void Start()
    {
       
        //turnToken = 1;
        enemyAnim = GetComponent<Animator>();
        playerObject = PlayerStateScript.Instance.gameObject;
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
        death = true;
        //ResourceManager.Instance.GiveToken();
        Debug.Log("Mouse click or tap detected on " + gameObject.name);
        if (tileState.canAction == true && tileState.possibleKill == true)
        {
            PlayerStateScript.Instance.ShootAnimationTrigger(gameObject);
            tileState.canAction = false;
            tileState.possibleKill = false;
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

    private IEnumerator DeathDestroy()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    public void TriggerDeathAnimation()
    {
        death = true;
        tileState.possibleKill = false;
        tileState.enemyExist = false;
        ResourceManager.Instance.EnemyChecker();
        bloodExplode.Play();
        enemyAnim.SetTrigger("Death");
        StartCoroutine(DeathDestroy());
    }
    public void TriggerShootingAnimation()
    {
        enemyAnim.SetTrigger("Shooting");
        Debug.LogError("should shoot");
        //ResourceManager.Instance.EnemyChecker();
        //bloodExplode.Play();
        
    }

    public void BulletMovementTrigger()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootDirection.position, shootDirection.rotation);
        StartCoroutine(bulletTravel(bullet.transform, PlayerStateScript.Instance.gameObject.transform));
    }

    private IEnumerator bulletTravel(Transform bulletPosition, Transform targetPosition)
    {

        ParticleManager.Instance.PlayParticle("PlayerBullet", bulletPosition.position, bulletPosition.rotation, bulletPosition.transform);
        while (targetPosition != null)
        {
            // Move the bullet towards the target
            bulletPosition.position = Vector3.MoveTowards(bulletPosition.position, targetPosition.position, 300f * Time.deltaTime);

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
    public void TriggerMovementAnimation()
    {
        enemyAnim.SetTrigger("Jumping");
    }

    public void TriggerStabbingAnimation()
    {
        StartCoroutine(MoveCoroutine(PlayerStateScript.Instance.gameObject.transform.position, 0.8f));
        enemyAnim.SetTrigger("Slash");
    }


    public void TriggerPlayerDeath()
    {
        PlayerStateScript.Instance.DeathAnimationTrigger();
    }
    public void TakeTurn()
    {
        turnToken--;
    }


    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position
        transform.position = targetPosition;
    }
}
