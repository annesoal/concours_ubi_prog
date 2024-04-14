namespace Ennemies
{
    public interface IDamageable
    {
        int Health { get; set; }
        int Damage(int damage);
    }
}