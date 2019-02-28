using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position_generator : MonoBehaviour
{
    public GameObject move_position_prefab;
    public GameObject is_select_chessman;

    void Start()
    {
    }

    void Update()
    {
    }

    public void show()
    {
        is_select_chessman = GameObject.Find("generator");
        Debug.Log(is_select_chessman.GetComponent<clic_manager>().cell_length);

        /*
        //配列が必要な場合
        GameObject[] position_show = new GameObject[5];
        for (var i = 0; i < 3; i++)
        {
            position_show[i] = Instantiate(move_position_prefab) as GameObject;
            position_show[i].transform.position = new Vector3(125 * i, 0, -625);
            //Debug.Log(this.position_show);
        }
                 */
    }


    public void destroy()
    {
        //デストロイしてくれ
        Destroy(gameObject);
        Debug.Log("オブジェクトが消せません");
    }
}
