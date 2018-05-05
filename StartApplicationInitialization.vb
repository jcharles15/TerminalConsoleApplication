Module StartApplicationInitialization


    Public WithEvents BackgroundWorkerDeviceLoad As BackgroundWorker

    Public Sub ApplicationInitialization()

        'START - This Is The Very Beginning Of The Application Startup Process

        'Load The Default Display And Keyboard.

        Terminal.Display.DisplayData("W008 PROGRAM IS", "     BEING LOADED")
        Threading.Thread.Sleep(1000)

        ControllerIP = My.Settings.ControllerIP
        ControllerPort = My.Settings.ControllerPort

        If ControllerIP = Nothing And ControllerPort = Nothing Then

            SearchForController()
            Threading.Thread.Sleep(5000)

        Else

            WriteLine("Connecting To Controller : " + ControllerIP + " Port : " + ControllerPort, False, ConsoleColor.Magenta)

            Terminal.Display.DisplayData("W064 CONTACTING", "     CONTROLLER")

            Threading.Thread.Sleep(1000)

            If InitializeControllerConnection() Then

                InitializeTerminalDevices()

            End If

        End If

    End Sub
    Public Const GET_DEVICES = "GETDEVICES|"
    Public Sub InitializeTerminalDevices()


        TerminalID = 1

        If TerminalID = Nothing Then
            MsgBox("TERMINAL ID ERROR")
        End If

        'Load The Database And Devices

        BackgroundWorkerDeviceLoad = New BackgroundWorker

        SendData(GET_DEVICES + TerminalID.ToString)



    End Sub

#Region "Initialize Devices"

    Public Sub LoadDevices()

        BackgroundWorkerDeviceLoad.RunWorkerAsync()

    End Sub

    Private Sub BackgroundWorkerDeviceLoad_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorkerDeviceLoad.DoWork

        For Each Device In DeviceLoadList

            MsgParts = Split(Device, "|")
            DeviceType1 = MsgParts(0)
            DeviceName = MsgParts(1)
            DeviceState = MsgParts(2)

            Select Case DeviceType1

                '******************************** CUSTOMER POLE DISPLAY INITIALIZATION *******************************************

                Case "Customer Pole Display"

                    If DeviceState = "Yes" Then

                        Console.WriteLine("Loading Customer Pole Display")

                        'Write TO LOG

                        Try

                            ' LineDisplayHandler()
                            ' DisplayText(LineDisplay, "LOADING DEVICES")
                            Threading.Thread.Sleep(5000)
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            '  DisplayText(LineDisplay, "LINE DISPLAY LOADED")
                            WriteLine("Line Display Loaded OK", False)

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                            'Write TO LOG - Something Happened Log It
                            MsgBox(ex.Message)
                        End Try

                    Else 'Device State Disabled
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Line Display Disabled", False)

                        'Write This To Log

                    End If

                    '******************************** OPERATOR DISPLAY INITIALIZATION *******************************************

                Case "Operator Display"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Operator Display", False)

                        'Write TO LOG

                        Try

                            '  OperatorDisplayHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "OPERATOR DISPLAY" + vbCrLf + "LOADED")
                            WriteLine("Operator Display Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else 'Device State Disabled
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Operator Display Disabled")

                        'Write This To Log

                    End If

                    '******************************** CASH DRAWER INITIALIZATION *******************************************

                Case "Cash Drawer"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Cash Drawer")

                        'Write TO LOG

                        Try

                            'CashDrawerHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "CASH DRAWER LOADED")
                            WriteLine("Cash Drawer Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else 'Device State Disabled
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Cash Drawer Disabled")

                    End If

                    '******************************** BARCODE SCANNER INITIALIZATION *******************************************

                Case "Barcode Scanner"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Barcode Scanner")

                        Try

                            '  BarcodeScannerHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            '  DisplayText(LineDisplay, "BARCODE SCANNER" + vbCrLf + "LOADED")
                            WriteLine("Barcode Scanner Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Barcode Scanner Disabled")

                    End If

                    '******************************** MSR INITIALIZATION *******************************************

                Case "Msr"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Msr")

                        Try

                            ' MsrHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            '  DisplayText(LineDisplay, "MSR LOADED")
                            WriteLine("Msr Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Msr Disabled")

                    End If

                    '******************************** POS KEYBOARD INITIALIZATION *******************************************

                Case "Pos Keyboard"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Pos Keyboard")

                        Try

                            '   PosKeyboardHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            '    DisplayText(LineDisplay, "POS KEYBOARD LOADED")
                            WriteLine("Pos Keyboard Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Pos Keyboard Disabled")

                    End If

                    '******************************** KEYLOCK INITIALIZATION *******************************************

                Case "Keylock"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Keylock")

                        Try

                            '  KeylockHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "KEYLOCK LOADED")
                            WriteLine("Keylock Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Keylock Disabled")

                    End If

                    '******************************** RECEIPT PRINTER INITIALIZATION *******************************************

                Case "Receipt Printer"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Receipt Printer")

                        Try

                            '  ReceiptPrinterHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "RECEIPT PRINTER" + vbCrLf + "LOADED")
                            WriteLine("Receipt Printer Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Receipt Printer Disabled")

                    End If

                    '******************************** MICR INITIALIZATION *******************************************

                Case "Micr"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Micr")

                        Try

                            ' MicrHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "MICR LOADED")
                            WriteLine("Micr Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Micr Disabled")

                    End If

                    '******************************** TONE INDICATOR INITIALIZATION *******************************************

                Case "Tone Indicator"

                    If DeviceState = "Yes" Then

                        WriteLine("Loading Tone Indicator")

                        Try

                            ' ToneIndicatorHandler()
                            SendData("DEVICELOADOK|" + DeviceType1 + "|" + TerminalID.ToString)
                            ' DisplayText(LineDisplay, "TONE INDICATOR" + vbCrLf + "LOADED")
                            WriteLine("Tone Indicator Loaded OK")

                        Catch ex As Exception
                            SendData("DEVICELOADERR|" + DeviceType1 + "|" + TerminalID.ToString)
                        End Try

                    Else
                        SendData("DEVICEDIS|" + DeviceType1 + "|" + TerminalID.ToString)
                        WriteLine("Tone Indicator Disabled")

                    End If

            End Select

            Threading.Thread.Sleep(1000)

        Next

    End Sub

    Private Sub BackgroundWorker2_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorkerDeviceLoad.RunWorkerCompleted

        ' DisplayText(LineDisplay, "DEVICE INIT COMPLETE")
        WriteLine("Device Initialization Has Completed")

        Call InitializeTerminalLogon()

    End Sub

#End Region ' NOT CURRENTLY USING THIS / NEEDS WORK

#Region "Initalize Terminal Logon"

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub InitializeTerminalLogon()

        'PosKeyboard.DirectIO(CommandLedOff, DataWaitLed, Null)

        Terminal.Display.DisplayData("CBJ POINT OF SALE")

        Console.Clear()
        Console.WriteLine("###############################")
        Console.WriteLine("#                             #")
        Console.WriteLine("#      CBJ POINT OF SALE      #")
        Console.WriteLine("#                             #")
        Console.WriteLine("###############################")
        Console.WriteLine(StrDup(Console.WindowWidth, "-"))
        Console.WriteLine()
        Threading.Thread.Sleep(5000)








    End Sub

#End Region

End Module
