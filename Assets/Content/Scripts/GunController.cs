using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    public Transform muzzle;
    float kickBackTime = 1.0f;
    public AnimationCurve kickBackCurve;
    private AudioSource audiosource;

    [SerializeField]
    private GameObject missilePrefab;

    [SerializeField]
    private List<Transform> waypoints;
    private int count = 0;

    public void Start ( )
    {
        audiosource = GetComponent<AudioSource> ( );
    }

    public void Update ( )
    {
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            Use ( );
        }

        kickBackTime = Mathf.Clamp01 ( kickBackTime + Time.deltaTime * 2.0f );
        // transform.localEulerAngles = new Vector3 ( kickBackCurve.Evaluate ( kickBackTime ) * -70, transform.localEulerAngles.y, transform.localEulerAngles.z );
    }

    public void Use ()
    {
        audiosource.Play ( );
        Missile missile = GameObject.Instantiate(missilePrefab, muzzle.position, muzzle.rotation, transform).GetComponent<Missile>();
        if(count < waypoints.Count )
        {
            missile.Init ( this, muzzle, waypoints [ count ] );
            count++;
        }
        else
        {
            // missile.Init ( this, muzzle.position + 100.0f * muzzle.forward );
        }
        
        kickBackTime = 0.0f;
    }
}
