using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimaSlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    
    [SerializeField] private Material grayscaleMaterial;

    [SerializeField]private AnimaDataSO animaData;
    private InventorySlotType slotType;
    private Action<AnimaDataSO> onClickCallback;

    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public AnimaDataSO AnimaData
    {
        get => animaData;
        set => animaData = value;
    }
    public InventorySlotType SlotType => slotType;

    public void SetData(AnimaDataSO data, InventorySlotType type, Action<AnimaDataSO> onClick)
    {
        animaData = data;
        slotType = type;
        onClickCallback = onClick;

        UpdateUI();
    }
    public void SetData(AnimaDataSO data, InventorySlotType type)
    {
        animaData = data;
        slotType = type;
    }

    private void UpdateUI()
    {
        if (portraitImage == null || button == null) return;

        if (animaData != null)
        {
            string imagePath = "Minwoo/Portrait/" + animaData.Objectfile;
            
            if (!spriteCache.TryGetValue(imagePath, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(imagePath);
                if (sprite != null)
                {
                    spriteCache[imagePath] = sprite;
                }
            }
            
            portraitImage.sprite = sprite;
            portraitImage.enabled = sprite != null;
            button.interactable = true;
            
            if (animaData.Animadie || animaData.Stamina <= 0)
            {
                portraitImage.material = grayscaleMaterial;
            }
            else
            {
                portraitImage.material = null;
            }
        }
        else
        {
            portraitImage.sprite = null;
            portraitImage.enabled = false;
            button.interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (animaData != null && onClickCallback != null)
        {
            onClickCallback.Invoke(animaData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (animaData == null || DraggedPortraitUI.Instance == null) return;
        
        if (portraitImage != null && portraitImage.sprite != null)
        {
            DraggedPortraitUI.Instance.BeginDrag(portraitImage.sprite, this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DraggedPortraitUI.Instance != null)
        {
            DraggedPortraitUI.Instance.UpdatePosition(eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DraggedPortraitUI.Instance != null)
        {
            DraggedPortraitUI.Instance.EndDrag();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DraggedPortraitUI.Instance == null) return;
        
        var draggedSlot = DraggedPortraitUI.Instance.OriginSlot;
        if (draggedSlot == null || draggedSlot == this) return;

        if (AnimaInventoryManager.Instance != null)
        {
            AnimaInventoryManager.Instance.SwapSlots(draggedSlot, this);
        }
        
        if (DraggedPortraitUI.Instance != null)
        {
            DraggedPortraitUI.Instance.EndDrag();
        }
    }
    
    public static void ClearSpriteCache()
    {
        spriteCache.Clear();
    }
}