function Set-DotnetUserSecrets {
    param ($path, $lines)
    Push-Location
    cd $path
    dotnet user-secrets init
    dotnet user-secrets clear
    foreach ($line in $lines) {
        $name, $value = $line -split '='
        $value = $value -replace '"', ''
        $name = $name -replace '__', ':'
        if ($value -ne '') {
            dotnet user-secrets set $name $value | Out-Null
        }
    }
    Pop-Location
}

# Get all of the generated env variables, and store them as user secrets for the project
$lines = (azd env get-values) -split "`n"
Set-DotnetUserSecrets -path "./src/AIChatApp/" -lines $lines