using System.Collections.Generic;
using UnityEngine;

public class RegionController : MonoBehaviour
{
    
    [Tooltip("클리어 & 마을 상태")]
    public bool isVillaged = false;
    [Tooltip("이 영역과 해제할 타일 영역")]
    public List<RegionController> neighbors;
    public string type;
    public RegionController otherStartPoint;
    public System.Action OnTileClicked;

    void OnMouseDown()
    {
        OnTileClicked?.Invoke();
    }
}
