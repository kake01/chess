using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    private const int PLAYER_WHITE = 1;
    private const int PLAYER_BLACK = 2;
    private const int WHITE_CHESSMAN = 1;
    private const int BLACK_CHESSMAN = -1;
    private const int NUM_NOT_CHESSMAN = 0;
    private const int TURN_CHENGE = -1;
    public int player_id;
    private const int CELL_SIZE = 125;
    protected int[,] board_2D_array =
    {
            {BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN,BLACK_CHESSMAN, BLACK_KING, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN},
            {BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN},
            {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
            {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
            {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
            {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
            {WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN},
            {WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_KING, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN}
    };

    protected PhotonView photonView;
    protected GameObject[] chessman_list = new GameObject[16];
    private List<Vector2> display_chessman;
    private string[] chessman_type = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight", "rook" };
    public Cell[,] cells = new Cell[8, 8];


    //boardが持ってるのはおかしい
    public int turn = 1;// 1が白で-1が黒のturn
    protected static string winner = "あなたの勝ちです";
    protected static string loser = "あなたの負けです。";
    protected GameObject particle;
    protected AudioSource move_sound;
    public static string result;
    protected GameObject timer;
    //不要になる可能性がある
    private const int WHITE_KING = 2;
    private const int BLACK_KING = -2;


    Board()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                cells[i, j] = new Cell(i, j);
    }


    public void Start()
    {
        this.timer = GameObject.Find("Text");
        this.particle = GameObject.Find("Particle");
        this.photonView = GetComponent<PhotonView>();
        this.move_sound = GetComponent<AudioSource>();
    }

    public void ChessmanMoveShow(RaycastHit first_hit, int pos_x, int pos_y)
    {
        pos_y = -pos_y / CELL_SIZE;
        pos_x = pos_x / CELL_SIZE;
        if (board_2D_array[pos_y, pos_x] * turn > NUM_NOT_CHESSMAN)
            first_hit.collider.gameObject.GetComponent<Chessman>().MovePossibleShow(board_2D_array, pos_x, pos_y, turn);
    }

    public void ChessmanMove(RaycastHit first_hit, int move_pos_x, int move_pos_y, int select_x, int select_y)
    {
        move_pos_y = -(int)move_pos_y / CELL_SIZE;
        move_pos_x = (int)move_pos_x / CELL_SIZE;
        select_y = -select_y / CELL_SIZE;
        select_x = select_x / CELL_SIZE;

        //相手に伝える為の仮変数
        Vector2 before_chessman_cell = new Vector2(select_x, select_y);
        Vector2 after_chessman_cell = new Vector2(move_pos_x, move_pos_y);
        photonView.RPC("MoveSound", PhotonTargets.MasterClient);

        //相手の駒が合ったら削除する
        if (board_2D_array[move_pos_y, move_pos_x] != NUM_NOT_CHESSMAN)
        {
            //キングの場合はゲームオーバー
            if (board_2D_array[move_pos_y, move_pos_x] == WHITE_KING)
                photonView.RPC("Result", PhotonTargets.All, WHITE_KING);
            if (board_2D_array[move_pos_y, move_pos_x] == BLACK_KING)
                photonView.RPC("Result", PhotonTargets.All, BLACK_KING);

            photonView.RPC("ParticleOccurrence", PhotonTargets.All, after_chessman_cell);
            photonView.RPC("ChessmanDestroy", PhotonTargets.Others, after_chessman_cell);
        }

        //移動の描画
        first_hit.collider.gameObject.GetComponent<Chessman>().ChessmanMove(move_pos_x, move_pos_y);
        //boardの更新
        photonView.RPC("BoardUpdate", PhotonTargets.All, before_chessman_cell, after_chessman_cell);

        //自分のディスプレイの駒の描画処理
        DrawingUpdate();
        //turnの更新
        photonView.RPC("TurnUpdate", PhotonTargets.All);
        //相手のディスプレイの駒の描画処理
        photonView.RPC("DrawingUpdate", PhotonTargets.Others);

        //自分のタイマーを止め,相手のタイマーを開始させる
        timer.gameObject.GetComponent<Timer>().TimerUpdate();
    }

    [PunRPC]
    protected void MoveSound() => move_sound.Play();

    [PunRPC]
    public void DrawingUpdate()
    {
        //敵の全駒の描画削除
        photonView.RPC("HideChessman", PhotonTargets.Others);

        //自分の駒の数だけ
        foreach (GameObject Obj in this.chessman_list)
        {
            if (Obj != null)
            {
                //一つの駒に対する敵の駒の発見した駒の値が入る
                display_chessman = Obj.gameObject.GetComponent<Chessman>().FindChessman(turn, board_2D_array);
                //その場所に該当する駒の描画
                for (int i = 0; i < display_chessman.Count; i++)
                    photonView.RPC("ShowChessman", PhotonTargets.Others, display_chessman[i]);
            }
        }
    }
    //見える駒の描画を行う
    [PunRPC]
    public void ShowChessman(Vector2 disp_pos)
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.GetComponent<Chessman>().DisplayRenderer(disp_pos);
    }
    //相手の駒の描画を全て消す
    [PunRPC]
    public void HideChessman()
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.GetComponent<Chessman>().NoneRenderer();
    }
    [PunRPC]
    void BoardUpdate(Vector2 before_board_array, Vector2 after_board_array)
    {
        this.board_2D_array[(int)after_board_array.y, (int)after_board_array.x] = board_2D_array[(int)before_board_array.y, (int)before_board_array.x];
        this.board_2D_array[(int)before_board_array.y, (int)before_board_array.x] = NUM_NOT_CHESSMAN;
    }
    [PunRPC]
    public void ChessmanDestroy(Vector2 destroy_pos)
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.gameObject.GetComponent<Chessman>().ChessmanDestroy((int)destroy_pos.x, (int)destroy_pos.y);
    }
    [PunRPC]
    public void ParticleOccurrence(Vector2 occurrence_cell) => particle.GetComponent<Particle>().Occurrence(occurrence_cell);
    [PunRPC]
    public void TurnUpdate() => this.turn *= TURN_CHENGE;
    [PunRPC]
    public void Result(int king_name)
    {
        //黒kingが取られた場合
        if (king_name == BLACK_KING)
        {
            if (player_id == PLAYER_WHITE)
                result = winner;
            if (player_id == PLAYER_BLACK)
                result = loser;
        }
        //白kingが取られた場合
        if (king_name == WHITE_KING)
        {
            if (player_id == PLAYER_WHITE)
                result = loser;
            if (player_id == PLAYER_BLACK)
                result = winner;
        }
        SceneManager.LoadScene("CheckMateScene");
    }

    /* turnの変化をタイマーで使う部分 */
    public void TurnChange() => photonView.RPC("TurnUpdate", PhotonTargets.All);

    public void OnJoinedRoom()
    {
        player_id = PhotonNetwork.player.ID;
        //白駒の生成
        if (PLAYER_WHITE == player_id)
        {
            CreatePawnArmy("white_", 6);
            CreateStrongArmy("white_", 7);
        }
        //黒駒の生成
        if (PLAYER_BLACK == player_id)
        {
            CreatePawnArmy("black_", 1);
            CreateStrongArmy("black_", 0);
        }
    }


    private void CreateChessman(string color, string type, int chessman_num, int pos_x, int pos_y)
    {
        Vector3 pos = new Vector3(pos_x * CELL_SIZE, 0, -CELL_SIZE * pos_y);
        chessman_list[chessman_num] = PhotonNetwork.Instantiate(color + type, pos, this.transform.rotation, 0);
        chessman_list[chessman_num].gameObject.GetComponent<Renderer>().enabled = true;
        chessman_list[chessman_num].gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    private void CreatePawnArmy(string chessman_color, int row)
    {
        for (int i = 0; i < 8; i++)
        {
            CreateChessman(chessman_color, "pawn", i, i, row);
        }
    }
    private void CreateStrongArmy(string chessman_color, int row)
    {
        for (int i = 0; i < 8; i++)
        {
            CreateChessman(chessman_color, chessman_type[i], 8 + i, i, row);
            //kingのみ描画を行う
            if (chessman_type[i] == "king")
                chessman_list[8 + i].gameObject.GetComponent<Renderer>().enabled = true;
        }
    }

    //強制終了
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.Quit();
    }
}

