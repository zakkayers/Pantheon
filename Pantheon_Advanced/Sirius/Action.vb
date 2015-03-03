
Imports System.Windows.Input
Imports Autodesk.Windows


Public Class Action
    Implements ICommand

    ' FUNCTION: CANEXECTUE
    ' This Function Returns TRUE If A Button Press Can Be Exectuted
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function

    Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

    ' METHOD: EXECUTE
    ' This Method Executes The Input Ribbon Button Command
    Public Sub Execute(parameter As Object) Implements ICommand.Execute

        Dim ribBtn As RibbonButton = TryCast(parameter, RibbonButton)

        If (ribBtn IsNot Nothing) Then

            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(
                ribBtn.CommandParameter, True, False, True)

        End If
    End Sub


End Class
