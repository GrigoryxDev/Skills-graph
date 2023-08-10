public class SkillDynamicData
{
    public SkillStaticData GetStaticData { get; }
    public SkillDynamicState State { get; private set; }

    public SkillDynamicData(SkillStaticData staticData)
    {
        GetStaticData = staticData;
    }
    public bool IsLearned() => State is SkillDynamicState.Learned or
                   SkillDynamicState.LearnedImmutable;
    public void ChangeState(SkillDynamicState state) => State = state;
}

public enum SkillDynamicState
{
    NotLearned,
    LearnedImmutable,
    Learned,
    CouldBeLearn
}