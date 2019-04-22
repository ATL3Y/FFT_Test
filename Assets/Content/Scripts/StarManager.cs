using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public static StarManager instance;

    public float mult = 1.0f;

    private Star[] stars;
    private Vector3[] origPos;
    private Vector3[] origScale;

    private void Awake ( )
    {
        instance = this;
    }

    void Start ( )
    {
        stars = GetComponentsInChildren<Star> ( );
        origPos = new Vector3 [ stars.Length ];
        origScale = new Vector3 [ stars.Length ];
        for ( int i = 0; i < stars.Length; i++ )
        {
            origPos [ i ] = stars [ i ].transform.position;
            origScale [ i ] = stars [ i ].transform.localScale;
        }
    }

    // add curve
    /*
    void Update ( )
    {
        // For pulling stars in and out. 
        float tL = GameLord.instance.player.leftHand.Trigger;
        float tR = GameLord.instance.player.rightHand.Trigger;

        // Go to the nearest hand. 
        float t = Mathf.Max ( tL, tR );
        Vector3 targetPos;

        // Both hands
        if ( tL > .95f && tR > .95f )
        {
            targetPos = ( GameLord.instance.player.leftHand.transform.position + GameLord.instance.player.rightHand.transform.position ) / 2.0f;
        }
        // Left hand
        else if ( tL > tR )
        {
            targetPos = GameLord.instance.player.leftHand.transform.position;
        }
        // right hand
        else
        {
            targetPos = GameLord.instance.player.rightHand.transform.position;
        }

        // Add a shake to the stars the closer they are.
        float shake = mult * 0.1f * Mathf.Clamp( (t - 0.5f), 0.0f, 0.5f);

        for ( int i = 0; i < stars.Length; i++ )
        {
            Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * shake;
            stars [ i ].transform.position = offset + Vector3.Lerp ( origPos [ i ], targetPos, t * t * t );
            stars [ i ].transform.localScale = Vector3.Lerp ( origScale [ i ], 0.1f * origScale [ i ], t * t * t );
        }

        // Use hand-unique values for haptics.
        if( tL > 0.001f )
        {
            GameLord.instance.player.hapticsRT.IntensityL = tL;
            GameLord.instance.player.hapticsRT.TriggerNowL = true;
        }
        else
        {
            GameLord.instance.player.hapticsRT.TriggerNowL = false;
        }

        if ( tR > 0.001f )
        {
            GameLord.instance.player.hapticsRT.IntensityR = tR;
            GameLord.instance.player.hapticsRT.TriggerNowR = true;

        }
        else
        {
            GameLord.instance.player.hapticsRT.TriggerNowR = false;
        }
    }

    private void OnDisable ( )
    {
        GameLord.instance.player.hapticsRT.TriggerNowL = false;
        GameLord.instance.player.hapticsRT.TriggerNowR = false;
    }
    */
}
