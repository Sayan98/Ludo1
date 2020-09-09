using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{
    public GameObject dice,bluePlayer1;

    public GameObject[] totalTiles;

    public Vector3 strtPos, endPos;

    public int diceCount=0, playerCurrentTileLength;

    float diceSpeed=3f,radius=.4f;

    public Text diceCountText;

    private void Start()
    {

        //UVMappingOfDice();
        if (diceCountText)
        diceCountText.text = "1";
        //Invoke ("followPath",2f);
    }
    void Update()
    {
        //playerMovemnt();
        Invoke ("followPath",2f);
        
    }

    public void OnMouseOver()
    {
        if (Vector3.Distance (totalTiles[diceCount].transform.position,transform.position) < radius)
        { 
        diceCount++;

            //if (diceCount >= totalTiles.Length)
            //{
            //    diceCount = 0;
        }

        if (Input.GetMouseButton(0))
            {

            print("diceCount :: " + diceCount);
            
                if (diceCount == 6)
                {
                  for (int i = 0; i > totalTiles.Length; i++)
                  {
                    //diceCount = playerCount;

                    //print("diceCount :: " + diceCount + " :: "+ playerCount);
                    
                  }

                 }
            bluePlayer1.transform.position = transform.position;

                transform.position = Vector3.MoveTowards(bluePlayer1.transform.position, totalTiles[diceCount].transform.position, diceSpeed * Time.deltaTime);
            print("chngPos :: " + transform.position);

            //}
        }
    }


    public void followPath ()
    {

        if (gameObject.tag == "bluePlayer")
        { 

            if (Input.GetMouseButton(0)  && dice.gameObject.tag == "bluePlayer")
            {
                diceCount = Random.Range(1, 7);
                diceCountText.text = "" + diceCount;

                print("chckdiceCountText :: "+diceCount);

                 if (Vector3.Distance (totalTiles[diceCount].transform.position,transform.position) < radius) 
                {
                 diceCount++;

                    if (diceCount>=totalTiles.Length)
                 {
                  diceCount = 0;
                 }
                }
                playerCurrentTileLength++;
                StartCoroutine("updatePlayerPos", 1f);
            }
        }
    }

    public bool IsPos = false;
    public IEnumerator updatePlayerPos (float delayTym=1f)
    {
        yield return new WaitForSeconds(delayTym);
        if (IsPos == false)
        { 
        playerCurrentTileLength = diceCount;
        for(int i=0;i<playerCurrentTileLength;i++)
            {

                Vector3 currentTilePos = totalTiles[i].gameObject.transform.position;
            //bluePlayer1.transform.position = transform.position+ pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position = Vector3.MoveTowards(transform.position, currentTilePos, diceSpeed * Time.deltaTime);
            print("chckdiceCountText :: " + diceCount + "::" + currentTilePos + "::" + playerCurrentTileLength);

        }
        }

    }

    void UVMappingOfDice ()
    {
        float size = 1f;
        Vector3[] vertices = {
            new Vector3(0, size, 0),
            new Vector3(0, 0, 0),
            new Vector3(size, size, 0),
            new Vector3(size, 0, 0),

            new Vector3(0, 0, size),
            new Vector3(size, 0, size),
            new Vector3(0, size, size),
            new Vector3(size, size, size),

            new Vector3(0, size, 0),
            new Vector3(size, size, 0),

            new Vector3(0, size, 0),
            new Vector3(0, size, size),

            new Vector3(size, size, 0),
            new Vector3(size, size, size),
        };

        int[] triangles = {
            0, 2, 1, // front
			1, 2, 3,
            4, 5, 6, // back
			5, 7, 6,
            6, 7, 8, //top
			7, 9 ,8,
            1, 3, 4, //bottom
			3, 5, 4,
            1, 11,10,// left
			1, 4, 11,
            3, 12, 5,//right
			5, 12, 13


        };


        Vector2[] uvs = {
            new Vector2(0, 0.66f),
            new Vector2(0.25f, 0.66f),
            new Vector2(0, 0.33f),
            new Vector2(0.25f, 0.33f),

            new Vector2(0.5f, 0.66f),
            new Vector2(0.5f, 0.33f),
            new Vector2(0.75f, 0.66f),
            new Vector2(0.75f, 0.33f),

            new Vector2(1, 0.66f),
            new Vector2(1, 0.33f),

            new Vector2(0.25f, 1),
            new Vector2(0.5f, 1),

            new Vector2(0.25f, 0),
            new Vector2(0.5f, 0),
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        //mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
