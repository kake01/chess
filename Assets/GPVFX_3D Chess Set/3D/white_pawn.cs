
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn : MonoBehaviour
{
    private int cell_mix = 0;
    private int cell_max = 7;
    private int move_length = 125;
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

    public void move_possible_show(int[,] board_state, int is_position_x, int is_position_y)
    {
        if ((cell_mix <= is_position_y - 1) && (is_position_y + 1 <= cell_max))
        {
            //前に駒がなかったら
            if (board_state[is_position_y - 1, is_position_x] == 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x, is_position_y - 1);
            }
            //左に駒が合ったら
            if (cell_mix <= is_position_x - 1 && board_state[is_position_y - 1, is_position_x - 1] < 0)
            {
                plane.GetComponent<position_generator>().move_possible_plane(is_position_x - 1, is_position_y - 1);
            }
            //右に駒が合ったら
            if (is_position_x + 1 < cell_max && board_state[is_position_y - 1, is_position_x + 1] < 0)
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
