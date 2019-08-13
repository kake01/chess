using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float holding_time = 30f;
    private float count_time = 0;
    private float remaining_time;
    private int penalty_num = 0;
    private bool timer_trigger = false;
    private const string IS_NOT_MY_TURN = "相手のターンです";
    private const string TIME_OVER_WINNER = "相手の持ち時間が切れた為、あなたの勝利です。";
    private const string TIME_OVER_LOSER = "あなたの持ち時間が切れた為、あなたの負けです。";
    public static string result;
    public GameObject board;
    protected PhotonView photonView;

    void Start()
    {
        this.photonView = GetComponent<PhotonView>();
        this.board = GameObject.Find("Board");
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //自分のターンになったら
        if (timer_trigger)
        {
            //タイマーを始動させる
            count_time += Time.deltaTime;

            remaining_time = holding_time - count_time;
            this.GetComponent<Text>().text = remaining_time.ToString("F2");

            //タイムアウトになったら
            if (remaining_time < 0)
            {
                penalty_num++;//ペナルティを一回増やす

                //ペナルティの数に応じて時間を減らす
                if (penalty_num == 1 || penalty_num == 2)
                    holding_time -= 5f;
                if (penalty_num == 3 || penalty_num == 4)
                    holding_time -= 10f;
                //ペナルティを5回行ったら負けの処理をここに書く
                if (penalty_num == 5)
                {
                    //相手に相手が勝利した事を送る
                    photonView.RPC("Result", PhotonTargets.Others);
                    //自分が負けた処理
                    result = TIME_OVER_LOSER;
                    SceneManager.LoadScene("GameOverScene");
                }
                //相手にターンを渡す
                board.gameObject.GetComponent<Board>().TurnChange();
                photonView.RPC("TimerReset", PhotonTargets.Others);
                timer_trigger = false;
            }
        }
        else
            this.GetComponent<Text>().text = IS_NOT_MY_TURN;
    }

    [PunRPC]
    public void TimerReset()
    {
        this.timer_trigger = true;
        count_time = 0;
    }
    [PunRPC]
    public void Result()
    {
        result = TIME_OVER_WINNER;
        SceneManager.LoadScene("GameOverScene");
    }
    //自分のタイマーを止めて相手のタイマーを進める
    public void TimerUpdate()
    {
        photonView.RPC("TimerReset", PhotonTargets.Others);
        this.timer_trigger = false;
    }
}