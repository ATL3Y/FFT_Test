using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	private float speed = 100.0f; 
    private GunController gunController;
    public Transform target;
    private EasingCurveHelper.Curve curve = EasingCurveHelper.Curve.EaseInOutQuartic;
    private LineRenderer line;
    public Transform source;

    [SerializeField]
    private GameObject poofFXPrefab;

    public void Init ( GunController myGunController, Transform mySource, Transform myTarget )
    {
        line = GetComponent<LineRenderer> ( );
        gunController = myGunController;
        source = mySource;
        target = myTarget;
    }

    private void Update()
    {
        line.SetPosition ( 0, source.position );
        line.SetPosition ( 1, target.position );
        Vector3 targetDir = Vector3.Normalize( target.position - transform.position);
        Vector3 velocity = speed * targetDir;
		GetComponent< Rigidbody >().MovePosition( transform.position + velocity * Time.deltaTime);
	}

    private void OnCollisionEnter ( Collision collision )
    {
        if( collision.collider.gameObject.layer == 12 )
        {
            CoHo.Instance.WaitAndCallback ( 5.0f, DestroyMe );
        }
    }

    private void DestroyMe ( )
    {
        Destroy ( this.gameObject );
    }

    private void OnDestroy ( )
    {
        // Instantiate poof effect
        GameObject.Instantiate ( poofFXPrefab, transform.position, transform.rotation );
    }
}
