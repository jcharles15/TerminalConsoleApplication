Namespace CBJ_TERMINAL_SYSTEM

    Public Class Backend

        Public Shared Test As Boolean = False


#Region "Pos Device Config Menu"

        Public Shared Sub PosDeviceConfig()

            Console.Clear()
            SelectDeviceMenu()

            Dim MenuOption As Integer = Val(Console.ReadLine())

            Select Case MenuOption

                Case 1 'Cash Drawer

                    Try

                        Terminal.Drawer = New Drawer

                        CashDrawerMenu()

                    Catch ex As Exception

                        WriteLine("Could Not Connect To Cash Drawer" + vbCrLf + ex.Message)

                    End Try

                Case 2 'Line Display

                    Try

                        Terminal.Display = New Display

                        LineDisplayMenu()

                    Catch ex As Exception

                        WriteLine("Could Not Connect To Line Display" + vbCrLf + ex.Message)

                    End Try

                Case 3 'Pos Printer

                    Try

                        Terminal.Printer = New Printer()

                        PosPrinterMenu()

                    Catch ex As Exception

                        WriteLine("Could Not Connect To Printer" + vbCrLf + ex.Message)

                    End Try

                Case 4 'Pos Keyboard

                    Terminal.IBMKEY = New Keyboard

                    Test = True

                    PosKeyboardMenu()

                Case 5 ' Keylock

                    Terminal.Keylock = New KeyLK

                    Test = True

                    KeylockMenu()


                Case 6 ' Hard Totals **Currently Not In Service**

                    ' Terminal.HardTotals = New Totals

                    'HardTotalsMenu()

                    Console.WriteLine("Function Currently Not Available")

                Case 7

                    Terminal.Msr = New MsrReader

                    MsrMenu()

                Case 8

                    Terminal.Tone = New ToneIND

                    ToneIndicatorMenu()

                Case 9




                Case 10 'Main Menu

                    Console.Clear()
                    ProcessMainInput()


                Case Else

                    InvalidOption()

                    PosDeviceConfig()

            End Select

            PressAnyKey()

            PosDeviceConfig()

        End Sub


        Private Shared Sub SelectDeviceMenu()

            Console.WriteLine("###############################")
            Console.WriteLine("# POS DEVICE CONFIG           #")
            Console.WriteLine("###############################")
            Console.WriteLine()

            Console.WriteLine("-----------------------------------------------")
            Console.WriteLine(" 1- CASH DRAWER" + vbCrLf +
                              " 2- LINE DISPLAY" + vbCrLf +
                              " 3- POS PRINTER" + vbCrLf +
                              " 4- POS KEYBOARD" + vbCrLf +
                              " 5- KEYLOCK" + vbCrLf +
                              " 6- HARD TOTALS" + vbCrLf +
                              " 7- MSR" + vbCrLf +
                              " 8- TONE INDICATOR" + vbCrLf +
                              " 9- SHOW DEVICES INSTALLED" + vbCrLf +
                              " 10- RETURN TO MAIN MENU")
            Console.WriteLine("-----------------------------------------------")

            Console.WriteLine()
            Console.Write("SELECT MENU OPTION > ")
        End Sub

