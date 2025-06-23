# Functionality
This app renames pdf files based on the content of the pdf file. The app uses a set of rules to determine the new file name.
For the date there the latest date in the document, but before today is used.

There are two executeables:
- PdfFileRenameFromContent.Executable - searches for all PDF files in a path from the settings
- PdfFileRenameFromContent.Specific - takes a filepath as command argument

## Rules
The rules file is a yaml file that contains the rules for renaming the pdf files. More information in the sample-rules.txt file.
All tagnames of all machtung rules are applied to the pdf file name. The order of the tagnames is defined through the ascending order of the rules by the rulename the rules file.
e.g. "Sender_UBS_Mieterkautionssparkonto:" will always before "Subject_Rechnung:", because the rulename "Sender_UBS_Mieterkautionssparkonto" is before "Subject_Rechnung" when ordering the rulenames ascending.

# PdfFileRenameFromContent.Specific

## Functionality
This app renames a specified filepath to rename a pdf file based on the content of the pdf file.

## App Settings
The app settings are stored in the `appsettings.json` file. The following settings are available:
- `RulesFilePath`: The paths to the rules files. The rules file is a yaml file that contains the rules for renaming the pdf files.

```json
{
  "RulesFilePath": [
    "X:\\Dokumente\\_Scan\\sample-rules.txt",
    "Z:\\Dokumente\\Subject-rules.txt"
  ]
}
```

# PdfFileRenameFromContent.Executable

## Functionality
This app renames all found pdf files in a configured folder based on the content of the pdf file. 

## App Settings
The app settings are stored in the `appsettings.json` file. The following settings are available:
- `RulesFilePath`: The paths to the rules files. The rules file is one or more yaml files that contains the rules for renaming the pdf files.
- `SourceFilePath`: The path to the folder containing the pdf files to be renamed.
- `DestinationFilePath`: The path to the folder where the renamed pdf files should be saved. It is allwed to use the same path specified under `SourceFilePath`, so the file only gets renamed.
```json
{
  "RulesFilePath": [
    "X:\\Dokumente\\_Scan\\sample-rules.txt",
    "Z:\\Dokumente\\Subject-rules.txt"
  ],
  "SourceFilePath": "Z:\\Dokumente\\_Scan\\Durchsuchbare-PDF",
  "DestinationFilePath": "Z:\\Dokumente\\_Scan\\Durchsuchbare-PDF\\renamed"
}
```

# License

This project is licensed under the MIT License. The MIT License allows for great freedom in software use, modification, and distribution.

# Inspiration
The YAML format used in this project was inspired by the format introduced by synOCR (https://github.com/geimist/synOCR). I found their structure and approach helpful and used parts of it as a basis when designing my own format.