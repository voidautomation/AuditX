Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                ITGC_AUDIT.EnsureDpiAware()
            Catch ex As Exception
                Logger.LogMessage("EnsureDpiAware failed in Startup: " & ex.Message, False)
            End Try
        End Sub
    End Class
End Namespace