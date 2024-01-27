using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClip : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private List<AudioClip> _clips;

    public void PlayRandomClip()
    {
        _source.clip = _clips[Random.Range(0, _clips.Count)];
        _source.Play();
    }
}
