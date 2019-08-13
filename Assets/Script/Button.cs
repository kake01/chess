using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Button : MonoBehaviour
{
    /// ボタンをクリックした時の処理
    public void OnClick()
    {
    //    Debug.Log("Button click!");
        Application.Quit();
    }
}
