Public Class Solve

    Public Shared Run As Double
    Public Shared Rise As Double
    Public Shared Slope As Double
    Public Shared Pitch As Double

    Public Shared Sub RunIn(ByVal runIn As Double, ByVal pitchIn As Double)

        Pitch = pitchIn
        Run = runIn
        Rise = Run * Pitch
        Slope = Math.Sqrt(Math.Pow(Run, 2) + Math.Pow(Rise, 2))
    End Sub

    Public Shared Sub RiseIn(ByVal riseIn As Double, ByVal pitchIn As Double)

        Pitch = pitchIn
        Rise = riseIn
        Run = Rise / Pitch
        Slope = Math.Sqrt(Math.Pow(Run, 2) + Math.Pow(Rise, 2))
    End Sub

    Public Shared Sub SlopeIn(ByVal slopeIn As Double, ByVal pitchIn As Double)

        Pitch = pitchIn
        Slope = slopeIn
        Run = (Slope / Math.Sqrt(Math.Pow(Pitch, 2) + 1))
        Rise = Math.Sqrt(Math.Pow(Slope, 2) - Math.Pow(Run, 2))
    End Sub

    Public Shared Sub RunAndRise(ByVal runIn As Double, ByVal riseIn As Double)

        Run = runIn
        Rise = riseIn
        Slope = Math.Sqrt(Math.Pow(Run, 2) + Math.Pow(Rise, 2))
        Pitch = Rise / Run
    End Sub

    Public Shared Sub RunAndSlope(ByVal runIn As Double, ByVal slopeIn As Double)

        Run = runIn
        Slope = slopeIn
        Rise = Math.Sqrt(Math.Pow(Slope, 2) - Math.Pow(Run, 2))
        Pitch = Rise / Run
    End Sub

    Public Shared Sub RiseAndSlope(ByVal riseIn As Double, ByVal slopeIn As Double)

        Rise = riseIn
        Slope = slopeIn
        Run = Math.Sqrt(Math.Pow(Slope, 2) - Math.Pow(Rise, 2))
        Pitch = Rise / Run
    End Sub

    Public Shared TruePitch As Double

    Public Shared Function GetTruePitch(ByVal lowY As Double, ByVal highY As Double, ByVal distance As Double)

        If (distance <> 0.0) Then

            TruePitch = (highY - lowY) / distance

        End If

        Return TruePitch
    End Function

End Class
