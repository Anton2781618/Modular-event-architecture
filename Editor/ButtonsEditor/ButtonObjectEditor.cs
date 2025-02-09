using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using Tools;

[CustomEditor(typeof(MonoBehaviour), true)] // Изменили с Object на MonoBehaviour
public class ButtonObjectEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var methods = target.GetType().GetMethods(flags);
        
        
        var buttonMethods = methods.Where(m => 
            m.GetParameters().Length == 0 && 
            m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0).ToList(); // Добавили ToList()
                
        foreach (var method in buttonMethods)
        {
            var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            
            if (buttonAttribute != null)
            {
                string buttonName = buttonAttribute.buttonName ?? ObjectNames.NicifyVariableName(method.Name);
                
                if (GUILayout.Button(buttonName))
                {
                    foreach (var targetObj in targets)
                    {
                        method.Invoke(targetObj, null);
                    }
                }
            }
        }
        
        base.DrawDefaultInspector();
    }
}