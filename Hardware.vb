
Namespace CBJ_TERMINAL_SYSTEM

#Region "Keyboard Class"

    Public Class Keyboard

        Public WithEvents IBM50KEY As PosKeyboard
        Public GO As Boolean
        Public InputSeq As KeySeq = New KeySeq()
        Dim Buffer1 As String = ""
        Dim Buffer2 As String = ""

        Public Sub New()

            Dim Explorer As New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.PosKeyboard, "PosKeyboard")

            IBM50KEY = Explorer.CreateInstance(Device)

                IBM50KEY.Open()
                IBM50KEY.Claim(5000)
                IBM50KEY.DeviceEnabled = True
            IBM50KEY.DataEventEnabled = True


        End Sub

        Public Sub ClearToCont()

            ' Simple Method That Has The User Press The CLEAR Key Before It Will Return. 
            ' Used For Uninteractive Messages.

            Terminal.Tone.Beep()

            GO = True

            While GO
                '----------------------------'Keys.Clear
                If Utilites.GetOneKey() = Keys.Clear Then

                    GO = False

                Else

                    Terminal.Tone.CustomBeep(False, 75)

                End If

            End While

            WaitLightOn()

        End Sub

        Public Function Input(AllowFunctionKeys As Boolean) As KeySeq

            Terminal.IBMKEY.WaitLightOff()

            InputSeq = New KeySeq
            Buffer1 = Terminal.Display.ScreenBuffer1
            Buffer2 = Terminal.Display.ScreenBuffer2

            Dim Aux_Data As String = ""

            While InputSeq.Enter = False AndAlso InputSeq.ClearOut = False

                If Aux_Data <> "" Then
                    Terminal.Display.DisplayDataL2(Aux_Data)
                Else
                    Terminal.Display.DisplayDataL2(Buffer2)
                End If

                Select Case Utilites.GetOneKey()

                    Case Keys.Zero

                        ' Numpad 0
                        Aux_Data = Aux_Data & Convert.ToString("0")
                        Exit Select

                    Case Keys.N1
                        ' Numpad 1
                        Aux_Data = Aux_Data & Convert.ToString("1")
                        Exit Select
                    Case Keys.N2
                        ' Numpad 2
                        Aux_Data = Aux_Data & Convert.ToString("2")
                        Exit Select
                    Case Keys.N3
                        ' Numpad 3
                        Aux_Data = Aux_Data & Convert.ToString("3")
                        Exit Select
                    Case Keys.N4
                        ' Numpad 4
                        Aux_Data = Aux_Data & Convert.ToString("4")
                        Exit Select
                    Case Keys.N5
                        ' Numpad 5
                        Aux_Data = Aux_Data & Convert.ToString("5")
                        Exit Select
                    Case Keys.N6
                        ' Numpad 6
                        Aux_Data = Aux_Data & Convert.ToString("6")
                        Exit Select
                    Case Keys.N7
                        ' Numpad 7
                        Aux_Data = Aux_Data & Convert.ToString("7")
                        Exit Select
                    Case Keys.N8
                        ' Numpad 8
                        Aux_Data = Aux_Data & Convert.ToString("8")
                        Exit Select
                    Case Keys.N9
                        ' Numpad 9
                        Aux_Data = Aux_Data & Convert.ToString("9")
                        Exit Select


                    Case Keys.Clear
                        ' CLEAR Key
                        If Aux_Data = "" AndAlso InputSeq.Aux_Data = "" Then
                            InputSeq.ClearOut = True
                            Exit Select
                        End If
                        Aux_Data = ""
                        Terminal.Display.DisplayData(Buffer1, Buffer2)
                        Exit Select

                    Case Keys.Enter
                        ' ENTER Key
                        InputSeq.Numerics = Aux_Data
                        InputSeq.Enter = True
                        Exit Select

                        'Case CInt(Keys.Backspace)
                        ' Backspace
                        ' If Aux_Data <> "" Then
                        'Aux_Data = Aux_Data.Remove(Aux_Data.Length - 1)
                        ' End If
                        ' Exit Select

                    Case Else

                        Exit Select

                End Select

            End While

            Return InputSeq

        End Function

        Public Function Input() As KeySeq

            Return Input(False)

        End Function

        Private Sub IBM50KEY_DataEvent(sender As Object, e As DataEventArgs) Handles IBM50KEY.DataEvent

            InputSeq.UnprocessedKeypress = IBM50KEY.PosKeyData

            InputSeq.WaitingKey = True

            IBM50KEY.DataEventEnabled = True

        End Sub

        Public Sub WaitLightOn()

            IBM50KEY.DirectIO(201, 1, Nothing)

        End Sub

        Public Sub OfflineLightOn()

            IBM50KEY.DirectIO(201, 2, Nothing)

        End Sub

        Public Sub MsgLightOn()

            IBM50KEY.DirectIO(201, 4, Nothing)

        End Sub

        Public Sub OtherLightOn()

            IBM50KEY.DirectIO(201, 8, Nothing)

        End Sub

        Public Sub WaitLightOff()

            IBM50KEY.DirectIO(202, 1, Nothing)

        End Sub

        Public Sub OfflineLightOff()

            IBM50KEY.DirectIO(202, 2, Nothing)

        End Sub

        Public Sub MsgLightOff()

            IBM50KEY.DirectIO(202, 4, Nothing)

        End Sub

        Public Sub OtherLightOff()

            IBM50KEY.DirectIO(202, 8, Nothing)

        End Sub

    End Class

