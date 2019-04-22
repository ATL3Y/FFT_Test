using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( AudioSource ) )]
public class AudioFFT : MonoBehaviour
{

    AudioSource _audioSource;
    public static float[] samples = new float[512];
    private float[] _freqBand = new float[8];
    private float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    private float[] _freqBandHighest = new float[8];
    public static float[] audioBand = new float [8];
    public static float[] audioBandBuffer = new float[8];


    // Start is called before the first frame update
    void Start ( )
    {
        _audioSource = GetComponent<AudioSource> ( );
        GetSpectrumAudioSource ( );
        MakeFrequencyBands ( );
        BandBuffer ( );
        CreateAudioBands ( );
    }

    // Update is called once per frame
    void Update ( )
    {
        GetSpectrumAudioSource ( );
        MakeFrequencyBands ( );
        BandBuffer ( );
        CreateAudioBands ( );
    }

    private void CreateAudioBands ( )
    {
        for ( int i = 0; i < 8; i++ )
        {
            if ( _freqBand [ i ] > _freqBandHighest [ i ] )
            {
                _freqBandHighest [ i ] = _freqBand [ i ];
            }
            audioBand [ i ] = ( _freqBand [ i ] / _freqBandHighest [ i ] );
            audioBandBuffer [ i ] = ( _bandBuffer [ i ] / _freqBandHighest [ i ] );
        }
    }

    private void GetSpectrumAudioSource ( )
    {
        _audioSource.GetSpectrumData ( samples, 0, FFTWindow.Blackman );
    }

    private void MakeFrequencyBands ( )
    {
        /*
         * TODO: Write an algorithm that takes in the range of a given song. 
         * Start with the Hz of the song you have, eg 2050
         * Then divide that by the number of bands you have, eg 2050 / 512 = 43 Hz per sample
         * 
         * "Frequency bands" fall nicely into the following ranges: 
         * 20-60 Hz
         * 60-250 Hz
         * 250-500 Hz
         * 500-2000 Hz
         * 2000-4000 Hz
         * 4000-6000 Hz
         * 6000-20000 Hz
         * But we're gonna split the last range into two to make 8 bands instead of 7. 
         * 
         * Try to fit the Hz for your song into eight bands instead of 512 using powers of 2, eg:
         *  
         * 0 (20-60 Hz) => 2 * 43 = 86 Hz => 0 to 86
         * 1 (60-250 Hz) => 4 * 43 = 172 Hz => 87 to 258
         * 2 (250-500 Hz) => 8 * 43 = 344 Hz => 259 to 602
         * 3 (500-2000 Hz) => 16 * 43 = 688 Hz => 603 to 1290
         * 4 (2000-4000 Hz) => 32 * 43 = 1376 Hz => 1291 to 2666
         * 5 (4000-6000 Hz) => 64 * 43 = 2752 Hz => 2667 to 5418
         * 6 (6000-11000 Hz) => 128 * 43 = 5504 Hz => 5419 to 10922
         * 7 (11000-22000) => 256 * 43 = 11008 Hz => 10923 to 21930
         * 
         * The total used now is 510 samles.  To use all 512, add the last two to the last band:
         * 7 (11000-22000) => (256 + 2) * 43 = 11094 Hz => 10923 to 22016
        */

        int count = 0;

        for ( int i = 0; i < 8; i++ )
        {
            float average = 0f;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if ( i == 7 )
            {
                sampleCount += 2;
            }

            for ( int j = 0; j < sampleCount; j++ )
            {
                average += samples [ count ] * ( count + 1 );
                count++;
            }
            average /= count;
            _freqBand [ i ] = average * 10f;
        }

    }

    private void BandBuffer ( )
    {
        for ( int i = 0; i < 8; ++i )
        {
            if ( _freqBand [ i ] > _bandBuffer [ i ] )
            {
                _bandBuffer [ i ] = _freqBand [ i ];
                _bufferDecrease [ i ] = .005f;
            }
            if ( _freqBand [ i ] < _bandBuffer [ i ] )
            {
                _bandBuffer [ i ] -= _bufferDecrease [ i ];
                _bufferDecrease [ i ] *= 1.2f;
            }
        }
    }
}
