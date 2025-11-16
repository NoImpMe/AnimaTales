using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleAttack : MonoBehaviour
{
    IBattleManager bm;
    List<string> expiredBuffList;
    List<Transform> allys;
    List<Transform> enemys;
    AbilityManager abilityManager;
    public void initialize(IBattleManager bm)
    {
        this.bm = bm;
        abilityManager = GameObject.Find("Game Manager").GetComponent<AbilityManager>();
    }
    
    public IEnumerator MultiAllySkill(AnimaActions anima, int skillNum, float weight)
    {
        enemys = new List<Transform>();
        for (int i = 0; i < bm.EnemyActions.Count; i++)
        {
            enemys.Add(bm.EnemyBattleSetting.EnemyInstance[i].transform);
        }
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomMultiOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, enemys, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        yield return anima.MultiSkill(anima, bm.EnemyActions, bm.EnemyBattleSetting, bm.EnemyHealthBar, bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)], weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on Enemys for {Mathf.Ceil(anima.maxDamage)}damage", true);
        bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        DamageParserUpdate();
        BuffUpdate(anima.animaData);

        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> enemyList = new List<int>();
        
        int fast = 0;
        float allySpeed = anima.animaData.Speed;
        foreach ( var enemy in bm.EnemyActions)
        {
            if (enemy.animaData.Animadie)
            {
                if (allySpeed <= enemy.animaData.Speed)
                {
                    fast += 1;
                }
                enemyList.Add(bm.EnemyActions.IndexOf(enemy));
            }
        }
        for(int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        
        while(enemyList.Count > 0)
        {
            DefeatEnemy(bm.EnemyActions[enemyList[0]], enemyList[0]);
            enemyList.RemoveAt(0);
            if (enemyList.Count != 0)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    enemyList[j] -= 1;
                }
            }
        }
    }
    public IEnumerator MultiEnemySkill(EnemyActions enemy, float weight) 
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.MultiSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, weight, bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        BuffUpdate(enemy.animaData);

        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0) 
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
    }
    public IEnumerator MultiAllyHeal(AnimaActions anima, int skillNum, float weight)
    {
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomMultiIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, allys, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.MultiHeal(anima, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.AllyHealBar[bm.AllyActions.IndexOf(anima)], weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on Allys for {Mathf.Ceil(anima.maxHeal)}heal", true);
        bm.AllyHealText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyHealBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator MultiAllyShield(AnimaActions anima, int skillNum, float weight)
    {
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomMultiIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, allys, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.MultiShield(anima, bm.AllyActions, bm.AllyBattleSetting, bm.AllyShieldBar, weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name}이 \"{anima.animaData.skillName[skillNum]}\"를 사용해 아군 아니마에게 {Mathf.Ceil(anima.maxHeal)}배리어 줌", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator MultiEnemyHeal(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        enemys = new List<Transform>();
        for (int i = 0; i < bm.EnemyActions.Count; i++)
        {
            enemys.Add(bm.EnemyBattleSetting.EnemyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, enemys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.MultiHeal(enemy, bm.EnemyActions, bm.EnemyBattleSetting, bm.EnemyHealthBar, weight, bm.EnemyHealBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Enemys for {Mathf.Ceil(enemy.heal)} heal", false);
        bm.EnemyHealText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyHealBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator MultiAllyBuff(AnimaActions anima, int skillNum, float weight) 
    {
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomMultiIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, allys, true, anima.animaData.skillName[skillNum    ]);
        bm.Canvas.SetActive(true);
        yield return anima.MultiIncreaseAbility(anima, bm.AllyActions, bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on Allys for {string.Join(", ", bm.MatchedSkill[0].Affect)} up", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator MultiEnemyBuff(EnemyActions enemy, float weight) 
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        enemys = new List<Transform>();
        for (int i = 0; i < bm.EnemyActions.Count; i++)
        {
            enemys.Add(bm.EnemyBattleSetting.EnemyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, enemys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.MultiIncreaseAbility(enemy, bm.EnemyActions, bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Enemys for {string.Join(", ", bm.MatchedSkill[0].Affect)} up", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator MultiAllyDebuff(AnimaActions anima, int skillNum, float weight) 
    {
        enemys = new List<Transform>();
        for (int i = 0; i < bm.EnemyActions.Count; i++)
        {
            enemys.Add(bm.EnemyBattleSetting.EnemyInstance[i].transform);
        }
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomMultiOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, enemys, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.MultiDecreaseAbility(anima, bm.EnemyActions, bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on Enemys for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator MultiEnemyDebuff(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.MultiDecreaseAbility(enemy, bm.AllyActions, bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }

    private void PrepareAttack(AnimaActions anima)
    {
        anima.animaData.turnCheck = true;
        bm.AttackButton.interactable = true;
        bm.SkillButton.interactable = true;
        bm.Skill1.interactable = true;
        bm.Skill2.interactable = true;
        bm.AnimaActionUIController.selectSkill.SetActive(false);
        bm.AnimaActionUIController.selectAction.SetActive(true);
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
        if (bm.TurnList.Contains(enemy.animaData)) bm.TurnList.Remove(enemy.animaData);
        
        for (int i = 0; i < bm.TmpturnList.Count; i++)
        {
            if (ReferenceEquals(bm.TmpturnList[i], enemy.animaData))
            {
                DestroyImmediate(bm.Turn[i]);
                bm.TmpturnList.RemoveAt(i);
                bm.Turn.RemoveAt(i);
                bm.IsTurn.RemoveAt(i);
                if (UnityEngine.Random.Range(0, 101) <= (enemy.animaData.DropRate * (1 + abilityManager.DropSymbol)) && !enemy.animaData.isClone)
                {
                    AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                    animadata.GetAnima(enemy.animaData.Name, enemy.animaData.level);
                    bm.AllyBattleSetting.PlayerInfo.GetAnima(animadata);
                    bm.DropAnima.Add(animadata);
                    bm.AnimaTable.ForEachEntity(entity =>
                    {
                        if (entity.Get<string>("name") == enemy.animaData.Name && entity.Get<int>("Meeted") != 2)
                        {
                            entity.Set<int>("Meeted", 2);
                            DBUpdater.Save();
                        }
                    });
                }
                foreach (var tmp in bm.AllyActions)
                {
                    if (!tmp.animaData.Animadie && !enemy.animaData.isClone)
                    {
                        tmp.animaData.LevelUp();
                        StartCoroutine(bm.AllyHealthBar[bm.AllyActions.IndexOf(tmp)].UpdateHealthBar());
                        GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                    }
                }
                if (bm.TurnManager.CheckChanged())
                {
                    bm.TurnRebuild = true;
                    bm.TurnUISetting(bm.TurnManager.OnLevelUpTurnChanged());
                }
            }
        }
        if (enemy.animaData.isClone)
        {
            DestroyImmediate(bm.EnemyBattleSetting.EnemyParserInstance[selectEnemy]);
            bm.EnemyBattleSetting.EnemyParserInstance.RemoveAt(selectEnemy);
            bm.EnemyDamageBar.RemoveAt(selectEnemy);
            bm.EnemyDamageText.RemoveAt(selectEnemy);
            bm.EnemyHealBar.RemoveAt(selectEnemy);
            bm.EnemyHealText.RemoveAt(selectEnemy);
        }
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name}is dead", false);
        GoldManager.Instance.AddGold((int)(enemy.animaData.DropGold * (1 + abilityManager.GoldSymbol)));
        
        DestroyImmediate(bm.EnemyBattleSetting.EnemyHpInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyHpInstance.RemoveAt(selectEnemy);
        bm.EnemyHealthBar.RemoveAt(selectEnemy);
        bm.EnemyShieldBar.RemoveAt(selectEnemy);
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

        if (bm.EnemyActions.Count == 0 || enemy.animaData.isBoss)
        {
            bm.stat = BattleState.win;
            if (bm.RunningCoroutine != null)
            {
                StopCoroutine(bm.RunningCoroutine);
            }
            bm.WinBattle();
        }
    }

    private bool DefeatAlly(AnimaActions ally, int selectAlly)
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
            bm.IsDefeat = true;
            bm.LoseBattle();
            return true;
            
        }
        return false;
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
    public IEnumerator FelixMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.FelixSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
                
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator PhobiaMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.PhobiaSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator LacrimaMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.LacrimaSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator AmareMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.AmareSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator IrascorMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.IrascorSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator HavetMultiSkill(EnemyActions enemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.HavetSkill(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar, bm.EnemyDamageBar[enemy.animaData.enemyIndex], weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on Allys for {Mathf.Ceil(enemy.maxDamage)}damage", true);
        bm.EnemyDamageText[bm.EnemyActions.IndexOf(enemy)].text = Mathf.Ceil(bm.EnemyDamageBar[bm.EnemyActions.IndexOf(enemy)].thisPoint).ToString();
        DamageParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator IrascorRoundSkill(EnemyActions enemy)
    {
        yield return new WaitForSeconds(0.5f);
        bm.Canvas.SetActive(false);
        allys = new List<Transform>();
        for (int i = 0; i < bm.AllyActions.Count; i++)
        {
            if (bm.AllyActions[i].animaData.Animadie) continue;
            allys.Add(bm.AllyBattleSetting.AllyInstance[i].transform);
        }
        yield return bm.CameraManager.ZoomMultiOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, allys,false, enemy.animaData.skillName[2]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.IrascorRound(enemy,bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar);
        bm.BattleLogManager.AddLog($"\"우루루쾅쾅\" 으로 아니마들이 for {Mathf.Ceil(enemy.damage)} 뜨거워", false);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
    }
    public IEnumerator HavetRoundSkill(EnemyActions enemy)
    {
        yield return new WaitForSeconds(0.5f);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomSingleIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, false, enemy.animaData.skillName[2]);
        bm.Canvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        yield return enemy.HavetRound(enemy, bm.AllyActions, bm.AllyBattleSetting, bm.AllyHealthBar);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name}가 \"우루루쾅쾅\"을 사용해 {enemy.downGold}골드가 도둑맞았다! ", false);
        List<int> allyList = new List<int>();

        int fast = 0;
        float enemySpeed = enemy.animaData.Speed;
        foreach (var ally in bm.AllyActions)
        {
            if (ally.animaData.Animadie)
            {
                if (enemySpeed <= ally.animaData.Speed)
                {
                    fast += 1;
                }
                allyList.Add(bm.AllyActions.IndexOf(ally));
            }
        }
        for (int i = 0; i < fast; i++)
        {
            bm.TurnIndex -= 1;
        }
        while (allyList.Count > 0)
        {
            if (DefeatAlly(bm.AllyActions[allyList[0]], allyList[0]))
            {
                yield break;
            }
            allyList.RemoveAt(0);
            if (allyList.Count != 0)
            {
                for (int j = 0; j < allyList.Count; j++)
                {
                    allyList[j] -= 1;
                }
            }
        }
    }
}
