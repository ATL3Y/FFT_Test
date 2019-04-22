using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stately;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private Transform lHand;

    [SerializeField]
    private Transform rHand;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float targetWeight = 0.5f; // 0 = no weight, 1 = move to target

    [SerializeField]
    private float speed = 10.0f;

    private Vector3 oLHandPos;
    private Vector3 oRHandPos;

    private State rootState = new State("root");
    private State idleState = new State("idle");
    private State jumpState = new State("move");
    private State coolDownState = new State("coolDown");

    private void Start()
    {
        oLHandPos = lHand.transform.position;
        oRHandPos = rHand.transform.position;
    }

    private Vector3 CalculateDirection ( )
    {
        // Calculates a vector that veers towards the target. 
        Vector3 lookDir = head.forward;
        Vector3 targetDir = Vector3.Normalize(target.position - head.position);

        return Vector3.Normalize ( (1.0f - targetWeight) * lookDir + targetWeight * targetDir );
    }

    private void Update()
    {
        Vector3 dir = CalculateDirection ( );
        
        Vector3 lHandMoveDist = oLHandPos - lHand.transform.position;
        Vector3 rHandMoveDist = oRHandPos - rHand.transform.position;

        // Calculate if hands are pushing back from our direction.
        // TODO: Integrate dist magnitude in this. 
        float pushPower = 0.5f * (Vector3.Dot(dir, lHandMoveDist.normalized) + Vector3.Dot(dir, rHandMoveDist.normalized)); 

        if(pushPower > 0.0f)
        {
            root.position = Vector3.Lerp ( root.position, root.position + pushPower * dir, speed * Time.deltaTime );
        }

        oLHandPos = lHand.transform.position;
        oRHandPos = rHand.transform.position;
    }
}
