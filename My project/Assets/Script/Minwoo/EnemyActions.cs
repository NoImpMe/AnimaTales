using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public enum ActionType { Attack, UseSkill }
    public List<ActionWeight> actionWeights;
    public string performance = "";
    public float damage;
    public float heal;
    public List<float> damages;
    public List<float> heals;
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
    public IEnumerator Attack(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Skill(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar, ParserBar damageBar)
    {

        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcSkillDamage(enemy.animaData.Damage, ally);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator MultiSkill(EnemyActions enemy, List<AnimaActions> ally, List<HealthBar> allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie)
        {
            damages = new List<float>();
            damage = 0f;
            for (int i = 0; i < ally.Count; i++)
            {
                if (!ally[i].animaData.Animadie)
                {
                    damages.Add(CalcSkillDamage(enemy.animaData.Damage, ally[i]));
                    allyHealthBar[i].TakeDamage(damages[i]);
                    ally[i].TakeDamage(damages[i]);
                    damage += damages[i];
                }
            }
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator Heal(EnemyActions healer, EnemyActions target, HealthBar enemyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            heal = CalcHealAmount(healer.animaData.Damage, target);
            yield return enemyHealthBar.TakeHeal(heal);
            target.TakeHeal(heal);
            yield return healBar.PutDamage(heal);
        }
    }

    public IEnumerator MultiHeal(EnemyActions healer, List<EnemyActions> target, List<HealthBar> enemyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie)
        {
            heals = new List<float>();
            heal = 0f;
            for(int i = 0; i < target.Count; i++)
            {
                if (!target[i].animaData.Animadie)
                {
                    heals.Add(CalcHealAmount(healer.animaData.Damage, target[i]));
                    enemyHealthBar[i].TakeHeal(heals[i]);
                    target[i].TakeHeal(heal);
                    heal += heals[i];
                }
            }
            yield return healBar.PutDamage(heal);
        }
    }
    public IEnumerator MultiIncreaseAbility(EnemyActions buffer, List<EnemyActions> target, string[] abi)
    {
        yield return null;
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
}
