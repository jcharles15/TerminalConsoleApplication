
Module Module1

    Dim Thread As Threading.Thread

    Public POSDTYPE As String
    Public POSSONAME As String
    Public Enabled As String = "NO"
    Public Defaults As String = "NO"
    Public DeviceType1 As String
    Public SecurityPASS As Boolean = False
    Public ListOptionsfp As New List(Of String)
    Public printedlist As String
    Public AccountNo As String

#Region "Main"

    Public Sub Main()

        '** This Is The Beginning Of The Whole Application

        'StartReceiving()

        Thread = New Threading.Thread(AddressOf DoMain)
        Thread.Start() '







    End Sub

    Public Sub DoMain()

        Console.Clear()
        Console.CursorVisible = False
        Console.SetBufferSize(80, 30)
        TerminalWindow.DrawHeader(False)

        'Terminal Startup---------------------------------------------------------

        ProcessMainInput()

    End Sub

    Dim Dataset As String

    Public Sub ProcessMainInput()

        MainMenu()

        'Dim MenuOption As Integer = Val(Console.ReadLine())

        Dim MenuOption As Integer = Val(Console.ReadLine())

        Select Case MenuOption

            Case 1

                WriteLine("Program Loading Initalized", False)

                ' ApplicationInitialization()

                TerminalSystem.RunPosProgram()



                Exit Select

            Case 2

                ' Try



                Backend.PosDeviceConfig()

                Exit Select

                'Catch ex As Exception

               ' End Try

            Case 3

                '...

                Exit Select

            Case 4

                ControllerConnectionSettings()

                Exit Select

            Case 5

                TerminalConfig()

                Exit Select

            Case 6

                Console.Clear()
                Console.WriteLine("SHUTDOWN/RESTART TERMINAL")
                Console.WriteLine()
                Console.WriteLine("THIS FEATURE IS CURRENTLY NOT AVAILABLE")
                Threading.Thread.Sleep(5000)
                Console.Clear()
                ProcessMainInput()

                Exit Select

            Case 7

                Console.Clear()
                Console.WriteLine("STORE OPENING AND CLOSING")
                Console.WriteLine()

                StoreOpenClose()

            Case 8

                Console.Clear()
                Console.WriteLine("EXITING . . . GOODBYE")

                Try
                    Terminal.Display.LineDisplay1.DeviceEnabled = False
                    Terminal.Display.LineDisplay1.Release()
                    Terminal.Display.LineDisplay1.Close()

                Catch ex As Exception

                End Try

                Threading.Thread.Sleep(2000)

                Environment.Exit(0)   'Exits The Application


            Case Else

                InvalidOption()

                ProcessMainInput()

                Exit Select

        End Select


    End Sub

#End Region

#Region "Menu Functions"

    Public Sub MainMenu()

        Console.Clear()
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("           AAAAA" + "           SSSSSSS     //" + "        444")
        Console.WriteLine("         AAA AAA" + "         SSS    SSS   //" + "        444 ")
        Console.WriteLine("        AAA  AAA" + "         SSS         //" + "        444")
        Console.WriteLine("       AAA   AAA" + "          SSS       //" + "        444")
        Console.WriteLine("      AAA    AAA" + "           SSS     //" + "        444  444")
        Console.WriteLine("     AAAAAAAAAAA" + "            SSS   //" + "        444444444")
        Console.WriteLine("    AAA      AAA" + "      SSS   SSS  //" + "              ...")
        Console.WriteLine("   AAA       AAA" + "       SSSSSSS  // " + "             4444")
        Console.ResetColor()

        Console.WriteLine()
        Console.WriteLine(" CBJ TERMINAL SYSTEM VERSION 1.00" + vbCrLf + " COPYRIGHT CBJ NETWORKS 2018")
        Console.WriteLine()
        Console.WriteLine("-----------------------------------------------")
        Console.WriteLine(" 1 - RUN POS APPLICATION" + vbCrLf + " 2 - POS DEVICE CONFIGURATION" + vbCrLf + " 3 - DATABASE CONFIG" + vbCrLf + " 4 - POS CONTROLLER CONFIG" + vbCrLf + " 5 - TERMINAL CONFIG" + vbCrLf + " 6 - SHUTDOWN/RESTART" + vbCrLf + " 7 - OPEN/CLOSE" + vbCrLf + " 8 - EXIT")
        Console.WriteLine("-----------------------------------------------")
        Console.WriteLine()
        Console.Write(" SELECT MENU OPTION: > ")

    End Sub

    Public Sub InvalidOption()

        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Red
        Console.Write("INVALID MENU OPTION. PLEASE WAIT A MOMENT...")
        Console.ResetColor()
        Console.Beep()
        Threading.Thread.Sleep(2000)

    End Sub

