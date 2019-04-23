using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stately;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance = null;

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

    public int currentHitCount = 0;

    public float playerVelocity = 0.0f;

    [SerializeField]
    private List<Transform> waypoints;

    public void RegisterHit ( )
    {
        print ( "called" );
        currentHitCount += 1;
    }

    public void RegisterLoss ( )
    {
        if( currentHitCount > 0 )
        {
            currentHitCount -= 1;
        }
    }

    public Transform GetTargetWaypoint ( Vector3 sourcePos, Vector3 aimDir )
    {
        int index = waypoints.Count - 1;
        float highestDot = -1.0f;

        for(int i=0; i < waypoints.Count; i++)
        {
            Vector3 toWaypoint = Vector3.Normalize(waypoints[i].position - sourcePos);
            float dot = Vector3.Dot(aimDir, toWaypoint);
            if ( dot > highestDot )
            {
                highestDot = dot;
                index = i; 
            }
        }

        return waypoints [ index ];
    }

    private void Awake ( )
    {
        if ( instance == null )
        {
            instance = this;
        }
        else if ( instance != this )
        {
            Destroy ( gameObject );
        }
    }

    private void Start()
    {
        oLHandPos = lHand.transform.position;
        oRHandPos = rHand.transform.position;
    }

    /*
    private Vector3 CalculateDirection ( )
    {
        // Calculates a vector that veers towards the target. 
        Vector3 lookDir = head.forward;
        Vector3 targetDir = Vector3.Normalize(target.position - head.position);

        return Vector3.Normalize ( (1.0f - targetWeight) * lookDir + targetWeight * targetDir );
    }
    */

    public float deceleration = 1.0f;
    public float handVelocityMult = 1.2f;
    public float maxVelocity = 15.0f;
    private void Update()
    {

        // Decay playerVelocity to simulate drag. 
        playerVelocity = Mathf.MoveTowards ( playerVelocity, 0.0f, deceleration * Time.deltaTime );

        Vector3 targetDir = Vector3.Normalize(target.position - head.position);

        // Distance hand moves / time;
        Vector3 handVelocity = handVelocityMult * (rHand.transform.position - oRHandPos) / Time.deltaTime;

        // Calculate if hands are pushing back from our direction.
        if ( Vector3.Dot ( targetDir, handVelocity.normalized ) < 0.0f )
        {
            if( currentHitCount > 0 && playerVelocity < maxVelocity )
            {
                print ( "inTwo" );
                playerVelocity += Vector3.Magnitude ( handVelocity );
            }
        }

        
        // oLHandPos = lHand.transform.position;
        oRHandPos = rHand.transform.position;

        float landingDist = Vector3.Distance ( root.position, waypoints [ waypoints.Count - 1 ].position );
        float landingLength = 40.0f;
        float landingT = 1.0f;
        if ( landingDist < landingLength )
        {
            // starting to land = 1, landed = 0f.
            landingT = landingDist / landingLength;
            landingT = landingT * landingT * landingT;
        }

        // Add playerVelocity to the player's position in terms of time. 
        root.position += landingT * playerVelocity * Time.deltaTime * targetDir;
    }
}
