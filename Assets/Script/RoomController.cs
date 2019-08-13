using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private AudioSource bgm;
    protected PhotonView photonView;
    protected GameObject timer_controller;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(null);
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;
        this.photonView = GetComponent<PhotonView>();
        this.timer_controller = GameObject.Find("Text");
    }

    void OnJoinedLobby()
    {
        // ルームに入室する
        PhotonNetwork.JoinRandomRoom();
    }

    // ルームの入室に失敗すると呼ばれる
    void OnPhotonRandomJoinFailed()
    {
        // ルームがないと入室に失敗するため、その時は自分で作る
        // 引数でルーム名を指定できる
        PhotonNetwork.CreateRoom("myRoomName");
    }

    //他のplayerが部屋に入った際実行される
    public void OnPhotonPlayerConnected()
    {
        photonView.RPC("StartMusic", PhotonTargets.MasterClient);
        bgm.Play();
    }

    [PunRPC]
    public void StartMusic() => timer_controller.gameObject.GetComponent<Timer>().TimerReset();
}