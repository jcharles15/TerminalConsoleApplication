


Namespace CBJ_TERMINAL_SYSTEM

    Public Class TerminalAuthentication

        Public Shared PSO As New ProcessSignon
        Shared Acct = RetrieveSetting(Accountability)


        'Public Shared PSO As New ProcessSignon

        Shared SignonStep As Integer = 0
        Shared InitialSignOn As Boolean = True
        Shared CurrentPassword As Integer = 0


#Region "SignOn Class"

        Public Class SignON

            Public Shared S As SignON
            Public Shared OPRID As String
            Public Shared PASS As String

            Public Sub New()

                Auth()

            End Sub

            Private Shared Sub Auth()

                Try
                    Terminal.Display.DisplayData("****** CLOSED ******")
                    'Manages Logging On To The Terminal.

                    SignonStep = 1

                    While True

                        InSeq = TerminalSystem.Key_Data_Input()

                        If InSeq.SignOnOFF Then

                            Select Case SignonStep

                                Case 1 'Collect Operator ID

                                    DataInput1 = InSeq.Numerics

                                    OPRID = DataInput1

                                    Terminal.Display.DisplayData("ENTER PASSWORD")

                                    SignonStep = 2

                                Case Else

                                    Terminal.Display.DisplayData("KEY PASSWORD THEN", "ENTER  -CLEAR-")
                                    Terminal.IBMKEY.ClearToCont()
                                    Auth()
                                    Exit Select

                            End Select

                        ElseIf InSeq.Enter Then

                            Select Case SignonStep

                                Case 2 'Collect Operator Pin

                                    DataInput1 = InSeq.Numerics

                                    PASS = DataInput1

                                    SignonStep = 0

                                    Exit While

                                Case Else

                                    Terminal.Display.DisplayData("KEY OPERATOR # THEN", "SIGN-ON  -CLEAR-")
                                    Terminal.IBMKEY.ClearToCont()
                                    Auth()
                                    Exit Select

                            End Select

                        ElseIf InSeq.Override Then

                            Select Case SignonStep

                        'If Override Is Pressed On The Enter Pin Logon Step, Then Execute The Password Change Function
                        'Base Function Name: Initalize Sign On With New Password

                                Case 2

                                    DataInput1 = InSeq.Numerics
                                    CurrentPassword = Val(DataInput1)

                                    If PasswordChange(CurrentPassword) Then

                                        Exit While

                                    Else

                                        Auth()
                                        Exit Select

                                    End If

                                Case Else

                                    Terminal.Display.DisplayData("CHECK KEY SEQUENCE", "-CLEAR-")
                                    Terminal.IBMKEY.ClearToCont()
                                    Auth()
                                    Exit Select

                            End Select

                        End If

                    End While

                    'PROCESS SIGN ON-------------------------------------------------------------------------------------------------------------

                    If PSO.ProcessLogon Then

                        'Operator Authentication Passed

                        ' TerminalSystem.MainProgram()


                        '1 - Initialize Terminal Accounting

                        Accounting.InitializeAccounting()


                    Else

                        Auth()

                    End If

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End Sub


        End Class

#End Region

