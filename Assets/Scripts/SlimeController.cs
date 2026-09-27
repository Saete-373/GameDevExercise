using System.Collections;
using UnityEngine;

public enum SlimeState
{
    Patrol,
    ReturnToPatrol,
    Chase,
    Attack
}

public class SlimeController : MonoBehaviour, IDamagable
{
    [Header("Health")]
    [SerializeField] float _health = 20f;

    [Header("Attack")]
    public float AttackDamage = 5f;
    [SerializeField] float _attackRange = 3f;

    [Header("Patrol")]
    [SerializeField] float _patrolSpeed = 2f;
    [SerializeField] float _patrolRange = 8f;
    [SerializeField] float _patrolWaitTime = 2f;
    [SerializeField] float _patrolMinDistance = 2f;
    [SerializeField] Vector2 _patrolStartPos;
    Vector2 _patrolTargetPos;
    float _patrolDirection = 1f;

    [Header("Chase")]
    [SerializeField] float _chaseSpeed = 3f;

    [Header("Death")]
    [SerializeField] GameObject _deathSpawnPrefab;
    [SerializeField] int _deathSpawnCount;

    [Header("Animation")]
    [SerializeField] Animator _anim;

    bool _isHit;
    SlimeState _currentState = SlimeState.Patrol;
    Transform _player;
    [SerializeField] float _yPosOffsetTuning = 0f;
    Coroutine _stateCoroutine;


    void Start()
    {
        _patrolStartPos = transform.position + new Vector3(0, _yPosOffsetTuning, 0);

        EnterState(_currentState);
    }


    void Update()
    {
        switch (_currentState)
        {
            case SlimeState.Chase:
                UpdateChase();
                break;

            case SlimeState.Attack:
                UpdateAttack();
                break;
        }
    }



    #region State Management

    public void ChangeState(SlimeState newState)
    {
        if (_currentState == newState)
            return;

        ExitState(_currentState);

        _currentState = newState;

        EnterState(_currentState);
    }


    void EnterState(SlimeState state)
    {
        switch (state)
        {
            case SlimeState.Patrol:
                StartPatrol();
                break;

            case SlimeState.ReturnToPatrol:
                StartReturnToPatrol();
                break;

            case SlimeState.Chase:
                break;

            case SlimeState.Attack:
                StartAttack();
                break;
        }
    }


    void ExitState(SlimeState state)
    {
        switch (state)
        {
            case SlimeState.Patrol:
            case SlimeState.ReturnToPatrol:
                StopStateCoroutine();
                break;

            case SlimeState.Attack:
                CancelAttack();
                break;
        }
    }


    public bool CheckSlimeState(SlimeState state)
    {
        return _currentState == state;
    }

    #endregion


    #region Patrol

    void StartPatrol()
    {
        StopStateCoroutine();

        _stateCoroutine = StartCoroutine(PatrolRoutine());
    }


    IEnumerator PatrolRoutine()
    {
        while (_currentState == SlimeState.Patrol)
        {
            yield return new WaitForSeconds(_patrolWaitTime);

            if (_currentState != SlimeState.Patrol)
                yield break;

            _patrolTargetPos = GetNextPatrolPosition();

            while (_currentState == SlimeState.Patrol)
            {
                Vector2 currentPos = transform.position;

                Debug.DrawLine(currentPos, _patrolTargetPos, Color.green);

                if (HasReachedPosition(currentPos, _patrolTargetPos))
                {
                    transform.position = _patrolTargetPos;
                    break;
                }

                transform.position = Vector2.MoveTowards(
                    currentPos,
                    _patrolTargetPos,
                    _patrolSpeed * Time.deltaTime
                );

                UpdateFacingDirection(_patrolTargetPos.x - currentPos.x);

                yield return null;
            }
        }
    }


    Vector2 GetNextPatrolPosition()
    {
        float distance = Random.Range(_patrolMinDistance, _patrolRange);

        float targetX = transform.position.x + (_patrolDirection * distance);

        float minX = _patrolStartPos.x - _patrolRange;
        float maxX = _patrolStartPos.x + _patrolRange;

        if (targetX < minX || targetX > maxX)
        {
            _patrolDirection *= -1f;

            targetX = transform.position.x + (_patrolDirection * distance);

            targetX = Mathf.Clamp(targetX, minX, maxX);
        }

        _patrolDirection *= -1f;

        return new Vector2(targetX, _patrolStartPos.y);
    }

    #endregion


    #region Return To Patrol

    void StartReturnToPatrol()
    {
        StopStateCoroutine();

        _stateCoroutine = StartCoroutine(ReturnToPatrolRoutine());
    }


