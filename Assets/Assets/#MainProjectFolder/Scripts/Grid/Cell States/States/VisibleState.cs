using UnityEngine;

public class VisibleState : BaseCellState
{
    public override CellState State => CellState.Visible;

    public override void Enter(HexCell cell)
    {
        //Debug.LogError($"Cell {cell.AxialCoordinates} is entering Visible State");
        if(cell.Terrain != null && !cell.Terrain.gameObject.activeSelf)
        {
            cell.Terrain.gameObject.SetActive(true);
        }

        if (cell.AxialCoordinates == new Vector2(0.00f, 0.00f))
        {
   
            cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk = true;
        }
        
    }

    public override void Exit(HexCell cell)
    {
        //Debug.LogError($"Cell {cell.AxialCoordinates} is exiting Visible State");
    }

    //public override ICellState OnActive()
    //{
    //    return new OnActiveState();
    //}

    public override ICellState OnMouseEnter()
    {
        return new HighlightedState();
    }

    public override void Update(HexCell cell)
    {

    }
}
