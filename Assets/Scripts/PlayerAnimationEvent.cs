using System.Collections;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;

    public void OnDeathAnimationFinished()
    {
        StartCoroutine(OnDeathAnimationFinishedCoroutine());
    }

    IEnumerator OnDeathAnimationFinishedCoroutine()
    {
        yield return new WaitForSeconds(2f);

        _playerController.OnDeathAnimationFinished();
    }
}