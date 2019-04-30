using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource> ( );
    }

    private bool played = false;
    // Update is called once per frame
    void Update()
    {
        if ( played && aud.volume < .7f )
        {
            aud.volume = Mathf.Lerp ( aud.volume, .7f, Time.deltaTime );
        }
    }

    private void OnTriggerEnter ( Collider other )
    {
        if(other.gameObject.layer == 9 )
        {
            played = true;
            print ( "found player" );
            
        }
    }
}