#End Region

#Region "Cash Drawer Class"

    Public Class Drawer

        Public CashDrawer As CashDrawer

        Public Sub New()

            Dim Explorer As PosExplorer = New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.CashDrawer, "CashDrawer")

            CashDrawer = Explorer.CreateInstance(Device)

            CashDrawer.Open()
            CashDrawer.Claim(5000)
            CashDrawer.DeviceEnabled = True

        End Sub

        Public Sub Close()

            CashDrawer.DeviceEnabled = False
            CashDrawer.Release()
            CashDrawer.Close()

        End Sub

        Public Sub OpenDrawer()

            CashDrawer.OpenDrawer()

            Terminal.Display.DisplayData("CLOSE CASH", "DRAWER")

            CashDrawer.WaitForDrawerClose(5000, 1000, 1000, 1000)


        End Sub

    End Class

#End Region

#Region "Line Display Class"

    Public Class Display

        Public LineDisplay1 As LineDisplay
        Public LineDisplay2 As LineDisplay
        Public ScreenBuffer1 As String = ""
        Public ScreenBuffer2 As String
        Public UseDisp2 As Boolean
        Public UseVirtualDisplay As Boolean = True

        Public Property UseVirtualDisp As Boolean

            Get
                Return UseVirtualDisplay
            End Get

            Set

                UseVirtualDisplay = Value

                If UseVirtualDisplay Then
                    TerminalWindow.DrawVirtualDisplay()
                Else
                    TerminalWindow.ResetWindow(True)
                End If

            End Set

        End Property

        Public Sub New()

            Dim Explorer As PosExplorer = New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.LineDisplay, "LineDisplayA")

            LineDisplay1 = Explorer.CreateInstance(Device)

            LineDisplay1.Open()
            LineDisplay1.Claim(5000)
            LineDisplay1.DeviceEnabled = True


            Try

                UseDisp2 = True

                Dim Device2 As DeviceInfo = Explorer.GetDevice(DeviceType.LineDisplay, "LineDisplayB")

                LineDisplay2 = Explorer.CreateInstance(Device2)

                LineDisplay2.Open()
                LineDisplay2.Claim(1000)
                LineDisplay2.DeviceEnabled = True

            Catch

                UseDisp2 = False

            End Try

        End Sub

        Public Sub Display(Line1 As String)

            Display(Line1, "", 0, False)

        End Sub

        Public Sub Display(Line1 As String, Line2 As String)

            Display(Line1, Line2, 0, False)

        End Sub

        Public Sub Display(Line1 As String, Line2 As String, Display As Integer, L2LeftJustified As Boolean)

            '//Display -  0 = Both, 1 = Primary Only, 2 = Secondary Only

            Dim UsePrimary As Boolean = True
            Dim UseSecondary As Boolean = True

            Select Case Display

                Case 0
                    UsePrimary = True
                    UseSecondary = True
                    Exit Select
                Case 1
                    UsePrimary = True
                    UseSecondary = False
                    Exit Select
                Case 2
                    UsePrimary = False
                    UseSecondary = True
                    Exit Select

            End Select

            '// Check To Prevent Unneeded Redrawing of Display
            If ScreenBuffer1 <> Line1 OrElse ScreenBuffer2 <> Line2 Then

                If Line1.Length > 20 Then Line1 = Line1.Substring(0, 20) '// Truncates String That Is Too Long  
                If Line2.Length > 20 Then Line2 = Line2.Substring(0, 20)
                ScreenBuffer1 = Line1
                ScreenBuffer2 = Line2

                Dim DisplayLine1 As String
                Dim DisplayLine2 As String

                DisplayLine1 = Line1 + Utilites.GetBlankSpace(Line1.Length)

                If L2LeftJustified Then

                    DisplayLine2 = Utilites.GetBlankSpace(Line2.Length) + Line2

                Else

                    DisplayLine2 = Line2 + Utilites.GetBlankSpace(Line2.Length)

                End If


                If UsePrimary Then

                    LineDisplay1.DisplayTextAt(0, 0, DisplayLine1)
                    LineDisplay1.DisplayTextAt(1, 0, DisplayLine2)

                End If

                'If UseSecondary & UseDisp2 Then

                '// Does Not Send To Display 2 If Disabled In Settings

                ' Disp2.DisplayTextAt(0, 0, DisplayLine1)
                'Disp2.DisplayTextAt(1, 0, DisplayLine2)

                '' End If

                If UseVirtualDisplay Then

                    '// This Block Is Used To Draw The Virtual Display If It Is Enabled.

                    TerminalWindow.UpdateVirtualDisplay(DisplayLine1, DisplayLine2)

                End If

            End If

        End Sub

        Public Sub DisplayData(Line1 As String)

            '// Used To Display A Single Line Of Text On The Top Row Of The Display, 
            '// Max Length Is 20 Chars. The Second Line Is Blanked If This Is Used.

            If ScreenBuffer1 <> Line1 Then

                ' Check To Prevent Unneeded Redrawing Of Display

                ' This Will Truncate Any String Too Long For The Display
                If Line1.Length > 20 Then Line1 = Line1.Substring(0, 20)

                ' The Screens Contents Are Stored In A Public String So Outside Methods Can Reference Them.
                ScreenBuffer1 = Line1

                ' The Buffer String For The Second Line Is Empty
                ScreenBuffer2 = ""

                Dim Space As String = Utilites.GetBlankSpace(Line1.Length)


                LineDisplay1.DisplayTextAt(0, 0, Line1 & Space)
                LineDisplay1.DisplayTextAt(1, 0, "                    ")

                If UseDisp2 Then
                    LineDisplay2.DisplayTextAt(0, 0, Line1 & Space)
                    LineDisplay2.DisplayTextAt(1, 0, "                    ")
                End If

                If UseVirtualDisplay Then

                    ' This Block Is Used To Draw The Virtual Display If It Is Enabled.
                    TerminalWindow.UpdateVirtualDisplay(Line1 & Space, "                    ")

                End If

            End If

            Return

        End Sub

        Public Sub DisplayData(Line1 As String, Line2 As String)

            ' Used To Display Two Lines Of Text. The Two Lines Are Sent As Seperate Strings. Max Length Of Either Is 20 Chars.

            ' Check To Prevent Unneeded Redrawing Of Display
            If ScreenBuffer1 <> Line1 OrElse ScreenBuffer2 <> Line2 Then

                ' This Will Truncate Any String Too Long For The Display
                If Line1.Length > 20 Then Line1 = Line1.Substring(0, 20)
                If Line2.Length > 20 Then Line2 = Line2.Substring(0, 20)

                ScreenBuffer1 = Line1
                ScreenBuffer2 = Line2

                Dim Space As String = Utilites.GetBlankSpace(Line1.Length)
                Dim Space2 As String = Utilites.GetBlankSpace(Line2.Length)
                LineDisplay1.DisplayTextAt(0, 0, Line1 & Space)
                LineDisplay1.DisplayTextAt(1, 0, Line2 & Space2)

                If UseDisp2 Then
                    LineDisplay2.DisplayTextAt(0, 0, Line1 & Space)
                    LineDisplay2.DisplayTextAt(1, 0, Line2 & Space2)
                End If

                If UseVirtualDisplay Then

                    ' This Block Is Used To Draw The Virtual Display If It Is Enabled.
                    TerminalWindow.UpdateVirtualDisplay(Line1 & Space, Line2 & Space2)

                End If

            End If

            Return

        End Sub

        Public Sub DisplayDataL2(Line2 As String)

            ' Used To Display A Line Of Text On The Bottom Line. 
            ' THE TOP LINE Is Not BLANKED If THIS Is USED. 
            ' Contents Of Top Line Will Remain The Same, And Only The Bottom Will Change.

            ' Check To Prevent Unneeded Redrawing Of Display
            If ScreenBuffer2 <> Line2 Then

                If Line2.Length > 20 Then Line2 = Line2.Substring(0, 20)

                ScreenBuffer2 = Line2

                Dim Space2 As String = Utilites.GetBlankSpace(Line2.Length)
                LineDisplay1.DisplayTextAt(1, 0, Line2 & Space2)

                If UseDisp2 Then
                    LineDisplay2.DisplayTextAt(1, 0, Line2 & Space2)
                End If

                If UseVirtualDisplay Then

                    TerminalWindow.UpdateVirtualDisplayL2(Line2 & Space2)

                End If

            End If

            Return

        End Sub

        Public Sub DisplayData_Primary(Line1 As String)

            ' Check To Prevent Unneeded Redrawing Of Display
            If ScreenBuffer1 <> Line1 Then

                ' This Will Truncate Any String Too Long For The Display
                If Line1.Length > 20 Then Line1 = Line1.Substring(0, 20)

                ' The Screens Contents Are Stored In A Public String So Outside Methods Can Reference Them.
                ScreenBuffer1 = Line1

                ' The Buffer String For The Second Line Is Empty
                ScreenBuffer2 = ""

                Dim Space As String = Utilites.GetBlankSpace(Line1.Length)
                LineDisplay1.DisplayTextAt(0, 0, Line1 & Space)
                LineDisplay1.DisplayTextAt(1, 0, "                    ")

                If UseVirtualDisplay Then

                    ' This Block Is Used To Draw The Virtual Display If It Is Enabled.
                    TerminalWindow.UpdateVirtualDisplay(Line1 & Space, "                    ")

                End If

            End If

            Return

        End Sub

        Public Sub DisplayData_Primary(Line1 As String, Line2 As String)

            ' Sends Data Only To The Primary Display, Even If The Secondary One Is Enabled.
            ' Contents Of The Secondary Will Remain The Same.

            ' Check To Prevent Unneeded Redrawing Of Display
            If ScreenBuffer1 <> Line1 OrElse ScreenBuffer2 <> Line2 Then

                ' This Will Truncate Any String Too Long For The Display
                If Line1.Length > 20 Then
                    Line1 = Line1.Substring(0, 20)
                End If

                If Line2.Length > 20 Then
                    Line2 = Line2.Substring(0, 20)
                End If

                ScreenBuffer1 = Line1
                ScreenBuffer2 = Line2

                Dim Space As String = Utilites.GetBlankSpace(Line1.Length)
                Dim Space2 As String = Utilites.GetBlankSpace(Line2.Length)
                LineDisplay1.DisplayTextAt(0, 0, Line1 & Space)
                LineDisplay1.DisplayTextAt(1, 0, Line2 & Space2)

                If UseVirtualDisplay Then

                    ' This Block Is Used To Draw The Virtual Display If It Is Enabled.
                    TerminalWindow.UpdateVirtualDisplay(Line1 & Space, Line2 & Space2)

                End If

            End If

            Return

        End Sub

        Public Sub DisplayDataL2_Primary(Line2 As String)

            ' Check To Prevent Unneeded Redrawing Of Display
            If ScreenBuffer2 <> Line2 Then

                If Line2.Length > 20 Then
                    Line2 = Line2.Substring(0, 20)
                End If

                ScreenBuffer2 = Line2

                Dim Space2 As String = Utilites.GetBlankSpace(Line2.Length)
                LineDisplay1.DisplayTextAt(1, 0, Line2 & Space2)

                If UseVirtualDisplay Then

                    TerminalWindow.UpdateVirtualDisplayL2(Line2 & Space2)

                End If

            End If

            Return

        End Sub

        Public Sub DisplayDataL2_LeftJustified(Line2 As String)

            If Line2.Length <= 20 Then

                Dim Spacer As String = ""
                Dim Spaces As Integer = 20 - Line2.Length

                For x As Integer = 1 To Spaces
                    Spacer = Spacer & Convert.ToString(" ")
                Next

                DisplayDataL2(Spacer & Line2)

            End If

        End Sub

        Public Sub DisplayDataL2_LeftRightJustified(Data1 As String, Data2 As String)

            Dim Spacer As String = ""
            Dim Spaces As Integer = 20 - (Data1.Length + Data2.Length)

            For x As Integer = 1 To Spaces
                Spacer = Spacer & Convert.ToString(" ")
            Next

            DisplayDataL2(Data1 & Spacer & Data2)

        End Sub

        Public Sub DisplayData_LeftJustified(Line1 As String)

            If Line1.Length <= 20 Then

                Dim Spacer As String = ""
                Dim Spaces As Integer = 20 - Line1.Length

                For x As Integer = 1 To Spaces
                    Spacer = Spacer & Convert.ToString(" ")
                Next

                DisplayData(Spacer & Line1)

            End If

        End Sub

        Public Sub DisplayAt(Line As String, Vertical As Integer, Horizontal As Integer)

            LineDisplay1.DisplayTextAt(Vertical, Horizontal, Line)

            If UseDisp2 Then
                LineDisplay2.DisplayTextAt(Vertical, Horizontal, Line)
            End If

        End Sub

        Public Sub DisplayAt(Line As String, Vertical As Integer, Horizontal As Integer, Display As Integer)

            ' 0 = Both, 1 = Primary Only, 2 = Secondary Only

            If Display = 0 Then
                DisplayAt(Line, Vertical, Horizontal)
            End If

            If Display = 1 Then
                LineDisplay1.DisplayTextAt(Vertical, Horizontal, Line)
            End If

            If Display = 2 Then
                LineDisplay2.DisplayTextAt(Vertical, Horizontal, Line)
            End If

        End Sub

    End Class

