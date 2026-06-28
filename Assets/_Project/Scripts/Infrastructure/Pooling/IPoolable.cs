namespace ArenaShooter.Infrastructure.Pooling
{
    public interface IPoolable
    {
        void Spawn();
        void Despawn();
    }
}
