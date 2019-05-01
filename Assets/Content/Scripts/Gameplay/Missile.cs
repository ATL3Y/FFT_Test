using UnityEngine;
using Stately;

public class Missile : MonoBehaviour 
{
    // Cached variables
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform _source;

    [SerializeField]
    private Rigidbody _rb;

    private GunController _gunController;
    private LineRenderer _line;
    private Collision _col;

    // References dropped in from editor.
    [SerializeField]
    private GameObject _explodeFXPrefab;

    [SerializeField]
    private AudioSource _audPositive;

    [SerializeField]
    private AudioSource _audNegative;

    // Tuning movement
    private float _speed = 1000.0f;
    private Vector3 _oToTarget;
    private float _targetWeight = 0.3f;
    private Vector3 _oTargetDir;
    private GameObject _waypoint;
    private float _deceleration = 12.0f;
    private float _maxTurnSpeed = 15.0f;
    private float _currentTurnSpeed = 0.0f;
    private float _lifeTime = 4.0f;

    // Animation
    private Coroutine _turnSpeedCoroutine;

    // State Machine 
    private State _rootState = new State("root");
    private State _propellState = new State("propell");
    private State _hitState = new State("hit");
    private State _defuseState = new State("defuse");
    private State _selfDestructState = new State("selfDestruct");
    private bool _hit = false;

    public void Init ( GunController myGunController, Transform mySource )
    {
        _line = GetComponent<LineRenderer> ( );
        _line.enabled = false;
        _gunController = myGunController;
        _source = mySource;
        _target = PlayerMovement.Instance.GetTargetWaypoint ( mySource.position, mySource.forward );
        _oToTarget = _target.position - transform.position;
        _rb = GetComponent<Rigidbody> ( );

        DefineStateMachine();
    }

    public void DefineStateMachine()
    {
        _rootState.StartAt(_propellState);
        _propellState.OnEnter = delegate
        {
            float timeStamp = Time.timeSinceLevelLoad;
            float turnSpeedDuration = 2.0f;
            _turnSpeedCoroutine = CoroutineHelper.Instance.DoWhile(() =>
            {
                return _currentTurnSpeed < _maxTurnSpeed;
            }, () =>
            {
                float t = EasingCurveHelper.EaseInQuad(Time.timeSinceLevelLoad - timeStamp, 0.0f, Time.deltaTime, turnSpeedDuration);
                _currentTurnSpeed = t * _maxTurnSpeed;
            });

            Vector3 targetDir = Vector3.Normalize(_target.position - transform.position);
            Quaternion targetRot = Quaternion.LookRotation(targetDir);

            // TODO: Create a "target lock" UI to communicate if/what the missile will hit upon pulling the trigger.
            // TODO: For a miss, make some insane fire cracker gone wild scenario. 
            // TODO: For a hit, use SmoothDamp instead of this fake Slerp.  
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _currentTurnSpeed * Time.deltaTime);
            _speed -= _deceleration * Time.deltaTime;
            _rb.velocity = transform.forward * _speed * Time.deltaTime;

            // If we pass the target, self destruct.
            Vector3 toTarget = _target.position - transform.position;
            if (Vector3.Dot(_oToTarget, toTarget) < 0.0f)
            {
                _propellState.ChangeTo(_defuseState);
            }
        };

        // If our lifetime is up and we haven't hit anything, self destruct. 
        _propellState.ChangeTo(_defuseState).After(_lifeTime);

        _hitState.OnEnter = delegate {
            _hit = true;
            PlayerMovement.Instance.RegisterHit();
            _waypoint = _col.gameObject;
            _waypoint.GetComponent<Renderer>().material.color = Color.cyan;
            _audPositive.Play();
            _line.enabled = true;
            _line.SetPosition(0, this.transform.position);
            _line.SetPosition(1, _source.position);
            transform.parent = _col.gameObject.transform;
        };

        _hitState.OnUpdate = delegate {

            // Line should run from our hand to target.
            if (_line.enabled)
            {
                _line.SetPosition(0, this.transform.position);
                _line.SetPosition(1, _source.position);
            }
        };

        // Give the player time to pull themselves towards home planet. 
        _hitState.ChangeTo(_defuseState).After(3.0f);

        _defuseState.OnEnter = delegate {

            if (_turnSpeedCoroutine != null)
            {
                CoroutineHelper.Instance.StopCoroutine(_turnSpeedCoroutine);
            }

            GetComponent<TrailRenderer>().time = 0.1f;
            _rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            _audNegative.Play();
            Instantiate(_explodeFXPrefab, transform.position, transform.rotation);
        };

        // Give explode fx some time to play out. 
        _defuseState.ChangeTo(_selfDestructState).After(2.0f);

        _selfDestructState.OnEnter = delegate {
            if (_hit)
            {
                PlayerMovement.Instance.RegisterLoss();
                _waypoint.GetComponent<Renderer>().material.color = Color.white;
            }
            gameObject.SetActive(false);
        };
    }

    private void FixedUpdate ( )
    {
        _rootState.FixedUpdate();
        Debug.Log(_rootState.CurrentStatePath);
    }

    private void OnCollisionEnter ( Collision collision )
    {
        _col = collision;

        // Send message to go from propell to hit
        _propellState.SendSignal("hit");
    }
}
