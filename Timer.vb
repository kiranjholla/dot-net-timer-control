Option Explicit On
Option Strict On

Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Threading

Public Class Timer

#Region "Events"
    Public Event TimeExhausted(ByVal sender As System.Object, ByVal e As System.EventArgs)
#End Region

#Region "Contants"
    Public Enum TIMER_MODES As Integer
        MODE_STOPWATCH = 1
        MODE_TIMER = 2
    End Enum

    Public Enum TIMER_ACTIONS As Integer
        ACTION_DONOTHING = 1
        ACTION_LOOP = 2
        ACTION_STOPWATCH = 3
    End Enum

    Public Enum TIMER_NOTIFICATIONS As Integer
        NOTIFY_DONOTHING = 1
        NOTIFY_BEEP = 2
    End Enum

#End Region

#Region "Private Variables"
    Private __iTimerMode As TIMER_MODES
    Private __iActionOnEnd As TIMER_ACTIONS
    Private __cColorAtEnd As Color
    Private __iNotifyOnEnd As TIMER_NOTIFICATIONS
    Private __lTimeAtStart As Long
    Private __bLapVisible As Boolean
    Private __bExcludeDayIfZero As Boolean
    Private __bExcludeHourIfZero As Boolean

    Private __bStartEnabled As Boolean
    Private __bLapEnabled As Boolean
    Private __bResetEnabled As Boolean

    Private dicLapTims As Dictionary(Of Integer, TimeSpan)
    Private iLapNum As Integer = 0
    Private iCurLap As Integer = 1

    Private stpClock As Stopwatch
#End Region

