using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFFT : MonoBehaviour
{

    AudioSource _audioSource;
    public static float[] samples = new float[512];

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource> ( );
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource ( );
    }

    private void GetSpectrumAudioSource ( )
    {
        _audioSource.GetSpectrumData (samples, 0, FFTWindow.Blackman );
    }
}
