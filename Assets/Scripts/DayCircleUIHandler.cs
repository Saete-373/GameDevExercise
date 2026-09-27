using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayCircleUIHandler : MonoBehaviour
{
    GameTimeManager _timeManager;

    [Header("UI")]
    [SerializeField] TMP_Text _dateText;
    [SerializeField] TMP_Text _dayText;

    [Header("Visual")]
    [SerializeField] Color[] _dayColors;

    Material _dayCircleMaterial;

    string[] _dayInSeven =
    {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday"
    };

    void Awake()
    {
        _dayCircleMaterial = GetComponent<Image>().material;
    }

    void Start()
    {
        _timeManager = GameTimeManager.Instance;
        _timeManager.OnDayStarted += HandleDayStarted;
        _timeManager.OnPhaseChanged += HandlePhaseChanged;

        UpdateDateUI(_timeManager.CurrentDay);
        UpdateDayCircle();
        UpdateDayColor(_timeManager.CurrentPhase);
    }

    void OnDestroy()
    {
        if (_timeManager != null)
        {
            _timeManager.OnDayStarted -= HandleDayStarted;
            _timeManager.OnPhaseChanged -= HandlePhaseChanged;
        }
    }

    void Update()
    {
        UpdateDayCircle();
    }

    void UpdateDayCircle()
    {
        float percentage = Mathf.Clamp01(_timeManager.CurrentTime / _timeManager.DayDuration) * 100f;

        _dayCircleMaterial.SetFloat("_Percentage", percentage);
    }

    void HandlePhaseChanged(TimePhase phase)
    {
        UpdateDayColor(phase);
    }

    void UpdateDayColor(TimePhase phase)
    {
        int colorIndex = (int)phase;

        _dayCircleMaterial.SetColor("_Color", _dayColors[colorIndex]);
    }

    void HandleDayStarted(int day)
    {
        UpdateDateUI(day);

        UpdateDayCircle();
    }

    void UpdateDateUI(int day)
    {
        _dateText.text = "DAY " + day;

        int dayIndex = (day - 1) % _dayInSeven.Length;

        _dayText.text = _dayInSeven[dayIndex];
    }
}