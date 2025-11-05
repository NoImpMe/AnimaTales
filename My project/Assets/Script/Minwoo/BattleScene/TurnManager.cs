using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : ScriptableObject
{
    List <AnimaDataSO> turnList;
    List<AnimaDataSO> partTurnList;
   
    public void ResetTurnList()
    {
        turnList = new List <AnimaDataSO>();
    }
    public void InsertAnimaData(AnimaDataSO animaData)
    {
        turnList.Add(animaData);
    }
    public List<AnimaDataSO> UpdateTurnList()
    {
        turnList.Sort((a,b) => b.Speed.CompareTo(a.Speed));
        return turnList;
    }
    public bool CheckChanged()
    {
        List<AnimaDataSO> check = turnList.ToList();
        check.Sort((a, b) => b.Speed.CompareTo(a.Speed));
        if (check.SequenceEqual(turnList))
        {
            return false;
        }
        return true;
    }
    public List<AnimaDataSO> OnLevelUpTurnChanged()
    {
        partTurnList = new List <AnimaDataSO>();
        for(int i = 0; i < turnList.Count; i++)
        {
            if (turnList[i].turnCheck) continue;
            else
            {
                partTurnList.Add(turnList[i]);
            }
        }
        int lastIndex = turnList.Count - partTurnList.Count;
        for(int i = turnList.Count - 1; i >= lastIndex ; i--)
        {
            turnList.RemoveAt(i);
        }
        partTurnList.Sort((a, b) => b.Speed.CompareTo(a.Speed));
        for(int i = 0; i < partTurnList.Count; i++)
        {
            turnList.Add(partTurnList[i]);
        }
        partTurnList = null;
        return turnList;
    }
}
