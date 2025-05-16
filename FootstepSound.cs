using System.Collections;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioClip[] footstepSounds; // Assign footstep sound files in Inspector
    public AudioSource audioSource;    // Assign AudioSource in Inspector
    public float footstepInterval = 0.4f; // Adjust based on movement speed
    public float fadeDuration = 0.3f;  // Adjust fade-in/out time

    private bool isWalking = false;
    private float stepTimer = 0f;

    void Update()
    {
        if (IsPlayerMoving())
        {
            if (!isWalking)
            {
                isWalking = true;
                StartCoroutine(FadeInSound());
            }

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                PlayRandomFootstep();
                stepTimer = footstepInterval; // Reset timer
            }
        }
        else if (isWalking)
        {
            isWalking = false;
            StartCoroutine(FadeOutSound());
        }
    }

    bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    void PlayRandomFootstep()
    {
        if (footstepSounds.Length == 0) return;

        int index = Random.Range(0, footstepSounds.Length);
        audioSource.PlayOneShot(footstepSounds[index]);
    }

    IEnumerator FadeInSound()
    {
        float startVolume = 0f;
        float targetVolume = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    IEnumerator FadeOutSound()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
    }
}
