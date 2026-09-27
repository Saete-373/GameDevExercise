using UnityEngine;

[CreateAssetMenu(
    fileName = "ResetDayAction",
    menuName = "Game/Time Area/Reset Day"
)]
public class ResetDayAction : TimeAreaAction
{
    public override void Execute()
    {
        GameTimeManager.Instance.ResetDay();
    }
}