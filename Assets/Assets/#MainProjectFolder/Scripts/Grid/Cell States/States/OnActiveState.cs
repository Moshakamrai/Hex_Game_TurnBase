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
    public static HexCell activeCell;
    
    public override void Enter(HexCell cell)
    {
       
        Debug.LogWarning("Active ceel enemy " + cell);
        HexTerrain currentCell = cell.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
        EnemyBrain enemyObject = cell.Terrain.gameObject.GetComponentInChildren<EnemyBrain>();
        

        if (currentCell.barrelExist && currentCell.barrelExploded)
        {
            Debug.Log("should be on fire");
            currentCell.barrelExploded = false;
            cell.SetNeighbours(cell.Neighbours);
            foreach (HexCell neighbour in cell.Neighbours)
            {
                HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>(); 
                neighboredCell.Onfire();

            }
        }

        if (currentCell.enemyExist)
        {

            cell.SetNeighbours(cell.Neighbours);
            foreach (HexCell neighbour in cell.Neighbours)
            {
                Debug.LogError("after enemy Moved it should Unmesh now");
                neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>().UnMesher();
            }
            if (currentCell.currentEnemyObject.GetComponent<EnemyBrain>() != null)
            {
                if (currentCell.currentEnemyObject.GetComponent<EnemyBrain>().gunner)
                {
                    Debug.LogError("EnemyMesh process starts");
                    cell.EnemySetAttackHexes(cell._AttackCells);
                    foreach (HexCell attacker in cell._AttackCells)
                    {
                        Debug.LogError("EnemyMesh process Mid Point");
                        HexTerrain attackCell = attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                        if (attackCell.playerExist || attackCell.possibleKillPlayer)
                        {
                            Debug.LogError("Got the freaking player");
                            //currentCell.currentEnemyObject.GetComponent<EnemyBrain>().canKillPlayer = true;

                            currentCell.currentEnemyObject.GetComponent<EnemyBrain>().TriggerShootingAnimation();
                            attackCell.EnemyKillPlayerMesher();
                            //attackCell.canMoveEnemy = false;
                        }
                        else if (!attackCell.enemyExist && !attackCell.barrelExist && !attackCell.obstacleExist)
                        {
                            Debug.LogError("enemyMeshing");
                            attackCell.EnemyViewMesher();

                        }
                        if (attackCell.playerExist || attackCell.obstacleExist)
                        {
                            Debug.LogError("breaking");
                            break;
                        }
                    }
                }
            }


            foreach (HexCell neighbour in cell.Neighbours)
            {
                 HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                if (neighbour.TerrainType.ID == 0 && !neighboredCell.playerExist && !neighboredCell.enemyExist )
                {
                    if (enemyObject.turnToken == 1 && enemyObject != null)
                    {
                        Vector2 distanceToNeighbour = neighbour.AxialCoordinates - HexCell.playerPosCell.AxialCoordinates;
                        Vector2 distanceToCell = cell.AxialCoordinates - HexCell.playerPosCell.AxialCoordinates;
                        if (distanceToNeighbour.magnitude < distanceToCell.magnitude && neighboredCell.canMoveEnemy)
                        {
                            neighboredCell.possibleKill = true;
                            if (neighboredCell.playerExist)
                            {
                                enemyObject.TriggerStabbingAnimation();
                            }

                            enemyObject.turnToken = 0;
                            enemyObject.StartCoroutineExternally(cell.MoveToCell(enemyObject.gameObject.transform, neighbour));

                            activeCell = neighbour;

                        }
                    }
                }
                if (activeCell != null)
                {
                    activeCell.SetNeighbours(activeCell.Neighbours);
                    foreach (HexCell newNeighbor in activeCell.Neighbours)
                    {
                        HexTerrain neighboredCell2 = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                        if (neighboredCell2.playerExist)
                        {

                            neighboredCell2.EnemyKillPlayerMesher();
                        }
                        else if (neighboredCell2.barrelExist || neighboredCell2.obstacleExist)
                        {
                            neighboredCell2.canMoveEnemy = false;
                        }
                        else
                        {
                            neighboredCell2.canMoveEnemy = true;
                            neighboredCell2.EnemyViewMesher();
                        }

                    }
                }
                else
                    {
                        neighboredCell.canMoveEnemy = true;
                        neighboredCell.EnemyViewMesher();
                    }
                      
                 
                 if (neighbour.TerrainType.ID == 0 && neighboredCell.playerExist && !currentCell.currentEnemyObject.GetComponent<EnemyBrain>().gunner)
                 {
                    neighboredCell.EnemyKillPlayerMesher();
                    currentCell.currentEnemyObject.GetComponent<EnemyBrain>().TriggerStabbingAnimation();
                    Debug.LogError("kill player");
                 }
                 
                    //ResourceManager.Instance.GiveToken(enemyObject.gameObject);
                
                
                

                }

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

