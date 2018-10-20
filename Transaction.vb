
Namespace CBJ_TERMINAL_SYSTEM

#Region "Transaction Class"

    Public Class Transaction

        Public Shared Status As Boolean

        Private Transaction As Transaction
        Public Shared OperatorNo As Integer
        Public Shared TransNo As Integer
        Public Shared BeginTransNo As Integer = 4635
        Public Shared NumOfItems As Integer
        Public Shared Total As Decimal
        Public Shared Subtotal As Decimal
        Public Shared Tax As Decimal

        Public Shared DBItemTable As DataTable
        Public Shared TRItemTable As DataTable
        Public Shared Found As Boolean = False

        Public Sub New()

            TransNo = GetTransNo()

            TRItemTable = New DataTable

            TRItemTable.Columns.Add("ITEMCODE", GetType(String))
            TRItemTable.Columns.Add("UPC", GetType(String))
            TRItemTable.Columns.Add("QUANTITY", GetType(Integer))
            TRItemTable.Columns.Add("XPRICE", GetType(Double))
            TRItemTable.Columns.Add("DEPTNO", GetType(String))
            TRItemTable.Columns.Add("UNITPRICE", GetType(Double))
            TRItemTable.Columns.Add("SALEPRICE", GetType(Double))
            TRItemTable.Columns.Add("DISCOUNTABLE", GetType(String)) 'INDICATOR
            TRItemTable.Columns.Add("SALETYPE", GetType(Integer)) 'INDICATOR
            TRItemTable.Columns.Add("TAX", GetType(Integer))      'INDICATOR
            TRItemTable.Columns.Add("WEIGHT", GetType(Integer))   'INDICATOR
            TRItemTable.Columns.Add("PRICEMETHOD", GetType(Integer))   'INDICATOR
            TRItemTable.Columns.Add("SALEQTY", GetType(Integer))   'INDICATOR


            NumOfItems = 0
            Total = 0
            Subtotal = 0
            Tax = 0

            Status = True

        End Sub

        Public Function GetTransNo() As Integer

            'Get The Last Transaction Number

            BeginTransNo += 1

            TransNo = BeginTransNo

            Dim NewTransNo As Integer = TransNo

            Return NewTransNo

        End Function

        Public Shared Function LookupItem(ItemCode As String) As Boolean

            Dim DBAdapter As New MySqlDataAdapter("SELECT * FROM Itemlist WHERE Barcode = '" & ItemCode & "'", DBConnect)

            Dim DBDataset As New DataSet
            DBAdapter.Fill(DBDataset, "Itemlist")
            DBItemTable = DBDataset.Tables("Itemlist")

            If DBAdapter.Fill(DBDataset, "Itemlist") = 0 Then

                Terminal.Display.DisplayData("ITEM NOT FOUND")
                Terminal.IBMKEY.ClearToCont()

                Return False

            Else

                Return True

            End If

        End Function

    End Class

