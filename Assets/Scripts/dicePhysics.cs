using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dicePhysics : MonoBehaviour
{
    Rigidbody rb;
    bool hasLanded,thrown;

    Vector3 initPos;

    public int diceValue;

    public diceSide[] diceSides;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown (0))
        {
            rollDice();
        }

        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = false;
            rb.useGravity = false;
            sideValueCheck();
        }
        else if (rb.IsSleeping ( )&& hasLanded && diceValue == 0)
        {
            RollAgain();
        }
    }

    void rollDice()
    {
        if (!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            Invoke("reset", 7f);
        }

    }
    void reset()
    {
        transform.position = initPos;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void RollAgain()
    {
        reset();
    }

    void sideValueCheck()
    {
        diceValue = 0;
        foreach (diceSide side in diceSides)
        {
            if (side.OnGround())
            {
                diceValue = side.sideValue;
                //Debug.Log(diceValue + "has been rolled");
            }
        }
    }



}
