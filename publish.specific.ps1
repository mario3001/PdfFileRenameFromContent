# Variables
Set-Location -Path $PSScriptRoot

$project = "PdfFileRenameFromContent.Specific"
$configuration = "Release"
$runtime = "win-x64" # Change as needed
$outputDir = "publish/specific"
$zipFile = "publish/$project.zip"

# Publish as self-contained
dotnet publish $project/$project.csproj -c $configuration -r $runtime --self-contained true -o $outputDir -p:PublishSingleFile=true

# Remove old zip if exists
if (Test-Path $zipFile) {
    Remove-Item $zipFile
}

# Zip the output
Compress-Archive -Path "$outputDir\*" -DestinationPath $zipFile

Write-Host "Publish and zip complete: $zipFile"