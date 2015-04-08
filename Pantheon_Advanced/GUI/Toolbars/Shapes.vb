Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Internal

Public Class Shapes

    Private Sub PurlinLeft_Click(sender As Object, e As EventArgs) Handles PurlinLeft.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_zee _purlin _left ", True, False, False)
    End Sub

    Private Sub PurlinRight_Click(sender As Object, e As EventArgs) Handles PurlinRight.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_zee _purlin _right ", True, False, False)
    End Sub

    Private Sub GirtLeft_Click(sender As Object, e As EventArgs) Handles GirtLeft.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_zee _girt _left ", True, False, False)
    End Sub

    Private Sub GirtRight_Click(sender As Object, e As EventArgs) Handles GirtRight.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_zee _girt _right ", True, False, False)
    End Sub

    Private Sub PostVert_Click(sender As Object, e As EventArgs) Handles PostVert.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_cee _post _vertical ", True, False, False)
    End Sub

    Private Sub PostHor_Click(sender As Object, e As EventArgs) Handles PostHor.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_cee _post _horizontal ", True, False, False)
    End Sub

    Private Sub JambVert_Click(sender As Object, e As EventArgs) Handles JambVert.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_cee _jamb _vertical ", True, False, False)
    End Sub

    Private Sub JambHor_Click(sender As Object, e As EventArgs) Handles JambHor.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_cee _jamb _horizontal ", True, False, False)
    End Sub

    Private Sub DSU_Click(sender As Object, e As EventArgs) Handles DSU.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_eavecee _orientation _dsu ", True, False, False)
    End Sub

    Private Sub SSU_Click(sender As Object, e As EventArgs) Handles SSU.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_eavecee _orientation _ssu ", True, False, False)
    End Sub

    Private Sub DSD_Click(sender As Object, e As EventArgs) Handles DSD.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_eavecee _orientation _dsd ", True, False, False)
    End Sub

    Private Sub SSD_Click(sender As Object, e As EventArgs) Handles SSD.Click
        Utils.SetFocusToDwgView()
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        acDoc.SendStringToExecute("_eavecee _orientation _ssd ", True, False, False)
    End Sub

    Private Sub RolledI_Click(sender As Object, e As EventArgs) Handles RolledI.Click

    End Sub

    Private Sub Cee_Click(sender As Object, e As EventArgs) Handles Cee.Click

    End Sub

    Private Sub Channel_Click(sender As Object, e As EventArgs) Handles Channel.Click

    End Sub

    Private Sub Angle_Click(sender As Object, e As EventArgs) Handles Angle.Click

    End Sub

    Private Sub T_Click(sender As Object, e As EventArgs) Handles T.Click

    End Sub

    Private Sub Tube_Click(sender As Object, e As EventArgs) Handles Tube.Click

    End Sub
End Class
