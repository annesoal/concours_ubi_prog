using Ennemies;

namespace Interfaces
{
 public interface ICanDamage
 {
   public int AttackDamage
   {
       get;
       set;
   }

   public void Attack(IDamageable damageable)
   {
      damageable.Damage(AttackDamage); 
   }
 }  
}