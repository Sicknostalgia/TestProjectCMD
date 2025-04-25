using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFillImage;
    public Animator anim;

    private bool isInvincible = false;
    private float invincibilityThreshold = 0.15f;
    private float invincibilityDuration = 2f;
    private void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthFillImage.color = healthGradient.Evaluate(1f);

        GameManager.Instance.OnPlayerDamaged
            .Subscribe(dmg => TakeDamage(dmg))
            .AddTo(this);
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;
        anim.Play("gotHit");
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthSlider.value = currentHealth;

        float healthPercentage = (float)currentHealth / maxHealth;
        healthFillImage.color = healthGradient.Evaluate(healthPercentage);

        if (healthPercentage <= invincibilityThreshold && currentHealth > 0)
        {
            StartCoroutine(TemporaryInvincibility());
        }

        if (currentHealth <= 0)
        {
            GameManager.Instance.OnPlayerDied.OnNext(Unit.Default);
            Destroy(gameObject);
        }
    }

    private IEnumerator TemporaryInvincibility()
    {
        isInvincible = true;
        anim.Play("gotHit", -1, 0f);

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }
}
