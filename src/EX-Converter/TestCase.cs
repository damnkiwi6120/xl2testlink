namespace EX_Converter;

internal sealed class TestCase : ITlElement
{
    //Should refer to TL.
    private const int ExecutionTypeDefault = 1;
    //Should refer to TL.
    private const int ImportanceDefault = 2;

    public string AttrInternalId { get; } = string.Empty;
    //Implement ITlElement.
    public string AttrName { get; }
    //Implement ITlElement.
    public string NodeOrder { get; } = string.Empty;
    public string ExternalId { get; } = string.Empty;
    public string Version { get; } = string.Empty;
    public string? Summary { get; }
    public string? Preconditions { get; }
    public int ExecutionType { get; }
    public int Importance { get; }
    public List<TestStep> Steps { get; } = new();

    public TestCase(string name, string? summary, string? preconditions, int importance)
    {
        AttrName = name;
        Summary = summary;
        Preconditions = preconditions;
        ExecutionType = ExecutionTypeDefault;
        Importance = importance;
    }

    public void AddTestStep(TestStep newStep)
    {
        Steps.Add(newStep);
    }

    public bool NameEquals(ITlElement other)
    {
        return other is TestCase && AttrName == other.AttrName;
    }
}