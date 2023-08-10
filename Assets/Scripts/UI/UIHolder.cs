using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    [SerializeField] private SkillsScreen skillsScreen;

    public void Init()
    {
        skillsScreen.Init();
    }
}
