using Unity.VisualScripting;
using UnityEngine;

public class SelectedState : BaseCellState
{
    public override CellState State => CellState.Selected;
    public static HexCell storedHexcel;
    private Coroutine moveCameraCoroutine;

    public override void Enter(HexCell cell)
    {
       // Debug.LogError($"Cell {cell.AxialCoordinates} is entering Selected State");
        CameraController.Instance.onDeselectAction += cell.OnDeselect;
        CameraController.Instance.onFocusAction += cell.OnFocus;
        CameraController.Instance.IsLocked = true;
        storedHexcel = cell;

     
        if (moveCameraCoroutine != null)
        {
            CameraController.Instance.StopCoroutine(moveCameraCoroutine);
        }
        
        if (cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk )
        {
            cell.SetNeighbours(cell.Neighbours);
            foreach (HexCell neighbour in cell.Neighbours)
            {
                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().Mesher();
                if (neighbour.TerrainType.ID == 0)
                {
                    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction = true;
                    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk = true;
                    neighbour.TerrainType.possibleAction = true;
                }
            }
            
        }
        cell.SetAttackHexes(cell._AttackCells);

        foreach (HexCell attacker in cell._AttackCells)
        {
            if (attacker.TerrainType.ID == 5 || attacker.TerrainType.ID == 1)
            {

                if (attacker != null)
                {
                    attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>().MesherEnemy();
                }
                else if (attacker.TerrainType.ID == 1 || attacker.TerrainType.ID == 5)
                {
                    attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction = true;
                    attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk = false;
                    attacker.TerrainType.possibleAction = true;
                }
                else
                {
                    return;
                }
            }
            else if (cell.TerrainType.ID == 0)
            {
                attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>().Mesher();
            }
            else
            {
                return;
            }


        }
        if (cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk)
        {
            moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
        }
        if (cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction && !cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk)
        {
            if (cell.TerrainType.ID == 5)
            {
                PlayerStateScript.Instance.ShootAnimationTrigger();
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
}
