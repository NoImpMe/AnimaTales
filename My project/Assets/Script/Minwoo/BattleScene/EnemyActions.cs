using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    private DamageNumber dn;
    public DamageNumber DN
    {
        get => dn;
        set => dn = value;
    }
    private DamageNumber hn;
    public DamageNumber HN
    {
        get => hn;
        set => hn = value;
    }
    public enum ActionType { Attack, UseSkill }
    public List<ActionWeight> actionWeights;
    public string performance = "";
    public float damage;
    public float heal;
    public float maxDamage;
    public float maxHeal;
    public bool isBoss = false;
    public int downGold;
    public class ActionWeight
    {
        public ActionType actionType;
        public float weight;
    }
    public void SetCustomWeights(List<ActionWeight> customWeights)
    {
        actionWeights = customWeights;
    }

    public void InitializeWeights()
    {
        if (actionWeights == null || actionWeights.Count == 0)
        {
            actionWeights = new List<ActionWeight>
            {
                new ActionWeight { actionType = ActionType.Attack, weight = 1.0f },
                new ActionWeight { actionType = ActionType.UseSkill, weight = 1.0f }
            };
        }
    }

    public void DecideAction()
    {
        float totalWeight = 0f;
        foreach (ActionWeight actionWeight in actionWeights)
        {
            totalWeight += actionWeight.weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (ActionWeight actionWeight in actionWeights)
        {
            cumulativeWeight += actionWeight.weight;
            if (randomValue <= cumulativeWeight)
            {
                SetAction(actionWeight.actionType);
                return;
            }
        }
    }
    public void SetAction(ActionType actionType)
    {
        if (isBoss)
        {
            switch (actionType)
            {
                case ActionType.Attack:
                    performance = "BossAttack";
                    break;
                case ActionType.UseSkill:
                    performance = "BossSkill";
                    break;
            }
        }
        else
        {
            switch (actionType)
            {
                case ActionType.Attack:
                    performance = "Attack";
                    break;
                case ActionType.UseSkill:
                    performance = "Skill";
                    break;
            }
        }
            
    }
    public IEnumerator Attack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Skill(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {

        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcSkillDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
  
    
    public IEnumerator MultiSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);
                    
                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator Heal(EnemyActions healer, EnemyActions target, IEnemyBattleSetting targetPos, HealthBar enemyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            heal = CalcHealAmount(healer.animaData.Damage, target);
            hn.Spawn(new Vector2(targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(target)].transform.position.x - 0.1f, targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(target)].transform.position.y + 0.1f), heal);
            yield return enemyHealthBar.TakeHeal(heal);
            target.TakeHeal(heal);
            yield return healBar.PutDamage(heal);
        }
    }

    public IEnumerator MultiHeal(EnemyActions healer, List<EnemyActions> target, IEnemyBattleSetting targetPos, List<HealthBar> enemyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie)
        {
            maxHeal = 0f;
            for(int i = 0; i < target.Count; i++)
            {
                if (!target[i].animaData.Animadie)
                {
                    heal = CalcHealAmount(healer.animaData.Damage, target[i]);
                    hn.Spawn(new Vector2(targetPos.EnemyInstance[i].transform.position.x - 0.1f, targetPos.EnemyInstance[i].transform.position.y + 0.1f), heal);
                    if (maxHeal < heal)
                    {
                        maxHeal = heal;
                    }
                    target[i].TakeHeal(heal);
                    yield return enemyHealthBar[i].TakeHeal(heal);
                }
            }
            yield return healBar.PutDamage(maxHeal);
        }
    }
    public IEnumerator MultiIncreaseAbility(EnemyActions buffer, List<EnemyActions> target, string[] abi)
    {
        for(int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strength":
                        StrengthUp(buffer, target[i]); 
                        break;
                    case "speed":
                        SpeedUp(buffer, target[i]);
                        break;
                    case "defense":
                        DefenseUp(buffer, target[i]);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator IncreaseAbility(EnemyActions buffer, EnemyActions target, string[] abi)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthUp(buffer, target);
                    break;
                case "speed":
                    yield return SpeedUp(buffer, target);
                    break;
                case "defense":
                    yield return DefenseUp(buffer, target);
                    break;
            }
        }
    }
    private IEnumerator StrengthUp(EnemyActions buffer, EnemyActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["strength"] = target.animaData.Damage;
            target.animaData.Damage *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator StrengthDown(EnemyActions debuffer, AnimaActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["strength"] = target.animaData.Damage;
            target.animaData.Damage *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    private IEnumerator SpeedUp(EnemyActions buffer, EnemyActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator SpeedDown(EnemyActions debuffer, AnimaActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    private IEnumerator DefenseUp(EnemyActions buffer, EnemyActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator DefenseDown(EnemyActions debuffer, AnimaActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    public IEnumerator DecreaseAbility(EnemyActions debuffer, AnimaActions target, string[] abi)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthDown(debuffer, target);
                    break;
                case "speed":
                    yield return SpeedDown(debuffer, target);
                    break;
                case "defense":
                    yield return DefenseDown(debuffer, target);
                    break;
            }
        }
    }
    public IEnumerator MultiDecreaseAbility(EnemyActions debuffer, List<AnimaActions> target, string[] abi)
    {
        for (int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strength":
                        StrengthDown(debuffer, target[i]);
                        break;
                    case "speed":
                        SpeedDown(debuffer, target[i]);
                        break;
                    case "defense":
                        DefenseDown(debuffer, target[i]);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    private float CalcAttackDamage(float damage, AnimaActions ally)
    {
        return damage * (1 - ally.animaData.Defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    private float CalcSkillDamage(float damage, AnimaActions ally)
    {
        return damage * (1 - ally.animaData.Defense * 0.002f) * Random.Range(0.95f, 1.11f) * 1.13f;
    }
    private float CalcHealAmount(float damage, EnemyActions target)
    {
        float a = damage * Random.Range(0.95f, 1.11f) * 1.13f;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    private float CalcBuffRatio(float damage)
    {
        return 0.0004f * damage + 1.02f;
    }
    private float CalcDebuffRatio(float damage)
    {
        return -0.0002f * damage + 0.94f;
    }
    public void TakeDamage(float damage)
    {
        this.animaData.Stamina -= damage;
        
        if (this.animaData.Stamina <= 0)
        {
            Die();
        }
        
    }
    public void TakeHeal(float heal)
    {
        this.animaData.Stamina += heal;
        if (this.animaData.Stamina > animaData.Maxstamina)
        {
            animaData.Stamina = animaData.Maxstamina;
        }
    }
    public void Die()
    {
        this.animaData.Stamina = 0;
        this.animaData.Animadie = true;
    }

    public IEnumerator FelixAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator PhobiaAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator LacrimaAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator AmareAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator IrascorAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator HavetAttack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator FelixSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator PhobiaSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator LacrimaSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator AmareSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator IrascorSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator HavetSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);

                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator FelixRound(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, IEnemyBattleSetting targetPos, HealthBar allyHealthBar, HealthBar enemyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            heal = damage;
            hn.Spawn(new Vector2(targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.x - 0.1f, targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.y + 0.1f), heal);
            yield return enemyHealthBar.TakeHeal(heal);
            enemy.TakeHeal(heal);
        }
    }
    public IEnumerator LacrimaRound(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
        }
    }
    public IEnumerator AmareRound(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
        }
    }
    public IEnumerator IrascorRound(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar)
    {
        if (!enemy.animaData.Animadie)
        {

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i]) / 6;
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    ally[i].TakeDamage(damage);
                    yield return allyHealthBar[i].TakeDamage(damage);
                }
            }
        }
    }
    public IEnumerator HavetRound(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar)
    {
        if (!enemy.animaData.Animadie )
        {
            TextMeshProUGUI textUI = GoldManager.Instance.GoldText;
            
            downGold = enemy.animaData.level * 10;
            if (GoldManager.Instance.GetCurrentGold() <= downGold)
            {
                yield return GoldManager.Instance.SpendGold(GoldManager.Instance.GetCurrentGold());
                for (int i = 0; i < ally.Count; i++)
                {
                    if (!ally[i].animaData.Animadie)
                    {
                        damage = 77777f;
                        dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                        ally[i].TakeDamage(damage);
                        yield return allyHealthBar[i].TakeDamage(damage);
                    }
                }
            }
            else
            {
                yield return GoldManager.Instance.SpendGold(downGold);
            }
        }
    }
}
