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
    private Collider parentCollider;
    public bool canAction;

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

    public void UnMesher()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
       // Debug.LogError("UNMESHER");
        //canMove = false;
    }
}
