public class SpendEarnService : ISpendEarnService
{
    private readonly IUserStorageService userStorageService;
    public SpendEarnService(IUserStorageService userStorageService)
    {
        this.userStorageService = userStorageService;
    }

    public void Earn(ItemTypes item, int amount)
    {
        switch (item)
        {
            case ItemTypes.SkillPoint:
                var skillData = userStorageService.GetSkillsData();
                skillData.SkillPoints.Value += amount;
                break;
        }
    }

    public bool IsCouldSpend(ItemTypes item, int amount)
    {
        switch (item)
        {
            case ItemTypes.SkillPoint:
                var skillData = userStorageService.GetSkillsData();
                return skillData.SkillPoints.Value >= amount;
        }

        return false;
    }

    public void Spend(ItemTypes item, int amount)
    {
        switch (item)
        {
            case ItemTypes.SkillPoint:
                var skillData = userStorageService.GetSkillsData();
                skillData.SkillPoints.Value -= amount;
                break;
        }
    }
}