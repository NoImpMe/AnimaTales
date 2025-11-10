using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public class AnimaActions : MonoBehaviour
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
    public float damage;
    public float heal;
    public float maxDamage;
    public float maxHeal;
    public IEnumerator Attack(AnimaActions ally, EnemyActions enemy, IEnemyBattleSetting enemyPos, HealthBar enemyHealthBar, ParserBar damageBar)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcAttackDamage(ally.animaData.Damage, enemy);
            dn.Spawn(new Vector2(enemyPos.EnemyInstance[enemyPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.x - 0.1f, enemyPos.EnemyInstance[enemyPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.y + 0.1f), damage);
            yield return enemyHealthBar.TakeDamage(damage);
            enemy.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator Skill(AnimaActions ally, EnemyActions enemy, IEnemyBattleSetting enemyPos, HealthBar enemyHealthBar, ParserBar damageBar, float weight)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcSkillDamage(ally.animaData.Damage, enemy, weight);
            dn.Spawn(new Vector2(enemyPos.EnemyInstance[enemyPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.x - 0.1f, enemyPos.EnemyInstance[enemyPos.BattleManager.EnemyActions.IndexOf(enemy)].transform.position.y + 0.1f), damage);
            yield return enemyHealthBar.TakeDamage(damage);
            enemy.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator MultiSkill(AnimaActions ally, List<EnemyActions> enemy, IEnemyBattleSetting enemyPos, List<HealthBar> enemyHealthBar, ParserBar damageBar, float weight)
    {
        if (!ally.animaData.Animadie)
        {
            maxDamage = 0f;
            for(int i = 0; i < enemy.Count; i++)
            {
                if (!enemy[i].animaData.Animadie)
                {
                    damage = CalcSkillDamage(ally.animaData.Damage, enemy[i], weight);
                    dn.Spawn(new Vector2(enemyPos.EnemyInstance[i].transform.position.x - 0.1f, enemyPos.EnemyInstance[i].transform.position.y + 0.1f), damage);
                    if (maxDamage < damage)
                    {
                        maxDamage = damage;
                    }
                    enemy[i].TakeDamage(damage);
                    yield return enemyHealthBar[i].TakeDamage(damage);
                    
                }
            }
            yield return damageBar.PutDamage(maxDamage);
        }
    }
    public IEnumerator Heal(AnimaActions healer, AnimaActions target, IAllyBattleSetting targetPos, HealthBar allyHealthBar, ParserBar healBar, float weight)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            heal = CalcHealAmount(healer.animaData.Damage, target, weight);
            hn.Spawn(new Vector2(targetPos.AllyInstance[targetPos.BattleManager.AllyActions.IndexOf(target)].transform.position.x - 0.1f, targetPos.AllyInstance[targetPos.BattleManager.AllyActions.IndexOf(target)].transform.position.y + 0.1f), heal);
            yield return allyHealthBar.TakeHeal(heal);
            target.TakeHeal(heal);
            yield return healBar.PutDamage(heal);
        }
    }
    public IEnumerator MultiHeal(AnimaActions healer, List<AnimaActions> target, IAllyBattleSetting targetPos, List<HealthBar> allyHealthBar, ParserBar healBar, float weight)
    {
        if (!healer.animaData.Animadie)
        {
            maxHeal = 0f;
            for (int i = 0; i < target.Count; i++)
            {
                if (!target[i].animaData.Animadie)
                {
                    heal = CalcHealAmount(healer.animaData.Damage, target[i], weight);
                    hn.Spawn(new Vector2(targetPos.AllyInstance[i].transform.position.x - 0.1f, targetPos.AllyInstance[i].transform.position.y + 0.1f), heal);
                    if (maxHeal < heal)
                    {
                        maxHeal = heal;
                    }
                    target[i].TakeHeal(heal);
                    yield return allyHealthBar[i].TakeHeal(heal);
                }
            }
            yield return healBar.PutDamage(maxHeal);
        }
    }
    public IEnumerator Shield(AnimaActions healer, AnimaActions target, IAllyBattleSetting targetPos, ShieldBar allyShieldBar, float weight)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            heal = CalcShieldAmount(healer.animaData.Damage, target, weight);
            hn.Spawn(new Vector2(targetPos.AllyInstance[targetPos.BattleManager.AllyActions.IndexOf(target)].transform.position.x - 0.1f, targetPos.AllyInstance[targetPos.BattleManager.AllyActions.IndexOf(target)].transform.position.y + 0.1f), heal);
            yield return allyShieldBar.TakeShield(heal);
            target.TakeShield(heal);
        }
    }
    public IEnumerator MultiShield(AnimaActions healer, List<AnimaActions> target, IAllyBattleSetting targetPos, List<ShieldBar> allyHealthBar,  float weight)
    {
        if (!healer.animaData.Animadie)
        {
            maxHeal = 0f;
            for (int i = 0; i < target.Count; i++)
            {
                if (!target[i].animaData.Animadie)
                {
                    heal = CalcShieldAmount(healer.animaData.Damage, target[i], weight);
                    hn.Spawn(new Vector2(targetPos.AllyInstance[i].transform.position.x - 0.1f, targetPos.AllyInstance[i].transform.position.y + 0.1f), heal);
                    if (maxHeal < heal)
                    {
                        maxHeal = heal;
                    }
                    target[i].TakeShield(heal);
                    yield return allyHealthBar[i].TakeShield(heal);
                }
            }
        }
    }
    public IEnumerator IncreaseAbility(AnimaActions buffer, AnimaActions target, string[] abi, float weight)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthUp(buffer, target, weight);
                    break;
                case "speed":
                    yield return SpeedUp(buffer, target, weight);
                    break;
                case "defense":
                    yield return DefenseUp(buffer, target, weight);
                    break;
            }
        }
    }
    public IEnumerator MultiIncreaseAbility(AnimaActions buffer, List<AnimaActions> target, string[] abi, float weight)
    {
        for (int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strength":
                        StrengthUp(buffer, target[i], weight);
                        break;
                    case "speed":
                        SpeedUp(buffer, target[i], weight);
                        break;
                    case "defense":
                        DefenseUp(buffer, target[i], weight);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    private IEnumerator StrengthUp(AnimaActions buffer, AnimaActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["strength"] = target.animaData.Damage;
            target.animaData.Damage *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator StrengthDown(AnimaActions debuffer, EnemyActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["strength"] = target.animaData.Damage;
            target.animaData.Damage *= CalcDebuffRatio(debuffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator SpeedUp(AnimaActions buffer, AnimaActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator SpeedDown(AnimaActions debuffer, EnemyActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcDebuffRatio(debuffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator DefenseUp(AnimaActions buffer, AnimaActions target, float weight)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcBuffRatio(buffer.damage, weight);
        }
        yield return null;
    }
    private IEnumerator DefenseDown(AnimaActions debuffer, EnemyActions target, float weight)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcDebuffRatio(debuffer.damage, weight);
        }
        yield return null;
    }
    public IEnumerator DecreaseAbility(AnimaActions debuffer, EnemyActions target, string[] abi, float weight)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthDown(debuffer, target, weight);
                    break;
                case "speed":
                    yield return SpeedDown(debuffer, target, weight);
                    break;
                case "defense":
                    yield return DefenseDown(debuffer, target, weight);
                    break;
            }
        }
    }
    public IEnumerator MultiDecreaseAbility(AnimaActions debuffer, List<EnemyActions> target, string[] abi, float weight)
    {
        for (int i = 0; i < target.Count; i++)
        {
            foreach (string stat in abi)
            {
                switch (stat)
                {
                    case "strength":
                        StrengthDown(debuffer, target[i], weight);
                        break;
                    case "speed":
                        SpeedDown(debuffer, target[i], weight);
                        break;
                    case "defense":
                        DefenseDown(debuffer, target[i], weight);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    public void TakeDamage(float damage)
    {
        if(this.animaData.Shield > 0)
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
        if(this.animaData.Stamina > animaData.Maxstamina)
        {
            animaData.Stamina = animaData.Maxstamina;
        }
    }
    private float CalcAttackDamage(float damage , EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.Defense * 0.002f) * UnityEngine.Random.Range(0.95f, 1.11f);
    }

    private float CalcSkillDamage(float damage, EnemyActions enemy, float weight)
    {
        return damage * (1 - enemy.animaData.Defense * 0.002f) * UnityEngine.Random.Range(0.95f, 1.11f) * weight;
    }
    private float CalcHealAmount(float damage, AnimaActions target, float weight)
    {
        float a = damage * UnityEngine.Random.Range(0.95f, 1.11f) * weight;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    private float CalcShieldAmount(float damage, AnimaActions target, float weight)
    {
        return damage * UnityEngine.Random.Range(0.95f, 1.11f) * weight;
    }
    private float CalcBuffRatio(float damage, float weight)
    {
        return 0.0004f * damage + weight;
    }
    private float CalcDebuffRatio(float damage, float weight)
    {
        return -0.0002f * damage + weight;
    }
    public void Die()
    {
        this.animaData.Stamina = 0;
        this.animaData.Animadie = true;
    }
   
}