#End Region

    Public Class CustomerCheckoutProcedure

        Public Sub New()

            'Check if Operator is Allowed

            'OK

            'Make Sure Cash Drawer is Closed

            Checkdrawer() 'if Closed, continues

            Transaction.Status = True

            KeyMenu()


        End Sub

        Private Sub Checkdrawer()

            While True 'Loops until drawer is closed

                If Terminal.Drawer.CashDrawer.DrawerOpened Then

                    Terminal.Display.DisplayData("PLEASE CLOSE", "CASH DRAWER")
                    Terminal.IBMKEY.ClearToCont()

                Else

                    Exit While

                End If

            End While

        End Sub

        Private Shared Sub KeyMenu()

            'Clear Any Entered Data (If Possibly Entered)
            ResetEnteredData()

            Terminal.Display.DisplayData("ENTER ITEM")

            While True

                Dim InSeq As KeySeq = TerminalSystem.Key_Data_Input()

                If InSeq.SignOnOFF Then

                    'Check if Transaction was entered
                    If Transaction.Status = True Then

                        'Transaction was Started. Need to complete.
                        Terminal.Display.DisplayData("TRANSACTION MUST", "BE COMPLETED")
                        Terminal.IBMKEY.ClearToCont()

                        Transaction.Status = False

                        KeyMenu()

                    Else

                        'Sign Off Allowed

                        Select Case InSeq.Numerics

                            Case ""

                                'Signs off of Procedure and returns to main menu

                                TerminalSystem.Menu()

                                Exit Select

                            Case 1 To 9

                                Terminal.Display.DisplayData("FUNCTION NOT", "AVAILABLE")
                                Terminal.IBMKEY.ClearToCont()

                                KeyMenu()
                                ' Dim ProcedureNo As String = InSeq.Numerics

                                'ProcedureSignON(ProcedureNo)

                                Exit Select

                        End Select

                    End If

                ElseIf InSeq.Enter Then

                    Dim DataCollected As String = InSeq.Numerics

                    If Not DataCollected = "" Then

                        TerminalSystem.KeyedItemCode = DataCollected

                        ProcessEnteredItem()

                    Else

                        Terminal.Display.DisplayData("    CHECK ITEM CODE", "    DATA IS MISSING")
                        Terminal.IBMKEY.ClearToCont()
                        Terminal.Display.DisplayData("      RETRY KEYING", "     SEQUENCE")
                        Terminal.IBMKEY.ClearToCont()

                        KeyMenu()

                    End If




                ElseIf InSeq.PriceOverride Then

                    Dim DataCollected As String = InSeq.Numerics

                    If Not DataCollected = "" Then

                        TerminalSystem.PriceData = InSeq.Numerics

                        TerminalSystem.PriceDataEntered = True

                    Else

                        Terminal.Display.DisplayData("B037 CHECK PRICE", "     DATA IS MISSING")
                        Terminal.IBMKEY.ClearToCont()
                        Terminal.Display.DisplayData("      RETRY KEYING", "     SEQUENCE")
                        Terminal.IBMKEY.ClearToCont()

                        KeyMenu()

                    End If




                ElseIf InSeq.Slash Then

                    Dim DataCollected As String = InSeq.Numerics

                    If Not DataCollected = "" Then

                        TerminalSystem.SlashData = InSeq.Numerics

                        TerminalSystem.SlashDataEntered = True


                    End If




                ElseIf InSeq.Quantity Then

                    Dim DataCollected As String = InSeq.Numerics

                    If Not DataCollected = "0" Then

                        TerminalSystem.QuantityData = DataCollected
                        TerminalSystem.QuantityDataEntered = True

                    Else

                        Terminal.Display.DisplayData("B036 CHECK QUANTITY", "     CANNOT BE ZERO")
                        Terminal.IBMKEY.ClearToCont()
                        Terminal.Display.DisplayData("      RETRY KEYING", "     SEQUENCE")
                        Terminal.IBMKEY.ClearToCont()


                        KeyMenu()

                    End If




                ElseIf InSeq.ClearOut Then

                Else

                    TerminalSystem.ProgramError(7, "")

                    KeyMenu()

                End If


            End While

        End Sub


        Private Sub ProcedureSignON(ByVal ProcedureNo As String)

            Select Case ProcedureNo

                Case 3

                    'Go to Loan 

            End Select

        End Sub

        Public Shared Sub ProcessEnteredItem()

            If Transaction.LookupItem(TerminalSystem.KeyedItemCode) Then

                'An Item Record Was Found, Continue.

                For Each Row As DataRow In Transaction.DBItemTable.Rows

                    Dim ItemName As String = Row("PosDescription")
                    Dim UPC As Integer = Row("Barcode")
                    Dim UnitPrice As Decimal = Convert.ToDecimal(Row("Price"))
                    Dim PriceRequired As Integer = Row("PriceRequired")
                    Dim QuantityRequired As Integer = Row("QuantityRequired")
                    Dim WeightRequired As Integer = Row("WeightRequired")

                    Dim SaleType As Integer = Row("SaleType")
                    Dim SalePriceMethod As Integer = Row("SalePriceMethod")
                    Dim SaleGroupPrice As Decimal = Row("SaleGroupPrice")
                    Dim SaleQuantity As Integer = Row("SaleQuantity")
                    Dim SaleLimit As Integer = Row("SaleLimit")

                    Dim Quantity As Integer = Convert.ToInt32(TerminalSystem.QuantityData)
                    Dim PriceDataString As String = TerminalSystem.PriceData
                    Dim PriceMod As Decimal

                    Dim DealQuantity As Integer = Convert.ToInt32(TerminalSystem.SlashData)
                    Dim DealPrice As String = PriceMod

                    Dim XPrice As Decimal
                    Dim PriceA As Decimal

                    PriceMod = Convert.ToDecimal(PriceDataString / 100)




                    Terminal.Display.DisplayData(ItemName)





                    If TerminalSystem.QuantityDataEntered AndAlso TerminalSystem.SlashDataEntered AndAlso TerminalSystem.PriceDataEntered Then  ''OVERRIDES PRICES IN DATABASE

                        'A Combination of Quantity, Deal Quantity and Price Mode
                        'Otherwise referred to as a Split Package Price Override Key Sequence

                        Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString + " @ ", DealQuantity.ToString + "/" + FormatNumber(PriceMod) + " PR")

                        XPrice = Quantity * PriceMod / DealQuantity

                    ElseIf TerminalSystem.QuantityDataEntered AndAlso TerminalSystem.PriceDataEntered Then   ''OVERRIDES PRICES IN DATABASE

                        'Only Quantity and Price Mod Data Entered
                        'Referred to as a Price Override

                        Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString + " @ ", FormatNumber(PriceMod) + " PR")

                        XPrice = Quantity * PriceMod

                    ElseIf TerminalSystem.PriceDataEntered Then   ''OVERRIDES PRICES IN DATABASE

                        'Only Price Mod Data Entered
                        'Referred to as a Price Override

                        Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString, FormatNumber(PriceMod) + " PR")

                        XPrice = Quantity * PriceMod

                    Else

                        Terminal.Display.DisplayDataL2_LeftJustified(FormatNumber(UnitPrice))

                        XPrice = Quantity * UnitPrice

                    End If



                    If TerminalSystem.QuantityDataEntered And SalePriceMethod = 0 Then

                        'Only Quantity Data Entered
                        'Operator specifed how many items were being rung.

                        Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString, FormatNumber(UnitPrice))

                        XPrice = Quantity * UnitPrice

                    End If



                    'Check if item is on sale

                    If SalePriceMethod > 0 Then

                        'Sale Pricing is Active


                        If SalePriceMethod = 1 Then

                            'Group Price / Quantity. Full Set Not Required

                            Dim LPrice As Decimal = Quantity * (SaleGroupPrice / SaleQuantity)

                            Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString + " @ " + SaleQuantity.ToString + "/" + FormatNumber(SaleGroupPrice), FormatNumber(LPrice) + " RC")

                            XPrice = LPrice

                        End If


                    End If












                    If Not TerminalSystem.PriceDataEntered And PriceRequired = "Y" Then

                        'No Price Data Entered and A Price is Required

                        Terminal.Display.DisplayData("B024 PRICE NEEDED", "  " + ItemName)
                        Terminal.IBMKEY.ClearToCont()

                        Terminal.Display.DisplayData("ENTER PRICE OR", "   PRESS CLEAR")

                        While True

                            Dim InSeq As KeySeq = TerminalSystem.Key_Data_Input()

                            If InSeq.Enter Then

                                DataInput1 = InSeq.Numerics

                                PriceA = Convert.ToDecimal(DataInput1 / 100)

                                Terminal.Display.DisplayData(ItemName)

                                If TerminalSystem.QuantityDataEntered Then

                                    Terminal.Display.DisplayDataL2_LeftRightJustified(Quantity.ToString, FormatNumber(PriceA))

                                    XPrice = Quantity * PriceA

                                Else

                                    Terminal.Display.DisplayDataL2_LeftJustified(FormatNumber(PriceA))

                                    XPrice = Quantity * PriceA

                                End If

                                Exit While

                            ElseIf InSeq.ClearOut Then

                                KeyMenu()


                            End If

                        End While

                    End If



                    Transaction.TRItemTable.Rows.Add(ItemName, UPC, Quantity, XPrice, SalePriceMethod)








                    Try

                        Terminal.Tone.QuickBeep()

                    Catch ex As Exception

                    End Try

                    Exit For

                Next




            Else

                'Clear All Data

                ResetEnteredData()

                'Return to Menu
                KeyMenu()

            End If

            ResetEnteredData()


        End Sub

        Public Shared Sub OnSaleCheck()



        End Sub


        Private Shared Sub ResetEnteredData()

            TerminalSystem.QuantityDataEntered = False
            TerminalSystem.SlashDataEntered = False
            TerminalSystem.PriceDataEntered = False

            TerminalSystem.QuantityData = ""
            TerminalSystem.SlashData = ""
            TerminalSystem.PriceData = ""

        End Sub

    End Class

End Namespace


