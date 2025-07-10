using System;
using System.Collections;
using UnityEngine;
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
    private const int MaxBulletsChamber = 5;
    
    private int _totalBullets = 15;
    private int _currentBullets = 5;
    private Vector3 _lastPointPosition;
    
    private Animator _animator;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    
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
        
        //bulletSound.PlayOneShot(bulletSound.clip);
        _audioManager.PlayAudioPitched(AudioManager.AudioList.Correct);
        
        _currentBullets -= 1;
        _animator.SetTrigger("Shoot");
        InstantiateBullet();
        StartCoroutine("MuzzleEffect");
    }

    void Reload()
    {
        if (_totalBullets < MaxBulletsChamber) return;
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

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
