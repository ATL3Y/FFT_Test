using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This is a bad habit.
    private static PlayerMovement _instance;
    public static PlayerMovement Instance
    {
        get
        {
            return _instance;
        }
    }

    // Editor references
    [SerializeField]
    private GameObject _gun;

    [SerializeField]
    private Transform _root;

    [SerializeField]
    private Transform _head;

    [SerializeField]
    private Transform _lHand;

    [SerializeField]
    private Transform _rHand;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private List<Transform> _waypoints;

    // Logic
    private int _currentWaypointIndex;
    private Vector3 _oLHandPos;
    private Vector3 _oRHandPos;
    private int _currentHitCount = 0;
    private bool gunGone = false;

    // Tuning movement
    private float _targetWeight = 0.5f; // 0 = no weight, 1 = move to target
    private float _speed = 10.0f;
    private float _playerVelocity = 0.0f;
    private float _deceleration = 3.5f;
    private float _handVelocityMult = 1.7f;
    private float _maxVelocity = 14.0f;

    public void RegisterHit()
    {
        _currentHitCount += 1;
    }

    public void RegisterLoss()
    {
        if (_currentHitCount > 0)
        {
            _currentHitCount -= 1;
        }
    }

    public Transform GetTargetWaypoint(Vector3 sourcePos, Vector3 aimDir)
    {
        int index = _waypoints.Count - 1;
        float highestDot = -1.0f;

        for (int i = 0; i < _waypoints.Count; i++)
        {
            Vector3 toWaypoint = Vector3.Normalize(_waypoints[i].position - sourcePos);
            float dot = Vector3.Dot(aimDir, toWaypoint);
            if (dot > highestDot)
            {
                highestDot = dot;
                index = i;
            }
        }

        return _waypoints[index];
    }

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _oLHandPos = _lHand.transform.position;
        _oRHandPos = _rHand.transform.position;
        _currentWaypointIndex = _waypoints.Count - 1;
    }

    private void Update()
    {
        // Decay playerVelocity to simulate drag. 
        _playerVelocity = Mathf.MoveTowards ( _playerVelocity, 0.0f, _deceleration * Time.deltaTime );

        Vector3 targetDir = Vector3.Normalize(_target.position - _head.position);

        Vector3 handVelocity = _handVelocityMult * (_rHand.transform.position - _oRHandPos) / Time.deltaTime;

        // Calculate the targetDirection relative to the waypoint line, but move towards the planet.
        Vector3 targetPullDir = Vector3.Normalize(_waypoints[_currentWaypointIndex].position - _rHand.transform.position);
        if ( Vector3.Dot ( targetPullDir, handVelocity.normalized ) < 0.0f )
        {
            if( _currentHitCount > 0 && _playerVelocity < _maxVelocity )
            {
                _playerVelocity += Vector3.Magnitude ( handVelocity );
            }
        }

        _oRHandPos = _rHand.transform.position;

        // Slow down towards landing.
        float landingDist = Vector3.Distance ( _root.position, _target.position );
        float landingLength = 40.0f;
        float landingT = 1.0f;
        if ( landingDist < landingLength )
        {
            // For starting to land, landingT = 1. For landed, landingT = 0f.
            landingT = landingDist / landingLength;
            landingT = Mathf.Max ( .25f, landingT );

            // Remove player gun.
            if(!gunGone && landingDist < 1f )
            {
                gunGone = true;
                _gun.SetActive ( false );
                GetComponent<AudioSource> ( ).Play ( );
            }
        }

        // Add playerVelocity to the player's position in terms of time. 
        _root.position += landingT * _playerVelocity * Time.deltaTime * targetDir;
    }
}
