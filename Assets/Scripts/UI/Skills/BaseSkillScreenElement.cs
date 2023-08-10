using UnityEngine;

public abstract class BaseSkillScreenElement : MonoBehaviour
{
    protected SkillsScreenModel model;
    
    public void Init(SkillsScreenModel model)
    {
        this.model = model;
        OnInit(model);
    }

    public abstract void OnInit(SkillsScreenModel model);

    public abstract void OnReset();

    public abstract void OnUpdateSkillPoints();

    public abstract void OnSelect(SkillUiElement skillUiElement);
    public abstract void OnDeSelect();

    public abstract void OnLearn(SkillUiElement skillUiElement);
    public abstract void OnForget(SkillUiElement skillUiElement);
}