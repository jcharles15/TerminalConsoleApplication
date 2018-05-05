Module Declarations

    Public Attempt As Integer = 1
    Public MaxAttempts As Integer = 3

    Public FindControllerTimeoutTime As Integer = 10
    Public ServerResponseTimeoutTime As Integer = 5

    Public BroadcastInterval As Integer = 5000

    Public ControllerIP As String = Nothing
    Public ControllerPort As String = Nothing


    Public DBError As Boolean = False
    Public Errors As Boolean = False





    Public CommError As Boolean = False

    Public ErrorType As String

    Public DataSessionNumber As String


    'Integers

    Public DeviceCountExpected As Integer = Nothing
    Public DeviceCountToLoad As Integer = Nothing
    Public TerminalID As Integer = Nothing


    'Strings

    Public DeviceName As String = "LineDisplay"
    Public DeviceState As String
    Public MsgParts() As String


    'Lists


    'True/False

    Public LoadDevicess As Boolean = False

    'Pos Keyboard Light Commands

    Public CommandLedOn As Int32 = 201
    Public CommandLedOff As Int32 = 202
    Public DataWaitLed As Long = 1
    Public DataOfflineLed As Long = 2
    Public DataMessagePendingLed As Long = 4
    Public DataReservedLed As Long = 8
    Public Null As String

    'Database

    Public DBConnect As New MySqlConnection("Server=192.168.10.10;Database=pointofsale;Uid=admincj;Pwd=guitarhero3;")
    Public DBDataset
    Public DBTable As DataTable
    Public Command As MySqlCommand
    Public DBReader As MySqlDataReader
    Public DBQueryUpdate As String
    Public CommandSelect
    Public CommandUpdate


    'Integers And Decimals

    Public LoginAttempts As Integer

    'Booleans

    Public InvalidUser As Boolean = False


    'Strings

    Public AccountStatus As String

    Public CurrentOperatorLoginName As String

    Public DataInput1 As String = ""
    Public InSeq As KeySeq

    Public Role As String

    'Settings

    Public Accountability As String = "Accountability"

    Public MaskData As Boolean = False


End Module
