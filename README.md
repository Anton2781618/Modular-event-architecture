# Модульно-событийная архитектура

Базовый набор систем и утилит для Unity проектов, предоставляющий гибкую и расширяемую архитектуру.

## Основные компоненты

### EventBus
Система событий с поддержкой глобальных и локальных шин:
- `GlobalEventBus` - для общих событий
- `LocalEventBus` - для событий в рамках одной сущности
- `MonoEventBus` - для MonoBehaviour компонентов

### GameEntity
Базовые классы для игровых объектов:
- `GameEntity` - базовый класс для всех игровых сущностей
- `ManagerEntity` - для менеджеров (GameManager, UIManager и т.д.)
- `UIEntity` - для UI элементов
- `UnitEntity` - для игровых юнитов (Player, NPC)

### Модульная система
Расширяемая система модулей:
- `ModuleBase` - базовый класс для создания модулей
- Готовые модули:
  - `EntityLifecycleModule` - управление жизненным циклом
  - `InputModule` - обработка ввода
  - `UIModules` - набор UI модулей

### Инструменты
- Атрибуты для инспектора Unity
- Активаторы событий
- Редакторные расширения

## Установка

### Через Unity Package Manager

1. Window > Package Manager
2. "+" > "Add package from git URL"
3. Вставьте: `https://github.com/Anton2781618/caer-core.git`

## Примеры использования

### Создание игровой сущности
```csharp
public class Enemy : GameEntity 
{
    protected override void OnInit()
    {
        // Инициализация
        AddModule<EntityLifecycleModule>();
    }
}
```

### Работа с событиями
```csharp
// Подписка на событие
GlobalEventBus.Subscribe<GameStartEvent>(OnGameStart);

// Отправка события
GlobalEventBus.Publish(new GameStartEvent());

// Отписка
GlobalEventBus.Unsubscribe<GameStartEvent>(OnGameStart);
```

### Создание модуля
```csharp
public class CustomModule : ModuleBase
{
    public override void OnInit()
    {
        // Инициализация модуля
    }

    public override void OnUpdate()
    {
        // Логика обновления
    }
}
```

## Структура проекта

```
Assets/Scripts/Core/
├── ModularEventArchitecture/    # Основной код
│   ├── EventBus/               # Система событий
│   ├── GameEntitys/            # Базовые сущности
│   ├── Modules/                # Система модулей
│   └── Tools/                  # Утилиты
└── Editor/                     # Редакторные расширения
    ├── GameEntityEditor/       # Редактор сущностей
    └── ModulesEditor/          # Редактор модулей
```

## Лицензия
MIT License
