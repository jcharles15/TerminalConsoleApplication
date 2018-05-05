Module ModuleErrorHandler
    Dim FunctionsToRestoreList As New List(Of String)

    Public Sub CheckFunctions()
        If LastDataSent = "RSVTRAN" Then
            ' MsgBox("ERROR Getting The Transaction Number. Once Connected To Store Control, application will attempt to get the transaction number again")
            ' MsgBox("Adding this error to the list to rerun it upon connect")
            FunctionsToRestoreList.Add("RSVTRAN")
        End If
    End Sub

End Module
