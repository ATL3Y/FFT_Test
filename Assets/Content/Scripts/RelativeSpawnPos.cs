using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeSpawnPos : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
