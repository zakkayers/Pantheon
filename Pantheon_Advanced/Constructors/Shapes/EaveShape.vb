
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class EaveShape
    Public Top As Double = 3
    Public Bottom As Double = 5
    Public Orient = "DSU"
    Public Pitch As Double = 0

    Public Sub Draw()
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Dim eaveBottomOptions As PromptPointOptions =
                New PromptPointOptions(
                    ControlChars.Lf + "Select Eave Web Botttom Point or [Topflange/Bottomflange/Orientation]",
                    "Topflange Bottomflange Orientation")
        Dim eaveBottomResult As PromptPointResult = editor.GetPoint(eaveBottomOptions)

        If (eaveBottomResult.Status = PromptStatus.Keyword) Then

            Select Case eaveBottomResult.StringResult

                Case "Topflange"

                    Dim topOption As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Top Flange Length In Inches : ")
                    Dim topResult As PromptDoubleResult = editor.GetDistance(topOption)

                    If (topResult.Status = PromptStatus.OK) Then

                        Top = topResult.Value
                        Draw()
                    End If

                Case "Bottomflange"

                    Dim botOption As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Bottom Flange Length In Inches : ")
                    Dim botResult As PromptDoubleResult = editor.GetDistance(botOption)

                    If (botResult.Status = PromptStatus.OK) Then

                        Top = botResult.Value
                        Draw()
                    End If

                Case "Orientation"

                    Dim keywordOption As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Enter Orientation Type [DSU/DSD/SSU/SSD]",
                                                     "DSU DSD SSU SSD")
                    Dim keywordResult As PromptResult = editor.GetKeywords(keywordOption)

                    If (keywordResult.Status = PromptStatus.OK) Then

                        Orient = keywordResult.StringResult
                        Draw()
                    End If

            End Select

        ElseIf (eaveBottomResult.Status = PromptStatus.OK) Then

            Dim topPointOptions As PromptPointOptions =
                    New PromptPointOptions(ControlChars.Lf + "Select Eave Web Top Point : ")
            Dim topResult As PromptPointResult = editor.GetPoint(topPointOptions)

            If (topResult.Status = PromptStatus.OK) Then

                Dim peakOption As PromptPointOptions =
                        New PromptPointOptions(ControlChars.Lf + "Select Pitch Point : ")
                Dim peakResult As PromptPointResult = editor.GetPoint(peakOption)

                If (peakResult.Status = PromptStatus.OK) Then

                    Dim topPoint As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)

                    Dim peakPoint As Point2d = New Point2d(peakResult.Value.X, peakResult.Value.Y)


                    ' Start Transaction

                    Using doc.LockDocument()
                        Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                        Try

                            If (peakPoint.X >= topPoint.X) Then

                                If (topPoint.Y < peakPoint.Y) Then

                                    Pitch = TriSolv.GetTruePitch(topPoint.Y, peakPoint.Y, peakPoint.X - topPoint.X)

                                    TriSolv.SlopeIn(Top, Pitch)
                                    Dim topRun = TriSolv.Run
                                    Dim topRise = TriSolv.Rise

                                    TriSolv.SlopeIn(Bottom, Pitch)
                                    Dim botRun = TriSolv.Run
                                    Dim botRise = TriSolv.Rise

                                    Dim eave As Polyline = New Polyline

                                    Select Case Orient

                                        Case "DSU"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X + topRun,
                                                                                  topWeb.Y + topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X + botRun,
                                                                                  botWeb.Y + botRise)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)


                                        Case "SSU"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X + topRun,
                                                                                  topWeb.Y + topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X + Bottom, botWeb.Y)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                    End Select

                                    Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                        OpenMode.ForWrite)

                                    btr.AppendEntity(eave)

                                    transaction.AddNewlyCreatedDBObject(eave, True)

                                ElseIf (peakPoint.Y < topPoint.Y) Then

                                    Pitch = TriSolv.GetTruePitch(peakPoint.Y, topPoint.Y, peakPoint.X - topPoint.X)

                                    TriSolv.SlopeIn(Top, Pitch)
                                    Dim topRun = TriSolv.Run
                                    Dim topRise = TriSolv.Rise

                                    TriSolv.SlopeIn(Bottom, Pitch)
                                    Dim botRun = TriSolv.Run
                                    Dim botRise = TriSolv.Rise

                                    Dim eave As Polyline = New Polyline

                                    Select Case Orient

                                        Case "DSD"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X + topRun,
                                                                                  topWeb.Y - topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X + botRun,
                                                                                  botWeb.Y - botRise)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                        Case "SSD"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X + topRun,
                                                                                  topWeb.Y - topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X + Bottom, botWeb.Y)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                    End Select

                                    Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                        OpenMode.ForWrite)

                                    btr.AppendEntity(eave)

                                    transaction.AddNewlyCreatedDBObject(eave, True)

                                End If

                            ElseIf (peakPoint.X < topPoint.X) Then

                                If (topPoint.Y < peakPoint.Y) Then

                                    Pitch = TriSolv.GetTruePitch(topPoint.Y, peakPoint.Y, topPoint.X - peakPoint.X)

                                    TriSolv.SlopeIn(Top, Pitch)
                                    Dim topRun = TriSolv.Run
                                    Dim topRise = TriSolv.Rise

                                    TriSolv.SlopeIn(Bottom, Pitch)
                                    Dim botRun = TriSolv.Run
                                    Dim botRise = TriSolv.Rise

                                    Dim eave As Polyline = New Polyline

                                    Select Case Orient

                                        Case "DSU"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X - topRun,
                                                                                  topWeb.Y + topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X - botRun,
                                                                                  botWeb.Y + botRise)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)


                                        Case "SSU"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X - topRun,
                                                                                  topWeb.Y + topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X - Bottom, botWeb.Y)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                    End Select

                                    Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                        OpenMode.ForWrite)

                                    btr.AppendEntity(eave)

                                    transaction.AddNewlyCreatedDBObject(eave, True)

                                ElseIf (peakPoint.Y < topPoint.Y) Then

                                    Pitch = TriSolv.GetTruePitch(peakPoint.Y, topPoint.Y, topPoint.X - peakPoint.X)

                                    TriSolv.SlopeIn(Top, Pitch)
                                    Dim topRun = TriSolv.Run
                                    Dim topRise = TriSolv.Rise

                                    TriSolv.SlopeIn(Bottom, Pitch)
                                    Dim botRun = TriSolv.Run
                                    Dim botRise = TriSolv.Rise

                                    Dim eave As Polyline = New Polyline

                                    Select Case Orient

                                        Case "DSD"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X - topRun,
                                                                                  topWeb.Y - topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X - botRun,
                                                                                  botWeb.Y - botRise)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                        Case "SSD"

                                            Dim topWeb As Point2d = New Point2d(topResult.Value.X, topResult.Value.Y)
                                            Dim botWeb As Point2d = New Point2d(eaveBottomResult.Value.X,
                                                                                eaveBottomResult.Value.Y)
                                            Dim topRight As Point2d = New Point2d(topWeb.X - topRun,
                                                                                  topWeb.Y - topRise)
                                            Dim botRight As Point2d = New Point2d(botWeb.X - Bottom, botWeb.Y)
                                            Dim topToe As Point2d = New Point2d(topRight.X, topRight.Y - 1)
                                            Dim botToe As Point2d = New Point2d(botRight.X, botRight.Y + 1)

                                            eave.AddVertexAt(0, botToe, 0, 0, 0)
                                            eave.AddVertexAt(1, botRight, 0, 0, 0)
                                            eave.AddVertexAt(2, botWeb, 0, 0, 0)
                                            eave.AddVertexAt(3, topWeb, 0, 0, 0)
                                            eave.AddVertexAt(4, topRight, 0, 0, 0)
                                            eave.AddVertexAt(5, topToe, 0, 0, 0)

                                    End Select

                                    Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                        OpenMode.ForWrite)

                                    btr.AppendEntity(eave)

                                    transaction.AddNewlyCreatedDBObject(eave, True)

                                End If
                            End If


                            transaction.Commit()
                            editor.WriteMessage(ControlChars.Lf)

                        Catch ex As Exception

                            editor.WriteMessage("! A Problem Has Occured - " + ex.Message)
                        Finally
                            transaction.Dispose()
                            editor.WriteMessage(ControlChars.Lf)
                        End Try
                    End Using

                    ' End Transaction
                End If

            End If
        End If
    End Sub
End Class
