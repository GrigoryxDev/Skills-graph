using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LearnButtonsPanel : BaseSkillScreenElement, ISkillScreenLearnUpdate, ISkillScreenSelectUpdate, ISkillScreenPointsUpdate
{
    private ISkillsDynamicService skillsDynamicService;

    [SerializeField] private Button learn;
    [SerializeField] private Button forget;

    [Inject]
    private void Constructor(ISkillsDynamicService skillsDynamicService)
    {
        this.skillsDynamicService = skillsDynamicService;
    }

    private void Start()
    {
        learn.onClick.AddListener(OnLearnClickInvoke);
        forget.onClick.AddListener(OnForgetClickInvoke);
    }

    private void OnLearnClickInvoke() => model.OnLearn?.Invoke(model.Selected);
    private void OnForgetClickInvoke() => model.OnForget?.Invoke(model.Selected);

    public override void OnInit(SkillsScreenModel model)
    {
        ActiveButtons(false, false);
    }

    public void OnUpdateSkillPoints()
    {
        if (model.Selected == null)
        {
            return;
        }
        learn.interactable = skillsDynamicService.CouldBeLearned(model.Selected.SkillId);
        forget.interactable = skillsDynamicService.CouldBeForget(model.Selected.SkillId);
    }

    public void OnDeSelect()
    {
        ActiveButtons(false, false);
    }

    public void OnForget(SkillUiElement skillUiElement)
    {
        ActiveButtons(true, false);
    }

    public void OnLearn(SkillUiElement skillUiElement)
    {
        ActiveButtons(false, true);
    }

    public override void OnReset()
    {
        ActiveButtons(false, false);
    }

    public void OnSelect(SkillUiElement skillUiElement)
    {
        learn.interactable = skillsDynamicService.CouldBeLearned(skillUiElement.SkillId);
        forget.interactable = skillsDynamicService.CouldBeForget(skillUiElement.SkillId);
    }

    private void ActiveButtons(bool isLearnActive, bool isForgetActive)
    {
        learn.interactable = isLearnActive;
        forget.interactable = isForgetActive;
    }
}