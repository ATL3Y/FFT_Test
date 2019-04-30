using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	private float speed = 1000.0f; 
    private GunController gunController;
    public Transform target;
    private EasingCurveHelper.Curve curve = EasingCurveHelper.Curve.EaseInOutQuartic;
    private LineRenderer line;
    public Transform source;
    public Rigidbody rb;

    [SerializeField]
    private GameObject poofFXPrefab;

    private Vector3 oToTarget;
    private float targetWeight = .3f;
    private bool hit = false;
    private bool done = false;
    private Vector3 oTargetDir;

    private GameObject waypoint;
    private float deceleration = 12.0f;
    private float maxTurnSpeed = 15.0f;
    private float currentTurnSpeed = 0.0f;
    private float lifeTime = 3.5f;

    [SerializeField]
    private AudioSource audioSourcePositive;

    [SerializeField]
    private AudioSource audioSourceNegative;

    public void Init ( GunController myGunController, Transform mySource )
    {
        line = GetComponent<LineRenderer> ( );
        line.enabled = false;
        gunController = myGunController;
        source = mySource;
        target = PlayerMovement.instance.GetTargetWaypoint ( mySource.position, mySource.forward );

        oToTarget = target.position - transform.position;

        rb = GetComponent<Rigidbody> ( );
        print ( "in init " + rb.name );
    }

    private void FixedUpdate ( )
    {
        // Line should run from our hand to target.
        if ( line.enabled )
        {
            line.SetPosition ( 0, this.transform.position );
            line.SetPosition ( 1, source.position );
        }

        if ( done )
        {
            return;
        }
        // If we pass the target, self destruct.
        Vector3 toTarget = target.position - transform.position;
        if ( Vector3.Dot ( oToTarget, toTarget ) < 0.0f )
        {
            EmitDust ( );
        }

        lifeTime -= Time.deltaTime;
        // If our lifeTime is up and we haven't hit anything, self destruct;
        if ( !hit && lifeTime < 0.0f )
        {
            EmitDust ( );
        }

        if ( !hit )
        {
            currentTurnSpeed = Mathf.Lerp( currentTurnSpeed, maxTurnSpeed, Time.deltaTime);
            Vector3 targetDir = Vector3.Normalize( target.position - transform.position);
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp ( transform.rotation, targetRot, currentTurnSpeed * Time.deltaTime );
            speed -= deceleration * Time.deltaTime;
            rb.velocity = transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter ( Collision collision )
    {
        if ( done )
            return;

        // if( collision.gameObject.layer == 12 )
        if(!hit)
        {
            hit = true;
            PlayerMovement.instance.RegisterHit ( );
            waypoint = collision.gameObject;
            waypoint.GetComponent<Renderer> ( ).material.color = Color.cyan;
            audioSourcePositive.Play ( );
            line.enabled = true;
            line.SetPosition ( 0, this.transform.position );
            line.SetPosition ( 1, source.position );
            transform.parent = collision.gameObject.transform;
            Done ( );
            CoHo.Instance.WaitAndCallback ( 2.5f, EmitDust );
        }
    }

    private void Done ( )
    {
        done = true;
        GetComponent<TrailRenderer> ( ).time = 0.1f;
        rb.isKinematic = true;
        GetComponent<Collider> ( ).enabled = false;
    }

    private void EmitDust ( )
    {
        Done ( );
        audioSourceNegative.Play ( );
        GameObject.Instantiate ( poofFXPrefab, transform.position, transform.rotation );
        CoHo.Instance.WaitAndCallback ( 2.0f, DestroyMe );
    }

    private void DestroyMe ( )
    {
        if ( hit )
        {
            PlayerMovement.instance.RegisterLoss ( );
            waypoint.GetComponent<Renderer> ( ).material.color = Color.white;
        }
        gameObject.SetActive ( false );
    }
}
