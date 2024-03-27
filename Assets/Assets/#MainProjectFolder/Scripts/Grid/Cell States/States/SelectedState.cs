using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SelectedState : BaseCellState
{
    public override CellState State => CellState.Selected;
    public static HexCell storedHexcel;
    private Coroutine moveCameraCoroutine;
    //public static int moveCount;
    public override void Enter(HexCell cell)
    {
        
       // Debug.LogWarning($"Cell {cell.AxialCoordinates} is entering Selected State");
        HexTerrain currentCell = cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        CameraController.Instance.onDeselectAction += cell.OnDeselect;
        
        CameraController.Instance.onFocusAction += cell.OnFocus;
        CameraController.Instance.IsLocked = true;
        storedHexcel = cell;
        
        if (currentCell.enemyExist)
        {
            //Debug.LogError("Can Action is true for this cell " + cell.AxialCoordinates);
            if (cell.TerrainType.ID == 5 || cell.TerrainType.ID == 1)
            {
                PlayerStateScript.Instance.isShooting = true;
                //Debug.LogError("shoould shoot");
               // PlayerStateScript.Instance.ShootAnimationTrigger();
            }
        }
        if(currentCell.playerExist && currentCell.possibleKillPlayer)
        {
            currentCell.EnemyKillPlayerMesher();
            Debug.LogError("DANGER");
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
                if (neighbour.TerrainType.ID == 0 && neighboredCell.possibleKill != true && !neighboredCell.obstacleExist)
                { 
                    neighboredCell.canWalk = true;
                 
                }
                else if (neighbour.TerrainType.ID == 0 && neighboredCell.possibleKill)
                {
                    neighboredCell.MesherEnemy();
                    neighboredCell.canWalk = false;
                    
                }
                if (neighbour.TerrainType.ID == 5 || neighbour.TerrainType.ID == 1 || neighbour.TerrainType.ID == 0)
                {
                    if (neighboredCell.possibleKill && neighboredCell.enemyExist)
                    {
                        neighboredCell.canAction = true;
                        neighboredCell.MesherEnemy();
                        neighboredCell.canWalk = false;     

                    }
                    else if (!neighboredCell.possibleKill && !neighboredCell.obstacleExist && !neighboredCell.canMoveEnemy && !neighboredCell.possibleKillPlayer)
                    {
                        neighboredCell.Mesher();
                        neighboredCell.canWalk = true;
                        
                    }
                }
            }           
        }
        if (currentCell.playerExist)
        {
            cell.SetAttackHexes(cell._AttackCells);
            foreach (HexCell attacker in cell._AttackCells)
            {
                HexTerrain attackCell = attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                if (attacker.TerrainType.ID == 5 || attacker.TerrainType.ID == 1 || cell.TerrainType.ID == 0)
                {
                    if (attackCell.enemyExist || attacker == OnActiveState.activeCell)
                    {
                        // Debug.LogError("enemy should be here to kill");
                        attackCell.canAction = true;
                        attackCell.MesherEnemy();
                        attackCell.canWalk = false;
                    }
                    else if (!attackCell.possibleKill && !attackCell.barrelExist && !attackCell.obstacleExist)
                    {
                        attackCell.Mesher();

                    }
                    if (attackCell.enemyExist)
                    {
                        break;
                    }

                }
            }
        }
       
        //&& PlayerStateScript.Instance.playerTurn
        //&& moveCount % 2 == 0
        if (currentCell.canWalk )
        {
            if ( cell.AxialCoordinates == new Vector2(0.00f, 0.00f))
            {

                moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
                currentCell.canWalk = false;
            }
            //
            else if (cell.AxialCoordinates != HexCell.playerPosCell.AxialCoordinates &&  currentCell.playerExist != true)
            {

                moveCameraCoroutine = CameraController.Instance.StartCoroutine(cell.MoveCameraToCell(cell));
                
            }
        }
        //moveCount++;
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
