using UnityEngine;
using System.Collections.Generic;
using System;

namespace ModularEventArchitecture
{
    [DefaultExecutionOrder(-150)]
    public class LevelManager : ManagerEntity
    {
        /* LevelManager в событийно-ориентированной архитектуре должен отвечать за:

        1)Управление состоянием уровня:
            Инициализация уровня
            Управление игровым циклом (старт, пауза, завершение)
            Отслеживание целей/условий уровня
            Управление состоянием игрового мира
        
        2)Спавн и управление сущностями:
            Создание начальных сущностей
            Управление точками спавна
            Отслеживание активных сущностей
            Очистка/сброс уровня
        
        3)Игровые события уровня:
            Триггеры событий уровня
            Контрольные точки
            Условия победы/поражения
            Переходы между состояниями уровня
            Взаимодействие с другими менеджерами:
            Уведомление UIManager о событиях уровня
            Координация с другими системами

        LevelManager НЕ должен:
            Управлять UI
            Заниматься конкретной игровой логикой сущностей
            Обрабатывать ввод игрока */
            
        [SerializeField] private GameObject text;
        
        protected override void Initialize()
        {
            Globalevents.Add((GlobalEventBus.События.Команды.Показать_текст_в_точке, (data) => ShowText((ShowTextEvent)data)));
        }

        internal void ShowText(ShowTextEvent obj)
        {
            text.transform.position = obj.Point;

            text.SetActive(obj.Enabled);
        }
    }
}