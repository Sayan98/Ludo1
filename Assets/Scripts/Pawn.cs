using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
   // Pathpoint pathpointObjs;
   // ParentPathpoints parentPaths;
    private void Awake()
    {
       // pathpointObjs = GameObject.FindObjectOfType<Pathpoint>();
        //parentPaths = GameObject.FindObjectOfType<ParentPathpoints>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveSteps()
    {
        StartCoroutine(MoveSteps());
        print("method working");
    }
    public  IEnumerator MoveSteps()
    {
        yield return new WaitForSeconds(1f);
         //print("method working");
        for (int i = 0; i < 8; i++)
        {
            //transform.position = parentPaths.commonPathpoints[i].transform.position;
            yield return new WaitForSeconds(0.2f);
            print(i);

        }
    }
}
