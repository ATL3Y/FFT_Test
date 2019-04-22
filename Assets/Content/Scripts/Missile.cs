using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	private float speed = 350.0f; 
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

    public void Init ( GunController myGunController, Transform mySource, Transform myTarget )
    {
        line = GetComponent<LineRenderer> ( );
        line.enabled = false;
        gunController = myGunController;
        source = mySource;
        target = myTarget;
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

        if ( !hit )
        {
            Vector3 targetDir = Vector3.Normalize( target.position - transform.position);
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp ( transform.rotation, targetRot, Time.deltaTime );
            rb.velocity = transform.forward * speed * Time.deltaTime;
        }

        // If we pass the target, self destruct.
        Vector3 toTarget = target.position - transform.position;
        if ( Vector3.Dot ( oToTarget, toTarget ) < 0.0f )
        {
            EmitDust ( );
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
            line.enabled = true;
            line.SetPosition ( 0, this.transform.position );
            line.SetPosition ( 1, source.position );
            transform.parent = collision.gameObject.transform;
            Done ( );
            CoHo.Instance.WaitAndCallback ( 5.0f, EmitDust );
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
        GameObject.Instantiate ( poofFXPrefab, transform.position, transform.rotation );
        CoHo.Instance.WaitAndCallback ( 0.5f, DestroyMe );
    }

    private void DestroyMe ( )
    {
        Destroy ( this.gameObject );
    }
}
