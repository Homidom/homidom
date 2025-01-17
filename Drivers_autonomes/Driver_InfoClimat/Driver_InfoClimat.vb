﻿Imports HoMIDom
Imports HoMIDom.HoMIDom.Server
Imports HoMIDom.HoMIDom.Device

Imports System.Text
Imports System.String
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports STRGS = Microsoft.VisualBasic.Strings
Imports System.Net
Imports System.IO
Imports System.Web



' Auteur : jphomi
' Date : 01/11/2019

<Serializable()> Public Class Driver_InfoClimat
    Implements HoMIDom.HoMIDom.IDriver

#Region "Variables génériques"
    '!!!Attention les variables ci-dessous doivent avoir une valeur par défaut obligatoirement
    'aller sur l'adresse http://www.somacon.com/p113.php pour avoir un ID
    Dim _ID As String = "10798F9C-A1DF-11E5-809A-396B1D5D46B0" 'ne pas modifier car utilisé dans le code du serveur
    Dim _Nom As String = "InfoClimat" 'Nom du driver à afficher
    Dim _Enable As Boolean = False 'Activer/Désactiver le driver
    Dim _Description As String = "Driver InfoClimat" 'Description du driver
    Dim _StartAuto As Boolean = False 'True si le driver doit démarrer automatiquement
    Dim _Protocol As String = "Http" 'Protocole utilisé par le driver, exemple: RS232
    Dim _IsConnect As Boolean = False 'True si le driver est connecté et sans erreur
    Dim _IP_TCP As String = "@" 'Adresse IP TCP à utiliser, "@" si non applicable pour le cacher côté client
    Dim _Port_TCP As String = "@" 'Port TCP à utiliser, "@" si non applicable pour le cacher côté client
    Dim _IP_UDP As String = "@" 'Adresse IP UDP à utiliser, , "@" si non applicable pour le cacher côté client
    Dim _Port_UDP As String = "@" 'Port UDP à utiliser, , "@" si non applicable pour le cacher côté client
    Dim _Com As String = "@" 'Port COM à utiliser, , "@" si non applicable pour le cacher côté client
    Dim _Refresh As Integer = 0 'Valeur à laquelle le driver doit rafraichir les valeurs des devices (ex: toutes les 200ms aller lire les devices)
    Dim _Modele As String = "" 'Modèle du driver/interface
    Dim _Version As String = My.Application.Info.Version.ToString 'Version du driver
    Dim _OsPlatform As String = "3264" 'plateforme compatible 32 64 ou 3264 bits
    Dim _Picture As String = "" 'Image du driver (non utilisé actuellement)
    Dim _Server As HoMIDom.HoMIDom.Server 'Objet Reflètant le serveur
    Dim _DeviceSupport As New ArrayList 'Type de Device supporté par le driver
    Dim _Device As HoMIDom.HoMIDom.Device 'Image reflétant un device
    Dim _Parametres As New ArrayList 'Paramètres supplémentaires associés au driver
    Dim _LabelsDriver As New ArrayList 'Libellés, tooltip associés au driver
    Dim _LabelsDevice As New ArrayList 'Libellés, tooltip des devices associés au driver
    Dim MyTimer As New Timers.Timer 'Timer du driver
    Dim _IdSrv As String 'Id du Serveur (pour autoriser à utiliser des commandes)
    Dim _DeviceCommandPlus As New List(Of HoMIDom.HoMIDom.Device.DeviceCommande) 'Liste des commandes avancées du driver
    Dim _AutoDiscover As Boolean = False

    'A ajouter dans les ppt du driver
    Dim _urlInfoClimat As String = "http://www.infoclimat.fr"
    Dim _urlMeteoTpsRelle As String = "https://www.infoclimat.fr/observations-meteo/temps-reel/"

    'param avancé
    Dim _DEBUG As Boolean = False

#End Region

#Region "Variables Internes"
    'Insérer ici les variables internes propres au driver et non communes

    Dim liststation As Stations
    Public Class Pays
        Public abrev As String
        Public nom As String
    End Class

    Public Class Stations
        Public json As List(Of Station)
    End Class
    Public Class Station
        Public name As String
        Public abrevpays As String
        Public pays As String
        Public dept As String
        Public id As String
        Public lat As String
        Public lon As String
        Public seo As String
        Public km As String
    End Class
    Public Class DataJour
        Public Humidity As Double    '<span class="tab-units-v">%
        Public Pressure As Double    'title="Vent de direction 310&deg;"
        Public Rain As Double
        Public sum_rain_1 As Double    '<td>0 <span class="tab-units-v">mm/1h</span>
        Public Temperature As Double
        Public max_temp As Double       '<div>Maximale sur 1h : 5.9&deg;C</div>
        Public min_temp As Double       '<div>Minimale sur 1h : 5.9&deg;C</div>
        Public WindAngle As Integer     'title="Vent de direction 310&deg;"
        Public WindStrength As Integer   '<span class="tab-units-v">km/h</span>
        Public max_wind_str As Integer
        Public time_utc As String           '<b>18h00 UTC</b>"
        Public UV As Integer
    End Class


#End Region

