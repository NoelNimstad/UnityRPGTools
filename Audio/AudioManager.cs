using UnityEngine;

[RequireComponent(typeof(AudioSource))] public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else 
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // such that it continues acros scenes
        }
    }

    public void ChangeMusicAndPlay(AudioClip audio) // call function if you want to change the music playing
    {   
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void Pause() // call function if you want to pause the music
    {
        audioSource.Pause();
    }

    public void Play() // call the function if you want to resume the current music
    {
        audioSource.Play();
    }
}
