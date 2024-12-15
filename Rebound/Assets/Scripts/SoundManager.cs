using System;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] float volumeCoeff = 0.5f;
    public static SoundManager Instance;
    public AudioClip pop;
    public AudioClip win;
    public AudioClip backBtn;
    public AudioClip click;
    [SerializeField] private AudioSource sfxSource;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, volumeCoeff*PlayerPrefs.GetFloat("volume"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
