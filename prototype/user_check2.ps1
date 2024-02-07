Add-Type -AssemblyName System.Windows.Forms

add-type -name user32 -namespace win32 -memberDefinition '[DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr hWnd, Int32 nCmdShow);'
$consoleHandle = (get-process -id $pid).mainWindowHandle

# load your form or whatever ...

# hide console
[win32.user32]::showWindow($consoleHandle, 0)
# show console
# [win32.user32]::showWindow($consoleHandle, 5)


# param([switch]$Elevated)

function Test-Admin {
    $currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
    $currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

if ((Test-Admin) -eq $false)  {
    if ($elevated) {
        # tried to elevate, did not work, aborting
    } else {
        Start-Process powershell.exe -Verb RunAs -ArgumentList ('-noprofile -noexit -file "{0}" -elevated' -f ($myinvocation.MyCommand.Definition))
    }
    exit
}

'running with full privileges'


function Get-ActiveUsers {
    # Get distinct user names from the list of processes and active connections
    $processUsers = Get-Process -IncludeUserName | Where-Object {
        $_.SessionId -ne 0 -and
        $_.UserName -notmatch "NT AUTHORITY|SYSTEM|Font Driver|Window Manager" 
    } | Select-Object -ExpandProperty UserName -Unique


    $rdpUsers = Get-WmiObject -Query "SELECT * FROM Win32_LogonSession WHERE LogonType = 10" | ForEach-Object {
        $_.GetRelated('Win32_LoggedOnUser').Antecedent.Name
    } | Sort-Object -Unique

    #$allUsers = $processUsers + $shareUsers + $rdpUsers | Sort-Object -Unique
    return $processUsers
}

function Get-ActiveShareUsers {
    $shareUsers = Get-WmiObject -Query "SELECT * FROM Win32_ServerConnection" | ForEach-Object {
        $_.UserName
    } | Sort-Object -Unique

    return $shareUsers
}


function Get-ActiveRdpUsers {
    $rdpUsers = Get-WmiObject -Query "SELECT * FROM Win32_LogonSession WHERE LogonType = 10" | ForEach-Object {
        $_.GetRelated('Win32_LoggedOnUser').Antecedent.Name
    } | Sort-Object -Unique

    return $rdpUsers
}

# Import Windows.Forms module
Import-Module -Name Windows.Forms

# Create a form
$form = New-Object System.Windows.Forms.Form
$form.Text = "Active Users"
$form.Size = New-Object System.Drawing.Size(600, 400)

# Create a DataGridView for users
$usersDataGridView = New-Object System.Windows.Forms.DataGridView
$usersDataGridView.Location = New-Object System.Drawing.Point(10,30)
$usersDataGridView.Size = New-Object System.Drawing.Size(560,120)
$form.Controls.Add($usersDataGridView)

# Add a column for Process Users
$usersDataGridView.Columns.Add("ProcessUsers", "Process Users")

# Add a column for Share Users


# Create a DataGridView for RDP users
$shareDataGridView = New-Object System.Windows.Forms.DataGridView
$shareDataGridView.Location = New-Object System.Drawing.Point(10,170)
$shareDataGridView.Size = New-Object System.Drawing.Size(560,120)
$form.Controls.Add($shareDataGridView)

$shareDataGridView.Columns.Add("ShareUsers", "Share Users")


# Create a DataGridView for RDP users
$rdpDataGridView = New-Object System.Windows.Forms.DataGridView
$rdpDataGridView.Location = New-Object System.Drawing.Point(10,310)
$rdpDataGridView.Size = New-Object System.Drawing.Size(560,120)
$form.Controls.Add($rdpDataGridView)

# Add a column for RDP Users
$rdpDataGridView.Columns.Add("RDPUsers", "RDP Users")

# Timer to refresh every 10 seconds
$timer = New-Object System.Windows.Forms.Timer
$timer.Interval = 10000  # 10 seconds
$timer.Add_Tick({
    $usersDataGridView.Rows.Clear()
    Get-ActiveUsers | ForEach-Object {
        $usersDataGridView.Rows.Add($_, "")
    }

    # $shareUsers = Get-ActiveShareUsers
    # $rdpUsers = Get-ActiveRdpUsers
    $rdpDataGridView.Rows.Clear()
    Get-ActiveRdpUsers | ForEach-Object {
        $rdpDataGridView.Rows.Add($_, "")
    }

    $shareDataGridView.Rows.Clear()
    Get-ActiveShareUsers | ForEach-Object {
        $shareDataGridView.Rows.Add($_, "")
    }

    # for ($i = 0; $i -lt [System.Math]::Max($shareUsers.Count, $rdpUsers.Count); $i++) {
    #     $usersDataGridView.Rows[$i].Cells["ShareUsers"].Value = $shareUsers[$i]
    #     $rdpDataGridView.Rows.Clear()
    #     $rdpDataGridView.Rows.Add($rdpUsers[$i])
    # }
})
$timer.Start()

# Show the form
$form.Add_Shown({
    $usersDataGridView.Rows.Clear()
    Get-ActiveUsers | ForEach-Object {
        $usersDataGridView.Rows.Add($_, "")
    }

    $shareDataGridView.Rows.Clear()
    Get-ActiveShareUsers | ForEach-Object {
        $shareDataGridView.Rows.Add($_, "")
    }
    $rdpDataGridView.Rows.Clear()
    Get-ActiveRdpUsers | ForEach-Object {
        $rdpDataGridView.Rows.Add($_, "")
    }

})

# Run the UI
[System.Windows.Forms.Application]::Run($form)