#Region "Cash Drawer Menu"

        Private Shared Sub CashDrawerMenu()

            Dim Counter As Integer = 0
            Dim Counter2 As Integer = 0

            Console.Clear()

            If Not My.Settings.CashDrawer = "" Then

                'Device Name Is Set. Show Menu


                IndividualDeviceMenus("CASH DRAWER")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.CashDrawer)
                Console.Write("  DEVICE STATUS: ")
                Dim Status As String = ""
                If Terminal.Drawer.CashDrawer.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If


                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write(" SELECT MENU OPTION > ")

                Dim CDOPTIONS As String = Console.ReadLine

                Select Case CDOPTIONS

                    Case 1
                        Terminal.Drawer.CashDrawer.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Cash Drawer Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        CashDrawerMenu()
                    Case 2
                        Terminal.Drawer.CashDrawer.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Cash Drawer Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        CashDrawerMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim CDDEVICENAME As String = Console.ReadLine

                        My.Settings.CashDrawer = CDDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        CashDrawerMenu()

                    Case 4
                        If My.Settings.CashDrawer = Nothing Then
                            Console.WriteLine()
                            Console.WriteLine("PLEASE SET DEVICE NAME")
                            Console.WriteLine()
                            Console.WriteLine("PRESS ANY KEY TO CONTINUE")
                            Console.ReadKey()
                            CashDrawerMenu()
                        Else

                            If Terminal.Drawer.CashDrawer.DeviceDescription = "Microsoft CashDrawer Simulators" Then
                                Terminal.Drawer.CashDrawer.OpenDrawer()
                            End If

                            Counter = 0

                            If SecurityPASS = False Then

                                ' Security Check Failed

                                If Terminal.Drawer.CashDrawer.DrawerOpened = True Then 'Drawer Already Open

                                    Console.WriteLine()
                                    Console.WriteLine("WAITING 10 SECONDS FOR DRAWER TO BE CLOSED")

                                    Do
                                        Threading.Thread.Sleep(1000)
                                        Counter += 1
                                        Console.Write(".")
                                    Loop Until Terminal.Drawer.CashDrawer.DrawerOpened = False Or Counter = 10  ' Wait For Drawer To Be Physically Closed So We Can Test It

                                    If Terminal.Drawer.CashDrawer.DrawerOpened = False Then 'Drawer Was Physically Closed
                                        ' Drawer Close Was Detected

                                        Console.WriteLine()
                                        Console.WriteLine("DRAWER CLOSED DETECTED - NOW TRYING TO OPEN THE CASH DRAWER. PLEASE WAIT")


                                        Do
                                            Terminal.Drawer.CashDrawer.OpenDrawer()
                                            Threading.Thread.Sleep(1000)
                                            Counter2 += 1
                                            Console.Write(".")
                                        Loop Until Terminal.Drawer.CashDrawer.DrawerOpened = True Or Counter2 = 10

                                        If Terminal.Drawer.CashDrawer.DrawerOpened = True Then

                                            Console.WriteLine()
                                            Console.WriteLine("DRAWER OPEN DETECTED - THE CASH DRAWER SEEMS TO HAVE OPENED OKAY")

                                        ElseIf Terminal.Drawer.CashDrawer.DrawerOpened = False And Counter2 = 10 Then

                                            Console.WriteLine()
                                            Console.ForegroundColor = ConsoleColor.Red
                                            Console.WriteLine("FAILED OPENING THE CASH DRAWER. THIS MAY INDICATE THERE IS A PROBLEM")
                                            Console.ResetColor()

                                        End If

                                    ElseIf Terminal.Drawer.CashDrawer.DrawerOpened = True And Counter = 10 Then
                                        'Drawer Close Was Not Detected And Counter Has Reached 10

                                        Console.WriteLine()
                                        Console.ForegroundColor = ConsoleColor.Red
                                        Console.WriteLine("DID NOT DETECT DRAWER CLOSE. DRAWER WAS NOT CLOSED OR DRAWER IS FAILING")
                                        Console.ResetColor()
                                        Threading.Thread.Sleep(5000)
                                        Counter = 0
                                        CashDrawerMenu()

                                    End If

                                    SecurityPASS = True

                                    PressAnyKey()

                                    Counter = 0
                                    Counter2 = 0

                                    CashDrawerMenu()

                                Else
                                    Console.WriteLine()
                                    Console.WriteLine("FOR SECURITY REASONS, THE DRAWER MUST BE MANUALLY OPENED FIRST")

                                    Console.WriteLine()
                                    Console.WriteLine("WAITING FOR 10 SECONDS")

                                    Do
                                        Threading.Thread.Sleep(1000)
                                        Counter += 1
                                        Console.Write(".")

                                    Loop Until Terminal.Drawer.CashDrawer.DrawerOpened = True Or Counter = 10

                                    If Terminal.Drawer.CashDrawer.DrawerOpened = True Then

                                        Terminal.Drawer.CashDrawer.WaitForDrawerClose(10000, 5000, 10000, 5000)

                                        Terminal.Drawer.CashDrawer.OpenDrawer()

                                        SecurityPASS = True

                                    Else

                                        Console.WriteLine()
                                        Console.ForegroundColor = ConsoleColor.Red
                                        Console.WriteLine("DRAWER WAS NOT OPENED WITH KEY OR DRAWER IS FAILING")
                                        Console.ResetColor()
                                        SecurityPASS = False

                                        PressAnyKey()

                                        CashDrawerMenu()

                                    End If



                                End If

                            Else
                                'Security Check Passed

                                If Terminal.Drawer.CashDrawer.DrawerOpened Then
                                    WriteLine("Drawer Already Open, Close To Test and Then Try Again.")
                                Else
                                    Terminal.Drawer.CashDrawer.OpenDrawer()
                                    WriteLine("Drawer Opened")

                                End If

                                PressAnyKey()

                                CashDrawerMenu()

                            End If


                        End If

                    Case 5

                        SecurityPASS = False 'Exiting Cash Drawer Menu So, Return Drawer Security Back To ENFORCED

                        Terminal.Drawer.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()

                    Case Else

                        InvalidOption()

                        CashDrawerMenu()

                End Select

            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim CDDEVICENAME As String = Console.ReadLine

                If Not CDDEVICENAME = "" Then


                    My.Settings.CashDrawer = CDDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If
                CashDrawerMenu()

            End If

            Try

                Terminal.Drawer.Close()


            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Line Display Menu"

        Private Shared Sub LineDisplayMenu()

            Console.Clear()

            If Not My.Settings.LineDisplay = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("LINE DISPLAY")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.LineDisplay)
                Console.Write("  DEVICE STATUS: ")
                Dim Status As String = ""
                If Terminal.Display.LineDisplay1.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write(" SELECT MENU OPTION > ")

                Dim LDOPTIONS As String = Console.ReadLine

                Select Case LDOPTIONS

                    Case 1
                        Terminal.Display.LineDisplay1.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Line Display Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        LineDisplayMenu()
                    Case 2
                        Terminal.Display.LineDisplay1.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Line Display Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        LineDisplayMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim LDDEVICENAME As String = Console.ReadLine

                        My.Settings.LineDisplay = LDDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        LineDisplayMenu()

                    Case 4

                        Terminal.Display.LineDisplay1.ClearText()
                        Terminal.Display.LineDisplay1.DisplayText("CBJ TERMINAL CONFIG" + vbCrLf + "TEST - TEST - TEST")

                        Terminal.Display.LineDisplay1.SetDescriptor(1 - 10, DisplaySetDescriptor.On)

                        PressAnyKey()
                        Terminal.Display.LineDisplay1.ClearText()
                        LineDisplayMenu()

                    Case 5

                        Terminal.Display.LineDisplay1.ClearText()
                        Terminal.Display.LineDisplay1.Release()
                        Terminal.Display.LineDisplay1.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()

                    Case Else

                        InvalidOption()

                        LineDisplayMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim LDDEVICENAME As String = Console.ReadLine

                If Not LDDEVICENAME = "" Then


                    My.Settings.LineDisplay = LDDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                LineDisplayMenu()

            End If


            Try


                Terminal.Display.LineDisplay1.Close()
                Terminal.Display.LineDisplay1.DeviceEnabled = False
                Terminal.Display.LineDisplay1.Claim(1000)

            Catch ex As Exception

            End Try





        End Sub

