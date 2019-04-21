using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMult;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ls = transform.localScale;
        transform.localScale = new Vector3 ( ls.x, ( AudioFFT.freqBand [ band ] * scaleMult ) + startScale, ls.z );
        float y = transform.localScale.y;
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3 ( pos.x, y / 2f, pos.z );
    }
}
