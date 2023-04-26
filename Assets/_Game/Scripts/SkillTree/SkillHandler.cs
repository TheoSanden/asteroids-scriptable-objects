using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill : ScriptableObject
{   
    public enum Type 
    {
         none, 
         LAZER_TIER1,
         LAZER_TIER2,
         LAZER_DOUBLE,
         LAZER_TIER3,
         LAZER_TIER4,
         LAZER_AOE,
    }
    public void TryUnlock()
    {
        tryUnlockEvent?.Raise(this);
    }
    public void Unlock()
    {
        unlockEvent.Raise();
    }
    public Type Name
    {
        get => name;
    }
    public Type[] UnlockRequirements
    {
        get => unlockRequirments;
    }
    public int Tiers 
    {
        get => tiers;
    }
    public int Cost { get => cost; }

    public SkillEvent tryUnlockEvent;
    public GameEvent  unlockEvent;
    [SerializeField] Type name;
    [SerializeField] Type[] unlockRequirments;
    [SerializeField] int tiers;
    [SerializeField] int cost;
}
public class SkillHandler : MonoBehaviour
{
    Dictionary<Skill.Type, int> unlockedSkills = new Dictionary<Skill.Type, int>();
    int skillCurrency = 0;

    public void AddSkillCurrency(int amount)
    {
        if(amount < 0) { amount = 0; }
        skillCurrency += amount;
    }
    public void OnTryUnlock(Skill skill)
    {
       // if (skill.Cost > skillCurrency) { return; }
        foreach(Skill.Type type in skill.UnlockRequirements)
        {
            if (!unlockedSkills.ContainsKey(type) && type != Skill.Type.none) { return;}
        }
        if(unlockedSkills.ContainsKey(skill.Name) && unlockedSkills[skill.Name] >= skill.Tiers)
        {
            return; 
        }
        if (!ProcessCurrency(skill.Cost))
        {
            Debug.Log("Currency Too low");
            return; 
        }
        if (unlockedSkills.ContainsKey(skill.Name))
        {
            unlockedSkills[skill.Name]++;
        }
        else
        {
            unlockedSkills.Add(skill.Name,1);
        }
        skill.unlockEvent.Raise();
    }
    private bool ProcessCurrency(int cost)
    {
        if(cost > skillCurrency) { return false; }
        skillCurrency -= cost;
        return true;
    }
}
