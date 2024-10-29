using System;

//включает тип и все производные
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CompatibleUnitAttribute : Attribute
{
    public Type UnitType { get; }

    public CompatibleUnitAttribute(Type unitType)
    {
        UnitType = unitType;
    }
}