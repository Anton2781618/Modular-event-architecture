# Модульно-событийная архитектура

Простая и гибкая архитектура для Unity проектов с поддержкой модульности и событийной системы.

## Основные возможности

### GameEntity
Базовый класс для всех игровых объектов с поддержкой модульности:
```csharp
// Просто унаследуйтесь от GameEntity
public class Player : GameEntity 
{
    protected override void OnInit()
    {
        // Добавляйте любые модули
        AddModule<InputModule>();
        AddModule<EntityLifecycleModule>();
    }
}
```

### Готовые модули
- `EntityLifecycleModule` - управление жизненным циклом
- `InputModule` - обработка ввода
- `UIModules` - набор UI модулей
- `SystemControlModule` - управление системными функциями
- `FPSCounterModule` - счетчик FPS
- И другие...

### Система событий
Глобальная и локальная шины событий:
```csharp
// Подписка на событие
GlobalEventBus.Subscribe<GameStartEvent>(OnGameStart);

// Отправка события
GlobalEventBus.Publish(new GameStartEvent());
```

### Менеджеры
Готовые менеджеры для типовых задач:
- `GameManager`
- `UIManager`
- `LevelManager`
- `InputManager`

## Установка

1. Window > Package Manager
2. "+" > "Add package from git URL"
3. Вставьте: `https://github.com/Anton2781618/Modular-event-architecture.git`

## Быстрый старт

1. Создайте новый скрипт
2. Унаследуйтесь от GameEntity
3. Добавляйте модули в OnInit()
```csharp
public class Enemy : GameEntity 
{
    protected override void OnInit()
    {
        // Добавляем нужные модули
        AddModule<EntityLifecycleModule>();
    }
}
```

## Структура проекта

```
Assets/Scripts/Core/
├── Runtime/                  # Основной код
│   ├── EventBus/            # Система событий
│   ├── GameEntitys/         # Базовые сущности
│   ├── Modules/             # Готовые модули
│   └── Tools/               # Утилиты
└── Editor/                  # Редакторные расширения
```

## Создание своих модулей

```csharp
public class CustomModule : ModuleBase
{
    public override void OnInit()
    {
        // Инициализация
    }

    public override void OnUpdate()
    {
        // Логика обновления
    }
}
```

## Лицензия
MIT License
