namespace AEngine;

[AttributeUsage(AttributeTargets.Field)]
public sealed class HeaderAttribute : Attribute
{
    private readonly string _name;
    public HeaderAttribute(string name) => this._name = name;
}