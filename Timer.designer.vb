<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Timer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lblTime = New System.Windows.Forms.Label()
        Me.tmrDisplay = New System.Windows.Forms.Timer(Me.components)
        Me.lblLapNum = New System.Windows.Forms.Label()
        Me.lblLapTime = New System.Windows.Forms.Label()
        Me.tmrSuspend = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'lblTime
        '
        Me.lblTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTime.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(9, 20)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(141, 26)
        Me.lblTime.TabIndex = 0
        Me.lblTime.Text = "000 00:00:00.0"
        Me.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tmrDisplay
        '
        '
        'lblLapNum
        '
        Me.lblLapNum.AutoSize = True
        Me.lblLapNum.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapNum.Location = New System.Drawing.Point(6, 5)
        Me.lblLapNum.Name = "lblLapNum"
        Me.lblLapNum.Size = New System.Drawing.Size(24, 14)
        Me.lblLapNum.TabIndex = 1
        Me.lblLapNum.Text = "Lap"
        Me.lblLapNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLapTime
        '
        Me.lblLapTime.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapTime.Location = New System.Drawing.Point(63, 5)
        Me.lblLapTime.Name = "lblLapTime"
        Me.lblLapTime.Size = New System.Drawing.Size(85, 14)
        Me.lblLapTime.TabIndex = 2
        Me.lblLapTime.Text = "000 00:00:00.0"
        Me.lblLapTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tmrSuspend
        '
        Me.tmrSuspend.Interval = 4000
        '
        'Timer
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.lblLapTime)
        Me.Controls.Add(Me.lblLapNum)
        Me.Controls.Add(Me.lblTime)
        Me.Name = "Timer"
        Me.Size = New System.Drawing.Size(154, 49)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents lblTime As System.Windows.Forms.Label
    Private WithEvents lblLapNum As System.Windows.Forms.Label
    Private WithEvents lblLapTime As System.Windows.Forms.Label
    Private WithEvents tmrDisplay As System.Windows.Forms.Timer
    Private WithEvents tmrSuspend As System.Windows.Forms.Timer

End Class