#Region "Propriétés génériques"
    ''' <summary>
    ''' Evènement déclenché par le driver au serveur
    ''' </summary>
    ''' <param name="DriveName"></param>
    ''' <param name="TypeEvent"></param>
    ''' <param name="Parametre"></param>
    ''' <remarks></remarks>
    Public Event DriverEvent(ByVal DriveName As String, ByVal TypeEvent As String, ByVal Parametre As Object) Implements HoMIDom.HoMIDom.IDriver.DriverEvent

        ''' <summary>
        ''' ID du serveur
        ''' </summary>
        ''' <value>ID du serveur</value>
        ''' <remarks>Permet d'accéder aux commandes du serveur pour lesquels il faut passer l'ID du serveur</remarks>
        Public WriteOnly Property IdSrv As String Implements HoMIDom.HoMIDom.IDriver.IdSrv
            Set(ByVal value As String)
                _IdSrv = value
            End Set
        End Property

        ''' <summary>
        ''' Port COM du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Retourne la liste des devices supportés par le driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Voir Sub New</remarks>
        Public ReadOnly Property DeviceSupport() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.DeviceSupport
            Get
                Return _DeviceSupport
            End Get
        End Property

        ''' <summary>
        ''' Liste des paramètres avancés du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Voir Sub New</remarks>
        Public Property Parametres() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.Parametres
            Get
                Return _Parametres
            End Get
            Set(ByVal value As System.Collections.ArrayList)
                _Parametres = value
            End Set
        End Property

        ''' <summary>
        ''' Liste les libellés et tooltip des champs associés au driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LabelsDriver() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDriver
            Get
                Return _LabelsDriver
            End Get
            Set(ByVal value As System.Collections.ArrayList)
                _LabelsDriver = value
            End Set
        End Property

        ''' <summary>
        ''' Liste les libellés et tooltip des champs associés au device associé au driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LabelsDevice() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDevice
            Get
                Return _LabelsDevice
            End Get
            Set(ByVal value As System.Collections.ArrayList)
                _LabelsDevice = value
            End Set
        End Property

        ''' <summary>
        ''' Active/Désactive le driver
        ''' </summary>
        ''' <value>True si actif</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Enable() As Boolean Implements HoMIDom.HoMIDom.IDriver.Enable
            Get
                Return _Enable
            End Get
            Set(ByVal value As Boolean)
                _Enable = value
            End Set
        End Property

        ''' <summary>
        ''' ID du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ID() As String Implements HoMIDom.HoMIDom.IDriver.ID
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Adresse IP TCP du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IP_TCP() As String Implements HoMIDom.HoMIDom.IDriver.IP_TCP
            Get
                Return _IP_TCP
            End Get
            Set(ByVal value As String)
                _IP_TCP = value
            End Set
        End Property

        ''' <summary>
        ''' Adresse IP UDP du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IP_UDP() As String Implements HoMIDom.HoMIDom.IDriver.IP_UDP
            Get
                Return _IP_UDP
            End Get
            Set(ByVal value As String)
                _IP_UDP = value
            End Set
        End Property

        ''' <summary>
        ''' Permet de savoir si le driver est actif
        ''' </summary>
        ''' <value>Retourne True si le driver est démarré</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsConnect() As Boolean Implements HoMIDom.HoMIDom.IDriver.IsConnect
            Get
                Return _IsConnect
            End Get
        End Property

        ''' <summary>
        ''' Modèle du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modele() As String Implements HoMIDom.HoMIDom.IDriver.Modele
            Get
                Return _Modele
            End Get
            Set(ByVal value As String)
                _Modele = value
            End Set
        End Property

        ''' <summary>
        ''' Nom du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Nom() As String Implements HoMIDom.HoMIDom.IDriver.Nom
            Get
                Return _Nom
            End Get
        End Property

        ''' <summary>
        ''' Image du driver (non utilisé)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Picture() As String Implements HoMIDom.HoMIDom.IDriver.Picture
            Get
                Return _Picture
            End Get
            Set(ByVal value As String)
                _Picture = value
            End Set
        End Property

        ''' <summary>
        ''' Port TCP du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Port_TCP() As String Implements HoMIDom.HoMIDom.IDriver.Port_TCP
            Get
                Return _Port_TCP
            End Get
            Set(ByVal value As String)
                _Port_TCP = value
            End Set
        End Property

        ''' <summary>
        ''' Port UDP du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Port_UDP() As String Implements HoMIDom.HoMIDom.IDriver.Port_UDP
            Get
                Return _Port_UDP
            End Get
            Set(ByVal value As String)
                _Port_UDP = value
            End Set
        End Property

        ''' <summary>
        ''' Type de protocole utilisé par le driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Protocol() As String Implements HoMIDom.HoMIDom.IDriver.Protocol
            Get
                Return _Protocol
            End Get
        End Property

        ''' <summary>
        ''' Valeur de rafraichissement des devices
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Refresh() As Integer Implements HoMIDom.HoMIDom.IDriver.Refresh
            Get
                Return _Refresh
            End Get
            Set(ByVal value As Integer)
                _Refresh = value
            End Set
        End Property

        ''' <summary>
        ''' Objet représentant le serveur
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Server() As HoMIDom.HoMIDom.Server Implements HoMIDom.HoMIDom.IDriver.Server
            Get
                Return _Server
            End Get
            Set(ByVal value As HoMIDom.HoMIDom.Server)
                _Server = value
            End Set
        End Property

        ''' <summary>
        ''' Version du driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' True si le driver doit démarrer automatiquement
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartAuto() As Boolean Implements HoMIDom.HoMIDom.IDriver.StartAuto
            Get
                Return _StartAuto
            End Get
            Set(ByVal value As Boolean)
                _StartAuto = value
            End Set
        End Property

        Public Property AutoDiscover() As Boolean Implements HoMIDom.HoMIDom.IDriver.AutoDiscover
            Get
                Return _AutoDiscover
            End Get
            Set(ByVal value As Boolean)
                _AutoDiscover = value
            End Set
        End Property
#End Region

