using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_UI : MonoBehaviour
{
    int healthPerPip = 0;
    int lastSavedHealth;
    [SerializeField]
    List<GameObject> healthPips = new List<GameObject>(),
                     healthDivisions = new List<GameObject>();
    [SerializeField] GameObject healthPip;
    [SerializeField] Ship.Health health;
    private void Start()
    {
        Populate();
        health.onHealthChange += UpdateHealth;
        lastSavedHealth = healthDivisions.Count - 1;
        UpdateHealth();
    }
    private void Populate()
    {
        healthPerPip = healthPip.transform.childCount;
        int pipAmount = Mathf.CeilToInt((float)Ship.Health.MAX_HEALTH / healthPerPip);
        for (int i = 0; i < pipAmount; i++)
        {
            GameObject pip = GameObject.Instantiate(healthPip, this.transform);
            healthPips.Add(pip);
            foreach (Transform child in pip.transform)
            {
                healthDivisions.Add(child.gameObject);
            }
        }
    }
    private void UpdateHealth()
    {
        //lost Life
        if (lastSavedHealth - health.CurrentHealth < 0)
        {
            for (int i = lastSavedHealth; i < health.CurrentHealth - 1; i++)
            {
                healthDivisions[healthDivisions.Count - i - 1].SetActive(true);
            }
        }//Gained Life
        else if (lastSavedHealth - health.CurrentHealth > 0)
        {
            for (int i = health.CurrentHealth; i < lastSavedHealth; i++)
            {
                healthDivisions[healthDivisions.Count - i - 1].SetActive(false);
            }
        }
        lastSavedHealth = health.CurrentHealth;
    }
}
