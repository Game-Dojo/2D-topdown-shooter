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
    
    private void Awake()
    {
        _camera = Camera.main;
        _playerWeapon = GetComponent<PlayerWeapon>();
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
}