#End Region

#Region "Tone Indicator Class"

    Public Class ToneIND

        Public Tone As ToneIndicator

        ' Boolean If Set True Will Stop The TI From Generating Any Noise
        Public Mute As Boolean

        ' If Set True The TI Will Use High Volume, If False, It Will Use Low Volume
        Private M_Highvolume As Boolean

        Public Property HighVolume() As Boolean

            Get
                Return M_Highvolume
            End Get

            Set
                M_Highvolume = Value

                If M_Highvolume = True Then

                    ' Sets Tone Volumes To 100 When Bool Is Written As True
                    Tone.Tone1Volume = 100
                    Tone.Tone2Volume = 100
                Else
                    Tone.Tone1Volume = 20
                    Tone.Tone2Volume = 20
                End If

            End Set

        End Property

        Public Sub New()

            Try

                Dim Explorer As New PosExplorer()

                Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.ToneIndicator, "ToneIndicator")

                Tone = Explorer.CreateInstance(Device)

                Tone.Open()
                Tone.DeviceEnabled = True
                Tone.AsyncMode = False

                ' Default State Is To Use High Volume, This Can Be Changed Within The System Later
                HighVolume = True

                Mute = False

            Catch e As Exception

            End Try

        End Sub

        Public Sub BeepTest()

            ' Tests The Tone Indicator. Should Make A Short Low And A Short High Beep

            If Tone.State = ControlState.Idle Then
                Tone.Tone1Duration = 100
                Tone.Tone1Pitch = 500
                Tone.Tone2Duration = 100
                Tone.Tone2Pitch = 2000
                Tone.InterToneWait = 1

                If Mute = False Then
                    Tone.Sound(1, 0)
                End If

            End If

        End Sub

        Public Sub QuickBeep()

            Try

                If Tone.State = ControlState.Idle Then

                    Tone.Tone1Duration = 50
                    Tone.Tone1Pitch = 2000
                    Tone.Tone2Duration = 0

                    If Mute = False Then

                        Tone.Sound(1, 0)

                    End If

                End If

            Catch ex As Exception

            End Try

        End Sub

        Public Sub Beep()

            ' Longer, Lower Pitched Beep, Good For Error Prompts.

            Try

                If Tone.State = ControlState.Idle Then
                    Tone.Tone1Duration = 250
                    Tone.Tone1Pitch = 500

                    If Mute = False Then
                        Tone.Sound(1, 0)
                    End If

                End If

            Catch ex As Exception

            End Try

        End Sub

        Public Sub CustomBeep(HighPitch As Boolean, Duration As Integer)

            Try

                If Tone.State = ControlState.Idle Then

                    If HighPitch Then

                        Tone.Tone1Pitch = 2000

                    Else

                        Tone.Tone1Pitch = 500
                        Tone.Tone1Duration = Duration

                        If Mute = False Then Tone.Sound(1, 0)

                    End If

                End If

            Catch ex As Exception

            End Try

        End Sub

    End Class

