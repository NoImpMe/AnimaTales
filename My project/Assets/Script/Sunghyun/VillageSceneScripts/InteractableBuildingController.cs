using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InteractableBuilding : MonoBehaviour
{
    [Header("건물 설정")]
    [SerializeField] private string buildingName;
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private Vector3 nameDisplayOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private FadeEffect fadePanel;
    private VillageController _villageController;
    
    public enum BuildingType
    {
        Inn,
        Shop,
        Corridor,
        MagicTree
    }
    
    private void Awake()
    {
        _villageController = FindObjectOfType<VillageController>();
    }
    
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        _villageController.PlayClickSound();
        
        switch (buildingType)
        {
            case BuildingType.Inn:
                _villageController.OpenInn();
                break;
                
            case BuildingType.Shop:
                _villageController.OpenShop();
                break;
                
            case BuildingType.Corridor:
                LoadCorridorScene();
                break;
                
            case BuildingType.MagicTree:
                LoadMixScene();
                break;
        }
    }
    
    private void LoadCorridorScene()
    {
        StartCoroutine(fadePanel.LoadSceneWithFade("CorridorScene"));
    }

    private void LoadMixScene()
    {
        StartCoroutine(fadePanel.LoadSceneWithFade("MixScene"));
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
            
        Vector3 displayPosition = transform.position + nameDisplayOffset;
        _villageController.ShowBuildingName(buildingName, displayPosition);
    }

    private void OnMouseExit()
    {
        if (_villageController != null)
            _villageController.HideBuildingName();
    }
}