using System.Xml;

namespace EX_Converter;

internal class XmlWriter
{
    #region Fields and const values.
    private readonly string newFilePath;
    private readonly bool writingTestSuite;

    private readonly XmlDocument document;
    private readonly XmlElement root;

    private const string NODENAME_TEST_SUITE = "testsuite";
    private const string NODENAME_DETAILS = "details";
    private const string NODENAME_TEST_CASES = "testcases";
    private const string NODENAME_TEST_CASE = "testcase";
    private const string NODENAME_NAME = "name";
    private const string NODENAME_INTERNALID = "internalid";

    private const string NODENAME_NODE_ORDER = "node_order";
    private const string NODENAME_EXTERNALID = "externalid";
    private const string NODENAME_VERSION = "version";
    private const string NODENAME_SUMMARY = "summary";
    private const string NODENAME_PRECONDITIONS = "preconditions";
    private const string NODENAME_EXECUTION_TYPE = "execution_type";
    private const string NODENAME_IMPORTANCE = "importance";

    private const string NODENAME_STEPS = "steps";
    private const string NODENAME_STEP = "step";
    private const string NODENAME_STEP_NUMBER = "step_number";
    private const string NODENAME_ACTIONS = "actions";
    private const string NODENAME_EXPECTED_RESULTS = "expectedresults";
    #endregion

    #region Constructor
    public XmlWriter(string newPath, bool isWritingSuite)
    {
        newFilePath = newPath;
        writingTestSuite = isWritingSuite;

        //XML declaration.
        document = new XmlDocument();
        document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));

        //Set root node.
        if (writingTestSuite)
        {
            root = document.CreateElement(NODENAME_TEST_SUITE);
            root.SetAttribute(NODENAME_NAME, string.Empty);
            AppendCDataElement(root, NODENAME_NODE_ORDER, string.Empty);
            AppendCDataElement(root, NODENAME_DETAILS, string.Empty);
        }
        else
        {
            root = document.CreateElement(NODENAME_TEST_CASES);
        }
        document.AppendChild(root);
    }
    #endregion

    #region The Writing method and its utility methods.
    //Main method for writing to XML file.
    public void Write(TestSuite suite)
    {
        if (writingTestSuite)
        {
            WriteToTestSuite(suite);
        }
        else
        {
            WriteToTestCases(suite);
        }
        //Finally write to file.
        document.Save(newFilePath);
    }
    private void WriteToTestCases(TestSuite suite)
    {
        foreach (ITlElement elem in suite.ChildrenElements)
        {
            if (elem is TestCase testCase)
            {
                AppendTestCase(root, testCase);
            }
            else
            {
                throw new ArgumentException("Error! XML element:\n"
                    + $"Name: {elem.AttrName}\n"
                    + $"(Node Order: {elem.NodeOrder})\n"
                    + "is not a \"testcase\"");
            }
        }
    }
    private void WriteToTestSuite(TestSuite suite)
    {
        AppendTestSuite(root, suite);
    }

    #region Methods for appending elements.
    private void AppendCDataElement(XmlNode parentNode, string name, string? text)
    {
        XmlElement elem = document.CreateElement(name);
        parentNode.AppendChild(elem);

        XmlNode cData = document.CreateNode(XmlNodeType.CDATA, string.Empty, string.Empty);
        elem.AppendChild(cData);

        //Null text is stored as an empty node.
        cData.InnerText = text ?? string.Empty;
    }
    private void AppendSingleStep(XmlNode parentSteps, TestStep step)
    {
        XmlElement newStep = document.CreateElement(NODENAME_STEP);
        parentSteps.AppendChild(newStep);

        AppendCDataElement(newStep, NODENAME_STEP_NUMBER, step.StepNumber.ToString());
        AppendCDataElement(newStep, NODENAME_ACTIONS, step.Actions);
        AppendCDataElement(newStep, NODENAME_EXPECTED_RESULTS, step.ExpectedResults);
        AppendCDataElement(newStep, NODENAME_EXECUTION_TYPE, step.ExecutionType.ToString());
    }
    private void AppendSteps(XmlNode parentTestCase, List<TestStep> steps)
    {
        XmlElement newSteps = document.CreateElement(NODENAME_STEPS);
        parentTestCase.AppendChild(newSteps);

        foreach (TestStep step in steps)
        {
            AppendSingleStep(newSteps, step);
        }
    }
    private void AppendCaseInfo(XmlNode parentTestCase, TestCase testCase)
    {
        AppendCDataElement(parentTestCase, NODENAME_NODE_ORDER, testCase.NodeOrder);
        AppendCDataElement(parentTestCase, NODENAME_EXTERNALID, testCase.ExternalId);
        AppendCDataElement(parentTestCase, NODENAME_VERSION, testCase.Version);
        AppendCDataElement(parentTestCase, NODENAME_SUMMARY, testCase.Summary);
        AppendCDataElement(parentTestCase, NODENAME_PRECONDITIONS, testCase.Preconditions);
        AppendCDataElement(parentTestCase, NODENAME_EXECUTION_TYPE,
                        testCase.ExecutionType.ToString());
        AppendCDataElement(parentTestCase, NODENAME_IMPORTANCE,
                        testCase.Importance.ToString());
        //All steps.
        AppendSteps(parentTestCase, testCase.Steps);
    }
    private void AppendTestCase(XmlNode parentNode, TestCase testCase)
    {
        XmlElement newTestCase = document.CreateElement(NODENAME_TEST_CASE);
        newTestCase.SetAttribute(NODENAME_INTERNALID, testCase.AttrInternalId);
        newTestCase.SetAttribute(NODENAME_NAME, testCase.AttrName);
        AppendCaseInfo(newTestCase, testCase);

        parentNode.AppendChild(newTestCase);
    }
    private void AppendSuiteInfo(XmlNode parentTestSuite, TestSuite testSuite)
    {
        AppendCDataElement(parentTestSuite, NODENAME_NODE_ORDER, string.Empty);
        AppendCDataElement(parentTestSuite, NODENAME_DETAILS, testSuite.Details);
    }
    private void AppendTestSuite(XmlNode parentNode, TestSuite testSuite)
    {
        XmlElement newTestSuite = document.CreateElement(NODENAME_TEST_SUITE);
        newTestSuite.SetAttribute(NODENAME_NAME, testSuite.AttrName);
        AppendSuiteInfo(newTestSuite, testSuite);

        foreach (ITlElement elem in testSuite.ChildrenElements)
        {
            if (elem is TestSuite childSuite)
            {
                //Recursively append all children of this test suite.
                AppendTestSuite(newTestSuite, childSuite);
            }
            else
            {
                AppendTestCase(newTestSuite, (TestCase)elem);
            }
        }

        //Determine if parent node is root. If is root, move newTestSuite nodes to root.
        if (parentNode != root)
        {
            parentNode.AppendChild(newTestSuite);
        }
        else
        {
            int origChildCount = newTestSuite.ChildNodes.Count;
            for (int nodeCounter = 0; nodeCounter < origChildCount; nodeCounter++)
            {
                if (nodeCounter < 2)
                {
                    newTestSuite.RemoveChild(newTestSuite.FirstChild!);
                }
                else
                {
                    root.AppendChild(newTestSuite.FirstChild!);
                }
            }
        }
    }
    #endregion
    #endregion
}