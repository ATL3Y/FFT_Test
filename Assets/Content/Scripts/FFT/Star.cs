using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    private int _band;
    private ParticleSystem _particleSystem;

    private void Start ( )
    {
        // Assign each star a band from 0 to 7.
        _band = Random.Range ( 0, 8 );

        if((transform.parent.name.Contains("0") && (_band == 6 || _band == 7 ) )
            || ( transform.parent.name.Contains ( "1" ) && ( _band == 2 || _band == 3 ) )
            || ( transform.parent.name.Contains ( "2" ) && ( _band == 0 || _band == 1 ) )
            || transform.parent.name.Contains ( "3" ))
        {
            // If our zone already alligns with our band, stay active. 
        }
        else
        {
            gameObject.SetActive ( false );
        }

        transform.localScale *= _band;

        GameObject sparkle = Instantiate(_prefab);
        sparkle.transform.SetParent ( transform );
        sparkle.transform.localPosition = Vector3.zero;
        sparkle.transform.localRotation = Quaternion.identity;
        sparkle.transform.localScale = Vector3.one;
        _particleSystem = sparkle.GetComponent<ParticleSystem> ( );

        float hue = 1.0f / 8.0f * _band;
        Vector3 rgb = GraphicsHelper.HSVtoRGB ( new Vector3 ( hue, 1.0f, 1.0f ) );
        Color col = new Color(rgb.x, rgb.y, rgb.z, 1.0f);
        var main = _particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient ( col, Color.white );
    }

    private void Update ( )
    {
        float val = AudioFFT.audioBandBuffer [ _band ];
        var main = _particleSystem.main;
        main.simulationSpeed = val;
        main.startSizeMultiplier = 1.6f * val * val;
    }
}
