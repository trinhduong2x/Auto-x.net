for /r "C:\Users\admin\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup" %%a in (*.exe) do start "" "%%~fa"