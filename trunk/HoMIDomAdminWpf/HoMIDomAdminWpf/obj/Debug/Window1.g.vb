﻿#ExternalChecksum("..\..\Window1.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","D5DBB07713B333EFC0B61593927BD132")
'------------------------------------------------------------------------------
' <auto-generated>
'     Ce code a été généré par un outil.
'     Version du runtime :2.0.50727.3615
'
'     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
'     le code est régénéré.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports WpfPropertyGrid
Imports WpfPropertyGrid.Controls
Imports WpfPropertyGrid.Converters


'''<summary>
'''Window1
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class Window1
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\Window1.xaml",74)
    Friend WithEvents GridTop As System.Windows.Controls.Grid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",75)
    Friend WithEvents MyLine As System.Windows.Shapes.Rectangle
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",84)
    Friend WithEvents Menu1 As System.Windows.Controls.Menu
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",86)
    Friend WithEvents MenuItem2 As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",87)
    Friend WithEvents MenuItem3 As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",88)
    Friend WithEvents MenuItem4 As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",89)
    Friend WithEvents MenuItem1 As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",92)
    Friend WithEvents Image2 As System.Windows.Controls.Image
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",94)
    Friend WithEvents GridLeft As System.Windows.Controls.Grid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",95)
    Friend WithEvents Canvas1 As System.Windows.Controls.Canvas
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",96)
    Friend WithEvents TabControl1 As System.Windows.Controls.TabControl
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",104)
    Friend WithEvents TabItem1 As System.Windows.Controls.TabItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",107)
    Friend WithEvents TreeViewDriver As System.Windows.Controls.TreeView
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",108)
    Friend WithEvents BtnStart As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",109)
    Friend WithEvents BtnStop As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",114)
    Friend WithEvents TreeViewDevice As System.Windows.Controls.TreeView
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",115)
    Friend WithEvents BtnNewDevice As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",116)
    Friend WithEvents BtnDelDevice As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",121)
    Friend WithEvents PropertyGrid1 As WpfPropertyGrid.PropertyGrid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",123)
    Friend WithEvents Grid1 As System.Windows.Controls.Grid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",124)
    Friend WithEvents Image1 As System.Windows.Controls.Image
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",125)
    Friend WithEvents CanvasRight As System.Windows.Controls.Canvas
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",127)
    Friend WithEvents GridBottom As System.Windows.Controls.Grid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",128)
    Friend WithEvents MyLine2 As System.Windows.Shapes.Rectangle
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Window1.xaml",137)
    Friend WithEvents LblStatus As System.Windows.Controls.Label
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/HoMIDomAdminWpf;component/window1.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\Window1.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.GridTop = CType(target,System.Windows.Controls.Grid)
            Return
        End If
        If (connectionId = 2) Then
            Me.MyLine = CType(target,System.Windows.Shapes.Rectangle)
            Return
        End If
        If (connectionId = 3) Then
            Me.Menu1 = CType(target,System.Windows.Controls.Menu)
            Return
        End If
        If (connectionId = 4) Then
            Me.MenuItem2 = CType(target,System.Windows.Controls.MenuItem)
            Return
        End If
        If (connectionId = 5) Then
            Me.MenuItem3 = CType(target,System.Windows.Controls.MenuItem)
            Return
        End If
        If (connectionId = 6) Then
            Me.MenuItem4 = CType(target,System.Windows.Controls.MenuItem)
            Return
        End If
        If (connectionId = 7) Then
            Me.MenuItem1 = CType(target,System.Windows.Controls.MenuItem)
            Return
        End If
        If (connectionId = 8) Then
            Me.Image2 = CType(target,System.Windows.Controls.Image)
            Return
        End If
        If (connectionId = 9) Then
            Me.GridLeft = CType(target,System.Windows.Controls.Grid)
            Return
        End If
        If (connectionId = 10) Then
            Me.Canvas1 = CType(target,System.Windows.Controls.Canvas)
            Return
        End If
        If (connectionId = 11) Then
            Me.TabControl1 = CType(target,System.Windows.Controls.TabControl)
            Return
        End If
        If (connectionId = 12) Then
            Me.TabItem1 = CType(target,System.Windows.Controls.TabItem)
            Return
        End If
        If (connectionId = 13) Then
            Me.TreeViewDriver = CType(target,System.Windows.Controls.TreeView)
            Return
        End If
        If (connectionId = 14) Then
            Me.BtnStart = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 15) Then
            Me.BtnStop = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 16) Then
            Me.TreeViewDevice = CType(target,System.Windows.Controls.TreeView)
            Return
        End If
        If (connectionId = 17) Then
            Me.BtnNewDevice = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 18) Then
            Me.BtnDelDevice = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 19) Then
            Me.PropertyGrid1 = CType(target,WpfPropertyGrid.PropertyGrid)
            Return
        End If
        If (connectionId = 20) Then
            Me.Grid1 = CType(target,System.Windows.Controls.Grid)
            Return
        End If
        If (connectionId = 21) Then
            Me.Image1 = CType(target,System.Windows.Controls.Image)
            Return
        End If
        If (connectionId = 22) Then
            Me.CanvasRight = CType(target,System.Windows.Controls.Canvas)
            Return
        End If
        If (connectionId = 23) Then
            Me.GridBottom = CType(target,System.Windows.Controls.Grid)
            Return
        End If
        If (connectionId = 24) Then
            Me.MyLine2 = CType(target,System.Windows.Shapes.Rectangle)
            Return
        End If
        If (connectionId = 25) Then
            Me.LblStatus = CType(target,System.Windows.Controls.Label)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class
