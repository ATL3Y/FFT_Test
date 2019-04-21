using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMult;
    public bool useBuffer;
    private Material _mat;

    // Start is called before the first frame update
    void Start ( )
    {
        _mat = GetComponent<Renderer> ( ).material;
        if ( !_mat ) Debug.LogWarning ( "No material found" );
    }

    // Update is called once per frame
    void Update ( )
    {
        float _val = 0.0f;
        if ( useBuffer )
        {

            _val = AudioFFT.audioBandBuffer [ band ];
            transform.localScale = new Vector3 ( transform.localScale.x, ( _val * scaleMult ) + startScale, transform.localScale.z );
        }
        else
        {
            _val = AudioFFT.audioBand [ band ];
            transform.localScale = new Vector3 ( transform.localScale.x, ( _val * scaleMult ) + startScale, transform.localScale.z );
        }

        Color _col = new Color(_val, _val, _val);
        _mat.SetColor ( "_EmissionColor", _col );

        float y = transform.localScale.y;
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3 ( pos.x, y / 2f, pos.z );
    }
}
