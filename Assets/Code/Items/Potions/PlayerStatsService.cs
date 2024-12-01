using Assets.Code.Shooting;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerStatsService : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private int maxHealth;
    [SerializeField] private int poisonInSecondDamage;
    [SerializeField] private float maxAntiPoisonCooldown = 10;
    [SerializeField] private float maxAntiBlindnessCooldown = 10;

    public LayerMask withoutInvisibleMask;
    public LayerMask withInvisibleMask;

    public UnityEvent OnDrink;

    public Volume volumeDefault;
    public Volume volumeToxic;
    public Volume volumeDamaged;

    public Image antiPoisonEffectImage;
    public Image antiBlindnessEffectImage;
    public Slider healthBar;

    public GameObject inPoisonEffect;

    public bool CanSeeInvisible => canSeeInvisible;

    private Health health;
    public bool canSeeInvisible;
    public bool poisonable;

    public float antiPoisonCooldown;
    public float antiBlindnessCooldown;

    private bool inPoison;

    private float currentPoison;

    private void Awake()
    {
        health = new Health(maxHealth);
        health.OnDie += Death;
        health.OnChange += UpdateSlider;
    }

    private void UpdateSlider()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = health.MaxHealth;
        healthBar.value = health.CurrentHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryDrinkPotion();
        }

        if (antiBlindnessCooldown > 0)
        {
            canSeeInvisible = true;
            antiBlindnessCooldown -= Time.deltaTime;
            Camera.main.cullingMask = withInvisibleMask;
            antiBlindnessEffectImage.fillAmount = antiBlindnessCooldown / maxAntiBlindnessCooldown;
        }
        else
        {
            canSeeInvisible = false;
            Camera.main.cullingMask = withoutInvisibleMask;
            antiBlindnessEffectImage.fillAmount = 0;
        }

        if (antiPoisonCooldown > 0)
        {
            poisonable = false;
            antiPoisonCooldown -= Time.deltaTime;
            antiPoisonEffectImage.fillAmount = antiPoisonCooldown / maxAntiPoisonCooldown;
        }
        else
        {
            poisonable = true;
            antiPoisonEffectImage.fillAmount = 0;
        }

        if (inPoison && poisonable)
        {
            currentPoison += Time.deltaTime;
            volumeToxic.weight += Time.deltaTime;
            volumeToxic.weight = Mathf.Min(volumeToxic.weight, 1);
        }
        else
        {
            volumeToxic.weight -= Time.deltaTime;
            volumeToxic.weight = Mathf.Max(volumeToxic.weight, 0);
        }

        if (currentPoison > 1)
        {
            int poisonDamage = (int)currentPoison;
            health.TryDealDamage(poisonDamage);

            currentPoison -= poisonDamage;
        }

    }

    private void TryDrinkPotion()
    {
        if (game.PotionInventory.PotionInSlot)
        {
            health.TryHeal(game.PotionInventory.PotionInSlot.healthRegenAmount);

            var time = game.PotionInventory.PotionInSlot.timeWorking;

            if (time > 0)
            {
                switch (game.PotionInventory.PotionInSlot.EffectTypes)
                {
                    case PotionEffectTypes.AntiPoison:
                        antiPoisonCooldown += time;
                        antiPoisonCooldown = Mathf.Min(antiPoisonCooldown, maxAntiPoisonCooldown);
                        break;
                    case PotionEffectTypes.AntiBlindness:
                        antiBlindnessCooldown += time;
                        antiBlindnessCooldown = Mathf.Min(antiBlindnessCooldown, maxAntiBlindnessCooldown);
                        break;
                }
            }

            game.PotionInventory.RemoveItemFromInventory(game.PotionInventory.PotionInSlot, 1);
            OnDrink?.Invoke();
        }
    }

    private void Death()
    {
        game.Die();
    }

    public void GetDamage(int amount)
    {
        health.TryDealDamage(amount);

        StartCoroutine(GetDamageEffect());
    }

    public IEnumerator GetDamageEffect()
    {
        while (volumeDamaged.weight < 1)
        {
            volumeDamaged.weight += 0.3f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        while (volumeDamaged.weight > 0)
        {
            volumeDamaged.weight -= 0.3f;
            yield return new WaitForSeconds(0.1f);
        }

        volumeDamaged.weight = 0;
    }

    public void Heal(int amount)
    {
        health.TryHeal(amount);
    }

    public void ActivateAntiBlindEffect(float time)
    {
        antiBlindnessCooldown += time;
    }

    public void ActivateAntiPoisonEffect(float time)
    {
        antiPoisonCooldown += time;
    }

    public void InPoison()
    {
        inPoison = true;
    }

    public void OutFromPoison()
    {
        inPoison = false;
    }
}
