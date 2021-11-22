$connectTestResult = Test-NetConnection -ComputerName apicategorizer.file.core.windows.net -Port 445
if ($connectTestResult.TcpTestSucceeded) {
    # Save the password so the drive will persist on reboot
    cmd.exe /C "cmdkey /add:`"apicategorizer.file.core.windows.net`" /user:`"localhost\apicategorizer`" /pass:`"ztmcpSoVYexv45VMja0eT95fiZm7f5tPkMot5IDvmGUAhOFcl5U2syNaTCTF2uM9KBoIb88T0Nq4lwnpqm9G2Q==`""
    # Mount the drive
    New-PSDrive -Name Z -PSProvider FileSystem -Root "\\apicategorizer.file.core.windows.net\image-histogram"
} else {
    Write-Error -Message "Unable to reach the Azure storage account via port 445. Check to make sure your organization or ISP is not blocking port 445, or use Azure P2S VPN, Azure S2S VPN, or Express Route to tunnel SMB traffic over a different port."
}