#Region "Properties"

    <CategoryAttribute("Timer")> _
    Public Property Mode() As TIMER_MODES
        Get
            Mode = __iTimerMode
        End Get
        Set(ByVal iValue As TIMER_MODES)
            __iTimerMode = iValue
            LapEnabled = CBool(IIf(iValue = TIMER_MODES.MODE_STOPWATCH, True, False))
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property LapVisible() As Boolean
        Get
            LapVisible = __bLapVisible
        End Get
        Set(ByVal bValue As Boolean)
            __bLapVisible = bValue
            If bValue Then
                Me.Size = New Size(Me.Size.Width, 49)
                'lblTime.Location = New Point(lblTime.Location.X, 20)
                lblLapNum.Visible = bValue
                lblLapTime.Visible = bValue
            Else
                LapEnabled = bValue
                lblLapNum.Visible = bValue
                lblLapTime.Visible = bValue
                'lblTime.Location = New Point(lblTime.Location.X, 3)
                Me.Size = New Size(Me.Size.Width, 29)
            End If
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property LapEnabled() As Boolean
        Get
            LapEnabled = __bLapEnabled
        End Get
        Set(ByVal bValue As Boolean)
            __bLapEnabled = bValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property ResetEnabled() As Boolean
        Get
            ResetEnabled = __bResetEnabled
        End Get
        Set(ByVal bValue As Boolean)
            __bResetEnabled = bValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property ExcluldeDayIfZero() As Boolean
        Get
            ExcluldeDayIfZero = __bExcludeDayIfZero
        End Get
        Set(ByVal bValue As Boolean)
            __bExcludeDayIfZero = bValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property ExcluldeHourIfZero() As Boolean
        Get
            ExcluldeHourIfZero = __bExcludeHourIfZero
        End Get
        Set(ByVal bValue As Boolean)
            __bExcludeHourIfZero = bValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property ActionAtEnd() As TIMER_ACTIONS
        Get
            ActionAtEnd = __iActionOnEnd
        End Get
        Set(ByVal iValue As TIMER_ACTIONS)
            __iActionOnEnd = iValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property ColorAtEnd() As Color
        Get
            ColorAtEnd = __cColorAtEnd
        End Get
        Set(ByVal cValue As Color)
            __cColorAtEnd = cValue
        End Set
    End Property

    <CategoryAttribute("Timer")> _
    Public Property NotificationMethod() As TIMER_NOTIFICATIONS
        Get
            NotificationMethod = __iNotifyOnEnd
        End Get
        Set(ByVal iValue As TIMER_NOTIFICATIONS)
            __iNotifyOnEnd = iValue
        End Set
    End Property

    <CategoryAttribute("Timer"),
    DefaultValueAttribute(0)> _
    Public Property TimeAtStart() As Long
        Get
            TimeAtStart = __lTimeAtStart
        End Get
        Set(ByVal lValue As Long)
            If lValue > 0 Then
                __lTimeAtStart = lValue
            Else
                __lTimeAtStart = 0
            End If
            lblTime.Text = formatTimeString(New TimeSpan(__lTimeAtStart * TimeSpan.TicksPerMillisecond))
        End Set
    End Property

    <CategoryAttribute("Appearance")> _
    Public Property FontTime() As Font
        Get
            Return lblTime.Font
        End Get
        Set(ByVal objValue As Font)
            lblTime.Font = objValue
        End Set
    End Property

    <CategoryAttribute("Appearance")> _
    Public Property FontLap() As Font
        Get
            Return lblLapNum.Font
        End Get
        Set(ByVal objValue As Font)
            lblLapNum.Font = objValue
            lblLapTime.Font = objValue
        End Set
    End Property

    <CategoryAttribute("Appearance")> _
    Public Property ColorTime() As Color
        Get
            Return lblTime.ForeColor
        End Get
        Set(ByVal objValue As Color)
            lblTime.ForeColor = objValue
        End Set
    End Property

    <CategoryAttribute("Appearance")> _
    Public Property ColorLap() As Color
        Get
            Return lblLapNum.ForeColor
        End Get
        Set(ByVal objValue As Color)
            lblLapNum.ForeColor = objValue
            lblLapTime.ForeColor = objValue
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        stpClock = New Stopwatch
        Process.GetCurrentProcess().ProcessorAffinity = New IntPtr(2)
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High
        Thread.CurrentThread.Priority = ThreadPriority.Highest
    End Sub

    Public Sub New(ByVal lTimeAtStart As Integer, Optional ByVal bLapVisible As Boolean = True)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        stpClock = New Stopwatch
        Process.GetCurrentProcess().ProcessorAffinity = New IntPtr(2)
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High
        Thread.CurrentThread.Priority = ThreadPriority.Highest

        Mode = TIMER_MODES.MODE_STOPWATCH
        LapVisible = bLapVisible
        TimeAtStart = lTimeAtStart
    End Sub

    Public Sub New(ByVal iMode As Integer, ByVal lTimeAtStart As Long, Optional ByVal iNotify As Integer = TIMER_NOTIFICATIONS.NOTIFY_BEEP, Optional ByVal iActionOnEnd As Integer = TIMER_ACTIONS.ACTION_STOPWATCH)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        stpClock = New Stopwatch
        Process.GetCurrentProcess().ProcessorAffinity = New IntPtr(2)
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High
        Thread.CurrentThread.Priority = ThreadPriority.Highest

        Mode = TIMER_MODES.MODE_TIMER
        TimeAtStart = lTimeAtStart
    End Sub

#End Region

