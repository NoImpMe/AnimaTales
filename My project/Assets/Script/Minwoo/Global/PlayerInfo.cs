using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;


public class PlayerInfo : ScriptableObject
{
    public int maxAnimaNum = 3;
    public List<AnimaDataSO> battleAnima = new List<AnimaDataSO>();
    public List<AnimaDataSO> haveAnima = new List<AnimaDataSO>();
    public AnimaDataSO animaData;
    public bool onBossStage = false;
    int tmp = 0;
    public int maxLevel = 5;
    public List<AbilitySO> abilitys;
    public void Initialize()
    {

        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        battleAnima.Clear();
        haveAnima.Clear();
        abilitys = new List<AbilitySO>();
        onBossStage = false;
        maxLevel = 5;
        int a = Random.Range(0, 6);
        int b;
        do
        {
            b = Random.Range(0,6 );
        } while (a == b);

        animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
        animaData.Initialize(animaTable[a].Get<string>("name"), 5);
        animaData.location = tmp;
        GetAnima(animaData);
        BattleSetting(haveAnima[tmp]);

        animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
        animaData.Initialize(animaTable[b].Get<string>("name"), 5);
        animaData.location = tmp;
        GetAnima(animaData);
        BattleSetting(haveAnima[tmp]);
        for (int i = 0; i < 2; i++)
        {
            animaTable.ForEachEntity(entity =>
            {
                if (entity.Get<string>("name") == battleAnima[i].Name && entity.Get<int>("Meeted") == 0)
                {
                    entity.Set<int>("Meeted", 1);
                    DBUpdater.Save();
                }
            });
        }

    }
    public void BattleSetting(AnimaDataSO animaData)
    {
        if (haveAnima.Contains(animaData) && battleAnima.Count < maxAnimaNum)
        {
            battleAnima.Add(animaData);
            haveAnima.Remove(animaData);
        }
    }

    public void GetAnima(AnimaDataSO animaData)
    {
        haveAnima.Add(animaData);
    }
    public void DieAnima(AnimaDataSO animaData)
    {
        if (battleAnima.Contains(animaData))
        {
            haveAnima.Add(animaData);
            battleAnima.Remove(animaData);
        }
    }
    public void MaxLevelChanged()
    {
        if (haveAnima.Count > 0)
        {
            foreach (var anima in haveAnima)
            {
                if (anima.level >= maxLevel)
                {
                    maxLevel = anima.level;
                }
            }
        }
        if (battleAnima.Count > 0)
        {
            foreach (var anima in battleAnima)
            {
                if (anima.level >= maxLevel)
                {
                    maxLevel = anima.level;
                }
            }
        }
    }
    public void AddAbility(AbilitySO ability)
    {
        var database = BGRepo.I;
        var abilityTable = database.GetMeta("Ability");
        abilitys.Add(ability);
        abilityTable.ForEachEntity(entity =>
        {
            if (entity.Get<string>("name") == ability.data.name && entity.Get<int>("IsGotten") == 0)
            {
                entity.Set<int>("IsGotten", 1);
                DBUpdater.Save();
            }
        });
    }

}
