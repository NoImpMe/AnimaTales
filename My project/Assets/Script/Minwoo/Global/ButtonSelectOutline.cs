using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectedOutline : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Outline Image (자식 오브젝트)")]
    public Image outlineImage;

    private void Awake()
    {
        if (outlineImage != null)
            outlineImage.enabled = false;  // 시작은 꺼둠
    }

    // 버튼이 네비게이션/코드로 선택될 때
    public void OnSelect(BaseEventData eventData)
    {
        if (outlineImage != null)
            outlineImage.enabled = true;
    }

    // 선택이 풀릴 때
    public void OnDeselect(BaseEventData eventData)
    {
        if (outlineImage != null)
            outlineImage.enabled = false;
    }
}
