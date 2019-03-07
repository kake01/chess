using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : MonoBehaviour
{
    private const int cell_min = 0;
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
        this.transform.position = new Vector3(move_length * move_x, 0, -move_length * move_z);
    }

    public void move_possible_show(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
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
            if (cell_max < is_position_y + board_num || is_position_x - board_num < cell_min)
            {
                break;
            }

            if (is_position_y + board_num <= cell_max && cell_min <= is_position_x - board_num && turn * board_state[is_position_y + board_num, is_position_x - board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1 * board_num, is_position_y + 1 * board_num);
            }
            if (is_position_y + board_num <= cell_max && cell_min <= is_position_x - board_num && turn * board_state[is_position_y + board_num, is_position_x - board_num] < 0)
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
            if (is_position_y - board_num < cell_min || cell_max < is_position_x + board_num)
            {
                break;
            }
            if (cell_min <= is_position_y - board_num && is_position_x + board_num <= cell_max && turn * board_state[is_position_y - board_num, is_position_x + board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1 * board_num, is_position_y - 1 * board_num);
            }
            if (cell_min <= is_position_y - board_num && is_position_x + board_num <= cell_max && turn * board_state[is_position_y - board_num, is_position_x + board_num] < 0)
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
            if (is_position_y - board_num < cell_min || is_position_x - board_num < cell_min)
            {
                break;
            }
            if (cell_min <= is_position_y - board_num && cell_min <= is_position_x - board_num && turn * board_state[is_position_y - board_num, is_position_x - board_num] <= 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1 * board_num, is_position_y - 1 * board_num);
            }
            if (cell_min <= is_position_y - board_num && cell_min <= is_position_x - board_num && turn * board_state[is_position_y - board_num, is_position_x - board_num] < 0)
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
