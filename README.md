# Модульная событийная архитектура

Этот проект представляет собой модульную событийную архитектуру для Unity, которая позволяет создавать гибкие и расширяемые игровые системы.

## Основные компоненты

### Система событий (Event System)

В проекте реализована гибкая система событий с двумя типами шин:
- `GlobalEventBus` - для глобальных событий
- `LocalEventBus` - для локальных событий внутри сущности

#### Типы событий

События определяются через интерфейс `IEventType`:
```csharp
public interface IEventType
{
    int GetEventId();
    string GetEventName();
}
```

#### Добавление новых типов событий

Чтобы добавить новые события в систему:

1. Добавьте новые события в существующий `ActionsTypeEnum`:
```csharp
public enum ActionsTypeEnum
{
    // Существующие события
    Unit_Die,
    Unit_Created,
    // Ваши новые события
    YourNewEvent1,
    YourNewEvent2
}
```

2. Добавьте статические свойства в класс `ActionsType`:
```csharp
public static ActionsType YourNewEvent1 => new ActionsType(ActionsTypeEnum.YourNewEvent1);
public static ActionsType YourNewEvent2 => new ActionsType(ActionsTypeEnum.YourNewEvent2);
```

#### Использование событий

```csharp
// Подписка на событие
GlobalEventBus.Instance.Subscribe(ActionsType.Unit_Created, OnUnitCreated);

// Публикация события
GlobalEventBus.Instance.Publish(ActionsType.Unit_Created, new CreateUnitEvent { Unit = this });

// Отписка от события
GlobalEventBus.Instance.Unsubscribe(ActionsType.Unit_Created, OnUnitCreated);
```

### MonoEventBus

`MonoEventBus` - базовый класс для сущностей, которые работают с событиями. Он автоматически управляет подпиской и отпиской от событий в течение жизненного цикла объекта.

```csharp
public class YourEntity : MonoEventBus
{
    protected override void Initialize()
    {
        // Добавляем подписки на глобальные события
        Globalevents.Add((ActionsType.YourEvent, (data) => HandleEvent((YourEventData)data)));
    }
}
```

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

## Лучшие практики

1. Используйте `LocalEventBus` для событий, которые должны быть обработаны только внутри конкретной сущности
2. Используйте `GlobalEventBus` для событий, которые должны быть доступны всей игре
3. Всегда отписывайтесь от событий при уничтожении объекта (это делается автоматически в `MonoEventBus`)
4. Создавайте отдельные классы данных событий, реализующие `IEventData`
5. Группируйте связанные события в отдельные классы внутри `События` для лучшей организации

## Примечания

- Система автоматически управляет жизненным циклом подписок через `MonoEventBus`
- Все события типобезопасны благодаря использованию интерфейса `IEventData`
- Система поддерживает добавление новых типов событий без изменения существующего кода
