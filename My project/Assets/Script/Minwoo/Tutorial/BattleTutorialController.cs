using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject upFinger;
    public GameObject leftFinger;
    public GameObject rightFinger;
    GameObject finger;
    GameObject finger2;
    GameObject obj;
    int lastIdx = -1;
    public GameObject actionUI;
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
        actionUI.SetActive(false);
        List<string> texts = new List<string>()
        {
            "여기가 전투 스테이지라네. 이곳에서는 늘 정신부터 챙기는 게 먼저지.",

"위쪽에 있는 녀석들이 적 아니마고, 아래쪽이 자네 아니마라네. 전투란 늘 이런 식으로 마주 보게 되어 있지.",

"저건 턴 순서를 보여주는 장치라네. 아니마마다 속도가 달라서, 누가 먼저 움직일지는 이걸 보면 알 수 있지.",

"이 버튼은 전투 기록을 확인하는 곳이네. 돌이켜보는 습관은 오래 싸우는 데 꽤 도움이 되지.",

"저건 데미지와 회복량을 정리해주는 분석기라네. 전투 도중 한 번쯤 들여다보는 것도 나쁘지 않지.",
    
"이건 아니마들의 상태창이라네. 왼쪽은 자네 아니마, 오른쪽은 적 아니마지. 싸움은 상태를 읽는 것에서 시작된다네.",

"그럼 이제 직접 해보게. 지금은 자네 아니마의 턴이라 행동을 고를 수 있지. 위는 공격, 아래는 기술이네.",

"아니마는 저마다 타입이 있고, 타입에 맞는 고유 기술을 하나씩 지니고 있지. 그 기술을 언제 쓰느냐가 실전에서는 더 중요하더군.",

"자, 이제 기술을 골라 적을 한 번 쳐보게.\nZ키로 기술을 선택하고, 공격할 대상을 정하면 된다네."
        };

        DialogueSystem.Instance.StartDialogue(
            texts,
            nextCondition: () =>
            {
                int idx = DialogueSystem.Instance.index;
                if(idx != lastIdx)
                {
                    lastIdx = idx;
                    if(finger != null) { Destroy(finger); finger = null; }
                    if(finger2 != null) { Destroy(finger2); finger = null; }
                }
                switch (idx) 
                {
                    case 2:
                        if (finger == null)
                        {
                            obj = GameObject.Find("Turn UI");
                            finger = Instantiate(leftFinger, Vector3.zero, leftFinger.transform.rotation, obj.transform.parent);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(160f, -22f);//160 -22
                        }   
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    case 3:
                        logBlock.SetActive(false);
                        if(finger == null)
                        {
                            obj = GameObject.Find("Maximize Log Button");
                            finger = Instantiate(leftFinger,Vector3.zero ,leftFinger.transform.rotation, obj.transform.parent);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(-335f ,-495f);
                        }
                        return logged;
                    case 4:
                        parserBlock.SetActive(false);
                        if (finger == null)
                        {
                            obj = GameObject.Find("Parser Button");
                            finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform.parent);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(795f, -80f);
                        }
                        return parsered;
                    case 5:
                        allyInfoBlock.SetActive(false);
                        enemyInfoBlock.SetActive(false);
                        if (finger == null)
                        {
                            obj = GameObject.Find("Ally0");
                            finger = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform.parent);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(-900f, 128f);
                            obj = GameObject.Find("Enemy0");
                            finger2 = Instantiate(upFinger, Vector3.zero, upFinger.transform.rotation, obj.transform.parent);
                            finger2.GetComponent<RectTransform>().anchoredPosition = new Vector2(890f, 128f);
                        }
                        return allyInfo && enemyInfo;
                    case 8:
                        bm.isTuto = false;
                        actionUI.SetActive(true);
                        if (finger == null)
                        {
                            finger = Instantiate(rightFinger, Vector3.zero, rightFinger.transform.rotation, actionUI.transform.parent);
                            finger.GetComponent<RectTransform>().anchoredPosition = new Vector2(550f, -420f);
                        }
                        return skilled;
                    default:
                        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                }
            },
            onFinished: () =>
            {
                if (finger != null) { Destroy(finger); finger = null; }
            }
        );
    }

}

