using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text bulletCountText;

    private GameManager _gameManager;
    private PlayerWeapon _playerWeapon;
    
    private void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _playerWeapon = _gameManager.GetPlayer.GetWeapon;
        
        // Add listeners
        _playerWeapon.GetReloadEvent.AddListener(() =>
        {
            SetBulletCount(_playerWeapon.GetTotalBullets);
        });
    }

    private void SetBulletCount(int count)
    {
        bulletCountText.text = count.ToString();
    }
}
