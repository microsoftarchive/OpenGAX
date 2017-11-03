@echo ussage: inst debug/release ver exp
@echo exp: exp or ""
@echo %1
@echo %2
@echo %3

:inst_gax_14exp
D:\GaxGat-VS2015\vsix-install\bin\Release\Root-VSIX.exe %2 %3 "D:\GaxGat-VS2015\OpenGax\GAX\Src\VisualStudio\Package\bin\%1\gax.vsix"

:inst_gat_14exp
D:\GaxGat-VS2015\vsix-install\bin\Release\Root-VSIX.exe %2 %3 "D:\GaxGat-VS2015\opengax\GAT\Src\bin\%1\gat.vsix"

:eof