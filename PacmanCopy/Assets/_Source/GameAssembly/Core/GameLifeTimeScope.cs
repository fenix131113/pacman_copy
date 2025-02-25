using Player;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            #region Player

            builder.Register<ITickable, PcPlayerInput>(Lifetime.Singleton).As<IPlayerInput>();
            builder.Register<Scores>(Lifetime.Singleton);

            #endregion
        }
    }
}
