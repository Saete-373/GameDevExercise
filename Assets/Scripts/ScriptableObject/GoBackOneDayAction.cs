using UnityEngine;

[CreateAssetMenu(
    fileName = "GoBackOneDayAction",
    menuName = "Game/Time Area/Go Back One Day"
)]
public class GoBackOneDayAction : TimeAreaAction
{
    public override void Execute()
    {
        GameTimeManager.Instance.GoBackOneDay();
    }
}