using UnityEngine;


public class HighlightedState : BaseCellState
{
    public override CellState State => CellState.Highlighted;

    public override void Enter(HexCell cell)
    {
        //Debug.LogError($"Cell {cell.AxialCoordinates} is in Highlighted State");
        //Tween local Scale of the cell.Terrain gameobject
        LeanTween.scale(cell.Terrain.gameObject, Vector3.one * 1f, 0.1f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveY(cell.Terrain.gameObject, 3.5f, 0.2f).setEase(LeanTweenType.easeOutBack);
        CameraController.Instance.onSelectAction += cell.OnSelect;
    }

    public override void Exit(HexCell cell)
    {
        Debug.Log($"Cell {cell.AxialCoordinates} is exiting Highlighted State");
        //Tween local Scale of the cell.Terrain gameobject
        LeanTween.scale(cell.Terrain.gameObject, Vector3.one * 0.8f, 0.1f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveY(cell.Terrain.gameObject, 0f, 0.2f).setEase(LeanTweenType.easeOutBack);
        CameraController.Instance.onSelectAction -= cell.OnSelect;
    }

    public override ICellState OnMouseExit()
    {
        return new VisibleState();
    }

    public override ICellState OnSelect()
    {
        if (SelectedState.storedHexcel != null )
        {
            SelectedState.storedHexcel.SetNeighbours(SelectedState.storedHexcel.Neighbours);
            SelectedState.storedHexcel.SetAttackHexes(SelectedState.storedHexcel._AttackCells);
            foreach (HexCell neighbour in SelectedState.storedHexcel.Neighbours)
            {

                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();

            }
            foreach (HexCell attacker in SelectedState.storedHexcel._AttackCells)
            {

                attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
            }
        }

        
        return new SelectedState();
    }

    public override void Update(HexCell cell)
    {

    }
}
