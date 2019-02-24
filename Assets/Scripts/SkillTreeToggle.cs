using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeToggle : MonoBehaviour
{
    //Attach this script to the toggle;
    public bool purchased = false;
    private Toggle Skill;

    // Start is called before the first frame update
    void Start()
    {
        Skill = GetComponent<Toggle>();
        Skill.isOn = false;
        Skill.onValueChanged.AddListener(delegate { GetUpgrade(); });
    }

    // Update is called once per frame
    void GetUpgrade()
    {
        if (!purchased)
        {
            Skill.isOn = true;
            purchased = true;
        }

        else
        {
            Skill.isOn = true;
        }
    }

}
