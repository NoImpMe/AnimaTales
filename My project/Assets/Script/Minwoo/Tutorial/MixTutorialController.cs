using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Analytics;

public class MixTutorialController : MonoBehaviour
{
    public Button enterButton;
    public Button mixButton;
    public Button exitButton;
    public GameObject exitBlock;
    public GameObject enterBlock;
    private bool mixButtonClicked = false;
    private bool enterButtonClicked = false;
    private bool exitButtonClicked = false;
    void Start()
    {
        mixButton.onClick.AddListener(() => mixButtonClicked = true);
        enterButton.onClick.AddListener(() => enterButtonClicked = true);
        exitButton.onClick.AddListener(() => exitButtonClicked = true);
        List<string> texts = new List<string>()
        {
            "여기가 교감의 나무 속이라네 여기선 아니마 두 마리를 교감시켜 강한 아니마를 얻을 수 있지",
            "나무 속의 보석을 클릭해보게",
            "교감은 주 아니마와 보조 아니마가 존재해야 진행가능하고 교감에 실패하면 보조 아니마는 사라진다네",
            "교감을 시도하기 전 주 아니마의 기술 중 하나를 택하게 되는데 교감에 성공하여 나온 아니마는 주 아니마의 기술을 계승받은 상태가 된다네",
            "또한 교감을 할 때는 아니마가 전투에 들어갈 준비가 되어있으면 안된다네",
            "그럼 나의 애완동물을 인벤토리로 이동시킨 후 조합을 해보겠나",
            "운 좋게 교감에 성공해서 나를 얻었구만 자네!",
            "자 이제 모든 기본 설명이 끝났으니 모험을 떠나 불안정한 아니마들을 막아주게"
        };

        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () =>
            {
                int idx = DialogueSystem.Instance.index;
                switch (idx)
                {
                    case 1:
                        enterBlock.SetActive(false);
                        return enterButtonClicked;
                    case 5:
                        return mixButtonClicked;
                    case 7:
                        exitBlock.SetActive(false);
                        return exitButtonClicked;
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
