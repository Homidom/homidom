﻿

Imports HoMIDom
Imports HoMIDom.HoMIDom
Imports HoMIDom.HoMIDom.Server
Imports HoMIDom.HoMIDom.Device
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Text
'Imports System.Web.HttpUtility

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
'Imports System.Drawing
'Imports System.Windows.Forms
Imports GsmComm.GsmCommunication
Imports GsmComm.Interfaces
Imports GsmComm.PduConverter
Imports GsmComm.Server

Imports System.Data.OleDb
Imports System.Management


' Auteur : Fabien
' Date : 09/09/2012
'-------------------------------------------------------------------------------------
'                                                                      
'-------------------------------------------------------------------------------------

' Driver GSM
''' <summary>Class Driver GSM </summary>
''' <remarks>Les drivers de votre modem doivent etre installés
''' Nécessite les dll GSMCommServer.dll,  GSMCommunication.dll, GSMCommShared.dll,  PDUConverter.dll </remarks>


<Serializable()> Public Class Driver_Gsm
    Implements HoMIDom.HoMIDom.IDriver


#Region "Variables génériques"
    '!!!Attention les variables ci-dessous doivent avoir une valeur par défaut obligatoirement
    'aller sur l'adresse http://www.somacon.com/p113.php pour avoir un ID
    ' Dim _ID As String = "7396FA30-0050-11E2-9BC2-523B6288709B"
    Dim _ID As String = "596AD836-FA1D-11E1-BDE2-639C6188709B"
    Dim _Nom As String = "GSM"
    Dim _Enable As String = False
    Dim _Description As String = "Driver GSM"
    Dim _StartAuto As Boolean = False
    Dim _Protocol As String = "GSM"
    Dim _IsConnect As Boolean = False
    Dim _IP_TCP As String = "@"
    Dim _Port_TCP As String = "@"
    Dim _IP_UDP As String = "@"
    Dim _Port_UDP As String = "@"
    Dim _Com As String = "COM6"
    Dim _Refresh As Integer = 0
    Dim _Modele As String = "GSM"
    Dim _Version As String = My.Application.Info.Version.ToString
    Dim _OsPlatform As String = "3264"
    Dim _Picture As String = ""
    Dim _Server As HoMIDom.HoMIDom.Server
    Dim _DeviceSupport As New ArrayList
    Dim _Device As HoMIDom.HoMIDom.Device
    Dim _Parametres As New ArrayList
    Dim _LabelsDriver As New ArrayList
    Dim _LabelsDevice As New ArrayList
    Dim MyTimer As New Timers.Timer
    Dim _Idsrv As String
    Dim _DeviceCommandPlus As New List(Of HoMIDom.HoMIDom.Device.DeviceCommande)

    'param avancé
    Dim _DEBUG As Boolean = False
    '    Dim _STORAGE As PhoneStorageType



#End Region

#Region "Variables Internes"

    Private ackreceived As Boolean = False
    Private comm As GsmCommMain


#End Region

#Region "Propriétés génériques"
    Public WriteOnly Property IdSrv As String Implements HoMIDom.HoMIDom.IDriver.IdSrv
        Set(ByVal value As String)
            _Idsrv = value
        End Set
    End Property

    Public Property COM() As String Implements HoMIDom.HoMIDom.IDriver.COM
        Get
            Return _Com
        End Get
        Set(ByVal value As String)
            _Com = value
        End Set
    End Property
    Public ReadOnly Property Description() As String Implements HoMIDom.HoMIDom.IDriver.Description
        Get
            Return _Description
        End Get
    End Property
    Public ReadOnly Property DeviceSupport() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.DeviceSupport
        Get
            Return _DeviceSupport
        End Get
    End Property
    Public Property Parametres() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.Parametres
        Get
            Return _Parametres
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _Parametres = value
        End Set
    End Property

    Public Property LabelsDriver() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDriver
        Get
            Return _LabelsDriver
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _LabelsDriver = value
        End Set
    End Property
    Public Property LabelsDevice() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDevice
        Get
            Return _LabelsDevice
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _LabelsDevice = value
        End Set
    End Property

    Public Event DriverEvent(ByVal DriveName As String, ByVal TypeEvent As String, ByVal Parametre As Object) Implements HoMIDom.HoMIDom.IDriver.DriverEvent

    Public Property Enable() As Boolean Implements HoMIDom.HoMIDom.IDriver.Enable
        Get
            Return _Enable
        End Get
        Set(ByVal value As Boolean)
            _Enable = value
        End Set
    End Property
    Public ReadOnly Property ID() As String Implements HoMIDom.HoMIDom.IDriver.ID
        Get
            Return _ID
        End Get
    End Property

    Public Property IP_TCP() As String Implements HoMIDom.HoMIDom.IDriver.IP_TCP
        Get
            Return _IP_TCP
        End Get
        Set(ByVal value As String)
            _IP_TCP = value
        End Set
    End Property
    Public Property IP_UDP() As String Implements HoMIDom.HoMIDom.IDriver.IP_UDP
        Get
            Return _IP_UDP
        End Get
        Set(ByVal value As String)
            _IP_UDP = value
        End Set
    End Property
    Public ReadOnly Property IsConnect() As Boolean Implements HoMIDom.HoMIDom.IDriver.IsConnect
        Get
            Return _IsConnect
        End Get
    End Property
    Public Property Modele() As String Implements HoMIDom.HoMIDom.IDriver.Modele
        Get
            Return _Modele
        End Get
        Set(ByVal value As String)
            _Modele = value
        End Set
    End Property
    Public ReadOnly Property Nom() As String Implements HoMIDom.HoMIDom.IDriver.Nom
        Get
            Return _Nom
        End Get
    End Property
    Public Property Picture() As String Implements HoMIDom.HoMIDom.IDriver.Picture
        Get
            Return _Picture
        End Get
        Set(ByVal value As String)
            _Picture = value
        End Set
    End Property
    Public Property Port_TCP() As Object Implements HoMIDom.HoMIDom.IDriver.Port_TCP
        Get
            Return _Port_TCP
        End Get
        Set(ByVal value As Object)
            _Port_TCP = value
        End Set
    End Property
    Public Property Port_UDP() As String Implements HoMIDom.HoMIDom.IDriver.Port_UDP
        Get
            Return _Port_UDP
        End Get
        Set(ByVal value As String)
            _Port_UDP = value
        End Set
    End Property
    Public ReadOnly Property Protocol() As String Implements HoMIDom.HoMIDom.IDriver.Protocol
        Get
            Return _Protocol
        End Get
    End Property
    Public Property Refresh() As Integer Implements HoMIDom.HoMIDom.IDriver.Refresh
        Get
            Return _Refresh
        End Get
        Set(ByVal value As Integer)
            _Refresh = value
        End Set
    End Property

    Public Property Server() As HoMIDom.HoMIDom.Server Implements HoMIDom.HoMIDom.IDriver.Server
        Get
            Return _Server
        End Get
        Set(ByVal value As HoMIDom.HoMIDom.Server)
            _Server = value
        End Set
    End Property
    Public ReadOnly Property Version() As String Implements HoMIDom.HoMIDom.IDriver.Version
        Get
            Return _Version
        End Get
    End Property

    Public ReadOnly Property OsPlatform() As String Implements HoMIDom.HoMIDom.IDriver.OsPlatform
        Get
            Return _OsPlatform
        End Get
    End Property
    Public Property StartAuto() As Boolean Implements HoMIDom.HoMIDom.IDriver.StartAuto
        Get
            Return _StartAuto
        End Get
        Set(ByVal value As Boolean)
            _StartAuto = value
        End Set
    End Property


#End Region

#Region "Fonctions génériques"
    ''' <summary>
    ''' Retourne la liste des Commandes avancées
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCommandPlus() As List(Of DeviceCommande)
        Return _DeviceCommandPlus
    End Function

    ''' <summary>Execute une commande avancée</summary>
    ''' <param name="MyDevice">Objet représentant le Device </param>
    ''' <param name="Command">Nom de la commande avancée à éxécuter</param>
    ''' <param name="Param">tableau de paramétres</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function ExecuteCommand(ByVal MyDevice As Object, ByVal Command As String, Optional ByVal Param() As Object = Nothing) As Boolean
        Dim retour As Boolean = False
        Try
            If MyDevice IsNot Nothing Then

                'Pas de commande demandée donc erreur
                If Command = "" Then
                    Return False
                Else
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, Me.Nom & "ExecuteCommand", Command & " ; " & Param(0))
                    Write(MyDevice, Command, Param(0), Param(1))
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "ExecuteCommand", "exception : " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Permet de vérifier si un champ est valide
    ''' </summary>
    ''' <param name="Champ"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function VerifChamp(ByVal Champ As String, ByVal Value As Object) As String Implements HoMIDom.HoMIDom.IDriver.VerifChamp
        Try
            Dim retour As String = "0"
            Select Case UCase(Champ)
                Case "ADRESSE2" ' numero de telephone 
                    If Value IsNot Nothing Then
                        If Value = "" Or Value = " " Then
                            retour = "Veuillez saisir un numero de telephone valide."
                        End If
                    End If
                Case "ADRESSE1" ' textmessage
                    If Value IsNot Nothing Then
                        If Value = "" Or Value = " " Then
                            retour = "Veuillez saisir un message"

                        End If
                        If (Len(Value) > 150) Then
                            retour = "Veuillez saisir un message de taille inférieure à 150 characteres"

                        End If

                    End If
            End Select


            Return retour
        Catch ex As Exception
            Return "Une erreur est apparue lors de la vérification du champ " & Champ & ": " & ex.ToString
        End Try
    End Function

    ''' <summary>Démarrer le du driver</summary>
    ''' <remarks></remarks>
    Public Sub Start() Implements HoMIDom.HoMIDom.IDriver.Start
        Dim retour As String
        'récupération des paramétres avancés
        Try
            _DEBUG = _Parametres.Item(0).Valeur
            '  _STORAGE = _Parametres.Item(1).Valeur
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM Start", "Erreur dans les paramétres avancés. utilisation des valeur par défaut" & ex.Message)
        End Try

        'ouverture de la communication avec le GSM
        Try
            If _Com <> "" Then
                retour = ouvrir()
            Else
                retour = "ERR: Port Com non défini. Impossible d'ouvrir le port !"
            End If
            _IsConnect = True
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "Gsm", "Driver " & Me.Nom & " démarré")
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "Gsm Start", " Driver en erreur lors du démarrage: " & ex.Message)

        End Try
    End Sub

    ''' <summary>Arrêter le du driver</summary>
    ''' <remarks></remarks>
    Public Sub [Stop]() Implements HoMIDom.HoMIDom.IDriver.Stop
        Try
            'on ferme la connexion avec le téléphone
            fermer()

            '????
            For i As Integer = 0 To _Server.Devices.Count - 1
                If _Server.Devices.Item(i).Type = "Gsm" And _Server.Devices.Item(i).Adresse1 <> "" Then
                    Dim ProcId As Object = Shell(_Server.Devices.Item(i).Adresse1 & " /exit", AppWinStyle.Hide)
                End If
            Next

            _IsConnect = False
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "Gsm", "Driver " & Me.Nom & " arrêté")
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "Gsm Stop", ex.Message)
        End Try
    End Sub

    ''' <summary>Re-Démarrer le du driver</summary>
    ''' <remarks></remarks>
    Public Sub Restart() Implements HoMIDom.HoMIDom.IDriver.Restart
        [Stop]()
        Start()
    End Sub

    ''' <summary>Intérroger un device</summary>
    ''' <param name="Objet">Objet représetant le device à interroger</param>
    ''' <remarks>pas utilisé</remarks>
    Public Sub Read(ByVal Objet As Object) Implements HoMIDom.HoMIDom.IDriver.Read
        Try
            If _Enable = False Then Exit Sub
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "Gsm Read", ex.Message)
        End Try
    End Sub

    ''' <summary>Commander un device</summary>
    ''' <param name="Objet">Objet représetant le device à interroger</param>
    ''' <param name="Command">La commande à passer</param>
    ''' <param name="Parametre1">Message text</param>
    ''' <param name="Parametre2">Phone Number</param>
    Public Sub Write(ByVal Objet As Object, ByVal Command As String, Optional ByVal Parametre1 As Object = Nothing, Optional ByVal Parametre2 As Object = Nothing) Implements HoMIDom.HoMIDom.IDriver.Write
        'Parametre1 = TxtMsg
        'Parametre2 = phoneNumber
        Try
            If _Enable = False Then Exit Sub

            If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "WRITE", "Commande: " & Command & ", Texte: " & Parametre1 & ", Composant: " & Objet.Name)
            Select Case UCase(Command)
                Case "SEND"
                    Try
                        Dim pdu As SmsSubmitPdu
                        If comm.IsConnected Then
                            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write", "IsConnected")
                            'on verifie si un texte est passé en paramétre
                            If Parametre1 = "" Then
                                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write", "SMS à envoyer vide, annulation")
                            Else
                                pdu = New SmsSubmitPdu(Parametre1, Objet.adresse1, "")
                                comm.SendMessage(pdu)
                                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write", "SMS envoyé : " & Parametre1)
                            End If
                        Else
                            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write", "No GSM Phone / Modem Connected")
                        End If
                    Catch ex As Exception
                        _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write", "Error Sending SMS: " & ex.ToString)
                    End Try

                Case "RECEIVE"
                    Try
                        If comm.IsConnected() = True Then
                            Try
                                Dim MsgLocation As Integer
                                Dim counter As Integer = 0
                                '       counter = 0

                                ' Dim messages As DecodedShortMessage() = comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Sim)
                                ' Dim messages As DecodedShortMessage() = comm.ReadMessages(PhoneMessageStatus.All, PhoneStorageType.Phone)
                                Dim messages As DecodedShortMessage() = comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Phone)
                                For Each Message In messages
                                    MsgLocation = Message.Index
                                    _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write SmsReceive", " message recu: " & Message.Data.UserDataText)
                                    ' Message.Data.UserDataText) '& (Message.Status).ToString & (Message.Storage).ToString & (Message.Index).ToString
                                    counter = counter + 1
                                    'Thread.Sleep(1000)
                                    Try
                                        comm.DeleteMessage(MsgLocation, PhoneStorageType.Phone)
                                    Catch ex As Exception
                                        _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write SmsReceive", "error deleting from inbox")
                                        Exit Sub
                                    Finally
                                        messages = Nothing
                                    End Try
                                Next
                                If counter = 0 Then
                                    _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write SmsReceive", "Aucun message n'a été recu")
                                    'comm.Close()
                                End If
                                Exit Sub

                                '  If counter > 0 Then
                                ' NewMsg = True
                                ' End If
                            Catch ex As GsmComm.GsmCommunication.CommException
                                _Server.Log(Server.TypeLog.ERREUR, Server.TypeSource.DRIVER, "GSM Write SmsReceive", "error:" & ex.InnerException.Message)
                            End Try
                        End If
                    Catch ex As GsmComm.GsmCommunication.CommException
                        _Server.Log(Server.TypeLog.ERREUR, Server.TypeSource.DRIVER, "GSM Write SmsReceive", "error:" & ex.InnerException.Message)
                    End Try

                Case "CALL"
                    _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM Write CALL", "Not Yet Implemented")
                Case Else
                    _Server.Log(Server.TypeLog.ERREUR, Server.TypeSource.DRIVER, "GSM Write", "Commande " & Command & " non gérée")
            End Select

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM Write", ex.Message)
        End Try

    End Sub

    ''' <summary>Fonction lancée lors de la suppression d'un device</summary>
    ''' <param name="DeviceId">Objet représetant le device à interroger</param>
    ''' <remarks></remarks>
    Public Sub DeleteDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.DeleteDevice
        Try

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM DeleteDevice", ex.Message)
        End Try
    End Sub

    ''' <summary>Fonction lancée lors de l'ajout d'un device</summary>
    ''' <param name="DeviceId">Objet représetant le device à interroger</param>
    ''' <remarks></remarks>
    Public Sub NewDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.NewDevice
        Try

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM NewDevice", ex.Message)
        End Try
    End Sub

    ''' <summary>ajout des commandes avancées pour les devices</summary>
    ''' <param name="nom">Nom de la commande avancée</param>
    ''' <param name="description">Description qui sera affichée dans l'admin</param>
    ''' <param name="nbparam">Nombre de parametres attendus</param>
    ''' <remarks></remarks>
    Private Sub Add_DeviceCommande(ByVal Nom As String, ByVal Description As String, ByVal NbParam As Integer)
        Try
            Dim x As New DeviceCommande
            x.NameCommand = Nom
            x.DescriptionCommand = Description
            x.CountParam = NbParam
            _DeviceCommandPlus.Add(x)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>ajout Libellé pour le Driver</summary>
    ''' <param name="nom">Nom du champ : HELP</param>
    ''' <param name="labelchamp">Nom à afficher : Aide</param>
    ''' <param name="tooltip">Tooltip à afficher au dessus du champs dans l'admin</param>
    ''' <remarks></remarks>
    Private Sub Add_LibelleDriver(ByVal Nom As String, ByVal Labelchamp As String, ByVal Tooltip As String, Optional ByVal Parametre As String = "")
        Try
            Dim y0 As New HoMIDom.HoMIDom.Driver.cLabels
            y0.LabelChamp = Labelchamp
            y0.NomChamp = UCase(Nom)
            y0.Tooltip = Tooltip
            y0.Parametre = Parametre
            _LabelsDriver.Add(y0)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>Ajout Libellé pour les Devices</summary>
    ''' <param name="nom">Nom du champ : HELP</param>
    ''' <param name="labelchamp">Nom à afficher : Aide, si = "@" alors le champ ne sera pas affiché</param>
    ''' <param name="tooltip">Tooltip à afficher au dessus du champs dans l'admin</param>
    ''' <remarks></remarks>
    Private Sub Add_LibelleDevice(ByVal Nom As String, ByVal Labelchamp As String, ByVal Tooltip As String, Optional ByVal Parametre As String = "")
        Try
            Dim ld0 As New HoMIDom.HoMIDom.Driver.cLabels
            ld0.LabelChamp = Labelchamp
            ld0.NomChamp = UCase(Nom)
            ld0.Tooltip = Tooltip
            ld0.Parametre = Parametre
            _LabelsDevice.Add(ld0)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>ajout de parametre avancés</summary>
    ''' <param name="nom">Nom du parametre (sans espace)</param>
    ''' <param name="description">Description du parametre</param>
    ''' <param name="valeur">Sa valeur</param>
    ''' <remarks></remarks>
    Private Sub Add_ParamAvance(ByVal nom As String, ByVal description As String, ByVal valeur As Object)
        Try
            Dim x As New HoMIDom.HoMIDom.Driver.Parametre
            x.Nom = nom
            x.Description = description
            x.Valeur = valeur
            _Parametres.Add(x)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>Creation d'un objet de type</summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try
            _Version = Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString

            'Liste des devices compatibles
            '_DeviceSupport.Add(ListeDevices.GENERIQUESTRING.ToString)
            _DeviceSupport.Add(ListeDevices.GENERIQUESTRING)

            'ajout des commandes avancées pour les devices
            'add_devicecommande("COMMANDE", "DESCRIPTION", nbparametre)
            Add_DeviceCommande("SEND", "envoyer un SMS", 1)
            Add_DeviceCommande("RECEIVE", "recevoir un ou des SMS", 0)
            Add_DeviceCommande("CALL", "Appeler", 0)

            Add_ParamAvance("Debug", "Activer le Debug complet (True/False)", True)
            'Add_ParamAvance("storage", "sim card : true / gsm : false", True)
            'ajout des commandes avancées pour les devices

            'Libellé Driver
            Add_LibelleDriver("HELP", "Aide...", "Pas d'aide actuellement...")

            'Libellé Device
            Add_LibelleDevice("ADRESSE1", "Numero destinataire", "")
            Add_LibelleDevice("ADRESSE2", "@", "")

            Add_LibelleDevice("SOLO", "@", "")
            Add_LibelleDevice("MODELE", "Type de stockage", "Type de stockage : GSM/SIM", "GSM|SIM")

            Add_LibelleDevice("REFRESH", "Refresh", "")
            Add_LibelleDevice("LASTCHANGEDUREE", "@", "")
            ' Add_LibelleDevice("ComPort", "COM7", "")
            Add_LibelleDevice("CommBaudRate", "57600", "Vitesse du port : 300, 600, 1200, 2400, 9600, 14400, 19200, 38400, 57600, 115200 ", "300|600|1200|2400|9600|14400|19200|38400|57600|115200")
            ' Add_LibelleDevice("CommTimeout", "LastChange Durée", "")

        Catch ex As Exception
            ' WriteLog("ERR: New Exception : " & ex.Message)
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM New", ex.Message)
        End Try
    End Sub

    ''' <summary>Si refresh >0 gestion du timer</summary>
    ''' <remarks>PAS UTILISE CAR IL FAUT LANCER UN TIMER QUI LANCE/ARRETE CETTE FONCTION dans Start/Stop</remarks>
    Private Sub TimerTick()

    End Sub

