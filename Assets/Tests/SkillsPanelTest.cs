using NUnit.Framework;
using UnityEditor;

public class SkillsPanelTest
{
    private ISkillsStaticService skillsStaticService;
    private SkillsPanel prefab;

    [SetUp]
    public void Setup()
    {
        skillsStaticService = new SkillsStaticService();
        var path = "Assets/Prefabs/UI/SkillView/SkillsPanel.prefab";
        prefab = (SkillsPanel)AssetDatabase.LoadAssetAtPath(path, typeof(SkillsPanel));
    }

    [Test]
    public void TestAllElementsCouldBePlaced()
    {
        //Arrange test
        var allStatic = skillsStaticService.GetAllSkills();
        SerializedObject serializedObject = new SerializedObject(prefab);
        SerializedProperty spawnedElementsProperty = serializedObject.FindProperty("spawnedElements");
        bool result = true;

        //Act test
        result = spawnedElementsProperty.arraySize == allStatic.Length;

        //Assert test
        Assert.AreEqual(true, result);
    }

    [TearDown]
    public void TearDown()
    {
        prefab = null;
    }
}
