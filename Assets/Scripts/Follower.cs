using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;

  public class Follower : MonoBehaviourPun
  {
     public PathCreator pathCreator;
     Dice dice;
     EndOfPathInstruction endOfPathInstruction;
     float speed = 2.98f;
     public float distanceTravelled;
     public bool canplay;
     [HideInInspector] public Vector3 InHousePos;
     public bool PawnoutOfTheHouse;
     public bool canCollide;
     
     public bool checkOpponentCollide = false;
     Collider col;
     
     //AI
     Ai_initialize _Initialize;

    private void Awake()
    {
        InHousePos = transform.position; 
    }
    // Start is called before the first frame update
    void Start()
    {
        dice = FindObjectOfType<Dice>();
        col = GetComponent<Collider>();
        canplay = true;

        //AI
        _Initialize = Ai_initialize.Instance;

        //dice.CheckForTurn();
    }

        // Update is called once per frame
    void Update()
    {
    }
    
    private void OnMouseDown()
    {
        Dice.Tokens CurrentToken = dice.ActiveToken;
        switch(CurrentToken)
        {
        case Dice.Tokens.Red:
            if(gameObject.CompareTag("RedPawn") && photonView.IsMine)
            {
                CheckForTheStart();
            // ExecuteMoveCode();            
            }
        break;
        case Dice.Tokens.Yellow:
            if (gameObject.CompareTag("YellowPawn") && photonView.IsMine)
            {
            // ExecuteMoveCode();
                CheckForTheStart();
            }
        break;
        case Dice.Tokens.Green:
            if (gameObject.CompareTag("GreenPawn") && photonView.IsMine)
            {
            // ExecuteMoveCode();
                CheckForTheStart();
            }
        break;
        case Dice.Tokens.Blue:
            if(gameObject.CompareTag("BluePawn") && photonView.IsMine)
            {
            //ExecuteMoveCode();
                CheckForTheStart();
            }
        break;
        }


    }
    public void CheckForTheStart()
    {

        if (dice.DiceNumber == 6)
        {       
            if (!PawnoutOfTheHouse)
            {
                canplay = false;
                MovePlayerToStartPos();
                GameManager.Instance.LocalPlayer.AllTokensInHouse = false;               
            }
            else
            {
                if (canplay)
                {
                    StartCoroutine(Hold());
                }
            }
        }
        else
        {
            if(PawnoutOfTheHouse)
            {
                StartCoroutine(Hold());        
            }
            /*else
            {
                dice.DiceNumber = 0;
                dice.DiceButton.interactable = true;
                StartCoroutine(Dice.Instance.UpdateCurrentToken());
            }*/ // player can mistakely click on in house token
            
        }

    }
    void MovePlayerToStartPos()
    {
        canCollide = true;
        StartCoroutine(PauseNturnover());
        print("Player will go to start Position");
        //transform.position = manager.StartPos.position;
        transform.position = pathCreator.path.GetPointAtDistance(0);
        PawnoutOfTheHouse = true;
        canplay = true;
        dice.DiceNumber = 0;
        dice.diceNumberDisplay.text = "Dice " + dice.DiceNumber;
        dice.DiceButton.interactable = true;

        //AI
        StartCoroutine(check_for_ai_2_turn());

    }

    public IEnumerator check_for_ai_2_turn() {

        yield return new WaitForSeconds(1f);

        switch (dice.ActiveToken.ToString()) {

            case "Blue" :   if(_Initialize.token_blue == false)
                                dice.Check_Ai_Turn();
            break;

            case "Yellow" :   if(_Initialize.token_yellow == false)
                                dice.Check_Ai_Turn();
            break;

            case "Red" :   if(_Initialize.token_red == false)
                                dice.Check_Ai_Turn();
            break;

            case "Green" :   if(_Initialize.token_green == false)
                                dice.Check_Ai_Turn();
            break;

        }

    }

    IEnumerator Hold()
    {
        dice.DiceButton.interactable = false;
        float remainingdist = dice.DiceNumber * 2.98f + distanceTravelled;
        if(remainingdist>=182f)
        {
            //print("dont move");
            StartCoroutine(TurnOver());
        }
        else
        {            
            GetComponent<BoxCollider>().enabled = false;
            for (int i = 0; i < dice.DiceNumber; i++)
            {

                yield return new WaitForSeconds(.15f);
                distanceTravelled += speed;
                //print(distanceTravelled);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
               // transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);   

                if (distanceTravelled > 181f)
                {
                    StartCoroutine(TurnOver());
                    //print("plus 1");
                    Destroy(gameObject);
                    Dice.Instance.CallWinUpdateRPC((int)dice.ActiveToken);
                }
            }
            //dice.CheckForTurn();
           
        }
        //canCollide = true;
        StartCoroutine(PauseNturnover());
        StartCoroutine(TurnOver());
        if(dice.DiceNumber !=6)
            StartCoroutine(Dice.Instance.UpdateCurrentToken());
        else
            StartCoroutine(check_for_ai_2_turn());//AI
    }
    IEnumerator TurnOver()
    {
        //canplay = false;
        // dice.CheckForTurn();
        if(dice.DiceNumber == 6)
         yield return null;

        dice.DiceNumber = 0;

        dice.diceNumberDisplay.text = "Dice " + dice.DiceNumber;

        dice.DiceButton.interactable = true;   
        yield return new WaitForSeconds(0.5f);         
        GetComponent<BoxCollider>().enabled = true;
        //print("TurnOver");
    }
    IEnumerator PauseNturnover()
    {
        yield return new WaitForSeconds(0.5f);
        //TurnOver();
        canCollide = true;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("SafeZone"))//&&(other.gameObject.T))
        {

            canCollide = false;   
            //GetComponent<BoxCollider>().enabled = false;
            //transform.position += new Vector3(0,1f, 0);
            //transform.localScale = new Vector3(1, 1.25f, 1);
        }

        if (canCollide)
        {
            if (other.gameObject.tag != gameObject.tag && !gameObject.CompareTag(Dice.Instance.ActiveToken.ToString()+"Pawn"))
            {
                if(other.tag != "SafeZone")
                {
                    print("reset the " + other.gameObject.tag + " position");
                    //photonView.RPC("RPC_CheckPlayerCollision",RpcTarget.All); 
                    ResetPlayer(this);
                }       
            }
            else
            {
                    Follower folsc = other.gameObject.GetComponent<Follower>();
                    folsc.transform.position += new Vector3(1f,0,0);
                    transform.position += new Vector3(0,1f, 0);
                    //transform.localScale = new Vector3(1, 1.25f, 1);
                    //folsc.transform.localScale = new Vector3(1, 1.25f, 1);              
            }
        }
    }
    public void CheckPlayerCollision()
    {
        checkOpponentCollide = true;
    }
    void ResetPlayer(Follower folsc)
    {
                Debug.LogError("RPC_ResetPlayer");
                folsc.transform.position = folsc.InHousePos;
                folsc.PawnoutOfTheHouse = false;
                folsc.distanceTravelled = 0f;
                folsc.transform.eulerAngles = Vector3.zero;
                dice.PreviousValue = 6;
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("SafeZone"))
        {
           // GetComponent<BoxCollider>().enabled = true;
        }
        // if(checkOpponentCollide)
        // {
        //     Debug.LogError("checkOpponentCollide IS TRUE");
        //     if (other.tag != gameObject.tag)
        //     {                
        //         Debug.LogError("other.tag != gameObject.tag IS TRUE");
        //         if(other.tag != "SafeZone")
        //         {                       
        //             Debug.LogError("SafeZone IS TRUE");
        //             ResetPlayer(this);
        //             checkOpponentCollide = false;
        //         }
        //     }
        // }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SafeZone"))
        {

            //transform.localScale = new Vector3(1.5f, 2f, 1.5f);
        }
        
    }

  }


