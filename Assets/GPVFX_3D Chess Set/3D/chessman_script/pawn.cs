
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : MonoBehaviour
{
    private const int cell_min = 0;
    private const int cell_max = 7;
    private const int move_length = 125;
    private bool first_move = true;
    private GameObject plane;

    void Start()
    {
        this.plane = GameObject.Find("generator");
    }

    public void chessman_move(int move_x, int move_z)
    {
        GetComponent<AudioSource>().Play();
        this.transform.position = new Vector3(move_length * move_x, 0, -move_length * move_z);
        first_move = false;
    }

    public void move_possible_show(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        //初動のみ有効
        if (first_move && board_state[is_position_y - 2 * turn, is_position_x] == 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 2 * turn);
        }

        if ((cell_min <= is_position_y - 1) && (is_position_y + 1 <= cell_max))
        {
            //前に駒がなかったら
            if (board_state[is_position_y - 1 * turn, is_position_x] == 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1 * turn);
            }
            //左に駒が合ったら
            if (cell_min <= is_position_x - 1 && turn * board_state[is_position_y - 1 * turn, is_position_x - 1] < 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y - 1 * turn);
            }
            //右に駒が合ったら
            if (is_position_x + 1 < cell_max && turn * board_state[is_position_y - 1 * turn, is_position_x + 1] < 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1, is_position_y - 1 * turn);
            }
        }
    }
}