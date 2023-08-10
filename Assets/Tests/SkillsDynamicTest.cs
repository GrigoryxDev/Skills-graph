using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class SkillsDynamicTest
{
    private (UserStorageService userStorageService, ISkillsDynamicService skillsDynamicService, ISkillsStaticService skillsStaticService) GetServices()
    {
        var skillsStaticService = new SkillsStaticService();
        skillsStaticService.UpdateSkillsData(StaticDB.GetInitSkillsModel());
        var userStorageService = new UserStorageService(skillsStaticService);

        var skillsDynamicService = new SkillsDynamicService(skillsStaticService, userStorageService,
          new SpendEarnService(userStorageService));
        skillsDynamicService.Init();

        return (userStorageService, skillsDynamicService, skillsStaticService);
    }

    [Test]
    public void TestCircularTryForgetTwoSideLinkedLearn()
    {
        //Arrange test
        var (userStorageService, skillsDynamicService, skillsStaticService) = GetServices();
        var staticInitModel = new SkillsInitModel
        {
            learnedOnStart = new HashSet<int> { SkillConstants.BASE_START_ID },
            skillsDict = new Dictionary<int, SkillStaticData>
            {
                {SkillConstants.BASE_START_ID,new SkillStaticData{ID = SkillConstants.BASE_START_ID, Price = 0, Linked = new int[] { 1,7 } }},
                {1,new SkillStaticData{ID = 1, Price = 0, Linked = new int[] { 2 } }},
                {2,new SkillStaticData{ID = 2, Price = 0, Linked = new int[] { 3 } }},
                {3,new SkillStaticData{ID = 3, Price = 0, Linked = new int[] { 4 } }},
                {4,new SkillStaticData{ID = 4, Price = 0, Linked = new int[] { 5 } }},
                {5,new SkillStaticData{ID = 5, Price = 0, Linked = new int[] { 6 } }},
                {6,new SkillStaticData{ID = 6, Price = 0, Linked = new int[] { 7 } }},
                {7,new SkillStaticData{ID = 7, Price = 0, Linked = new int[] {  } }},
            }
        };
        staticInitModel.skills = staticInitModel.skillsDict.Values.ToArray();

        skillsStaticService.UpdateSkillsData(staticInitModel);
        skillsDynamicService.Init();

        var skillDta = userStorageService.GetSkillsData();
        skillDta.SkillPoints.Value += 100;
        bool result = true;

        //Act test
        skillsDynamicService.Learn(1);
        skillsDynamicService.Learn(2);
        skillsDynamicService.Learn(3);
        skillsDynamicService.Learn(4);
        skillsDynamicService.Learn(5);
        skillsDynamicService.Learn(6);
        skillsDynamicService.Learn(7);

        result = skillsDynamicService.CouldBeForget(2);

        //Assert test
        Assert.AreEqual(true, result);
    }

    [Test]
    public void TestTryForgetWithTwoSideLinkedLearn()
    {
        //Arrange test
        var (userStorageService, skillsDynamicService, skillsStaticService) = GetServices();
        var skillDta = userStorageService.GetSkillsData();
        skillDta.SkillPoints.Value += 100;
        bool result = true;

        //Act test
        skillsDynamicService.Learn(4);
        skillsDynamicService.Learn(5);
        skillsDynamicService.Learn(7);

        result = skillsDynamicService.CouldBeForget(5);

        //Assert test
        Assert.AreEqual(false, result);
    }

    [Test]
    public void TestAnyLearnableOnStart()
    {
        //Arrange test
        var (userStorageService, skillsDynamicService, skillsStaticService) = GetServices();
        bool result = true;

        //Act test
        var firstLearnable = skillsDynamicService.GetAllSkills().FirstOrDefault(x => x.Value.State == SkillDynamicState.CouldBeLearn);
        result = firstLearnable.Value != null;

        //Assert test
        Assert.AreEqual(true, result);
    }

    [Test]
    public void TestSkillLearn()
    {
        //Arrange test
        var (userStorageService, skillsDynamicService, skillsStaticService) = GetServices();
        var skillModel = userStorageService.GetSkillsData();
        skillModel.SkillPoints.Value += 10;
        var firstLearnable = skillsDynamicService.GetAllSkills().FirstOrDefault(x => x.Value.State == SkillDynamicState.CouldBeLearn);
        bool result = true;

        //Act test
        skillsDynamicService.Learn(firstLearnable.Key);
        result = skillModel.SkillPoints.Value == 9 &&
        skillModel.GetLearnedSkills.Contains(firstLearnable.Key) &&
        firstLearnable.Value.State == SkillDynamicState.Learned;

        //Assert test
        Assert.AreEqual(true, result);
    }

    [Test]
    public void TestSkillForget()
    {
        //Arrange test
        var (userStorageService, skillsDynamicService, skillsStaticService) = GetServices();
        var skillModel = userStorageService.GetSkillsData();
        skillModel.SkillPoints.Value += 10;
        var firstLearnable = skillsDynamicService.GetAllSkills().FirstOrDefault(x => x.Value.State == SkillDynamicState.CouldBeLearn);
        bool result = true;

        //Act test
        skillsDynamicService.Learn(firstLearnable.Key);
        skillsDynamicService.Forget(firstLearnable.Key);

        result = skillModel.SkillPoints.Value == 10 &&
        !skillModel.GetLearnedSkills.Contains(firstLearnable.Key) &&
        firstLearnable.Value.State == SkillDynamicState.CouldBeLearn;

        //Assert test
        Assert.AreEqual(true, result);
    }
}
