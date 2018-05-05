
Namespace CBJ_TERMINAL_SYSTEM

    Public Class Procedures

        Public Shared ProcedureStateStatus As ProcedureState = ProcedureState.Inactive

        Public Enum ProcedureState

            Active
            Inactive

        End Enum

        Public Shared Sub CustomerCheckout()

            '// TODO //

            '1 - Check Operator Permission -- DONE!

            '2 - Cash Drawer Must Be Closed To Begin Transaction -- DONE!

            '4 - Print Heading And Store Data Line

            '3 - Assign Transaction Number

            'Begin Transaction

            While True

                Dim InSeq As KeySeq = TerminalSystem.Key_Data_Input()

                DataInput1 = InSeq.Numerics

                If InSeq.Enter Then

                    ProcedureStateStatus = ProcedureState.Active

                    If Not DataInput1 = "" Then

                        ' TerminalSystem.LookupItem(DataInput1)

                    Else

                        Terminal.Display.DisplayData("DATA MISSING", "PRESS CLEAR")
                        Terminal.IBMKEY.ClearToCont()
                        Terminal.Display.DisplayData("ENTER ITEM")

                    End If

                ElseIf InSeq.SignOnOFF Then

                    'Attempt Sign Off Inside Procedure
                    'Check If Allowed To Exit Procedure

                    If ProcedureStateStatus = ProcedureState.Active Then

                        'Transaction Must Be Ended or Voided

                        Terminal.Display.DisplayData("PROCEDURE NOT", "AVAILABLE NOW")
                        Terminal.IBMKEY.ClearToCont()

                    Else

                        ' SignOFF()

                    End If

                End If

            End While

        End Sub

        Public Shared Sub SignOFFknb()

            'When the operator signs off: 
            '1. ******CLOSED****** appears. 
            '2. The store data line prints on the Transaction Summary Journal. 
            '3. The cash drawer opens. The cash drawer can remain open without
            '   a Tone sounding until another sign-on occurs

            'The Sign Off Procedure
            'Four Types Of Sign Off's
            'Standard, Special, Forced and Automatic

            Terminal.Display.DisplayData("REPEAT SIGN ON/OFF", "KEY OR PRESS CLEAR")

            While True

                Dim NewKey As Integer = Utilites.GetOneKey()

                Select Case NewKey

                    Case Keys.SignOnOff

                        'Cash Drawer Must Be Closed For Sign OFF To Occur
                        If Terminal.Drawer.CashDrawer.DrawerOpened Then

                            Terminal.Display.DisplayData("CLOSE THE CASH", "DRAWER")

                            Terminal.Drawer.CashDrawer.WaitForDrawerClose(5000, 1000, 1000, 1000)

                        End If

                        TerminalAuthentication.SignON.S = New TerminalAuthentication.SignON


                    Case Keys.Clear

                        Exit While

                End Select

            End While

        End Sub

    End Class

End Namespace
