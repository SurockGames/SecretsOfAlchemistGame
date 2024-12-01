using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Code.Shooting
{
    [Serializable]
    public class Health
    {
        public event Action OnDie;
        public event Action OnChange;

        public int MaxHealth => maxHealth;
        public int CurrentHp => currentHealth;

        [ShowInInspector, ReadOnly]
        private int maxHealth, currentHealth;

        public Health (int maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
        }

        public bool TryDealDamage(int amount)
        {
            if (currentHealth == 0) return false;

            if (currentHealth - amount > 0)
            {
                currentHealth -= amount;
                //OnGetDamage?.Invoke(currentHealth, position);
                OnChange?.Invoke();
                return true;
            }
            else
            {
                currentHealth = 0;
                Die();
                OnDie?.Invoke();
                OnChange?.Invoke();
                return true;
            }
        }
        public bool TryHeal(int amount)
        {
            if (currentHealth == 0) return false;

            if (currentHealth + amount <= maxHealth)
            {
                currentHealth += amount;
                //OnGetDamage?.Invoke(currentHealth, position);
                OnChange?.Invoke();
                return true;
            }
            else
            {
                currentHealth = maxHealth;
                OnChange?.Invoke();
                return true;
            }
        }

        private void Die()
        {

        }
    }

    public interface IDamagable
    {
        bool TryDealDamage(int amount, Vector3 position);
    }
}