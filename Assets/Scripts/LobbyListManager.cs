using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LobbyListManager : MonoBehaviourPunCallbacks , ILobbyCallbacks

{
    Transform RoomListPanel;
    public GameObject RoomInfoPrefab;
    void Start()
    {
        RoomListPanel = this.transform;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.LogError("OnRoomListUpdate");
        RefreshList(roomList);
    }
    void RefreshList(List<RoomInfo> roomInfos)
    {
        while(RoomListPanel.childCount != 0)
        {
            Destroy(RoomListPanel.GetChild(0));    
        }
        foreach(RoomInfo _roominfo in roomInfos)
        {
            CreateRoomList(_roominfo);
        }
    }
    void CreateRoomList(RoomInfo Room)
    {
       GameObject _CloneRoom = Instantiate(RoomInfoPrefab,RoomListPanel);
       _CloneRoom.GetComponent<RoomListInfo>()._RoomInfo = Room;
       _CloneRoom.GetComponent<RoomListInfo>().SetRoom();
    }
}
