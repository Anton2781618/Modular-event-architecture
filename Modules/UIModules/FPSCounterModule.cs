using TMPro;
using UnityEngine;

[CompatibleUnit(typeof(UIHelpMenu))]
public class FPSCounterModule : ModuleBase
{
    [Tools.Information("Этот модуль отображает текущий и средний FPS", Tools.InformationAttribute.InformationType.Info, false)]
    
    [Tooltip("Как часто обновлять FPS")]
    [SerializeField] [Range(0.1f, 1f)] private float _updateInterval = 0.5f; // Как часто обновлять FPS

    [Tooltip("Текстовое поле для отображения FPS")]
    [SerializeField] private TMP_Text _fpsText; // Текстовое поле для отображения FPS

    [Tooltip("Текстовое поле для отображения среднего FPS")]
    [SerializeField] private TMP_Text _averageFPSText; // Текстовое поле для отображения среднего FPS

    private float _timeleft;
    private float _currentFPS;
    private float _averageFPS;


    protected override void Initialize()
    {
        base.Initialize();

        _timeleft = _updateInterval;

        _fpsText.text = $"FPS: {_currentFPS:0.}"; // Отображаем текущий FPS
        _averageFPSText.text = $"Средний FPS: {_averageFPS:0.}"; // Отображаем средний FPS
    }

    public override void UpdateMe() => СalculateFPS();

    private void СalculateFPS()
    {
        _timeleft -= Time.deltaTime;

        if (_timeleft <= 0.0)
        {
            _currentFPS = 1.0f / Time.smoothDeltaTime; // Текущий FPS (сглаженный)
            _averageFPS = Time.frameCount / Time.time; // Средний FPS с начала игры
            _timeleft = _updateInterval;

            // Debug.Log($"Current FPS: {currentFPS}, Average FPS: {averageFPS}"); // Временно для проверки
            _fpsText.text = $"FPS: {_currentFPS:0.}"; // Отображаем текущий FPS
            _averageFPSText.text = $"Средний FPS: {_averageFPS:0.}"; // Отображаем средний FPS
        }
    }

    public float GetCurrentFPS() => _currentFPS;
    public float GetAverageFPS() => _averageFPS;
}
