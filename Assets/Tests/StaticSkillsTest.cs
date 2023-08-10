using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class StaticSkillsTest
{
    private ISkillsStaticService skillsStaticService;

    [SetUp]
    public void Setup()
    {
        skillsStaticService = new SkillsStaticService();
    }

    [Test]
    public void TestAllSkillsLinked()
    {
        //Arrange test
        var allStatic = skillsStaticService.GetAllSkills();
        bool result = true;
        string selfLinked = string.Empty;

        //Act test
        foreach (SkillStaticData staticSkill in allStatic)
        {
            if (staticSkill.Linked.Length < 1)
            {
                selfLinked += $"-Not linked:{staticSkill.ID}-";
                result = false;
            }
        }

        //Assert test
        Assert.AreEqual(true, result, message: selfLinked);
    }

    [Test]
    public void TestAllSkillsNotLinkSelf()
    {
        //Arrange test
        var allStatic = skillsStaticService.GetAllSkills();
        bool result = true;
        string selfLinked = string.Empty;

        //Act test
        foreach (SkillStaticData staticSkill in allStatic)
        {
            if (staticSkill.Linked.Contains(staticSkill.ID))
            {
                selfLinked += $"-Self linked:{staticSkill.ID}-";
                result = false;
            }
        }

        //Assert test
        Assert.AreEqual(true, result, message: selfLinked);
    }

    [Test]
    public void TestAllSkillIdsExistsInDb()
    {
        //Arrange test
        var allStatic = skillsStaticService.GetAllSkills();
        var visited = new HashSet<int>();
        bool result = true;

        //Act test
        foreach (SkillStaticData staticSkill in allStatic)
        {
            if (!visited.Contains(staticSkill.ID))
            {
                visited.Add(staticSkill.ID);
            }

            foreach (var item in staticSkill.Linked)
            {
                if (visited.Contains(staticSkill.ID))
                {
                    continue;
                }

                if (!skillsStaticService.TryGetStaticSkill(item, out _))
                {
                    result = false;
                }
            }
        }

        //Assert test
        Assert.AreEqual(true, result);
    }
}
