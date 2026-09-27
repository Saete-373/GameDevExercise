using UnityEngine;
using UnityEngine.UI;

public class DragIconUI : MonoBehaviour
{
    [SerializeField] Image _icon;

    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}