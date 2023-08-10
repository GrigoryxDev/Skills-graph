public interface ISpendEarnService
{
    void Earn(ItemTypes item, int amount);
    void Spend(ItemTypes item, int amount);
    bool IsCouldSpend(ItemTypes item, int amount);
}