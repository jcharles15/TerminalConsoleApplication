Module ErrorHandlerModule
    Public Sub ErrorHandler(ByVal ErrorType As Integer, ByVal ErrorMessage As String, ByVal ErrorMessageTech As String)

        'Handles Errors and Provides Recovery Options

        'ErrorType of 001 = Controller Communcation Error
        '..        of 002 = Database Error
        '..        of 003 = Communication User/Pass Not Specified
        '..        of 004 = Controller Response Timeout
        '..        of 005 = Did Not Received OK From Login Request




        'BackgroundWorker1.CancelAsync()



        Errors = True

        If ErrorType = 1 Then
            ControllerCommError = True
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ErrorMessage)
            Console.WriteLine(ErrorMessageTech)
            Console.ResetColor()
            Console.WriteLine()
            Console.WriteLine("Application Halted - Returning To Main Menu. Please Wait...")

            Console.Beep()
            System.Threading.Thread.Sleep(5000)
            Console.Clear()
            MainMenu()


        ElseIf ErrorType = 2 Then
            DBError = True
            Console.WriteLine(ErrorMessage)


        ElseIf ErrorType = 3 Then
            Console.WriteLine(ErrorMessage)


        ElseIf ErrorType = 4 Then
            Console.WriteLine(ErrorMessage)


        ElseIf ErrorType = 5 Then
            Console.WriteLine(ErrorMessage)
        End If


        

       








        'Logging.WriteToLog(ErrorMessage + " : " + ErrorMessageTech)

    End Sub
End Module
