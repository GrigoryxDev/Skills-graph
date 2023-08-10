using System.Collections.Generic;
using UniRx;

public class UserSkillsModel : IReadOnlyUserSkillsModel
{
    public readonly ReactiveProperty<int> SkillPoints = new();
    public readonly HashSet<int> LearnedSkills = new(); //Could be used in game logic

    public IReadOnlyReactiveProperty<int> ReadOnlySkillPoints => SkillPoints;
    public HashSet<int> GetLearnedSkills => new HashSet<int>(LearnedSkills);
}