﻿Imports System.Windows.Threading
Imports System.IO
Imports System.Threading

Public Class uWMeteo
    Dim _Id As String

    Public Property ID As String
        Get
            Return _Id
        End Get
        Set(ByVal value As String)
            _Id = value
            If _Id <> "" Then
                GetMeteo()
            End If

        End Set
    End Property

    Private Sub GetMeteo()
        Try

            If IsConnect = True Then
                If _Id <> "" Then
                    Dim _dev As HoMIDom.HoMIDom.TemplateDevice = myService.ReturnDeviceByID(IdSrv, _Id)
                    If IsNothing(_dev) Then Exit Sub
                    'LblVille.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblVille.Content = _dev.Name, ThreadStart))
                    'LblTemp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblTemp.Content = _dev.TemperatureActuel & "°", ThreadStart))
                    'LblMin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMin.Content = "Min: " & _dev.MinToday & "°", ThreadStart))
                    'LblMinJ1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMinJ1.Content = _dev.MinToday & "°", ThreadStart))
                    'LblMinJ2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMinJ2.Content = _dev.MinJ1 & "°", ThreadStart))
                    'LblMinJ3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMinJ3.Content = _dev.MinJ2 & "°", ThreadStart))
                    'LblMinJ4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMinJ4.Content = _dev.MinJ3 & "°", ThreadStart))
                    'LblMax.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMax.Content = "Max: " & _dev.MaxToday & "°", ThreadStart))
                    'LblMaxJ1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMaxJ1.Content = _dev.MaxToday & "°", ThreadStart))
                    'LblMaxJ2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMaxJ2.Content = _dev.MaxJ1 & "°", ThreadStart))
                    'LblMaxJ3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMaxJ3.Content = _dev.MaxJ2 & "°", ThreadStart))
                    'LblMaxJ4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblMaxJ4.Content = _dev.MaxJ3 & "°", ThreadStart))
                    'LblJ1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblJ1.Content = _dev.JourToday, ThreadStart))
                    'LblJ2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblJ2.Content = _dev.JourJ1, ThreadStart))
                    'LblJ3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblJ3.Content = _dev.JourJ2, ThreadStart))
                    'LblJ4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() LblJ4.Content = _dev.JourJ3, ThreadStart))

                    LblVille.Content = _dev.Name
                    LblTemp.Content = _dev.TemperatureActuel & "°"
                    LblMin.Content = "Min: " & _dev.MinToday & "°"
                    LblMinJ1.Content = _dev.MinToday & "°"
                    LblMinJ2.Content = _dev.MinJ1 & "°"
                    LblMinJ3.Content = _dev.MinJ2 & "°"
                    LblMinJ4.Content = _dev.MinJ3 & "°"
                    LblMax.Content = "Max: " & _dev.MaxToday & "°"
                    LblMaxJ1.Content = _dev.MaxToday & "°"
                    LblMaxJ2.Content = _dev.MaxJ1 & "°"
                    LblMaxJ3.Content = _dev.MaxJ2 & "°"
                    LblMaxJ4.Content = _dev.MaxJ3 & "°"
                    LblJ1.Content = _dev.JourToday
                    LblJ2.Content = _dev.JourJ1
                    LblJ3.Content = _dev.JourJ2
                    LblJ4.Content = _dev.JourJ3

                    Dim bmpImage As New BitmapImage()
                    bmpImage.BeginInit()
                    If File.Exists(_MonRepertoire & "\Images\Meteo\" & _dev.IconActuel & ".png") = True Then
                        bmpImage.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\" & _dev.IconActuel & ".png", UriKind.Absolute)
                    Else
                        bmpImage.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\na.png", UriKind.Absolute)
                    End If
                    bmpImage.EndInit()
                    'Icon.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() Icon.Source = bmpImage, ThreadStart))
                    Icon.Source = bmpImage

                    Dim bmpImage2 As New BitmapImage()
                    bmpImage2.BeginInit()
                    If File.Exists(_MonRepertoire & "\Images\Meteo\" & _dev.IconToday & ".png") = True Then
                        bmpImage2.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\" & _dev.IconToday & ".png", UriKind.Absolute)
                    Else
                        bmpImage2.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\na.png", UriKind.Absolute)
                    End If
                    bmpImage2.EndInit()
                    IconJ1.Source = bmpImage2
                    'IconJ1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() IconJ1.Source = bmpImage2, ThreadStart))

                    Dim bmpImage3 As New BitmapImage()
                    bmpImage3.BeginInit()
                    If File.Exists(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ1 & ".png") = True Then
                        bmpImage3.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ1 & ".png", UriKind.Absolute)
                    Else
                        bmpImage3.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\na.png", UriKind.Absolute)
                    End If
                    bmpImage3.EndInit()
                    IconJ2.Source = bmpImage3
                    'IconJ2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() IconJ2.Source = bmpImage3, ThreadStart))

                    Dim bmpImage4 As New BitmapImage()
                    bmpImage4.BeginInit()
                    If File.Exists(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ2 & ".png") = True Then
                        bmpImage4.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ2 & ".png", UriKind.Absolute)
                    Else
                        bmpImage4.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\na.png", UriKind.Absolute)
                    End If
                    bmpImage4.EndInit()
                    IconJ3.Source = bmpImage4
                    'IconJ3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() IconJ3.Source = bmpImage4, ThreadStart))

                    Dim bmpImage5 As New BitmapImage()
                    bmpImage5.BeginInit()
                    If File.Exists(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ3 & ".png") = True Then
                        bmpImage5.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\" & _dev.IconJ3 & ".png", UriKind.Absolute)
                    Else
                        bmpImage5.UriSource = New Uri(_MonRepertoire & "\Images\Meteo\na.png", UriKind.Absolute)
                    End If
                    bmpImage5.EndInit()
                    IconJ4.Source = bmpImage5
                    'IconJ4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, DirectCast(Sub() IconJ4.Source = bmpImage5, ThreadStart))

                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur GetMeteo: " & ex.Message)
        End Try

    End Sub


    Private Function TraduireJour(ByVal Jour As String) As String
        TraduireJour = "?"
        Select Case Jour
            Case "Thu"
                TraduireJour = "Jeu"
            Case "Fri"
                TraduireJour = "Ven"
            Case "Sat"
                TraduireJour = "Sam"
            Case "Sun"
                TraduireJour = "Dim"
            Case "Mon"
                TraduireJour = "Lun"
            Case "Tue"
                TraduireJour = "Mar"
            Case "Wed"
                TraduireJour = "Mer"
        End Select
    End Function

    Public Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        'Dim x As New Thread(AddressOf GetMeteo)
        'x.Start()
        GetMeteo()
    End Sub

    Private Function Traduire(ByVal txt)

        Traduire = txt

        If txt = "A Few Clouds" Then
            Traduire = "Quelques nuages"
        End If
        If txt = "A Few Clouds and Breezy" Then
            Traduire = "Quelques nuages et Frais"
        End If
        If txt = "A Few Clouds and Windy" Then
            Traduire = "Quelques nuages et  et grand vent"
        End If
        If txt = "A Few Clouds with Haze" Then
            Traduire = "Quelques nuages et brume"
        End If
        If txt = "AM Clouds / PM Sun" Then
            Traduire = "Matin nuages / Après-Midi Soleil"
        End If
        If txt = "AM Fog / PM Sun" Then
            Traduire = "Matin Brouillard / Après-Midi Soleil"
        End If
        If txt = "AM Rain / Snow Showers" Then
            Traduire = "Matin pluie / averses neigeuses"
        End If
        If txt = "AM Showers" Then
            Traduire = "Averses matinales"
        End If
        If txt = "AM Snow Showers" Then
            Traduire = "Averses de neige le matin"
        End If
        If txt = "AM T-Storms" Then
            Traduire = "Orages le matin"
        End If
        If txt = "Blowing Dust" Then
            Traduire = "Vent de poussière"
        End If
        If txt = "Blowing Sand" Then
            Traduire = "Vent de sable"
        End If
        If txt = "Blowing Snow" Then
            Traduire = "Vent de neige"
        End If
        If txt = "Blowing Snow in Vicinity" Then
            Traduire = "Vent de neige dans les environs"
        End If
        If txt = "Clear" Then
            Traduire = "Clair"
        End If
        If txt = "Clear / Wind" Then
            Traduire = "Clair / vent"
        End If
        If txt = "Clear and Breezy" Then
            Traduire = "Clair et frais"
        End If
        If txt = "Clear with Haze" Then
            Traduire = "Clair avec brume légère"
        End If
        If txt = "Cloudy Clair" Then
            Traduire = "Nuageux clair"
        End If
        If txt = "Clouds Early / Clearing Late" Then
            Traduire = "Nuages matinaux suivis d'éclaircies"
        End If
        If txt = "Cloudy" Then
            Traduire = "Nuageux"
        End If
        If txt = "Cloudy / Wind" Then
            Traduire = "Nuageux / Vent"
        End If
        If txt = "Cloudy / Windy" Then
            Traduire = ""
        End If
        If txt = "Drifting Snow" Then
            Traduire = "Amoncellement de neige"
        End If
        If txt = "Drizzle" Then
            Traduire = "Crachin"
        End If
        If txt = "Drizzle Fog/Mist" Then
            Traduire = "Crachin brouillard/brume"
        End If
        If txt = "Drizzle Fog" Then
            Traduire = "Crachin brouillard"
        End If
        If txt = "Drizzle Ice Pellets" Then
            Traduire = "Crachin grêlons"
        End If
        If txt = "Drizzle Snow" Then
            Traduire = "Crachin neige"
        End If
        If txt = "Dust" Then
            Traduire = "Poussière"
        End If
        If txt = "Dust Storm in Vicinity" Then
            Traduire = "Tempête de poussière dans les environs"
        End If
        If txt = "Dust Storm" Then
            Traduire = "Tempête de poussière"
        End If
        If txt = "Dust/Sand Whirls" Then
            Traduire = "Tourbillons de poussière/sable"
        End If
        If txt = "Dust/Sand Whirls in Vicinity" Then
            Traduire = "Tourbillons de poussière/sable dans les environs"
        End If
        If txt = "Fair" Then
            Traduire = "Ciel dégagé"
        End If
        If txt = "Fair/ Windy" Then
            Traduire = "Ciel dégagé / grand vent"
        End If
        If txt = "Fair and Breezy" Then
            Traduire = "Ciel dégagé et frais"
        End If
        If txt = "Fair and Windy" Then
            Traduire = "Ciel dégagé et grand vent"
        End If
        If txt = "Fair with Haze" Then
            Traduire = "Ciel dégagé avec brume légère"
        End If
        If txt = "Few Showers" Then
            Traduire = "Quelques averses"
        End If
        If txt = "Few Snow Showers" Then
            Traduire = "Quelques averses de neige"
        End If
        If txt = "Few Snow Showers / Wind" Then
            Traduire = "Quelques averses de neige / Vent"
        End If
        If txt = "Fog" Then
            Traduire = "Brouillard"
        End If
        If txt = "Fog in Vicinity" Then
            Traduire = "Brouillard dans les environs"
        End If
        If txt = "Fog/Mist" Then
            Traduire = "Brouillard/brume"
        End If
        If txt = "Freezing Drizzle" Then
            Traduire = "Crachin givrant"
        End If
        If txt = "Freezing Drizzle in Vicinity" Then
            Traduire = "Crachin givrant dans les environs"
        End If
        If txt = "Freezing Drizzle Rain" Then
            Traduire = "Crachin pluie givrant "
        End If
        If txt = "Freezing Drizzle Snow" Then
            Traduire = "Crachin neige givrant"
        End If
        If txt = "Freezing Fog" Then
            Traduire = "Brouillard givrant"
        End If
        If txt = "Freezing Fog in Vicinity" Then
            Traduire = "Brouillard givrant dans les environs"
        End If
        If txt = "Freezing Rain" Then
            Traduire = "Pluie givrante"
        End If
        If txt = "Freezing Rain in Vicinity" Then
            Traduire = "Pluie givrante dans les environs"
        End If
        If txt = "Freezing Rain Snow" Then
            Traduire = "Pluie neige givrante dans les environs"
        End If
        If txt = "Frigid" Then
            Traduire = "Grand froid"
        End If
        If txt = "Funnel Cloud in Vicinity" Then
            Traduire = "Nuage en entonnoir dans les environs"
        End If
        If txt = "Funnel Cloud" Then
            Traduire = "Nuage en entonnoir"
        End If
        If txt = "Hail" Then
            Traduire = "Grêle"
        End If
        If txt = "Hail Showers" Then
            Traduire = "Averses de grêle"
        End If
        If txt = "Haze" Then
            Traduire = "Brume légère"
        End If
        If txt = "Heavy Drizzle" Then
            Traduire = "Gros Crachin "
        End If
        If txt = "Heavy Drizzle Fog/Mist" Then
            Traduire = "Gros Crachin Brouillard/brume"
        End If
        If txt = "Heavy Drizzle Fog" Then
            Traduire = "Gros Crachin Brouillard"
        End If
        If txt = "Heavy Drizzle Ice Pellets" Then
            Traduire = "Gros Crachin Grêlons"
        End If
        If txt = "Heavy Drizzle Snow" Then
            Traduire = "Gros Crachin Neige"
        End If
        If txt = "Heavy Dust Storm" Then
            Traduire = "Grosse tempête de poussière"
        End If
        If txt = "Heavy Freezing Drizzle Rain" Then
            Traduire = "Gros Crachin verglassant Pluie"
        End If
        If txt = "Heavy Freezing Drizzle Snow" Then
            Traduire = "Gros Crachin verglassant Neige"
        End If
        If txt = "Heavy Freezing Drizzle" Then
            Traduire = "Gros Crachin verglassant"
        End If
        If txt = "Heavy Freezing Fog" Then
            Traduire = "Gros brouillard verglassant"
        End If
        If txt = "Heavy Freezing Rain Snow" Then
            Traduire = "Grosse pluie verglassante Neige"
        End If
        If txt = "Heavy Freezing Rain" Then
            Traduire = "Grosse pluie verglassante"
        End If
        If txt = "Heavy Ice Pellets" Then
            Traduire = "Gros Grêlons"
        End If
        If txt = "Heavy Ice Pellets Drizzle" Then
            Traduire = "Gros Grêlons Crachin"
        End If
        If txt = "Heavy Ice Pellets Rain" Then
            Traduire = "Gros Grêlons Pluie"
        End If
        If txt = "Heavy Rain" Then
            Traduire = "Grosse pluie"
        End If
        If txt = "Heavy Rain Fog/Mist" Then
            Traduire = "Grosse pluie Brouillard/brume"
        End If
        If txt = "Heavy Rain Fog" Then
            Traduire = "Grosse pluie Brouillard"
        End If
        If txt = "Heavy Rain Freezing Drizzle" Then
            Traduire = "Grosse pluie Crachin verglassant"
        End If
        If txt = "Heavy Rain Freezing Rain" Then
            Traduire = "Grosse pluie Pluie verglassante"
        End If
        If txt = "Heavy Rain Ice Pellets" Then
            Traduire = "Grosse pluie Grêlons"
        End If
        If txt = "Heavy Rain Icy" Then
            Traduire = "Grosse pluie verglassante"
        End If
        If txt = "Heavy Rain Showers Fog/Mist" Then
            Traduire = "Grosse pluie Averses Brouillard/brume"
        End If
        If txt = "Heavy Rain Showers" Then
            Traduire = "Grosse pluie Averses"
        End If
        If txt = "Heavy Rain Snow" Then
            Traduire = "Grosse pluie Neige"
        End If
        If txt = "Heavy Sand Storm" Then
            Traduire = "Grosse tempête de sable "
        End If
        If txt = "Heavy Showers Rain Fog/Mist" Then
            Traduire = "Grosses averses Pluie Brouillard/brume"
        End If
        If txt = "Heavy Showers Rain" Then
            Traduire = "Grosses averses Pluie"
        End If
        If txt = "Heavy Showers Snow Fog/Mist" Then
            Traduire = "Grosses Neige Pluie Brouillard/brume"
        End If
        If txt = "Heavy Showers Snow Fog" Then
            Traduire = "Grosses averses Neige Brouillard"
        End If
        If txt = "Heavy Showers Snow" Then
            Traduire = "Grosses averses Neige"
        End If
        If txt = "Heavy Snow" Then
            Traduire = "Beaucoup de neige"
        End If
        If txt = "Heavy Snow Drizzle" Then
            Traduire = "Beaucoup de neige Crachin"
        End If
        If txt = "Heavy Snow Fog/Mist" Then
            Traduire = "Beaucoup de neige Brouillard/brume"
        End If
        If txt = "Heavy Snow Fog" Then
            Traduire = "Beaucoup de neige Brouillard"
        End If
        If txt = "Heavy Snow Freezing Drizzle" Then
            Traduire = "Beaucoup de neige Crachin verglassant"
        End If
        If txt = "Heavy Snow Freezing Rain" Then
            Traduire = "Beaucoup de neige Pluie verglassante"
        End If
        If txt = "Heavy Snow Rain" Then
            Traduire = "Beaucoup de neige Pluie"
        End If
        If txt = "Heavy Snow Showers Fog/Mist" Then
            Traduire = "Beaucoup de neige Averses Brouillard/brume"
        End If
        If txt = "Heavy Snow Showers Fog" Then
            Traduire = "Beaucoup de neige Averses Brouillard"
        End If
        If txt = "Heavy Snow Showers" Then
            Traduire = "Beaucoup de neige Averses"
        End If
        If txt = "Heavy T-Storm" Then
            Traduire = "Gros orage"
        End If
        If txt = "Heavy T-Storms Rain Fog/Mist" Then
            Traduire = "Gros orage Brouillard/brume"
        End If
        If txt = "Heavy T-Storms Rain Fog" Then
            Traduire = "Gros orage Brouillard"
        End If
        If txt = "Heavy T-Storms Rain Hail Fog/Mist" Then
            Traduire = "Gros orage Grêle Brouillard/brume"
        End If
        If txt = "Heavy T-Storms Rain Hail Fog" Then
            Traduire = "Gros orage Grêle  Brouillard"
        End If
        If txt = "Heavy T-Storms Rain Hail Haze" Then
            Traduire = "Gros orage Grêle Brume légère"
        End If
        If txt = "Heavy T-Storms Rain Hail" Then
            Traduire = "Gros orage Grêle"
        End If
        If txt = "Heavy T-Storms Rain Haze" Then
            Traduire = "Gros orage Brume légère"
        End If
        If txt = "Heavy T-Storms Rain" Then
            Traduire = "Gros orage Pluie"
        End If
        If txt = "Heavy T-Storms Snow" Then
            Traduire = "Gros orage Neige"
        End If
        If txt = "Ice Crystals" Then
            Traduire = "Cristaux de glace"
        End If
        If txt = "Ice Pellets Drizzle" Then
            Traduire = "Grêlons Crachin"
        End If
        If txt = "Ice Pellets in Vicinity" Then
            Traduire = "Grêlons dans les environs "
        End If
        If txt = "Ice Pellets Rain" Then
            Traduire = "Grêlons Pluie"
        End If
        If txt = "Ice Pellets" Then
            Traduire = "Grêlons"
        End If
        If txt = "Isolated T-Storms" Then
            Traduire = "Orages isolés"
        End If
        If txt = "Light Drizzle" Then
            Traduire = "Léger crachin"
        End If
        If txt = "Light Drizzle Fog/Mist" Then
            Traduire = "Léger crachin Brouillard/brume"
        End If
        If txt = "Light Drizzle Fog" Then
            Traduire = "Léger crachin Brouillard"
        End If
        If txt = "Light Drizzle Ice Pellets" Then
            Traduire = "Léger crachin Grêlons"
        End If
        If txt = "Light Drizzle Snow" Then
            Traduire = "Léger crachin Neige"
        End If
        If txt = "Light Freezing Drizzle" Then
            Traduire = "Léger crachin verglassant"
        End If
        If txt = "Light Freezing Drizzle Rain" Then
            Traduire = "Léger crachin verglassant Pluie"
        End If
        If txt = "Light Freezing Drizzle Snow" Then
            Traduire = "Léger crachin verglassant Neige"
        End If
        If txt = "Light Freezing Fog" Then
            Traduire = "Léger brouillard verglassant"
        End If
        If txt = "Light Freezing Rain" Then
            Traduire = "Légère pluie verglassante"
        End If
        If txt = "Light Freezing Rain Snow" Then
            Traduire = "Légère pluie verglassante Neige"
        End If
        If txt = "Light Ice Pellets Drizzle" Then
            Traduire = "Petit Grêlons Crachin"
        End If
        If txt = "Light Ice Pellets Rain" Then
            Traduire = "Petit Grêlons Pluie"
        End If
        If txt = "Light Ice Pellets" Then
            Traduire = "Petit Grêlons"
        End If
        If txt = "Light Rain" Then
            Traduire = "Légère Pluie"
        End If
        If txt = "Light Rain Early" Then
            Traduire = "Légère Pluie matinal"
        End If
        If txt = "Light Rain Fog/Mist" Then
            Traduire = "Légère Pluie Brouillard/brume"
        End If
        If txt = "Light Rain Fog" Then
            Traduire = "Légère Pluie Brouillard"
        End If
        If txt = "Light Rain Freezing Drizzle" Then
            Traduire = "Légère Pluie Crachin verglassant"
        End If
        If txt = "Light Rain Freezing Rain" Then
            Traduire = "Légère Pluie Pluie verglassante"
        End If
        If txt = "Light Rain Ice Pellets" Then
            Traduire = "Légère Pluie Grêlons"
        End If
        If txt = "Light Rain Icy" Then
            Traduire = "Légère pluie verglassante"
        End If
        If txt = "Light Rain Late" Then
            Traduire = "Légère Pluie tardive"
        End If
        If txt = "Light Rain Shower" Then
            Traduire = "Légère pluie Averse"
        End If
        If txt = "Light Rain Shower and Windy" Then
            Traduire = "Légère pluie Averses et Vent"
        End If
        If txt = "Light Rain Showers" Then
            Traduire = "Légères pluie Averses"
        End If
        If txt = "Light Rain Snow" Then
            Traduire = "Légères pluie Neige"
        End If
        If txt = "Light Rain with Thunder" Then
            Traduire = "Légère Pluie avec tonnerre"
        End If
        If txt = "Light Showers Rain" Then
            Traduire = "Légère averses Pluie"
        End If
        If txt = "Light Showers Rain Fog/Mist" Then
            Traduire = "Légère averses Pluie Brouillard/brume"
        End If
        If txt = "Light Showers Snow" Then
            Traduire = "Légère averses Neige"
        End If
        If txt = "Light Showers Snow Fog" Then
            Traduire = "Légère averses Neige Brouillard"
        End If
        If txt = "Light Showers Snow Fog/Mist" Then
            Traduire = "Légère averses Neige Brouillard/brume"
        End If
        If txt = "Light Snow" Then
            Traduire = "Peu de neige"
        End If
        If txt = "Light Snow Drizzle" Then
            Traduire = "Peu de neige Crachin"
        End If
        If txt = "Light Snow Fall" Then
            Traduire = "Légère chutes de neige"
        End If
        If txt = "Light Snow Fog/Mist" Then
            Traduire = "Peu de neige Brouillard/brume"
        End If
        If txt = "Light Snow Fog" Then
            Traduire = "Peu de neige Brouillard"
        End If
        If txt = "Light Snow Freezing Drizzle" Then
            Traduire = "Peu de neige Crachin verglassant"
        End If
        If txt = "Light Snow Freezing Rain" Then
            Traduire = "Peu de neige Pluie Verglassante"
        End If
        If txt = "Light Snow Grains" Then
            Traduire = "Quelques flocons de neige"
        End If
        If txt = "Light Snow Rain" Then
            Traduire = "Peu de neige Pluie"
        End If
        If txt = "Light Snow Shower" Then
            Traduire = "Légère averse de neige"
        End If
        If txt = "Light Snow Showers Fog/Mist" Then
            Traduire = "Légère averses de neige Brouillard/brume"
        End If
        If txt = "Light Snow Showers Fog" Then
            Traduire = "Légère averses de neige Brouillard"
        End If
        If txt = "Light T-Storms Rain Fog/Mist" Then
            Traduire = "Léger orages Pluie Brouillard/brume"
        End If
        If txt = "Light T-Storms Rain Fog" Then
            Traduire = "Léger orages Pluie Brouillard"
        End If
        If txt = "Light T-Storms Rain Hail Fog/Mist" Then
            Traduire = "Léger orages Pluie Grêle Brouillard/brume"
        End If
        If txt = "Light T-Storms Rain Hail Fog" Then
            Traduire = "Léger orages Pluie Grêle Brouillard"
        End If
        If txt = "Light T-Storms Rain Hail Haze" Then
            Traduire = "Léger orages Pluie Grêle Brume légère"
        End If
        If txt = "Light T-Storms Rain Hail" Then
            Traduire = "Léger orages Pluie Grêle"
        End If
        If txt = "Light T-Storms Rain Haze" Then
            Traduire = "Léger orages Pluie Brume légère"
        End If
        If txt = "Light T-Storms Rain" Then
            Traduire = "Léger orages Pluie"
        End If
        If txt = "Light T-Storms Snow" Then
            Traduire = "Léger orages Neige"
        End If
        If txt = "Lightening" Then
            Traduire = "Eclairs"
        End If
        If txt = "Lightenings" Then
            Traduire = "Eclairs"
        End If
        If txt = "Mostly Clear" Then
            Traduire = "Ciel plutôt dégagé"
        End If
        If txt = "Mostly Cloudy" Then
            Traduire = "Plutôt nuageux"
        End If
        If txt = "Mostly Cloudy and Breezy" Then
            Traduire = "Plutôt nuageux et Venteux"
        End If
        If txt = "Mostly Cloudy and Windy" Or txt = "Mostly Cloudy/Wind" Then
            Traduire = "Plutôt nuageux et Grand vent"
        End If
        If txt = "Mostly Cloudy with Haze" Then
            Traduire = "Plutôt nuageux avec Légère Brume"
        End If
        If txt = "Mostly Sunny" Then
            Traduire = "Plutôt ensoleillé"
        End If
        If txt = "Mostly Sunny / Wind" Then
            Traduire = "Plutôt ensoleillé / vent"
        End If
        If txt = "Overcast" Then
            Traduire = "Couvert"
        End If
        If txt = "Overcast and Breezy" Then
            Traduire = "Couvert et Venteux"
        End If
        If txt = "Overcast and Windy" Then
            Traduire = "Couvert et Grand vent"
        End If
        If txt = "Overcast with Haze" Then
            Traduire = "Couvert avec légère brume"
        End If
        If txt = "Partial Fog" Then
            Traduire = "Banc de Brouillard"
        End If
        If txt = "Partial Fog in Vicinity" Then
            Traduire = "Banc de Brouillard dans les environs"
        End If
        If txt = "P Cloudy" Then
            Traduire = "Partiellement nuageux"
        End If
        If txt = "Partly Cloudy" Then
            Traduire = "Partiellement nuageux"
        End If
        If txt = "Partly Cloudy and Breezy" Then
            Traduire = "Partiellement nuageux et Venteux"
        End If
        If txt = "Partly Cloudy and Windy" Then
            Traduire = "Partiellement nuageux et Grand vent"
        End If
        If txt = "Partly Cloudy / Wind" Then
            Traduire = "Partiellement nuageux / Vent"
        End If
        If txt = "Partly Cloudy/ Windy" Or txt = "Partly Cloudy/Wind" Then
            Traduire = "Partiellement nuageux / Venteux"
        End If
        If txt = "Party Cloudy with Haze" Then
            Traduire = "Partiellement nuageux avec légère brume"
        End If
        If txt = "Partly Sunny" Then
            Traduire = "Partiellement ensoleillé"
        End If
        If txt = "Patches of Fog" Then
            Traduire = "Nappes de Brouillard"
        End If
        If txt = "Patches of Fog in Vicinity" Then
            Traduire = "Nappes de Brouillard dans les environs"
        End If
        If txt = "PM light rain" Then
            Traduire = "PM Légère Pluie"
        End If
        If txt = "PM Rain / Snow" Then
            Traduire = "PM Pluie / Neige"
        End If
        If txt = "PM Rain / Wind" Then
            Traduire = "PM Pluie / Vent"
        End If
        If txt = "PM Showers" Then
            Traduire = "PM Averses"
        End If
        If txt = "PM Snow Showers" Then
            Traduire = "PM averses neigeuses"
        End If
        If txt = "PM T-Storms" Then
            Traduire = "Orages l après-midi"
        End If
        If txt = "Rain" Then
            Traduire = "Pluie"
        End If
        If txt = "Rain / Snow" Then
            Traduire = "Pluie / neige"
        End If
        If txt = "Rain / Snow Showers" Then
            Traduire = "Pluie / averses neigeuses"
        End If
        If txt = "Rain / Snow Showers Early" Then
            Traduire = "Pluie / averses neigeuses matinales"
        End If
        If txt = "Rain / Thunder" Then
            Traduire = "Pluie / Tonnerre"
        End If
        If txt = "Rain / Wind" Then
            Traduire = "Pluie / Vent"
        End If
        If txt = "Rain and Snow" Then
            Traduire = "Pluie et Neige"
        End If
        If txt = "Rain Early" Then
            Traduire = "Pluie matinale"
        End If
        If txt = "Rain Fog/Mist" Then
            Traduire = "Pluie Brouillard/brume"
        End If
        If txt = "Rain Fog" Then
            Traduire = "Pluie Brouillard"
        End If
        If txt = "Rain Freezing Drizzle" Then
            Traduire = "Pluie Crachin verglassant"
        End If
        If txt = "Rain Freezing Rain" Then
            Traduire = "Pluie Pluie Verglassante"
        End If
        If txt = "Rain Ice Pellets" Then
            Traduire = "Pluie Grêlons"
        End If
        If txt = "Rain Shower" Then
            Traduire = "Pluie Averses"
        End If
        If txt = "Rain Showers Fog/Mist" Then
            Traduire = "Pluie Averses Brouillard/brume"
        End If
        If txt = "Rain Showers in Vicinity Fog/Mist" Then
            Traduire = "Pluie Averses dans les environs Brouillard/brume"
        End If
        If txt = "Rain Showers in Vicinity" Then
            Traduire = "Pluie Averses dans les environs"
        End If
        If txt = "Rain Snow" Then
            Traduire = "Pluie Neige"
        End If
        If txt = "Rain to Snow" Then
            Traduire = "Pluie vers Neige"
        End If
        If txt = "Sand Storm" Then
            Traduire = "Tempête de Sable"
        End If
        If txt = "Sand Storm in Vicinity" Then
            Traduire = "Tempête de Sable dans les environs"
        End If
        If txt = "Sand" Then
            Traduire = "Sable"
        End If
        If txt = "Shallow Fog" Then
            Traduire = "Brouillard superficiel"
        End If
        If txt = "Shallow Fog in Vicinity" Then
            Traduire = "Brouillard superficiel dans les environs"
        End If
        If txt = "Scattered Showers" Then
            Traduire = "Averses éparses"
        End If
        If txt = "Scattered Showers / Wind" Then
            Traduire = "Averses éparses / Vent"
        End If
        If txt = "Scattered Snow Showers" Then
            Traduire = "Averses neigeuses éparses"
        End If
        If txt = "Scattered Snow Showers / Wind" Then
            Traduire = "Averses neigeuses éparses / Vent"
        End If
        If txt = "Scattered Strong Storms" Then
            Traduire = "Violents orages locals"
        End If
        If txt = "Scattered T-Storms" Then
            Traduire = "Orages éparses"
        End If
        If txt = "Showers" Then
            Traduire = "Averses"
        End If
        If txt = "Showers / Wind" Then
            Traduire = "Averses / Vent"
        End If
        If txt = "Showers Hail" Then
            Traduire = "Averses Grêle"
        End If
        If txt = "Showers Ice Pellets" Then
            Traduire = "Averses Grêlons"
        End If
        If txt = "Showers in the Vicinity" Then
            Traduire = "Averses dans les environs"
        End If
        If txt = "Showers in Vicinity Fog/Mist" Then
            Traduire = "Averses dans les environs Brouillard/brume"
        End If
        If txt = "Showers in Vicinity Fog" Then
            Traduire = "Averses dans les environs Brouillard"
        End If
        If txt = "Showers in Vicinity Haze" Then
            Traduire = "Averses dans les environs Brume légère"
        End If
        If txt = "Showers in Vicinity Snow" Then
            Traduire = "Averses dans les environs Neige"
        End If
        If txt = "Showers Early" Then
            Traduire = "Averses matinales"
        End If
        If txt = "Showers Late" Then
            Traduire = "Averses tardives"
        End If
        If txt = "Showers Rain" Then
            Traduire = "Averses Pluie"
        End If
        If txt = "Showers Rain Fog/Mist" Then
            Traduire = "Averses Pluie Brouillard/brume"
        End If
        If txt = "Showers Rain in Vicinity" Then
            Traduire = "Averses Pluie dans les environs"
        End If
        If txt = "Showers Rain in Vicinity Fog/Mist" Then
            Traduire = "Averses Pluie dans les environs Brouillard/brume"
        End If
        If txt = "Showers Snow" Then
            Traduire = "Averses Neige"
        End If
        If txt = "Showers Snow Fog" Then
            Traduire = "Averses Neige Brouillard"
        End If
        If txt = "Showers Snow Fog/Mist" Then
            Traduire = "Averses Neige Brouillard/brume"
        End If
        If txt = "Smoke" Then
            Traduire = "fumée"
        End If
        If txt = "Snow" Then
            Traduire = "Neige"
        End If
        If txt = "Snow / Rain Icy Mix" Then
            Traduire = "Mélange Neige / Pluie Verglassante"
        End If
        If txt = "Snow and Fog" Then
            Traduire = "Neige et Brouillard"
        End If
        If txt = "Snow Drizzle" Then
            Traduire = "Neige Crachin"
        End If
        If txt = "Snow Fog/Mist" Then
            Traduire = "Neige Brouillard/brume"
        End If
        If txt = "Snow Freezing Drizzle" Then
            Traduire = "Neige Crachin verglassant"
        End If
        If txt = "Snow Freezing Rain" Then
            Traduire = "Neige Pluie verglassante"
        End If
        If txt = "Snow Rain" Then
            Traduire = "Neige Pluie"
        End If
        If txt = "Snow Shower" Then
            Traduire = "Averses de neige"
        End If
        If txt = "Snow Shower / Wind" Then
            Traduire = "Averses de neige / vent"
        End If
        If txt = "Snow Shower Early" Then
            Traduire = "Averses de neige matinales"
        End If
        If txt = "Snow Showers Fog/Mist" Then
            Traduire = "Averses de neige Brouillard/brume"
        End If
        If txt = "Snow Showers Fog" Then
            Traduire = "Averses de neige Brouillard"
        End If
        If txt = "Snow Showers in Vicinity" Then
            Traduire = "Averses de neige dans les environs"
        End If
        If txt = "Snow Showers in Vicinity Fog" Then
            Traduire = "Averses de neige dans les environs Brouillard"
        End If
        If txt = "Snow Showers in Vicinity Fog/Mist" Then
            Traduire = "Averses de neige dans les environs Brouillard/brume"
        End If
        If txt = "Snow Showers Late" Then
            Traduire = "Averses de neige tardives"
        End If
        If txt = "Snow to Rain" Then
            Traduire = "Neige vers Pluie"
        End If
        If txt = "Snowflakes" Then
            Traduire = "Flocons de neige"
        End If
        If txt = "Sunny" Then
            Traduire = "Ensoleillé"
        End If
        If txt = "Sunny / Wind" Then
            Traduire = "Ensoleillé / Vent"
        End If
        If txt = "Sunny Day" Then
            Traduire = "Journée ensoleillé"
        End If
        If txt = "Thunder" Then
            Traduire = "Tonnerre"
        End If
        If txt = "Thunder in the Vicinity" Then
            Traduire = "Tonnerre dans les environs"
        End If
        If txt = "T-Storms" Then
            Traduire = "Orages"
        End If
        If txt = "T-Storms Early" Then
            Traduire = "Orages matinaux"
        End If
        If txt = "T-Storms Fog" Then
            Traduire = "Orages Brouillard"
        End If
        If txt = "T-Storms Hail Fog" Then
            Traduire = "Orages Grêle Brouillard"
        End If
        If txt = "T-Storms Hail" Then
            Traduire = "Orages Grêle"
        End If
        If txt = "T-Storms Haze in Vicinity Hail" Then
            Traduire = "Orages brume dans les environs Grêle"
        End If
        If txt = "T-Storms Haze in Vicinity" Then
            Traduire = "Orages brume dans les environs"
        End If
        If txt = "T-Storms Heavy Rain" Then
            Traduire = "Orages Grosse Pluie"
        End If
        If txt = "T-Storms Heavy Rain Fog" Then
            Traduire = "Orages Grosse Pluie Brouillard"
        End If
        If txt = "T-Storms Heavy Rain Fog/Mist" Then
            Traduire = "Orages Grosse pluie Brouillard/brume"
        End If
        If txt = "T-Storms Heavy Rain Hail Fog" Then
            Traduire = "Orages Grosse Pluie Grêle Brouillard"
        End If
        If txt = "T-Storms Heavy Rain Hail Fog/Mist" Then
            Traduire = "Orages Grosse Pluie Grêle Brouillard/brume"
        End If
        If txt = "T-Storms Heavy Rain Hail Haze" Then
            Traduire = "Orages Grosse Pluie Grêle Brume légère"
        End If
        If txt = "T-Storms Heavy Rain Hail" Then
            Traduire = "Orages Grosse Pluie Grêle"
        End If
        If txt = "T-Storms Heavy Rain Haze" Then
            Traduire = "Orages Grosse Pluie Brume légère"
        End If
        If txt = "T-Storms Ice Pellets" Then
            Traduire = "Orages Grêlons"
        End If
        If txt = "T-Storms in Vicinity" Then
            Traduire = "Orages dans les environs"
        End If
        If txt = "T-Storms in Vicinity Fog" Then
            Traduire = "Orages dans les environs Brouillard"
        End If
        If txt = "T-Storms in Vicinity Fog/Mist" Then
            Traduire = "Orages dans les environs Brouillard/brume"
        End If
        If txt = "T-Storms in Vicinity Hail Fog/Mist" Then
            Traduire = "Orages dans les environs Grêle Brouillard/brume"
        End If
        If txt = "T-Storms in Vicinity Hail Haze" Then
            Traduire = "Orages dans les environs Grêle Brume légère"
        End If
        If txt = "T-Storms in Vicinity Hail" Then
            Traduire = "Orages dans les environs Grêle"
        End If
        If txt = "T-Storms in Vicinity Haze" Then
            Traduire = "Orages dans les environs Brume légère"
        End If
        If txt = "T-Storms Light Rain" Then
            Traduire = "Orages Légère pluie"
        End If
        If txt = "T-Storms Light Rain Fog" Then
            Traduire = "Orages Légère pluie Brouillard"
        End If
        If txt = "T-Storms Light Rain Fog/Mist" Then
            Traduire = "Orages Légère pluie Brouillard/brume"
        End If
        If txt = "T-Storms Light Rain Hail" Then
            Traduire = "Orages Légère pluie Grêle"
        End If
        If txt = "T-Storms Light Rain Hail Fog" Then
            Traduire = "Orages Légère pluie Grêle Brouillard"
        End If
        If txt = "T-Storms Light Rain Hail Fog/Mist" Then
            Traduire = "Orages Légère pluie Grêle Brouillard/brume"
        End If
        If txt = "T-Storms Light Rain Hail Haze" Then
            Traduire = "Orages Légère pluie Grêle Brume légère"
        End If
        If txt = "T-Storms Light Rain Haze" Then
            Traduire = "Orages Légère pluie Brume légère"
        End If
        If txt = "T-Storms Rain Fog/Mist" Then
            Traduire = "Orages Pluie Brouillard/brume"
        End If
        If txt = "T-Storms Rain Hail Fog/Mist" Then
            Traduire = "Orages Pluie Grêle Brouillard/brume"
        End If
        If txt = "T-Storms Showers in Vicinity" Then
            Traduire = "Orages Averses dans les environs"
        End If
        If txt = "T-Storms Showers in Vicinity Hail" Then
            Traduire = "Orages Averses dans les environs Grêle"
        End If
        If txt = "T-Storms Snow" Then
            Traduire = "Orages Neige"
        End If
        If txt = "Windy" Then
            Traduire = "Grand vent"
        End If
        If txt = "Windy / Snowy" Then
            Traduire = "Grand vent / neigeux"
        End If
        If txt = "Windy Rain" Then
            Traduire = "giboulées"
        End If
        If txt = "Wintry Mix" Then
            Traduire = "Mélanges pluie/neige"
        End If
        If txt = "Wintry Mix / Wind" Then
            Traduire = "Mélanges pluie/neige / vent"
        End If

    End Function

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Dim dt As DispatcherTimer = New DispatcherTimer()
        AddHandler dt.Tick, AddressOf dispatcherTimer_Tick
        dt.Interval = New TimeSpan(0, 5, 0)
        dt.Start()
    End Sub
End Class