#Region "Fonctions génériques"

        ''' <summary>Retourne la liste des Commandes avancées de type DeviceCommande</summary>
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
        Public Function ExecuteCommand(ByVal MyDevice As Object, ByVal Command As String, Optional ByVal Param() As Object = Nothing) As Boolean
            Dim retour As Boolean = False
            Try
                If MyDevice IsNot Nothing Then
                    'Pas de commande demandée donc erreur
                    If Command = "" Then
                        Return False
                    Else
                        'Write(deviceobject, Command, Param(0), Param(1))
                        Select Case UCase(Command)
                            Case ""
                            Case Else
                        End Select
                        Return True
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                WriteLog("ERR: ExecuteCommand exception : " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>Permet de vérifier si un champ est valide</summary>
        ''' <param name="Champ">Nom du champ à vérifier, ex ADRESSE1</param>
        ''' <param name="Value">Valeur à vérifier</param>
        ''' <returns>Retourne 0 si OK, sinon un message d'erreur</returns>
        ''' <remarks></remarks>
        Public Function VerifChamp(ByVal Champ As String, ByVal Value As Object) As String Implements HoMIDom.HoMIDom.IDriver.VerifChamp
            Try
                Dim retour As String = "0"
                Select Case UCase(Champ)
                    Case "ADRESSE1"

                    Case "ADRESSE2"

                End Select
                Return retour
            Catch ex As Exception
                Return "Une erreur est apparue lors de la vérification du champ " & Champ & ": " & ex.ToString
            End Try
        End Function

        ''' <summary>Démarrer le driver</summary>
        ''' <remarks></remarks>
        Public Sub Start() Implements HoMIDom.HoMIDom.IDriver.Start
            Try
                'récupération des paramétres avancés
                If My.Computer.Network.IsAvailable = False Then
                    _IsConnect = False
                    WriteLog("ERR: Start, Pas d'accés réseau! Vérifiez votre connection")
                    WriteLog("ERR: Driver non démarré")
                    Exit Sub
                End If

                Try
                    _DEBUG = _Parametres.Item(0).Valeur
                Catch ex As Exception
                    _DEBUG = False
                    _Parametres.Item(0).Valeur = False
                    WriteLog("ERR: Start, Erreur dans les paramétres avancés. utilisation des valeur par défaut : " & ex.Message)
                End Try

                If GetStations() Then
                    WriteLog("Start, connection au serveur " & _urlInfoClimat)
                    _IsConnect = True
                Else
                    _IsConnect = False
                    WriteLog("ERR: Start, connection au serveur " & _urlInfoClimat & " impossible")
                End If

                'GetData()

            Catch ex As Exception
                _IsConnect = False
                WriteLog("ERR: Start, Erreur démarrage " & ex.Message)
                WriteLog("ERR: Start, Driver non démarré")
            End Try
        End Sub

        ''' <summary>Arrêter le du driver</summary>
        ''' <remarks></remarks>
        Public Sub [Stop]() Implements HoMIDom.HoMIDom.IDriver.Stop
            Try
                _IsConnect = False
                WriteLog("Driver arrêté")
            Catch ex As Exception
                WriteLog("ERR: Stop, Erreur arrêt " & ex.Message)
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
        ''' <remarks>Le device demande au driver d'aller le lire suivant son adresse</remarks>
        Public Sub Read(ByVal Objet As Object) Implements HoMIDom.HoMIDom.IDriver.Read
            Try
                If _Enable = False Then
                    WriteLog("ERR: Read, Erreur: Impossible de traiter la commande car le driver n'est pas activé (Enable)")
                    Exit Sub
                End If

                GetData(Objet)

            Catch ex As Exception
                WriteLog("ERR: Read, adresse1 : " & Objet.adresse1 & " - adresse2 : " & Objet.adresse2)
                WriteLog("ERR: Read, Exception : " & ex.Message)
            End Try
        End Sub

        ''' <summary>Commander un device</summary>
        ''' <param name="Objet">Objet représetant le device à commander</param>
        ''' <param name="Command">La commande à passer</param>
        ''' <param name="Parametre1">parametre 1 de la commande, optionnel</param>
        ''' <param name="Parametre2">parametre 2 de la commande, optionnel</param>
        ''' <remarks></remarks>
        Public Sub Write(ByVal Objet As Object, ByVal Command As String, Optional ByVal Parametre1 As Object = Nothing, Optional ByVal Parametre2 As Object = Nothing) Implements HoMIDom.HoMIDom.IDriver.Write
            Try
                If _Enable = False Then
                    WriteLog("ERR: Write, Erreur: Impossible de traiter la commande car le driver n'est pas activé (Enable)")
                    Exit Sub
                End If

                If _IsConnect = False Then
                    WriteLog("ERR: Write, Erreur: Impossible de traiter la commande car le driver n'est pas connecté")
                    Exit Sub
                End If
            Catch ex As Exception
                WriteLog("ERR: WRITE, Exception : " & ex.Message)
            End Try
        End Sub

        ''' <summary>Fonction lancée lors de la suppression d'un device</summary>
        ''' <param name="DeviceId">Objet représetant le device à interroger</param>
        ''' <remarks></remarks>
        Public Sub DeleteDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.DeleteDevice
            Try

            Catch ex As Exception
                WriteLog("ERR: DeleteDevice, Exception : " & ex.Message)
            End Try
        End Sub

        ''' <summary>Fonction lancée lors de l'ajout d'un device</summary>
        ''' <param name="DeviceId">Objet représetant le device à interroger</param>
        ''' <remarks></remarks>
        Public Sub NewDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.NewDevice
            Try

            Catch ex As Exception
                WriteLog("ERR: NewDevice, Exception : " & ex.Message)
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
                WriteLog("ERR: add_DeviceCommande, Exception :" & ex.Message)
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
                WriteLog("ERR: add_LibelleDriver, Exception : " & ex.Message)
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
                WriteLog("ERR: add_LibelleDevice, Exception : " & ex.Message)
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
                WriteLog("ERR: add_ParamAvance, Exception : " & ex.Message)
            End Try
        End Sub

        ''' <summary>Creation d'un objet de type</summary>
        ''' <remarks></remarks>
        Public Sub New()
            Try
                _Version = Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString

                'liste des devices compatibles
                _DeviceSupport.Add(ListeDevices.BAROMETRE)
                _DeviceSupport.Add(ListeDevices.DIRECTIONVENT)
                _DeviceSupport.Add(ListeDevices.HUMIDITE)
                _DeviceSupport.Add(ListeDevices.METEO)
                _DeviceSupport.Add(ListeDevices.PLUIECOURANT)
                _DeviceSupport.Add(ListeDevices.TEMPERATURE)
                _DeviceSupport.Add(ListeDevices.UV)
                _DeviceSupport.Add(ListeDevices.VITESSEVENT)

                'Parametres avancés
                Add_ParamAvance("Debug", "Activer le Debug complet (True/False)", False)

                'ajout des commandes avancées pour les devices
                'add_devicecommande("COMMANDE", "DESCRIPTION", nbparametre)
                'add_devicecommande("PRESETDIM", "permet de paramétrer le DIM : param1=niveau, param2=timer", 2)

                'Libellé Driver
                'add_libelledriver("HELP", "Aide...", "Pas d'aide actuellement...")

                'Libellé Device
                Add_LibelleDevice("ADRESSE1", "Pays", "")
                Add_LibelleDevice("ADRESSE2", "Station météo", "")
                Add_LibelleDevice("SOLO", "@", "")
                Add_LibelleDevice("MODELE", "@", "")
                'Add_LibelleDevice("REFRESH", "Refresh (sec)", "Valeur de rafraîchissement de la mesure en secondes")
                'Add_LibelleDevice("LASTCHANGEDUREE", "LastChange Durée", "")

            Catch ex As Exception
                WriteLog("ERR: New, Exception : " & ex.Message)
            End Try
        End Sub

        ''' <summary>Si refresh >0 gestion du timer</summary>
        ''' <remarks>PAS UTILISE CAR IL FAUT LANCER UN TIMER QUI LANCE/ARRETE CETTE FONCTION dans Start/Stop</remarks>
        Private Sub TimerTick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)
            Try

            Catch ex As Exception
                WriteLog("ERR: TimerTick, " & ex.Message)
            End Try
        End Sub

