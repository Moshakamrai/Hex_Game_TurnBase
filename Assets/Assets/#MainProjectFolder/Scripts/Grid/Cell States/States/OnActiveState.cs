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

        if (currentCell.enemyExist && !currentCell.currentEnemyObject.GetComponent<EnemyBrain>().death )
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
            if (!currentCell.currentEnemyObject.GetComponent<EnemyBrain>().death)
            {
                foreach (HexCell neighbour in cell.Neighbours)
                {
                    HexTerrain neighboredCell = neighbour.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                    if (neighbour.TerrainType.ID == 0 && !neighboredCell.playerExist && !neighboredCell.enemyExist && !currentCell.obstableObject)
                    {
                        if (enemyObject.turnToken == 1 && enemyObject != null)
                        {
                            Vector2 distanceToNeighbour = neighbour.AxialCoordinates - HexCell.playerPosCell.AxialCoordinates;
                            Vector2 distanceToCell = cell.AxialCoordinates - HexCell.playerPosCell.AxialCoordinates;
                            if(distanceToNeighbour.magnitude < distanceToCell.magnitude)
                            {
                                enemyObject.turnToken = 0;

                                enemyObject.StartCoroutineExternally(cell.MoveToCell(enemyObject.gameObject.transform, neighbour));
                            }
                            //enemyObject.gameObject.transform.position = neighboredCell.gameObject.transform.position;
                            
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
                cell.EnemySetAttackHexes(cell._AttackCells);
                foreach (HexCell attacker in cell._AttackCells)
                {
                    HexTerrain attackCell = attacker.Terrain.gameObject.GetComponentInChildren<HexTerrain>();
                    if (attacker.TerrainType.ID == 5 || attacker.TerrainType.ID == 1 || cell.TerrainType.ID == 0)
                    {
                        if (attackCell.playerExist)
                        {
                            Debug.LogError("Can Kill Player next turn");
                            attackCell.EnemyKillPlayerMesher();
                            //attackCell.canMoveEnemy = false;
                        }
                        else if (attacker != null && !attackCell.enemyExist)
                        {
                            attackCell.EnemyKillPlayerMesher();

                        }
                        if (attackCell.playerExist || attackCell.barrelExist)
                        {
                            break;
                        }
                    }

                }

            }
            
        }


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