# Pre-build event command line:

```cmd
REM deletes the compressed file that contains the Javascript library
del "$(SolutionDir)PdfLib\Javascript.zip
```

```cmd
REM copies the Javascript library source code (with the exception of the node_modules folder) to a temporary folder
robocopy "$(SolutionDir)PdfLib\Javascript" "$(SolutionDir)PdfLib\temp\Javascript" /mir /xd node_modules
```

```cmd
REM compress the Javascript library which is in the temp folder to the project root
powershell Compress-Archive "$(SolutionDir)PdfLib\temp\Javascript\*" "$(SolutionDir)PdfLib\Javascript.zip"
```

```cmd
REM apaga a pasta temporária
rmdir /Q /S "$(SolutionDir)PdfLib\temp"
```