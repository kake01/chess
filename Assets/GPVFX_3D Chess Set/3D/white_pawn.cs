
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn : MonoBehaviour
{

    private int move_length = 125;
    public GameObject plane;


    void Start()
    {
    }
    void Update()
    {
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
        if ((0 <= is_position_x - 1) || (is_position_x + 1 <= 7))
        {
            if ((is_position_y + 1 <= 7 || 0 <= is_position_y - 1) && temp[is_position_y - 1, is_position_x] == 0)
            {
                //引数を使って移動可能の地面作成のメソッドを呼び出す
                //create_plan(,y);
                move_possible_plane(is_position_x, is_position_y - 1);
            }

            //下はpawnが持っている仕事ではなく、generatorが持っている仕事
            //GameObject position_show = GameObject.Find("generator");
            //position_show.transform.position = new Vector3(0, 0, 0);


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
