using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public Transform bulletContainer;
    
    [Header("Effects")]
    public Light2D muzzleFlash;
    public LineRenderer laserRenderer;
    
    [Header("Audio")]
    public AudioSource bulletSound;
    
    private const float LaserDistance = 8.0f;
    private const int MaxBulletsChamber = 6;
    
    private int _totalBullets = 24;
    private int _currentBullets = 6;
    private Vector3 _lastPointPosition;
    
    private Animator _animator;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    
    private UnityEvent _onShoot = new UnityEvent();
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _audioManager = _gameManager.GetAudioManager;
        
        muzzleFlash.intensity = 0;
        _lastPointPosition = laserRenderer.GetPosition(1);
    }
    
    void DrawLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPosition.position, transform.right, LaserDistance);
        if (hit)
        {
            Debug.DrawLine(bulletSpawnPosition.position, hit.point, Color.green);
            laserRenderer.SetPosition(1, (Vector3) transform.InverseTransformPoint(hit.point));
            return;
        }
        
        laserRenderer.SetPosition(1, _lastPointPosition);
        Debug.DrawRay(bulletSpawnPosition.position, transform.right * LaserDistance, Color.red);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        
        DrawLaser();
    }
    
    void Shoot()
    {
        if (_currentBullets <= 0) return;
        _onShoot?.Invoke();
        _audioManager.PlayAudioPitched(AudioManager.AudioList.Shoot);
        
        _currentBullets -= 1;
        _animator.SetTrigger("Shoot");
        InstantiateBullet();
        StartCoroutine("MuzzleEffect");
    }

    void Reload()
    {
        if (_totalBullets < MaxBulletsChamber) return;
        _audioManager.PlayAudioPitched(AudioManager.AudioList.Reload);
        StartCoroutine("ReloadCoroutine");
    }

    private IEnumerator ReloadCoroutine()
    {
        _animator.SetTrigger("Reload");
        yield return new WaitForSeconds(1.0f);
        _totalBullets -= MaxBulletsChamber;
        _currentBullets = MaxBulletsChamber;
    }
    
    void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
        if (!bullet) return;
        bullet.transform.parent = bulletContainer;
        bullet.transform.right = bulletSpawnPosition.right;
    }
    
    private IEnumerator MuzzleEffect()
    {
        muzzleFlash.intensity = Random.Range(6.0f, 8.0f);
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.intensity = 0f;
    }

    public UnityEvent GetShootEvent => _onShoot;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