#Region "ProcessSignon Class"
        Public Class ProcessSignon

            Dim DbAdapter As MySqlDataAdapter

            Private Function Login(ByVal LogonData1 As String, ByVal LogonData2 As String)

                DbAdapter = New MySqlDataAdapter("SELECT * FROM employees WHERE EMPID='" + LogonData1 + "'", DBConnect)

                DBDataset = New DataSet()
                DbAdapter.FillSchema(DBDataset, SchemaType.Source, "employees")

                DBTable = DBDataset.Tables("employees")

                'Check If Operator Exists

                If DbAdapter.Fill(DBDataset, "employees") = 0 Then

                    LoginAttempts -= 1
                    InvalidUser = True



                    Return False

                Else

                    InvalidUser = False

                    'Operator Exists

                    For i As Integer = 0 To DBTable.Rows.Count - 1

                        Dim CurrentOperatorEMPID As String = Convert.ToInt32(DBTable.Rows(i)("EMPID"))
                        Dim CurrentOperatorPIN As String = DBTable.Rows(i)("PIN").ToString().Trim()
                        CurrentOperatorLoginName = DBTable.Rows(i)("LoginName").ToString().Trim()

                        ' MsgBox("Found Operator: " + CurrentOperatorLoginName + vbCrLf + "Operator EMPID: " + CurrentOperatorEMPID + vbCrLf + "Operator PIN: " + CurrentOperatorPIN)

                        If (CurrentOperatorEMPID = LogonData1 AndAlso CurrentOperatorPIN = LogonData2) Then

                            Return True

                        Else

                            CheckLoginAttempts()

                            Terminal.Display.DisplayData("LOGIN FAILED", "PRESS CLEAR")
                            Terminal.IBMKEY.ClearToCont()

                            Return False

                        End If

                    Next

                End If

                DBDataset.Dispose
                DBConnect.Close()

                Return False

            End Function

            Private Function CheckAccountStatus(ByVal OPRID As String) As Boolean

                Dim DBQuerySelect As String = Nothing
                DBQuerySelect = "SELECT * FROM employees WHERE EMPID = '" & OPRID & "'"
                CommandSelect = New MySqlCommand(DBQuerySelect, DBConnect)

                DBConnect.Open()
                DBReader = CommandSelect.ExecuteReader()

                While DBReader.Read

                    AccountStatus = DBReader("AccountStatus")
                    Dim RequirePinReset As Integer = DBReader("ReqPinReset")

                    Select Case AccountStatus

                        Case "Locked"

                            Terminal.Display.DisplayData("ACCOUNT LOCKED", "PLEASE SEE MANAGER")
                            Terminal.IBMKEY.ClearToCont()

                            DBReader.Close()
                            DBConnect.Close()

                            Return False

                        Case "Inactive"

                            Terminal.Display.DisplayData("ACCOUNT INACTIVE", "PLEASE SEE MANAGER")
                            Terminal.IBMKEY.ClearToCont()

                            DBReader.Close()
                            DBConnect.Close()

                            Return False

                        Case Else

                            Exit Select

                    End Select

                    Select Case RequirePinReset

                        Case 1

                            'Pin Reset Required
                            Return False

                        Case Else

                            Exit Select

                    End Select

                    'DBReader.Close()
                    'DBConnect.Close()

                    CloseDatabase(DBConnect, DBReader)

                    Return True

                End While




                Return False

            End Function

            Private Sub CheckLoginAttempts()

                DBQueryUpdate = "UPDATE employees SET AccountStatus = 'Locked' WHERE EMPID = '" & SignON.OPRID & "'"
                CommandUpdate = New MySqlCommand(DBQueryUpdate, DBConnect)


                Select Case My.Settings.LoginAttempts

                    Case 0


                    Case Else

                        LoginAttempts += 1

                        If InvalidUser = True Then

                            Terminal.Display.DisplayData("INVALID OPERATOR ID")
                            Terminal.IBMKEY.ClearToCont()

                            If LoginAttempts >= 6 Then

                                MsgBox("Logon Violation: Too Many Failed Attempts" + vbCrLf + "Application Must Shut Down")
                                Exit Sub

                            End If

                        Else

                            InvalidUser = False

                            DBConnect.Open()

                            If LoginAttempts = My.Settings.LoginAttempts Then
                                Try


                                    Terminal.Display.DisplayData("ACCOUNT LOCKED", "TOO MANY ATTEMPTS")
                                    Terminal.IBMKEY.ClearToCont()

                                Catch ex As Exception
                                End Try

                                CommandUpdate.CommandText = DBQueryUpdate
                                CommandUpdate.ExecuteNonQuery()

                            End If

                        End If

                        DBConnect.Close()

                End Select

            End Sub

            Public Function ProcessLogon() As Boolean

                'Perform Logon And Check If It Was Successful
                If Login(SignON.OPRID, MD5Hash(SignON.PASS)) Then

                    If CheckAccountStatus(SignON.OPRID) Then

                        Return True

                    Else

                        Return False

                    End If


                End If

                Return False

            End Function

        End Class

#End Region

#Region "Procedure Permission Check"

        Public Shared Function CheckPermissions(ByVal Procedure As String) As Boolean

            'Get The Operator Role / Clearance Level

            Dim DBQueryGetRole As String = "SELECT Role FROM employees WHERE LoginName = '" & CurrentOperatorLoginName & "'"
            Dim CommandGetRole As New MySqlCommand(DBQueryGetRole, DBConnect)




            Try

                DBConnect.Open()
                Dim DBReader As MySqlDataReader
                DBReader = CommandGetRole.ExecuteReader()

                While DBReader.Read

                    Role = DBReader("Role").ToString()


                End While

                DBReader.Close()
                DBConnect.Close()

            Catch ex As Exception

            End Try

            'Check Against The Permission For The Role

            Dim PermissionToGrant As String = Procedure

            Dim DBQueryGetRolePermission As String = "SELECT Permission FROM rolepermissions WHERE Role = '" & Role & "' AND Permission= '" & PermissionToGrant & "'"
            Dim CommandGetRolePermission As New MySqlCommand(DBQueryGetRolePermission, DBConnect)

            Try

                DBConnect.Open()
                Dim DBReader As MySqlDataReader
                DBReader = CommandGetRolePermission.ExecuteReader()

                While DBReader.Read

                    Dim Permission As String = ""

                    Permission = DBReader("Permission").ToString()

                    If Permission = PermissionToGrant Then

                        Terminal.Display.DisplayData("ACCESS GRANTED")

                        Terminal.IBMKEY.ClearToCont()

                        DBReader.Close()
                        DBConnect.Close()

                        Return True

                    Else

                        Terminal.Display.DisplayData("ACCESS DENIED")

                        Terminal.IBMKEY.ClearToCont()

                        DBReader.Close()
                        DBConnect.Close()

                        Return False

                    End If

                End While

                DBReader.Close()
                DBConnect.Close()

            Catch ex As Exception

            End Try

            'Record Not Listed

            Try

                Dim DbAdapter As New MySqlDataAdapter("SELECT * FROM rolepermissions WHERE Role = '" & Role & "' AND Permission = '" & PermissionToGrant & "'", DBConnect)
                Dim DbDataSet As New DataSet
                DbAdapter.Fill(DbDataSet, "rolepermissions")
                Dim DbTable As DataTable = DbDataSet.Tables("rolepermissions")

                If DbAdapter.Fill(DbDataSet, "rolepermissions") = 0 Then

                    Terminal.Display.DisplayData("ACCESS DENIED")

                    Terminal.IBMKEY.ClearToCont()

                    DBConnect.Close()

                    Return False

                End If

            Catch ex As Exception

            End Try

            Return False

        End Function

