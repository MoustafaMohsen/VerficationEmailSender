#push nuget package script

function GetFilesByExtension {
    param 
    ( 
        [parameter()][string] $FolderPath, 
        [parameter()][string] $FileExtension 
    ) 
    function TestPath()  
    { 
        $FileExists = Test-Path $FolderPath 
        If ($FileExists -eq $True)  
        { 
            Return $true 
        } 
        Else  
        { 
            Return $false 
        } 
    }
    $Result = (TestPath($FolderPath)); 
        
    If ($Result) 
    { 
        $Dir = get-childitem $FolderPath #-recurse 
        $List = $Dir | where {$_.extension -eq $FileExtension}
        return $List;
    } 
    else 
    { 
        #"Folder path is incorrect." 
        return $null
    } 
        
}
    
function GetFileByExtension() {
    param (
        [parameter()][string] $Extension
    )
    
    $Path= Get-Location
    
    $filename = (GetFilesByExtension -FolderPath  $Path -FileExtension '.exe')[0]
    return $filename
}
    
    
Function ConfirmDialog() {
    #=======usage Example
    #$dialog = ConfirmDialog -message "confirme operation?" -description 'confirme operation description' `
    #-default 0  -yes "yes option description" -no "no option descriptio"

    # if($dialog -eq 1){
        #yes logic
    #}else{
        #no logic
    #}
    # === -default yes is 1, -default no is 0
    # ==  no parameter is Mandatory

    param(
        [Parameter(Mandatory=$false)][string]$message,
        [Parameter(Mandatory=$false)][string]$description,
        [Parameter(Mandatory=$false)][int]$default,
        [Parameter(Mandatory=$false)][string]$yes,
        [Parameter(Mandatory=$false)][string]$no
    )

    [int]$defaultChoice1 = 0
    $message_description1 = 'Continue'
    $caption1 = 'Proceed?'
    $yesmsg = 'Confirm'
    $nomsg = 'reject'
    if ($default -eq 0 -or 1) {
        [int]$defaultChoice1 = !$default
    } 
    if ($description) {
        [string]$message_description1 = $description
    }    
    if ($message) {
        [string]$caption1 = $message
    } 

    if ($yes) {
        [string]$yesmsg = $yes
    }       
    if ($no) {
        [string]$nomsg = $no
    }       

    $yes1 = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", $yesmsg
    $no1 = New-Object System.Management.Automation.Host.ChoiceDescription "&No", $nomsg
    $options1 = [System.Management.Automation.Host.ChoiceDescription[]]($yes1, $no1)
    $choiceRTN1 = $host.ui.PromptForChoice($caption1,$message_description1, $options1,$defaultChoice1)
    if ( $choiceRTN1 -ne 1 )
    {
        return 1;
    }
    else
    {
        return 0;
    }
    
}
function ReadInput() {
    # ==== usage example
    #$var = ReadInput -message "please enter value" -default "default value"
    # === No parameter is Mandatory
    param (
        [string]$message = "Values is",
        [string]$default = "",
        [boolean]$active = 1,
        [boolean]$view = 0

    )
    if ($active -eq $true) {  
        # if there is a default change massage style
        if (-not [string]::IsNullOrEmpty($default)) {
            [string]$message = "$message (default is [$default])"
        }
        # read host
        $result = Read-Host $message;
        # if input is empty read, reslut default value
        if ([string]::IsNullOrEmpty($result)) {
            $result = $default
        }
        # return value
        if ($view) {
            Write-Host "You Entered:$result"
        }
        return $result

    }else{
        if ($view) {
            Write-Host "You Entered:$default"
        }
        return $default
    }
}
Function Apause(){
    param (
        [string]$message="",
        [boolean]$active = $true
    )
    Write-Host $message
    if ($active) {
        Pause
    }
}

#create
$createnuspec= ConfirmDialog -message "Create .nuspec file (only for first time)" -default 1;

if($createnuspec){
    $defultproject = GetFileByExtension -Extension ".csproj"
    $projectname = ReadInput -message "enter .csproj file name here" -default $defultproject;

    Write-Host "Createing   .nuspec file..."
    ./nuget spec $projectname
}


#Pack
$ispack  =ConfirmDialog -message "Pack Package?" -default 1;
if ($ispack) {
    Write-Host "Packing..."
    ./nuget pack    
}

#Push
$ispush = ConfirmDialog -message "Push Nuget Package?" -default 1;
if ($ispush) {
    $apiKey = ReadInput -message "enter Nuget.org APIKEY";

    $defultnupkg = GetFileByExtension -Extension ".csproj"
    $nupkgFile = ReadInput -message ".nupkg file name" -default $defultnupkg;
    
    Write-Host "Pushing image..."
    if ( -not [string]::IsNullOrWhiteSpace($nupkgFile) ) {
        ./nuget push $nupkgFile $apiKey -Source https://api.nuget.org/v3/index.json
    }else{
        Write-Host "invalide .nupkg file name"
    }
}


Apause -message "Finished"