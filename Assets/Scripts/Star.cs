using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Light l;
    [SerializeField]
    GameObject prefab;

    // Use this for initialization
    void Start ( )
    {
        //l = GetComponent<Light> ( );
        //l.intensity = transform.lossyScale.x;
        //l.range = 10.0f * transform.lossyScale.x * transform.lossyScale.x;
        GameObject sparkle = GameObject.Instantiate(prefab);
        sparkle.transform.SetParent ( transform );
        sparkle.transform.localPosition = Vector3.zero;
        sparkle.transform.localRotation = Quaternion.identity;
        sparkle.transform.localScale = Vector3.one;
    }
}
