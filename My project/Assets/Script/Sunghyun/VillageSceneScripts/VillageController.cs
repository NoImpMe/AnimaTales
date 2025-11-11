using UnityEngine;
using System.Collections;

public class VillageController : MonoBehaviour
{
    
    
    [Header("UI 관리")]
    [SerializeField] private GameObject buildingNamePanel;
    [SerializeField] private TMPro.TextMeshProUGUI buildingNameText;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private InnUIManager innUIManager;
    
    [SerializeField] private float fadeSpeed = 5f;
    [Header("오디오")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip interactClip;

    private CanvasGroup _nameCanvasGroup;

    private Camera _mainCamera;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (buildingNamePanel != null)
            buildingNamePanel.SetActive(false);
        
        if (buildingNamePanel != null)
        {
            _nameCanvasGroup = buildingNamePanel.GetComponent<CanvasGroup>();
            if (_nameCanvasGroup == null)
                _nameCanvasGroup = buildingNamePanel.AddComponent<CanvasGroup>();
        }
    }

    
    private void Start()
    {
        AudioManager.Instance.PlayBGM(bgmClip);
    }
    
    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX(interactClip);
    }
    
    public void ShowBuildingName(string buildingName, Vector3 worldPosition)
    {
        if (buildingNamePanel == null || buildingNameText == null)
            return;
            
        buildingNameText.text = buildingName;
        
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
        
        screenPosition.y += 50f;
        buildingNamePanel.transform.position = screenPosition;
        
        buildingNamePanel.SetActive(true);
        
        _nameCanvasGroup.alpha = 0f;
        StopAllCoroutines();
        StartCoroutine(FadePanelIn());
    }

    
    public void HideBuildingName()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanelOut());
    }
    
    private IEnumerator FadePanelIn()
    {
        while (_nameCanvasGroup.alpha < 1f)
        {
            _nameCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _nameCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadePanelOut()
    {
        while (_nameCanvasGroup.alpha > 0f)
        {
            _nameCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _nameCanvasGroup.alpha = 0f;
        buildingNamePanel.SetActive(false);
    }

    public void OpenShop()
    {
        if (ShopUIManager.Instance != null)
        {
            ShopUIManager.Instance.OpenShopPanel();
        }
    }
    
    public void CloseShop()
    {
        if (ShopUIManager.Instance != null)
        {
            ShopUIManager.Instance.CloseShopPanel();
        }
    }
    
    public void OpenInn()
    {
        if (innUIManager != null)
        {
            innUIManager.OpenInnPanel();
        }
    }
}