# EX-Converter
A tool that generates XML file for TestLink importing from an excel file. 

## Description
EX-Converter is a lightweight Windows application that could convert your test specifications from Excel sheets to TestLink XML formats; then you can use the
XML files for TestLink importing.

It aims to minimize your effort on modifying existing xls/xlsx format test cases and free you from directly handling XML elements.

Download source code, sample templates, a quick tutorial and the application itself from "Files" as you wish. Just try it and see if it helps with your work!

NOTE: This tool needs the following prerequisites: .NET 10 Desktop Runtime (or upper) and Microsoft Excel installed at runtime
(the Excel sheet is read through COM interop with Excel).

<!-- Future consideration: replace COM interop with a pure-managed reader such as NPOI
     (https://npoi.github.io/) so the tool can run on machines without Excel installed
     and in headless/CI scenarios. The parsing logic in ExcelReader.cs would be the only
     component affected. -->

## Features
  *  Generate XML in TestLink's Test Cases format
  *  Generate XML in TestLink's Test Suite format; support up to 2 level depth of sub suite hierarchy
  *  Support multiple steps in one test case
  *  Supported customizable fields: Name/Importance/Summary/Preconditions/Actions/Expected Results (other non-customizable fields will have default values during importing)
  *  Print out logs with possible warnings/errors; help to identify invalid formats
  *  Completely or partially convert your test specification