    IEnumerator ReturnToPatrolRoutine()
    {
        while (_currentState == SlimeState.ReturnToPatrol)
        {
            Vector2 currentPos = transform.position;

            if (HasReachedPosition(currentPos, _patrolStartPos))
            {
                transform.position = _patrolStartPos;
                break;
            }

            transform.position = Vector2.MoveTowards(
                currentPos,
                _patrolStartPos,
                _patrolSpeed * Time.deltaTime
            );

            UpdateFacingDirection(_patrolStartPos.x - currentPos.x);

            yield return null;
        }

        if (_currentState == SlimeState.ReturnToPatrol)
        {
            _player = null;
            ChangeState(SlimeState.Patrol);
        }
    }

    #endregion


    #region Target Management

    public void SetTarget(Transform target)
    {
        if (target == null)
            return;

        _player = target;

        if (_currentState == SlimeState.Patrol)
        {
            ChangeState(SlimeState.Chase);
        }
    }


    public void ClearTarget()
    {
        _player = null;

        if (_currentState == SlimeState.Chase || _currentState == SlimeState.Attack)
        {
            ChangeState(SlimeState.ReturnToPatrol);
        }
    }


    public Transform GetTarget()
    {
        return _player;
    }

    #endregion


    #region Chase

    void UpdateChase()
    {
        if (_player == null)
        {
            ChangeState(SlimeState.ReturnToPatrol);
            return;
        }

        float distanceX = Mathf.Abs(_player.position.x - transform.position.x);

        if (distanceX <= _attackRange)
        {
            ChangeState(SlimeState.Attack);
            return;
        }

        Vector2 currentPos = transform.position;
        Vector2 targetPos = new Vector2(_player.position.x, currentPos.y);

        transform.position = Vector2.MoveTowards(
            currentPos,
            targetPos,
            _chaseSpeed * Time.deltaTime
        );

        UpdateFacingDirection(_player.position.x - currentPos.x);
    }

    #endregion


    #region Attack

    void StartAttack()
    {
        _isHit = false;

        _anim.ResetTrigger("attack");
        _anim.SetTrigger("attack");
    }

    void UpdateAttack()
    {
        if (_player == null)
        {
            ChangeState(SlimeState.ReturnToPatrol);
            return;
        }

        float distanceX = Mathf.Abs(_player.position.x - transform.position.x);

        if (distanceX > _attackRange)
        {
            ChangeState(SlimeState.Chase);
        }
    }


    public void SetIsHit(bool isHit)
    {
        _isHit = isHit;
    }


    public void OnAttackAnimationFinished()
    {
        if (_player == null)
        {
            ChangeState(SlimeState.ReturnToPatrol);
            return;
        }

        float distanceX = Mathf.Abs(_player.position.x - transform.position.x);

        if (distanceX > _attackRange)
        {
            ChangeState(SlimeState.Chase);
            return;
        }

        if (_isHit)
        {
            StartAttack();
            return;
        }

        ChangeState(SlimeState.Chase);
    }


    void CancelAttack()
    {
        _anim.ResetTrigger("attack");
    }

    #endregion


    #region Damage

    public void TakeDamage(float dmgValue, GameObject dmgSource)
    {
        _health -= dmgValue;

        if (_health <= 0f)
        {
            Die();
        }
        else
        {
            _anim.SetTrigger("hurt");
        }
    }

    #endregion

    #region Death
    void Die()
    {
        StopStateCoroutine();

        SpawnOnDeath();

        Destroy(gameObject);
    }

    void SpawnOnDeath()
    {
        if (_deathSpawnPrefab == null)
            return;

        Vector3[] spawnOffsets = new Vector3[]
        {
            new(-1.5f, 2, 0),
            new(0, 2, 0),
            new(1.5f, 2, 0)
        };
        for (int i = 0; i < _deathSpawnCount; i++)
        {
            Vector3 spawnPos = transform.position + spawnOffsets[i % spawnOffsets.Length];

            Instantiate(_deathSpawnPrefab, spawnPos, Quaternion.identity);

        }
    }

    #endregion


    #region Utility

    bool HasReachedPosition(Vector2 current, Vector2 target)
    {
        const float threshold = 0.05f;

        return (current - target).sqrMagnitude <= threshold * threshold;
    }


    void StopStateCoroutine()
    {
        if (_stateCoroutine == null)
            return;

        StopCoroutine(_stateCoroutine);
        _stateCoroutine = null;
    }


    void UpdateFacingDirection(float direction)
    {
        if (Mathf.Abs(direction) < 0.01f)
            return;

        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);

        transform.localScale = scale;
    }

    #endregion


    #region Gizmos

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_patrolStartPos, _patrolRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 0), _attackRange);
    }

    #endregion
}