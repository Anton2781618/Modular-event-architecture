using System;
using UnityEngine;
using UnityEngine.Events;


namespace Tools
{

    [Serializable]
    public class CustomUnityEvent : UnityEvent{}

    //Класс для работы с UI, спасобен вклбючать/выключать объекты, сохранять данные в буфер итп
    public class EventActivator : MonoBehaviour
    {
        [SerializeField] private Mod mod;
        [SerializeField] private CustomUnityEvent customUnityEvent;
        private string bufer;
        private bool hover = false;
        
        private enum Mod
        {
            Выполнить_по_ховеру, 
            Выполнить_по_клику,
            Выполнять_постоянно,
            Выполнить_при_старте,
            Выполнить_при_включении,
            Выполнить_при_выключении,
            Не_выполнять,
        }

        public void OnHover(bool isOver)
        {
            hover = isOver;
            
            CheckedMode(Mod.Выполнить_по_ховеру);
        }

        //проверить правильность выбора мода и выполнить
        private void CheckedMode(Mod value)
        {
            if(mod != value) return;

            Execute();
        }

        private void Start() => CheckedMode(Mod.Выполнить_при_старте);
        
        private void OnEnable() => CheckedMode(Mod.Выполнить_при_включении);

        private void OnDisable() => CheckedMode(Mod.Выполнить_при_выключении);
        
        public void OnClick() => CheckedMode(Mod.Выполнить_по_клику);

        private void Update() => CheckedMode(Mod.Выполнять_постоянно);

        public void Execute() => customUnityEvent?.Invoke();
        public void Ховер_объекта(GameObject value) => value.SetActive(hover);
        public void Активация_объекта(GameObject value) => value.SetActive(!value.activeSelf);
        public void Включить(GameObject value) => value.SetActive(true);
        public void Выключить(GameObject value) => value.SetActive(false);

        public void Курсор_невидимый() => Cursor.visible = false;
        
        public void Курсор_видимый() => Cursor.visible = true;

        //получить объект по индексу
        public object GetTypePersistentTarget(int index) => customUnityEvent.GetPersistentTarget(index);
    }
}


