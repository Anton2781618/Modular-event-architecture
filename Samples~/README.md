# Дополнительные модули для Modular-event-architecture

## Установка модулей

1. Откройте Package Manager в Unity (Window > Package Manager)
2. Найдите пакет "Modular-event-architecture"
3. В правой панели найдите секцию "Samples"
4. Нажмите "Import" рядом с нужным модулем

## Доступные модули

### Inventory Module
Базовый модуль инвентаря с настраиваемым количеством слотов.

Использование:
```csharp
// Добавьте модуль к любому GameEntity
[SerializeField] private InventoryModule inventoryModule;

// Или добавьте через код
var inventoryModule = gameEntity.AddModule<InventoryModule>();
```

### Dialog Module
Модуль системы диалогов с поддержкой автоматического пропуска.

Использование:
```csharp
// Добавьте модуль к любому GameEntity
[SerializeField] private DialogModule dialogModule;

// Или добавьте через код
var dialogModule = gameEntity.AddModule<DialogModule>();
```

## Особенности
- Модули автоматически интегрируются с существующей системой событий
- Каждый модуль имеет свой Assembly Definition для правильной организации зависимостей
- Модули можно добавлять и удалять в любой момент через Package Manager
