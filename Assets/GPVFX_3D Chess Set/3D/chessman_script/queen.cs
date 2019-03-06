using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queen : MonoBehaviour
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
        //rookと同じ動き
        //上
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (is_position_y - board_num < cell_mix)
            {
                break;
            }
            //移動可能場所に板を設置
            if (cell_mix <= is_position_y - board_num && turn * board_state[is_position_y - board_num, is_position_x] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1 * board_num);
            }
            //敵の駒があったら
            if (cell_mix <= is_position_y - board_num && turn * board_state[is_position_y - board_num, is_position_x] < 0)
            {
                break;
            }
            //自分の駒があったら
            if (turn * board_state[is_position_y - board_num, is_position_x] > 0)
            {
                break;
            }
        }
        //下
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (cell_max < is_position_y + board_num)
            {
                break;
            }

            if (is_position_y + board_num <= cell_max && turn * board_state[is_position_y + board_num, is_position_x] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y + 1 * board_num);
            }
            if (is_position_y + board_num <= cell_max && turn * board_state[is_position_y + board_num, is_position_x] < 0)
            {
                break;
            }
            if (turn * board_state[is_position_y + board_num, is_position_x] > 0)
            {
                break;
            }
        }
        //右
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (cell_max < is_position_x + board_num)
            {
                break;
            }
            if (is_position_x + board_num <= cell_max && turn * board_state[is_position_y, is_position_x + board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1 * board_num, is_position_y);
            }
            if (is_position_x + board_num <= cell_max && turn * board_state[is_position_y, is_position_x + board_num] < 0)
            {
                break;
            }
            if (turn * board_state[is_position_y, is_position_x + board_num] > 0)
            {
                break;
            }
        }
        //左
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (is_position_x - board_num < 0)
            {
                break;
            }
            if (0 <= is_position_x - board_num && turn * board_state[is_position_y, is_position_x - board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1 * board_num, is_position_y);
            }

            if (0 <= is_position_x - board_num && turn * board_state[is_position_y, is_position_x - board_num] < 0)
            {
                break;

            }
            if (turn * board_state[is_position_y, is_position_x - board_num] > 0)
            {
                break;
            }
        }
        //bishopと同じ動き
        //右下
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (cell_max < is_position_y + board_num || cell_max < is_position_x + board_num)
            {
                break;
            }
            //移動可能場所に板を設置
            if (is_position_y + board_num <= cell_max && is_position_x + board_num <= cell_max && turn * board_state[is_position_y + board_num, is_position_x + board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1 * board_num, is_position_y + 1 * board_num);
            }
            //敵の駒があったら
            if (is_position_y + board_num <= cell_max && is_position_x + board_num <= cell_max && turn * board_state[is_position_y + board_num, is_position_x + board_num] < 0)
            {
                break;
            }
            //自分の駒
            if (turn * board_state[is_position_y + board_num, is_position_x + board_num] > 0)
            {
                break;
            }
        }
        //左下
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (cell_max < is_position_y + board_num || is_position_x - board_num < cell_mix)
            {
                break;
            }

            if (is_position_y + board_num <= cell_max && cell_mix <= is_position_x - board_num && turn * board_state[is_position_y + board_num, is_position_x - board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1 * board_num, is_position_y + 1 * board_num);
            }
            if (is_position_y + board_num <= cell_max && cell_mix <= is_position_x - board_num && turn * board_state[is_position_y + board_num, is_position_x - board_num] < 0)
            {
                break;
            }
            if (turn * board_state[is_position_y + board_num, is_position_x - board_num] > 0)
            {
                break;
            }
        }
        //右上
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (is_position_y - board_num < cell_mix || cell_max < is_position_x + board_num)
            {
                break;
            }
            if (cell_mix <= is_position_y - board_num && is_position_x + board_num <= cell_max && turn * board_state[is_position_y - board_num, is_position_x + board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1 * board_num, is_position_y - 1 * board_num);
            }
            if (cell_mix <= is_position_y - board_num && is_position_x + board_num <= cell_max && turn * board_state[is_position_y - board_num, is_position_x + board_num] < 0)
            {
                break;
            }
            if (turn * board_state[is_position_y - board_num, is_position_x + board_num] > 0)
            {
                break;
            }
        }
        //左上
        for (int board_num = 1; board_num <= cell_max; board_num++)
        {
            if (is_position_y - board_num < cell_mix || is_position_x - board_num < cell_mix)
            {
                break;
            }
            if (cell_mix <= is_position_y - board_num && cell_mix <= is_position_x - board_num && turn * board_state[is_position_y - board_num, is_position_x - board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1 * board_num, is_position_y - 1 * board_num);
            }
            if (cell_mix <= is_position_y - board_num && cell_mix <= is_position_x - board_num && turn * board_state[is_position_y - board_num, is_position_x - board_num] < 0)
            {
                break;
            }
            if (turn * board_state[is_position_y - board_num, is_position_x - board_num] > 0)
            {
                break;
            }
        }
    }
}
