Public Class AnchorObject

    Public AnchorSize As String
    Public Diam As Double
    Public FromFlange As Double
    Public Between As Double

    Public Sub New(ByVal anchorSize As String,
                   ByVal fromFlange As Double,
                   ByVal between As Double)

        Me.AnchorSize = anchorSize
        Me.FromFlange = fromFlange
        Me.Between = between

        If anchorSize.Contains("1/2""") Then

            Diam = 0.5

        ElseIf anchorSize.Contains("5/8""") Then

            Diam = 0.625

        ElseIf anchorSize.Contains("3/4""") Then

            Diam = 0.75
        End If

    End Sub

End Class
