using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip FlipAudio, MatchAudio, MismatchAudio, GameOverAudio;
    private AudioSource Source;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayFlip() => Source.PlayOneShot(FlipAudio);
    public void PlayMatch() => Source.PlayOneShot(MatchAudio);
    public void PlayMismatch() => Source.PlayOneShot(MismatchAudio);
    public void PlayGameOver() => Source.PlayOneShot(GameOverAudio);
}