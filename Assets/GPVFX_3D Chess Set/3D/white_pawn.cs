﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn : MonoBehaviour
{
    private int cell_mix = 0;
    private int cell_max = 7;
    private int move_length = 125;
    public GameObject plane;

    void Start()
    {
        this.plane = GameObject.Find("generator");
    }

    public void special_move()
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(0, 0, move_length * 2);
    }
    public void attack_move()
    {
        GetComponent<AudioSource>().Play();
        //右に攻撃
        transform.Translate(move_length, 0, move_length);
        //左に攻撃
        transform.Translate(-move_length, 0, move_length);
    }
    public void normal_move()
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(0, 0, move_length);
    }

    public void move_possible_show(int[,] temp, int is_position_x, int is_position_y)
    {
        if ((cell_mix <= is_position_y - 1) || (is_position_y + 1 <= cell_max))
        {
            //前に駒がなかったら
            if (temp[is_position_y - 1, is_position_x] == 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1);
            }
            //左に駒が合ったら
            if (cell_mix <= is_position_x - 1 && temp[is_position_y - 1, is_position_x - 1] < 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y - 1);
            }
            //右に駒が合ったら
            if (is_position_x + 1 < cell_max && temp[is_position_y - 1, is_position_x + 1] < 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x + 1, is_position_y - 1);
            }
        }
    }
}

//選択状態で移動出来る場所かの検知
//カメラから光線を飛ばす準備
//Ray ray = new Ray();
//RaycastHit hit = new RaycastHit();
//ray = Camera.main.ScreenPointToRay(Input.mousePosition);
/*
 * 移動する場合の座標変更
 * board_2D_array[select_y, select_x] = 0;
 * board_2D_array[select_y - 1, select_x] = 6;
 * hit.collider.gameObject.GetComponent<white_pawn>().normal_move();
 * is_select_time = false;
 */
