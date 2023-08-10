using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AppStarter : MonoBehaviour
{
    private ISkillsDynamicService skillsDynamicService;
    private IUserStorageService userStorageService;
    private UIHolder uiHolder;

    [Inject]
    private void Constructor(ISkillsDynamicService skillsDynamicService,
    IUserStorageService userStorageService, UIHolder uiHolder)
    {
        this.skillsDynamicService = skillsDynamicService;
        this.userStorageService = userStorageService;
        this.uiHolder = uiHolder;
    }

    private void Start()
    {
        var initServices = new List<IInitOnStart>()
        {
            skillsDynamicService,
            userStorageService
        };

        foreach (var service in initServices)
        {
            service.Init();
        }

        uiHolder.Init();
    }
}
