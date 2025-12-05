using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    void Start()
    {
        gameObject.SetActive(true);
        if (DontDesManager.Instance.tutoCleared)
        {
            Destroy(gameObject);
        }
        if(AnimaInventoryManager.Instance.playerInfo.haveAnima.Count > 0)
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
            "이 곳은 시작 스테이지라네",
            "이 곳은 전투스테이지로 입장하면 사나운 불안정한 아니마들과 만날 수 있다네",
            "내 애완동물을 빌려줄테니 한 번 들어가서 싸워보게 타일을 클릭하면 된다네",
            "자네 녀석을 잘도 길들였구만 인벤토리를 확인해보면 자네가 방금 길들인 아니마가 추가되었다네",
            "스테이지를 무사히 빠져나오면 인접한 스테이지가 보이기 시작한다네",
            "이 곳은 마을스테이지라네 구경해보겠나?"
        };

        int startIndex = cameFromBattle ? 3 : 0;
        DialogueSystem.Instance.index = startIndex;
        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () => {
                int idx = DialogueSystem.Instance.index;

                if (idx == 2)
                {
                    stageBlock.SetActive(false);
                    invenBlock.SetActive(false);
                    return cameFromBattle;
                }
                if(idx == 3)
                {
                    stageBlock.SetActive(false);
                    return invenClicked;
                }
                if(idx == 5)
                {
                    stageBlock.SetActive(false);
                    invenBlock.SetActive(false);
                    if (GameObject.Find("Anima Inventory Panel") != null) GameObject.Find("Anima Inventory Panel").SetActive(false);
                    return villageTileClicked;
                }
                return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
            },
            onFinished: () =>
            {
                if (villageTileClicked)
                {
                    gameObject.SetActive(false);
                }
            }
        );
    }

}
