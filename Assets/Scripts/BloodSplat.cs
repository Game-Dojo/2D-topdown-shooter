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

        _animator.speed = 1.0f;
        _animator.Play("FadeBloodSplat");
        Invoke("Hide", 5.0f);
    }

    private void SetFrame(int frame)
    {
        _animator.Play("Splash", 0, (float) frame / frameCount);
        _animator.speed = 0f;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
