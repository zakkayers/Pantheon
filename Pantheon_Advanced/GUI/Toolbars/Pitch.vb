Imports System.Media
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Internal
Imports Telerik.WinControls.UI

Public Class Pitch
    Private Sub Send(ByVal command As String)
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute(command, True, False, False)
    End Sub

    Private Sub PitchWorld_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchWorld.ToggleStateChanged
        If PitchWorld.IsChecked Then
            Const command As String = "_ucs _world "
            Send(command)
            ToggleCheck(0)
        End If       
    End Sub

    Private Sub Pitch1_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch1.ToggleStateChanged
        If Pitch1.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 4.763642 "
            Send(command)
            ToggleCheck(1)
        End If
    End Sub

    Private Sub Pitch2_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch2.ToggleStateChanged
        If Pitch2.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 9.462322 "
            Send(command)
            ToggleCheck(2)
        End If      
    End Sub

    Private Sub Pitch3_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch3.ToggleStateChanged
        If Pitch3.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 14.03624 "
            Send(command)
            ToggleCheck(3)
        End If        
    End Sub

    Private Sub Pitch4_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch4.ToggleStateChanged
        If Pitch4.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 18.43495 "
            Send(command)
            ToggleCheck(4)
        End If
    End Sub

    Private Sub Pitch5_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch5.ToggleStateChanged
        If Pitch5.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 22.61986 "
            Send(command)
            ToggleCheck(5)
        End If      
    End Sub

    Private Sub Pitch6_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Pitch6.ToggleStateChanged
        If Pitch6.IsChecked Then
            Const command As String = "_ucs _world _ucs _z 26.56505 "
            Send(command)
            ToggleCheck(6)
        End If       
    End Sub

    Private Sub PitchNeg1_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg1.ToggleStateChanged
        If PitchNeg1.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -4.763642 "
            Send(command)
            ToggleCheck(7)
        End If       
    End Sub

    Private Sub PitchNeg2_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg2.ToggleStateChanged
        If PitchNeg2.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -9.462322 "
            Send(command)
            ToggleCheck(8)
        End If       
    End Sub

    Private Sub PitchNeg3_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg3.ToggleStateChanged
        If PitchNeg3.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -14.03624 "
            Send(command)
            ToggleCheck(9)
        End If       
    End Sub

    Private Sub PitchNeg4_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg4.ToggleStateChanged
        If PitchNeg4.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -18.43495 "
            Send(command)
            ToggleCheck(10)
        End If      
    End Sub

    Private Sub PitchNeg5_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg5.ToggleStateChanged
        If PitchNeg5.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -22.61986 "
            Send(command)
            ToggleCheck(11)
        End If      
    End Sub

    Private Sub PitchNeg6_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchNeg6.ToggleStateChanged
        If PitchNeg6.IsChecked Then
            Const command As String = "_ucs _world _ucs _z -26.56505 "
            Send(command)
            ToggleCheck(12)
        End If       
    End Sub

    Private Sub PitchCustom_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchCustom.ToggleStateChanged
        If PitchCustom.IsChecked Then
            Dim pitchValue As Double
            If Double.TryParse(CustomField.Text, pitchValue) Then
                pitchValue = pitchValue / 12

                Dim degree As Double = Math.Atan(pitchValue) * (180.0 / Math.PI)

                Dim degreeString As String = degree.ToString()

                Dim command As String = "_ucs _world _ucs _z " + degreeString + " "
                Send(command)

                ToggleCheck(13)
            Else
                MsgBox("Pitch Value Is Entered Incorrectly")
                SystemSounds.Beep.Play()

            End If
        End If   
    End Sub

    Private Sub PitchObject_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchObject.ToggleStateChanged
        If PitchObject.IsChecked Then
            Const command As String = "_ucs _object "
            Send(command)
            ToggleCheck(14)
        End If
        
    End Sub

    Private Sub PitchPrevious_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles PitchPrevious.ToggleStateChanged
        If PitchPrevious.IsChecked Then
            Const command As String = "_ucs _p "
            Send(command)
            ToggleCheck(15)
        End If
    End Sub

    Private Sub ToggleCheck(ByVal int As Integer)

        Select Case int

            Case 0 ' World

                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 1 ' 1/12

                PitchWorld.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 2 ' 2/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 3 ' 3/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 4 ' 4/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 5 ' 5/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 6 ' 6/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 7 ' -1/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 8 ' -2/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 9 ' -3/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 10 ' -4/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 11 ' -5/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 12 ' -6/12

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 13 ' Custom

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchObject.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 14 ' Object

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchPrevious.IsChecked = False

            Case 15 ' Previous

                PitchWorld.IsChecked = False
                Pitch1.IsChecked = False
                Pitch2.IsChecked = False
                Pitch3.IsChecked = False
                Pitch4.IsChecked = False
                Pitch5.IsChecked = False
                Pitch6.IsChecked = False
                PitchNeg1.IsChecked = False
                PitchNeg2.IsChecked = False
                PitchNeg3.IsChecked = False
                PitchNeg4.IsChecked = False
                PitchNeg5.IsChecked = False
                PitchNeg6.IsChecked = False
                PitchCustom.IsChecked = False
                PitchObject.IsChecked = False

        End Select

    End Sub
End Class
