Feature: FileImportApi
	Simple File Importer

@mytag
Scenario: Add two numbers
	Given the import from location is valid
	When the data is imported
	Then a new file with is created in output location 