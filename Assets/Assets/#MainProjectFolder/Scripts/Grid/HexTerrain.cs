using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class HexTerrain : MonoBehaviour
{
    public event Action OnMouseEnterAction;
    public event Action OnMouseExitAction;
    [SerializeField] private Material activeMatColor;
    [SerializeField] private Material InteractableMat;
    private Collider parentCollider;
    public bool canAction;
    public bool canWalk;

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
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().material = activeMatColor;
        
    }
    public void MesherEnemy()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().material = InteractableMat;
        canAction = true;
    }

    public void UnMesher()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().material = null;
        // Debug.LogError("UNMESHER");
        //canMove = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.LogError("Enemy is here");
        }
    }

}
