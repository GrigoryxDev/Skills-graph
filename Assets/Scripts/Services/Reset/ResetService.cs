public class ResetService : IResetService
{
    private readonly ISkillsDynamicService skillsDynamicService;

    public ResetService(ISkillsDynamicService skillsDynamicService)
    {
        this.skillsDynamicService = skillsDynamicService;
    }

    public void Reset()
    {
        skillsDynamicService.Reset();
    }
}