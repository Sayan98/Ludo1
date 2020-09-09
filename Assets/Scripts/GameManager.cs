using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{       
    public Player LocalPlayer;
    [Header("Player Prefabs")]
    public Player[] PlayerPrefabs;

    public static GameManager Instance;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        //Player.RefreshInstance(ref LocalPlayer,PlayerPrefabs[(int)NetworkManager.Instance.LocalPlayerToken]);
    }
   public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
   {
       base.OnPlayerEnteredRoom(newPlayer);       
   }
   public void RefreshPlayer()
   {
       Player.RefreshInstance(ref LocalPlayer,PlayerPrefabs[(int)NetworkManager.Instance.LocalPlayerToken]);
   }
}
