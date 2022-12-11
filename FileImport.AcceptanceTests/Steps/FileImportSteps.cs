using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace FileImport.AcceptanceTests.Steps;

[Binding]
public sealed class FileImportSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;

    public FileImportSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }


    [Given(@"the import from location is valid")]
    public void GivenTheImportFromLocationIsValid()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"the data is imported")]
    public void WhenTheDataIsImported()
    {
        ScenarioContext.StepIsPending();
    }
}