using Unity.VisualScripting;
using UnityEngine;

public class SelectedState : BaseCellState
{
    public override CellState State => CellState.Selected;
    private HexCell storedHexcel;
    public override void Enter(HexCell cell)
    {
        Debug.LogError($"Cell {cell.AxialCoordinates} is entering Selected State");
        CameraController.Instance.onDeselectAction += cell.OnDeselect;
        CameraController.Instance.onFocusAction += cell.OnFocus;
        CameraController.Instance.IsLocked = true;
        CameraController.Instance.CameraTarget.transform.position = cell.Terrain.transform.position;
        cell.SetNeighbours(cell.Neighbours);
        storedHexcel = cell;
        foreach (HexCell neighbour in cell.Neighbours)
        {
            
            neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().Mesher();
        }

    }

    public override void Exit(HexCell cell)
    {
        Debug.Log($"Cell {cell.AxialCoordinates} is exiting Selected State");
        CameraController.Instance.onDeselectAction -= cell.OnDeselect;
        CameraController.Instance.onFocusAction -= cell.OnFocus;
        CameraController.Instance.IsLocked = false;
    }

    public override ICellState OnDeselect()
    {
        storedHexcel.SetNeighbours(storedHexcel.Neighbours);
        foreach (HexCell neighbour in storedHexcel.Neighbours)
        {

            neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
        }
        return new VisibleState();
    }

    public override ICellState OnFocus()
    {
        return new FocusedState();
    }
}
