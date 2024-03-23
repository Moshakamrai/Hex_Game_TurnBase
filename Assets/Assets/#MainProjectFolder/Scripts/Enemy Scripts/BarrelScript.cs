using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    public HexTerrain tileObjectScript;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HexTerrain>() != null)
        {
            tileObjectScript = collision.gameObject.GetComponent<HexTerrain>(); 
            Debug.LogError("Got the tile");
        }
    }

    private void OnMouseDown()
    {
        // This method will be called when the object is clicked or tapped
        // Add your desired functionality here
       
        //if (tileObjectScript.canAction == true && tileObjectScript.barrelExist == true && tileObjectScript != null)
        //{
        //    PlayerStateScript.Instance.ShootBarrelTrigger(gameObject);
        //    tileObjectScript.canAction = false;
        //    tileObjectScript.possibleKill = false;
        //}

    }

    public void TriggerDeathAnimation()
    {
        ParticleManager.Instance.PlayParticle("BarrelExplostion", transform.position, transform.rotation);
        Destroy(gameObject);
        Debug.LogError("particle effects");
    }
}
