using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : MonoBehaviour
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
        //下右
        if (is_position_x + 1 <= cell_max && is_position_y + 2 <= cell_max && turn * board_state[is_position_y + 2, is_position_x + 1] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1, is_position_y + 2);
        }
        //下左
        if (cell_min <= is_position_x - 1 && is_position_y + 2 <= cell_max && turn * board_state[is_position_y + 2, is_position_x - 1] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y + 2);
        }
        //上右
        if (is_position_x + 1 <= cell_max && cell_min <= is_position_y - 2 && turn * board_state[is_position_y - 2, is_position_x + 1] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1, is_position_y - 2);
        }
        //上左
        if (cell_min <= is_position_x - 1 && cell_min <= is_position_y - 2 && turn * board_state[is_position_y - 2, is_position_x - 1] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y - 2);
        }
        //右上
        if (is_position_x + 2 <= cell_max && cell_min <= is_position_y - 1 && turn * board_state[is_position_y - 1, is_position_x + 2] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 2, is_position_y - 1);
        }
        //右下
        if (is_position_x + 2 <= cell_max && is_position_y + 1 <= cell_max && turn * board_state[is_position_y + 1, is_position_x + 2] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 2, is_position_y + 1);
        }
        //左上
        if (cell_min <= is_position_x - 2 && cell_min <= is_position_y - 1 && turn * board_state[is_position_y - 1, is_position_x - 2] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 2, is_position_y - 1);
        }
        //左下
        if (cell_min <= is_position_x - 2 && is_position_y + 1 <= cell_max && turn * board_state[is_position_y + 1, is_position_x - 2] <= 0)
        {
            plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 2, is_position_y + 1);
        }
    }
}