using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BansheeGz.BGDatabase;
using UnityEngine.UI;
using TMPro;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager Instance { get; private set; }
    [SerializeField]
    public List<AnimaEntry> animaDatabase = new();  // BGDatabase에서 로드된 데이터
    public List<RecipeEntry> recipeDatabase = new();
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] Transform [] badgePanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        Init();
        MedalInit();
        AudioManager.Instance.PlayBGM(bgmClip);
    }

    void Init()
    {
        animaDatabase = AnimaEntry.LoadAll();
        recipeDatabase = RecipeEntry.LoadAll();
    }
    void MedalInit()
    {
        var abilityTable = BGRepo.I.GetMeta("Ability");
        abilityTable.ForEachEntity(entity =>
        {
            if (entity.Get<int>("IsGotten") == 1)
            {
                GameObject badge;
                switch (entity.Get<string>("Rank"))
                {
                    case "Bronze":
                        badge = Instantiate(Resources.Load<GameObject>("Minwoo/Ability/BronzeMedal"), badgePanel[0]);
                        badge.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Minwoo/Ability/{entity.Get<string>("Objectfile")}");
                        badge.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text = entity.Get<string>("Description");
                        break;
                    case "Silver":
                        badge = Instantiate(Resources.Load<GameObject>("Minwoo/Ability/SilverMedal"), badgePanel[1]);
                        badge.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Minwoo/Ability/{entity.Get<string>("Objectfile")}");
                        badge.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text = entity.Get<string>("Description");
                        break;
                    case "Gold":
                        badge = Instantiate(Resources.Load<GameObject>("Minwoo/Ability/GoldMedal"), badgePanel[2]);
                        badge.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Minwoo/Ability/{entity.Get<string>("Objectfile")}");
                        badge.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text = entity.Get<string>("Description");
                        break;
                    case "Diamond":
                        badge = Instantiate(Resources.Load<GameObject>("Minwoo/Ability/DiamondMedal"), badgePanel[3]);
                        badge.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Minwoo/Ability/{entity.Get<string>("Objectfile")}");
                        badge.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text = entity.Get<string>("Description");
                        break;
                }
            }
        });
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
    public List<RecipeEntry> GetAllRecipe()
    {
        return recipeDatabase;
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
