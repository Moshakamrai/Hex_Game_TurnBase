using UnityEngine;

public class HiddenState : BaseCellState
{
    public override CellState State => CellState.Hidden;

    public override void Enter(HexCell cell)
    {
        Debug.Log("Entering Hidden State");
        if(cell.Terrain == null)
        {
            
            return;
        }
        cell.Terrain.gameObject.SetActive(false);
    }

    public override void Exit(HexCell cell)
    {
        if(cell.Terrain == null)
        {
            
            return;
        }
        cell.Terrain.gameObject.SetActive(true);
    }

    public override void Update(HexCell cell)
    {

    }
}
