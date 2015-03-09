Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class ZeeShape

    Public Guage As Integer = 16
    Public Web As Double = 8
    Public Flange As Double = 2.5

    Public GirtLeft As Boolean
    Public GirtRight As Boolean
    Public PurlinLeft As Boolean
    Public PurlinRight As Boolean

    Public Sub Draw()

        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim keywordOptions As PromptKeywordOptions =
                New PromptKeywordOptions(ControlChars.Lf + "Enter Zee Type [Girt/Purlin]", "Girt Purlin")
        Dim keywordResult As PromptResult = editor.GetKeywords(keywordOptions)

        If keywordResult.Status = PromptStatus.OK Then

            Select Case keywordResult.StringResult

                Case "Girt"

                    Dim orientOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Enter Orientation [Left/Right]", "Left Right")
                    Dim orientResult As PromptResult = editor.GetKeywords(orientOptions)

                    If orientResult.Status = PromptStatus.OK Then

                        Select Case orientResult.StringResult

                            Case "Left"

                                GirtLeft = True
                                GirtRight = False
                                PurlinLeft = False
                                PurlinRight = False
                                DrawZee()

                            Case "Right"

                                GirtLeft = False
                                GirtRight = True
                                PurlinLeft = False
                                PurlinRight = False
                                DrawZee()

                        End Select

                    End If

                Case "Purlin"

                    Dim orientOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Enter Orientation [Left/Right]", "Left Right")
                    Dim orientResult As PromptResult = editor.GetKeywords(orientOptions)

                    If orientResult.Status = PromptStatus.OK Then

                        Select Case orientResult.StringResult

                            Case "Left"

                                GirtLeft = False
                                GirtRight = False
                                PurlinLeft = True
                                PurlinRight = False
                                DrawZee()

                            Case "Right"

                                GirtLeft = False
                                GirtRight = False
                                PurlinLeft = False
                                PurlinRight = True
                                DrawZee()

                        End Select

                    End If

            End Select

        End If
    End Sub

    Public Sub DrawZee()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Dim zeeOptions As PromptPointOptions =
                New PromptPointOptions(ControlChars.Lf + "Select Zee Entry Point or [Guage/Flange/Web] : ",
                                       "Guage Flange Web")
        Dim zeeResult As PromptPointResult = editor.GetPoint(zeeOptions)

        If zeeResult.Status = PromptStatus.Keyword Then

            Select Case zeeResult.StringResult

                Case "Guage"

                    Dim guageOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Select Guage [Twelve/Fourteen/Sixteen] : ",
                                                     "Twelve Fourteen Sixteen")
                    Dim guageResult As PromptResult = editor.GetKeywords(guageOptions)

                    If (guageResult.Status = PromptStatus.OK) Then

                        Select Case guageResult.StringResult

                            Case "Twelve"
                                Guage = 12
                                DrawZee()
                            Case "Fourteen"
                                Guage = 14
                                DrawZee()
                            Case "Sixteen"
                                Guage = 16
                                DrawZee()
                        End Select

                    End If

                Case "Flange"
                    Dim flangeOptions As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Flange Length In Inches : ")
                    Dim flangeResult As PromptDoubleResult = editor.GetDistance(flangeOptions)

                    If (flangeResult.Status = PromptStatus.OK) Then

                        Flange = flangeResult.Value
                        DrawZee()

                    End If

                Case "Web"
                    Dim webOptions As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Web Depth In Inches : ")
                    Dim webResult As PromptDoubleResult = editor.GetDistance(webOptions)

                    If (webResult.Status = PromptStatus.OK) Then

                        Web = webResult.Value
                        DrawZee()

                    End If
            End Select

        ElseIf zeeResult.Status = PromptStatus.OK Then

            ' Start Transaction

            Using doc.LockDocument()
                Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                Try

                    If GirtLeft Then

                        Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                        Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - Flange)
                        Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                        Dim botPoint As Point2d = New Point2d(topPoint.X + Web, topPoint.Y)
                        Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + Flange)
                        Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                        Dim zee As Polyline = New Polyline

                        zee.AddVertexAt(0, botToe, 0, 0, 0)
                        zee.AddVertexAt(1, botLeft, 0, 0, 0)
                        zee.AddVertexAt(2, botPoint, 0, 0, 0)
                        zee.AddVertexAt(3, topPoint, 0, 0, 0)
                        zee.AddVertexAt(4, botRight, 0, 0, 0)
                        zee.AddVertexAt(5, topToe, 0, 0, 0)

                        Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                            OpenMode.ForWrite)

                        btr.AppendEntity(zee)
                        transaction.AddNewlyCreatedDBObject(zee, True)

                    ElseIf GirtRight Then


                        Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                        Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - Flange)
                        Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                        Dim botPoint As Point2d = New Point2d(topPoint.X - Web, topPoint.Y)
                        Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + Flange)
                        Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                        Dim zee As Polyline = New Polyline

                        zee.AddVertexAt(0, botToe, 0, 0, 0)
                        zee.AddVertexAt(1, botLeft, 0, 0, 0)
                        zee.AddVertexAt(2, botPoint, 0, 0, 0)
                        zee.AddVertexAt(3, topPoint, 0, 0, 0)
                        zee.AddVertexAt(4, botRight, 0, 0, 0)
                        zee.AddVertexAt(5, topToe, 0, 0, 0)

                        Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                            OpenMode.ForWrite)

                        btr.AppendEntity(zee)
                        transaction.AddNewlyCreatedDBObject(zee, True)

                    ElseIf PurlinLeft Then

                        Dim pitchOptions As PromptDoubleOptions =
                                New PromptDoubleOptions(ControlChars.Lf + "Enter Pitch : ")
                        Dim pitchResult As PromptDoubleResult = editor.GetDouble(pitchOptions)

                        If (pitchResult.Status = PromptStatus.OK) Then

                            Dim zeePitch = pitchResult.Value / 12

                            If (zeePitch = 0) Then

                                Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                                Dim topRight As Point2d = New Point2d(topPoint.X + Flange, topPoint.Y)
                                Dim topToe As Point2d = New Point2d(topRight.X + 0.6875, topRight.Y - 0.6875)
                                Dim botPoint As Point2d = New Point2d(topPoint.X, topPoint.Y - Web)
                                Dim botLeft As Point2d = New Point2d(botPoint.X - Flange, botPoint.Y)
                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                Dim zee As Polyline = New Polyline

                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                zee.AddVertexAt(4, topRight, 0, 0, 0)
                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)

                                btr.AppendEntity(zee)
                                transaction.AddNewlyCreatedDBObject(zee, True)


                            Else

                                TriSolv.SlopeIn(Flange, zeePitch)
                                Dim flangeRun = TriSolv.Run
                                Dim flangeRise = TriSolv.Rise

                                TriSolv.SlopeIn(Web, zeePitch)
                                Dim webRun = TriSolv.Run
                                Dim webRise = TriSolv.Rise

                                TriSolv.SlopeIn(1, 1)
                                Dim oneRun = TriSolv.Run
                                Dim oneRise = TriSolv.Rise

                                Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                                Dim topRight As Point2d = New Point2d(topPoint.X + flangeRun, topPoint.Y + flangeRise)
                                Dim topToe As Point2d = New Point2d(topRight.X + oneRun, topRight.Y - oneRise)
                                Dim botPoint As Point2d = New Point2d(topPoint.X + webRise, topPoint.Y - webRun)
                                Dim botLeft As Point2d = New Point2d(botPoint.X - flangeRun, botPoint.Y - flangeRise)
                                Dim botToe As Point2d = New Point2d(botLeft.X - oneRun, botLeft.Y + oneRise)

                                Dim zee As Polyline = New Polyline

                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                zee.AddVertexAt(4, topRight, 0, 0, 0)
                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)

                                btr.AppendEntity(zee)
                                transaction.AddNewlyCreatedDBObject(zee, True)

                            End If
                        End If

                    ElseIf PurlinRight Then

                        Dim pitchOptions As PromptDoubleOptions =
                                New PromptDoubleOptions(ControlChars.Lf + "Enter Pitch : ")
                        Dim pitchResult As PromptDoubleResult = editor.GetDouble(pitchOptions)

                        If (pitchResult.Status = PromptStatus.OK) Then

                            Dim zeePitch = pitchResult.Value / 12

                            If (zeePitch = 0) Then

                                Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                                Dim topRight As Point2d = New Point2d(topPoint.X + Flange, topPoint.Y)
                                Dim topToe As Point2d = New Point2d(topRight.X + 0.6875, topRight.Y - 0.6875)
                                Dim botPoint As Point2d = New Point2d(topPoint.X, topPoint.Y - Web)
                                Dim botLeft As Point2d = New Point2d(botPoint.X - Flange, botPoint.Y)
                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                Dim zee As Polyline = New Polyline

                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                zee.AddVertexAt(4, topRight, 0, 0, 0)
                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)

                                btr.AppendEntity(zee)
                                transaction.AddNewlyCreatedDBObject(zee, True)


                            Else

                                TriSolv.SlopeIn(Flange, zeePitch)
                                Dim flangeRun = TriSolv.Run
                                Dim flangeRise = TriSolv.Rise

                                TriSolv.SlopeIn(Web, zeePitch)
                                Dim webRun = TriSolv.Run
                                Dim webRise = TriSolv.Rise

                                TriSolv.SlopeIn(1, 1)
                                Dim oneRun = TriSolv.Run
                                Dim oneRise = TriSolv.Rise

                                Dim topPoint As Point2d = New Point2d(zeeResult.Value.X, zeeResult.Value.Y)
                                Dim topRight As Point2d = New Point2d(topPoint.X - flangeRun, topPoint.Y + flangeRise)
                                Dim topToe As Point2d = New Point2d(topRight.X - oneRun, topRight.Y - oneRise)
                                Dim botPoint As Point2d = New Point2d(topPoint.X - webRise, topPoint.Y - webRun)
                                Dim botLeft As Point2d = New Point2d(botPoint.X + flangeRun, botPoint.Y - flangeRise)
                                Dim botToe As Point2d = New Point2d(botLeft.X + oneRun, botLeft.Y + oneRise)

                                Dim zee As Polyline = New Polyline

                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                zee.AddVertexAt(4, topRight, 0, 0, 0)
                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)

                                btr.AppendEntity(zee)
                                transaction.AddNewlyCreatedDBObject(zee, True)

                            End If
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
    End Sub
End Class
