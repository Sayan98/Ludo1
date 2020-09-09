using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class Ai_initialize : MonoBehaviourPun
{
    NetworkManager manager;
    public Player[] PlayerPrefabs_ai;

    //[HideInInspector]
    public bool room_owner, token_green, token_blue, token_yellow, token_red;

    public  GameObject _AiTokenBlue;
    public GameObject _AiTokenRed;
    public GameObject _AiTokenGreen;
    public GameObject _AiTokenYelow;

    public static Ai_initialize Instance;
    //if tokens are true then player else ai
    //public Follower[] ai_player = new Follower[4];


    void Awake() {
        if(Instance == null)
        {
            Instance = this;
        }
        room_owner = false;

    }

    void Start() {

        DontDestroyOnLoad(gameObject);
        manager = GameObject.Find("NetWorkManager").GetComponent<NetworkManager>();

    }

    public void room_owner_controls_ai() {

        room_owner = true;
        StartCoroutine(waitandspawnai());

    }

    IEnumerator waitandspawnai() {
        
            //time to wait before spawning ai
            yield return new WaitForSeconds(1f);
            
            photonView.RPC("set_ai", RpcTarget.AllBuffered);
            
            

    }

    [PunRPC]
    public void set_ai() {

        
            //check which are players 
            if(GameObject.Find("Green(Clone)") != null)
                token_green = true;
            if(GameObject.Find("Yellow(Clone)") != null)
                token_yellow = true;
            if(GameObject.Find("Red(Clone)") != null)
                token_red = true;
            if(GameObject.Find("Blue(Clone)") != null)
                token_blue = true;
        
        if(room_owner == true) {
            //spawn ai
            if(token_blue == false)
               _AiTokenBlue = PhotonNetwork.Instantiate(PlayerPrefabs_ai[0].gameObject.name, PlayerPrefabs_ai[0].transform.position,PlayerPrefabs_ai[0].transform.rotation);
            if(token_red == false)
               _AiTokenRed = PhotonNetwork.Instantiate(PlayerPrefabs_ai[1].gameObject.name, PlayerPrefabs_ai[1].transform.position,PlayerPrefabs_ai[1].transform.rotation);
            if(token_yellow == false)
                _AiTokenYelow = PhotonNetwork.Instantiate(PlayerPrefabs_ai[2].gameObject.name, PlayerPrefabs_ai[2].transform.position,PlayerPrefabs_ai[2].transform.rotation);
            if(token_green == false)
                _AiTokenGreen = PhotonNetwork.Instantiate(PlayerPrefabs_ai[3].gameObject.name, PlayerPrefabs_ai[3].transform.position,PlayerPrefabs_ai[3].transform.rotation);

        }
    }


}
