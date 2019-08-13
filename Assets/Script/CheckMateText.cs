using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMateText : MonoBehaviour
{
    protected string board_result;

    void Start()
    {
        this.board_result = Board.result;
        this.GetComponent<Text>().text = board_result;
    }
}