#End Region

#Region "Msr Class"

    Public Class MsrReader

        Public WithEvents Msr As Msr

        Public Sub New()

            Dim Explorer As PosExplorer = New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.Msr, "Msr")

            Msr = Explorer.CreateInstance(Device)

            Msr.Open()
            Msr.Claim(5000)
            Msr.DeviceEnabled = True
            Msr.DataEventEnabled = True
            Msr.DecodeData = True

        End Sub

        Private Sub Msr_DataEvent(sender As Object, e As DataEventArgs) Handles Msr.DataEvent


            Console.WriteLine(Environment.NewLine + "ACCOUNT NUMBER: " + Msr.AccountNumber)


            Msr.DataEventEnabled = True


        End Sub

        Private Sub Msr_StatusUpdateEvent(sender As Object, e As StatusUpdateEventArgs) Handles Msr.StatusUpdateEvent

            MsgBox("Status")

        End Sub

    End Class

#End Region

#Region "Printer Class"

    Public Class Printer

        Public Station As PrinterStation
        Public Printer As PosPrinter

        Public Sub New()

            Dim Explorer As PosExplorer = New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(My.Settings.ReceiptPrinter, "PosPrinter")

            Printer = Explorer.CreateInstance(Device)

            Printer.Open()
            Printer.Claim(5000)
            Printer.PowerNotify = PowerNotification.Enabled
            Printer.DeviceEnabled = True
            Printer.AsyncMode = True

            Station = PrinterStation.Receipt

        End Sub

        Public Sub Print(Job As PrintJob)

            Try

                Terminal.IBMKEY.WaitLightOn()

                If CheckPrinterReady() Then

                    If Job.Station = PrinterStation.Slip Then

                        'Begins The Insertion Operation For Slip Printing

                        Terminal.Display.DisplayData("PRINTER READY", "INSERT FORM")
                        Printer.BeginInsertion(5000)
                        Printer.EndInsertion()
                        Printer.AsyncMode = False
                        Terminal.Display.DisplayData("", "PRINTING...")

                    End If

                    For Each Line As String In Job.Document

                        Try

                            Printer.PrintNormal(Job.Station, Line)

                        Catch ex As PosControlException

                            If ex.ErrorCode = ErrorCode.Extended AndAlso ex.ErrorCodeExtended = 204 Then

                                'Paper Ran Out During Print

                                TerminalSystem.ProgramError(6, False)

                                If Not ReloadAndResume() Then

                                    Return

                                End If

                            End If

                        End Try

                    Next

                    If Job.CutPaperWhenFinished Then

                        'Feeds 6 Lines And Cuts Paper

                        Printer.PrintNormal(Job.Station, ChrW(27) & "|6fP")

                    End If

                    If Job.Station = PrinterStation.Slip AndAlso Job.EjectDocument Then

                        'Concludes Slip Printing

                        Terminal.Display.DisplayData("PRINTING COMPLETE", "REMOVE FORM")

                        Try

                            Printer.BeginRemoval(100)

                            ' Ejects Form

                            Printer.EndRemoval()

                        Catch

                        End Try

                        Printer.AsyncMode = True

                    End If

                End If

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                TerminalSystem.ProgramError(3, True)

            End Try

        End Sub

        Public Sub PrintDirect(Line As String, Station As PrinterStation)

            Printer.PrintNormal(Station, Line + vbLf)

        End Sub

        Public Sub Feed(Lines As Integer)

            Printer.PrintNormal(PrinterStation.Receipt, ChrW(27) & "|" + Lines + "lF")

        End Sub

        Public Sub ReverseFeed(Lines As Integer)

            Printer.PrintNormal(PrinterStation.Receipt, ChrW(27) & "|" + Lines + "rF")

        End Sub

        Public Sub FeedAndCut()

            ' Feeds 6 Lines And Cuts Paper

            Printer.PrintNormal(PrinterStation.Receipt, ChrW(27) & "|6fP")

        End Sub

        Public Function ReloadAndResume() As Boolean

            Terminal.Display.DisplayData("ENTER TO PROCEED", "CLEAR TO ABORT")

            If Not Utilites.ClearOrContinue() Then

                Return False

            End If

            Terminal.Display.DisplayData("", "INSERT FORM")
            Printer.BeginInsertion(100000)
            Printer.EndInsertion()

            Return True

        End Function

        Public Sub PrinterTest()

            Terminal.Display.DisplayData("EXECUTE", "PRINTER TEST?")

            If Terminal.IBMKEY.input().ClearOut Then

                TerminalSystem.Menu()

            End If

            Dim TestJob As PrintJob = New PrintJob("PRINTER TEST", "")

            TestJob.Station = SelectPrinterStation()

            Dim Station As String = "ERROR"

            Select Case TestJob.Station

                Case PrinterStation.Receipt

                    Station = "THERMAL TAPE"
                    Exit Select

                Case PrinterStation.Slip

                    Station = "DOCUMENT PRINTER"
                    Exit Select

            End Select

            Try
                TestJob.CreateHeader()
            Catch ex As Exception

            End Try

            TestJob.AddLine(" ")
            TestJob.AddLine("PRINTER: IBM 4610 SUREMARK - TI4")
            TestJob.AddLine("PRINTER S/N: N/A")
            TestJob.AddLine(" ")
            TestJob.AddLine(Convert.ToString("STATION: ") & Station)
            TestJob.AddLine(" ")
            TestJob.AddLine(" ")
            TestJob.AddLine("ABCDEFGIJKLMOPQRSTUVWXYZ 0123456789")
            TestJob.AddLine("abcdefghijklmnopqustuvwxyz")
            TestJob.AddLine("!@#$%^&*()_+-=[]\{}|;':"",./<>?")
            TestJob.AddLine(" ")
            TestJob.AddLine("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG")
            TestJob.AddLine("the quick brown fox jumps over the lazy dog")
            TestJob.AddLine(" ")
            TestJob.AddLineBold("BOLDED TEXT")
            TestJob.AddLineFormat("RIGHT JUSTIFIED TEXT", False, False, False, False, 2)
            TestJob.AddLineFormat("TALL TEXT", False, True, False, False, 0)
            TestJob.AddLineFormat("WIDE TEXT", False, False, True, False, 0)
            TestJob.AddLineFormat("BIG & CENTER", False, False, False, True, 1)
            TestJob.AddLine(" ")
            TestJob.AddLine("        *** OPERATION COMPLETE ***        ")

            Print(TestJob)

            TerminalSystem.Menu()

        End Sub

        Private Function CheckPrinterReady() As Boolean

            Dim PrinterOK As Boolean = False

            While PrinterOK = False

                If Printer.RecEmpty OrElse Printer.JrnEmpty OrElse Printer.CoverOpen Then

                    TerminalSystem.ProgramError(4, False)
                    Terminal.Display.DisplayData("ENTER TO RETRY OR", "CLEAR TO CANCEL")

                    If Utilites.ClearOrContinue() = False Then

                        Return False

                    End If

                Else

                    PrinterOK = True

                End If

            End While

            Return True

        End Function

        Public Function SelectPrinterStation() As PrinterStation
            Dim Options As New List(Of String) From {
                "1-THERMAL PRINTER",
                "2-SLIP PRINTER"
            }

            ' Select Case Utilites.ListMenu("SELECT STATION", Options)

            'Case 0
            'Return PrinterStation.Receipt

            'Case 1
            'Return PrinterStation.Slip

            'Case 2
            'Return PrinterStation.None

            'Case -1

            'TerminalSystem.Menu()
            'Exit Select

            'End Select

            Return PrinterStation.Receipt

        End Function

    End Class

