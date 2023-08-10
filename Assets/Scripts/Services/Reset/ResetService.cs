public class ResetService : IResetService
{
    private readonly ISkillsDynamicService skillsDynamicService;
    private readonly IUserStorageService userStorageService;

    public ResetService(ISkillsDynamicService skillsDynamicService, IUserStorageService userStorageService)
    {
        this.skillsDynamicService = skillsDynamicService;
        this.userStorageService = userStorageService;
    }

    public void Reset()
    {
        skillsDynamicService.Reset();
        userStorageService.Reset();
    }
}