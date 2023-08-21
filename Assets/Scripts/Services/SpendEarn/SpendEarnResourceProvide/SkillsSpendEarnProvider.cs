public class SkillsSpendEarnProvider : ISpendEarnProvider
{
    private readonly UserSkillsModel userSkillsModel;

    public SkillsSpendEarnProvider(UserSkillsModel userSkillsModel)
    {
        this.userSkillsModel = userSkillsModel;
    }

    public void Earn(int amount)
    {
        userSkillsModel.SkillPoints.Value += amount;
    }

    public bool IsCouldSpend(int amount)
    {
        return userSkillsModel.SkillPoints.Value >= amount;
    }

    public void Spend(int amount)
    {
        userSkillsModel.SkillPoints.Value -= amount;
    }
}