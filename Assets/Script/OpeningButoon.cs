using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningButoon : MonoBehaviour
{
    public void OnClick() => SceneManager.LoadScene("MainScene");
}
