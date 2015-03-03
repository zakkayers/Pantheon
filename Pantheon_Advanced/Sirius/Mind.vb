Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput

Public Class Mind

    Public Shared Function AnnoCount(ByVal int As Integer) As String

        Dim letter As String = "A"

        Select Case int

            Case 1
                letter = "A"
            Case 2
                letter = "B"
            Case 3
                letter = "C"
            Case 4
                letter = "D"
            Case 5
                letter = "E"
            Case 6
                letter = "F"
            Case 7
                letter = "G"
            Case 8
                letter = "H"
            Case 9
                letter = "I"
            Case 10
                letter = "J"
            Case 11
                letter = "K"
            Case 12
                letter = "L"
            Case 13
                letter = "M"
            Case 14
                letter = "N"
            Case 15
                letter = "O"
            Case 16
                letter = "P"
            Case 17
                letter = "Q"
            Case 18
                letter = "R"
            Case 19
                letter = "S"
            Case 20
                letter = "T"
            Case 21
                letter = "U"
            Case 22
                letter = "V"
            Case 23
                letter = "W"
            Case 24
                letter = "X"
            Case 25
                letter = "Y"
            Case 26
                letter = "Z"

        End Select

        Return letter

    End Function

End Class
