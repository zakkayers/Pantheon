<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WallForm
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
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.Copy = New Telerik.WinControls.UI.RadButton()
        Me.Create = New Telerik.WinControls.UI.RadButton()
        Me.NameLabel = New Telerik.WinControls.UI.RadLabel()
        Me.Add = New Telerik.WinControls.UI.RadButton()
        Me.MarkCombo = New System.Windows.Forms.ComboBox()
        Me.HeightField = New Telerik.WinControls.UI.RadTextBox()
        Me.LengthField = New Telerik.WinControls.UI.RadTextBox()
        Me.ThicknessField = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel4 = New Telerik.WinControls.UI.RadLabel()
        Me.Vertical = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadToggleButton2 = New Telerik.WinControls.UI.RadToggleButton()
        Me.Horizontal = New Telerik.WinControls.UI.RadToggleButton()
        Me.TopView = New Telerik.WinControls.UI.RadToggleButton()
        Me.FrontView = New Telerik.WinControls.UI.RadToggleButton()
        Me.EndView = New Telerik.WinControls.UI.RadToggleButton()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Copy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Create, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NameLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Add, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HeightField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LengthField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ThicknessField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Vertical, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Vertical.SuspendLayout()
        CType(Me.RadToggleButton2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Horizontal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TopView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FrontView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EndView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
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
        Me.RadTitleBar1.Size = New System.Drawing.Size(182, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(28, 109)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(40, 18)
        Me.RadLabel1.TabIndex = 1
        Me.RadLabel1.Text = "Height"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(28, 136)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(41, 18)
        Me.RadLabel2.TabIndex = 2
        Me.RadLabel2.Text = "Length"
        Me.RadLabel2.ThemeName = "VisualStudio2012Dark"
        '
        'Copy
        '
        Me.Copy.Enabled = False
        Me.Copy.Image = Global.Pantheon.My.Resources.Resources.EditName
        Me.Copy.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.Copy.Location = New System.Drawing.Point(43, 54)
        Me.Copy.Name = "Copy"
        Me.Copy.Size = New System.Drawing.Size(25, 25)
        Me.Copy.TabIndex = 67
        Me.Copy.ThemeName = "VisualStudio2012Dark"
        '
        'Create
        '
        Me.Create.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Create.Location = New System.Drawing.Point(15, 405)
        Me.Create.Name = "Create"
        Me.Create.Size = New System.Drawing.Size(160, 40)
        Me.Create.TabIndex = 66
        Me.Create.Text = "Create"
        Me.Create.ThemeName = "VisualStudio2012Dark"
        '
        'NameLabel
        '
        Me.NameLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameLabel.Location = New System.Drawing.Point(12, 30)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(30, 18)
        Me.NameLabel.TabIndex = 65
        Me.NameLabel.Text = "Wall"
        Me.NameLabel.ThemeName = "VisualStudio2012Dark"
        '
        'Add
        '
        Me.Add.Image = Global.Pantheon.My.Resources.Resources.Add
        Me.Add.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.Add.Location = New System.Drawing.Point(12, 54)
        Me.Add.Name = "Add"
        Me.Add.Size = New System.Drawing.Size(25, 25)
        Me.Add.TabIndex = 63
        Me.Add.ThemeName = "VisualStudio2012Dark"
        '
        'MarkCombo
        '
        Me.MarkCombo.Enabled = False
        Me.MarkCombo.FormattingEnabled = True
        Me.MarkCombo.Location = New System.Drawing.Point(74, 58)
        Me.MarkCombo.Name = "MarkCombo"
        Me.MarkCombo.Size = New System.Drawing.Size(100, 21)
        Me.MarkCombo.TabIndex = 64
        '
        'HeightField
        '
        Me.HeightField.Location = New System.Drawing.Point(75, 108)
        Me.HeightField.Name = "HeightField"
        Me.HeightField.Size = New System.Drawing.Size(100, 21)
        Me.HeightField.TabIndex = 68
        Me.HeightField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.HeightField.ThemeName = "VisualStudio2012Dark"
        '
        'LengthField
        '
        Me.LengthField.Location = New System.Drawing.Point(75, 135)
        Me.LengthField.Name = "LengthField"
        Me.LengthField.Size = New System.Drawing.Size(100, 21)
        Me.LengthField.TabIndex = 69
        Me.LengthField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.LengthField.ThemeName = "VisualStudio2012Dark"
        '
        'ThicknessField
        '
        Me.ThicknessField.Location = New System.Drawing.Point(75, 162)
        Me.ThicknessField.Name = "ThicknessField"
        Me.ThicknessField.Size = New System.Drawing.Size(100, 21)
        Me.ThicknessField.TabIndex = 69
        Me.ThicknessField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ThicknessField.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(15, 163)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(54, 18)
        Me.RadLabel3.TabIndex = 70
        Me.RadLabel3.Text = "Thickness"
        Me.RadLabel3.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel4
        '
        Me.RadLabel4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadLabel4.Location = New System.Drawing.Point(12, 85)
        Me.RadLabel4.Name = "RadLabel4"
        Me.RadLabel4.Size = New System.Drawing.Size(84, 18)
        Me.RadLabel4.TabIndex = 71
        Me.RadLabel4.Text = "Measurements"
        Me.RadLabel4.ThemeName = "VisualStudio2012Dark"
        '
        'Vertical
        '
        Me.Vertical.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.Vertical.Controls.Add(Me.RadToggleButton2)
        Me.Vertical.Controls.Add(Me.Horizontal)
        Me.Vertical.HeaderText = "Orientation"
        Me.Vertical.Location = New System.Drawing.Point(15, 188)
        Me.Vertical.Name = "Vertical"
        Me.Vertical.Size = New System.Drawing.Size(160, 89)
        Me.Vertical.TabIndex = 72
        Me.Vertical.Text = "Orientation"
        Me.Vertical.ThemeName = "VisualStudio2012Dark"
        '
        'RadToggleButton2
        '
        Me.RadToggleButton2.Location = New System.Drawing.Point(6, 52)
        Me.RadToggleButton2.Name = "RadToggleButton2"
        Me.RadToggleButton2.Size = New System.Drawing.Size(149, 24)
        Me.RadToggleButton2.TabIndex = 1
        Me.RadToggleButton2.Text = "Vertical"
        Me.RadToggleButton2.ThemeName = "VisualStudio2012Dark"
        '
        'Horizontal
        '
        Me.Horizontal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Horizontal.Location = New System.Drawing.Point(6, 22)
        Me.Horizontal.Name = "Horizontal"
        Me.Horizontal.Size = New System.Drawing.Size(149, 24)
        Me.Horizontal.TabIndex = 0
        Me.Horizontal.Text = "Horizontal"
        Me.Horizontal.ThemeName = "VisualStudio2012Dark"
        Me.Horizontal.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'TopView
        '
        Me.TopView.Location = New System.Drawing.Point(6, 51)
        Me.TopView.Name = "TopView"
        Me.TopView.Size = New System.Drawing.Size(149, 24)
        Me.TopView.TabIndex = 3
        Me.TopView.Text = "Top View"
        Me.TopView.ThemeName = "VisualStudio2012Dark"
        '
        'FrontView
        '
        Me.FrontView.CheckState = System.Windows.Forms.CheckState.Checked
        Me.FrontView.Location = New System.Drawing.Point(5, 21)
        Me.FrontView.Name = "FrontView"
        Me.FrontView.Size = New System.Drawing.Size(150, 24)
        Me.FrontView.TabIndex = 2
        Me.FrontView.Text = "Front View"
        Me.FrontView.ThemeName = "VisualStudio2012Dark"
        Me.FrontView.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'EndView
        '
        Me.EndView.Location = New System.Drawing.Point(6, 81)
        Me.EndView.Name = "EndView"
        Me.EndView.Size = New System.Drawing.Size(149, 24)
        Me.EndView.TabIndex = 4
        Me.EndView.Text = "End View"
        Me.EndView.ThemeName = "VisualStudio2012Dark"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.FrontView)
        Me.RadGroupBox2.Controls.Add(Me.EndView)
        Me.RadGroupBox2.Controls.Add(Me.TopView)
        Me.RadGroupBox2.HeaderText = "View"
        Me.RadGroupBox2.Location = New System.Drawing.Point(15, 283)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(160, 116)
        Me.RadGroupBox2.TabIndex = 73
        Me.RadGroupBox2.Text = "View"
        Me.RadGroupBox2.ThemeName = "VisualStudio2012Dark"
        '
        'WallForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(184, 456)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.Vertical)
        Me.Controls.Add(Me.RadLabel4)
        Me.Controls.Add(Me.RadLabel3)
        Me.Controls.Add(Me.ThicknessField)
        Me.Controls.Add(Me.LengthField)
        Me.Controls.Add(Me.HeightField)
        Me.Controls.Add(Me.Copy)
        Me.Controls.Add(Me.Create)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.Add)
        Me.Controls.Add(Me.MarkCombo)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.MaximumSize = New System.Drawing.Size(184, 456)
        Me.MinimumSize = New System.Drawing.Size(184, 456)
        Me.Name = "WallForm"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = ""
        Me.ThemeName = "VisualStudio2012Dark"
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Copy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Create, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NameLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Add, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HeightField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LengthField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ThicknessField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Vertical, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Vertical.ResumeLayout(False)
        CType(Me.RadToggleButton2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Horizontal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TopView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FrontView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EndView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Copy As Telerik.WinControls.UI.RadButton
    Friend WithEvents Create As Telerik.WinControls.UI.RadButton
    Friend WithEvents NameLabel As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Add As Telerik.WinControls.UI.RadButton
    Friend WithEvents MarkCombo As System.Windows.Forms.ComboBox
    Friend WithEvents HeightField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents LengthField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents ThicknessField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel4 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Vertical As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadToggleButton2 As Telerik.WinControls.UI.RadToggleButton
    Friend WithEvents Horizontal As Telerik.WinControls.UI.RadToggleButton
    Friend WithEvents TopView As Telerik.WinControls.UI.RadToggleButton
    Friend WithEvents FrontView As Telerik.WinControls.UI.RadToggleButton
    Friend WithEvents EndView As Telerik.WinControls.UI.RadToggleButton
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
End Class

