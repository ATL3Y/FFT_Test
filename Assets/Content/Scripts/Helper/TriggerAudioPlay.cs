using UnityEngine;

public class TriggerAudioPlay : MonoBehaviour
{
    [SerializeField]
    private bool _fade = false;

    [SerializeField]
    private float _targetVolume = 0.7f;

    private bool _played = false;
    private AudioSource _aud;

    private void Start()
    {
        _aud = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_fade && _played && _aud.volume < _targetVolume)
        {
            // TODO: REPLACE
            _aud.volume = Mathf.Lerp(_aud.volume, _targetVolume, Time.deltaTime);
        }
    }

    private void OnTriggerEnter ( Collider other )
    {
        if ( _played )
            return;

        if ( other.gameObject.layer == 9 ) // Player hands.
        {
            _played = true;
            if (!_fade)
            {
                _aud.Play();
            }
        }
    }
}