#End Region

#Region "Controller Connection Settings Menu"

    Private Sub ControllerConnectionSettings()

        Console.Clear()
        Console.WriteLine("POS CONTROLLER CONNECTION SETTINGS")
        Console.WriteLine()
        Console.WriteLine("1 - CONFIGURE CONTROLLER HOST/IP/PORT" + vbCrLf + "2 - CONFIGURE CONTROLLER AUTH USER/PASS" + vbCrLf + "3 - RETURN TO MAIN MENU")
        Console.WriteLine()
        Console.Write("SELECT MENU OPTION > ")

        Dim MenuOption As Integer = Val(Console.ReadLine())

        Select Case MenuOption

            Case 1

                ControllerConnectionSettingsMenu1()

                Exit Select

            Case 2

                ControllerConnectionSettingsMenu2()

                Exit Select

            Case 3

                Console.Clear()
                MainMenu()

                Exit Select

            Case Else

                InvalidOption()

                ControllerConnectionSettings()

                Exit Select

        End Select

    End Sub

#Region "Menu 1 - Controller Host/IP/Port"

    Private Sub ControllerConnectionSettingsMenu1()

        Console.Clear()
        Console.WriteLine("CONFIGURE CONTROLLER HOST/IP/PORT")

        Console.WriteLine()
        Console.WriteLine("CURRENT SETTINGS")
        Console.WriteLine("-----------------------------------------------")

        Console.WriteLine("CONTROLLER IP/HOST: " + My.Settings.ControllerIP + vbCrLf + "CONTROLLER PORT: " + My.Settings.ControllerPort.ToString)

        Console.WriteLine("-----------------------------------------------")

        Console.WriteLine()
        Console.WriteLine("PRESS F1 TO CHANGE, F3 TO RETURN TO PREVIOUS MENU")

        Select Case Console.ReadKey(True).Key

            Case ConsoleKey.F1

                Do

                    Console.WriteLine()
                    Console.Write("ENTER CONTROLLER HOSTNAME OR IP > ")

                    ControllerIP = Console.ReadLine()

                    If ControllerIP = Nothing Then
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("PLEASE ENTER CONTROLLER HOSTNAME OR IP")
                        Console.ResetColor()
                        Console.Beep()
                    End If

                Loop While ControllerIP = Nothing

                Do
                    Console.WriteLine()
                    Console.Write("ENTER CONTROLLER PORT > ")
                    ControllerPort = Console.ReadLine()

                    If ControllerPort = Nothing Then
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("PLEASE ENTER CONTROLLER PORT")
                        Console.ResetColor()
                        Console.Beep()
                    End If

                Loop While ControllerPort = Nothing


                'Save The Settings
                My.Settings.ControllerIP = ControllerIP
                My.Settings.ControllerPort = ControllerPort
                My.Settings.Save()

                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("SAVE SUCCESSFUL!")
                Console.ResetColor()
                ControllerIP = Nothing
                ControllerPort = Nothing



                While True
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.Blue
                    Console.WriteLine("Press Enter To Go Back To Previous Menu...")
                    Console.ResetColor()
                    Console.WriteLine()

                    If Console.ReadKey(True).Key = ConsoleKey.Enter Then
                        Console.Clear()
                        ControllerConnectionSettings()
                        Exit While
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("INVALID KEY PRESSED")
                        Console.ResetColor()
                        Console.Beep()
                    End If


                End While


            Case ConsoleKey.F3

                Console.Clear()
                ControllerConnectionSettings()

            Case Else

                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("INVALID KEY PRESSED. PLEASE WAIT A MOMENT...")
                Console.ResetColor()
                Console.Beep()
                System.Threading.Thread.Sleep(2000)

                ControllerConnectionSettingsMenu1()

        End Select
    End Sub

