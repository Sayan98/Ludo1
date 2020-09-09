using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MovePawn : MonoBehaviour
{
    int DiceNumber;
    public Text dice;
    public float speed;
    public Vector3 startPos, CurPos;
    public Vector3 DistanceCovered, DistanceTobeCovered;
    public bool move;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        DistanceCovered=CurPos-startPos;
        
        dice.text = "Dice "+ DiceNumber;
        if(move)
        {
            
             
         
            Pawnmovement(); 
            if(DistanceCovered==DistanceTobeCovered)
            {
                move=false;
            }
        }

    }
    public void GenerateRandomNumber()
    {
        DiceNumber = Random.Range(1, 7);
        startPos = transform.position;
        
        DistanceTobeCovered = transform.forward * 0.61f * DiceNumber;

        StartCoroutine(WaitnMove());

    }
    void Pawnmovement()
    {
        
        transform.Translate((Vector3.forward)*speed) ;
        CurPos = transform.position;
        Vector3 F = new Vector3(-0.61F, 0.25F, -0.669f);
        if (transform.position == F)
        {
            transform.eulerAngles = new Vector3(0, -90f, 0);

        }

    }
    IEnumerator WaitnMove()
    {
        yield return new WaitForSeconds(1f);
        move = true;
        
    }
}
