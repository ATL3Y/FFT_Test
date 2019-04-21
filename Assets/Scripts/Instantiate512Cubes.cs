using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    [SerializeField]
    private GameObject _sampleCubePrefab;

    private GameObject[] _sampleCube = new GameObject[512];

    public float maxScale;

    private 
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< 512; i++ )
        {
            GameObject instanceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
            instanceSampleCube.transform.position = this.transform.position;
            instanceSampleCube.transform.parent = this.transform;
            instanceSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3 ( 0, -.703125f * i, 0 );
            instanceSampleCube.transform.position = Vector3.forward * 100f;
            _sampleCube [ i ] = instanceSampleCube;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<512; i++ )
        {
            if(_sampleCube != null )
            {
                _sampleCube [ i ].transform.localScale = new Vector3 ( 1f, ( AudioFFT.samples [ i ] * maxScale ) + 2f, 1f );
                float y = _sampleCube [ i ].transform.localScale.y;
                Vector3 pos = _sampleCube[i].transform.localPosition;
                _sampleCube [ i ].transform.localPosition = new Vector3 ( pos.x, y / 2f, pos.z );
            }
        }
    }
}
