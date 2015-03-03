Public Class CableObject

    Public Check As Boolean
    Public Diameter As String
    Public DiamDouble As Double
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

        Select Case diameter

            Case "1/4"""

                DiamDouble = 0.25

            Case "3/8"""

                DiamDouble = 0.375

            Case "1/2"""

                DiamDouble = 0.5

        End Select

    End Sub

End Class
