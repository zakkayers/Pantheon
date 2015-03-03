Imports System.Windows.Forms

Public Class StraightColumnForm

    Public Sub Jolt()

        Clean.Populate = True

        If Lists.StraightColumnList.Count <> 0 Then

            For i = 0 To Lists.StraightColumnList.Count - 1

                MarkCombo.Items.Add(Lists.StraightColumnList.Item(i).Mark)

            Next
            Copy.Enabled = True
            MarkCombo.Enabled = True
            MarkCombo.SelectedIndex = MarkCombo.Items.Count - 1
        End If

        Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(Me)
    End Sub

End Class
