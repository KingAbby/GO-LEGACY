using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    bool hasLanded;
    bool thrown;

    Vector3 initPosition;
    int diceValue;

    [SerializeField] DiceSide[] diceSides;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void Update()
    {
        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            StartCoroutine(CheckDiceValue());
        }
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            ReRollDice();
        }
        else if (transform.position.y < -3)
        {
            ReRollDice();
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }
    }

    IEnumerator CheckDiceValue()
    {
        yield return new WaitForSeconds(1); // Wait for a second to ensure both dice have finished rolling
        SideValueCheck();
    }

    public void RollDice()
    {
        Reset();
        if (!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.isKinematic = false;
            // rb.AddForce(new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1)) * Random.Range(1, 7)); // Add force for bouncing
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            rb.AddForce(new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500))); // Add force for bouncing
        }
        // else if (thrown && hasLanded)
        // {
        //     // RESET DICE
        //     Reset();
        // }
    }
    void Reset()
    {
        transform.position = initPosition;
        transform.rotation = Random.rotation; // Add randomness to initial rotation
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void ReRollDice()
    {
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        // rb.AddForce(new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1)) * Random.Range(1, 7)); // Add force for bouncing
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        rb.AddForce(new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500))); // Add force for bouncing
    }

    void SideValueCheck()
    {
        diceValue = 0;
        foreach (var side in diceSides)
        {
            if (side.OnGround)
            {
                diceValue = side.SideValue();
                // Debug.Log("ROLLED NUMBER = " + diceValue);
                break;
            }
        }
        GameManager.instance.ReportDiceRolled(diceValue);
    }
    void LateUpdate()
    {
        if (transform.position.y < -3)
        {
            ReRollDice();
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }
    }
}