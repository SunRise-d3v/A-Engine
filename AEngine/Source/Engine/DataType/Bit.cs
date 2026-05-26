namespace AEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS8981 // Имя типа содержит только строчные символы ASCII. Такие имена могут резервироваться для языка.
public struct bit
#pragma warning restore CS8981 // Имя типа содержит только строчные символы ASCII. Такие имена могут резервироваться для языка.
{
    private sbyte _value;
    public const sbyte MinValue = -50;
    public const sbyte MaxValue = 50;

    public sbyte Value
    {
        get => _value;
        set => _value = value;
    }
}