#End Region

#Region "PrintJob Class"

    Public Class PrintJob

        Public Title As String
        Public Program As String
        Public Station As PrinterStation
        Public CutPaperWhenFinished As Boolean
        Public EjectDocument As Boolean
        Public Document As List(Of String)
        Public UnformattedDocument As List(Of String)

        Public Sub New(Name As String, Program As String)

            Document = New List(Of String)
            Station = PrinterStation.Receipt
            CutPaperWhenFinished = True

        End Sub

        Public Sub AddLine(Line As String)

            Document.Add(Line & vbLf)

        End Sub

        Public Sub AddLineBold(Line As String)

            Document.Add(Chr(27) & "|bC" & Line + vbLf)
            UnformattedDocument.Add(Line)

        End Sub

        Public Sub AddLineFormat(Line As String, Bold As Boolean, DoubleHeight As Boolean, DoubleWidth As Boolean, DoubleSize As Boolean, Justification As Integer)

            ' 0 = Left, 1 = Center, 2 = Right

            Dim Prefix As String = ""

            If Bold Then

                Prefix = Prefix & Convert.ToString(Chr(27) & "|bC")

            End If

            If DoubleSize Then

                Prefix = Prefix & Convert.ToString(Chr(27) & "|4C")
                DoubleHeight = False
                DoubleWidth = False

            End If

            If DoubleHeight Then

                Prefix = Prefix & Convert.ToString(Chr(27) & "|3C")

            End If

            If DoubleWidth Then

                Prefix = Prefix & Convert.ToString(Chr(27) & "|2C")

            End If

            Select Case Justification

                Case 0

                    Exit Select

                Case 1

                    Prefix = Prefix & Convert.ToString(Chr(27) & "|cA")
                    Exit Select

                Case 2

                    Prefix = Prefix & Convert.ToString(Chr(27) & "|rA")
                    Exit Select

                Case Else

                    Exit Select

            End Select

            Document.Add((Prefix & Line) + vbLf)
            UnformattedDocument.Add(Line)

        End Sub

        Public Sub AddLineN(Line As String)

            ' Narrow Paper / 34 Col

            Document.Add((Convert.ToString("          ") & Line) + vbLf)

        End Sub

        Public Sub CreateHeader()

            Try

                Dim Timestamp As String = Utilites.TimeDatestamp()

                AddLine("--------------------------------------------")
                AddLine(Convert.ToString("HSDC/TS " + My.Settings.Version + Utilites.GetBlankSpace(My.Settings.Version.Length + 8, Timestamp.Length, 44)) & Timestamp)
                AddLine(Title + Utilites.GetBlankSpace(Title.Length, Program.Length, 44) + Program)
                AddLine("--------------------------------------------")

            Catch ex As Exception

            End Try

        End Sub

        Public Sub CreateMiniHeader()

            Dim Timestamp As String = Utilites.TimeDatestamp()
            AddLineN(Convert.ToString(Title + "  ") & Timestamp)

        End Sub

    End Class

