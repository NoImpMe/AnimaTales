using UnityEngine;

public class UIBlocker : MonoBehaviour
{
    public CanvasGroup blockerPanel;

    public void BlockAllAndAllow(string[] allowUIIds)
    {
        if (blockerPanel != null)
            blockerPanel.blocksRaycasts = true;

    }
    public void ClearBlock()
    {
        if (blockerPanel != null)
            blockerPanel.blocksRaycasts = false;
    }
}
