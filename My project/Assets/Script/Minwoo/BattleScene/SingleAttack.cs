
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
public class SingleAttack:MonoBehaviour
{
    IBattleManager bm;
    List<string> expiredBuffList;
    AbilityManager abilityManager;
    public void initialize(IBattleManager bm)
    {
        this.bm = bm;
        abilityManager = GameObject.Find("Game Manager").GetComponent<AbilityManager>();
    }

    public IEnumerator SingleAllyAttack(AnimaActions anima , int selectEnemy)
    {
        PrepareAttack(anima);

        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.attackName);
        bm.Canvas.SetActive(true);
        yield return anima.Attack(anima, bm.EnemyActions[selectEnemy], bm.EnemyBattleSetting ,bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)]);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} hit {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(anima.damage)}damage", true);
        bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.EnemyActions[selectEnemy].animaData.Animadie)
        {
            if (anima.animaData.Speed < bm.EnemyActions[selectEnemy].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatEnemy(bm.EnemyActions[selectEnemy], selectEnemy);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(anima.animaData);


    }
    public IEnumerator SingleAllySkill(AnimaActions anima, int selectEnemy, int skillNum, float weight)
    {
        PrepareAttack(anima);

        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.skillName[skillNum]);

        bm.Canvas.SetActive(true);
        yield return anima.Skill(anima, bm.EnemyActions[selectEnemy], bm.EnemyBattleSetting, bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)], weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(anima.damage)}damage", true);
        bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.EnemyActions[selectEnemy].animaData.Animadie)
        {
            if (anima.animaData.Speed < bm.EnemyActions[selectEnemy].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatEnemy(bm.EnemyActions[selectEnemy], selectEnemy);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }

        BuffUpdate(anima.animaData);

    }

    public IEnumerator SingleEnemyAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.attackName);
        bm.Canvas.SetActive(true);
        yield return enemy.Attack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} hit {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if(DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator SingleEnemySkill(EnemyActions enemy, int selectAlly, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.Skill(enemy, bm.AllyActions[selectAlly],bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], weight, bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }

        BuffUpdate(enemy.animaData);

    }

    public IEnumerator SingleAllyHeal(AnimaActions anima, int selectAlly, int skillNum, float weight)
    {
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomSingleIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.Heal(anima, bm.AllyActions[selectAlly],bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.AllyHealBar[bm.AllyActions.IndexOf(anima)], weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(anima.heal)}heal", true);
        bm.AllyHealText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyHealBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyHeal(EnemyActions enemy, int selectEnemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.Heal(enemy, bm.EnemyActions[selectEnemy], bm.EnemyBattleSetting, bm.EnemyHealthBar[selectEnemy], weight, bm.EnemyHealBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(enemy.heal)} heal", false);
        bm.EnemyHealText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyHealBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator SingleAllyShield(AnimaActions anima, int selectAlly, int skillNum, float weight)
    {
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomSingleIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.Shield(anima, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyShieldBar[selectAlly], weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name}ÀÌ \"{anima.animaData.skillName[skillNum]}\"¸¦ »ç¿ëÇØ {bm.AllyActions[selectAlly].animaData.Name} ¿¡°Ô{Mathf.Ceil(anima.heal)}¹è¸®¾î ÁÜ", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleAllyBuff(AnimaActions anima, int selectAlly, int skillNum, float weight)
    {
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomSingleIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.IncreaseAbility(anima, bm.AllyActions[selectAlly], bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {string.Join(", ",bm.MatchedSkill[0].Affect)} up", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyBuff(EnemyActions enemy, int selectEnemy, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomSingleIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.IncreaseAbility(enemy, bm.EnemyActions[selectEnemy], bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} up", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator SingleAllyDebuff(AnimaActions anima, int selectEnemy, int skillNum, float weight)
    {
        PrepareAttack(anima);
        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.DecreaseAbility(anima, bm.EnemyActions[selectEnemy], bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyDebuff(EnemyActions enemy, int selectAlly, float weight)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.DecreaseAbility(enemy, bm.AllyActions[selectAlly], bm.MatchedSkill[0].Affect.ToArray(), weight);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator FelixSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.FelixAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator PhobiaSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.PhobiaAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator LacrimaSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.LacrimaAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator AmareSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.AmareAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator IrascorSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.IrascorAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator HavetSingleAttack(EnemyActions enemy, int selectAlly)
    {
        enemy.animaData.turnCheck = true;
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.HavetAttack(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();

        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);

    }
    public IEnumerator FelixRoundSkill(EnemyActions enemy, int selectAlly)
    {
        yield return new WaitForSeconds(0.5f);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomRoundOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, enemy.animaData.skillName[2]);
        bm.Canvas.SetActive(true);
        yield return enemy.FelixRound(enemy, bm.AllyActions[selectAlly], bm.AllyBattleSetting,bm.EnemyBattleSetting, bm.AllyHealthBar[selectAlly], bm.EnemyHealthBar[0]);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"¿ì·ç·çÄçÄç\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            yield return new WaitForSeconds(1f);
            if (DefeatAlly(bm.AllyActions[selectAlly], selectAlly))
            {
                yield break;
            }
        }


    }
    public IEnumerator PhobiaRoundSkill(EnemyActions enemy, List<AnimaDataSO> turnList, int selectAlly)
    {
        yield return new WaitForSeconds(0.5f);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomRoundOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, enemy.animaData.skillName[2]);
        bm.Canvas.SetActive(true);
        foreach(var anima in turnList)
        {
            if(ReferenceEquals(anima, bm.AllyActions[selectAlly].animaData))
            {
                anima.turnCheck = true;
                bm.Turn[turnList.IndexOf(anima)].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
            }
        }
        bm.BattleLogManager.AddLog($"\"¿ì·ç·çÄçÄç\"À¸·Î {bm.AllyActions[selectAlly].animaData.Name}Àº ÅÏ ºÀÀÎµÊ", false);
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
        foreach(var max in bm.AllyHealBar)
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
                if (UnityEngine.Random.Range(0, 101) <= (enemy.animaData.DropRate * (1+abilityManager.DropSymbol)) && !enemy.animaData.isClone)
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
        bm.EnemyBattleSetting.EnemyObjPrefab.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.EnemyInfoPrefab.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.EnemyHpPrefab.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.EnemyParserPrefab.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.BattleEnemyAnima.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.EnemyHpInstance.RemoveAt(selectEnemy);
        bm.EnemyHealthBar.RemoveAt(selectEnemy);
        bm.EnemyShieldBar.RemoveAt(selectEnemy);
        bm.EnemyActions.RemoveAt(selectEnemy);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInstance[selectEnemy]);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInfoInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyInstance.RemoveAt(selectEnemy);
        bm.EnemyBattleSetting.EnemyInfoInstance.RemoveAt(selectEnemy);
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
            if(bm.RunningCoroutine != null)
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
        while (expiredBuffList.Count > 0)
        {
            switch (expiredBuffList[0])
            {
                case "strengthup":
                    anima.Damage = anima.CalcStat(anima.level, anima.weight, anima.defAP);
                    anima.tmpAbility.Remove("strengthup");
                    break;
                case "speedup":
                    anima.Speed = anima.CalcStat(anima.level, anima.weight, anima.defSP);
                    anima.tmpAbility.Remove("speedup");
                    break;
                case "defenseup":
                    anima.Defense = anima.CalcStat(anima.level, anima.weight, anima.defDP);
                    anima.tmpAbility.Remove("defenseup");
                    break;
                case "strengthdown":
                    anima.Damage = anima.CalcStat(anima.level, anima.weight, anima.defAP);
                    anima.tmpAbility.Remove("strengthdown");
                    break;
                case "speeddown":
                    anima.Speed = anima.CalcStat(anima.level, anima.weight, anima.defSP);
                    anima.tmpAbility.Remove("speeddown");
                    break;
                case "defensedown":
                    anima.Defense = anima.CalcStat(anima.level, anima.weight, anima.defDP);
                    anima.tmpAbility.Remove("defensedown");
                    break;
            }
            expiredBuffList.RemoveAt(0);
        }
    }

}
