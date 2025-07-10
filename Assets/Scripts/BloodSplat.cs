using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    [SerializeField] private int frameCount = 10;
    
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetFrame(Random.Range(0, frameCount-1));
    }

    private void SetFrame(int frame)
    {
        _animator.Play("Splash", 0, (float) frame / frameCount);
        _animator.speed = 0f;
    }
}
