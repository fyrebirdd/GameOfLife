using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private MeshRenderer myMeshRenderer;
    
    public bool isAlive;
    public bool isObstacle;
    public bool isDead = true;
    
    [SerializeField] private Material aliveMat;
    [SerializeField] private Material deadMat;
    [SerializeField] private Material obstacleMat;
    [SerializeField] private Material debugMat;

    public int livingNeighbors = 0;

    private void Awake()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnCellSelected()
    {
        if (isDead)
        {
            //clicking a dead cell makes it alive
            SetCellLiving();
        }

        else if (isAlive)
        {
            //clicking an alive cell makes it an obstacle
            SetCellObstacle();
        }
        
        else if (isObstacle)
        {
            //clicking an obstacle makes it dead again
            SetCellDead();
        }
    }

    public void SetCellDead()
    {
        isDead = true;
        isAlive = false;
        isObstacle = false;
        UpdateCellColor();
    }

    public void SetCellLiving()
    {
        isDead = false;
        isAlive = true;
        isObstacle = false;
        UpdateCellColor();
    }

    public void SetCellObstacle()
    {
        isDead = false;
        isAlive = false;
        isObstacle = true;
        UpdateCellColor();
    }
    
    private void UpdateCellColor()
    {
        if (isDead)
        {
            myMeshRenderer.material = deadMat;
        }
        else if (isAlive)
        {
            myMeshRenderer.material = aliveMat;
        }
        else if (isObstacle)
        {
            myMeshRenderer.material = obstacleMat;
        }
    }

    public void SetLivingNeighbors(List<List<Cell>> cellList)
    {
        if (isObstacle) return;
        livingNeighbors = CheckForLivingNeighbors(cellList);
    }

    private int CheckForLivingNeighbors(List<List<Cell>> cellList)
    {
        myMeshRenderer.material = debugMat;
        var xPosAsInt = (int)transform.position.x;
        var zPosAsInt = (int)transform.position.z;

        var aliveNeighbors = 0;

        var atBottomEdge = xPosAsInt == 0;
        var atTopEdge = xPosAsInt == cellList[0].Count - 1;
        var atLeftEdge = zPosAsInt == 0;
        var atRightEdge = zPosAsInt == cellList[0].Count - 1;
        
        if (atLeftEdge)
        {
            if (atBottomEdge)
            {
                if (cellList[xPosAsInt + 1][zPosAsInt].isAlive) //cell above
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt][zPosAsInt + 1].isAlive) // cell right
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt + 1][zPosAsInt + 1].isAlive) // cell above right
                {
                    aliveNeighbors += 1;
                }
            }
            else if (atTopEdge)
            {
                if (cellList[xPosAsInt - 1][zPosAsInt].isAlive) //cell below
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt][zPosAsInt + 1].isAlive) // cell right
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt - 1][zPosAsInt + 1].isAlive) // cell below right
                {
                    aliveNeighbors += 1;
                }
            }
        }
        else if (atRightEdge)
        {
            if (atBottomEdge)
            {
                if (cellList[xPosAsInt + 1][zPosAsInt].isAlive) //cell above
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt][zPosAsInt - 1].isAlive) // cell left
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt + 1][zPosAsInt - 1].isAlive) // cell above left
                {
                    aliveNeighbors += 1;
                }
            }
            else if (atTopEdge)
            {
                if (cellList[xPosAsInt - 1][zPosAsInt].isAlive) //cell below
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt][zPosAsInt - 1].isAlive) // cell left
                {
                    aliveNeighbors += 1;
                }
                if (cellList[xPosAsInt - 1][zPosAsInt - 1].isAlive) // cell below left
                {
                    aliveNeighbors += 1;
                }
            }
        }
        else if (atTopEdge)
        {
            if (cellList[xPosAsInt - 1][zPosAsInt].isAlive) //cell below
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt - 1][zPosAsInt - 1].isAlive) // cell down left
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt - 1][zPosAsInt + 1].isAlive) // cell down right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt + 1].isAlive) // cell right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt - 1].isAlive) // cell left
            {
                aliveNeighbors += 1;
            }
        }
        else if (atBottomEdge)
        {
            if (cellList[xPosAsInt + 1][zPosAsInt].isAlive) //cell above
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt + 1][zPosAsInt - 1].isAlive) // cell up left
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt + 1][zPosAsInt + 1].isAlive) // cell up right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt + 1].isAlive) // cell right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt - 1].isAlive) // cell left
            {
                aliveNeighbors += 1;
            }
        }
        else
        {
            if (cellList[xPosAsInt + 1][zPosAsInt].isAlive) //cell above
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt + 1][zPosAsInt - 1].isAlive) // cell up left
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt + 1][zPosAsInt + 1].isAlive) // cell up right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt + 1].isAlive) // cell right
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt][zPosAsInt - 1].isAlive) // cell left
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt - 1][zPosAsInt].isAlive) //cell below
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt - 1][zPosAsInt - 1].isAlive) // cell down left
            {
                aliveNeighbors += 1;
            }
            if (cellList[xPosAsInt - 1][zPosAsInt + 1].isAlive) // cell down right
            {
                aliveNeighbors += 1;
            }
        }

        return aliveNeighbors;
    }

    public void NewGeneration()
    {
        if (isObstacle)
        {
            return;
        }
        if (isAlive)
        {
            if (livingNeighbors is 2 or 3)
            {
                //cell lives
                SetCellLiving();
            }
            else
            {
                //cell dies
                SetCellDead();
            }
        }
        else if (isDead)
        {
            if (livingNeighbors == 3)
            {
                //cell becomes alive
                SetCellLiving();
            }
            else
            {
                SetCellDead();
            }
        }
        
        
    }
}
