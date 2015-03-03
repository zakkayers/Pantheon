
Imports System.Drawing
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.Windows
Imports Autodesk.AutoCAD.EditorInput


Public Class Sculpt
    ' FUNCTION: MAKEBUTTON
    ' Constructs a Ribbon Button With Set Parameters
    Public Shared Function MakeButton(ByVal orientation As String,
                               ByVal size As String, ByVal text As String, ByVal desc As String,
                               ByVal image As Bitmap, ByVal largeImage As Bitmap, ByVal command As String) _
        As RibbonButton

        Dim button As RibbonButton = New RibbonButton()

        ' Set Button Orientation
        If orientation = "Vertical" Then
            button.Orientation = Windows.Controls.Orientation.Vertical
        Else
            button.Orientation = Windows.Controls.Orientation.Horizontal
        End If

        ' Set Button Size
        If size = "Large" Then
            button.Size = RibbonItemSize.Large
        Else
            button.Size = RibbonItemSize.Standard
        End If

        ' Set Button Text
        button.Text = text
        button.ShowText = True
        button.Description = desc

        ' Set Button ID
        Dim idText As String = text.Replace(" ", "")
        button.Id = idText

        ' Set Button Image
        button.Image = Read.LoadImage(image)
        button.LargeImage = Read.LoadImage(largeImage)
        button.ShowImage = True

        ' Set Command Parameter & Handler
        button.CommandParameter = command
        button.CommandHandler = New Action()

        Return button
    End Function

    ' FUNCTION: MAKESPLITBUTTON
    ' Constructs a Ribbon Split Button & Populates It With Button Items
    Public Shared Function MakeSplitButton(ByVal orientation As String,
                                    ByVal size As String, ByVal buttons As RibbonButton()) As RibbonSplitButton

        Dim button As RibbonSplitButton = New RibbonSplitButton()

        ' Set Button Orientation
        If orientation = "Vertical" Then
            button.Orientation = Windows.Controls.Orientation.Vertical
        Else
            button.Orientation = Windows.Controls.Orientation.Horizontal
        End If

        ' Set Button Size
        If size = "Large" Then
            button.Size = RibbonItemSize.Large
        Else
            button.Size = RibbonItemSize.Standard
        End If

        ' Show Text And Images
        button.ShowText = True
        button.ShowImage = True

        For i = 0 To buttons.Count() - 1

            button.Items.Add(buttons(i))

        Next
        Return button
    End Function

    ' FUNCTION: MAKELABEL
    ' Constructs a Ribbon Label & Sets Its Text 
    Public Shared Function MakeLabel(ByVal text As String) As RibbonLabel

        Dim label As RibbonLabel = New RibbonLabel
        label.Text = text
        Return label
    End Function

    ' FUNCTION: MAKERIBBON
    ' Constructs a Ribbon
    Public Function MakeRibbon(ByVal ribbonTabs As RibbonTab())

        Dim ribbon As RibbonControl = ComponentManager.Ribbon

        If (ribbon IsNot Nothing) Then


            For i = 0 To ribbonTabs.Count - 1

                Dim ribbonTab As RibbonTab = ribbon.FindTab(ribbonTabs(i).Name)

                If (ribbonTab IsNot Nothing) Then

                    ribbon.Tabs.Remove(ribbonTab)

                End If

                ribbon.Tabs.Add(ribbonTabs(i))

            Next

        End If

        Return ribbon
    End Function

    ' FUNCTION: MAKERIBBONTAB
    ' Constructs a Ribbon Tab
    Public Shared Function MakeRibbonTab(ByVal title As String, ByVal id As String) As RibbonTab

        ' Construct Ribbon Tab
        Dim ribbonTab As RibbonTab = New RibbonTab()

        ' Set Tab Title & ID
        ribbonTab.Title = title
        ribbonTab.Id = id

        Return ribbonTab
    End Function

    ' FUNCTION: MAKERIBBONPANEL
    ' Constructs a Ribbon Panel
    Public Shared Function MakeRibbonPanel(ByVal title As String, ByVal image As Bitmap, ByVal ribTab As RibbonTab,
                                    ByVal items As RibbonItemCollection) As RibbonPanelSource

        Dim ribSrc As RibbonPanelSource = New RibbonPanelSource()

        ribSrc.Title = title

        Dim panel As New RibbonPanel()
        panel.Source = ribSrc
        panel.CollapsedPanelImage = Read.LoadImage(image)

        ribTab.Panels.Add(panel)

        For i = 0 To items.Count - 1

            ribSrc.Items.Add(items(i))

        Next

        Return ribSrc
    End Function

    ' FUNCTION: MAKEAPPMENUITEM
    ' Constructs Application Menu Item
    Public Shared Function MakeAppMenuitem(ByVal name As String, ByVal desc As String, ByVal img As Bitmap, ByVal lrgImg As Bitmap, ByVal command As String) As ApplicationMenuItem

        Dim app As ApplicationMenuItem = New ApplicationMenuItem()

        app.Text = name
        app.Description = desc
        app.Image = Read.LoadImage(img)
        app.LargeImage = Read.LoadImage(lrgImg)
        app.CommandParameter = command
        app.CommandHandler = New Action()

        Return app

    End Function

    ' METHOD: MATCHBUTTONSIZE
    ' This Method Takes In A RibbonButton Array And Transforms Their Width To Match The Button With Greatest Width
    Public Shared Sub MatchButtonSize(ByVal buttons As RibbonButton())

        Dim highestWidth As Double = 0

        For i = 0 To buttons.Count - 1

            If buttons(i).Width > highestWidth Then

                highestWidth = buttons(i).Width

            End If

        Next

        For i = 0 To buttons.Count - 1

            buttons(i).MinWidth = highestWidth
            buttons(i).Width = highestWidth

        Next

    End Sub


End Class
