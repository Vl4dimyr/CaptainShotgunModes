# powershell -executionpolicy remotesigned -File "$(ProjectDir)dev\post_build.ps1" $(TargetDir) $(TargetFileName) $(ProjectDir) $(SolutionName)

$dllFilePath = $args[0] + $args[1]
$csFilePath = $args[2] + $args[3] + 'Plugin.cs'
$outFolder = $args[2] + 'out\'

$version = Get-Content "$($args[2])VERSION"

Copy-Item $dllFilePath -Destination "$env:APPDATA\r2modmanPlus-local\RiskOfRain2\profiles\$($args[3])\BepInEx\plugins\"

Remove-Item -Recurse -Force $outFolder

New-Item -Path $args[2] -Name 'out' -ItemType 'directory'

Copy-Item "$($args[2])images\icon.png" -Destination $outFolder
Copy-Item "$($args[2])README.md" -Destination $outFolder
Copy-Item "$($args[2])manifest.json" -Destination $outFolder
Copy-Item $dllFilePath -Destination $outFolder

(Get-Content "$($outFolder)manifest.json") -replace '{VERSION}', $version | Out-File -encoding UTF8 "$($outFolder)manifest.json"

Compress-Archive -Path "$($outFolder)*" -DestinationPath "$($outFolder)$($args[3])_$($version).zip"

(Get-Content $csFilePath) -replace $version, '{VERSION}' | Out-File -encoding UTF8 $csFilePath
