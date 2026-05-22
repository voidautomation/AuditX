Imports System.Net.NetworkInformation
Imports System.Management

Public Class UsageInfo
    Public Property cpu As Integer
    Public Property ram As Integer
    Public Property disk As Integer
    Public Property netSpeed As String
    Public Property availableRamMB As Integer
    Public Property netValue As Double
End Class

Public Class SystemMonitor
    Private cpuCounter As New PerformanceCounter("Processor", "% Processor Time", "_Total")
    Private ramCounter As New PerformanceCounter("Memory", "% Committed Bytes In Use")
    Private diskCounter As New PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total")

    Private netInterface As NetworkInterface
    Private lastBytesReceived As Long = 0
    Private lastBytesSent As Long = 0

    Public Sub New()
        netInterface = NetworkInterface.GetAllNetworkInterfaces().
            FirstOrDefault(Function(ni) ni.OperationalStatus = OperationalStatus.Up AndAlso
                                          ni.NetworkInterfaceType <> NetworkInterfaceType.Loopback AndAlso
                                          ni.Speed > 0 AndAlso
                                          Not ni.Description.ToLower().Contains("virtual") AndAlso
                                          Not ni.Description.ToLower().Contains("pseudo"))

        If netInterface IsNot Nothing Then
            Dim stats = netInterface.GetIPv4Statistics()
            lastBytesReceived = stats.BytesReceived
            lastBytesSent = stats.BytesSent
        End If
    End Sub

    Public Function GetUsageValues() As UsageInfo
        Dim cpu = CInt(cpuCounter.NextValue())
        Dim ram = CInt(ramCounter.NextValue())
        Dim disk = CInt(diskCounter.NextValue())

        Dim dummySpeed As Double
        Dim netSpeed = GetFormattedNetworkSpeed(dummySpeed)
        Dim availableRamMB = CInt(My.Computer.Info.AvailablePhysicalMemory / (1024 * 1024))

        Return New UsageInfo With {
            .cpu = cpu,
            .ram = ram,
            .disk = disk,
            .netSpeed = netSpeed,
            .availableRamMB = availableRamMB,
            .netValue = dummySpeed
        }
    End Function

    Private Function GetFormattedNetworkSpeed(ByRef valueOut As Double) As String
        If netInterface Is Nothing Then
            valueOut = 0
            Return "0 Bps"
        End If

        Dim stats = netInterface.GetIPv4Statistics()
        Dim bytesReceived = stats.BytesReceived
        Dim bytesSent = stats.BytesSent

        Dim delta = (bytesReceived + bytesSent) - (lastBytesReceived + lastBytesSent)
        lastBytesReceived = bytesReceived
        lastBytesSent = bytesSent

        valueOut = delta

        If delta < 1024 Then
            Return delta & " Bps"
        ElseIf delta < 1024 * 1024 Then
            valueOut = delta / 1024
            Return valueOut.ToString("0.0") & " KBps"
        Else
            valueOut = delta / (1024 * 1024)
            Return valueOut.ToString("0.0") & " MBps"
        End If
    End Function
End Class
