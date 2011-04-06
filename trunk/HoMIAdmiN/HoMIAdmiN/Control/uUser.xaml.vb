﻿Imports HoMIDom.HoMIDom
Imports System.IO

Partial Public Class uUser
    '--- Variables ------------------
    Public Event CloseMe(ByVal MyObject As Object)
    Dim _Action As EAction 'Définit si modif ou création d'un device
    Dim _UserId As String 'Id de la zone à modifier

    Public Enum EAction
        Nouveau
        Modifier
    End Enum

    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnOK.Click
        If TxtPassword.Password <> TxtConfirm.Password Then
            MessageBox.Show("Le mot de passe est différent après confirmation!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        End If

        If TxtUsername.Text = "" Or TxtPassword.Password = "" Or ComboProfil.SelectedIndex < 0 Or TxtNom.Text = "" Or TxtPrenom.Text = "" Then
            MessageBox.Show("Le username, le mot de passe, le profil, le nom et prénom sont obligatoires!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        End If

        Window1.myService.SaveUser(_UserId, TxtUsername.Text, TxtPassword.Password, ComboProfil.SelectedIndex, TxtNom.Text, TxtPrenom.Text, TxtIden.Text, ImgIcon.Tag, TxteMail.Text, TxteMailAutre.Text, TxtTelFixe.Text, TxtTelMobile.Text, TxtTelAutre.Text, TxtAdresse.Text, TxtVille.Text, TxtCodePostal.Text)
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnClose.Click
        RaiseEvent CloseMe(Me)
    End Sub

    Public Sub New(ByVal Action As EAction, ByVal UserId As String)

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ImgIcon.Tag = " "

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        If Action = EAction.Nouveau Then 'Nouveau user

        Else 'Modifier zone
            Dim x As Users.User = Window1.myService.ReturnUserById(UserId)
            _UserId = UserId
            If x IsNot Nothing Then
                TxtUsername.Text = x.UserName
                TxtNom.Text = x.Nom
                TxtPrenom.Text = x.Prenom
                TxtPassword.Password = x.Password
                TxtConfirm.Password = x.Password
                ComboProfil.SelectedIndex = x.Profil
                TxtIden.Text = x.NumberIdentification
                TxteMail.Text = x.eMail
                TxteMailAutre.Text = x.eMailAutre
                TxtTelFixe.Text = x.TelFixe
                TxtTelMobile.Text = x.TelMobile
                TxtTelAutre.Text = x.TelAutre
                TxtAdresse.Text = x.Adresse
                TxtVille.Text = x.Ville
                TxtCodePostal.Text = x.CodePostal

                If File.Exists(x.Image) = True And x.Image <> "" And x.Image <> " " Then
                    Dim bmpImage As New BitmapImage()
                    bmpImage.BeginInit()
                    bmpImage.UriSource = New Uri(x.Image, UriKind.Absolute)
                    bmpImage.EndInit()
                    ImgIcon.Source = bmpImage
                    ImgIcon.Tag = x.Image
                End If
            End If
        End If

    End Sub

       Private Sub ImgIcon_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImgIcon.MouseLeftButtonDown
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Filter = "jpeg (*.jpg) |*.jpg;*.jpeg|(*.png) |*.png|(*.*) |*.*"

        If dlg.ShowDialog() = True Then
            Dim bmpImage As New BitmapImage()
            bmpImage.BeginInit()
            bmpImage.UriSource = New Uri(dlg.FileName, UriKind.Absolute)
            bmpImage.EndInit()
            ImgIcon.Source = bmpImage
            ImgIcon.Tag = dlg.FileName
        End If
    End Sub

End Class
