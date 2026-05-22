Imports Newtonsoft.Json
Imports System.IO
Imports System.Windows.Forms

Public Module LanguageManager

    Private Translations As Dictionary(Of String, Dictionary(Of String, String))
    Private CurrentLanguageCode As String = "en"

    Public Sub LoadLanguages(filePath As String)
        If Not File.Exists(filePath) Then
            MessageBox.Show("Language file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim json = File.ReadAllText(filePath)
        Translations = JsonConvert.DeserializeObject(Of Dictionary(Of String, Dictionary(Of String, String)))(json)
    End Sub

    Public Sub ApplyLanguage(form As Form, langCode As String, Optional tooltip As ToolTip = Nothing)
        If Translations Is Nothing Then Exit Sub
        If Not Translations.ContainsKey(langCode) Then Exit Sub

        CurrentLanguageCode = langCode
        Dim langSet = Translations(langCode)

        For Each pair In langSet
            If pair.Key.StartsWith("ToolTip_") Then
                ' Tooltip support
                Dim controlName = pair.Key.Replace("ToolTip_", "")
                Dim ctrl = form.Controls.Find(controlName, True).FirstOrDefault()
                If ctrl IsNot Nothing AndAlso tooltip IsNot Nothing Then
                    tooltip.SetToolTip(ctrl, pair.Value)
                End If
            Else
                Dim ctrl = form.Controls.Find(pair.Key, True).FirstOrDefault()
                If ctrl IsNot Nothing Then
                    ctrl.Text = pair.Value
                End If
            End If
        Next
    End Sub



    ' Use for translating message boxes
    Public Function GetText(key As String) As String
        If Translations Is Nothing Then Return key
        If Translations.ContainsKey(CurrentLanguageCode) AndAlso Translations(CurrentLanguageCode).ContainsKey(key) Then
            Return Translations(CurrentLanguageCode)(key)
        End If
        Return key ' fallback
    End Function

End Module
