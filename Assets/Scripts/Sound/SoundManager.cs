using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] public float musicVolume;

    public const float DEFALT_VOLUME = 0.3f;
    private AudioSource musicAudioSource;
    public AudioClip[] musicClips;  //배경음악을 배열로 저장
    public SoundSource soundSourcePrefab;
    public Transform soundSlot;
    public Slider musicVolumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        musicAudioSource = GetComponent<AudioSource>();
        if (musicVolume == 0) musicVolume = DEFALT_VOLUME;
        musicAudioSource.loop = true;
        musicAudioSource.volume = musicVolume;
    }
    
    private void Start()
    {
        MusicVolume();
        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = DEFALT_VOLUME;
    }

    public void MusicVolume()
    {
        if (musicVolumeSlider == null)
        {
            musicVolume = DEFALT_VOLUME;
            return;
        }

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
        if (musicVolume == 0)
        {
            return;
        }
        SoundSource obj = Instantiate(soundSourcePrefab, soundSlot);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, musicVolume);
    }
}
