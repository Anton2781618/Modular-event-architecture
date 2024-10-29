# Модульно-событийная архитектура

Базовый набор систем и утилит для Unity проектов:

- EventBus - Система событий с поддержкой глобальных и локальных шин
- Modules - Модульная архитектура для игровых сущностей
- GameEntity - Базовые классы для игровых объектов
- Utils - Набор полезных утилит и расширений

## Установка

### Через Unity Package Manager

1. Откройте Window > Package Manager
2. Нажмите "+" в верхнем левом углу
3. Выберите "Add package from git URL"
4. Вставьте URL вашего репозитория: `https://github.com/YourUsername/caer-core.git`

### Ручная установка

1. Склонируйте репозиторий
2. Скопируйте содержимое в папку Packages вашего проекта

## Использование

```csharp
// Пример использования GameEntity
public class Enemy : GameEntity 
{
    protected override void OnInit()
    {
        // Инициализация компонентов
    }
}

// Пример использования EventBus
public class GameManager : MonoBehaviour
{
    void Start()
    {
        GlobalEventBus.Subscribe<GameStartEvent>(OnGameStart);
    }
    
    private void OnGameStart(GameStartEvent evt)
    {
        // Обработка события
    }
}