#End Region

#Region "Fonctions internes"
        'Insérer ci-dessous les fonctions propres au driver et nom communes (ex: start)
        Private Function GetStations()
            Try
                Dim responsebodystr As String = ""
                Dim request As HttpWebRequest
                Dim response As HttpWebResponse

                request = CType(WebRequest.Create("https://www.infoclimat.fr/observations-meteo/temps-reel/paris-montsouris/07156.html"), HttpWebRequest)
                request.KeepAlive = True

                response = request.GetResponse()
                Dim responsereader = New StreamReader(response.GetResponseStream())
                responsebodystr = responsereader.ReadToEnd()
                responsereader.Close()
                response.Close()
                Dim cookieToken As CookieContainer = request.CookieContainer

                responsebodystr = HttpUtility.HtmlDecode(responsebodystr)

            'Dim strtmpbody As String = Mid(responsebodystr, InStr(responsebodystr, "<option data-seo="), Len(responsebodystr))
            'strtmpbody = Mid(strtmpbody, 1, InStr(strtmpbody, "</select>"))
            'Dim strtmp As String
            'Dim stationtmp As New Station
            'stations.Clear()

            'While InStr(strtmpbody, "</option>") > 0
            '    strtmp = Mid(strtmpbody, 1, InStr(strtmpbody, "</option>") + 9)
            '    stationtmp = New Station
            '    stationtmp.data_seo = Mid(strtmp, InStr(strtmpbody, "-seo=") + 6, InStr(strtmp, """ value=") - (InStr(strtmp, "-seo=") + 6))
            '    stationtmp.data_seo.Replace(" ", "")
            '    stationtmp.code_station = Mid(strtmp, InStr(strtmp, "value=") + 7, 5)
            '    stationtmp.code_station.Replace(" ", "")
            '    If InStr(strtmp, """ class=") > 0 Then
            '        stationtmp.type_station = Mid(strtmp, InStr(strtmp, "class=") + 7, (InStr(strtmp, """>")) - (InStr(strtmp, "class=") + 7))
            '    End If
            '    stationtmp.nom_station = Mid(strtmp, InStr(strtmp, """>") + 2, (InStr(strtmp, "</option>")) - (InStr(strtmp, """>") + 2))
            '    stations.Add(stationtmp)
            '    strtmpbody = strtmpbody.Replace(strtmp, "")
            'End While
            'WriteLog(stations.Count & " stations trouvées")
            '   WriteLog("derniere station -> " & stations.Item(stations.Count - 1).nom_station)
            '-------------------------------------------------------------------------------------------------------------------------------------------
            Dim strtmp As String = ""
            Dim strtmpbody = Mid(responsebodystr, InStr(responsebodystr, "select_pays"), Len(responsebodystr))
            strtmpbody = Mid(strtmpbody, 1, InStr(strtmpbody, "</select>"))
            strtmpbody = Mid(strtmpbody, InStr(strtmpbody, "</option>") + 9, Len(strtmpbody))

            'recherche des pay
            Dim listpays As New List(Of Pays)
            Dim paystmp As New Pays
            listpays.Add(paystmp) ' pour n'avoir pas une liste vide et décaler l'index du prenier enreg à 1

            While InStr(strtmpbody, "</option>") > 0
                strtmp = Mid(strtmpbody, 1, InStr(strtmpbody, "</option>") + 9)
                paystmp = New Pays
                paystmp.abrev = Mid(strtmp, InStr(strtmp, "value=") + 7, InStr(strtmp, """>") - (InStr(strtmp, "value") + 7))
                paystmp.nom = Mid(strtmp, InStr(strtmp, """>") + 2, (InStr(strtmp, "</option>")) - (InStr(strtmp, """>") + 2))
                listpays.Add(paystmp)
                strtmpbody = strtmpbody.Replace(strtmp, "")
            End While
            WriteLog("Nbre de pays trouvés -> " & listpays.Count)

            'recherche ds stations
            request = CType(WebRequest.Create("https://www.infoclimat.fr/stations-meteo/getData.php?id=07156&id_txt=LFPW&lat=48.8217&lon=2.33778&pays=FR"), HttpWebRequest)
                request.KeepAlive = True
                request.Referer = "https://www.infoclimat.fr/observations-meteo/temps-reel/paris-montsouris/07156.html"
                request.Accept = "application/json, text/javascript, */*; q=0.01"
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0"
                request.Host = "www.infoclimat.fr"
                request.CookieContainer = cookieToken
                request.Headers.Add("TE", "Trailers")
                request.Headers.Add("Accept-Language", "fr-FR")
                request.Headers.Add("X-Requested-With", "XMLHttpRequest")

                response = request.GetResponse()
                responsereader = New StreamReader(response.GetResponseStream())
                responsebodystr = responsereader.ReadToEnd()
                responsereader.Close()
                response.Close()

            'Met en forme la chaine
            responsebodystr = "{""JSON"":" & responsebodystr & "}"
                responsebodystr = HttpUtility.HtmlDecode(responsebodystr)

            liststation = Newtonsoft.Json.JsonConvert.DeserializeObject(responsebodystr, GetType(Stations))

            'renseigne le pays
            For i = 0 To liststation.json.Count - 1
                If IsNumeric(liststation.json.Item(i).dept) Then
                    liststation.json.Item(i).pays = "FR"
                Else
                    If liststation.json.Item(i).dept = "2A" Or liststation.json.Item(i).dept = "2B" Then
                        liststation.json.Item(i).pays = "FR"
                    Else
                        liststation.json.Item(i).pays = liststation.json.Item(i).dept
                    End If
                End If
                liststation.json.Item(i).abrevpays = liststation.json.Item(i).pays
            Next

            'tri par ordre alphabétique sur le nom de la station
            Dim liststationclassees = liststation.json.OrderBy(Function(s) s.name).ToList
            WriteLog("Nbre de stations trouvées -> " & liststation.json.Count)

            'renseigne les champs adr1 et adr2
            Dim IdAdr1 As String = ""
            Dim IdAdr2 As String = ""
            Dim indexpays As Integer = 0
            For i = 0 To liststationclassees.Count - 1
                If InStr(IdAdr1, liststationclassees(i).pays) = 0 Then             ' evite les doublons 
                    indexpays = (From n In listpays
                                 Where n.abrev = liststationclassees(i).abrevpays
                                 Select listpays.IndexOf(n)).FirstOrDefault()
                    If indexpays = 0 Then
                        IdAdr1 += liststationclassees(i).abrevpays & " # " & "" & "|"
                    Else
                        IdAdr1 += liststationclassees(i).abrevpays & " # " & listpays.Item(indexpays).nom & "|"
                    End If
                End If
                IdAdr2 += liststationclassees(i).abrevpays & " #; " & liststationclassees(i).name & " # " & liststationclassees(i).id & "|"
            Next

            Dim ld0 As New HoMIDom.HoMIDom.Driver.cLabels
            For i As Integer = 0 To _LabelsDevice.Count - 1
                    ld0 = _LabelsDevice(i)
                    Select Case ld0.NomChamp
                        Case "ADRESSE1"
                            IdAdr1 = Mid(IdAdr1, 1, Len(IdAdr1) - 1) 'enleve le dernier | pour eviter davoir une ligne vide a la fin
                            ld0.Parametre = IdAdr1
                            _LabelsDevice(i) = ld0
                        Case "ADRESSE2"
                            IdAdr2 = Mid(IdAdr2, 1, Len(IdAdr2) - 1) 'enleve le dernier | pour eviter davoir une ligne vide a la fin
                            ld0.Parametre = IdAdr2
                            _LabelsDevice(i) = ld0
                    End Select
                Next

                Return True

            Catch ex As Exception
                WriteLog("ERR: GetStation, " & ex.Message)
                Return False
            End Try
        End Function

    Private Function GetData(ByVal objet As Object)
        Try
            Dim responsebodystr As String = ""
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse

            Dim nomstation As String = ""
            Dim numstation As String = ""
            numstation = objet.adresse2
            ' numstation = " rodez # 07552" 
            'numstation = " firmi # 000DC"
            ' numstation = " anglars-saint-felix # 000CI"
            numstation = Mid(numstation, InStr(numstation, "#") + 2, Len(numstation))
            Dim liststationclassees = liststation.json.OrderBy(Function(s) s.name).ToList

            Dim indexstation As Integer = (From n In liststationclassees
                                           Where n.id = numstation
                                           Select liststationclassees.IndexOf(n)).FirstOrDefault()
            nomstation = liststationclassees.Item(indexstation).seo

            Dim adrs As String = "https://www.infoclimat.fr/observations-meteo/temps-reel/" & nomstation & "/" & numstation & ".html"
            WriteLog("DBG: GetData, url -> " & adrs)
            If nomstation = "" Then Return False

            request = CType(WebRequest.Create(adrs), HttpWebRequest)
            request.AllowAutoRedirect = False
            request.KeepAlive = True

            response = request.GetResponse()
            Dim responsereader = New StreamReader(response.GetResponseStream())
            responsebodystr = HttpUtility.HtmlDecode(responsereader.ReadToEnd())
            responsereader.Close()
            response.Close()

            '           responsebodystr = "<!--BENCH[tz](l:1347)=0.01171612739563--><p class=""title-catd"" style=""margin:0;font-size:14px;text-align:right;padding-right:10px""><a href=""?metar"" style=""float:right"">Afficher les relev&eacute;s interm&eacute;diaires <span style=""font-size:12px"">et METAR</span> &raquo;</a></p><table id=""tableau-releves"" cellspacing=""0""><thead><tr class=""degrade-vertical-gris""><th>Heure</th><th class=""releve-colonne-odd"" <> a href=""javascript: Void(0)"" Class=""tipsy-trigger"" title=""Temps observ&eacute; &agrave; la station au moment du report"">Temps</a></th><th><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Temp&eacute;rature sous abri normalis&eacute;,<br />relev&eacute;e entre 1m50 et 2m du sol"" > Temp&eacute;rature</a></th>"
            '          responsebodystr =responsebodystr & "<th Class=""releve-colonne-odd""><a href=""javascript: Void(0)"" Class=""tipsy-trigger"" title=""<b>Temp&eacute;rature ressentie</b><br />Elle correspond au windchill (indice de refroidissement &eacute;olien) lorsque la temp&eacute;rature est inf&eacute;rieure &agrave; 10&deg;C, et &agrave; l'humidex (indice de chaleur) lorsque la temp&eacute;rature est sup&eacute;rieure &agrave; 20&deg;C. <b>Ces donn&eacute;es n'ont pas d'unit&eacute; et ne correspondent pas &agrave; une temp&eacute;rature observ&eacute;e.</b> <br />Cette colonne affiche aussi, si disponible, les valeurs d'indice UV et de radiations solaires."">Biom&eacute;t&eacute;o</a></th><th><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Pr&eacute;cipitations tomb&eacute;es en 1h"">Pluie</a></th>"
            '           responsebodystr =responsebodystr & "<th Class=""releve-colonne-odd""><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Humidit&eacute; relative"">Humidit&eacute;</a></th><th><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Point de ros&eacute;e observ&eacute; &agrave; 2m"">Pt. de ros&eacute;e</a></th><th Class=""releve-colonne-odd""><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Vent moyen et en rafales, observ&eacute; &agrave; 10m"">Vent moyen (raf.)</a></th><th><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Pression atmosph&eacute;rique ramen&eacute;e au niveau de la mer"">Pression</a></th><th Class=""releve-colonne-odd""><a href=""javascript:void(0)"" Class=""tipsy-trigger"" title=""Visibilit&eacute; horizontale"">Visibilit&eacute;</a></th>"
            '          responsebodystr = responsebodystr & "</tr></thead><tbody><tr style = ""background-color:RGB(220, 220, 220)"" id=""cdata0"" class=""cdata-hour18""><td style=""border-left:2px solid blue""><span class=""tipsy-trigger"" title=""Heure r&eacute;elle d'&eacute;mission :<br />01/12/2019<br /><b>18h00 UTC</b>"">19h</span></td><td style=""background-color:rgba(0,0,0,0.1)""></td><td><span title="""" class="""" style=""font-weight:bold;margin-top:10px;display:inline-block;font-size:16px"">4</span> <span class=""tab-units-v"">&deg;C</span><span class=""color-heatmap"" style=""background-color:rgb(13,253,51)""></span></td><td style""=""background-color:rgba(0,0,0,0.1)""></td><td></td>"
            '          responsebodystr = responsebodystr & "<td style=""background-color:rgba(0,0,0,0.1)""></td><td><span style""=""font-weight:bold;margin-top:10px;display:inline-block"">4</span> <span class=""tab-units-v"">&deg;C</span><span class=""color-heatmap"" style=""background-color:RGB(13, 253, 51)""></span></td><td style=""background-color:rgba(0, 0, 0, 0.1)""><span style=""font-weight:bold"">13</span> <span class=""tab-units-v"">km/h</span><div style=""float: Left();margin-left:3px;margin-top:10px;width:20px;height:20px;background-image:url(//static.infoclimat.net/images/pictos_vent2/sprite.png);background-position:-20px 0px;"" title=""Vent de direction 300&deg;"" alt=""vent"" class=""tipsy-trigger""></div></td><td>1013<span class=""tab-units-v"">hPa</span><span class=""color-heatmap"" style=""background-color:RGB(134, 233, 73)""></span><br />=</td><td style=""background-color:rgba(0, 0, 0, 0.1)"">10 <span Class=""tab-units-v"">km</span></td></tr>"
            '            responsebodystr = HttpUtility.HtmlDecode(responsebodystr)

            'recherche du nom des colonne
            Dim strtmpcol As String = ""
            strtmpcol = Mid(responsebodystr, InStr(responsebodystr, "<!--BENCH"), Len(responsebodystr))
            strtmpcol = Mid(strtmpcol, InStr(strtmpcol, "<tr"), Len(strtmpcol))
            strtmpcol = Mid(strtmpcol, 1, InStr(strtmpcol, " id=""cdata0"))
            '     WriteLog("DBG: GetData, responsebodycol -> " & strtmpcol)

            'recupération tableau colonne
            Dim ListColonne As List(Of String) = New List(Of String)
            While Len(strtmpcol) > 70
                ListColonne.Add(Mid(strtmpcol, 1, InStr(strtmpcol, "</th>") + 4))
                '    WriteLog("DBG: GetData, ListColonne(i) -> " & ListColonne.Item(ListColonne.Count - 1))
                strtmpcol = strtmpcol.Replace(ListColonne.Item(ListColonne.Count - 1), "")
            End While

            'recherche des données de l'heure précedente, on prend un enregistrement complet
            Dim strtmpbody As String = ""
            strtmpbody = Mid(responsebodystr, InStr(responsebodystr, " id=""cdata0"), Len(responsebodystr))
            strtmpbody = Mid(strtmpbody, 1, InStr(strtmpbody, "</tr>"))
            '     WriteLog("DBG: GetData, responsebody -> " & strtmpbody)

            'recupération tableau data
            Dim Tableaustr As List(Of String) = New List(Of String)
            Dim i As Integer
            While Len(strtmpbody) > 5
                Tableaustr.Add(Mid(strtmpbody, 1, InStr(strtmpbody, "</td>") + 4))
                '    WriteLog("DBG: GetData, Tableaustr -> " & Tableaustr.Item(Tableaustr.Count - 1))
                strtmpbody = Mid(strtmpbody, Tableaustr.Item(Tableaustr.Count - 1).Length + 1, Len(strtmpbody))
                '                WriteLog("DBG: GetData, strtmpbody -> " & strtmpbody)
            End While

            Dim strtmp As String = ""
            Dim DataJourTmp As New DataJour
            'boucle sur le tableau pour visualiser le résultat
            For i = 0 To Tableaustr.Count - 1
                Select Case True
                    Case InStr(ListColonne.Item(i), "Heure")
                        DataJourTmp.time_utc = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "<b>") + 3, 5)
                    Case InStr(ListColonne.Item(i), "Température</a>")
                        '                       WriteLog("DBG: GetData, Tableau(i) -> temperature")
                        If InStr(Tableaustr.Item(i), "Minimale sur 1h :") Then
                            strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "Minimale sur 1h :") + 18, 5)
                            DataJourTmp.min_temp = Mid(strtmp, 1, InStr(strtmp, "°") - 1)
                            'tmp maxi
                            strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "Maximale sur 1h :") + 18, 5)
                            DataJourTmp.max_temp = Mid(strtmp, 1, InStr(strtmp, "°") - 1)
                        End If
                        'tmp heure
                        strtmp = Mid(Tableaustr.Item(i), 1, InStr(Tableaustr.Item(i), "</span>") - 1)
                        DataJourTmp.Temperature = Mid(strtmp, InStr(strtmp, "px"">") + 4, Len(strtmp))
                    Case InStr(ListColonne.Item(i), "Biométéo") And InStr(Tableaustr.Item(i), "W/m²")
                        '                       WriteLog("DBG: GetData, Tableau(i) -> biometeo")
                        If InStr(Tableaustr.Item(i), "W/m²") Then
                            strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "W/m²"), Len(Tableaustr.Item(i)))
                            strtmp = Mid(strtmp, 1, InStr(strtmp, "</span>") - 2)
                            '      WriteLog("DBG: GetData, strtmp -> " & strtmp)
                            DataJourTmp.UV = Mid(strtmp, InStr(strtmp, "<span>") + 6, Len(strtmp))
                        End If
                    Case InStr(ListColonne.Item(i), "Pluie") And InStr(Tableaustr.Item(i), "mm/1h")
                        '                   WriteLog("DBG: GetData, Tableau(i) -> pluie")
                        strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), ">") + 1, InStr(Tableaustr.Item(i), "<span") - 1)
                        DataJourTmp.Rain = Mid(strtmp, 1, InStr(strtmp, "<s") - 1)
                    Case InStr(ListColonne.Item(i), "Humidité") And InStr(Tableaustr.Item(i), "%")
                        '                  WriteLog("DBG: GetData, Tableau(i) -> humidite")
                        strtmp = Mid(Tableaustr.Item(i), 1, InStr(Tableaustr.Item(i), "</span>") - 1)
                        strtmp = Mid(strtmp, Len(strtmp) - 10, Len(strtmp))
                        DataJourTmp.Humidity = Mid(strtmp, InStr(strtmp, """>") + 2, Len(strtmp))
                    Case InStr(ListColonne.Item(i), "Vent moyen") And InStr(Tableaustr.Item(i), "km/h")
                        '               WriteLog("DBG: GetData, Tableau(i) -> vent")
                        strtmp = Mid(Tableaustr.Item(i), 1, InStr(Tableaustr.Item(i), "</span>") - 1)
                        strtmp = Mid(strtmp, Len(strtmp) - 10, Len(strtmp))
                        ' WriteLog("DBG: GetData, Tableau(i) -> vent direct")
                        DataJourTmp.WindStrength = Mid(strtmp, InStr(strtmp, """>") + 2, Len(strtmp))
                        strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "Vent de direction") + 18, Len(Tableaustr.Item(i)))
                        DataJourTmp.WindAngle = Mid(strtmp, 1, InStr(strtmp, "°") - 1)
                        If InStr(Tableaustr.Item(i), "Rafale sur 10mn") > 0 Then
                            '  WriteLog("DBG: GetData, Tableau(i) -> vent rafale")
                            strtmp = Mid(Tableaustr.Item(i), InStr(Tableaustr.Item(i), "Rafale sur 10mn"), Len(Tableaustr.Item(i)))
                            strtmp = Mid(strtmp, 1, InStr(strtmp, "</span>") - 1)
                            strtmp = Mid(strtmp, InStr(strtmp, "<span"), Len(strtmp))
                            DataJourTmp.max_wind_str = Mid(strtmp, InStr(strtmp, ">") + 1, Len(strtmp))
                        End If
                    Case InStr(ListColonne.Item(i), "Pression") > 0 And InStr(Tableaustr.Item(i), "hPa") > 0
                        '                     WriteLog("DBG: GetData, Tableau(i) -> pression")
                        strtmp = Mid(Tableaustr.Item(i), 1, InStr(Tableaustr.Item(i), "</span>") - 1)
                        If InStr(strtmp, "Pression minimale") > 0 Then
                            strtmp = Mid(strtmp, InStr(strtmp, """>"), Len(strtmp))
                            strtmp = Mid(strtmp, 1, InStr(strtmp, "<span ") - 1)
                        Else
                            strtmp = Mid(strtmp, 1, InStr(strtmp, "<span ") - 1)
                            strtmp = Mid(strtmp, InStr(strtmp, ">"), Len(strtmp))
                        End If
                        DataJourTmp.Pressure = Mid(strtmp, InStr(strtmp, ">") + 1, Len(strtmp))
                End Select
            Next i

            WriteLog("DBG: GetData, datajourtmp.time_utc -> " & DataJourTmp.time_utc)
            WriteLog("DBG: GetData, DataJourTmp.min_temp -> " & DataJourTmp.min_temp)
            WriteLog("DBG: GetData, DataJourTmp.max_temp -> " & DataJourTmp.max_temp)
            WriteLog("DBG: GetData, DataJourTmp.temperature -> " & DataJourTmp.Temperature)
            WriteLog("DBG: GetData, DataJourTmp.uv -> " & DataJourTmp.UV)
            WriteLog("DBG: GetData, DataJourTmp.pluie -> " & DataJourTmp.Rain)
            WriteLog("DBG: GetData, DataJourTmp.humidité -> " & DataJourTmp.Humidity)
            WriteLog("DBG: GetData, DataJourTmp.vitesse vent -> " & DataJourTmp.WindStrength)
            WriteLog("DBG: GetData, DataJourTmp.angle vent -> " & DataJourTmp.WindAngle)
            WriteLog("DBG: GetData, DataJourTmp.rafale vent -> " & DataJourTmp.max_wind_str)
            WriteLog("DBG: GetData, DataJourTmp.pression -> " & DataJourTmp.Pressure)

            'releve de la batterie device/module
            Select Case objet.Type
                Case "METEO"
                    objet.TemperatureActuel = Regex.Replace(CStr(DataJourTmp.Temperature), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    objet.HumiditeActuel = Regex.Replace(CStr(DataJourTmp.Humidity), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    objet.MinToday = Regex.Replace(CStr(DataJourTmp.min_temp), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    objet.MaxToday = Regex.Replace(CStr(DataJourTmp.max_temp), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "TEMPERATURE"
                    objet.Value = Regex.Replace(CStr(DataJourTmp.Temperature), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "HUMIDITE"
                    objet.Value = Regex.Replace(CStr(DataJourTmp.Humidity), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "PLUIETOTAL"
             '       objet.Value = Regex.Replace(CStr(moduleIDalire.dashboard_data.sum_rain_24), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "PLUIECOURANT"
                    objet.Value = Regex.Replace(CStr(DataJourTmp.Rain), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "BAROMETRE"
                    objet.Value = Regex.Replace(CStr(DataJourTmp.Pressure), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "VITESSEVENT"
                    objet.Value = Regex.Replace(CStr(DataJourTmp.WindStrength), "[.,]", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                Case "DIRECTIONVENT"
                    objet.Value = DirVent(DataJourTmp.WindAngle)
                Case "UV"
                    objet.Value = DataJourTmp.UV
                Case Else
                    WriteLog("ERR: GetData=> Pas de valeur enregistrée")
            End Select

            WriteLog("Enregistrement des données pour la station -> " & objet.adresse2)

            Return True
        Catch ex As Exception
            WriteLog("ERR: GetData, " & ex.Message)
            Return False
        End Try
    End Function
    Private Function DirVent(ByVal direction As Integer) As String
            Try
                If direction > 348.75 Or direction < 11.26 Then
                    Return "N"
                ElseIf direction < 33.76 Then
                    Return "NNE"
                ElseIf direction < 56.26 Then
                    Return "NE"
                ElseIf direction < 78.76 Then
                    Return "ENE"
                ElseIf direction < 101.26 Then
                    Return "E"
                ElseIf direction < 123.76 Then
                    Return "ESE"
                ElseIf direction < 146.26 Then
                    Return "SE"
                ElseIf direction < 168.76 Then
                    Return "SSE"
                ElseIf direction < 191.26 Then
                    Return "S"
                ElseIf direction < 213.76 Then
                    Return "SSW"
                ElseIf direction < 236.26 Then
                    Return "SW"
                ElseIf direction < 258.76 Then
                    Return "WSW"
                ElseIf direction < 281.26 Then
                    Return "W"
                ElseIf direction < 303.76 Then
                    Return "WNW"
                ElseIf direction < 326.26 Then
                    Return "NW"
                Else
                    Return "NNW"
                End If
            Catch ex As Exception
                WriteLog("ERR: wrdirection : " & ex.Message)
                Return "ERR: " & ex.Message
            End Try
        End Function
        Private Sub WriteLog(ByVal message As String)
            Try
                'utilise la fonction de base pour loguer un event
                If STRGS.InStr(message, "DBG:") > 0 Then
                    If _DEBUG Then
                        _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, Me.Nom, STRGS.Right(message, message.Length - 5))
                    End If
                ElseIf STRGS.InStr(message, "ERR:") > 0 Then
                    _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom, STRGS.Right(message, message.Length - 5))
                Else
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, Me.Nom, message)
                End If
            Catch ex As Exception
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " WriteLog", ex.Message)
            End Try
        End Sub
#End Region
    End Class
