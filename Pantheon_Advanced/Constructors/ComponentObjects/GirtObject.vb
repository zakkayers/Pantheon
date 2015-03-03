Public Class GirtObject

    Public Add As Boolean
    Public Type As String
    Public Elevation As Double
    Public Brace As Boolean

    Public Sub New(ByVal add As Boolean, ByVal type As String, ByVal elevation As Double, ByVal brace As Boolean)

        Me.Add = add
        Me.Type = type
        Me.Elevation = elevation
        Me.Brace = brace

    End Sub

End Class
