using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeTest : MonoBehaviour
{
    [SerializeField] SkillHandler sh;
    [SerializeField] Skill skillTobind;
    private void Start()
    {
        sh.AddSkillCurrency(100);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillTobind.TryUnlock();
        }
    }

    public void ThisIsTheFunctionIWannaBind()
    {
        Debug.Log("I just Bought this");
    }
}
