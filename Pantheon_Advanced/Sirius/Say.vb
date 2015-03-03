Imports Autodesk.AutoCAD.EditorInput

Public Class Say

    ' METHOD: SPEAK
    ' Writes Input Text To AutoCAD Editor Line
    Public Sub Speak(ByVal text As String)

        Dim editor As Editor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor

        editor.WriteMessage(ControlChars.Lf + text)

    End Sub

End Class
