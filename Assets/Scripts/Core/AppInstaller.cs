using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AppInstaller : MonoInstaller
{
    [SerializeField] private UIHolder uIHolder;

    public override void InstallBindings()
    {
        Container.Bind<IResetService>().To<ResetService>().AsSingle().NonLazy();

        Container.Bind<ISkillsStaticService>().To<SkillsStaticService>().AsSingle().NonLazy();
        Container.Bind<ISkillsDynamicService>().To<SkillsDynamicService>().AsSingle().NonLazy();

        Container.Bind<ISpendEarnService>().To<SpendEarnService>().AsSingle().NonLazy();

        Container.Bind<IUserStorageService>().To<UserStorageService>().AsSingle().NonLazy();

        Container.Bind<UIHolder>().FromInstance(uIHolder).AsSingle().NonLazy();

        Container.Bind().FromInstance(gameObject);
    }
}
