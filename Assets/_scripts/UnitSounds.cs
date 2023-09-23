using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSounds : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip stepClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        audioSource.clip = stepClip;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

}
