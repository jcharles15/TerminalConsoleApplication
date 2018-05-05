
Imports System.Collections.Concurrent
Imports System.Net.NetworkInformation

Module AppCommunicationsModule

    '* AppCommunicationsModule - Makes Communications To Required Resources On The Network

    Public BroadcastThread As Threading.Thread
    Public UDPListenerThread As Threading.Thread
    Public ServerResponseThread As Threading.Thread

    Public FoundController As Boolean = False
    Private DataBytes As Byte()
    Private UDPClient As New UdpClient()
    Private BroadcastInterval As Integer
    Private BroadcastIP As New IPEndPoint(IPAddress.Broadcast, 15000)

    Public LoginReqOK As Boolean = False
    Public ControllerCommError As Boolean = False
    Public ReceivedMessage As Boolean = False

    Public Client As New TcpClient

    Public LastDataSent As String = ""
    Public ServerResponseData As String = ""
    Public ReceivedCommand As String = ""
    Public CommunicationsUsername As String = ""
    Public CommunicationsPassword As String = ""
    Public List As String = ""
    Public DataInput() As String = Nothing

    Public DeviceLoadList As New List(Of String)
    Dim LoadDeviceThread As Threading.Thread

#Region "UDP Controller Search"

    Public Sub SearchForController()
        '* Sets Up and Attempts To Search For and Connect To The Pos Controller On The Network

        Console.WriteLine("Searching For Available Controllers")

        '* Start A New Thread For Broadcasting Messages
        BroadcastThread = New Threading.Thread(AddressOf SendUDPBroadcast) With {
            .IsBackground = True
        }

        '* Start A New Thread To Listen For UDP Messages
        UDPListenerThread = New Threading.Thread(AddressOf ListenForUDPMessage) With {
            .IsBackground = True
        }

        Try

            BroadcastThread.Start()

            UDPListenerThread.Start()

        Catch ex As Exception
            MsgBox(ex.Message.ToString + "An Error Has Occured In The App Communications Module - Search For Controller")
        End Try

    End Sub

    Private Sub SendUDPBroadcast()

        Try

            Do
                DataBytes = Encoding.ASCII.GetBytes("CBJPOS")

                UDPClient.Send(DataBytes, DataBytes.Length, BroadcastIP)

                Threading.Thread.Sleep(BroadcastInterval)

            Loop Until FoundController = True

        Catch ex As Exception

            'MsgBox(ex.Message.ToString + vbCrLf + "Inside App Communications Module - Send UDP Broadcast")

        End Try

    End Sub

    Private Sub ListenForUDPMessage(ByVal AR As IAsyncResult)

        Try

            Do

                'Start Timer. If Timer Is Exhausted Before A Reply Is Received, Then Timeout.
                StartTimerFindControllerTimeout()

                'This Will Block The Thread Until A Reply Is Received. (Essentially, It's Waiting For A Reply)
                Dim RemoteIPEndPoint As New IPEndPoint(IPAddress.Any, 0)
                Dim ReceiveBytes As Byte() = UDPClient.Receive(RemoteIPEndPoint)
                Dim ReturnData As String = Encoding.ASCII.GetString(ReceiveBytes)

                'If This Line Gets Executed Then That Means A Reply Has Been Received
                ReceiveControllerReply(RemoteIPEndPoint, ReturnData)

            Loop

        Catch ex As Exception

            MsgBox(ex.Message.ToString + vbCrLf + "Inside App Communications")

        End Try

    End Sub

    Private Sub ReceiveControllerReply(ByVal ControllerIP As IPEndPoint, ByVal Port As String)

        'A Reply From A Controller Has Been Received, Indicating The Client Has Found An Available Controller
        FoundController = True

        'Wait 5 Seconds
        Threading.Thread.Sleep(5000)

        Console.BackgroundColor = ConsoleColor.Cyan
        Console.WriteLine("Controller Found - " + ControllerIP.Address.ToString + " Port - " + Port.ToString)
        Console.ResetColor()

        'Save Controller Information And Initialize Controller Connection
        If RecordControllerInfo(ControllerIP.Address.ToString, Port.ToString) Then

            InitializeControllerConnection()

        End If

    End Sub