#End Region

#Region "Menu 2 - Controller Auth User/Pass"

    Private Sub ControllerConnectionSettingsMenu2()

        Console.Clear()
        Console.WriteLine("CONFIGURE CONTROLLER AUTH USER/PASS")

        Console.WriteLine()
        Console.WriteLine("CURRENT SETTINGS")
        Console.WriteLine("-----------------------------------------------")

        Console.WriteLine("CONTROLLER AUTH USER: " + My.Settings.CommUser + vbCrLf + "CONTROLLER AUTH PASSCODE: " + My.Settings.CommPassword)

        Console.WriteLine("-----------------------------------------------")

        Console.WriteLine()
        Console.WriteLine("PRESS F1 TO CHANGE, F2 CLEAR SETTINGS, F3 TO RETURN TO PREVIOUS MENU")

        Select Case Console.ReadKey(True).Key

            Case ConsoleKey.F1

                Do
                    Console.WriteLine()
                    Console.Write("ENTER CONTROLLER AUTH USERNAME > ")
                    CommunicationsUsername = Console.ReadLine()

                    If CommunicationsUsername = Nothing Then
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("PLEASE ENTER CONTROLLER AUTH USERNAME")
                        Console.ResetColor()
                        Console.Beep()
                    End If
                Loop While CommunicationsUsername = Nothing

                Do
                    Console.WriteLine()
                    Console.Write("ENTER CONTROLLER AUTH PASSWORD > ")
                    CommunicationsPassword = Console.ReadLine()

                    If CommunicationsPassword = Nothing Then
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("PLEASE ENTER CONTROLLER AUTH PASSWORD")
                        Console.ResetColor()
                        Console.Beep()
                    End If
                Loop While CommunicationsPassword = Nothing

                'Save The Settings
                My.Settings.CommUser = CommunicationsUsername
                My.Settings.CommPassword = CommunicationsPassword
                My.Settings.Save()

                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("SAVE SUCCESSFUL!")
                Console.ResetColor()

                CommunicationsUsername = Nothing
                CommunicationsPassword = Nothing

                While True
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.Blue
                    Console.WriteLine("Press Enter To Go Back To Previous Menu...")
                    Console.ResetColor()
                    Console.WriteLine()

                    If Console.ReadKey(True).Key = ConsoleKey.Enter Then
                        Console.Clear()
                        ControllerConnectionSettings()
                        Exit While
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("INVALID KEY PRESSED")
                        Console.ResetColor()
                        Console.Beep()
                    End If


                End While

            Case ConsoleKey.F2

                My.Settings.CommUser = Nothing
                My.Settings.CommPassword = Nothing
                My.Settings.Save()

                WriteLine("SETTINGS HAVE BEEN ERASED", False)

                While True
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.Blue
                    Console.WriteLine("Press Enter To Go Back To Previous Menu...")
                    Console.ResetColor()
                    Console.WriteLine()

                    If Console.ReadKey(True).Key = ConsoleKey.Enter Then
                        Console.Clear()
                        ControllerConnectionSettings()
                        Exit While
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("INVALID KEY PRESSED")
                        Console.ResetColor()
                        Console.Beep()
                    End If


                End While

            Case ConsoleKey.F3

                Console.Clear()
                ControllerConnectionSettings()

            Case Else

                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("INVALID KEY PRESSED. PLEASE WAIT A MOMENT...")
                Console.ResetColor()
                Console.Beep()
                Threading.Thread.Sleep(2000)
                ControllerConnectionSettingsMenu2()

        End Select


    End Sub


#End Region

#End Region

