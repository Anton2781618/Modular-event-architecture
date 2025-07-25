using System;

namespace ModularEventArchitecture
{
    // Атрибут совместимости по тегу сущности
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CompatibleUnitAttribute : Attribute
    {
        public EntityTag Tag { get; }

        public CompatibleUnitAttribute(EntityTag tag)
        {
            Tag = tag;
        }
    }
}