using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModularEventArchitecture
{
    /// <summary>
    /// Модуль для обработки системных команд (перезапуск сцены и т.д.)
    /// </summary>
    [CompatibleUnit(typeof(UIHelpMenu))]
    public class SystemControlModule : ModuleBase
    {
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        [Tools.Button("Перезапустить сцену")]
        public void RestartCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        public override void UpdateMe()
        {
            
        }
    }
}