#End Region

#Region "Pos Printer Menu"

        Private Shared Sub PosPrinterMenu()

            Console.Clear()

            If Not My.Settings.ReceiptPrinter = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("RECEIPT PRINTER")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.ReceiptPrinter)
                Console.Write("  DEVICE STATUS: ")
                Dim Status As String = ""
                'If Terminal.Printer.ReceiptPrinter.DeviceEnabled = True Then
                'Console.ForegroundColor = ConsoleColor.DarkGreen
                'Status = "ENABLED"
                'Else
                'Console.ForegroundColor = ConsoleColor.Red
                'Status = "DISABLED"
                'End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write(" SELECT MENU OPTION > ")

                Dim RPOPTIONS As String = Console.ReadLine

                Select Case RPOPTIONS

                    Case 1
                        ' Terminal.Printer.ReceiptPrinter.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Receipt Printer Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        PosPrinterMenu()
                    Case 2

                        '  Terminal.Printer.ReceiptPrinter.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Receipt Printer Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        PosPrinterMenu()

                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim RPDEVICENAME As String = Console.ReadLine

                        My.Settings.ReceiptPrinter = RPDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        PosPrinterMenu()

                    Case 4


                        Try

                            Dim Job As PrintJob
                            Job = New PrintJob("", "") With {
                                .Station = PrinterStation.Receipt
                            }

                            Job.AddLine(" ")
                            Job.AddLine("CBJ TERMINAL CONFIGURATION")
                            Job.AddLine("**** POS DEVICE CONFIG MENU ****")
                            Job.AddLine(" ")
                            Job.AddLine("1 - CASH DRAWER")
                            Job.AddLine("2 - LINE DISPLAY")
                            Job.AddLine("3 - POS PRINTER")
                            Job.AddLine("4 - POS KEYBOARD")
                            Job.AddLine("5 - KEYLOCK")
                            Job.AddLine("6 - HARD TOTALS")
                            Job.AddLine("7 - MSR")

                            Job.AddLine(" ")
                            Job.AddLine(" ")
                            Job.AddLine(" ")
                            Job.AddLine("TITLE                     NUM     KEY")
                            Job.AddLine("-------------------------------------------")
                            Job.AddLine("KEYBOARD TEST             100")
                            Job.AddLine("DISPLAY TEST              101")
                            Job.AddLine("ERROR TEST                103")
                            Job.AddLine("MENU TEST                 104")
                            Job.AddLine("PRINTER TEST              105")
                            Job.AddLine("AUTH OVERRIDE TEST        106")
                            Job.AddLine("MESSAGE TEST              107")
                            Job.AddLine("MICR READ TEST            108")
                            Job.AddLine("DIRECT PRINT TEST         109")
                            Job.AddLine("ALPHANUMERIC TEST         110")
                            Job.AddLine("MSR TEST/CARD ID          111")
                            Job.AddLine("DISPLAY WHATEVER          113")
                            Job.AddLine(" ")
                            Job.AddLine("WX CONDITIONS             200     WX LOOKUP")
                            Job.AddLine("WX CITY LOOKUP            210")
                            Job.AddLine(" ")
                            Job.AddLine("CHANGE KEYBOARD VOLUME    300")
                            Job.AddLine("SALTY BET TABULATOR       301     SALTY BET")
                            Job.AddLine("CALCULATOR                400     CALC")
                            Job.AddLine("CHECK PROCESSING          410")
                            Job.AddLine("SCAN ON INTO TOMORROW     411")
                            Job.AddLine("VOID DOCUMENT             412")
                            Job.AddLine("ENDORSE DOCUMENT          413")
                            Job.AddLine("ELITE TRANSACT            420     TRANSACT")
                            Job.AddLine("GAME TESTING              430")
                            Job.AddLine(" ")
                            Job.AddLine("PLEX                      500     PLEX")
                            Job.AddLine(" ")
                            Job.AddLine("READ PENDING MESSAGES     1100    MESSAGE CK")
                            Job.AddLine(" ")
                            Job.AddLine("PROGRAM LISTING           1599    HELP")
                            Job.AddLine(" ")
                            Job.AddLine("NUMBERWANG!               0407253134")
                            Job.AddLine(" ")
                            Job.AddLine("SYSTEM COMMANDS AVAILIBLE VIA SIGNON.")
                            Job.AddLine("TO VIEW SIGN ON/OFF COMMANDS, KEY")
                            Job.AddLine("SIGNON, LIST/HELP")



                            Terminal.Printer.Print(Job)



                        Catch ex As Exception

                        End Try


                        PressAnyKey()
                        PosPrinterMenu()

                    Case 5


                        'Terminal.Printer.ReceiptPrinter.Release()
                        'Terminal.Printer.ReceiptPrinter.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()

                    Case Else

                        InvalidOption()

                        PosPrinterMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim RPDEVICENAME As String = Console.ReadLine

                If Not RPDEVICENAME = "" Then


                    My.Settings.ReceiptPrinter = RPDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                PosPrinterMenu()

            End If


            Try


                'Terminal.Printer.ReceiptPrinter.Close()
                'Terminal.Printer.ReceiptPrinter.DeviceEnabled = False
                'Terminal.Printer.ReceiptPrinter.Claim(1000)

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Pos Keyboard Menu"

        Private Shared Sub PosKeyboardMenu()

            Console.Clear()

            If Not My.Settings.PosKeyboard = "" Then

                'Device Name Is Set. Show Menu

                Console.WriteLine("# POS KEYBOARD #")
                Console.WriteLine()
                Console.WriteLine(" 1- ENABLE")
                Console.WriteLine(" 2- DISABLE")
                Console.WriteLine(" 3- SET DEVICE NAME")
                Console.WriteLine(" 4- TEST INDICATOR LIGHTS")
                Console.WriteLine(" 5- GO BACK")
                Console.WriteLine()
                Console.WriteLine("    DEVICE NAME: " + My.Settings.PosKeyboard)
                Console.Write("  DEVICE STATUS: ")
                Dim Status As String = ""
                If Terminal.IBMKEY.IBM50KEY.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write("POS KEYBOARD IS READY. PRESS ANY KEY TO TEST." + vbCrLf + vbCrLf)
                Console.Write("SELECT MENU OPTION > ")

                Dim PKOPTIONS As String = Console.ReadLine

                Select Case PKOPTIONS

                    Case 1
                        Terminal.IBMKEY.IBM50KEY.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Pos Keyboard Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        PosKeyboardMenu()
                    Case 2
                        Terminal.IBMKEY.IBM50KEY.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Pos Keyboard Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        PosKeyboardMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim PKDEVICENAME As String = Console.ReadLine

                        My.Settings.PosKeyboard = PKDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        PosKeyboardMenu()

                    Case 4

                        'Indicator Lights Test


                        Try


                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOn, DataWaitLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOn, DataOfflineLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOn, DataMessagePendingLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOn, DataReservedLed, Null)

                            Console.WriteLine()
                            Console.WriteLine("LIGHT TEST INITIATED")

                            PressAnyKey()

                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOff, DataWaitLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOff, DataOfflineLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOff, DataMessagePendingLed, Null)
                            Terminal.IBMKEY.IBM50KEY.DirectIO(CommandLedOff, DataReservedLed, Null)


                            PosKeyboardMenu()

                        Catch ex As Exception

                        End Try

                    Case 5

                        Terminal.IBMKEY.IBM50KEY.Release()
                        Terminal.IBMKEY.IBM50KEY.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()

                        Test = False

                    Case Else

                        InvalidOption()

                        PosKeyboardMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim PKDEVICENAME As String = Console.ReadLine

                If Not PKDEVICENAME = "" Then


                    My.Settings.PosKeyboard = PKDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                PosKeyboardMenu()

            End If


            Try


                Terminal.IBMKEY.IBM50KEY.Close()
                Terminal.IBMKEY.IBM50KEY.DeviceEnabled = False
                Terminal.IBMKEY.IBM50KEY.Claim(1000)

            Catch ex As Exception

            End Try



        End Sub

