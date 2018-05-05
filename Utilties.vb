Module Utilties

    'Retrieve Application Setting Values
    Public Function RetrieveSetting(ByVal SettingName As String)

        Try

            Return My.Settings.Item(SettingName)

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try

        Return My.Settings.Item(SettingName)

    End Function

    Public Sub CloseDatabase(ByRef Connection As MySqlConnection, Optional ByRef Reader As MySqlDataReader = Nothing)

        Connection.Close()

        If Reader IsNot Nothing Then

            Reader.Close()

        End If

    End Sub

    Public Function CompareDate(ByVal Date1 As Date, ByVal Date2 As Date) As Integer

        Dim result As Integer = DateTime.Compare(Date1, Date2)

        If result < 0 Then
            result = 1 ' Earlier
        ElseIf (result = 0) Then
            result = 2 ' Same
        Else
            result = 3 ' Later
        End If

        Return result

    End Function

    Public Function MD5Hash(ByVal Data As String)

        'Converts Password From MD5
        Dim md5 As MD5 = New MD5CryptoServiceProvider()
        Dim result As Byte()
        result = md5.ComputeHash(Encoding.ASCII.GetBytes(Data))
        Dim strBuilder As New StringBuilder()

        For i As Integer = 0 To result.Length - 1
            strBuilder.Append(result(i).ToString("x2"))
        Next
        Return strBuilder.ToString()

    End Function


End Module
