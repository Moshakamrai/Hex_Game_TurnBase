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
        //if (cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>().enemyExist)
        //{
        //    Debug.LogError("Understanding the Science");
        //    cell.SetNeighbours(cell.Neighbours);
        //    foreach (HexCell neighbour in cell.Neighbours)
        //    {
        //        neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().Mesher();
        //        if (neighbour.TerrainType.ID == 0)
        //        {
        //            neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk = true;
        //            neighbour.TerrainType.possibleAction = true;
        //        }
        //        if (neighbour.TerrainType.ID == 5 || neighbour.TerrainType.ID == 1)
        //        {
        //            if (neighbour != null)
        //            {
        //                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().MesherEnemy();
        //                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canWalk = false;
        //                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().canAction = false;
        //                neighbour.TerrainType.possibleAction = true;
        //            }
        //        }
        //    }
        //}
    }

    public override void Exit(HexCell cell)
    {
        //Debug.LogError($"Cell {cell.AxialCoordinates} is exiting Visible State");
    }

    public override ICellState OnActive()
    {
        return new OnActiveState();
    }

    public override ICellState OnMouseEnter()
    {
        return new HighlightedState();
    }

    public override void Update(HexCell cell)
    {

    }
}
