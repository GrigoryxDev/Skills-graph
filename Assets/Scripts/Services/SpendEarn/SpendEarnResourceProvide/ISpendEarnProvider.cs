public interface ISpendEarnProvider
{
    void Earn(int amount);
    bool IsCouldSpend(int amount);
    void Spend(int amount);
}