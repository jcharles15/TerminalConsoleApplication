Module AppFunctionsModule

    Public Sub RetryControllerConnection()

        'Terminal Unable To Make Connection To Controller

        If Attempt = MaxAttempts Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Max Retry Attempts Have Been Reached.")
            Console.ForegroundColor = ConsoleColor.Blue
            Console.WriteLine("Initialized Pos Keyboard To Allow Input")
            Console.ResetColor()
            Console.WriteLine()
            Attempt = 0

            PreloadFailure()

        Else

            Attempt += 1
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("RETRYING CONTROLLER CONNECTION. RETRY ATTEMPT " + Attempt.ToString + " OF " + MaxAttempts.ToString)
            Console.ResetColor()

            ApplicationInitialization()

        End If

    End Sub

    Public Sub RetryAuthRequest()

        'Terminal Authentication Request Failed or No Response

        ServerResponseTimeoutTime = 5

        If Attempt = MaxAttempts Then

            Attempt = 0

            PreloadFailure()

        Else

            Attempt += 1

            WriteLine("retrying auth Request", True, ConsoleColor.Yellow)

            SendData("LOGIN|" + My.Settings.TerminalID.ToString + "|" + CommunicationsUsername + "|" + CommunicationsPassword)

            StartTimerWaitForControllerResponseCountdown()
            ServerResponseThread.Start()

        End If

    End Sub

    Public Function RecordControllerInfo(ByVal ControllerIP As String, ByVal Port As Integer) As Boolean

        Try

            My.Settings.ControllerIP = ControllerIP
            My.Settings.ControllerPort = Port
            My.Settings.Save()

            Return True

        Catch ex As Exception

            MsgBox(ex.Message.ToString)
            Return False

        End Try

    End Function

    Public Sub DisplayText(ByRef Display As LineDisplay, ByVal Data As String, Optional ClearText As Boolean = True)

        Try


            If ClearText = True Then
                Display.ClearText()
            End If

            Display.DisplayText(Data)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub PreloadFailure()

        Terminal.Display.DisplayData("W065 PRELOAD FAILURE", "     PRESS ENTER")

        Threading.Thread.Sleep(5000)

        Console.ReadKey()

        Terminal.Display.DisplayData("IPL")

        Client.Close()
        Client = New TcpClient

        ProcessMainInput()





    End Sub



End Module
