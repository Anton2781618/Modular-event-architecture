# Модульная событийная архитектура

Этот проект представляет собой модульную событийную архитектуру для Unity, которая позволяет создавать гибкие и расширяемые игровые системы.

## Версия и требования

- Версия: 1.0.1
- Требуемая версия Unity: 2022.3
- Зависимости: TextMeshPro 3.0.6

## Основные компоненты

### Система событий (Event System)

В проекте реализована гибкая система событий с двумя типами шин:
- `GlobalEventBus` - для глобальных событий, доступных всей игре
- `LocalEventBus` - для локальных событий внутри сущности

#### Типы событий

События определяются через интерфейс `IEventType`:
```csharp
public interface IEventType
{
    int Id { get; }
    string EventName { get; }
}
```

#### Добавление новых типов событий

Чтобы добавить новые события в систему:

Создайте новый класс, реализующий интерфейс `IEventType`:
```csharp
public class YourEventsType : IEventType
{
    private string _eventName;
    
    private YourEventsType(string eventName)
    {
        _eventName = eventName;
    }
    
    public int Id => ("YourEventsType_" + EventName).GetHashCode();
    public string EventName => _eventName;
    
    public static readonly IEventType YourEvent1 = new YourEventsType("YourEvent1");
    public static readonly IEventType YourEvent2 = new YourEventsType("YourEvent2");
}
```

#### Использование событий

```csharp
// Подписка на событие
GlobalEventBus.Instance.Subscribe<EventBase>(BasicActionsTypes.SystemRequirements.Test_Event, OnTestEvent);

// Публикация события
GlobalEventBus.Instance.Publish(BasicActionsTypes.Commands.Unit_Created, new CreateUnitEvent { Unit = this });

// Отписка от события
GlobalEventBus.Instance.Unsubscribe<EventBase>(BasicActionsTypes.SystemRequirements.Test_Event, OnTestEvent);
```

### Игровые сущности (Game Entities)

Проект использует иерархию классов сущностей:

- `GameEntity` - базовый класс для всех игровых сущностей
- `ManagerEntity` - базовый класс для менеджеров
- `UIEntity` - базовый класс для UI элементов
- `UnitEntity` - базовый класс для игровых юнитов

### MonoEventBus

`MonoEventBus` - базовый класс для сущностей, которые работают с событиями. Он автоматически управляет подпиской и отпиской от событий в течение жизненного цикла объекта.

### Модульная система

Проект использует модульную архитектуру, где каждая сущность может иметь множество модулей:

```csharp
public class YourEntity : GameEntity
{
    protected override void Initialize()
    {
        // Добавляем модули
        AddModule(new YourModule());
    }
}
```

### Менеджеры

В проекте реализованы различные менеджеры:
- `GameManager` - управление игровым процессом
- `LevelManager` - управление уровнем и сущностями на нем
- `NetworkManager` - управление сетевым взаимодействием
- `QuestManager` - управление квестами
- `SaveManager` - управление сохранениями
- `UIManager` - управление пользовательским интерфейсом

### Атрибуты совместимости

Для указания совместимости модулей с сущностями используются атрибуты:
- `[CompatibleUnit]` - указывает, с какими типами сущностей совместим модуль
- `[IncompatibleUnit]` - указывает, с какими типами сущностей несовместим модуль

## Примеры модулей

Проект включает несколько примеров модулей:
- Модуль инвентаря - базовая система инвентаря с настраиваемым количеством слотов
- Модуль диалогов - система диалогов с поддержкой автопропуска
- События для RPG - набор событий для RPG проекта
- Сетевой модуль - базовая реализация сетевого взаимодействия

## Лучшие практики

1. Используйте `LocalEventBus` для событий, которые должны быть обработаны только внутри конкретной сущности
2. Используйте `GlobalEventBus` для событий, которые должны быть доступны всей игре
3. Всегда отписывайтесь от событий при уничтожении объекта (это делается автоматически в `MonoEventBus`)
4. Создавайте отдельные классы данных событий, реализующие `IEventData`
5. Используйте атрибуты [CompatibleUnit] и [IncompatibleUnit] для указания совместимости модулей с сущностями
6. Используйте кэширование компонентов через метод `GetCachedComponent<T>()` для оптимизации производительности

## Примечания

- Система автоматически управляет жизненным циклом подписок через `MonoEventBus`
- Все события типобезопасны благодаря использованию интерфейса `IEventData`
- Система поддерживает добавление новых типов событий без изменения существующего кода
- Модульный подход позволяет легко расширять функциональность сущностей
- `GameEntity` является основным классом для создания игровых сущностей и управления модулями
- Модуль `EntityLifecycleModule` автоматически отслеживает создание и уничтожение сущностей на уровне

## Установка

1. Добавьте пакет в свой проект через Package Manager
2. Используйте namespace `ModularEventArchitecture` в своих скриптах
3. Наследуйтесь от `GameEntity` для создания своих игровых сущностей
4. Создавайте модули, наследуясь от `ModuleBase`

## Репозиторий

Исходный код доступен на GitHub: https://github.com/Anton2781618/Modular-event-architecture.git