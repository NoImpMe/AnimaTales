using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StageTutorialController : MonoBehaviour
{
    public Button invenButton;
    public RegionController villageTile;
    public static bool cameFromBattle = false; 
    public GameObject stageBlock;
    public GameObject invenBlock;
    private bool invenClicked = false;
    private bool villageTileClicked = false;
    public GameObject upFinger;
    public GameObject downFinger;
    GameObject finger;
    GameObject obj;
    int lastIdx = -1;
    void Start()
    {
        gameObject.SetActive(true);
        if (DontDesManager.Instance.tutoCleared)
        {
            Destroy(gameObject);
        }
        
        if (AnimaInventoryManager.Instance.playerInfo.haveAnima.Count > 0)
        {
            cameFromBattle = true;
        }
        gameObject.SetActive(true);
        invenButton = GameObject.Find("Anima Inventory Togle Button").GetComponent<Button>();
        invenButton.onClick.AddListener(() => invenClicked = true);
        
        if (GameObject.Find("VillageTile") != null)
        {
            if (GameObject.Find("VillageTile").GetComponent<RegionController>() != null)
            {
                villageTile = GameObject.Find("VillageTile").GetComponent<RegionController>();
                villageTile.OnTileClicked += () => villageTileClicked = true;
            }
        }
        stageBlock.SetActive(true);
        invenBlock.SetActive(true);
        List<string> texts = new List<string>()
        {
            "여기가 자네 모험의 첫 스테이지라네. 시작은 언제나 소박한 법이지.",

"저곳은 전투 스테이지군 안에는 불안정한 아니마들이 들끓고 있지. 전투란 늘 뜻밖에 찾아오는 법이라네.",

"처음부터 빈손으로 던져 넣을 순 없지. 내 아니마 하나를 잠시 맡겨주겠네. 전투 타일을 눌러 직접 부딪혀보게.",

"흐음… 생각보다 잘 하는군. 인벤토리를 열어보면 자네가 방금 길들인 아니마가 등록되어 있을 걸세.",

"스테이지를 빠져나오면 이렇게 인접한 길들이 모습을 드러난다네. 모험은 늘 한 걸음씩 넓어지지.",

"저곳은 마을 스테이지라네. 쉬어갈 곳도 있고, 필요한 걸 챙기기도 좋지. 자네가 아마 가장 중요하게 생각할 곳이 될 것이네."
        };

        int startIndex = cameFromBattle ? 3 : 0;
        DialogueSystem.Instance.index = startIndex;
        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () => {
                int idx = DialogueSystem.Instance.index;
                if(idx != lastIdx)
                {
                    lastIdx = idx;
                    if (finger != null) { Destroy(finger); finger = null; }
                }
                if(idx == 0)
                {
                    if (finger == null)
                    {
                        obj = GameObject.Find("StartTile");
                        var pos = obj.transform.localPosition;
                        pos.y = 1.5f;
                        finger = Instantiate(downFinger, pos, downFinger.transform.rotation, obj.transform);
                    }
                    
                }
                if(idx == 1)
                {
                    if(finger == null)
                    {
                        obj = GameObject.Find("BattleTile");
                        var pos = obj.transform.localPosition;
                        pos.x += 2f;
                        pos.y = 1.5f;
                        finger = Instantiate(downFinger, pos, downFinger.transform.rotation, obj.transform);
                    }
                    
                }
                if (idx == 2)
                {
                    stageBlock.SetActive(false);
                    invenBlock.SetActive(false);
                    return cameFromBattle;
                }
                if(idx == 3)
                {
                    if(finger == null)
                    {
                        obj = GameObject.Find("Anima Inventory Togle Button");
                        finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform.parent);
                        finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(750f, 330f);
                    }
                    stageBlock.SetActive(false);
                    return invenClicked;
                }
                if(idx == 4)
                {
                if (GameObject.Find("Anima Inventory Panel") != null)
                    {
                        GameObject.Find("Anima Inventory Panel").SetActive(false);
                    }
                }
                if(idx == 5)
                {
                    if(finger == null)
                    {
                        obj = GameObject.Find("VillageTile");
                        var pos = obj.transform.localPosition;
                        pos.x += 4f;
                        pos.y = 1.5f;
                        finger = Instantiate(downFinger, pos, downFinger.transform.rotation, obj.transform);
                    }
                    stageBlock.SetActive(false);
                    invenBlock.SetActive(false);
                    if (GameObject.Find("Anima Inventory Panel") != null) GameObject.Find("Anima Inventory Panel").SetActive(false);
                    return villageTileClicked;
                }
                return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
            },
            onFinished: () =>
            {
                if(finger != null)
                {
                    Destroy(finger); finger = null;
                } 
            }
        );
    }

}
