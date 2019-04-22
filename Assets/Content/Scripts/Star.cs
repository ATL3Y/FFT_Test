﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Light l;
    [SerializeField] private GameObject prefab;
    private int band;
    private ParticleSystem particleSystem;

    private void Start ( )
    {
        // Assign each star a band from 0 to 7.
        band = Random.Range ( 0, 7 );
        

        //l = GetComponent<Light> ( );
        //l.intensity = transform.lossyScale.x;
        //l.range = 10.0f * transform.lossyScale.x * transform.lossyScale.x;
        GameObject sparkle = GameObject.Instantiate(prefab);
        sparkle.transform.SetParent ( transform );
        sparkle.transform.localPosition = Vector3.zero;
        sparkle.transform.localRotation = Quaternion.identity;
        sparkle.transform.localScale = Vector3.one;
        particleSystem = sparkle.GetComponent<ParticleSystem> ( );
    }

    private void Update ( )
    {
        float val = AudioFFT.audioBandBuffer [ band ];
        var main = particleSystem.main;
        main.simulationSpeed = val;
    }
}
