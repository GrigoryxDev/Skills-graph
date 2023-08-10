public class UserStorageService : IUserStorageService
{
    private readonly ISkillsStaticService skillsStaticService;
    private readonly UserSkillsModel userSkillsModel = new();

    public UserStorageService(ISkillsStaticService skillsStaticService)
    {
        this.skillsStaticService = skillsStaticService;
    }

    public UserSkillsModel GetSkillsData() => userSkillsModel;
    public IReadOnlyUserSkillsModel GetSkillsReadOnlyData() => userSkillsModel;

    public void Init()
    {
        Reset();
    }

    public void Reset()
    {
        userSkillsModel.SkillPoints.Value = 0;
        userSkillsModel.LearnedSkills.Clear();
        var skillsLearnedOnStart = skillsStaticService.GetLearnedOnStart();
        foreach (var skill in skillsLearnedOnStart)
        {
            userSkillsModel.LearnedSkills.Add(skill);
        }
    }
}