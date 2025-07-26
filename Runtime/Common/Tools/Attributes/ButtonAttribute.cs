using System;

namespace ModularEventArchitecture
{
    public enum ButtonMode
    {
        AlwaysEnabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }
    /// <summary>
    /// Attribute to create a button in the inspector for calling the method it is attached to.
    /// The method must be public and have no arguments.
    /// </summary>
    /// <example>
    /// [Button("Моя Кнопка")]
    /// public void MyMethod()
    /// {
    ///     Debug.Log("Clicked!");
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ButtonAttribute : Attribute
    {
        public ButtonMode mode;
        public string buttonName;

        public ButtonAttribute(string buttonName = null, ButtonMode mode = ButtonMode.AlwaysEnabled)
        {
            this.buttonName = buttonName;
            this.mode = mode;
        }

        public ButtonAttribute(ButtonMode mode = ButtonMode.AlwaysEnabled)
        {
            this.buttonName = null;
            this.mode = mode;
        }
    }
}
