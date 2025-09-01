using System.Collections;
using UnityEngine;

public class MultipleAttack : MonoBehaviour
{
    IBattleManager bm;
    
    public void initialize(IBattleManager bm)
    {
        this.bm = bm;
    }
    
    public IEnumerator MultiAllySkill(AnimaActions anima, int skillnum)
    {
        yield return null; 
    }
    public IEnumerator MultiEnemySkill(EnemyActions enemy) 
    {
        yield return null;
    }
    public IEnumerator MultiAllyHeal(AnimaActions anima, int skillnum)
    {
        yield return null;
    }
    public IEnumerator MultiEnemyHeal(EnemyActions enemy)
    {
        yield return null;
    }
    public IEnumerator MultiAllyBuff(AnimaActions anima, int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiEnemyBuff(EnemyActions enemy) 
    {
        yield return null;
    }
    public IEnumerator MultiAllyDebuff(AnimaActions anima, int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiEnemyDebuff(EnemyActions enemy)
    {
        yield return null;
    }
}
