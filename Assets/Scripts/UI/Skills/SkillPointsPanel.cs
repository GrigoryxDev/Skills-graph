using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class SkillPointsPanel : BaseSkillScreenElement
{
    private ISpendEarnService spendEarnService;
    private IUserStorageService userStorageService;
    private ISkillsStaticService skillsStaticService;

    [SerializeField] private Button increasePoints;
    [SerializeField] private TextMeshProUGUI currentPointsText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Inject]
    private void Constructor(ISpendEarnService spendEarnService, IUserStorageService userStorageService,
    ISkillsStaticService skillsStaticService)
    {
        this.spendEarnService = spendEarnService;
        this.userStorageService = userStorageService;
        this.skillsStaticService = skillsStaticService;
    }

    private void Start()
    {
        var skillData = userStorageService.GetSkillsReadOnlyData();
        skillData.ReadOnlySkillPoints.ObserveEveryValueChanged(x => x.Value)
        .DistinctUntilChanged().Subscribe(newVal =>
        {
            currentPointsText.text = $"Current skill points: <color=green><b>{newVal}";
        }).AddTo(this);

        increasePoints.onClick.AddListener(OnIncreasePoints);
    }

    private void OnIncreasePoints()
    {
        spendEarnService.Earn(ItemTypes.SkillPoint, 1);
        model.OnUpdateSkillPoints?.Invoke();
    }

    public override void OnInit(SkillsScreenModel model)
    {
        UpdatePrice(0);
    }

    public override void OnReset()
    {
        UpdatePrice(0);
    }

    public override void OnSelect(SkillUiElement skillUiElement)
    {
        if (skillsStaticService.TryGetStaticSkill(skillUiElement.SkillId, out var skill))
        {
            UpdatePrice(skill.Price);
        }
    }

    public override void OnDeSelect()
    {
        UpdatePrice(0);
    }

    public override void OnLearn(SkillUiElement skillUiElement)
    {
        return;
    }

    public override void OnForget(SkillUiElement skillUiElement)
    {
        return;
    }

    private void UpdatePrice(int price)
    {
        priceText.text = $"Price: <color=red><b>{price}";
    }

    public override void OnUpdateSkillPoints()
    {
        return;
    }
}