#End Region

#Region "Keylock Menu"

        Private Shared Sub KeylockMenu()

            Console.Clear()

            If Not My.Settings.Keylock = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("KEYLOCK")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.Keylock)
                Console.Write("  DEVICE STATUS: ")

                Dim Status As String = ""
                If Terminal.Keylock.Keylock.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.WriteLine("KEYLOCK IS READY. TURN KEY TO TEST.")
                Console.WriteLine()
                Console.Write("SELECT MENU OPTION > ")

                Dim KLOPTIONS As String = Console.ReadLine

                Select Case KLOPTIONS

                    Case 1
                        Terminal.Keylock.Keylock.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Keylock Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        KeylockMenu()
                    Case 2
                        Terminal.Keylock.Keylock.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Keylock Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        KeylockMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim KLDEVICENAME As String = Console.ReadLine

                        My.Settings.Keylock = KLDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        KeylockMenu()

                    Case 4




                    Case 5

                        Terminal.Keylock.Keylock.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()

                        Test = False

                    Case Else

                        InvalidOption()

                        KeylockMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim KLDEVICENAME As String = Console.ReadLine

                If Not KLDEVICENAME = "" Then


                    My.Settings.Keylock = KLDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                KeylockMenu()

            End If


            Try


                Terminal.Keylock.Keylock.Close()
                Terminal.Keylock.Keylock.DeviceEnabled = False

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Hard Totals Menu"

        Private Shared Sub HardTotalsMenu()

            Console.Clear()

            If Not My.Settings.HardTotals = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("HARD TOTALS")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.HardTotals)
                Console.Write("  DEVICE STATUS: ")

                Dim Status As String = ""
                ' If Terminal.HardTotals.HardTotals.DeviceEnabled = True Then
                'Console.ForegroundColor = ConsoleColor.DarkGreen
                'Status = "ENABLED"
                'Else
                'Console.ForegroundColor = ConsoleColor.Red
                'Status = "DISABLED"
                'End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write("SELECT MENU OPTION > ")

                Dim HTOPTIONS As String = Console.ReadLine

                Select Case HTOPTIONS

                    Case 1
                        ' Terminal.HardTotals.HardTotals.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Hard Totals Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        HardTotalsMenu()
                    Case 2
                        ' Terminal.HardTotals.HardTotals.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Hard Totals Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        HardTotalsMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim HTDEVICENAME As String = Console.ReadLine

                        My.Settings.HardTotals = HTDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        HardTotalsMenu()

                    Case 4




                    Case 5


                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()



                    Case Else

                        InvalidOption()

                        HardTotalsMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim HTDEVICENAME As String = Console.ReadLine

                If Not HTDEVICENAME = "" Then


                    My.Settings.HardTotals = HTDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                HardTotalsMenu()

            End If


            Try


                ' Terminal.HardTotals.HardTotals.DeviceEnabled = False
                'Terminal.HardTotals.HardTotals.Close()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Msr Menu"

        Private Shared Sub MsrMenu()

            Console.Clear()

            If Not My.Settings.Msr = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("MSR")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.Msr)
                Console.Write("  DEVICE STATUS: ")

                Dim Status As String = ""
                If Terminal.Msr.Msr.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.WriteLine("MSR IS READY. SLIDE A CARD.")
                Console.WriteLine()
                Console.Write("SELECT MENU OPTION > ")

                Dim MSROPTIONS As String = Console.ReadLine

                Select Case MSROPTIONS

                    Case 1
                        Terminal.Msr.Msr.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Msr Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        MsrMenu()
                    Case 2
                        Terminal.Msr.Msr.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Msr Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        MsrMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim MSRDEVICENAME As String = Console.ReadLine

                        My.Settings.Msr = MSRDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        MsrMenu()

                    Case 4




                    Case 5

                        Terminal.Msr.Msr.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()



                    Case Else

                        InvalidOption()

                        MsrMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim MSRDEVICENAME As String = Console.ReadLine

                If Not MSRDEVICENAME = "" Then


                    My.Settings.Msr = MSRDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                MsrMenu()

            End If


            Try


                Terminal.Msr.Msr.DeviceEnabled = False
                Terminal.Msr.Msr.Close()

            Catch ex As Exception

            End Try

        End Sub



