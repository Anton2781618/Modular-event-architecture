using System;

//исключает конкретный тип
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class IncompatibleUnitAttribute : Attribute
{
    public Type UnitType { get; }

    public IncompatibleUnitAttribute(Type unitType)
    {
        UnitType = unitType;
    }
}