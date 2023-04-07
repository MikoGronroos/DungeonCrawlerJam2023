using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    [SerializeField] private AudioSource source;

    public void PlaySoundOnce()
    {
        source.Play();
    }

}
