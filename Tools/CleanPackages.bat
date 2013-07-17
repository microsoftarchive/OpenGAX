Rem
Rem  This batch will remove any unkown project factory and item templates from the VisualStudio registry
Rem  Typically those factories are registered by creating new Guidance Packages
Rem 
Rem 

@echo off
set _ROOT=%~d0%~p0
Rem Remove Project factories
for /f %%i in ('reg query HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\NewProjectTemplates\TemplateDirs\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}') do call :DeleteRegKey %%i
Rem Remove item templates
for /f %%i in ('reg query HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\Projects\{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\AddItemTemplates\TemplateDirs') do call :DeleteRegKey %%i
regedit /s %_ROOT%\itemtemplates.reg
goto :eof

:DeleteRegKey
if [%1]==[(Default)] goto :eof
if [%1]==[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\NewProjectTemplates\TemplateDirs\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\/1] goto :eof
if [%1]==[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\NewProjectTemplates\TemplateDirs\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}] goto :eof
if [%1]==[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\Projects\{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\AddItemTemplates\TemplateDirs\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}] goto :eof
reg delete %1 /f
goto :eof
