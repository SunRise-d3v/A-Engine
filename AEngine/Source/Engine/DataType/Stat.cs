namespace AEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS8981 // Имя типа содержит только строчные символы ASCII. Такие имена могут резервироваться для языка.
public struct stat
#pragma warning restore CS8981 // Имя типа содержит только строчные символы ASCII. Такие имена могут резервироваться для языка.
{
    private short _value;
    public const short MinValue = -500;
    public const short MaxValue = 499;

    public short Value
    {
        get => _value;
        set => _value = value;
    }
}