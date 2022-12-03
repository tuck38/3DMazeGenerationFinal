using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerMaze : MonoBehaviour
{
    public int sideSize;
    protected List<MazeCell> _Cells;

    [SerializeField] GameObject cell = Resources.Load<GameObject>("Prefabs/Cell");



    int wallChance = 50;

    public EllerMaze(int side)
    {
        _Cells = new List<MazeCell>();
        sideSize = side;
    }

    public void BuildMaze()
    {
        for (int x = 0; x < sideSize; x++)
        {
            for (int y = 0; y < sideSize; y++)
            {
                GameObject newCell;
                newCell = Instantiate(cell, new Vector3(x, 0, y), Quaternion.identity);
                MazeCell cellComp = newCell.GetComponent<MazeCell>();
                _Cells.Add(cellComp);
            }
        }

        //for(sideSize) Step();

    }

    public virtual MazeCell GetCell(int x, int y)
    {
        if (x < sideSize && y < sideSize) //if cell is in maze (fix later)
        {
            return _Cells[x + y * sideSize];
        }
        else
        {
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
        int currentRow = 0;
        int maxSet = 1;
        List<int> usedSets;

        for (int count = 0; count <= sideSize; count++)
        {
            usedSets.Clear();

            if(currentRow < 0 || currentRow > sideSize)
            {
                return;
            }

            if(currentRow == 0) //top row
            {


            }

            if(currentRow == sideSize) //bottom row
            {
                for(int i = 0; i < sideSize; i++)
                {
                    SetHorizontal(GetCell(i, currentRow), GetCell(i+1, currentRow), false);
                }
                return;
            }

            for(int i = 0; i <= sideSize; i++) //assigning sets
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
            for(int i = 0; i < sideSize; i++)
            {
                if(GetCell(i, currentRow).GetSet() != GetCell(i + 1, currentRow).GetSet()) //next cell is different set
                {
                    



                }
                else //next cell is same set, put wall
                {


                }







            }






        }


    }


}