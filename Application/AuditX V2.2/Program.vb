Module Program
    <STAThread()>
    Sub Main()
        ' Ensure DPI awareness as early as possible
        Try
            ITGC_AUDIT.EnsureDpiAware()
        Catch ex As Exception
            'Logger.LogMessage("EnsureDpiAware failed in Main: " & ex.Message, False)
        End Try

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Start with LoginForm (or Home if you start there)
        Application.Run(New LoginForm())
    End Sub
End Module