using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*credit to Gregory Dyadichenko
 * https://github.com/Nox7atra/Eller-maze-generator-Unity/blob/master/Assets/Scripts/Maze/Maze.cs
 * 
 */


//can use this to create a class that is a maze of MazeCells

public abstract class Maze<T>
{
    public readonly int ColumnCount;
    public readonly int RowCount;
    protected List<T> _Cells;

    public Maze(int columnCount, int rowCount)
    {
        ColumnCount = columnCount;
        RowCount = rowCount;
    }

    public virtual T GetCell(int x, int y)
    {
        if (x < ColumnCount && z < ColumnCount)
        {
            return _Cells[x + y * ColumnCount];
        }
        else
        {
            return default(T);
        }
    }

}