#End Region

#Region "Tone Indicator Menu"

        Private Shared Sub ToneIndicatorMenu()

            Console.Clear()

            If Not My.Settings.ToneIndicator = "" Then

                'Device Name Is Set. Show Menu

                IndividualDeviceMenus("TONE INDICATOR")

                Console.WriteLine("    DEVICE NAME: " + My.Settings.ToneIndicator)
                Console.Write("  DEVICE STATUS: ")
                Dim Status As String = ""
                If Terminal.Tone.Tone.DeviceEnabled = True Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen
                    Status = "ENABLED"
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Status = "DISABLED"
                End If

                Console.Write(Status.ToUpper)
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine()
                Console.Write("SELECT MENU OPTION > ")

                Dim TIOPTIONS As String = Console.ReadLine

                Select Case TIOPTIONS

                    Case 1
                        Terminal.Tone.Tone.DeviceEnabled = True
                        Console.WriteLine()
                        Console.WriteLine("Tone Indicator Is Now Enabled")
                        Threading.Thread.Sleep(2000)
                        ToneIndicatorMenu()
                    Case 2
                        Terminal.Tone.Tone.DeviceEnabled = False
                        Console.WriteLine()
                        Console.WriteLine("Tone Indicator Is Now Disabled")
                        Threading.Thread.Sleep(2000)
                        ToneIndicatorMenu()
                    Case 3
                        Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                        Console.WriteLine()
                        Console.ForegroundColor = ConsoleColor.Blue
                        Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                        Console.ResetColor()
                        Console.WriteLine()
                        Console.Write("DEVICE NAME > ")

                        Dim TIDEVICENAME As String = Console.ReadLine

                        My.Settings.ToneIndicator = TIDEVICENAME
                        My.Settings.Save()
                        My.Settings.Reload()
                        Console.WriteLine()
                        Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                        Threading.Thread.Sleep(2000)
                        ToneIndicatorMenu()

                    Case 4

                        Try

                            Terminal.Tone.BeepTest()

                            PressAnyKey()

                            ToneIndicatorMenu()



                        Catch ex As Exception

                        End Try




                    Case 5

                        Terminal.Tone.Tone.Close()

                        WriteLine("PLEASE WAIT A MOMENT . . .", True)
                        Threading.Thread.Sleep(2000)
                        Console.Clear()
                        PosDeviceConfig()



                    Case Else

                        InvalidOption()

                        ToneIndicatorMenu()

                End Select





            Else

                'Device Name Not Set. Set Device Name

                Console.WriteLine("PLEASE ENTER THE DEVICE NAME - THEN PRESS ENTER . .  .")
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Blue
                Console.WriteLine("*NOTE - YOU MUST ENTER THE DEVICE NAME THAT WAS SETUP IN OPOS CONFIGURATION")
                Console.ResetColor()
                Console.WriteLine()
                Console.Write("DEVICE NAME > ")

                Dim TIDEVICENAME As String = Console.ReadLine

                If Not TIDEVICENAME = "" Then


                    My.Settings.ToneIndicator = TIDEVICENAME
                    My.Settings.Save()
                    My.Settings.Reload()
                    Console.WriteLine()
                    Console.WriteLine("THANK YOU! - DEVICE NAME HAS BEEN RECORDED")
                    Threading.Thread.Sleep(2000)

                Else

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine()
                    Console.WriteLine("DEVICE NAME CANNOT BE EMPTY. TRY AGAIN")
                    Console.ResetColor()
                    Threading.Thread.Sleep(2000)

                End If

                ToneIndicatorMenu()

            End If


            Try


                Terminal.Tone.Tone.DeviceEnabled = False
                Terminal.Tone.Tone.Close()

            Catch ex As Exception

            End Try

        End Sub

#End Region

        Private Shared Sub IndividualDeviceMenus(ByVal DeviceType As String)

            Console.WriteLine("# " + DeviceType + " #")
            Console.WriteLine()
            Console.WriteLine(" 1- ENABLE")
            Console.WriteLine(" 2- DISABLE")
            Console.WriteLine(" 3- SET DEVICE NAME")
            Console.WriteLine(" 4- TEST")
            Console.WriteLine(" 5- GO BACK")
            Console.WriteLine()


        End Sub

#End Region


    End Class

End Namespace
