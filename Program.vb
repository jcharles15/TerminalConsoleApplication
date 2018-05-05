

Namespace CBJ_TERMINAL_SYSTEM

#Region "Keymap"

    Public Enum Keys

        Zero = 0
        N1 = 1
        N2 = 2
        N3 = 3
        N4 = 4
        N5 = 5
        N6 = 6
        N7 = 7
        N8 = 8
        N9 = 9

        Slash = 10
        SignOnOff = 11
        Enter = 12
        Ctrl = 13

        GenMdse = 14
        Grocery = 15
        Dairy = 16
        Produce = 17
        Liquor = 18
        Bakery = 19
        Deli = 20
        Meat = 21
        FrozenFood = 22
        Floral = 23
        Fish = 24
        PhoneCards = 25
        HotDeli = 26
        Video = 27
        FsNoFs = 28

        Clear = 29
        Void = 30
        S1 = 30
        Override = 31
        S2 = 31
        NoSale = 32
        StoreCoupon = 33
        Discount = 34
        PriceOverride = 35
        TaxNoTax = 36
        MfrCoupon = 37
        SuspendRetrieve = 38
        Qty = 39
        TaxExempt = 40
        GiftCert = 41
        ClubCard = 42
        DataEntry = 42
        WicChecks = 43
        Weight = 44
        Cash = 45
        Check = 46
        CreditCards = 47
        Total = 48

        SimKey1 = 49
        SimKey2 = 50
        SimKey3 = 51
        SimKey4 = 52
        SimKey5 = 53

    End Enum

#End Region

#Region "Terminal Class"

    Public Class Terminal

        Public Shared T As Terminal
        Public Shared Display As Display
        Public Shared IBMKEY As Keyboard
        Public Shared Drawer As Drawer
        Public Shared Tone As ToneIND
        Public Shared Printer As Printer
        Public Shared Msr As MsrReader
        Public Shared Micr As MicrReader
        Public Shared Keylock As KeyLK

        Public Sub New()

            Display = New Display
            Display.DisplayData("X01 HARDWARE INIT", "DISPLAY OK")

            IBMKEY = New Keyboard
            IBMKEY.OfflineLightOn()
            IBMKEY.WaitLightOn()
            Display.DisplayDataL2("KEYBOARD OK")

            Drawer = New Drawer
            Display.DisplayDataL2("CASH DRAWER OK")

            Try

                Tone = New ToneIND
                Tone.BeepTest()
                Display.DisplayDataL2("TONE INDICATOR OK")

            Catch ex As Exception

            End Try

            Printer = New Printer()
            Display.DisplayData("PRINTER OK")

            Msr = New MsrReader
            Display.DisplayData("MSR OK")

            Micr = New MicrReader
            Display.DisplayData("MICR OK")

            Keylock = New KeyLK
            Display.Display("KEYLOCK OK")

        End Sub

    End Class

#End Region

