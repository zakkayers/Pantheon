<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddPiece
    Inherits Telerik.WinControls.UI.ShapedForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RoundRectShapeTitle = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RadTitleBar1 = New Telerik.WinControls.UI.RadTitleBar()
        Me.VisualStudio2012DarkTheme1 = New Telerik.WinControls.Themes.VisualStudio2012DarkTheme()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.PieceName = New Telerik.WinControls.UI.RadTextBox()
        Me.Confirm = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PieceName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Confirm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RoundRectShapeTitle
        '
        Me.RoundRectShapeTitle.BottomLeftRounded = False
        Me.RoundRectShapeTitle.BottomRightRounded = False
        '
        'RadTitleBar1
        '
        Me.RadTitleBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.RadTitleBar1.Location = New System.Drawing.Point(1, 1)
        Me.RadTitleBar1.Name = "RadTitleBar1"
        '
        '
        '
        Me.RadTitleBar1.RootElement.ApplyShapeToControl = True
        Me.RadTitleBar1.RootElement.Shape = Me.RoundRectShapeTitle
        Me.RadTitleBar1.Size = New System.Drawing.Size(135, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(13, 31)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(111, 18)
        Me.RadLabel1.TabIndex = 1
        Me.RadLabel1.Text = "Enter Member Name"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'PieceName
        '
        Me.PieceName.Location = New System.Drawing.Point(13, 55)
        Me.PieceName.Name = "PieceName"
        Me.PieceName.Size = New System.Drawing.Size(111, 21)
        Me.PieceName.TabIndex = 2
        Me.PieceName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.PieceName.ThemeName = "VisualStudio2012Dark"
        '
        'Confirm
        '
        Me.Confirm.Location = New System.Drawing.Point(13, 83)
        Me.Confirm.Name = "Confirm"
        Me.Confirm.Size = New System.Drawing.Size(111, 27)
        Me.Confirm.TabIndex = 3
        Me.Confirm.Text = "Confirm"
        Me.Confirm.ThemeName = "VisualStudio2012Dark"
        '
        'AddPiece
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(137, 124)
        Me.Controls.Add(Me.Confirm)
        Me.Controls.Add(Me.PieceName)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.MaximumSize = New System.Drawing.Size(137, 124)
        Me.MinimumSize = New System.Drawing.Size(137, 124)
        Me.Name = "AddPiece"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = ""
        Me.ThemeName = "VisualStudio2012Dark"
        Me.TopMost = True
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PieceName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Confirm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents PieceName As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents Confirm As Telerik.WinControls.UI.RadButton
End Class

