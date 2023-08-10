public interface IUserStorageService : IInitOnStart
{
    UserSkillsModel GetSkillsData();
    IReadOnlyUserSkillsModel GetSkillsReadOnlyData();
    void Reset();
}