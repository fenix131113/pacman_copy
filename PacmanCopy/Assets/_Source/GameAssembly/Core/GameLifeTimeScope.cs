using Level;
using Player;
using Player.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private ScoresSettingsSO scoresSettingsSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            #region Player

            builder.Register<ITickable, PcPlayerInput>(Lifetime.Singleton).As<IPlayerInput>();
            builder.Register<Scores>(Lifetime.Singleton);
            builder.Register<PlayerHealth>(Lifetime.Singleton);

            #endregion

            builder.RegisterInstance(scoresSettingsSO);
            
            builder.RegisterComponentInHierarchy<BonusLoader>();

            builder.RegisterComponentInHierarchy<Map>();
        }
    }
}
