using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultipleAttack : MonoBehaviour
{
    IBattleManager bm;
    List<string> expiredBuffList;

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

    private void PrepareAttack()
    {
        bm.IsZKeyPressed = false;
        bm.AttackButton.interactable = true;
        bm.SkillButton.interactable = true;
        bm.AnimaActionUI.SetActive(false);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
    }
    private void DamageParserUpdate()
    {
        foreach (var max in bm.AllyDamageBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var max in bm.EnemyDamageBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var foo in bm.AllyDamageBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
        foreach (var foo in bm.EnemyDamageBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
    }
    private void HealParserUpdate()
    {
        foreach (var max in bm.AllyHealBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var max in bm.EnemyHealBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var foo in bm.AllyHealBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
        foreach (var foo in bm.EnemyHealBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
    }
    private void DefeatEnemy(EnemyActions enemy, int selectEnemy)
    {
        for (int i = 0; i < bm.TmpturnList.Count; i++)
        {
            if (ReferenceEquals(bm.TmpturnList[i], enemy.animaData))
            {
                DestroyImmediate(bm.Turn[i]);
                bm.TmpturnList.RemoveAt(i);
                bm.Turn.RemoveAt(i);
                bm.IsTurn.RemoveAt(i);
                if (UnityEngine.Random.Range(0, 101) <= enemy.animaData.DropRate)
                {
                    AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                    animadata.GetAnima(enemy.animaData.Name, enemy.animaData.level);
                    bm.AllyBattleSetting.PlayerInfo.GetAnima(animadata);
                    bm.DropAnima.Add(animadata);
                    bm.AnimaTable.ForEachEntity(entity =>
                    {
                        if (entity.Get<string>("name") == enemy.animaData.Name)
                        {
                            entity.Set<int>("Meeted", 2);
                            DBUpdater.Save();

                        }
                    });
                }
                foreach (var tmp in bm.AllyActions)
                {
                    if (!tmp.animaData.Animadie)
                    {
                        tmp.animaData.LevelUp();
                        bm.AllyHealthBar[bm.AllyActions.IndexOf(tmp)].UpdateHealthBar();
                        GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                    }
                }
            }
        }
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name}is dead", false);
        GoldManager.Instance.AddGold(enemy.animaData.DropGold);
        bm.TurnList.Remove(enemy.animaData);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyHpInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyHpInstance.RemoveAt(selectEnemy);
        bm.EnemyHealthBar.RemoveAt(selectEnemy);
        bm.EnemyActions.RemoveAt(selectEnemy);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInstance[selectEnemy]);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInfoInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyInstance.RemoveAt(selectEnemy);
        bm.EnemyAnimaNum--;
        for (int i = 0; i < 3; i++)
        {
            var rebuild = GameObject.Find($"Enemy{i}");
            if (rebuild != null)
            {
                rebuild.transform.Find("Status").GetComponent<StatusSync>().dieanima++;
            }
        }

        if (bm.EnemyActions.Count == 0)
        {
            foreach (var ally in bm.AllyActions)
            {
                ally.animaData.location = -1;
            }
            bm.stat = BattleState.win;
            bm.TurnIndex = 0;
            if (bm.RunningCoroutine != null)
            {
                StopCoroutine(bm.RunningCoroutine);
            }
            bm.WinBattle();
        }
    }
    private void DefeatAlly(AnimaActions ally, int selectAlly)
    {
        for (int i = 0; i < bm.TmpturnList.Count; i++)
        {
            if (ReferenceEquals(bm.TmpturnList[i], ally.animaData))
            {
                DestroyImmediate(bm.Turn[i]);
                bm.TmpturnList.RemoveAt(i);
                bm.Turn.RemoveAt(i);
                bm.IsTurn.RemoveAt(i);
            }
        }
        bm.BattleLogManager.AddLog($"{ally.animaData.Name}is dead", true);
        bm.PlayerInfo.DieAnima(ally.animaData);
        bm.DieAllyAnima.Add(bm.AllyActions.IndexOf(ally));
        bm.TurnList.Remove(ally.animaData);
        bm.AllyBattleSetting.AllyHpInstance[ally.animaData.location].SetActive(false);
        bm.AllyBattleSetting.AllyInstance[ally.animaData.location].SetActive(false);
        bm.AllyBattleSetting.AllyInfoInstance[selectAlly].SetActive(false);
        bm.AllyAnimaNum--;


        if (bm.AllyAnimaNum == 0)
        {
            bm.stat = BattleState.defeat;
            if (bm.RunningCoroutine != null)
            {
                StopCoroutine(bm.RunningCoroutine);
            }
            bm.LoseBattle();

        }
    }
    private void BuffUpdate(AnimaDataSO anima)
    {
        expiredBuffList = bm.BuffManager.TickOne(anima);
        while (expiredBuffList.Count < 0)
        {
            switch (expiredBuffList[0])
            {
                case "strength":
                    anima.Damage = anima.tmpAbility["strength"];
                    break;
                case "speed":
                    anima.Speed = anima.tmpAbility["speed"];
                    break;
                case "defense":
                    anima.Defense = anima.tmpAbility["defense"];
                    break;
            }
            expiredBuffList.RemoveAt(0);
        }
    }
}
