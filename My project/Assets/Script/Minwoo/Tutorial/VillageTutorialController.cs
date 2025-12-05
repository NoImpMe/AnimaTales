using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Analytics;

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

    void Start()
    {
        shop.onBuildingClicked += () => shopButtonClicked = true;
        inn.onBuildingClicked += () => innButtonClicked = true;
        mix.onBuildingClicked += () => mixButtonClicked = true;
        List<string> texts = new List<string>()
        {
            "마을에 온 걸 환영한다네!",
            "마을에서는 많은 것들을 할 수 있다네!",
            "우선 여관을 클릭해보게",
            "이곳에서는 전투로 지친 아니마들을 모두 회복시킬 수 있다네 하지만 횟수에 따라 비용이 늘어나지",
            "다음은 상점을 클릭해보게",
            "상점에서는 여러 도움이 되는 물품을 구매할 수 있다네",
            "마을에서는 추억의 회랑도 둘러볼 수 있다네",
            "마지막으로 교감의 나무로 가서 더 둘러보자고"
        };

        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () =>
            {
                int idx = DialogueSystem.Instance.index;
                switch (idx)
                {
                    case 2:
                        innBlock.SetActive(false);
                        return innButtonClicked;
                    case 3:
                        innBlock.SetActive(true);
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    case 4:
                        if(GameObject.Find("Inn Panel") != null) GameObject.Find("Inn Panel").SetActive(false);
                        shopBlock.SetActive(false);
                        return shopButtonClicked;
                    case 5:
                        shopBlock.SetActive(true);
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    case 7:
                        if(GameObject.Find("Shop Panel") != null) GameObject.Find("Shop Panel").SetActive(false);
                        mixBlock.SetActive(false);
                        return mixButtonClicked;
                    default:
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                }
            },
            onFinished: () =>
            {
                if (mixButtonClicked)
                {
                    gameObject.SetActive(false);
                }
            }

        );
    }

}
