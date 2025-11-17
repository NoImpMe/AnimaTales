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
        if (animaData.type == "Irascor")
        {
            SetAction(ActionType.Attack);
            return;
        }
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
    public IEnumerator Attack(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, ParserBar damageBar = null)
    {
        
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            if (ally.animaData.Shield > 0)
            {
                yield return allyHealthBar.shieldBar.TakeDamage(damage);
            }
            yield return allyHealthBar.TakeDamage(damage - enemy.animaData.Shield);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Skill(EnemyActions enemy, AnimaActions ally, IAllyBattleSetting allyPos, HealthBar allyHealthBar, float weight, ParserBar damageBar = null)
    {

        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcSkillDamage(enemy.animaData.Damage, ally, weight);
            dn.Spawn(new Vector2(allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.x - 0.1f, allyPos.AllyInstance[allyPos.BattleManager.AllyActions.IndexOf(ally)].transform.position.y + 0.1f), damage);
            if (ally.animaData.Shield > 0)
            {
                yield return allyHealthBar.shieldBar.TakeDamage(damage);
            }
            yield return allyHealthBar.TakeDamage(damage - enemy.animaData.Shield);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
  
    
    public IEnumerator MultiSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, float weight, ParserBar damageBar = null)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
                    dn.Spawn(new Vector2(allyPos.AllyInstance[i].transform.position.x - 0.1f, allyPos.AllyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    if (ally[i].animaData.Shield > 0)
                    {
                        yield return allyHealthBar[i].shieldBar.TakeDamage(damage);
                    }
                    yield return allyHealthBar[i].TakeDamage(damage - enemy.animaData.Shield);
                    ally[i].TakeDamage(damage);
                    
                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator Heal(EnemyActions healer, EnemyActions target, IEnemyBattleSetting targetPos, HealthBar enemyHealthBar, float weight, ParserBar healBar = null)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            heal = CalcHealAmount(healer.animaData.Damage, target, weight);
            hn.Spawn(new Vector2(targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(target)].transform.position.x - 0.1f, targetPos.EnemyInstance[targetPos.BattleManager.EnemyActions.IndexOf(target)].transform.position.y + 0.1f), heal);
            yield return enemyHealthBar.TakeHeal(heal);
            target.TakeHeal(heal);
            yield return healBar.PutDamage(heal);
        }
    }

    public IEnumerator MultiHeal(EnemyActions healer, List<EnemyActions> target, IEnemyBattleSetting targetPos, List<HealthBar> enemyHealthBar, float weight, ParserBar healBar = null)
    {
        if (!healer.animaData.Animadie)
        {
            maxHeal = 0f;
            for(int i = 0; i < target.Count; i++)
            {
                if (!target[i].animaData.Animadie)
                {
                    heal = CalcHealAmount(healer.animaData.Damage, target[i], weight);
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
    public IEnumerator MultiIncreaseAbility(EnemyActions buffer, List<EnemyActions> target, string[] abi, float weight)
    {
        for(int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strengthup":
                        StrengthUp(buffer, target[i], weight); 
                        break;
                    case "speedup":
                        SpeedUp(buffer, target[i], weight);
                        break;
                    case "defenseup":
                        DefenseUp(buffer, target[i], weight);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator IncreaseAbility(EnemyActions buffer, EnemyActions target, string[] abi, float weight)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strengthup":
                    yield return StrengthUp(buffer, target, weight);
                    break;
                case "speedup":
                    yield return SpeedUp(buffer, target, weight);
                    break;
                case "defenseup":
                    yield return DefenseUp(buffer, target, weight);
                    break;
            }
        }
    }
    private IEnumerator StrengthUp(EnemyActions buffer, EnemyActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("strengthup"))
        {
            target.animaData.tmpAbility["strengthup"] = target.animaData.Damage;
            target.animaData.Damage *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator StrengthDown(EnemyActions debuffer, AnimaActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("strengthdown"))
        {
            target.animaData.tmpAbility["strengthdown"] = target.animaData.Damage;
            target.animaData.Damage = CalcDebuffRatio(target.animaData.Damage, debuffer.damage, weight) < 0 ? 0 : CalcDebuffRatio(target.animaData.Damage, debuffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator SpeedUp(EnemyActions buffer, EnemyActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("speedup"))
        {
            target.animaData.tmpAbility["speedup"] = target.animaData.Speed;
            target.animaData.Speed *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator SpeedDown(EnemyActions debuffer, AnimaActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("speeddown"))
        {
            target.animaData.tmpAbility["speeddown"] = target.animaData.Speed;
            target.animaData.Speed = CalcDebuffRatio(target.animaData.Speed, debuffer.damage, weight) < 0 ? 0 : CalcDebuffRatio(target.animaData.Speed, debuffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator DefenseUp(EnemyActions buffer, EnemyActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("defenseup"))
        {
            target.animaData.tmpAbility["defenseup"] = target.animaData.Defense;
            target.animaData.Defense *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator DefenseDown(EnemyActions debuffer, AnimaActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie && !target.animaData.tmpAbility.ContainsKey("defensedown"))
        {
            target.animaData.tmpAbility["defensedown"] = target.animaData.Defense;
            target.animaData.Defense = CalcDebuffRatio(target.animaData.Defense, debuffer.damage, weight) < 0 ? 0 : CalcDebuffRatio(target.animaData.Defense, debuffer.damage, weight);
        }
        yield return null;
    }
    public IEnumerator DecreaseAbility(EnemyActions debuffer, AnimaActions target, string[] abi, float weight)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strengthdown":
                    yield return StrengthDown(debuffer, target, weight);
                    break;
                case "speeddown":
                    yield return SpeedDown(debuffer, target, weight);
                    break;
                case "defensedown":
                    yield return DefenseDown(debuffer, target, weight);
                    break;
            }
        }
    }
    public IEnumerator MultiDecreaseAbility(EnemyActions debuffer, List<AnimaActions> target, string[] abi, float weight)
    {
        for (int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strengthdown":
                        StrengthDown(debuffer, target[i], weight);
                        break;
                    case "speeddown":
                        SpeedDown(debuffer, target[i], weight);
                        break;
                    case "defensedown":
                        DefenseDown(debuffer, target[i], weight);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    private float CalcAttackDamage(float damage, AnimaActions ally)
    {
        return damage * (1000f / (1000f + ally.animaData.Defense)) * UnityEngine.Random.Range(0.95f, 1.11f);
    }

    private float CalcSkillDamage(float damage, AnimaActions ally, float weight)
    {
        return damage * (900f / (900f + ally.animaData.Defense)) * UnityEngine.Random.Range(0.95f, 1.11f);
    }
    private float CalcHealAmount(float damage, EnemyActions target, float weight)
    {
        float a = damage * Random.Range(0.95f, 1.11f) * 1.13f;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    private float CalcBuffRatio(float damage, float weight)
    {
        return 0.0002f * damage + weight;
    }
    private float CalcDebuffRatio(float stat, float damage, float weight)
    {
        return (damage * -0.0002f + (weight - 1)) * (damage);
    }
    public void TakeDamage(float damage)
    {
        if (this.animaData.Shield > 0)
        {
            float remainDamage = Mathf.Min(this.animaData.Shield, damage);
            this.animaData.Shield -= remainDamage;
            damage -= remainDamage;
        }

        this.animaData.Stamina -= damage;
        
        if (this.animaData.Stamina <= 0)
        {
            Die();
        }
        
    }
    public void TakeShield(float shield)
    {
        this.animaData.Shield += shield;
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
    public IEnumerator FelixSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
    public IEnumerator PhobiaSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
    public IEnumerator LacrimaSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
    public IEnumerator AmareSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
    public IEnumerator IrascorSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
    public IEnumerator HavetSkill(EnemyActions enemy, List<AnimaActions> ally, IAllyBattleSetting allyPos, List<HealthBar> allyHealthBar, ParserBar damageBar, float weight)
    {
        if (!enemy.animaData.Animadie)
        {
            maxDamage = 0f;

            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(enemy.animaData.Damage, ally[i], weight);
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
                    damage = CalcAttackDamage(enemy.animaData.Damage, ally[i]) / 6;
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
