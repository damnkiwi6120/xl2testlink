namespace EX_Converter;

internal sealed class TestSuite : ITlElement
{
    //Implement ITlElement.
    public string AttrName { get; }
    //Implement ITlElement.
    public string? NodeOrder { get; }
    public string? Details { get; }
    public List<ITlElement> ChildrenElements { get; } = new();

    public TestSuite(string suiteName, string? details)
    {
        AttrName = suiteName;
        Details = details;
    }

    public void AddChild(ITlElement tlElement)
    {
        ChildrenElements.Add(tlElement);
    }

    public bool NameEquals(ITlElement other)
    {
        return other is TestSuite && AttrName == other.AttrName;
    }

    public TestSuite? FindSuite(string suiteName)
    {
        foreach (ITlElement elem in ChildrenElements)
        {
            if (elem is TestSuite suite && suite.AttrName == suiteName)
            {
                return suite;
            }
        }
        return null;
    }
}