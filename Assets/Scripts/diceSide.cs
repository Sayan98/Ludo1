using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceSide : MonoBehaviour
{
    bool onGround;
    public int sideValue;

    private void OnTriggerStay(Collider other)
    {
         if(other.tag == "Floor")
         {
            onGround = true;
         }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Floor")
        {
            onGround = false;
        }
    }

    public bool OnGround ()
    {
        return onGround;
    }
}
