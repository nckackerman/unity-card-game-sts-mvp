using UnityEngine;
public class AudioState
{
    public static AudioSource audioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
    public static AudioClip cardPlayed = Resources.Load<AudioClip>("audio/cardPlayed");

    public void onCardPlayed()
    {
        audioSource.clip = cardPlayed;
        audioSource.Play();
    }
}