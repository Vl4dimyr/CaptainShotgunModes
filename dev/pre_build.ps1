# powershell -executionpolicy remotesigned -File "$(ProjectDir)dev\pre_build.ps1" $(ProjectDir) $(SolutionName)

$csFilePath = $args[0] + $args[1] + 'Plugin.cs'

(Get-Content $csFilePath) -replace '{VERSION}', (Get-Content "$($args[0])VERSION") | Out-File -encoding UTF8 $csFilePath
