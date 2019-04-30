using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool played = false;
    private void OnTriggerEnter ( Collider other )
    {
        if ( played )
            return;

        if ( other.gameObject.layer == 9 )
        {
            played = true;
            print ( "found player" );
            GetComponent<AudioSource> ( ).Play ( );

        }
    }
}
