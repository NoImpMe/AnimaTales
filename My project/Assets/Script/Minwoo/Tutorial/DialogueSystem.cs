using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("UI References")]
    public TextMeshProUGUI dialogueText;
    public List<string> textList;
    public int index = 0;
    public float typingSpeed = 0.03f;     

    private System.Action onFinished;     
    private Coroutine typingCoroutine;
    private System.Func<bool> nextCondition;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(List<string> texts, System.Func<bool> nextCondition, System.Action onFinished = null)
    {
        gameObject.SetActive(true);
        this.nextCondition = nextCondition;
        this.onFinished = onFinished;
        this.textList = texts;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(textList[index]));
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            
            yield return new WaitForSeconds(typingSpeed);
            
        }

        yield return new WaitUntil(() => nextCondition());
        gameObject.SetActive(false);
        index++;

        // 다 읽으면 종료
        if (index >= textList.Count)
        {
            FinishDialogue();
        }
        else
        {
            gameObject.SetActive(true);
            typingCoroutine = StartCoroutine(TypeText(textList[index]));
        }
    }

    private void FinishDialogue()
    {
        gameObject.SetActive(false);
        onFinished?.Invoke();   // 튜토리얼 다음 단계로 넘김
    }
}