#End Region

#Region "Keylock Class"

    Public Class KeyLK

        Public Keylock As Keylock

        Public Sub New()

            Dim Explorer As PosExplorer = New PosExplorer()

            Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.Keylock, "Keylock")

            Keylock = Explorer.CreateInstance(Device)

            Keylock.Open()
            Keylock.DeviceEnabled = True


        End Sub

    End Class

#End Region

#Region "Micr Class"

    Public Class MicrReader

        Public Micr As Micr
        Public Sub New()

            Try

                Dim Explorer As PosExplorer = New PosExplorer()

                Dim Device As DeviceInfo = Explorer.GetDevice(DeviceType.Micr, "Micr")

                Micr = Explorer.CreateInstance(Device)

                Micr.Open()
                Micr.Claim(5000)
                Micr.DeviceEnabled = True
                Micr.DataEventEnabled = True

            Catch ex As Exception

            End Try

        End Sub

    End Class

#End Region

#Region "Hard Totals Class"

    Public Class Totals



        Public Sub New()





        End Sub

    End Class

#End Region

#Region "Terminal Window Class"

    Public Class TerminalWindow

        ' Governs The Terminal Services Command Prompt Window

        Public Shared Sub BlankDown(Start As Integer)

            ' Blanks Console Window From Selected Line Down To The Bottom

            For Line As Integer = Start To 24
                Console.SetCursorPosition(0, Line)
                Console.Write("                                                                             ")
            Next

            Console.SetCursorPosition(0, Start)

        End Sub

        Public Shared Sub DrawHeader(Online As Boolean)

            ' Draws The HSDC Header At The Top Of The Console, Boolean Selects Weather ONLINE OR OFFLINE Is Indicated

            Console.SetCursorPosition(0, 0)
            Console.Write("--------------------------------------------------------------------------------")

            If Online = False Then
                Console.Write("  HSDC TERMINAL SERVICES -- " + "v1.5b" + vbLf)
            Else
                Console.Write("  HSDC TERMINAL SERVICES -- " + "v1.5b" + vbLf)
            End If

            Console.Write("--------------------------------------------------------------------------------" & vbLf)

        End Sub

        Public Shared Sub ResetWindow(Online As Boolean)

            ' Resets Whole Console And Redraws Header.

            Console.Clear()
            DrawHeader(False)

        End Sub

        Public Shared Sub DrawVirtualDisplay()

            ' If The Virtual Display Is Enabled, This Will Help Draw It On Screen

            ResetWindow(True)
            Console.SetCursorPosition(0, 4)
            Console.WriteLine("--------------------")
            Console.WriteLine("                    ")
            Console.WriteLine("                    ")
            Console.WriteLine("--------------------")

        End Sub

        Public Shared Sub UpdateVirtualDisplay(Line1 As String, Line2 As String)

            ' This Updates The Virtual Display With New Information. Called By The Screen Class.

            Console.SetCursorPosition(0, 5)
            Console.Write(Line1)
            Console.SetCursorPosition(0, 6)
            Console.Write(Line2)

        End Sub

        Public Shared Sub UpdateVirtualDisplayL2(Line2 As String)

            Console.SetCursorPosition(0, 6)
            Console.Write(Line2)

        End Sub

    End Class

