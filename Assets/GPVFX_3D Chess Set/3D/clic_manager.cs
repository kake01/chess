using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clic_manager : MonoBehaviour
{
    //private bool turn = true;
    //こいつここ?
    public bool is_select_time = false;
    private int cell_length = 125;
    public int select_x, select_y;
    public GameObject board;
    public int[,] board_state;

    void Start()
    {
        this.board = GameObject.Find("board");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //カメラから光線を飛ばす準備
            Ray first_ray = new Ray();
            first_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit first_hit = new RaycastHit();

            //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
            if (Physics.Raycast(first_ray.origin, first_ray.direction, out first_hit, Mathf.Infinity))
            {
                //getしたオブジェクトの座標を取得
                Vector3 worldDir = first_hit.transform.position;
                select_y = -(int)worldDir.z / cell_length;
                select_x = (int)worldDir.x / cell_length;
                board_state = board.GetComponent<board>().get_board_2D_array();

                if (!is_select_time)
                {
                    first_hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    //クリックmanagerが盤面の情報を引数にして、駒の移動範囲のshowを呼び出す
                    switch (board_state[select_y, select_x])
                    {
                        case 6:
                            //PAWNが移動出来る場所を表示するメソッドを呼ぶ
                            first_hit.collider.gameObject.GetComponent<white_pawn>().move_possible_show(board_state, select_x, select_y);
                            break;
                        case 1:
                            //ROOKが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case 2:
                            //KNIGHTが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case 3:
                            //BISHOPが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case 4:
                            //QUEENが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case 5:
                            //KINGが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        default:
                            break;
                    }
                    is_select_time = true;
                }
                else
                {
                    //カメラから光線を飛ばす準備
                    Ray second_ray = new Ray();
                    RaycastHit second_hit = new RaycastHit();
                    second_ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                    //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
                    if (Physics.Raycast(second_ray.origin, second_ray.direction, out second_hit, Mathf.Infinity))
                    {
                        //当たったオブジェクトがplanなら
                        /*if (second_hit.collider.tag == "play"){}*/

                        //getしたオブジェクトの座標を取得
                        Vector3 worldDir = second_hit.transform.position;
                        select_y = -(int)worldDir.z / cell_length;
                        select_x = (int)worldDir.x / cell_length;



                        //もし、移動可能場所に触ったら
                        switch (board_state[select_y, select_x])
                        {
                            case 6:
                                first_hit.collider.gameObject.GetComponent<white_pawn>().chessman_move(-1/*移動可能場所のx座標*/, 1/*移動可能場所のy座標*/);
                                Debug.Log("ここにいきたよ");
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                break;
                            default:
                                break;
                        }
                        //色を戻す手段をここに書く
                    }
                    is_select_time = false;
                }
            }

            /*
             * 追加する内容
             * ray2を実装
             * ray2が黄色いマットだった場合、その場所の座標を取得し、駒のメソッドを呼ぶ呼ぶ
             */

            //選択状態で移動出来る場所かの検知
        }
    }
}

//click_managerが座標更新をボードに伝えるupdate
/*
 * 
 */
