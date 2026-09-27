using System;
using UnityEngine;

public enum TimePhase
{
    Morning,
    Afternoon,
    Evening
}

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Day Settings")]
    [SerializeField] int startingDay = 1;
    [SerializeField] float dayDuration = 180f;

    int _currentDay;
    float _currentTime;
    TimePhase _currentPhase;

    public int CurrentDay => _currentDay;
    public float CurrentTime => _currentTime;
    public TimePhase CurrentPhase => _currentPhase;
    public float DayDuration => dayDuration;

    public event Action<int> OnDayStarted;
    public event Action<int> OnDayEnded;
    public event Action<TimePhase> OnPhaseChanged;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _currentDay = startingDay;
        _currentTime = 0f;
        _currentPhase = GetPhase();
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        UpdatePhase();

        if (_currentTime >= dayDuration)
        {
            StartNextDay();
        }
    }

    void UpdatePhase()
    {
        TimePhase newPhase = GetPhase();

        if (newPhase == _currentPhase)
            return;

        _currentPhase = newPhase;
        OnPhaseChanged?.Invoke(_currentPhase);
    }

    TimePhase GetPhase()
    {
        float progress = _currentTime / dayDuration;

        if (progress < 0.33f)
            return TimePhase.Morning;

        if (progress < 0.66f)
            return TimePhase.Afternoon;

        return TimePhase.Evening;
    }

    void StartNextDay()
    {
        OnDayEnded?.Invoke(_currentDay);

        _currentDay++;
        _currentTime = 0f;

        _currentPhase = TimePhase.Morning;
        OnPhaseChanged?.Invoke(_currentPhase);

        OnDayStarted?.Invoke(_currentDay);
    }

    public void GoBackOneDay()
    {
        _currentDay = Mathf.Max(1, _currentDay - 1);
        _currentTime = 0f;

        _currentPhase = TimePhase.Morning;

        OnPhaseChanged?.Invoke(_currentPhase);
        OnDayStarted?.Invoke(_currentDay);
    }

    public void ResetDay()
    {
        _currentTime = 0f;
        _currentPhase = TimePhase.Morning;

        OnPhaseChanged?.Invoke(_currentPhase);
        OnDayStarted?.Invoke(_currentDay);
    }
}
