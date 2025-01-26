using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    Kick,
    Spawn,
    Slip,
    Select,
    SoftImpact,
    Spring
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioClip> sfxClips;
    public List <AudioClip> mscClips;

    public AudioSource sfxSource;
    public AudioSource mscSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        { 
            Destroy(gameObject);  
        }
    }

    public void PlaySfx(SFX sfx, bool pitchVar)
    {
        if(pitchVar)
        {
            sfxSource.pitch = Random.Range(0.8f,1.2f);
        }
        else
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }

    public void PlayMsc(int mscIndex)
    {
        mscSource.clip = mscClips[mscIndex];
    }
}
