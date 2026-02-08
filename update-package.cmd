@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

:menu
cls
echo.
echo ╔════════════════════════════════════════════╗
echo ║   Обновление пакета + Restore             ║
echo ╚════════════════════════════════════════════╝
echo.
echo   МОИ ПАКЕТЫ (PetFamily):
echo   [1] PetFamily.SharedKernel.Domain
echo   [2] PetFamily.SharedKernel.Application
echo   [3] PetFamily.SharedKernel.WebApi
echo   [4] PetFamily.SharedKernel.Infrastructure
echo   [5] PetFamily.SharedKernel.Contracts
echo.
echo   [0] Выход
echo.

set /p choice="Выберите номер [0-5]: "

if "%choice%"=="0" goto exit
if "%choice%"=="1" goto update_domain
if "%choice%"=="2" goto update_application
if "%choice%"=="3" goto update_webapi
if "%choice%"=="4" goto update_infrastructure
if "%choice%"=="5" goto update_contracts
goto invalid_choice

:update_domain
call :update_package "petfamily.sharedkernel.domain" "PetFamily.SharedKernel.Domain"
goto menu

:update_application
call :update_package "petfamily.sharedkernel.application" "PetFamily.SharedKernel.Application"
goto menu

:update_webapi
call :update_package "petfamily.sharedkernel.webapi" "PetFamily.SharedKernel.WebApi"
goto menu

:update_infrastructure
call :update_package "petfamily.sharedkernel.infrastructure" "PetFamily.SharedKernel.Infrastructure"
goto menu

:update_contracts
call :update_package "petfamily.sharedkernel.contracts" "PetFamily.SharedKernel.Contracts"
goto menu

:update_package
setlocal
set "package_name=%~1"
set "display_name=%~2"

cls
echo.
echo ╔════════════════════════════════════════════╗
echo ║   Обновление: %display_name%
echo ╚════════════════════════════════════════════╝
echo.

set "cache_dir=%USERPROFILE%\.nuget\packages\%package_name%"

echo [1/3] Очистка кэша "%display_name%"...
if exist "%cache_dir%" (
    rmdir /s /q "%cache_dir%" 2>nul
    echo ✓ Кэш удалён
) else (
    echo ℹ️  Кэш не найден
)

echo.
echo [2/3] Очистка HTTP кэша...
dotnet nuget locals http-cache --clear >nul 2>&1
echo ✓ HTTP кэш очищен

echo.
echo [3/3] Восстановление пакетов...
cd /d "%~dp0"
dotnet restore Solution\VolunteerManagement\VolunteerManagement.slnx --force --no-cache --verbosity minimal

if errorlevel 1 (
    echo.
    echo ❌ Ошибка при восстановлении!
    echo.
) else (
    echo.
    echo ✅ Пакет "%display_name%" успешно обновлён!

    echo.
    echo Версия в Directory.Packages.props:
    for /f "usebackq tokens=* delims=" %%i in (`findstr /C:"%display_name%" Directory.Packages.props`) do (
        echo   %%i
    )
)

echo.
echo Нажмите Enter для продолжения...
pause >nul
endlocal
goto :eof

:invalid_choice
echo.
echo ❌ Неверный выбор! Попробуйте снова.
echo.
pause
goto menu

:exit
echo.
echo 👋 До свидания!
echo.
exit /b 0
