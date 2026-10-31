using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler<OnDeathEventArgs> OnDeath;

    public class OnDeathEventArgs : EventArgs
    {
        public Vector3 damageSourcePosition;
    }

    [SerializeField] private int healthPoints = 100;

    public void TakeDamage(int damage , Vector3 damageSourcePosition)
    {
        if (healthPoints <= 0)
            return;

        healthPoints -= damage;

        if (healthPoints < 0)
            healthPoints = 0;

        if(healthPoints == 0)
        {
            Die(damageSourcePosition);
        }
    }

    private void Die(Vector3 damageSourcePosition)
    {
        OnDeath?.Invoke(this, new OnDeathEventArgs
        {
            damageSourcePosition = damageSourcePosition
        });
    }
}
