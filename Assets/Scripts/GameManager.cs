using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GetComponent<AudioManager>();
    }

    public Player GetPlayer => player;
    
    public AudioManager GetAudioManager => _audioManager;
}
