<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SaveOver
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
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.RadButton2 = New Telerik.WinControls.UI.RadButton()
        Me.RadButton3 = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.RadTitleBar1.Size = New System.Drawing.Size(221, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(12, 52)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(201, 18)
        Me.RadLabel1.TabIndex = 1
        Me.RadLabel1.Text = "Update Member Throughout Drawing?"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'RadButton1
        '
        Me.RadButton1.Location = New System.Drawing.Point(12, 76)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(201, 24)
        Me.RadButton1.TabIndex = 2
        Me.RadButton1.Text = "Confirm"
        Me.RadButton1.ThemeName = "VisualStudio2012Dark"
        '
        'RadButton2
        '
        Me.RadButton2.Location = New System.Drawing.Point(12, 106)
        Me.RadButton2.Name = "RadButton2"
        Me.RadButton2.Size = New System.Drawing.Size(201, 24)
        Me.RadButton2.TabIndex = 3
        Me.RadButton2.Text = "Create Copy"
        Me.RadButton2.ThemeName = "VisualStudio2012Dark"
        '
        'RadButton3
        '
        Me.RadButton3.Location = New System.Drawing.Point(12, 136)
        Me.RadButton3.Name = "RadButton3"
        Me.RadButton3.Size = New System.Drawing.Size(201, 24)
        Me.RadButton3.TabIndex = 4
        Me.RadButton3.Text = "Cancel"
        Me.RadButton3.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(51, 30)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(123, 18)
        Me.RadLabel2.TabIndex = 2
        Me.RadLabel2.Text = "Member Already Exists!"
        Me.RadLabel2.ThemeName = "VisualStudio2012Dark"
        '
        'SaveOver
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(223, 168)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadButton3)
        Me.Controls.Add(Me.RadButton2)
        Me.Controls.Add(Me.RadButton1)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.Name = "SaveOver"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = ""
        Me.ThemeName = "VisualStudio2012Dark"
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadButton2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadButton3 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
End Class

