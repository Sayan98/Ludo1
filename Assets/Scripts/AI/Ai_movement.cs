using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_movement : MonoBehaviour
{
    
    Ai_initialize _Initialize;
    Dice _dice;
    Follower[] _follower = new Follower[4];
    public static Ai_movement Instance;

    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }

    void Start() {

        _Initialize = Ai_initialize.Instance;
        _dice = Dice.Instance;

    }
    
    public void dice_play(int dice_number, string token_name) {

    var all_pawned = true;

        if(dice_number == 6) {

            for (int i = 0; i < 4; i++) {
                
                _follower[i] = GameObject.Find(token_name +"(Clone)").transform.GetChild(i).GetComponent<Follower>();
                    
                    if(_follower[i].PawnoutOfTheHouse == false) {
                    
                        all_pawned = false;
                        _follower[i].CheckForTheStart();
                        break;
                    
                    }
            
            }

            if(all_pawned == true) {

                    var i = Random.Range(0,4);

                    while (GameObject.Find(token_name+"(Clone)").transform.GetChild(i).gameObject.activeInHierarchy == false)
                        i = Random.Range(0,4);
                    
                    _follower[i] = GameObject.Find(token_name+"(Clone)").transform.GetChild(i).GetComponent<Follower>();
                    _follower[i].CheckForTheStart();

            }      

        }
        else {

            for (int i = 0; i < 4; i++) {
                
                _follower[i] = GameObject.Find(token_name+"(Clone)").transform.GetChild(i).GetComponent<Follower>();
                    
                    if(_follower[i].PawnoutOfTheHouse == true) {
                    
                        _follower[i].CheckForTheStart();
                        break;
                    
                    }

            }

        }

    }

}
