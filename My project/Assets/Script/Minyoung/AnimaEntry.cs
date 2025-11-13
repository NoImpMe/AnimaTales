using BansheeGz.BGDatabase;
using System.Collections.Generic;
using UnityEngine;

public class AnimaEntry
{
    public string name;           
    public string type;           
    public string description;     
    public string imageName;       
    public float hp;               
    public float ap;               
    public float sp;               
    public float dp;
    public int meeted;

    public string objectFile;      
    public EmotionType emotion;
    
    public Sprite GetImage()
    {
        return Resources.Load<Sprite>($"Anima_Sprites/{objectFile}");
    }

    public static List<AnimaEntry> LoadAll()
    {
        List<AnimaEntry> list = new();

        var animaTable = BGRepo.I.GetMeta("Anima");

        animaTable.ForEachEntity(entity =>
        {
            var entry = new AnimaEntry
            {
                name = entity.Get<string>("name"),
                type = entity.Get<string>("Type"),
                description = entity.Get<string>("Description"),
                hp = entity.Get<float>("HP"),
                ap = entity.Get<float>("AP"),
                sp = entity.Get<float>("SP"),
                dp = entity.Get<float>("DP"),
                objectFile = entity.Get<string>("Objectfile"),
                meeted = entity.Get<int>("Meeted"),
                emotion = ParseEmotion(entity.Get<string>("Type"))
            };

            list.Add(entry);
        });

        return list;
    }
    private static EmotionType ParseEmotion(string type)
    {
        if (System.Enum.TryParse(type, out EmotionType result))
            return result;

        return EmotionType.Inanis;
    }
}