#Region "Public Methods"

    Public Sub StartTimer()
        stpClock.Start()
        tmrDisplay.Enabled = True
        If IsNothing(dicLapTims) Then
            dicLapTims = New Dictionary(Of Integer, TimeSpan)
            dicLapTims.Add(0, New TimeSpan(0))
        End If
        LapEnabled = CBool(IIf(Mode = TIMER_MODES.MODE_STOPWATCH, True, False))
        ResetEnabled = False
    End Sub

    Public Sub LapTimer()
        If LapEnabled Then
            If tmrDisplay.Enabled Then
                tmrSuspend.Enabled = True
                dicLapTims.Add(iCurLap, stpClock.Elapsed)
                displayTime(stpClock.Elapsed)
                iCurLap = iCurLap + 1
            ElseIf iLapNum > 0 Then
                If iCurLap < iLapNum Then
                    iCurLap = iCurLap + 1
                Else
                    iCurLap = 1
                End If
                displayTime(dicLapTims(iCurLap))
            End If
        End If
    End Sub

    Public Sub StopTimer()
        tmrDisplay.Enabled = False
        stpClock.Stop()
        iLapNum = iCurLap
        ResetEnabled = True
    End Sub

    Public Sub ResetTimer()
        If ResetEnabled Then
            lblTime.Text = formatTimeString(New TimeSpan(TimeAtStart * TimeSpan.TicksPerMillisecond))
            stpClock.Reset()
            iLapNum = 0
            iCurLap = 1
            dicLapTims = New Dictionary(Of Integer, TimeSpan)
            dicLapTims.Add(0, New TimeSpan(0))
            LapEnabled = False
            ResetEnabled = False
        End If
    End Sub
#End Region

#Region "Private Methods"
    Private Sub tmrDisplay_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrDisplay.Tick
        If Mode = TIMER_MODES.MODE_STOPWATCH Then
            displayTime(New TimeSpan(CLng((TimeAtStart + stpClock.Elapsed.TotalMilliseconds) * TimeSpan.TicksPerMillisecond)))
        Else
            Dim timDiff As TimeSpan
            If stpClock.Elapsed.TotalMilliseconds > TimeAtStart Then
                Select Case ActionAtEnd
                    Case TIMER_ACTIONS.ACTION_STOPWATCH
                        TimeAtStart = 0
                        stpClock.Restart()
                        Mode = TIMER_MODES.MODE_STOPWATCH
                    Case TIMER_ACTIONS.ACTION_LOOP
                        stpClock.Restart()
                    Case TIMER_ACTIONS.ACTION_DONOTHING
                        StopTimer()
                End Select
                If NotificationMethod = TIMER_NOTIFICATIONS.NOTIFY_BEEP Then
                    System.Media.SystemSounds.Exclamation.Play()
                End If
                lblTime.ForeColor = ColorAtEnd
                RaiseEvent TimeExhausted(Me, e)
            Else
                timDiff = New TimeSpan(CLng((TimeAtStart - stpClock.Elapsed.TotalMilliseconds) * TimeSpan.TicksPerMillisecond))
            End If
            displayTime(timDiff)
        End If
    End Sub

    Private Sub tmrSuspend_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrSuspend.Tick
        tmrSuspend.Enabled = False
    End Sub

    Private Function formatTimeString(ByVal timTime As TimeSpan) As String
        Dim sFormat As String = "0"
        Dim sOutput As String = ""

        If timTime.Days > 0 Or Not ExcluldeDayIfZero Then
            sOutput = Format(timTime.Days, sFormat) & " " & Format(timTime.Hours, "00") & ":"
            sFormat = "00"
        ElseIf timTime.Hours > 0 Or Not ExcluldeHourIfZero Then
            sOutput = Format(timTime.Hours, sFormat) & ":"
            sFormat = "00"
        End If
        formatTimeString = sOutput & Format(timTime.Minutes, sFormat) & ":" & Format(timTime.Seconds, "00") & "." & Format(CInt(Fix(timTime.Milliseconds / 100)), "0")
    End Function

    Private Sub displayLap(ByVal timTime As TimeSpan)
        Dim timLap As TimeSpan = New TimeSpan(CLng(timTime.Ticks - dicLapTims(iCurLap - 1).Ticks))
        lblLapNum.Text = "Lap " & iCurLap
        lblLapTime.Text = formatTimeString(timLap)
    End Sub

    Private Sub displayTime(ByVal timTime As TimeSpan)
        If Not tmrSuspend.Enabled Then
            lblTime.Text = formatTimeString(timTime)
            If LapVisible Then
                displayLap(timTime)
            End If
        End If
    End Sub

#End Region

End Class
