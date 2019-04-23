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
}
