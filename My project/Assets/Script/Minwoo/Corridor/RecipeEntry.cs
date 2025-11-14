using BansheeGz.BGDatabase;
using System.Collections.Generic;
using UnityEngine;

public class RecipeEntry
{
    public string name;
    public string main;
    public string sub;
    public string result;
    public int sucess;

    public Sprite GetMainImage()
    {
        return Resources.Load<Sprite>($"Anima_Sprites/{main}");
    }
    public Sprite GetSubImage()
    {
        return Resources.Load<Sprite>($"Anima_Sprites/{sub}");
    }
    public Sprite GetResultImage()
    {
        return Resources.Load<Sprite>($"Anima_Sprites/{result}");
    }
    public Sprite GetQuestionImage()
    {
        return Resources.Load<Sprite>($"Minwoo/CorridorImage/Unknown");
    }
    public static List<RecipeEntry> LoadAll()
    {
        List<RecipeEntry> list = new();

        var animaTable = BGRepo.I.GetMeta("Recipe");

        animaTable.ForEachEntity(entity =>
        {
            var entry = new RecipeEntry
            {
                name = entity.Get<string>("name"),
                main = entity.Get<string>("Main"),
                sub = entity.Get<string>("Sub"),
                result = entity.Get<string>("Result"),
                sucess = entity.Get<int>("Sucess")
            };
            list.Add(entry);
        });
        return list;
    }
}
