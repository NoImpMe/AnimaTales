using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Outline Images (4개)")]
    public Image top;
    public Image bottom;
    public Image left;
    public Image right;

    [Header("테두리 색상")]
    public Color highlightColor = new Color(1f, 0.95f, 0.75f, 1f);   // 밝은색
    public Color dimColor = new Color(1f, 0.95f, 0.75f, 0.25f);       // 어두운색(투명도)

    [Header("애니메이션")]
    public float pulseSpeed = 3f;

    private bool isActive = false;

    private void Update()
    {
        if (!isActive) return;

        // 색이 밝아졌다 어두워졌다 반복
        float t = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;
        Color animatedColor = Color.Lerp(dimColor, highlightColor, t);

        SetBorderColor(animatedColor);
    }


    // 🔹 키보드/패드 네비게이션으로 선택됐을 때
    public void OnSelect(BaseEventData eventData)
    {
        ActivateBorder();
    }

    // 🔹 선택 해제됐을 때
    public void OnDeselect(BaseEventData eventData)
    {
        DeactivateBorder();
    }
    public void OnClick(BaseEventData eventData)
    {
        DeactivateBorder();
    }
    private void ActivateBorder()
    {
        isActive = true;

        SetBorderColor(highlightColor);
    }

    public void DeactivateBorder()
    {
        isActive = false;

        SetBorderColor(Color.clear);
    }

    private void SetBorderColor(Color c)
    {
        top.color = c;
        bottom.color = c;
        left.color = c;
        right.color = c;
    }
}
