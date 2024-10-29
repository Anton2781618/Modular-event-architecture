using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoEventBus, IUI
{
    protected override void Initialize()
    {
        LocalEvents.Subscribe<StaminaChangedEvent>(LocalEventBus.События.Выносливость_изменилась , UpdateStamina);

        LocalEvents.Subscribe<HealthChangedEvent>(LocalEventBus.События.Здоровье_изменилась , UpdateHealth);

        LocalEvents.Subscribe<HealthChangedEvent>(LocalEventBus.События.Команды.UI.Обновить_UI , UpdateHealth);
    }

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image HpBar;
    [SerializeField] private Image StaminaBar;

    public GameObject _target;

    public void SetTarget(GameObject target)
    {
        _target = target;

        _name.text = _target.name;
    }

    public void UpdateStamina(StaminaChangedEvent data)
    {
        StaminaBar.fillAmount = data.CurrentStamina / data.MaxStamina;
    }

    public void UpdateHealth(HealthChangedEvent data)
    {
        HpBar.fillAmount = data.CurrentHealth / data.MaxHealth;        
    }
}

public interface IUI
{
    public void SetLocalEventBus(LocalEventBus localEventBus);
}
