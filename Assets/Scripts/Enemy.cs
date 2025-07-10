using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Idle,
        Wonder,
        Follow,
        Attack
    }
    
    private State _state = State.Idle;
    
    private bool _isInArea = false;
    private bool _isAttacking = false;
    
    private GameManager _gameManager;
    private Player _player;
    
    private Animator _animator;
    private AudioSource _voiceSound;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _voiceSound = GetComponentInChildren<AudioSource>();
        
        _gameManager = FindAnyObjectByType<GameManager>();
        _player = _gameManager.GetPlayer;
    }
    
    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Wonder:
                break;
            case State.Follow:
                FollowState();
                break;
            case State.Attack:
                AttackState();
                break;
            default:
                IdleState();
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _isInArea = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _isInArea = false;
    }

    #region State Machine
    private void IdleState()
    {
        if (_isInArea) SetState(State.Follow);
    }

    private void FollowState()
    {
        // Movement
        transform.right = _player.transform.position - transform.position;
        
        // Check for Walls
        if (!WallsOnTheWay())
            transform.position += transform.right * (1.5f * Time.deltaTime);
        
        // Check if players exit trigger area
        if (!_isInArea) SetState(State.Idle);
        if (InAttackRange()) SetState(State.Attack);
    }

    private void AttackState()
    {
        if (_isAttacking) return;
        StartCoroutine("AttackProcess");
        print("Attack State");
    }

    private IEnumerator AttackProcess()
    {
        _isAttacking = true;
        _animator.SetBool("Attack", true);
        _voiceSound.PlayOneShot(_voiceSound.clip);
        yield return new WaitForSeconds(1.5f);
        _animator.SetBool("Attack", false);
        _isAttacking = false;
        
        SetState(State.Idle);
    }

    private void SetState(State newState)
    {
        _state = newState;
    }
    #endregion
    
    #region Utils
    private bool InAttackRange()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < 1.5f;
    }
    
    private bool WallsOnTheWay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 5.0f);
        if (hit)
        {
            bool isPlayer = hit.collider.gameObject.CompareTag("Player");
            //Debug.DrawRay(transform.position, transform.right * 5.0f, Color.red);
            return !isPlayer;
        }
        
        //Debug.DrawRay(transform.position, transform.right * 5.0f, Color.green);
        return false;
    }
    #endregion

}
