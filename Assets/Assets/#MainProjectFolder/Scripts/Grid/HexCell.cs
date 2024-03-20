using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class HexCell
{
    [Header("Cell Properties")]
    [SerializeField] private HexOrientation orientation;
    [field: SerializeField] public HexGrid Grid { get; set; }
    [field: SerializeField] public float HexSize { get; set; }
    [field: SerializeField] public TerrainType TerrainType { get; private set; }
    [field: SerializeField] public Vector2 OffsetCoordinates { get; set; }
    [field: SerializeField] public Vector3 CubeCoordinates { get; private set; }
    [field: SerializeField] public Vector2 AxialCoordinates { get; private set; }
    [field: NonSerialized] public List<HexCell> Neighbours { get; private set; }
    [field: NonSerialized] public List<HexCell> _AttackCells { get; private set; }

    [field: NonSerialized] public List<HexCell> _EnemyAttackCells { get; private set; }
    public static HexCell playerPosCell;
    [SerializeField]
    private CellState cellState;
    private ICellState state;
    public ICellState State
    {
        get { return state; }
        private set
        {
            state = value;
            cellState = state.State;
        }
    }

    //private PlayerInput playerInput;

    private Transform terrain;
    public Transform Terrain { get { return terrain; } }

    public void InitializeState(ICellState initalState = null)
    {
        if (initalState == null)
            ChangeState(new VisibleState());
        else
            ChangeState(initalState);
        
    }

    public IEnumerator MoveCameraToCell(HexCell cell)
    {
        
        playerPosCell = cell;
        Debug.LogError("Hello camera should move");
        PlayerStateScript.Instance.WalkAnimationTrigger();
        Transform cameraTarget = PlayerStateScript.Instance.gameObject.transform;
        Vector3 start = cameraTarget.position;
        Vector3 end = cell.Terrain.transform.position;

        // Adjust the end position to move up by 5 units in the Y axis
        end.y += 15f;

        float duration = 0.8f; // Adjust the duration as needed
        float elapsed = 0f;
        while (elapsed < duration)
        {
            end.y -= 0.025f;
            cameraTarget.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
            
        }
        end.y -= 10f; 

        cameraTarget.position = end; // Ensure the final position is accurate
        
        PlayerStateScript.Instance.IdleAnimationTrigger();
        
        ResourceManager.Instance.GiveToken();
        
        
    }

    public IEnumerator MoveToCell(Transform targetObject, HexCell cell)
    {
        
        Vector3 start = targetObject.position;
        Vector3 end = cell.Terrain.transform.position;
        cell.terrain.GetComponentInChildren<HexTerrain>().enemyExist = true;
        // Adjust the end position to move up by 5 units in the Y axis
        end.y += 5f;

        float duration = 0.8f; // Adjust the duration as needed
        float elapsed = 0f;
        while (elapsed < duration)
        {
            end.y -= 0.025f;
            targetObject.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        end.y -= 5f; // Lower the end position back down

     
        targetObject.position = end;
        targetObject.transform.SetParent(cell.Terrain.gameObject.transform);
        ResourceManager.Instance.PlayersTurn();

        Debug.LogError("after enemy Moved it should call active function now");

        Grid.SetSelectPlayerCell();

        //SelectedState.dontMove = false;

        //Grid.SetActiveCells();
        //OnActiveState.storedEnemyCell.SetNeighbours(OnActiveState.storedEnemyCell.Neighbours);
        //foreach (HexCell neighbour in OnActiveState.storedEnemyCell.Neighbours)
        //{
        //    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
        //}

    }




    public void SetCoordinates(Vector2 offsetCoordinates, HexOrientation orientation)
    {
        this.orientation = orientation;
        OffsetCoordinates = offsetCoordinates;
        CubeCoordinates = HexMetrics.OffsetToCube(offsetCoordinates, orientation);
        AxialCoordinates = HexMetrics.CubeToAxial(CubeCoordinates);
    }
    

    public void SetTerrainType(TerrainType terrainType)
    {
        TerrainType = terrainType;
    }

    public void CreateTerrain()
    {
        if (TerrainType == null)
        {
            Debug.LogError("TerrainType is null");
            return;
        }
        if (Grid == null)
        {
            Debug.LogError("Grid is null");
            return;
        }
        if (HexSize == 0)
        {
            Debug.LogError("HexSize is 0");
            return;
        }
        if (TerrainType.Prefab == null)
        {
            Debug.LogError("TerrainType Prefab is null");
            return;
        }

        Vector3 centrePosition = HexMetrics.Center(
            HexSize,
            (int)OffsetCoordinates.x,
            (int)OffsetCoordinates.y, orientation
            ) + Grid.transform.position;

        terrain = UnityEngine.Object.Instantiate(
            TerrainType.Prefab,
            centrePosition,
            Quaternion.identity,
            Grid.transform
            );
        terrain.gameObject.layer = LayerMask.NameToLayer("Grid");

        //TODO: Adjust the size of the prefab to the size of the grid cell

        if (orientation == HexOrientation.FlatTop)
        {
            terrain.Rotate(new Vector3(0, 30, 0));
        }
        //Temporary random rotation to make the terrain look more natural
        int randomRotation = UnityEngine.Random.Range(0, 6);
        terrain.Rotate(new Vector3(0, randomRotation * 60, 0));
        HexTerrain hexTerrrain = terrain.GetComponentInChildren<HexTerrain>();
        hexTerrrain.OnMouseEnterAction += OnMouseEnter;
        hexTerrrain.OnMouseExitAction += OnMouseExit;
        hexTerrrain.onTriggerEnemy += OnActive;
        terrain.gameObject.SetActive(false);
    }
    //void Update()
    //{
    //    HexTerrain hexTerrrain = terrain.GetComponentInChildren<HexTerrain>();
    //    hexTerrrain.onTriggerEnemy += OnActive;
    //}

    public void SetNeighbours(List<HexCell> neighbours)
    {
        Neighbours = neighbours;
    }

    public void SetAttackHexes(List<HexCell> attackCells)
    {
        _AttackCells = attackCells;
    }
    public void EnemySetAttackHexes(List<HexCell> EnemyAttackCells)
    {
        _EnemyAttackCells = EnemyAttackCells;
    }
    public void ClearTerrain()
    {
        if (terrain != null)
        {
            HexTerrain hexTerrrain = terrain.GetComponent<HexTerrain>();
            hexTerrrain.OnMouseEnterAction -= OnMouseEnter;
            hexTerrrain.OnMouseExitAction -= OnMouseExit;

            UnityEngine.Object.Destroy(terrain.gameObject);
        }
    }

    public void ChangeState(ICellState newState)
    {
        if (newState == null)
        {
            Debug.LogError("Trying to set null state.");
            return;
        }

        if (State != newState)
        {
            //Debug.Log($"Changing state from {State} to {newState}");
            if (State != null)
                State.Exit(this);
            State = newState;
            State.Enter(this);
        }
    }

    private void OnMouseEnter()
    {
        ChangeState(State.OnMouseEnter());
    }

    private void OnMouseExit()
    {
        ChangeState(State.OnMouseExit());
    }

    public void OnSelect()
    {

        ChangeState(State.OnSelect());
    }

    public void OnDeselect()
    {
        ChangeState(State.OnDeselect());
    }

    public void OnFocus()
    {
        ChangeState(State.OnFocus());
    }

    public void OnActive()
    {
        ChangeState(State.OnActive());
    }

}