#End Region

#Region "Password Change Function"

        Shared NewPWStep As Integer = 0
        Shared CurrentPassCollected As Integer = 0
        Shared NewPassword As Integer = 0

        Public Shared Function PasswordChange(CurrentPassword As Integer) As Boolean

            CurrentPassCollected = CurrentPassword

            NewPWStep = 1

            Terminal.Display.DisplayData("ENTER NEW PASSWORD")

            While True

                InSeq = TerminalSystem.Key_Data_Input()

                If InSeq.SignOnOFF Then

                    Select Case NewPWStep

                        'This Case Collects The New Password

                        Case 1

                            'If Step = 1

                            DataInput1 = InSeq.Numerics

                            NewPassword = Val(DataInput1)

                            Terminal.Display.DisplayData("VERIFY NEW PASSWORD", "BY RE-ENTERING IT")

                            NewPWStep = 2

                        Case Else

                            Terminal.Display.DisplayData("CHECK KEY SEQUENCE", "             -CLEAR-")
                            Terminal.IBMKEY.ClearToCont()
                            NewPWStep = 1
                            Terminal.Display.DisplayData("ENTER NEW PASSWORD")

                    End Select

                ElseIf InSeq.Enter Then

                    Select Case NewPWStep

                        'This Case Verifies The New Password

                        Case 2

                            'Verify New Pass Step

                            DataInput1 = InSeq.Numerics
                            Dim VerificationPass As Integer = Val(DataInput1)

                            If NewPassword = VerificationPass Then

                                'TODO DATABASE UPDATE
                                Dim SQL As String = "UPDATE employees SET PIN=@newpassword WHERE EMPID=@id AND PIN=@currentpassword"
                                Dim Command As MySqlCommand = New MySqlCommand

                                DBConnect.Open()
                                With Command

                                    .Connection = DBConnect
                                    .CommandText = SQL
                                    .Parameters.AddWithValue("@newpassword", MD5Hash(NewPassword))
                                    .Parameters.AddWithValue("@id", SignON.OPRID)
                                    .Parameters.AddWithValue("@currentpassword", MD5Hash(CurrentPassCollected))

                                End With

                                If Command.ExecuteNonQuery() > 0 Then

                                        SignON.PASS = NewPassword.ToString()
                                        Terminal.Display.DisplayData("B070 NEW PASSWORD", "ACCEPTED     -CLEAR-")
                                    Terminal.IBMKEY.ClearToCont()



                                Else

                                        Terminal.Display.DisplayData("PASS CHANGE FAILED", "-CLEAR-")
                                        Terminal.IBMKEY.ClearToCont()
                                    DBConnect.Close()
                                    Return False

                                    End If


                                DBConnect.Close()

                                Return True

                            Else

                                Terminal.Display.DisplayData("PASSWORDS DO NOT", "MATCH        -CLEAR-")
                                Terminal.IBMKEY.ClearToCont()
                                NewPWStep = 1
                                Terminal.Display.DisplayData("ENTER NEW PASSWORD")

                            End If

                        Case Else

                            Terminal.Display.DisplayData("CHECK KEY SEQUENCE", "             -CLEAR-")
                            Terminal.IBMKEY.ClearToCont()
                            NewPWStep = 1
                            Terminal.Display.DisplayData("ENTER NEW PASSWORD")

                    End Select

                End If

            End While




            Return False



        End Function

#End Region

        Public Shared Sub SignOFF()

            'Complete Sign OFF

            TerminalAuthentication.SignON.S = New SignON


        End Sub


        Private Enum AccountabilityType

            Oper = 0
            Term = 1

        End Enum

    End Class


End Namespace