#Region "Terminal Config Menu"

    Private Sub TerminalConfig()

        Console.Clear()
        Console.WriteLine("TERMINAL CONFIGURATION")
        Console.WriteLine()
        Console.WriteLine("1 - TERMINAL ID" + vbCrLf + "2 - RETURN TO MAIN MENU")
        Console.WriteLine()
        Console.Write("SELECT MENU OPTION > ")

        Dim MenuOption As Integer = Val(Console.ReadLine())

        Select Case MenuOption

            Case 1

                Console.Clear()
                Console.WriteLine("CONFIGURE TERMINAL ID")

                Console.WriteLine()
                Console.WriteLine("CURRENT SETTINGS")
                Console.WriteLine("-----------------------------------------------")

                Console.WriteLine("TERMINAL ID: " + My.Settings.TerminalID.ToString)

                Console.WriteLine("-----------------------------------------------")

                Console.WriteLine()
                Console.WriteLine("PRESS F1 TO CHANGE, F3 TO RETURN TO PREVIOUS MENU")

                Select Case Console.ReadKey(True).Key

                    Case ConsoleKey.F1

                        Do

                            Console.WriteLine()
                            Console.Write("ENTER TERMINAL ID > ")

                            TerminalID = Convert.ToInt32(Console.ReadLine())

                            If TerminalID = Nothing Then
                                Console.WriteLine()
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine("PLEASE ENTER TERMINAL ID")
                                Console.ResetColor()
                                Console.Beep()
                            End If

                        Loop While TerminalID = Nothing

                        'Save The Settings
                        My.Settings.TerminalID = TerminalID
                        My.Settings.Save()

                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.DarkGreen
                        Console.WriteLine("SAVE SUCCESSFUL!")
                        Console.ResetColor()
                        TerminalID = Nothing



                        While True
                            Console.WriteLine()
                            Console.ForegroundColor = ConsoleColor.Blue
                            Console.WriteLine("Press Enter To Go Back To Previous Menu...")
                            Console.ResetColor()
                            Console.WriteLine()

                            If Console.ReadKey(True).Key = ConsoleKey.Enter Then
                                Console.Clear()
                                TerminalConfig()
                                Exit While
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine("INVALID KEY PRESSED")
                                Console.ResetColor()
                                Console.Beep()
                            End If


                        End While


                    Case ConsoleKey.F3

                        Console.Clear()
                        TerminalConfig()

                    Case Else

                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.Write("INVALID KEY PRESSED. PLEASE WAIT A MOMENT...")
                        Console.ResetColor()
                        Console.Beep()
                        Threading.Thread.Sleep(2000)

                        TerminalConfig()

                End Select

            Case 2

                Console.Clear()
                ProcessMainInput()

            Case Else

                InvalidOption()

                TerminalConfig()

        End Select


    End Sub

#End Region

#Region "Store Open/Close Menu"

    Private Sub StoreOpenClose()

        Console.WriteLine("1   Open the Store")
        Console.WriteLine("2   Close the Store")
        Console.WriteLine("3   Enable a Terminal")
        Console.WriteLine("4   Disable a Terminal")
        Console.WriteLine("5   Display Terminal Status")
        Console.WriteLine("6   Display Terminal Suspended Transactions")
        Console.WriteLine("7   Run Automatic Procedures")

        Console.WriteLine()
        Console.Write("SELECT MENU OPTION > ")

        Dim MenuOption As Integer = Val(Console.ReadLine())

        Select Case MenuOption

            Case 3

                EnableTerminal()

            Case 4


        End Select

    End Sub

    Private Sub EnableTerminal()



    End Sub

#End Region

#Region "Line And Key Functions"

    Public Sub PressAnyKey()

        '** Waits For A Key Press and Then Continues

        Console.WriteLine()
        Console.WriteLine("PRESS ANY KEY TO CONTINUE...")
        Console.ReadKey()

    End Sub

    Public Sub WriteLine(ByVal Text As String, Optional UC As Boolean = False, Optional LineColor As ConsoleColor = ConsoleColor.Black)

        '* Writes A New Line With Spacing And Option For Uppercase

        Console.WriteLine()

        If UC = True Then

            Console.WriteLine(Text.ToUpper)

        Else

            If Not LineColor = ConsoleColor.Black Then

                Console.ForegroundColor = LineColor
                Console.WriteLine(Text)
                Console.ResetColor()

            Else

                Console.WriteLine(Text)

            End If

        End If

    End Sub

#End Region


End Module
