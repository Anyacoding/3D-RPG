using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    public event Action<int, int> UpdateHealthBarOnAttack;

    #region 生命周期
    void Awake() {
        // 深拷贝
        if (templateData != null) {
            characterData = Instantiate(templateData);
        }
    }

#endregion

#region Read from CharacterData_SO
    public int MaxHealth {
        get => characterData == null ? 0 : characterData.maxHealth;
        set => characterData.maxHealth = value;
    }

    public int CurrentHealth {
        get => characterData == null ? 0 : characterData.currentHealth;
        set => characterData.currentHealth = value;
    }

    public int BaseDefence {
        get => characterData == null ? 0 : characterData.baseDefence;
        set => characterData.baseDefence = value;
    }

    public int CurrentDefence {
        get => characterData == null ? 0 : characterData.currentDefence;
        set => characterData.currentDefence = value;
    }

    public float LevelMultiplier {
        get => 1 + (characterData.currentLevel - 1) * characterData.levelBuff;
    }

    public int CurrentLevel {
        get => characterData == null ? 1 : characterData.currentLevel;
        set => characterData.currentLevel = value;
    }

    public int MaxLevel {
        get => characterData == null ? 1 : characterData.maxLevel;
        set => characterData.maxLevel = value;
    }

    public int BaseExp {
        get => characterData == null ? 0 : characterData.baseExp;
        set => characterData.baseExp = value;
    }

    public int CurrentExp {
        get => characterData == null ? 1 : characterData.currentExp;
        set => characterData.currentExp = value;
    }

    public float LevelBuff {
        get => characterData == null ? 0.0f : characterData.levelBuff;
        set => characterData.levelBuff = value;
    }


#endregion


#region Character Combat
    public void TakeDamage(CharacterStats attacker, CharacterStats defender) {
        int damage = Math.Max(attacker.CurrentDamage() - CurrentDefence, 0);
        CurrentHealth = Math.Max(CurrentHealth - damage, 0);

        if (attacker.attackData.isCritical) {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        // DONE: 经验升级
        if (defender.CurrentHealth <= 0) {
            attacker.UpdateExp(defender.characterData.KillPoint);
        }
    }

    public void TakeDamage(int damage, CharacterStats defender) {
        int currentDamage = Math.Max(damage - CurrentDefence, 0);
        CurrentHealth = Math.Max(CurrentHealth - currentDamage, 0);

        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        // DONE: 经验升级
        if (defender.CurrentHealth <= 0) {
            var attacker = GameManager.Instance.playerStats;
            attacker.UpdateExp(defender.characterData.KillPoint);
        }
    }

    private int CurrentDamage() {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (attackData.isCritical) {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
#endregion

#region Level Up
    public void UpdateExp(int point) {
        characterData.currentExp += point;

        while (characterData.currentExp >= characterData.baseExp) {
            LevelUp();
        }
    }

    private void LevelUp() {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 1, MaxLevel);
        BaseExp += (int)(BaseExp * LevelMultiplier);
        MaxHealth = (int)(MaxHealth * LevelMultiplier);
        CurrentHealth = MaxHealth;
        Debug.Log("LEVEL UP " + CurrentLevel + "!   Max Health: " + MaxHealth);
    }

#endregion

}
