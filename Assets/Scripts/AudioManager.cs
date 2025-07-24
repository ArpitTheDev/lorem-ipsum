using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip flip, match, mismatch, gameOver;
    private AudioSource source;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            source = GetComponent<AudioSource>();
        }
        else Destroy(gameObject);
    }

    public void PlayFlip() => source.PlayOneShot(flip);
    public void PlayMatch() => source.PlayOneShot(match);
    public void PlayMismatch() => source.PlayOneShot(mismatch);
    public void PlayGameOver() => source.PlayOneShot(gameOver);
}