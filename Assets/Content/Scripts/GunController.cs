using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    public Transform muzzle;
    float kickBackTime = 1.0f;
    public AnimationCurve kickBackCurve;
    [SerializeField]
    private AudioSource audiosourceShoot;
    [SerializeField]
    private AudioSource audiosourceClick;

    [SerializeField]
    private GameObject missilePrefab;

    [SerializeField]
    private List<Transform> waypoints;
    private int count = 0;
    private float cooldown = 2.0f;
    private bool readyToShoot = true;

    public void Start ( )
    {
        audiosourceShoot = GetComponent<AudioSource> ( );
    }

    public void Update ( )
    {
        cooldown -= Time.deltaTime;
        Debug.Log ( OVRInput.Get ( OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch ) );
        // Debug.Log ( OVRInput.Get ( OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch ) );
        if ( !readyToShoot )
        {
            if ( OVRInput.Get ( OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch ) < 0.1f )
            {
                audiosourceClick.Play ( );
                readyToShoot = true;
            }
        }

        if(readyToShoot && cooldown < 0.0f )
        {
            // print ( "in" );
            if ( OVRInput.Get ( OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch ) > 0.9f )
            {
                // print ( "in again" );
                Use ( );
                cooldown = 1.0f;
                readyToShoot = false;
            }
        }

        kickBackTime = Mathf.Clamp01 ( kickBackTime + Time.deltaTime * 2.0f );
        //transform.localEulerAngles = new Vector3 ( kickBackCurve.Evaluate ( kickBackTime ) * -70, transform.localEulerAngles.y, transform.localEulerAngles.z );
    }

    public void Use ()
    {
        audiosourceShoot.Play ( );
        Missile missile = GameObject.Instantiate(missilePrefab, muzzle.position, muzzle.rotation).GetComponent<Missile>();
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
