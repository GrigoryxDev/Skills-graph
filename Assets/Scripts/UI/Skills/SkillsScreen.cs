using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillsScreen : MonoBehaviour
{
    private IResetService resetService;
    private ISkillsDynamicService skillsDynamicService;

    [SerializeField] private Button resetAll;
    [SerializeField] private BaseSkillScreenElement[] skillScreenPart;

    private readonly SkillsScreenModel model = new();

    private List<ISkillScreenLearnUpdate> skillScreenLearnUpdates;
    private List<ISkillScreenPointsUpdate> skillScreenPointsUpdates;
    private List<ISkillScreenSelectUpdate> skillScreenSelectUpdates;


    [Inject]
    private void Constructor(IResetService resetService,
    ISkillsDynamicService skillsDynamicService)
    {
        this.resetService = resetService;
        this.skillsDynamicService = skillsDynamicService;
    }

    public void Init()
    {
        PrepareData();
        InitButtons();

        InitScreenParts();
    }

    private void InitScreenParts()
    {
        foreach (var element in skillScreenPart)
        {
            InitScreenPart(element);
        }
    }

    private void InitScreenPart(BaseSkillScreenElement element)
    {
        element.Init(model);
        if (element.TryGetComponent<ISkillScreenLearnUpdate>(out var learn))
        {
            skillScreenLearnUpdates.Add(learn);
        }

        if (element.TryGetComponent<ISkillScreenPointsUpdate>(out var pointsUpdate))
        {
            skillScreenPointsUpdates.Add(pointsUpdate);
        }

        if (element.TryGetComponent<ISkillScreenSelectUpdate>(out var selectUpdate))
        {
            skillScreenSelectUpdates.Add(selectUpdate);
        }
    }

    private void InitButtons()
    {
        resetAll.onClick.AddListener(OnReset);
    }

    private void OnReset()
    {
        resetService.Reset();

        foreach (var element in skillScreenPart)
        {
            element.OnReset();
        }
    }

    private void PrepareData()
    {
        PrepareSkillPartGroups();

        model.OnSelect += OnSelect;
        model.OnDeselect += OnDeSelect;

        model.OnLearn += OnLearn;
        model.OnForget += OnForget;

        model.OnUpdateSkillPoints += OnUpdateSkillPoints;
    }

    private void PrepareSkillPartGroups()
    {
        skillScreenLearnUpdates = new List<ISkillScreenLearnUpdate>();
        skillScreenPointsUpdates = new List<ISkillScreenPointsUpdate>();
        skillScreenSelectUpdates = new List<ISkillScreenSelectUpdate>();
    }

    private void OnUpdateSkillPoints()
    {
        foreach (var element in skillScreenPointsUpdates)
        {
            element.OnUpdateSkillPoints();
        }
    }

    private void OnLearn(SkillUiElement skillUiElement)
    {
        skillsDynamicService.Learn(model.Selected.SkillId);

        foreach (var element in skillScreenLearnUpdates)
        {
            element.OnLearn(skillUiElement);
        }
    }

    private void OnForget(SkillUiElement skillUiElement)
    {
        skillsDynamicService.Forget(model.Selected.SkillId);
        foreach (var element in skillScreenLearnUpdates)
        {
            element.OnForget(skillUiElement);
        }
    }

    private void OnSelect(SkillUiElement skillUiElement)
    {
        foreach (var element in skillScreenSelectUpdates)
        {
            element.OnSelect(skillUiElement);
        }
    }

    private void OnDeSelect()
    {
        foreach (var element in skillScreenSelectUpdates)
        {
            element.OnDeSelect();
        }
    }
}
