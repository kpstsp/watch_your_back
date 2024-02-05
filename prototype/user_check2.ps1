Add-Type -AssemblyName System.Windows.Forms

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


function Show-UserList {
    # Create a form
    $form = New-Object System.Windows.Forms.Form
    $form.Text = "Active Users"
    $form.Size = New-Object System.Drawing.Size(300, 800)
    
# Create a list box for users
    $usersListBox = New-Object System.Windows.Forms.ListBox
    $usersListBox.Location = New-Object System.Drawing.Point(10,30)
    $usersListBox.Size = New-Object System.Drawing.Size(260,120)
    $form.Controls.Add($usersListBox)


    # Create a list box for connections
    $connectionsListBox = New-Object System.Windows.Forms.ListBox
    $connectionsListBox.Location = New-Object System.Drawing.Point(10,150)
    $connectionsListBox.Size = New-Object System.Drawing.Size(260,120)
    $connectionsListBox.Size = New-Object System.Drawing.Size(260,120)
    $form.Controls.Add($connectionsListBox)


    # Timer to refresh every 10 seconds
    $timer = New-Object System.Windows.Forms.Timer
    $timer.Interval = 10000  # 10 seconds
    $timer.Add_Tick({
        $usersListBox.Items.Clear()
        Get-ActiveUsers | ForEach-Object { $usersListBox.Items.Add($_) }
        $connectionsListBox.Items.Clear()
        Get-ActiveShareUsers | ForEach-Object { $connectionsListBox.Items.Add($_) }
    })
    $timer.Start()

    # Show the form
    $form.Add_Shown({ 
        $usersListBox.Items.Clear()
        Get-ActiveUsers | ForEach-Object { $usersListBox.Items.Add($_) }
        $connectionsListBox.Items.Clear()
        Get-ActiveShareUsers | ForEach-Object { $connectionsListBox.Items.Add($_) }
    })
    [System.Windows.Forms.Application]::Run($form)
}

# Run the UI
Show-UserList
