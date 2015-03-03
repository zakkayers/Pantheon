Public Class CableObject

    Public Check As Boolean
    Public Diameter As String
    Public FromFlange As Double
    Public FromTop As Double
    Public FromBottom As Double

    Public Sub New(ByVal check As Boolean, ByVal diameter As String,
                   ByVal fromFlange As Double, ByVal fromTop As Double,
                   ByVal fromBottom As Double)

        Me.Check = check
        Me.Diameter = diameter
        Me.FromFlange = fromFlange
        Me.FromTop = fromTop
        Me.FromBottom = fromBottom

    End Sub

End Class
