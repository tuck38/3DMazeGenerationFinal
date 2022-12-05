using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EllerMaze : MonoBehaviour
{
    public int sideSize;
    protected List<MazeCell> _Cells;

    [SerializeField] GameObject cell = Resources.Load<GameObject>("Prefabs/Cell");

    public int wallChance = 50;

    public EllerMaze(int side)
    {
        _Cells = new List<MazeCell>();
        sideSize = side;
    }

    public void BuildMaze()
    {
        for (int x = 0; x <= sideSize; x++) //having y then x makes it build row by row instead of column by column
        {
            for (int y = 0; y <= sideSize; y++)
            {
                GameObject newCell;
                newCell = Instantiate(cell, new Vector3(sideSize - (x * cell.transform.localScale.x), 0, sideSize - (y * cell.transform.localScale.z)), Quaternion.identity);
                MazeCell cellComp = newCell.GetComponent<MazeCell>();
                _Cells.Add(cellComp);
            }
        }

        Step();

    }

    public MazeCell GetCell(int x, int y)
    {
        int sideLength = sideSize + 1;
        if(x >= 0 && x <= sideLength && y >= 0 && y <= sideLength)
        {
            return _Cells[x + (y * sideLength)];
        }
        else
        {
            Debug.Log("out of bounds cell");
            return default(MazeCell);
            
        }

        
    }

    public void SetHorizontal(MazeCell left, MazeCell right, bool value)
    {
        left.SetEast(value);
        right.SetWest(value);
    }

    public void SetVertical(MazeCell top, MazeCell bottom, bool value)
    {
        top.SetSouth(value);
        bottom.SetNorth(value);
    }

    //generating stuff


    public void Step()
    {
        int maxSet = 1;
        List<int> usedSets = new List<int>();

        for (int currentRow = 0; currentRow <= sideSize; currentRow++)
        {
            usedSets.Clear();

            if(currentRow == 0) //top row
            {


            }

            if(currentRow == sideSize) //bottom row
            {
                for(int i = 0; i <= sideSize-1; i++)
                {
                    SetHorizontal(GetCell(i, currentRow), GetCell(i+1, currentRow), false);
                }
                return;
            }

            for(int i = 0; i <= sideSize-1; i++) //assigning sets
            {
                if(!GetCell(i, currentRow).GetNorth())
                {
                    GetCell(i, currentRow).SetSet(GetCell(i, currentRow - 1).GetSet());

                }
                else
                {
                    GetCell(i, currentRow).SetSet(maxSet);
                    maxSet++;

                }
            }


            //adding horizontal walls
            for (int i = 0; i < sideSize; i++)
            {
                int currentSet = GetCell(i, currentRow).GetSet();
                if (currentSet != GetCell(i + 1, currentRow).GetSet()) //next cell is different set
                {
                    int randNum = Random.Range(0, 100);//Random number between 0 and 100

                    if(randNum < wallChance)
                    {
                        SetHorizontal(GetCell(i, currentRow), GetCell(i + 1, currentRow), true);
                    }
                    else
                    {
                        SetHorizontal(GetCell(i, currentRow), GetCell(i + 1, currentRow), false);
                        List<MazeCell> cellsWithGroup = getGroup(i + 1, currentRow);
                        
                        foreach (MazeCell cell in cellsWithGroup)
                        {
                            cell.SetSet(currentSet);
                        }
                    }
                }
                else //next cell is same set, put wall
                {
                    SetHorizontal(GetCell(i, currentRow), GetCell(i + 1, currentRow), true);
                }
            }

            //adding vertical walls

            for(int i = 0; i <= sideSize; i++)
            {
                int currentSet = GetCell(i, currentRow).GetSet();
                if (!usedSets.Contains(currentSet)) //current set is not in used sets
                {
                    List<Vector2> currentSetGroup = getGroupPos(i, currentRow);
                    if(currentSetGroup.Count == 1)
                    {
                        SetVertical(GetCell((int)currentSetGroup[0].x, (int)currentSetGroup[0].y), GetCell((int)currentSetGroup[0].x, (int)currentSetGroup[0].y + 1), false);

                        
                    }
                    else
                    {
                        int randGroup = Random.Range(0, currentSetGroup.Count - 1);
                        for(int a = 0; a <= currentSetGroup.Count -1 ; a++)
                        {
                            if(a == randGroup)
                            {
                                SetVertical(GetCell((int)currentSetGroup[a].x, (int)currentSetGroup[a].y), GetCell((int)currentSetGroup[a].x, (int)currentSetGroup[a].y + 1), false);
                            }
                        }
                    }
                    usedSets.Add(currentSet);
                }

            }

        }
    }

    public List<MazeCell> getGroup(int columnOfSet, int row)
    {
        List<MazeCell> list = new List<MazeCell>();
        int set = GetCell(columnOfSet, row).GetSet();
        for(int i = 0; i <= sideSize; i++)
        {
            if (set == GetCell(i, row).GetSet())
            {
                list.Add(GetCell(i, row));
            }
        }
        return list;
    }

    public List<Vector2> getGroupPos(int columnOfSet, int row)
    {
        List<Vector2> list = new List<Vector2>();
        int set = GetCell(columnOfSet, row).GetSet();
        for (int i = 0; i <= sideSize; i++)
        {
            if (set == GetCell(i, row).GetSet())
            {
                list.Add(new Vector2(i, row));
            }
        }
        return list;
    }


}