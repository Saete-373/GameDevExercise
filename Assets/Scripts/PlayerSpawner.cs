using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] HealthBar _healthBar;

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);

        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Player prefab has no PlayerController.", player);

            Destroy(player);
            return;
        }

        PlayerManager.Instance.SetPlayer(playerController);

        _healthBar.SetPlayer(playerController);
    }
}