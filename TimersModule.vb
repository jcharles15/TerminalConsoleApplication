


Module TimersModule
    '* Timers Used By The Application

    Public WithEvents TimerFindControllerTimeout As New Timer
    Public WithEvents TimerWaitForControllerResponseCountdown As New Timer

#Region "TimerFindControllerTimeout"

    Public Sub StartTimerFindControllerTimeout()

        TimerFindControllerTimeout.Interval = 1000
        TimerFindControllerTimeout.Start()


    End Sub

    Private Sub TimerFindControllerTimeout_Elapsed(sender As Object, e As ElapsedEventArgs) Handles TimerFindControllerTimeout.Elapsed


        If FoundController = True Then
            TimerFindControllerTimeout.Stop()

        Else

            If FindControllerTimeoutTime > 0 Then

                FindControllerTimeoutTime -= 1

                Console.ForegroundColor = ConsoleColor.Green
                Console.Write("|")
                Console.ResetColor()

            Else
                Console.WriteLine()
                'Timer Is Done

                TimerFindControllerTimeout.Stop()

                If FoundController = False Then
                    ' Controller Has Not Been Found or No Reply Received

                    'Stop Broadcasting
                    ' BroadcastThread.Suspend() 'Obsolete

                    BroadcastThread.Abort()




                    'Stop Listening For Replies
                    ' UDPListenerThread.Suspend() 'Obsolete

                    UDPListenerThread.Abort()

                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("Controller Not Found")
                    Console.ResetColor()


                    'Retry Connection
                    RetryControllerConnection()


                End If

            End If

        End If

    End Sub

#End Region

#Region "TimerWaitForControllerResponseCountdown"

    Public Sub StartTimerWaitForControllerResponseCountdown()

        TimerWaitForControllerResponseCountdown.Interval = 1000
        TimerWaitForControllerResponseCountdown.Start()

    End Sub

    Public Sub StopTimerWaitForControllerResponseCountdown()

        TimerWaitForControllerResponseCountdown.Stop()

    End Sub

    Private Sub TimerWaitForControllerResponseCountdown_Elapsed(sender As Object, e As ElapsedEventArgs) Handles TimerWaitForControllerResponseCountdown.Elapsed

        If ServerResponseTimeoutTime > 0 Then

            ServerResponseTimeoutTime -= 1

            Console.WriteLine("Waiting For Response From Controller" + vbCrLf + "Timeout In " + ServerResponseTimeoutTime.ToString + " Seconds")

        Else
            TimerWaitForControllerResponseCountdown.Stop()
            ' "Timeout Has Occurred"

            If LastDataSent.StartsWith("LOGIN|") Then ' If the last message was login during timeout
                ErrorHandler(4, "Controller Auth Request Timeout", Nothing)
                ErrorHandler(4, "Controller Did Not Respond To Login Request", Nothing)

                Terminal.Display.DisplayData("W004 CONTROLLER", "     DOES NOT RESPOND")

                Threading.Thread.Sleep(5000)

                RetryAuthRequest()

            End If

        End If

    End Sub

#End Region

End Module