#Region "Terminal System Class"

    Public Class TerminalSystem

        Public Shared QuantityData As String = "1"
        Public Shared QuantityDataEntered As Boolean
        Public Shared SlashDataEntered As Boolean
        Public Shared SlashData As String
        Public Shared PriceDataEntered As Boolean
        Public Shared PriceData As String
        Public Shared DealPrice As Double
        Public Shared DealPriceEntered As Boolean
        Public Shared KeyedItemCode As String
        Public Shared Checkout As CustomerCheckoutProcedure

        Public Shared Sub RunPosProgram()

            While True

                Try

                    TerminalWindow.BlankDown(4)

                    Terminal.T = New Terminal

                    Exit While

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End While

            TerminalWindow.ResetWindow(True)
            Terminal.IBMKEY.OfflineLightOff()

            ' TerminalAuthentication.SignON.S = New TerminalAuthentication.SignON

            ' Menu()

            MsgBox("Test Entered Item")

            KeyedItemCode = "2147483647"

            QuantityDataEntered = True
            'SlashDataEntered = True
            PriceDataEntered = True

            QuantityData = "2"
            'SlashData = "5"
            PriceData = "500"

            CustomerCheckoutProcedure.ProcessEnteredItem()



        End Sub

        Public Shared Sub Menu()

            'Main System Prompt - Programs Are Launched From Here

            Try

                Terminal.Tone.QuickBeep()

            Catch ex As Exception

            End Try

            While True

                'Terminal.Display.DisplayData(Utilites.NiceTimeAndDate(), "     " + Utilites.GetBlankSpace(5, My.Settings.Version.Length, 20) + My.Settings.Version)

                Terminal.Display.DisplayData("ENTER MEMBER NUMBER", "OR SELECT PROCEDURE")

                Dim InSeq As KeySeq = Key_Data_Input()


                If InSeq.SignOnOFF Then

                    Select Case InSeq.Numerics

                        Case "0"

                            'Customer Checkout
                            Checkout = New CustomerCheckoutProcedure


                            Exit Select

                        Case "1"

                            'Tender Cashing

                            Exit Select

                        Case "2"

                            'Tender Exchange

                            Exit Select

                        Case "3"

                            'Cashier Loan

                            Exit Select

                        Case "4"

                            'Cashier Pickup

                            Exit Select

                        Case "5"

                            'Tender Listing

                            Exit Select

                        Case "6"

                            'Price Verify/Change

                            Exit Select

                        Case "7"

                            'Operator Training

                            Exit Select

                        Case "8"

                            'Terminal Transfer

                            Exit Select

                        Case "9"

                            'Terminal Monitor

                            Exit Select

                        Case "10"

                            'Tender Count

                            Exit Select

                        Case "11"

                            'Terminal Program Load

                            Exit Select

                        Case "14"

                            'WIC Mode

                            Exit Select



                        Case "999"

                            'Shutdown
                            Terminal.Tone.Beep()

                            Terminal.Display.DisplayData("TERMINAL WILL", "SHUTDOWN IN 20 SECS")

                            '  Process.Start("shutdown", "-s -t 20")

                            Exit Select

                        Case ""

                            Terminal.Display.DisplayData("REPEAT SIGN ON/OFF", "KEY OR PRESS CLEAR")

                            While True

                                Select Case Utilites.GetOneKey()

                                    Case Keys.SignOnOff

                                        TerminalAuthentication.SignOFF()

                                    Case Keys.Clear

                                        Exit While

                                End Select

                            End While

                        Case Else

                            ProgramError(1, InSeq.Numerics)

                            Exit Select

                    End Select


                End If


            End While

        End Sub




        Public Shared Sub InitalizeCheckout()

            Terminal.Display.DisplayData("CUSTOMER CHECKOUT")
            Terminal.IBMKEY.ClearToCont()

            'Make Sure Cash Drawer Is Closed. Cannot Start New Transaction With Cash Drawer Open.



            If Terminal.Drawer.CashDrawer.DrawerOpened Then

                Terminal.Display.DisplayData("CLOSE THE CASH", "DRAWER")

                Terminal.Drawer.CashDrawer.WaitForDrawerClose(5000, 1000, 1000, 1000)

            End If

            Dim Transaction As Transaction = New Transaction

            ' Transaction.SetupDataTable()

        End Sub



        Public Shared Sub ProgramError(ErrorNum As Integer, ReturnToMenu As Boolean)

            ProgramError(ErrorNum, "")

        End Sub

        Public Shared Sub ProgramError(ErrorNum As Integer, DebugText As String)

            Select Case ErrorNum

                Case 1

                    Terminal.Display.DisplayData("E01 PROGRAM FILE", "    NOT FOUND")
                    Terminal.Tone.Beep()
                    Terminal.IBMKEY.ClearToCont()
                    Exit Select

                Case 3

                    Terminal.Display.DisplayData("E03 PRINTER FAILURE", "    CHECK TS CONSOLE")
                    Terminal.Tone.Beep()
                    Terminal.IBMKEY.ClearToCont()
                    Exit Select

                Case 4

                    Terminal.Display.DisplayData("E04 PRINTER ERROR:", "    NO PAPER LOADED")
                    Terminal.Tone.Beep()
                    Terminal.IBMKEY.ClearToCont()

                Case 6

                    Terminal.Display.DisplayData("E06 PRINT INCOMPLETE", "    RELOAD PAPER")
                    Terminal.Tone.Beep()
                    Terminal.IBMKEY.ClearToCont()
                    Exit Select

                Case 7

                    Terminal.Display.DisplayData("C01 KEY NOT", "    AVAILABLE NOW")
                    Terminal.Tone.Beep()
                    Terminal.IBMKEY.ClearToCont()
                    Exit Select

            End Select

            Return

        End Sub


        Public Shared Function Key_Data_Input() As KeySeq

            Terminal.IBMKEY.WaitLightOff()
            Terminal.IBMKEY.InputSeq = New KeySeq()

            Dim Buffer1 As String = Terminal.Display.ScreenBuffer1
            Dim Buffer2 As String = Terminal.Display.ScreenBuffer2

            Terminal.IBMKEY.IBM50KEY.ClearInput()

            While Terminal.IBMKEY.InputSeq.Enter = False _
                    AndAlso Terminal.IBMKEY.InputSeq.ClearOut = False _
                    AndAlso Terminal.IBMKEY.InputSeq.SignOnOFF = False _
                    AndAlso Terminal.IBMKEY.InputSeq.Override = False _
                    AndAlso Terminal.IBMKEY.InputSeq.Quantity = False _
                    AndAlso Terminal.IBMKEY.InputSeq.Slash = False _
                    AndAlso Terminal.IBMKEY.InputSeq.PriceOverride = False _

                ' Terminal.Display.DisplayData(Utilites.NiceTimeAndDate)

                Threading.Thread.Sleep(15)


                If Terminal.IBMKEY.InputSeq.Numerics <> "" Then
                    Terminal.Display.DisplayDataL2(Terminal.IBMKEY.InputSeq.Numerics)

                Else
                    Terminal.Display.DisplayDataL2(Buffer2)

                End If



                If Terminal.IBMKEY.InputSeq.WaitingKey Then

                    'There Is A Waiting Key To Be Processed

                    'Set To False. Now, We Will Process The Key
                    Terminal.IBMKEY.InputSeq.WaitingKey = False


                    Select Case Terminal.IBMKEY.InputSeq.UnprocessedKeypress



                        Case 49
                            Console.WriteLine("  Simulator Key Pressed")
                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "1"
                            Exit Select

                        Case 50

                            'Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "2"
                            'Exit Select

                            Terminal.IBMKEY.InputSeq.Override = True

                            Exit Select

                        Case 51

                            'Simulator Clear Key

                            If Terminal.IBMKEY.InputSeq.Numerics = "" Then
                                Terminal.IBMKEY.InputSeq.ClearOut = True
                                Exit Select
                            End If

                            Terminal.IBMKEY.InputSeq.Numerics = ""
                            Terminal.Display.DisplayData(Buffer1, Buffer2)

                            Exit Select

                        Case 52

                            Terminal.IBMKEY.InputSeq.SignOnOFF = True
                            Exit Select

                        Case 53

                            Terminal.IBMKEY.InputSeq.Enter = True
                            Exit Select


                        Case Keys.N1

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "1"

                            Exit Select

                        Case Keys.N2

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "2"

                            Exit Select

                        Case Keys.N3

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "3"

                            Exit Select

                        Case Keys.N4

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "4"

                            Exit Select

                        Case Keys.N5

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "5"

                            Exit Select

                        Case Keys.N6

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "6"

                            Exit Select

                        Case Keys.N7

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "7"

                            Exit Select

                        Case Keys.N8

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "8"

                            Exit Select

                        Case Keys.N9

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "9"

                            Exit Select

                        Case Keys.Zero

                            Terminal.IBMKEY.InputSeq.Numerics = Terminal.IBMKEY.InputSeq.Numerics + "0"

                            Exit Select

                        Case Keys.Enter

                            'Enter Key

                            Terminal.IBMKEY.InputSeq.Enter = True

                            Exit Select


                        Case Keys.SignOnOff

                            Terminal.IBMKEY.InputSeq.SignOnOFF = True

                            Exit Select

                        Case Keys.Clear

                            'Clear Key

                            If Terminal.IBMKEY.InputSeq.Numerics = "" Then
                                Terminal.IBMKEY.InputSeq.ClearOut = True
                                Exit Select
                            End If

                            Terminal.IBMKEY.InputSeq.Numerics = ""
                            Terminal.Display.DisplayData(Buffer1, Buffer2)

                            Exit Select

                        Case Keys.Override

                            Terminal.IBMKEY.InputSeq.Override = True

                            Exit Select

                        Case Keys.Ctrl

                            'Control Key

                        Case Keys.Qty

                            Terminal.IBMKEY.InputSeq.Quantity = True

                        Case Keys.Slash

                            Terminal.IBMKEY.InputSeq.Slash = True

                        Case Keys.PriceOverride

                            Terminal.IBMKEY.InputSeq.PriceOverride = True


                        Case Else

                            Terminal.Tone.CustomBeep(False, 100)

                            Exit Select

                    End Select

                    If Terminal.IBMKEY.InputSeq.Slash = True Then

                        Terminal.Display.DisplayDataL2(Terminal.IBMKEY.InputSeq.Numerics + "/")

                    Else

                        Terminal.Display.DisplayDataL2(Terminal.IBMKEY.InputSeq.Numerics)

                    End If



                    'Terminal.Tone.QuickBeep()

                    Terminal.IBMKEY.InputSeq.UnprocessedKeypress = -1

                End If

            End While

            Terminal.IBMKEY.WaitLightOn()
            Return Terminal.IBMKEY.InputSeq

        End Function

    End Class

#End Region

#Region "KeySeq Class"

    Public Class KeySeq

        Public UnprocessedKeypress As Integer
        Public Numerics As String

        Public Aux_Data As String

        Public SignOnOFF As Boolean
        Public Override As Boolean

        Public Quantity As Boolean
        Public Slash As Boolean
        Public PriceOverride As Boolean


        Public Enter As Boolean
        Public ClearOut As Boolean
        Public WaitingKey As Boolean

        Public Sub New()

            UnprocessedKeypress = New Int32
            Numerics = ""

            SignOnOFF = False
            Override = False

            Quantity = False
            Slash = False
            PriceOverride = False

            Enter = False
            ClearOut = False
            WaitingKey = False

        End Sub



    End Class

#End Region


End Namespace
