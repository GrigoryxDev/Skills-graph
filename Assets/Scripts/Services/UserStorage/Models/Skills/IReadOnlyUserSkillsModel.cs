using System.Collections.Generic;
using UniRx;

public interface IReadOnlyUserSkillsModel
{
    IReadOnlyReactiveProperty<int> ReadOnlySkillPoints { get; }
    HashSet<int> GetLearnedSkills { get; }
}