Imports System.IO
Imports System.Xml.Linq

Public Module ITGCControlMetadataStore

    Private ControlMap As New Dictionary(Of String, String)

    Public Sub LoadControlActions(xmlPath As String)
        If Not IO.File.Exists(xmlPath) Then
            Throw New FileNotFoundException("Control metadata XML file not found.", xmlPath)
        End If

        Dim doc As XDocument = XDocument.Load(xmlPath)

        ' Populate dictionary with ControlRef → ControlAction
        ControlMap = (From row In doc...<Row>
                      Let id = row.<ControlRef>.Value.Trim().ToUpper()
                      Let action = row.<ControlAction>.Value.Trim()
                      Where Not String.IsNullOrWhiteSpace(id)
                      Select id, action).ToDictionary(Function(pair) pair.id, Function(pair) pair.action)
    End Sub

    Public Function GetControlAction(controlID As String) As String
        Dim id = controlID.Trim().ToUpper()
        If ControlMap.ContainsKey(id) Then
            Dim action = ControlMap(id)
            If Not String.IsNullOrWhiteSpace(action) Then
                Return action
            Else
                Return "No specific action defined. Please review manually."
            End If
        Else
            Return "Control ID not found in metadata."
        End If
    End Function

End Module