#End Region

#Region "Fonctions internes"
    ''' <summary>Ouvrir le port du modem</summary>
    Private Function ouvrir() As String
        Try
            If (_Com <> "") Then
                'ouverture du port
                If Not _IsConnect Then
                    'vitesse du port 300, 600, 1200, 2400, 9600, 14400, 19200, 38400, 57600, 115200
                    comm = New GsmCommMain(_Com, 57600, "100")
                    comm.EnableMessageNotifications()

                    ' AddHandler port.DataReceived, New SerialDataReceivedEventHandler(AddressOf DataReceived)
                    'AddHandler comm.MessageEventHandler, AddressOf ReceptionSMS

                    Return ("Port " & _Com & " ouvert")
                Else
                    Return ("Port " & _Com & " dejà ouvert")
                End If
            Else
                Return ("Port COM non défini.")
            End If
        Catch ex As Exception
            Return ("ERR: " & ex.Message)
        End Try
    End Function

    ''' <summary>Fermer le port du modem</summary>
    ''' <remarks></remarks>
    Private Function fermer() As String
        Try
            If _IsConnect Then
                If (Not (comm Is Nothing)) Then ' The COM port exists.
                    If comm.IsOpen Then
                        comm.Close()
                        _IsConnect = False
                        Return ("Port " & _Com & " fermé")
                    Else
                        Return ("Port " & _Com & "  est déjà fermé")
                    End If
                Else
                    Return ("Port " & _Com & " n'existe pas")
                End If
            Else
                Return ("Port " & _Com & "  est déjà fermé (Disconnected)")
            End If
        Catch ex As UnauthorizedAccessException
            Return ("ERR: Port " & _Com & " IGNORE")
            ' The port may have been removed. Ignore.
        End Try
        Return True
    End Function

    ''' <summary>Reception de SMS</summary>
    Private Sub ReceptionSMS()
        Try

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "GSM ReceptionSMS", "Exception" & ex.Message)
        End Try
    End Sub


    ' ''' <summary>Envoyer un SMS</summary>
    ' ''' <param name="parametre1">PhoneNumber</param>
    ' ''' <param name="parametre2">txtmsg</param>
    ' ''' <remarks></remarks>
    'Private Function sendsms(ByVal parametre1 As String, ByVal parametre2 As String) As String
    '    '_Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", Command)

    '    _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "SEND")
    '    'comm = New GsmCommMain(port_name.Substring(3), port.BaudRate, "100")

    '    Dim pdu As SmsSubmitPdu
    '    '_Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", _Com & " " & 57600) ' commx
    '    Try
    '        'comm.Open()
    '        'ouvrir(comm)
    '        If _IsConnect Then
    '            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "IsConnected")
    '            If parametre1 = "" Then
    '                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "Phone Number empty")
    '                Return ""
    '            ElseIf parametre2 = "" Then
    '                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "txt msg empty")
    '                Return ""
    '            End If
    '            Try
    '                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "before sending")

    '                pdu = New SmsSubmitPdu(parametre2, parametre1, "")
    '                comm.SendMessage(pdu)
    '                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "Message envoyé")
    '                'comm.Close()
    '            Catch E5 As Exception
    '                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "GSM", "Error Sending SMS To Destination")
    '                'comm.Close()
    '                'fermer()
    '            End Try
    '        Else
    '            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "Sms", "No GSM Phone / Modem Connected")
    '            Return ""
    '        End If

    '    Catch E5 As Exception
    '        _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "Sms", "Error Sending SMS To Destination")
    '    End Try
    '    Return ""

    'End Function

#End Region


End Class