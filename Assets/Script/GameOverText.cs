using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    protected string timer_result;

    private void Start()
    {
        this.timer_result = Timer.result;
        this.GetComponent<Text>().text = timer_result;
    }
}