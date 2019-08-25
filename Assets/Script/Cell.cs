using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private const int CELL_SIZE = 125;
    int pos_x, pos_y;

    public Cell(int row, int column)
    {
        pos_x = row * CELL_SIZE;
        pos_y = column * CELL_SIZE;
    }
}
