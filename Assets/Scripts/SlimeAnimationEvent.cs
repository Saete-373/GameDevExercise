using UnityEngine;

public class SlimeAnimationEvent : MonoBehaviour
{
    [SerializeField] SlimeController _slimeController;
    [SerializeField] Collider2D _attackCollider;

    public void OnOpenAttackCollider()
    {
        _attackCollider.enabled = true;
    }

    public void OnCloseAttackCollider()
    {
        _attackCollider.enabled = false;
    }

    public void OnAttackAnimationFinished()
    {
        _slimeController.OnAttackAnimationFinished();
    }
}
