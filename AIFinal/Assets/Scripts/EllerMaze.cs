using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EllerMaze : MonoBehaviour
{
    public int width;
    public int height;
    protected List<MazeCell> _Cells;

    [SerializeField] GameObject cell = Resources.Load<GameObject>("Prefabs/Cell");

    public int wallChance = 50;

    int leftEdge = 0; //keft wall is (0, y)
    int rightEdge; //right wall is (sideSize, y)
    int topEdge = 0; //top wall is (x, 0)
    int bottomEdge; //bottom wall is (x, sideSize)

    int maxSet = 1;

    public EllerMaze(int mazeWidth, int mazeHeight)
    {
        _Cells = new List<MazeCell>();
        width = mazeWidth;
        height = mazeHeight;

        rightEdge = width;
        bottomEdge = height;

    }

    public void BuildMaze()
    {
        for (int x = 0; x <= width; x++) //having y then x makes it build row by row instead of column by column
        {
            for (int y = 0; y <= height; y++)
            {
                GameObject newCell;
                newCell = Instantiate(cell, new Vector3(width - (x * cell.transform.localScale.x), 0, height - (y * cell.transform.localScale.z)), Quaternion.identity);
                MazeCell cellComp = newCell.GetComponent<MazeCell>();
                cellComp.x = x;
                cellComp.z = y;
                _Cells.Add(cellComp);
            }
        }
        Step();
    }

    public MazeCell GetCell(int x, int y)
    {
        int sideLength = height + 1;

        if(x >= 0 && x <= width && y >= 0 && y <= height)
        {
            return _Cells[x + (y * sideLength)];
        }
        else
        {
            Debug.Log("out of bounds cell: " + x + " " + y);
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
        List<int> usedSets = new List<int>();

        for (int currentRow = 0; currentRow <= height; currentRow++)
        {
            usedSets.Clear();

            if(currentRow == 0) //top row
            {


            }

            

            for(int i = 0; i <= width; i++) //assigning sets
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
            for (int i = 0; i < width; i++)
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

            if (currentRow == height) //bottom row check (after adding walls so BuildRow is easier, means the original generated wall is technically incorrect but oh well)
            {
                for (int i = 0; i < width; i++)
                {
                    SetHorizontal(GetCell(i, currentRow), GetCell(i + 1, currentRow), false);
                }
                return;
            }




            //adding vertical walls

            for (int i = 0; i <= width; i++)
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
        for(int i = 0; i <= width; i++)
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
        for (int i = 0; i <= width; i++)
        {
            if (set == GetCell(i, row).GetSet())
            {
                list.Add(new Vector2(i, row));
            }
        }
        return list;
    }

    public List<int> GetSetGroupFromList(List<MazeCell> group, int set, int start,int end) //returns indexes in group
    {
        List<int> list = new List<int>();
        for(int i = start; i <= end; i++)
        {
            if(set == group[i].GetSet())
            {
                list.Add(i);
            }
        }
        return list;
    }



    //this might not work for left and right edges
    public List<MazeCell> BuildRow(List<MazeCell> topRow, int length) //length is either width or height depending on which edge is being used
    {
        int realLength = length + 1;
        int endLength = 2 * length + 1;
        List<MazeCell> newRows = topRow;
        List<int> usedSets = new List<int>();

        //create (length) MazeCells, add to newRows
        for(int i = 0; i <= length; i++)
        {
            MazeCell newCell = new MazeCell();
            newRows.Add(newCell);
        }

        for(int i = 0; i <= newRows.Count - 1; i++)
        {
            Debug.Log(i);
        }


         //take out some bottom walls
        for (int i = 0; i <= length; i++)
        {
            int currentSet = newRows[i].GetSet();
            if (!usedSets.Contains(currentSet)) //current set is not in used sets
            {
                List<int> currentSetGroup = GetSetGroupFromList(newRows, currentSet, 0, length);
                if (currentSetGroup.Count == 1)
                {
                    int place = currentSetGroup[0];
                    SetVertical(newRows[place], newRows[place + realLength], false); //causing bug - assume it is going higher than newRows size, but it shouldn't - the highest place can be is 10, so the highest element it can go to is 21, which is in the array
                }
                else
                {
                    int randGroup = Random.Range(0, currentSetGroup.Count - 1);
                    for (int a = 0; a <= currentSetGroup.Count - 1; a++)
                    {
                        if (a == randGroup)
                        {
                            int place = currentSetGroup[a];
                            SetVertical(newRows[place], newRows[place + realLength], false);//causing bug
                        }
                    }
                }
                usedSets.Add(currentSet);
            }

        }

        //assign sets to new row (length -> length * 2)
        //      GetNorth -> newRows[i - length]
        for (int i = realLength; i <= endLength; i++) //assigning sets
        {
            if (!newRows[i].GetNorth())
            {
                newRows[i].SetSet(newRows[i - realLength].GetSet());

            }
            else
            {
                newRows[i].SetSet(maxSet);
                maxSet++;
            }
        }

        //add horizontal walls as normal

        for (int i = realLength; i < endLength; i++)
        {
            int currentSet = newRows[i].GetSet();
            if (currentSet != newRows[i+1].GetSet()) //next cell is different set
            {
                int randNum = Random.Range(0, 100);//Random number between 0 and 100

                if (randNum < wallChance)
                {
                    SetHorizontal(newRows[i], newRows[i+1], true);
                }
                else
                {
                    SetHorizontal(newRows[i], newRows[i+1], false);
                    List<int> cellsWithGroup = GetSetGroupFromList(newRows, currentSet, realLength, endLength);

                    foreach (int cell in cellsWithGroup)
                    {
                        newRows[cell].SetSet(currentSet);
                    }
                }
            }
            else //next cell is same set, put wall
            {
                SetHorizontal(newRows[i], newRows[i+1], true);
            }
        }

        //make sure all south walls for the bottom row are true
        for (int i = realLength; i <= endLength; i++)
        {
            newRows[i].SetSouth(true);


        }


            return newRows;

    }


    public void BuildNorth()
    {
        //get top row, from right to left
        //rotate the cells 180 degrees (north = south, east = west

        //call BuildRow with the new cells

        //for(0 - > width)
        //  actual cells[width - i] = new cells[i]

        //for(width + 1 -> width * 2
        //  build new cell and give it new cell[i]
    }

    public void BuildSouth()
    {
        List<MazeCell> bottomRow = new List<MazeCell>();

        for(int i = 0; i <= width; i++)
        {
            bottomRow.Add(GetCell(i, height));
        }

        List<MazeCell> newRows = BuildRow(bottomRow, width);


        for(int i = 0; i <= width; i++)
        {
            GetCell(i, height).CopyCell(newRows[i]);
        }

        
        for (int i = width+1; i <= 2*width+1; i++)
        {
            int x = i - (width + 1);
            int y = height + 1;

            GameObject newCell;
            newCell = Instantiate(cell, new Vector3(width - (x * cell.transform.localScale.x), 0, height - (y * cell.transform.localScale.z)), Quaternion.identity);
            newCell.GetComponent<MazeCell>().CopyCell(newRows[i]);
        }
        height++;

    }

    public List<AStarNode> getVisitables(AStarNode parent)
    {
        List<AStarNode> visitables = new List<AStarNode>();

        if(!parent.node.GetEast())
        {
            if(GetCell(parent.node.x + 1, parent.node.z) != parent.node)
            {
                visitables.Add(new AStarNode(parent.node.x + 1, parent.node.z, GetCell(parent.node.x + 1, parent.node.z), parent));
            }
        }
        if(!parent.node.GetSouth())
        {
            if(GetCell(parent.node.x, parent.node.z + 1) != parent.node)
            {
                visitables.Add(new AStarNode(parent.node.x, parent.node.z + 1, GetCell(parent.node.x, parent.node.z + 1), parent));
            }
        }
        if (!parent.node.GetWest())
        {
            if (GetCell(parent.node.x - 1, parent.node.z) != parent.node)
            {
                visitables.Add(new AStarNode(parent.node.x - 1, parent.node.z, GetCell(parent.node.x - 1, parent.node.z), parent));
            }
        }
        if (!parent.node.GetNorth())
        {
            if (GetCell(parent.node.x, parent.node.z - 1) != parent.node)
            {
                visitables.Add(new AStarNode(parent.node.x, parent.node.z - 1, GetCell(parent.node.x, parent.node.z - 1), parent));
            }
        }

        return visitables;
    }
}