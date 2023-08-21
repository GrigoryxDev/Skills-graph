using System.Collections.Generic;

public class SpendEarnService : ISpendEarnService
{
    private readonly Dictionary<ItemTypes, ISpendEarnProvider> spendEarnProviders;

    public SpendEarnService(IUserStorageService userStorageService)
    {
        //Could be created by zenject factory if need by DiContainer
        spendEarnProviders = new()
        {
            { ItemTypes.SkillPoint, new SkillsSpendEarnProvider(userStorageService.GetSkillsData()) }
        };

    }

    public void Earn(ItemTypes item, int amount)
    {
        var provider = spendEarnProviders[item];
        provider.Earn(amount);
    }

    public bool IsCouldSpend(ItemTypes item, int amount)
    {
        var provider = spendEarnProviders[item];
        return provider.IsCouldSpend(amount);
    }

    public void Spend(ItemTypes item, int amount)
    {
        var provider = spendEarnProviders[item];
        provider.Spend(amount);
    }
}