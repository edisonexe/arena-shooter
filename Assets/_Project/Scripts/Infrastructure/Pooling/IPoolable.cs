namespace ArenaShooter.Infrastructure.Pooling
{
    public interface IPoolable<T>
    {
        void Spawn();
        void Despawn();
    }
}
