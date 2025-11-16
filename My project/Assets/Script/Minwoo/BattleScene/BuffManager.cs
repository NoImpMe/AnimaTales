using System.Collections.Generic;

public class BuffManager
{
    private Dictionary<Buff, int> buffList = new Dictionary<Buff, int>();
    private List<string> expiredBuff = new List<string>();
    public Dictionary<Buff,int> BuffList
    {
        get => buffList;
        set => buffList = value;
    }
    public void AddOrRenuwBuff(Buff buff)
    {

        if (IsExistAndRenewBuff(buff)) return;

        else buffList.Add(buff, buff.distinct);
        
    }
    public List<string> TickOne(AnimaDataSO target)
    {
        expiredBuff.Clear();
        var deleteList = new List<Buff>();
        foreach (var buff in buffList) 
        {
            if(ReferenceEquals(buff.Key.target, target))
            {
                buff.Key.Tick();
                if (buff.Key.isExpired())
                {
                    deleteList.Add(buff.Key);
                    for (int i = 0; i < buff.Key.type.Count; i++)
                    {
                        expiredBuff.Add(buff.Key.type[i]);
                    }
                }
            }
        }
        foreach (var buff in deleteList)
        {
            buffList.Remove(buff);
        }
        return expiredBuff;
    }
    public bool IsExistAndRenewBuff(Buff buff)
    {
        foreach (var existbuff in buffList)
        {
            if(existbuff.Key.type == buff.type && ReferenceEquals(existbuff.Key.target, buff.target))
            {
                existbuff.Key.Renew(buff);
                return true;
            }
        }
        return false;
    }

    public List<Buff> GetBuffList() 
    { 
        List<Buff> returnBuffList = new List<Buff>();
        foreach(var buff in buffList)
        {
            returnBuffList.Add(buff.Key);
        }
        return returnBuffList;
    }
}