#End Region

#Region "Initialize Controller Connection"

    Public Function InitializeControllerConnection() As Boolean

        ControllerIP = My.Settings.ControllerIP
        ControllerPort = My.Settings.ControllerPort

        CommunicationsUsername = My.Settings.CommUser
        CommunicationsPassword = My.Settings.CommPassword

        '1. Check The Network Interface To Make Sure It's Up
        If CheckNetworkInterface() = Not True Then

            'Network Interface Down
            Terminal.Display.DisplayData("W058 NETWORK LINK", "     IS DOWN")

        Else

            Terminal.Display.DisplayData("NETWORK LINK OK")
            Threading.Thread.Sleep(1000)

            If ControllerIP = Nothing Or ControllerPort = Nothing Then

                'No Controller Info. Try To Search For The Controller On The Network.

                SearchForController()
                Threading.Thread.Sleep(10000)

            Else

                If CommunicationsUsername = Nothing Or CommunicationsPassword = Nothing Then

                    Terminal.Display.DisplayData("CONTROLLER AUTH", "SETTINGS INVALID")
                    WriteLine("Check Auth Settings", False, ConsoleColor.Red)

                    PressAnyKey()

                    ProcessMainInput()

                Else

                    'Make Connection To Controller
                    If Connect(ControllerIP, ControllerPort, CommunicationsUsername, CommunicationsPassword) Then

                        Dim thread As New Threading.Thread(AddressOf ThreadConnMonitor)
                        thread.Start()

                        Return True

                    End If

                End If

            End If

        End If

        Return False

    End Function

    Public Sub ThreadConnMonitor()

        Do

            Threading.Thread.Sleep(1000)


        Loop While Client.Connected = True

        Terminal.Display.DisplayData("LOST CONNECTION", "TO CONTROLLER")

        Console.WriteLine("Lost Connection with Server")

        PressAnyKey()

    End Sub

#Region "Connect Terminal"

    Public Function Connect(ByVal ControllerIP As String, ByVal ControllerPort As Integer, ByVal CommunicationsUsername As String, ByVal CommunicationsPassword As String) As Boolean

        'Try To Make Connection With Controller

        If Client.Connected = True Then

        Else

            Try

                Client.Connect(ControllerIP, ControllerPort)

                WriteLine("Link To Controller Successful", False, ConsoleColor.Blue)

                Terminal.Display.DisplayData("CONTROLLER CONN", "ESTABLISHED")

                WriteLine("Sending Auth Information", False, ConsoleColor.Blue)

                SendData("LOGIN|" + My.Settings.TerminalID.ToString + "|" + CommunicationsUsername + "|" + CommunicationsPassword)

                'ServerResponseThread = New Threading.Thread(AddressOf AuthRequestResponse)
                'StartTimerWaitForControllerResponseCountdown()
                ' ServerResponseThread.Start()

                If AuthRequestResponse() Then

                    WriteLine("Auth Request Successful", False, ConsoleColor.Green)

                    ActivateHighway()

                    Return True

                End If

            Catch ex As Exception

                'ErrorHandler(1, "Controller Communication Error", ex.Message)

                Terminal.Display.DisplayData("W004 CONTROLLER", "     DOES NOT RESPOND")

                Threading.Thread.Sleep(2000)

                RetryControllerConnection()

            End Try

        End If

        Return False

    End Function

#End Region

#Region "Sending Data"

    Public Sub SendData(ByVal Data As String) 'Sends Data to Server

        LastDataSent = Data

        Try

            If Data.StartsWith("LOGIN|") Then

                'Allow The Sending Of Login Message
                Dim Writer As New StreamWriter(Client.GetStream)
                Writer.WriteLine(Data)
                Writer.Flush()

            ElseIf LoginReqOK = True And Client.Connected Then

                Dim Writer As New StreamWriter(Client.GetStream)
                Writer.WriteLine(Data)
                Writer.Flush()

            Else
                ' MsgBox("Cannot Connect To Server")
            End If

        Catch ex As Exception

            CommError = True
            ErrorType = "1"

            If Data = "CONN?" Then

            Else

                CheckFunctions()
            End If


            Console.WriteLine(ex.Message.ToString)

        End Try
    End Sub