#End Region

#Region "Utilities Class"

    Public Class Utilites

        Public Shared Function GetBlankSpace(StringLength As Integer) As String

            '// Used To Generate Blankspace For Clearing The Screen Fields

            Dim BlankLength As Integer = 20 - StringLength
            Dim Space As String = ""

            For count As Integer = 1 To BlankLength
                Space = Space + " "
            Next

            Return Space

        End Function

        Public Shared Function GetBlankSpace(StringLength As Integer, TotalSpace As Integer) As String

            '// Used To Generate Blankspace For Clearing The Printer Fields

            Dim BlankLength As Integer = TotalSpace - StringLength
            Dim Space As String = ""

            For Count As Integer = 1 To BlankLength

                Space = Space + " "

            Next

            Return Space

        End Function

        Public Shared Function GetBlankSpace(String1Length As Integer, String2Length As Integer, TotalSpace As Integer) As String

            '// Used To Generate Blankspace For Clearing The Screen Fields

            Dim BlankLength As Integer = TotalSpace - (String1Length + String2Length)
            Dim Space As String = ""

            For Count As Integer = 1 To BlankLength

                Space = Space + " "

            Next

            Return Space

        End Function

        Public Shared Function Truncate(Str As String, Limit As Integer) As String

            If Str.Length > Limit Then

                Str = Str.Remove(Limit)
                Return Str

            Else Return Str

            End If

        End Function

        Public Shared Function GetOneKey() As Integer

            Terminal.IBMKEY.InputSeq = New KeySeq()

            Terminal.IBMKEY.WaitLightOff()

            While True
                Threading.Thread.Sleep(10)
                If Terminal.IBMKEY.InputSeq.WaitingKey Then

                    Return Terminal.IBMKEY.InputSeq.UnprocessedKeypress
                End If
            End While

            Return -1

        End Function

        Public Shared Function TimeDatestamp() As String

            ' Generates A Timestamp

            Dim Value As Date = Date.Now
            Return Value.ToString("HHmm/MMddyy")

        End Function

        Public Shared Function NiceTimeAndDate() As String

            Dim value As Date = Date.Now
            Return value.ToString("MMM dd      hh:mm tt")

        End Function

        Public Shared Function ClearOrContinue() As Boolean

            ' Used For Yes OR No Prompts

            While True

                Select Case GetOneKey()
                    Case Keys.Clear

                        ' CLEAR Key
                        'Terminal.IBMKEY.WaitLightOn()
                        Return False

                    Case Keys.Enter

                        ' ENTER Key
                        'Terminal.IBMKEY.WaitLightOn()
                        Return True

                    Case Else

                        'Terminal.Tone.CustomBeep(False, 75)
                        Exit Select

                End Select

            End While

            Return False

        End Function

    End Class

#End Region

End Namespace