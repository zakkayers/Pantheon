Public Class StraightColumnObject

    Public Mark As String
    Public EaveThick As Double
    Public EaveWidth As Double

    Public StiffThick As Double
    Public StiffWidth As Double

    Public OuterThick As Double
    Public OuterWidth As Double

    Public InnerThick As Double
    Public InnerWidth As Double

    Public WebThick As Double
    Public WebDepth As Double

    Public ConnectionThick As Double
    Public ConnectionWidth As Double
    Public ConnectionLength As Double

    Public BaseThick As Double
    Public BaseWidth As Double

    Public Flush As Boolean
    Public Bypass As Boolean

    Public Sub New(ByVal mark As String, ByVal eaveT As Double, ByVal eaveW As Double, ByVal stiffT As Double, ByVal stiffW As Double,
                   ByVal outT As Double, ByVal outW As Double, ByVal inT As Double, ByVal inW As Double,
                   ByVal webT As Double, ByVal webD As Double, ByVal conT As Double, ByVal conW As Double, ByVal conL As Double,
                   ByVal baseT As Double, ByVal baseW As Double, ByVal flush As Boolean, ByVal bypass As Boolean)

        Me.Mark = mark

        EaveThick = eaveT
        EaveWidth = eaveW

        StiffThick = stiffT
        StiffWidth = stiffW

        OuterThick = outT
        OuterWidth = outW

        InnerThick = inT
        InnerWidth = inW

        WebThick = webT
        WebDepth = webD

        ConnectionThick = conT
        ConnectionWidth = conW
        ConnectionLength = conL

        BaseThick = baseT
        BaseWidth = baseW

        Me.Flush = flush
        Me.Bypass = bypass

    End Sub

End Class