#End Region

#Region "Receiving Data"

    Public Function AuthRequestResponse() As Boolean

        'Waits For Response

        Try

            Dim Reader As New StreamReader(Client.GetStream)
            Dim Data As String = Reader.ReadLine

            ServerResponseData = Data

            If ServerResponseData.StartsWith("LOGREQOK") Then

                Dim DataParts() As String = ServerResponseData.Split("|")
                DataSessionNumber = DataParts(1)
                My.Settings.DataSessionNumber = DataSessionNumber
                My.Settings.Save()

                'StopTimerWaitForControllerResponseCountdown()

                LoginReqOK = True

                Return True

            ElseIf ServerResponseData.StartsWith("LOGREQINV") Then

                StopTimerWaitForControllerResponseCountdown()

                WriteLine("Login Invalid", False, ConsoleColor.Red)

            End If

        Catch ex As Exception

            WriteLine("Error Has Occured" + vbCrLf + ex.Message, False)

        End Try

        Return False

    End Function

    Private Sub ActivateHighway()

        'Enable Client And Server To Talk To Each Other

        Client.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf Reading, Nothing)

    End Sub



    Public Sub MessageReceived(ByVal Data As String)

        ReceivedCommand = Data
        ReceivedMessage = True


        ListenForCommands(ReceivedCommand)

        'Reset Data
        Data = Nothing
    End Sub

    Public Sub ListenForCommands(ByVal Cmd As String)

        DataInput = Cmd.Split("|")

        Select Case DataInput(0)

            Case "DEVICECOUNT"

                WriteLine("Receiving Device Count", False)
                Threading.Thread.Sleep(1000)
                DeviceCountExpected = DataInput(1)

                If DeviceCountExpected > 1 Or 0 Then
                    Console.WriteLine(DeviceCountExpected.ToString + " DEVICES FOUND")
                Else
                    Console.WriteLine(DeviceCountExpected.ToString + " DEVICE FOUND")
                End If

                Console.WriteLine("Waiting For Controller To Process Device")

            Case "LOADDEVICE"

                Console.Write("/")

                DeviceCountToLoad += 1

                DeviceType1 = DataInput(1)
                DeviceName = DataInput(2)
                DeviceState = DataInput(3)

                DeviceLoadList.Add(DeviceType1 + "|" + DeviceName + "|" + DeviceState)

                Threading.Thread.Sleep(2000)

                SendData("SendNextDevice")


            Case "LDREQDN"

                WriteLine("Device Retrieval Done")
                Threading.Thread.Sleep(2000)

                CheckDeviceCount()



        End Select


    End Sub
    Public MessageReceivedQueue As New ConcurrentQueue(Of String)
    Public Sub Reading()

        Try

            Dim Reader As New StreamReader(Client.GetStream)
            Dim Data As String = Reader.ReadLine()

            MessageReceived(Data)



            'MessageReceivedQueue.Enqueue(Data)


            Client.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf Reading, Nothing)

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region

#Region "Other Functions"

    Public Sub CheckDeviceCount()


        If DeviceCountToLoad = DeviceCountExpected Then

            Try

                List = String.Join(Environment.NewLine, DeviceLoadList.ToArray)
                Console.WriteLine("Now Loading Devices... Please Wait")
                Threading.Thread.Sleep(3000)

                DeviceType1 = Nothing
                DeviceState = Nothing
                DeviceName = Nothing

                LoadDeviceThread = New Threading.Thread(AddressOf LoadDevices)
                LoadDeviceThread.Start()

                ' LoadDevices()

            Catch ex As Exception
                MsgBox("HERE " + ex.Message.ToString)
            End Try
        Else

            Console.WriteLine("Failed To Properly Retrieve All Devices To Load")
            Console.WriteLine("Device Count: " + DeviceCountToLoad.ToString)

        End If

    End Sub

#End Region

    Private Function CheckNetworkInterface() As Boolean

        Dim Result As Boolean = False

        Result = NetworkInterface.GetIsNetworkAvailable()

        Return Result

    End Function


End Module
