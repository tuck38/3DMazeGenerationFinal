using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerMaze : MonoBehaviour
{
    public int ColumnCount;
    public int RowCount;
    protected List<MazeCell> _Cells;

    [SerializeField] GameObject cell;

    public EllerMaze(int width, int height)
    {
        ColumnCount = width;
        RowCount = height;
    }

    public void BuildMaze()
    {
        for (int x = 0; x < ColumnCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                GameObject newCell = Instantiate(cell, new Vector3(x, 0, y), Quaternion.identity);
                MazeCell cellComp = newCell.GetComponent<MazeCell>();
                _Cells.Add(cellComp);
            }
        }
    }

    public virtual MazeCell GetCell(int x, int y)
    {
        if (x < ColumnCount && y < ColumnCount)
        {
            return _Cells[x + y * ColumnCount];
        }
        else
        {
            return default(MazeCell);
        }
    }




}
