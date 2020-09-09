
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager :MonoBehaviourPunCallbacks,IPunObservable
{
    string debugText;
    public InputField RoomIDField;
    public Button JoinRoom;
    public Button CreateRoom;  

    //AI
    Ai_initialize _Initialize;

    public byte MaxPlayer = 1;
    byte PlayersReady = 0;

    public Dice.Tokens LocalPlayerToken;
    public static NetworkManager Instance;

    public GameObject PickToken;
    public GameObject NoOfPlayer;
    public Button StartGameButton;

    void Awake()
    {
        if(Instance==null)
            Instance = this;
    }

    private void Start()
    {
        SetupLobbyMenu();

        ConnectToGameServer();
        Debug.LogError("ROOMID&&&&&&&&&& " + RoomIDField.text);
    }
    private void Update()
    {
    }
    public void SetupLobbyMenu()
    {
        PickToken.SetActive(false);
        DontDestroyOnLoad(gameObject);
        JoinRoom.interactable = false;
        CreateRoom.interactable = false;
        NoOfPlayer.SetActive(false);
        StartGameButton.gameObject.SetActive(false);

    }
    public void ConnectToGameServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        debugText += "\n ConnectedToMaster";
        JoinRoom.interactable = true;
        CreateRoom.interactable = true;
        PhotonNetwork.JoinLobby();
    }
    public void OnClickCreateRoom()
    {
        NoOfPlayer.SetActive(true);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(this.gameObject);
    }
    public void OnclickNoOfPlayers(int NO_ofplayer)
    {
        string RoomId = Random.Range(100000, 999999).ToString();
        RoomOptions NewRoomOptions = new RoomOptions();
        NewRoomOptions.MaxPlayers = (byte)NO_ofplayer;
        NewRoomOptions.IsVisible = true;
        NewRoomOptions.IsOpen = true;
        if (RoomIDField.text != "")
        {
            PhotonNetwork.CreateRoom(RoomIDField.text, NewRoomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomId, NewRoomOptions);
        }

        debugText += "\n RoomID: " + RoomId;
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        NoOfPlayer.SetActive(false);
        PlayersReady = 0;
        debugText += "\n Room Created :" + PhotonNetwork.CurrentRoom.Name;
        Debug.LogError("RoomCreated");
        Debug.LogError("MaxNumberofPlayer" + PhotonNetwork.CurrentRoom.MaxPlayers);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        debugText += "\n Joined Room";
        debugText +="\n Number of Players: " + PhotonNetwork.PlayerList.Length.ToString();

        PhotonNetwork.LoadLevel("MainLudo");   
        PickToken.SetActive(true);
        StartGameButton.gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            StartGameButton.interactable = true;
            StartGameButton.GetComponentInChildren<Text>().text = "Start Game";
            Debug.LogError("MasterClient");
        }
        else
        {
            StartGameButton.GetComponentInChildren<Text>().text = "Waiting for Host";
            Debug.LogError("You aint No Master");
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        debugText += "\n CreateRoomFailed: " + message;
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        debugText += "\n Join Room Failed: " + message;
    }
    public void OnTokenPicked(int PlayerToken)
    {
        LocalPlayerToken = (Dice.Tokens)PlayerToken;
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
            {
                ["MasterPlayerToken"] = PlayerToken
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
        Dice.Instance.RefreshPlayer();
        GameManager.Instance.RefreshPlayer();
        for (int i = 0; i < PickToken.transform.childCount; i++)
        {
            PickToken.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
        photonView.RPC("RPC_ExcludeToken", RpcTarget.AllBuffered, PlayerToken);
    }

    [PunRPC]
    public void RPC_ExcludeToken(int token)
    {
        PickToken.transform.GetChild(token).GetComponent<Button>().interactable = false;
        PlayersReady += 1;
        debugText += "\n Color Excluded";
    }
    public void OnclickStartGame()
    {
        Debug.LogError("playersReady :" + PlayersReady);
        Debug.LogError("MaxPlayers :" + PhotonNetwork.CurrentRoom.MaxPlayers);

        if (PlayersReady == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC("RPC_StartGame", RpcTarget.AllBuffered);
            _Initialize = Ai_initialize.Instance;
            _Initialize.room_owner_controls_ai();
        }
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        PickToken.SetActive(false);
        StartGameButton.gameObject.SetActive(false);
    }
    private void OnGUI()
    {
        GUI.Label(new UnityEngine.Rect(10, 10, 3000, 500), debugText);
    }
    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)	
    {

    }

}