/*
 * 【Chessman①②③】


⑤Boardクラスに、CellにChessmanを生成する処理を追加します。
　Boardクラスのコンストラクタで、Cell毎にChessmanのインスタンスを生成して、Cellに渡します。
　Chessmanクラスのコンストラクタでは、黒か白かを判定する値を受け取り、保持します。
　Chessmanのインスタンスは、予め初期配置配列なりで作っておくと良いでしょう。
　Cellクラスでは、受け取ったChessmanのインスタンスを保持して、Chessmanに自身の描画座標を渡します。
　以降は、コマの移動がある度に、Cellに自身の描画座標を渡すようにさせます。
　また、黒か白か・駒種等は、BoardからCellに問い合わせ、CellからChessmanに問い合わせて、その結果で判定するようにします。
　これで、ChessmanがCELL_LENGTHを持つ必要が無くなったので、消えました。


⑥Cellクラスに、セル座標の割り出し処理を追加します。
　ClickManagerから、クリックしたセルを割り出すため、⑤の逆で、渡された値をCELL_SIZEで割る処理を追加します。
⑦Chessmanクラスから、移動ベクトル・色・駒種を取得します。
　Board>Cell>Chessmanの順に問い合わせて、最終的に結果がBoardに帰ります。
　次に、Boardクラスに移動可能場所を探す処理を追加します。帰って来た、移動ベクトルを使って、順々に調べていきます。
　この方法なら、Boardの状態をChessmanに知らせる必要がありませんし、ループ回数も1回で済みます。
⑧Boardクラスに、Cellが移動可能かを判定する処理を追加します。
　Board>Cell>Chessmanの順に問い合わせて、ClickManagerで示したCellのChessmanが、プレイヤーのものか調べます。
　次に、先程の⑥の処理を行います。調べる条件は、「盤上の範囲内であること」「駒がないこと」です。
　盤上の範囲は、当然Boardが知っていますし、Board>Cellの順に問い合わせれば、Chessmanがあるかどうかも分かります。
　これで、CELL_MIN・CELL_MAX・NUM_NOT_CHESSMANが消えました。
⑨Cellクラスに、Planeを置く処理を移行します。Boardからその処理が呼び出されたら、自身のPlaneを有効化します。
⑩Boardクラスに、ChessmanのCell移動処理を追加します。
　ClickManagerで移動先を指定したら、⑥の処理でCellを割り出して、移動元のChessmanを移行します。
　Cellに全てのPlaneを解除させて、ターン終了になります。
以上で、解決するはずです。
 */
