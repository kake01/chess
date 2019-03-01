
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn : MonoBehaviour
{
    private int move_length = 125;

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
    public void move_possible_show(int[,] temp, int x, int y)
    {
        /*
if (board_2D_array[select_y - 1, select_x] == 0)
{
    //移動出来る場所の表示

    GameObject position_show = Instantiate(move_position_prefab) as GameObject;
    //                        position_show[i] = Instantiate(move_position_prefab) as GameObject;
    position_show[i].transform.position = new Vector3(125 * i, 0, -625);

    */
        /*移動する場合の座標変更
         * board_2D_array[select_y, select_x] = 0;
         * board_2D_array[select_y - 1, select_x] = 6;
         * hit.collider.gameObject.GetComponent<white_pawn>().normal_move();
         * is_select_time = false;
         */

    }
}







//選択状態で移動出来る場所かの検知
//カメラから光線を飛ばす準備
//Ray ray = new Ray();
//RaycastHit hit = new RaycastHit();
//ray = Camera.main.ScreenPointToRay(Input.mousePosition);