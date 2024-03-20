using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SelectedState : BaseCellState
{
    public override CellState State => CellState.Selected;
    public static HexCell storedHexcel;
    private Coroutine moveCameraCoroutine;

    public override void Enter(HexCell cell)
    {
       // Debug.LogError($"Cell {cell.AxialCoordinates} is entering Selected State");
        HexTerrain currentCell = cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        CameraController.Instance.onDeselectAction += cell.OnDeselect;
        CameraController.Instance.onFocusAction += cell.OnFocus;
        CameraController.Instance.IsLocked = true;
        storedHexcel = cell;
        if (currentCell.enemyExist)
        {
            Debug.LogError("Can Action is true for this cell " + cell.AxialCoordinates);
            if (cell.TerrainType.ID == 5 || cell.TerrainType.ID == 1)
            {
                PlayerStateScript.Instance.isShooting = true;
                Debug.LogError("shoould shoot");
                PlayerStateScript.Instance.ShootAnimationTrigger();
            }
        }

        if (moveCameraCoroutine != null)
        {
            CameraController.Instance.StopCoroutine(moveCameraCoroutine);
        }
        
        if (currentCell.canWalk)
        {
            cell.SetNeighbours(cell.Neighbours);
            foreach (HexCell neighbour in cell.Neighbours)
            {
                HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                neighboredCell.Mesher();
                if (neighbour.TerrainType.ID == 0 && neighboredCell.enemyExist != true)
                { 
                    neighboredCell.canWalk = true;
                    neighbour.TerrainType.possibleAction = true;
                }
                else
                {
                    neighboredCell.MesherEnemy();
                    neighboredCell.canWalk = false;
                    neighbour.TerrainType.possibleAction = true;
                }
                if (neighbour.TerrainType.ID == 5 || neighbour.TerrainType.ID == 1)
                {
                    if (neighbour != null && neighboredCell.enemyExist)
                    {
                        neighboredCell.MesherEnemy();
                        neighboredCell.canWalk = false;     
                        neighbour.TerrainType.possibleAction = true;
                    }
                    else if (neighbour != null && !neighboredCell.enemyExist)
                    {
                        neighboredCell.Mesher();
                        neighboredCell.canWalk = true;
                        neighbour.TerrainType.possibleAction = true;
                    }
                }
            }
            
        }
        cell.SetAttackHexes(cell._AttackCells);
        foreach (HexCell attacker in cell._AttackCells)
        {
            HexTerrain attackCell = attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
            if (attacker.TerrainType.ID == 5 || attacker.TerrainType.ID == 1 || cell.TerrainType.ID == 0)
            {
                if (attacker != null && attackCell.enemyExist)
                {
                    attackCell.MesherEnemy();
                    attackCell.canWalk = false;
                }
                else if (attacker != null && !attackCell.enemyExist)
                {
                    attackCell.Mesher();
                   
                }
                if (attacker.TerrainType.ID == 5 && attackCell.enemyExist)
                {
                    break;
                }         
            }
            

        }
        //&& PlayerStateScript.Instance.playerTurn
        if (currentCell.canWalk )
        {
            if ( cell.AxialCoordinates == new Vector2(0.00f, 0.00f))
            {
                moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
            }
            else if (cell.AxialCoordinates != HexCell.playerPosCell.AxialCoordinates)
            {
                moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
            }
        }
        
    }

    public override void Exit(HexCell cell)
    {
        Debug.Log($"Cell {cell.AxialCoordinates} is exiting Selected State");
        CameraController.Instance.onDeselectAction -= cell.OnDeselect;
        CameraController.Instance.onFocusAction -= cell.OnFocus;
        CameraController.Instance.IsLocked = false;
        
        // Stop the coroutine if the cell exits the selected state
        if (moveCameraCoroutine != null)
            CameraController.Instance.StopCoroutine(moveCameraCoroutine);

    }

   
    public override ICellState OnDeselect()
    {
        
        return new VisibleState();
        
    }

    public override ICellState OnFocus()
    {
        return new FocusedState();
    }

    public override void Update(HexCell cell)
    {

    }
}
