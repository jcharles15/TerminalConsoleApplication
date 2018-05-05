
Namespace CBJ_TERMINAL_SYSTEM

    Public Class CheckoutSupport



        Public TermStatus As TerminalStatus = TerminalStatus.Enabled

        Public Enum TerminalStatus

            Enabled
            Disabled

        End Enum

        Public Sub DisableTerminal()

            TermStatus = TerminalStatus.Disabled

        End Sub

    End Class





End Namespace