using UnityEngine;

public class HeadAudioScaler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform headBone;
    [SerializeField] private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private float smoothSpeed = 15f;

    private Vector3 originalScale;
    private float[] samples = new float[256];

    private void Awake()
    {
        originalScale = headBone.localScale;
    }

    private void LateUpdate()
    {
        // Get audio samples from the AudioSource
        audioSource.GetOutputData(samples, 0);

        // Calculate volume (RMS)
        float sum = 0f;

        foreach (float sample in samples)
        {
            sum += sample * sample;
        }

        float volume = Mathf.Sqrt(sum / samples.Length);

        // Convert volume to scale
        float scale = Mathf.Clamp(
            1f + volume * sensitivity,
            1f,
            maxScale
        );

        // Smoothly scale the head
        Vector3 targetScale = originalScale * scale;

        headBone.localScale = Vector3.Lerp(
            headBone.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }
}