using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    protected RaycastHit first_hit, second_hit;
    protected Ray first_ray, second_ray;
    protected GameObject board;
    private bool is_select_time = false;
    //protected int select_x, select_y, move_pos_x, move_pos_y;

    private void Start()
    {
        this.board = GameObject.Find("Board");
        this.first_ray = new Ray();
        this.second_ray = new Ray();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!is_select_time)
                IsNotSelectTime();
            else
                IsSelectTime();
        }
    }

    public void IsNotSelectTime()
    {
        first_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        first_hit = new RaycastHit();
        //マウスクリックした場所にRayを飛ばし、オブジェクトがあるか
        if (Physics.Raycast(first_ray.origin, first_ray.direction, out first_hit, Mathf.Infinity))
        {
            board.gameObject.GetComponent<Board>().ChessmanMoveShow(first_hit, (int)first_hit.transform.position.x, (int)first_hit.transform.position.z);
            is_select_time = true;
        }
    }

    public void IsSelectTime()
    {
        second_hit = new RaycastHit();
        second_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //マウスをクリックしその場所に板が生成されていたら
        if (Physics.Raycast(second_ray.origin, second_ray.direction, out second_hit, Mathf.Infinity) && second_hit.collider.tag == "TargetTag")
            board.gameObject.GetComponent<Board>().ChessmanMove(first_hit, (int)second_hit.transform.position.x, (int)second_hit.transform.position.z, (int)first_hit.transform.position.x, (int)first_hit.transform.position.z);

        //選択状態での変更を戻す
        first_hit.collider.gameObject.GetComponent<Chessman>().PlaneDestroy();
        first_hit.collider.gameObject.GetComponent<Chessman>().ResetColor();
        first_hit.collider.gameObject.GetComponent<BoxCollider>().enabled = true;
        is_select_time = false;
    }
}