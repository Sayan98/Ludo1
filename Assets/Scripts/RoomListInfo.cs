using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListInfo : MonoBehaviourPun
{
    public RoomInfo _RoomInfo;
    public Text Name;
    void Start()
    {
        
    }
    public void SetRoom()
    {
        Name.text = _RoomInfo.Name;
    }
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(_RoomInfo.Name);
    }
}
