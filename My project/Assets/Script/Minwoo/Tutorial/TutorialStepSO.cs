using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/Step", order = 0)]
public class TutorialStepSO : ScriptableObject
{
    [Header("NPC 대사 텍스트")]
    [TextArea(2, 10)]
    public string[] dialogues;

    [Header("강조할 오브젝트 이름 / UI 이름")]
    public string[] highlightTarget;

    [Header("조건 파라미터 (UI 이름, 몬스터 ID 등)")]
    public string conditionParam;

    [Header("이 단계에서 비활성화할 UI들")]
    public string[] disableUI;

    [Header("이 단계에서만 활성화할 UI들")]
    public string[] allowUIIds; // 허용할 UI id 목록


    public float conditionFloat = 0f; // 예: 이동 거리 threshold 또는 시간

}
