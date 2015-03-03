Imports System.Media
Imports System.Windows.Forms.VisualStyles
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime
Imports Telerik.WinControls.UI

Public Class Clean

    Public Shared Populate As Boolean = True

    Public Shared Sub CleanText(textBox As RadTextBox)
        If textBox.Text = " , " Then

            textBox.Text = ""

        End If

        If textBox.Text = "0"" , 0"" " Then

            textBox.Text = ""

        End If

        If textBox.Text.EndsWith(" , ") Then

            textBox.Text = textBox.Text.Remove(textBox.Text.Length - 2)

        End If


        If textBox.Text.EndsWith(" , 0"" ") Then

            textBox.Text = textBox.Text.Remove(textBox.Text.Length - 6)

        End If

    End Sub

    Public Shared Function CleanBlock(ByVal text As String) As String

        Dim markName As String

        If text.Contains("_") Then

            Dim textParts As String() = text.Split(New Char() {"_"c})

            Dim count As Integer = Int32.Parse(textParts(1)) + 1

            markName = textParts(0) + "_" + count.ToString()

        Else

            markName = text + "_1"

        End If

        Return markName

    End Function


End Class
