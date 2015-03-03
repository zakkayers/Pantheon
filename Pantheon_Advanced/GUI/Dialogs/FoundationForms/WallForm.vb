Imports System.Windows.Forms

Public Class WallForm

    Public Sub Jolt(ByVal text As String)

        Clean.Populate = True

        If Lists.StandardSlabList.Count <> 0 Then

            For i = 0 To Lists.StandardSlabList.Count - 1

                MarkCombo.Items.Add(Lists.StandardSlabList.Item(i).Mark)

            Next
            Copy.Enabled = True
            MarkCombo.Enabled = True
            MarkCombo.SelectedIndex = MarkCombo.Items.Count - 1
        End If

        NameLabel.Text = text
        MarkCombo.DropDownStyle = ComboBoxStyle.DropDownList
        Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(Me)
    End Sub

End Class
