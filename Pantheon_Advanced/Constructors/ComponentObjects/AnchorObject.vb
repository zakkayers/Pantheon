Public Class AnchorObject

    Public AnchorSize As String
    Public FromFlange As Double
    Public Between As Double

    Public Sub New(ByVal anchorSize As String,
                   ByVal fromFlange As Double,
                   ByVal between As Double)

        Me.AnchorSize = anchorSize
        Me.FromFlange = fromFlange
        Me.Between = between

    End Sub

End Class
