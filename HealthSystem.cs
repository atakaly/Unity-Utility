using System;

public class HealthSystem
{
    public Action<float, float> OnHealthChanged;

    private float m_CurrentHealth;
    private float m_MaximumHealth;

    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
        private set { m_CurrentHealth = value; }
    }

    public float MaximumHealth
    {
        get { return m_MaximumHealth; }
        private set { m_MaximumHealth = value; }
    }

    public HealthSystem(float amount, float maxAmount)
    {
        Initialize(amount, maxAmount);
    }

    /// <summary>
    /// Initialize health manager with current amount and maximum amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="maxAmount"></param>
    public void Initialize(float amount, float maxAmount)
    {
        CurrentHealth = amount;
        MaximumHealth = maxAmount;
    }

    /// <summary>
    /// Set health to given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void SetHealth(float amount)
    {
        CurrentHealth = amount;
        OnHealthChanged?.Invoke(m_CurrentHealth, m_MaximumHealth);
    }

    /// <summary>
    /// Increase or decrease health.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(float amount)
    {
        CurrentHealth += amount;
        OnHealthChanged?.Invoke(m_CurrentHealth, m_MaximumHealth);
    }

    /// <summary>
    /// Set maximum health to given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void SetMaximumHealth(float amount)
    {
        MaximumHealth = amount;
    }
}
