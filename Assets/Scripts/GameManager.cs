using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform enemiesContainer;
    
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
    }

    public Player GetPlayer => player;
    
    public AudioManager GetAudioManager => _audioManager;
}
