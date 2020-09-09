using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    Dice.Tokens PlayerToken; 
    public bool AllTokensInHouse = true;
    // Start is called before the first frame update
    void Start()
    {
        AllTokensInHouse = true;
        if(photonView.IsMine)
        {
            GameManager.Instance.gameObject.GetComponent<Dice>().SetCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void RefreshInstance(ref Player player,Player prefab)
    {
        var positon = prefab.transform.position;
        var rotation = prefab.transform.rotation;
        if(player != null)
        {
            // switch(player.PlayerToken)
            // {
            //     case Dice.Tokens.Red:
            //     break;
            //     case Dice.Tokens.Blue:
            //     break;
            //     case Dice.Tokens.Green:
            //     break; 
            //     case Dice.Tokens.Yellow:      
            //     break;
            // }            
                positon = player.transform.position;
                rotation = player.transform.rotation;
                PhotonNetwork.Destroy(player.gameObject);
        }
        player = PhotonNetwork.Instantiate(prefab.gameObject.name,positon,rotation).GetComponent<Player>();
    }
}
