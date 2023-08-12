using System;

public class SkillsScreenModel
{
    public SkillUiElement Selected { get; private set; }

    public Action<SkillUiElement> OnSelect;
    public Action OnDeselect;
    public Action<SkillUiElement> OnForget;
    public Action<SkillUiElement> OnLearn;
    public Action OnUpdateSkillPoints;

    public void SetSkillUiElement(SkillUiElement newSelected) => Selected = newSelected;

}