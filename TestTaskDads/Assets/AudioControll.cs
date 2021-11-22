using UnityEngine.UI;
using UnityEngine;

public class AudioControll : MonoBehaviour
{
    [SerializeField] private AudioSource[] _effectsSources;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private Slider _music;
    [SerializeField] private Slider _effects;

    void Update()
    {
        _musicSource.volume = _music.value;
        foreach(AudioSource a in _effectsSources)
        {
            a.volume = _effects.value;
        }
    }
}
