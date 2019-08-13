using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    protected int player_id;
    private const int WHITE_PLAYEY = 1;
    private const int BLACK_PLAYER = 2;
    void OnJoinedRoom()
    {
        player_id = PhotonNetwork.player.ID;
        if (player_id == BLACK_PLAYER)
            this.transform.Rotate(0, 0, 180f);
    }
}
