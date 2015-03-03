Imports System.Windows.Forms
Imports Telerik.WinControls.UI

Public Class SaveOver

    Dim _combo As ComboBox
    Dim _button As RadButton

    Public Sub Save()

        Location = Windows.Forms.Cursor.Position
        Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(Me)

    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click



    End Sub

    Private Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click

    End Sub

    Private Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click

    End Sub
End Class
