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
       
        Debug.LogWarning("Active ceel enemy " + cell);
        HexTerrain currentCell = cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        EnemyBrain enemyObject = cell.Terrain.gameObject.GetComponentInChildren<EnemyBrain>();
        storedEnemyCell = cell;
        
        if (currentCell.enemyExist)
        {
            if (storedEnemyCell != null)
            {
                storedEnemyCell.SetNeighbours(storedEnemyCell.Neighbours);
                foreach (HexCell neighbour in storedEnemyCell.Neighbours)
                {
                    //Debug.LogError("after enemy Moved it should Unmesh now");

                    neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
                }
            }
            cell.SetNeighbours(cell.Neighbours);
            //if (PlayerStateScript.Instance.playerTurn == false) 
            {
                foreach (HexCell neighbour in cell.Neighbours)
                {
                    HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                    if (neighbour.TerrainType.ID == 0 && !neighboredCell.playerExist && !neighboredCell.enemyExist)
                    {
                        if (enemyObject.turnToken == 1 && enemyObject != null)
                        {
                            //enemyObject.gameObject.transform.position = neighboredCell.gameObject.transform.position;
                            enemyObject.turnToken = 0;
                            enemyObject.StartCoroutineExternally(cell.MoveToCell(enemyObject.gameObject.transform, neighbour));
                            //Debug.LogError("Should MOve Enemy t0 " + neighbour.AxialCoordinates);
                            
                            //currentCell.cellToken = 0;

                        }
                        neighboredCell.canMoveEnemy = true;
                        neighboredCell.EnemyViewMesher();
                    }
                    else if (neighbour.TerrainType.ID == 0 && neighboredCell.playerExist)
                    {
                        neighboredCell.EnemyKillPlayerMesher();
                        Debug.LogError("kill player");
                    }
                    if (neighbour.TerrainType.ID == 1)
                    {
                        if (neighbour != null)
                        {
                            neighboredCell.canMoveEnemy = false;
                        }
                    }
                    //ResourceManager.Instance.GiveToken(enemyObject.gameObject);
                }
                
            }
            
        }
        //cell.EnemySetAttackHexes(cell._AttackCells);
        //foreach (HexCell attacker in cell._AttackCells)
        //{
        //    HexTerrain attackCell = attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        //    if (attacker.TerrainType.ID == 5 || attacker.TerrainType.ID == 1 || cell.TerrainType.ID == 0)
        //    {
        //        if (attacker != null && attackCell.playerExist)
        //        {
        //            attackCell.MesherEnemy();
        //            attackCell.canMoveEnemy = false;
        //        }
        //        else if (attacker != null && !attackCell.enemyExist)
        //        {
        //            attackCell.EnemyViewMesher();

        //        }
        //        if (attacker.TerrainType.ID == 5 && attackCell.enemyExist)
        //        {
        //            break;
        //        }
        //    }

        //}
        
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