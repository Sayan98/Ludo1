using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public struct WinningStatus
{
    public int Red;
    public int Blue;
    public int Yellow;
    public int Green;

}
public class Dice : MonoBehaviourPun
{
    public enum Tokens
    {
        Blue,
        Red,
        Yellow,
        Green
    }

    public Tokens ActiveToken;
    public Tokens LocalToken;
    public int DiceNumber;
    public Text diceNumberDisplay;
    [HideInInspector] public int PreviousValue;
    public Button DiceButton;
    public bool snapCam;

    Vector3 camlerppos;
    Vector3 camlerprot;

    Color turncolorl;
    public Camera cam;


    public Material board;
    public static Dice Instance;

    public WinningStatus _winningStatus;
    public int _PlayersFinished = 0;
    public TextMeshProUGUI PlayerWonDisplay;


    //AI
    Ai_initialize _Initialize;
    Ai_movement _Movement;
    bool all_in = true;

    private string debugText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ai
        if (GameObject.Find("Ai_controller(initialize)") != null)
        {

            _Initialize = Ai_initialize.Instance;
            _Movement = Ai_movement.Instance;
        }

        turncolorl = new Color(0.6f, 0.6f, 0.6f);
        cam = GameObject.FindObjectOfType<Camera>();
        Transform DiceDisplay = GameObject.FindGameObjectWithTag("DiceDisplay").transform;
        DiceButton = DiceDisplay.GetChild(0).GetComponent<Button>();
        //RefreshPlayer();
    }
    public void RefreshPlayer()
    {
        photonView.RPC("RPC_SetBeginGame", RpcTarget.AllBuffered);
        LocalToken = NetworkManager.Instance.LocalPlayerToken;

        Debug.LogError("Current Active Token is ::::: " + ActiveToken);
        SetCamera();
        //photonView.RPC("UpdateTurn", RpcTarget.AllBuffered, ActiveToken, turncolorl);
        UpdateTurn();
    }
    [PunRPC]
    void RPC_SetBeginGame()
    {
        var _customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (_customProperties["MasterPlayerToken"] != null)
        {
            int MasterToken = (int)_customProperties["MasterPlayerToken"];
            ActiveToken = (Tokens)MasterToken;

        }
    }
    public void SetCamera()
    {
        switch (LocalToken)
        {
            case Tokens.Green:
                camlerppos = new Vector3(-1, 28, 2);
                camlerprot = new Vector3(50, 135, 0);
                cam.backgroundColor = Color.green;
                break;

            case Tokens.Yellow:
                camlerppos = new Vector3(52, 28, -1);
                camlerprot = new Vector3(50, 225, 0);
                cam.backgroundColor = Color.yellow;
                break;

            case Tokens.Blue:
                camlerppos = new Vector3(52, 28, -52);
                camlerprot = new Vector3(50, 315, 0);
                cam.backgroundColor = Color.blue;
                break;

            case Tokens.Red:
                camlerppos = new Vector3(-4, 28, -52);
                camlerprot = new Vector3(50, 45, 0);
                cam.backgroundColor = Color.red;
                break;
        }
        switch (ActiveToken)
        {
            case Tokens.Red:
                turncolorl = Color.red;
                break;

            case Tokens.Green:
                turncolorl = Color.green;
                break;

            case Tokens.Yellow:
                turncolorl = Color.yellow;
                break;

            case Tokens.Blue:
                turncolorl = Color.blue;
                break;
        }
        cam.transform.position = camlerppos;
        cam.transform.eulerAngles = camlerprot;
        Debug.Log("SET CAMERA");
    }
    // Update is called once per frame
    /*void Update()
    {
        //diceNumberDisplay.text = "Dice " + DiceNumber;
        // SmoothTurnTransform();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateRandomNumber(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GenerateRandomNumber(2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateRandomNumber(3);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRandomNumber(4);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GenerateRandomNumber(5);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GenerateRandomNumber(6);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(UpdateCurrentToken());
        }
    }*/
    public void Replay()
    {
        NetworkManager.Instance.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");

    }
    //[PunRPC]
    void UpdateTurn()
    {
        ColorBlock DiceButtonColor = DiceButton.colors;
        DiceButtonColor.normalColor = turncolorl;
        DiceButtonColor.highlightedColor = turncolorl;
        DiceButtonColor.disabledColor = new Color(turncolorl.r, turncolorl.g, turncolorl.b, .75f);
        DiceButton.colors = DiceButtonColor;
        DiceButton.interactable = (LocalToken == ActiveToken);
    }
    public void GenerateRandomNumber(int numb = 0)
    {
        Debug.LogError("GenerateRandomNumber");

        DiceNumber = Random.Range(1, 7);
        photonView.RPC("RPC_DisplayDiceNumber", RpcTarget.AllBuffered, DiceNumber);
        PreviousValue = DiceNumber;
        /*if(GameManager.Instance.LocalPlayer.AllTokensInHouse && DiceNumber < 6)
        {            
            StartCoroutine(UpdateCurrentToken());
        }*/ // replacement below
        Debug.Log("brfore Check_All_In");
            Check_All_In();

    }

    public void Check_All_In()
    {

        all_in = true;
        for (int i = 0; i < 4; i++)
        {
            if (GameObject.Find(ActiveToken.ToString() + "(Clone)").transform.GetChild(i).GetComponent<Follower>().PawnoutOfTheHouse == true)
                all_in = false;
        }
        Debug.Log("all in "+all_in + " active "+ActiveToken+ "before switch");
        switch (ActiveToken)
        {

            case Tokens.Blue:
                if (_Initialize.token_blue == true && all_in == true && DiceNumber < 6) {
                    Debug.Log("in switch active "+ActiveToken);
                    StartCoroutine(UpdateCurrentToken());
                }
                    break;

            case Tokens.Yellow:
                if (_Initialize.token_yellow == true && all_in == true && DiceNumber < 6) {
                    Debug.Log("in switch active "+ActiveToken);
                    StartCoroutine(UpdateCurrentToken());
                }
                    break;

            case Tokens.Red:
                if (_Initialize.token_red == true && all_in == true && DiceNumber < 6) {
                    Debug.Log("in switch active "+ActiveToken);
                    StartCoroutine(UpdateCurrentToken());
                }
                    break;

            case Tokens.Green:
                if (_Initialize.token_green == true && all_in == true && DiceNumber < 6) {
                    Debug.Log("in switch active "+ActiveToken);
                    StartCoroutine(UpdateCurrentToken());
                }
                    break;
        }

    }

    public void CheckPlayerCollision()
    {
        photonView.RPC("RPC_CheckPlayerCollision", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void RPC_CheckPlayerCollision()
    {
        foreach (Player playerPrefab in FindObjectsOfType<Player>())
        {
            Debug.LogError("FindObjectsOfType" + playerPrefab.name);
            for (int i = 0; i < 4; i++)
                playerPrefab.transform.GetChild(i).GetComponent<Follower>().CheckPlayerCollision();
        }
    }
    [PunRPC]
    public void RPC_DisplayDiceNumber(int number)
    {
        diceNumberDisplay.text = number.ToString();
        Debug.LogError("RPC_DisplayDiceNumber" + number);
    }
    public IEnumerator UpdateCurrentToken()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("call from active "+ActiveToken);
        Debug.LogError("RPC_CheckForTurn***********************");
        photonView.RPC("RPC_CheckForTurn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_CheckForTurn()
    {

        switch (ActiveToken)
        {
            case Tokens.Red:
                ActiveToken = Tokens.Green;
                turncolorl = Color.green;
                break;

            case Tokens.Green:
                ActiveToken = Tokens.Yellow;
                turncolorl = Color.yellow;
                break;

            case Tokens.Yellow:
                ActiveToken = Tokens.Blue;
                turncolorl = Color.blue;
                break;

            case Tokens.Blue:
                ActiveToken = Tokens.Red;
                turncolorl = Color.red;
                break;
        }
        Debug.LogError(ActiveToken + "'s Turn");

        UpdateTurn();

        //AI
        //if(PhotonNetwork.IsMasterClient) {
        if(_Initialize.room_owner == true) {
            Debug.Log("master client");
            Check_Ai_Turn();
        }
        else
            Debug.Log("not master");

    }

    public void Check_Ai_Turn()
    {

        switch (ActiveToken)
        {
            case Tokens.Blue:
                if (_Initialize.token_blue == false)
                {
                    RollAndMoveforAI("Blue");
                }
                break;

            case Tokens.Yellow:
                if (_Initialize.token_yellow == false)
                {
                    RollAndMoveforAI("Yellow");
                }
                break;

            case Tokens.Red:
                if (_Initialize.token_red == false)
                {
                    RollAndMoveforAI("Red");
                }
                break;

            case Tokens.Green:
                if (_Initialize.token_green == false)
                {
                    RollAndMoveforAI("Green");
                }
                break;
        }

    }

    public void CallWinUpdateRPC(int ReachedToken)
    {
        photonView.RPC("RPC_updateWinningStatus", RpcTarget.AllBuffered, ReachedToken);
    }

    [PunRPC]
    public void RPC_updateWinningStatus(int ReachedToken)
    {
        Tokens _reachedToken = (Tokens)ReachedToken;
        switch (_reachedToken)
        {
            case Tokens.Blue:
                _winningStatus.Blue += 1;
                if (_winningStatus.Blue >= 4)
                {
                    StartCoroutine(PlayerWon(Tokens.Blue));
                }
                break;
            case Tokens.Red:
                _winningStatus.Red += 1;
                if (_winningStatus.Red >= 4)
                {
                    StartCoroutine(PlayerWon(Tokens.Red));
                }
                break;
            case Tokens.Yellow:
                _winningStatus.Yellow += 1;
                if (_winningStatus.Yellow >= 4)
                {
                    StartCoroutine(PlayerWon(Tokens.Yellow));
                }
                break;
            case Tokens.Green:
                _winningStatus.Green += 1;
                if (_winningStatus.Green >= 4)
                {
                    StartCoroutine(PlayerWon(Tokens.Green));
                }
                break;
        }
        Debug.LogError("PLayer Reached :::::" + ReachedToken);
    }
    IEnumerator PlayerWon(Tokens FinishedPlayerToken)
    {
        _PlayersFinished += 1;
        PlayerWonDisplay.gameObject.SetActive(true);
        debugText += "\n" + FinishedPlayerToken + " FINISHED " + _PlayersFinished;
        PlayerWonDisplay.text = FinishedPlayerToken + " FINISHED " + _PlayersFinished;
        Debug.LogError("FinishedPlayerToken :::::" + FinishedPlayerToken);

        yield return new WaitForSecondsRealtime(3f);
        PlayerWonDisplay.gameObject.SetActive(false);
    }
    void RollAndMoveforAI(string TokenName)
    {
        GenerateRandomNumber(0);

        if (all_in == true && DiceNumber < 6)
        {
            StartCoroutine(UpdateCurrentToken());
        }
        else
            _Movement.dice_play(DiceNumber, TokenName);

    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), debugText);
    }
}
