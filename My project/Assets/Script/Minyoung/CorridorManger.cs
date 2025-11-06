using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager Instance { get; private set; }
    [SerializeField]
    public List<AnimaEntry> animaDatabase = new();  // BGDatabase에서 로드된 데이터

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        animaDatabase = AnimaEntry.LoadAll();

        //// 테스트용: 앞 50개를 수집된 것처럼
        //for (int i = 0; i < 50 && i < animaDatabase.Count; i++)
        //{
        //    animaDatabase[i].meeted = 1;  
        //}

        // LoadMeetedData();  나중에 저장된 meeted 불러오기
    }

    public bool IsDiscovered(AnimaEntry entry)
    {
        return entry.meeted >= 1;
    }

    public void MarkDiscovered(AnimaEntry entry)
    {
        if (entry.meeted < 1)
        {
            entry.meeted = 1;
            SaveMeetedData();  // 나중에 저장 구현
        }
    }

    public void MarkCollected(AnimaEntry entry)
    {
        if (entry.meeted < 2)
        {
            entry.meeted = 2;
            SaveMeetedData();  // 나중에 저장 구현
        }
    }

    public List<AnimaEntry> GetAllAnima()
    {
        return animaDatabase;
    }

    public List<AnimaEntry> GetByEmotion(EmotionType emotion)
    {
        return animaDatabase.Where(a => a.emotion == emotion).ToList();
    }

    private void SaveMeetedData()
    {
        // TODO: PlayerPrefs, JSON 등 구현 예정
    }

    private void LoadMeetedData()
    {
        // TODO: BGDatabase로부터 또는 외부 저장소에서 meeted 불러오기
    }
}
