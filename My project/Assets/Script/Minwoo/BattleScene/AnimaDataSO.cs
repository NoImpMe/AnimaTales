using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using JetBrains.Annotations;
using UnityEngine;

public class AnimaDataSO : ScriptableObject
{
    public bool Animadie = false;
    public bool isAlly = false;
    public bool turnCheck = false;
    public bool isTomb = false;
    public bool isBoss = false;
    public int level = 1;
    public string Name; 
    public float Maxstamina = 1;
    public float Stamina = 1;
    public float Damage = 1;
    public int DropGold = 1;
    public float Speed = 1;
    public float DropRate = 1;
    public int MaxSkill_pp = 10;
    public int Skill_pp = 10;
    public float defHP;
    public float defAP;
    public float defDP;
    public float defSP;
    public string Objectfile;
    public int location = -1;
    public float Defense = 0;
    public float EXP = 0;
    public float MAX_EXP = 0;
    public float weight;
    public int enemyIndex= -1;
    public int mood = -1;
    public string type = "";
    public List <string> skillName = new List<string>();
    public string attackName = "";
    private int[] maxLevel = new int[10]{ 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
    public Dictionary<string, float> tmpAbility = new Dictionary<string, float>();
    public void Initialize(string name, int level)
    {
        
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {

                mood = int.Parse(Name.Substring(Name.Length - 1));
                this.level = level;
                weight = entity.Get<float>("Weight");
                defHP = entity.Get<float>("HP");
                defAP = entity.Get<float>("AP");
                defDP = entity.Get<float>("DP");
                defSP = entity.Get<float>("SP");
                Maxstamina = Mathf.Ceil(CalcStat(level, weight, defHP));
                Stamina = Maxstamina;
                Damage = Mathf.Ceil(CalcStat(level, weight, defAP));
                Defense = Mathf.Ceil(CalcStat(level, weight, defDP));
                DropGold = entity.Get<int>("DropGold");
                Speed = Mathf.Ceil(CalcStat(level, weight, defSP));
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                attackName = entity.Get<string>("Attack");
                skillName = entity.Get<List<string>>("Skill");
                type = entity.Get<string>("Type");
            }
        });
    }
    public void GetAnima(string name, int level)
    {
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                mood = int.Parse(Name.Substring(Name.Length - 1));
                this.level = level;
                weight = entity.Get<float>("Weight");
                defHP = entity.Get<float>("HP");
                defAP = entity.Get<float>("AP");
                defDP = entity.Get<float>("DP");
                defSP = entity.Get<float>("SP");
                Maxstamina = Mathf.Ceil(CalcStat(level, weight, defHP));
                Stamina = Maxstamina * 0.4f;
                Damage = Mathf.Ceil(CalcStat(level, weight, defAP));
                Defense = Mathf.Ceil(CalcStat(level, weight, defDP));
                DropGold = entity.Get<int>("DropGold");
                Speed = Mathf.Ceil(CalcStat(level, weight, defSP));
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                attackName = entity.Get<string>("Attack");
                skillName = entity.Get<List<string>>("Skill");
                type = entity.Get<string>("Type");
            }
        });
    }
    public float CalcStat(int level, float weight, float stat)
    {
        //math.ceil(((2*j)*(j+0.9))*(k * math.sqrt(math.sqrt(pow(i,3))) + k*math.sqrt(math.sqrt(pow(j, i)))))
        
        return Mathf.Ceil(((2f * weight) * (weight + 0.9f)) * (stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(level, 3f)))) + stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(weight, level))));
    }
    public void LevelUp()
    {
        if (maxLevel[mood] > level)
        {
            level++;
            Maxstamina = Mathf.Ceil(CalcStat(level, weight, defHP));
            Damage = CalcStat(level, weight, defAP);
            Defense = CalcStat(level, weight, defDP);
            Speed = CalcStat(level, weight, defSP);
        }
        
    }
}
