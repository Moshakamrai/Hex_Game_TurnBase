using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class OnActiveState :BaseCellState
{
    public override CellState State => CellState.OnActive;
    private Coroutine moveEnemyCoroutine;
    public static HexCell storedEnemyCell;
    public override void Enter(HexCell cell)
    {
       
        Debug.LogError("Active ceel " + cell);
        HexTerrain currentCell = cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        EnemyBrain enemyObject = cell.Terrain.gameObject.GetComponentInChildren<EnemyBrain>();
        //if (storedEnemyCell != null)
        //{
        //    storedEnemyCell.SetNeighbours(storedEnemyCell.Neighbours);
        //    foreach (HexCell neighbour in storedEnemyCell.Neighbours)
        //    {
        //        neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
        //    }
        //}
        if (currentCell.enemyExist)
        {
            cell.SetNeighbours(cell.Neighbours);
            foreach (HexCell neighbour in cell.Neighbours)
            {
                HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                neighboredCell.EnemyViewMesher();
                if (neighbour.TerrainType.ID == 0 && !neighboredCell.playerExist && !neighboredCell.enemyExist)
                {
                    if (enemyObject.turnToken == 1 && enemyObject != null)
                    {
                        //enemyObject.gameObject.transform.position = neighboredCell.gameObject.transform.position;
                        
                        enemyObject.StartCoroutineExternally(cell.MoveToCell(enemyObject.gameObject.transform, neighbour));
                        Debug.LogError("Should MOve Enemy t0 " + cell.AxialCoordinates);
                        //currentCell.cellToken = 0;

                    }
                    neighboredCell.canMoveEnemy = true;
                    neighboredCell.EnemyViewMesher();
                }
                else if (neighbour.TerrainType.ID == 0 && neighboredCell.playerExist)
                {
                    Debug.LogError("kill player");
                }
                if (neighbour.TerrainType.ID == 1)
                {
                    if (neighbour != null)
                    {                      
                        neighboredCell.canWalk = false;               
                    }
                }
            }
            //cell.EnemySetAttackHexes(cell._AttackCells);
        }
        
        else
        {
            Debug.LogError("NOT TRUE BRO");
        }
        storedEnemyCell = cell;
    }
    public override void Exit(HexCell cell)
    {
        
    }
    
    public override ICellState OnMouseEnter()
    {
        return new HighlightedState();
    }
    public override void Update(HexCell cell)
    {
        Debug.LogError($"Cell {cell.AxialCoordinates} is Activated State");
    }
}