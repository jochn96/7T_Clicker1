using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] public float musicVolume;

    private AudioSource musicAudioSource;
    public AudioClip[] musicClips;  //배경음악을 배열로 저장
    public SoundSource soundSourcePrefab;
    public Slider musicVolumeSlider;

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = musicVolume;
    }

    public void MusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        musicAudioSource.volume = musicVolume;
    }

    public void ChangeBackGroundMusic(int index)  //배열에 있는 음악들을 상황에 맞춰 실행하기위한 코드
    {
        if (musicClips == null || musicClips.Length == 0)
        {
            return;
        }
        if (index < 0 || index >= musicClips.Length)
        {
            return;
        }

        musicAudioSource.Stop();
        musicAudioSource.clip = musicClips[index];
        musicAudioSource.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(soundSourcePrefab);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, soundEffectVolume, soundEffectPitchVariance);
    }
}
