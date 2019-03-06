using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : MonoBehaviour
{
    private const int cell_mix = 0;
    private const int cell_max = 7;
    private const int move_length = 125;
    private GameObject plane;

    void Start()
    {
        this.plane = GameObject.Find("generator");
    }

    public void chessman_move(int move_x, int move_z)
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(move_length * move_z, 0, move_length * move_x);
    }

    public void move_possible_show(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        //右上
        //右上1
        if ((cell_mix <= is_position_y - 2 || is_position_y + 2 <= cell_max) && (cell_mix <= is_position_x - 1 || is_position_x + 1 <= cell_max))
        {
            if (turn * board_state[is_position_y - 2 * turn, is_position_x + 1 * turn] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1 * turn, is_position_y - 2 * turn);

            }
        }

        //右上2


        //右下
        //左上
        //左下
    }
}
/*前に駒がなかったら
 * if (board_state[is_position_y - 1 * turn, is_position_x] == 0)
 * plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1 * turn);
 * */
