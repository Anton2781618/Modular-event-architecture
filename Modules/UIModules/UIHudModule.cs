using UnityEngine;
using UnityEngine.UI;

[CompatibleUnit(typeof(Player))]
[CompatibleUnit(typeof(NPC))]
[CompatibleUnit(typeof(RopeController))]
public class UIHudModule : ModuleBase
{    
    public Hud hudPrefab;
    public Hud hud;
    public Canvas canvas;
    private Camera _camera;
    private Animator _animator;
    

    protected override void Initialize()
    {
        base.Initialize();

        _camera = Camera.main;

        if (!_animator) _animator = GetComponent<Animator>();

        LocalEvents.Subscribe<StaminaChangedEvent>(LocalEventBus.События.Выносливость_изменилась , UpdateStamina);

        LocalEvents.Subscribe<HealthChangedEvent>(LocalEventBus.События.Здоровье_изменилась , UpdateHealth);

        LocalEvents.Subscribe<HealthChangedEvent>(LocalEventBus.События.Команды.UI.Обновить_UI , UpdateHealth);

        LocalEvents.Subscribe<StunEvent>(LocalEventBus.События.Оглушен , Unconscious);

        LocalEvents.Subscribe<DieEvent>(LocalEventBus.События.Юнит_погиб , Die);

        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Состояния.Усталость, TiredProcess);

        if (!hud) CreateHud();

        hud.SetStatus($"{Character.name} <color=green>(в норме)</color>" );
    }

    private void TiredProcess(BaseEvent data)
    {
        _animator.SetBool("HardWalk", data.Enabled);

        if (data.Enabled) 
        {

            hud.CreateImage(data.sprite);
            
            hud.SetColorStaminaBar(Color.gray);
        }
        else
        {
            hud.DestroyAllImages();
            
            hud.ResetColorStaminaBar();
        } 

        hud.SetStatus(data.Enabled ? $"{Character.name} <color=orange>(устал)</color>" : $"{Character.name} <color=green>(в норме)</color>");

    }

    private void Die(DieEvent @event) => hud.SetStatus($"{Character.name} <color=red>(мертв)</color>");

    private void Unconscious(StunEvent @event)
    {
        if (@event.isStun)
        {
            hud.SetStatus($"{Character.name} <color=yellow>(оглушен)</color>" );
        } 
        else
        {
            hud.SetStatus($"{Character.name} <color=green>(в норме)</color>" );
        }
    }

    public override void UpdateMe()
    {
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - _camera.transform.position);
    }

    private void UpdateStamina(StaminaChangedEvent data) => hud.UpdateStamina(data.CurrentStamina / data.MaxStamina);

    private void UpdateHealth(HealthChangedEvent data) => hud.UpdateHealth(data.CurrentHealth / data.MaxHealth);

    private void CreateHud()
    {
        //создать canvas
        canvas = new GameObject("Canvas").AddComponent<Canvas>();
        
        canvas.gameObject.AddComponent<CanvasScaler>();
        
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        
        canvas.renderMode = RenderMode.WorldSpace;
        
        canvas.transform.SetParent(transform);
        
        canvas.transform.localPosition = new Vector3(0, 3, 0);
        
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(3, 1);


        //создать hud
        hud = Instantiate(hudPrefab, canvas.transform);

        hud.transform.localPosition = Vector3.zero;

        hud.transform.localRotation = Quaternion.identity;
    }
}
