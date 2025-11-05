using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public List<string> type;
    public float weight;
    public int remainingTurns;
    public AnimaDataSO target;
    public int distinct = -1;
    public Buff(List<string> type, float weight, int remainingTurns, AnimaDataSO target, int distinct)
    {
        this.type = type;
        this.weight = weight;
        this.remainingTurns = remainingTurns;
        this.target = target;
        this.distinct = distinct;
    }
    public void Tick() => remainingTurns--;
    
    public void Renew(Buff buff)
    {
        this.remainingTurns = buff.remainingTurns;
    }

    public bool isExpired() => remainingTurns <= 0;

}
