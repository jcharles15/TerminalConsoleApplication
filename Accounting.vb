Namespace CBJ_TERMINAL_SYSTEM

    Public Class Accounting




        Public Class ReportingPeriod
            Public Shared Period As ReportingPeriod
            Public Shared PeriodStart As DateTime
            Public Shared PeriodEnd As DateTime
            Public Shared PeriodState As PeriodType
            Public Shared Time As String
            Public Shared TerminalNo As Integer = 1
            Public Shared OpeningBalance As Double
            Public Shared DrawerNo As String

            Public Sub New()

                ' MsgBox("Creating New Reporting Period")

                PeriodStart = DateTime.Now

                Time = Format(PeriodStart, "MM/dd/yy hh:mm tt")

                'MsgBox("Period Started " + Time)

                PeriodState = PeriodType.Current


                ''TODO Database Stuff

                TerminalSystem.Menu()




            End Sub



        End Class

        Enum PeriodType

            Current = 0
            Previous = 1

        End Enum

        Shared Accounting As DataTable = GetTable()

        Shared Function GetTable() As DataTable

            Dim Table As New DataTable

            Table.Columns.Add("StartDatetime", GetType(DateTime))
            Table.Columns.Add("EndDatetime", GetType(DateTime))
            Table.Columns.Add("TerminalNo", GetType(Integer))
            Table.Columns.Add("Opening Balance", GetType(Decimal))
            Table.Columns.Add("Indicator", GetType(Integer))  '0 = Current, 1 = Previous

            Return Table

        End Function

        Public Shared Function GetCurrentPeriod() As Boolean

            'Current Period = Today's Date

            If Accounting.Rows.Count = 0 Then

                Return False

            End If

            For Each row As DataRow In Accounting.Rows

                Dim StartDate As Date = FormatDateTime(row.Item(0), DateFormat.ShortDate)
                Dim Indicator As Integer = row.Item(4)
                Dim TerminalNo As Integer = row.Item(2)


                If CompareDate(StartDate, DateTime.Today) = 2 And Indicator = PeriodType.Current And TerminalNo = ReportingPeriod.TerminalNo Then

                    MsgBox("Found Reporting Record")

                    Return True

                Else

                    MsgBox("No Open Reporting Period for Today for Terminal " + ReportingPeriod.TerminalNo.ToString)

                    Return False

                End If


            Next


            Return False


        End Function

        Public Shared Sub InitializeAccounting()

            If GetCurrentPeriod() = False Then

                'False Will Start a New Reporting Period For Terminal Accountin

                SetOpeningBalance()

                ReportingPeriod.Period = New ReportingPeriod

            End If


        End Sub

        Private Shared Sub SetOpeningBalance()

            Dim StepSeq As Integer = 0

            'Sets Up Opening Balance on Terminal.

            Terminal.Display.DisplayData("ENTER OPENING BAL")


            While True

                InSeq = TerminalSystem.Key_Data_Input()

                If InSeq.Enter Then

                    Select Case StepSeq

                        Case 0 'Collect Opening Balance

                            Terminal.Display.LineDisplay1.ClearText()

                            DataInput1 = InSeq.Numerics

                            If DataInput1 = Nothing Then

                                Terminal.Display.DisplayData("DATA ENTRY", "REQUIRED")
                                Terminal.IBMKEY.ClearToCont()
                                Terminal.Display.DisplayData("ENTER OPENING BAL")
                                Exit Select

                            End If


                            ReportingPeriod.OpeningBalance = FormatNumber(DataInput1 / 100)


                            Terminal.Display.DisplayData("ENTER DRAWER #")

                            StepSeq = 1

                            Exit Select

                        Case 1 'Collect Drawer Number

                            DataInput1 = InSeq.Numerics

                            If DataInput1 = Nothing Then

                                Terminal.Display.DisplayData("DATA ENTRY", "REQUIRED")
                                Terminal.IBMKEY.ClearToCont()
                                Terminal.Display.DisplayData("ENTER DRAWER #")
                                Exit Select

                            End If

                            ReportingPeriod.DrawerNo = DataInput1


                            Exit While

                    End Select

                End If

            End While
            'MsgBox(String.Format("{0:#,###.##}", DataInput1 / 100))

            Terminal.Drawer.OpenDrawer()


        End Sub



    End Class

End Namespace