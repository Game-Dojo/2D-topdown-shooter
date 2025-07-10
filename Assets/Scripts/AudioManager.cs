using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public enum AudioList
    {
        Congratulations,
        Correct,
        FinalRound,
        GameOver,
        Go,
        Shoot,
        Hurt,
        Reload
    }
    
    [SerializeField] private List<AudioClip> audioClips;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    
    [SerializeField] private AudioMixer _mainMixer;

    private bool _masterMuted = false;
    private bool _musicMuted = false;
    private bool _soundMuted = false;
    
    public void PlayAudio(AudioList clipName, float pitch = 1.0f, bool oneShot = true)
    {
        _sfxSource.pitch = pitch;
        _sfxSource.PlayOneShot(audioClips[(int) clipName]);
    }

    public void PlayAudioPitched(AudioList clipName, bool oneShot = true)
    {
        float randomPitch = Random.Range(0.9f, 1.1f);
        PlayAudio(clipName, randomPitch, oneShot);
    }

    public void SetMasterVolume(float volume = 1.0f)
    {
        _mainMixer.SetFloat("MasterVolume", volume);
    }
    
    public void ToggleMusic()
    {
        _musicMuted = !_musicMuted;
        SetMixerState("MusicVolume", _musicMuted);
    }

    public void ToggleSfx()
    {
        _soundMuted = !_soundMuted;
        SetMixerState("SFXVolume", _soundMuted);
    }
    public void ToggleMaster()
    {
        _masterMuted = !_masterMuted;
        SetMixerState("MasterVolume", _masterMuted);
    }

    private void SetMixerState(string parameter, bool muted)
    {
        _mainMixer.SetFloat(parameter, (muted) ? -80.0f : 0f);
    }
}
