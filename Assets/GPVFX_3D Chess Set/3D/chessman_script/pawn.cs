
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : MonoBehaviour
{
    private const int cell_mix = 0;
    private const int cell_max = 7;
    private const int move_length = 125;
    private GameObject plane;

    void Start()
    {
        this.plane = GameObject.Find("generator");
    }

    public void chessman_move(int move_x, int move_y)
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(move_length * move_x, 0, move_length * move_y);
    }

    public void move_possible_show(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        if ((cell_mix <= is_position_y - 1) && (is_position_y + 1 <= cell_max))
        {
            //前に駒がなかったら
            if (board_state[is_position_y - 1 * turn, is_position_x] == 0)
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1 * turn);
            //左に駒が合ったら
            if (cell_mix <= is_position_x - 1 && turn * board_state[is_position_y - 1 * turn, is_position_x - 1] < 0)
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y - 1 * turn);
            //右に駒が合ったら
            if (is_position_x + 1 < cell_max && turn * board_state[is_position_y - 1 * turn, is_position_x + 1] < 0)
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1, is_position_y - 1 * turn);
        }
    }
}
