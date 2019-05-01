using UnityEngine;

public class RelativeSpawnPos : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;

    private void Start()
    {
        transform.position = _target.transform.position + _offset;
    }
}
