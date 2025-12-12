using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using static BansheeGz.BGDatabase.BGJsonRepoModel;

public class VillageTutorialController : MonoBehaviour
{
    public InteractableBuilding shop;
    public GameObject shopBlock;
    public InteractableBuilding inn;
    public GameObject innBlock;
    public InteractableBuilding mix;
    public GameObject mixBlock;
    private bool shopButtonClicked = false;
    private bool innButtonClicked = false;
    private bool mixButtonClicked = false;
    public GameObject upFinger;
    int lastIdx = -1;
    GameObject finger;
    GameObject obj;
    void Start()
    {
        shop.onBuildingClicked += () => shopButtonClicked = true;
        inn.onBuildingClicked += () => innButtonClicked = true;
        mix.onBuildingClicked += () => mixButtonClicked = true;
        List<string> texts = new List<string>()
        {
           " 마을에 온 걸 환영하지. 오래된 곳이지만, 쓸 만한 구석은 많다네.",

"이 마을에서는 할 수 있는 일이 제법이지. 자네도 곧 익숙해질 걸세.",

"우선 저 여관부터 눌러보게. 쉬어갈 필요가 있을 테니.",

"여관에서는 전투로 지친 아니마들을 회복시킬 수 있다네. 다만… 쓸수록 비용이 늘어나는 법이지.",

"다음은 상점이다. 내 자리가 바로 거기지. 필요한 물건이 있다면 그때그때 들러보게나.",

"상점에선 모험에 도움이 되는 물건들을 팔고 있지. 값은 정직하게 매긴다네.",

"마을 한켠엔 ‘추억의 회랑’도 있다네. 시간 날 때 한 번쯤 들여다보게.",

"마지막으로 교감의 나무로 가보지. 그곳도 자네에게 곧 필요해질 걸세."

        };

        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () =>
            {
                int idx = DialogueSystem.Instance.index;
                if (idx != lastIdx)
                {
                    lastIdx = idx;
                    if (finger != null) { Destroy(finger); finger = null; }
                }
                switch (idx)
                {
                    case 2:
                        innBlock.SetActive(false);
                        if (finger == null)
                        {
                            obj = GameObject.Find("Village Canvas");
                            finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440f, -400f);
                        }
                        return innButtonClicked;
                    case 3:
                        innBlock.SetActive(true);
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    case 4:
                        if(GameObject.Find("Inn Panel") != null) GameObject.Find("Inn Panel").SetActive(false);
                        if (finger == null)
                        {
                            obj = GameObject.Find("Village Canvas");
                            finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(420f, -420f);
                        }
                        shopBlock.SetActive(false);
                        return shopButtonClicked;
                    case 5:
                        shopBlock.SetActive(true);
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    case 7:
                        if(GameObject.Find("Shop Panel") != null) GameObject.Find("Shop Panel").SetActive(false);
                        if (finger == null)
                        {
                            obj = GameObject.Find("Village Canvas");
                            finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -120f);
                        }
                        mixBlock.SetActive(false);
                        return mixButtonClicked;
                    default:
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                }
            },
            onFinished: () =>
            {

            }

        );
    }

}
