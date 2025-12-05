using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTutorialController : MonoBehaviour
{
    
    public Button skillButton;
    private bool skilled = false;
    public Button logButton;
    private bool logged = false;
    public GameObject logBlock;
    public Button parserButton;
    private bool parsered = false;
    public GameObject parserBlock;
    public Button allyInfoButton;
    public Button enemyInfoButton;
    private bool allyInfo = false;
    private bool enemyInfo = false;
    public GameObject allyInfoBlock;
    public GameObject enemyInfoBlock;
    public void TutoInit()
    {
        allyInfoButton = GameObject.Find("Ally0").transform.Find("Button").GetComponent<Button>();
        enemyInfoButton = GameObject.Find("Enemy0").transform.Find("Button").GetComponent<Button>();
        skillButton.onClick.AddListener(() => skilled = true);
        logButton.onClick.AddListener(() => logged = true);
        parserButton.onClick.AddListener(() => parsered = true);
        allyInfoButton.onClick.AddListener(() => allyInfo = true);
        enemyInfoButton.onClick.AddListener(() => enemyInfo = true);
        var bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        List<string> texts = new List<string>()
        {
            "여기가 전투스테이지 안이라네",
            "위쪽에 있는 아니마가 적 아니마고 너와 같이 싸워줄 아니마가 아래쪽에 위치한다네",
            "이것은 턴 순서를 알려주는 것이고 아니마들의 속도에 따라 턴이 결정된다네",
            "이것은 전투 기록을 보여주는 버튼이라네",
            "이것은 이번 전투에서의 데미지와 힐량의 분석기라네",
            "이 것은 각 아니마의 상태를 볼 수 있는 상태창으로 좌측이 자네 아니마들이고 우측이 적 아니마라네",
            "그럼 전투를 진행해볼까 자네 아니마의 턴이라 행동을 결정할 수 있다네 위는 공격 아래는 기술이라네",
            "아니마들은 각 타입이 존재하고 그에 맞는 고유 기술을 하나씩 가지고 있다네",
            "그럼 기술을 사용해서 적 아니마를 공격해보도록 하게 \n(Z키를 입력하여 기술을 골라 상대를 선택하세요)"
        };

        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () =>
            {
                int idx = DialogueSystem.Instance.index;

                switch (idx) 
                {
                    case 3:
                        logBlock.SetActive(false);
                        return logged;
                    case 4:
                        parserBlock.SetActive(false);
                        return parsered;
                    case 5:
                        allyInfoBlock.SetActive(false);
                        enemyInfoBlock.SetActive(false);
                        return allyInfo && enemyInfo;
                    case 8:
                        bm.isTuto = false;
                        return skilled;
                    default:
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                }
            },
            onFinished: () =>
            {
                if (skilled)
                {
                    gameObject.SetActive(false);
                }
            }
        );
    }

}

