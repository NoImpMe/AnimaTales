using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimaUI : MonoBehaviour
{
    [Header("도감 슬롯 관련")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject slotPrefab;

    [Header("상세 정보 패널")]
    [SerializeField] private AnimaDetailUI detailPanel;

    [Header("탭 버튼 (전체 + 감정 8개)")]
    [SerializeField] private Toggle[] emotionToggles;
    [SerializeField] private AudioClip btnClip;

    private EmotionType? currentFilter = null;

    private void OnEnable()
    {
        SetupTabs();
        Refresh();
    }

    private void SetupTabs()
    {
        for (int i = 0; i < emotionToggles.Length; i++)
        {
            int index = i;
            emotionToggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    currentFilter = (index == 0) ? (EmotionType?)null : (EmotionType)(index - 1);
                    Refresh();

                    for (int j = 0; j < emotionToggles.Length; j++)
                        emotionToggles[j].interactable = j != index;
                }
            });
        }
    }

    private void Refresh()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        List<AnimaEntry> animaList = currentFilter == null
            ? CorridorManager.Instance.GetAllAnima()
            : CorridorManager.Instance.GetByEmotion(currentFilter.Value);

        foreach (var anima in animaList)
        {
            bool discovered = anima.meeted >= 1;

            if (currentFilter != null && !discovered)
                continue;

            GameObject obj = Instantiate(slotPrefab, gridParent);
            AnimaSlot slot = obj.GetComponent<AnimaSlot>();
            slot.Setup(anima, discovered);
            slot.onClick.AddListener(ShowDetail);
        }

        var first = animaList.Find(a => currentFilter == null || a.meeted >= 1);
        if (first != null)
            ShowDetail(first);
    }

    private void ShowDetail(AnimaEntry anima)
    {
        AudioManager.Instance.PlaySFX(btnClip);
        bool discovered = anima.meeted >= 1;
        detailPanel.Display(anima, discovered);
    }

}
