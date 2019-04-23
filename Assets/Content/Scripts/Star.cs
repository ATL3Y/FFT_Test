using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Light l;
    [SerializeField]
    private GameObject prefab;
    private int band;
    private ParticleSystem particleSystem;

    private void Start ( )
    {
        // Assign each star a band from 0 to 7.
        band = Random.Range ( 0, 8 );

        if(
            (transform.parent.name.Contains("0") && (band == 6 || band == 7 ) )
            || ( transform.parent.name.Contains ( "1" ) && ( band == 2 || band == 3 ) )
            || ( transform.parent.name.Contains ( "2" ) && ( band == 0 || band == 1 ) )
            || transform.parent.name.Contains ( "3" ) // && ( band == 4 || band == 5 ) )
            ){

        }
        else
        {
            gameObject.SetActive ( false );
        }

        transform.localScale *= band;
        //l = GetComponent<Light> ( );
        //l.intensity = transform.lossyScale.x;
        //l.range = 10.0f * transform.lossyScale.x * transform.lossyScale.x;
        GameObject sparkle = GameObject.Instantiate(prefab);
        sparkle.transform.SetParent ( transform );
        sparkle.transform.localPosition = Vector3.zero;
        sparkle.transform.localRotation = Quaternion.identity;
        sparkle.transform.localScale = Vector3.one;
        particleSystem = sparkle.GetComponent<ParticleSystem> ( );

        float hue = 1.0f / 8.0f * band;
        Vector3 rgb = GraphicsHelper.HSVtoRGB ( new Vector3 ( hue, 1.0f, 1.0f ) );
        Color col = new Color(rgb.x, rgb.y, rgb.z, 1.0f);
        var main = particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient ( col, Color.white );
    }

    private void Update ( )
    {
        float val = AudioFFT.audioBandBuffer [ band ];
        var main = particleSystem.main;
        main.simulationSpeed = val;
        main.startSizeMultiplier = 1.6f * val * val;

    }

}
