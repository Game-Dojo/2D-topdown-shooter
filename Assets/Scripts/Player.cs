using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public float movementSpeed = 3f;
    public float rotationSpeed = 5f;

    [Header("Bullets")]
    public Transform bulletSpawnPosition;
    
    private Camera _camera;
    private PlayerWeapon _playerWeapon;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    
    private float _health = 100f;
    private bool _isDead = false;
    
    private void Awake()
    {
        _camera = Camera.main;
        _playerWeapon = GetComponent<PlayerWeapon>();
        
        _gameManager = FindAnyObjectByType<GameManager>();
        _audioManager = _gameManager.GetAudioManager;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0);

        MoveTowards(movement);
        RotateTowardsMouse();
    }

    void MoveTowards(Vector3 target)
    {
        transform.position += target * (movementSpeed * Time.deltaTime);
    }

    void RotateTowardsMouse()
    {
        Vector3 mouseToWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        mouseToWorld.z = 0;
        
        //transform.right = mouseToWorld - transform.position;
        transform.right = Vector3.Lerp(transform.right, mouseToWorld - transform.position, rotationSpeed * Time.deltaTime);
    }

    public void Hurt()
    {
        if (_isDead) return;
        _audioManager.PlayAudioPitched(AudioManager.AudioList.Hurt);
        _health -= 5.0f;
        if (_health <= 0) _isDead = true;
    }
}
