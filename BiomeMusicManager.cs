using UnityEngine;
using System.Collections;

public class BiomeMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip forestMusic;
    public AudioClip oceanMusic;
    public AudioClip mushroomMusic;
    
    private string currentBiome = "";
    private Coroutine fadeCoroutine;
    private float fadeDuration = 3f; // Time in seconds for fade-in/out

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0; // Start silent
    }

    public void ChangeBiome(string biome)
    {
        if (biome == currentBiome) return; // Don't restart music if already playing

        currentBiome = biome;
        AudioClip newClip = GetBiomeMusic(biome);

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SwitchMusicWithFade(newClip));
    }

    private AudioClip GetBiomeMusic(string biome)
    {
        switch (biome)
        {
            case "Forest": return forestMusic;
            case "Ocean": return oceanMusic;
            case "Mushroom": return mushroomMusic;
            default: return null;
        }
    }

    private IEnumerator SwitchMusicWithFade(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // Fade Out Current Track
            yield return StartCoroutine(FadeAudio(1f, 0f));
            audioSource.Stop();
        }

        if (newClip != null)
        {
            // Change to New Track & Fade In
            audioSource.clip = newClip;
            audioSource.Play();
            yield return StartCoroutine(FadeAudio(0f, 1f));
        }
    }

    private IEnumerator FadeAudio(float startVolume, float endVolume)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = endVolume;
    }
}
