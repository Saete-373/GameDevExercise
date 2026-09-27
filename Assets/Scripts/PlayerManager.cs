using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerController CurrentPlayer { get; private set; }

    [SerializeField] PlayerSpawner _playerSpawner;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPlayer(PlayerController player)
    {
        if (CurrentPlayer != null)
        {
            CurrentPlayer.OnDeath -= HandlePlayerDeath;
        }

        CurrentPlayer = player;

        CurrentPlayer.OnDeath += HandlePlayerDeath;
    }

    public void ClearPlayer()
    {
        if (CurrentPlayer == null)
            return;

        CurrentPlayer.OnDeath -= HandlePlayerDeath;

        CurrentPlayer = null;
    }

    void HandlePlayerDeath()
    {
        PlayerController deadPlayer = CurrentPlayer;

        ClearPlayer();

        Destroy(deadPlayer.gameObject);

        GameTimeManager.Instance.ResetDay();

        _playerSpawner.SpawnPlayer();
    }
}