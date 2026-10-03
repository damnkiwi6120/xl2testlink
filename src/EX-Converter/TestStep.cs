namespace EX_Converter;

internal sealed class TestStep
{
    public int StepNumber { get; }
    public string? Actions { get; }
    public string? ExpectedResults { get; }
    public int ExecutionType { get; }

    public TestStep(int stepNumber, string? stepActions, string? stepExpected, int stepExeType = 1)
    {
        StepNumber = stepNumber;
        Actions = stepActions;
        ExpectedResults = stepExpected;
        ExecutionType = stepExeType;
    }
}