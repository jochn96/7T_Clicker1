using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;
    public void Play(AudioClip clip, float effectVolume)
    {
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            CancelInvoke();
            audioSource.clip = clip;
            audioSource.volume = effectVolume;
            audioSource.Play();

            Invoke("Disable", clip.length);
        }
    }
    public void Disable()
    {
        audioSource.Stop();
        Destroy(this.gameObject);
    }
}
