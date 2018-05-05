Module UDP

    Public ReceivingUdpClient As UdpClient
    Public SendingUdpClient As New UdpClient
    Public RemoteIpEndPoint As New Net.IPEndPoint(Net.IPAddress.Any, 0)
    Public ThreadReceive As Threading.Thread
    Dim SocketNO As Integer

    Public Sub StartReceiving()

        Try

            SocketNO = 10001
            ReceivingUdpClient = New UdpClient(SocketNO)
            ThreadReceive = New Threading.Thread(AddressOf ReceiveMessages)
            ThreadReceive.Start()

            Console.WriteLine()
            Console.WriteLine("UDP Started")

        Catch ex As Exception

            Console.WriteLine()
            Console.WriteLine("Error Starting UDP")

        End Try

    End Sub

    Private Sub ReceiveMessages()

        Dim ReceiveBytes As Byte() = ReceivingUdpClient.Receive(RemoteIpEndPoint)
        Dim RemoteIP As String = RemoteIpEndPoint.Address.ToString
        Dim Data As String = Encoding.ASCII.GetString(ReceiveBytes)

        Console.WriteLine("Terminal Received Message: " + Data)

        Select Case Data

            Case "L1OK"

                'Sign ON Request OK

                MsgBox("Sign ON Success")


        End Select

        ReceiveMessages()

    End Sub

    Public Sub SendMessage(Data As String)

        Try

            Dim IP As IPAddress
            Dim Port As Integer
            Dim ByteCommand As Byte() = New Byte() {}

            IP = IPAddress.Parse("192.168.10.11")
            Port = 10000

            SendingUdpClient.Connect(IP, Port)
            ByteCommand = Encoding.ASCII.GetBytes(Data)
            SendingUdpClient.Send(ByteCommand, ByteCommand.Length)

        Catch ex As Exception

            MsgBox("Error Sending")

        End Try

    End Sub

End Module
