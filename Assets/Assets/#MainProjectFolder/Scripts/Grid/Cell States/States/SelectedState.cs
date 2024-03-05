using Unity.VisualScripting;
using UnityEngine;

public class SelectedState : BaseCellState
{
    public override CellState State => CellState.Selected;
    public static HexCell storedHexcel;
    private Coroutine moveCameraCoroutine;

    public override void Enter(HexCell cell)
    {
        //Debug.LogError($"Cell {cell.AxialCoordinates} is entering Selected State");
        CameraController.Instance.onDeselectAction += cell.OnDeselect;
        CameraController.Instance.onFocusAction += cell.OnFocus;
        CameraController.Instance.IsLocked = true;
        storedHexcel = cell;

     
        if (moveCameraCoroutine != null)
        {
            CameraController.Instance.StopCoroutine(moveCameraCoroutine);
        }
        
        if (cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction)
        {
            PlayerStateScript.Instance.WalkAnimationTrigger();
            moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
            cell.SetNeighbours(cell.Neighbours);

            foreach (HexCell neighbour in cell.Neighbours)
            {  
                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().Mesher();
                if (neighbour.TerrainType.ID == 0)
                {
                    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction = true;
                    neighbour.TerrainType.possibleAction = true;
                }
            }
        }
        

    }

    public override void Exit(HexCell cell)
    {
        Debug.LogError($"Cell {cell.AxialCoordinates} is exiting Selected State");
        CameraController.Instance.onDeselectAction -= cell.OnDeselect;
        CameraController.Instance.onFocusAction -= cell.OnFocus;
        CameraController.Instance.IsLocked = false;
        
        // Stop the coroutine if the cell exits the selected state
        if (moveCameraCoroutine != null)
            CameraController.Instance.StopCoroutine(moveCameraCoroutine);
    }

    

    public override ICellState OnDeselect()
    {
        //storedHexcel.SetNeighbours(storedHexcel.Neighbours);
        //foreach (HexCell neighbour in storedHexcel.Neighbours)
        //{
        //    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
        //}
        return new VisibleState();
    }

    public override ICellState OnFocus()
    {
        return new FocusedState();
    }
}
