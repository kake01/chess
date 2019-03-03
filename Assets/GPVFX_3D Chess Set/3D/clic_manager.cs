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
        Debug.Log(Input.GetMouseButtonDown(0));
        //選択状態じゃなくマウスが押されたら
        if (Input.GetMouseButtonDown(0))
        {
            if (!is_select_time)
            {
                //カメラから光線を飛ばす準備
                Ray is_not_select_ray = new Ray();
                RaycastHit hit = new RaycastHit();
                is_not_select_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
                /*特定のオブジェクトじゃなきゃダメ?*/
                if (Physics.Raycast(is_not_select_ray.origin, is_not_select_ray.direction, out hit, Mathf.Infinity))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    //getしたオブジェクトの座標を取得
                    Vector3 worldDir = hit.transform.position;
                    select_y = -(int)worldDir.z / cell_length;
                    select_x = (int)worldDir.x / cell_length;
                    board_state = board.GetComponent<board>().get_board_2D_array();

                    //クリックmanagerが盤面の情報を引数にして、駒の移動範囲のshowを呼び出す
                    switch (board_state[select_y, select_x]/*board.board_2D_array[select_y, select_x]*/)
                    {
                        case 6:
                            //PAWNが移動出来る場所を表示するメソッドを呼ぶ
                            hit.collider.gameObject.GetComponent<white_pawn>().move_possible_show(board_state, select_x, select_y);
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
            }
            else
            {
//                Debug.Log("選択状態でマウスが押されました");
                //選択状態で移動出来る場所かの検知
                is_select_time = false;
            }
        }
    }

    //click_managerが座標更新をボードに伝えるupdate
    /*
     * 
     */
}