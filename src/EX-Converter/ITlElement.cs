namespace EX_Converter;

internal interface ITlElement
{
    string AttrName { get; }
    string? NodeOrder { get; }

    bool NameEquals(ITlElement other);
}