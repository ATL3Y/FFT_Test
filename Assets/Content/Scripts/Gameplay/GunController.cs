using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Transform _muzzle;

    [SerializeField]
    private AudioSource _audShoot;

    [SerializeField]
    private AudioSource _audClick;

    [SerializeField]
    private GameObject _missilePrefab;

    private float _coolDown = 1.0f;
    private bool _readyToShoot = true;

    public void Use()
    {
        _audShoot.Play();
        Missile missile = Instantiate(_missilePrefab, _muzzle.position, _muzzle.rotation).GetComponent<Missile>();
        missile.Init(this, _muzzle);
    }

    private void Start ( )
    {
        _audShoot = GetComponent<AudioSource> ( );
    }

    private void Update ( )
    {
        _coolDown -= Time.deltaTime;

        if ( !_readyToShoot )
        {
            if ( OVRInput.Get ( OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch ) < 0.1f )
            {
                _audClick.Play ( );
                _readyToShoot = true;
            }
        }

        if( _readyToShoot && _coolDown < 0.0f )
        {
            if ( OVRInput.Get ( OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch ) > 0.9f )
            {
                Use ( );
                _coolDown = 1.0f;
                _readyToShoot = false;
            }
        }
    }
}
