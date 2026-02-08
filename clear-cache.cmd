@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

:menu
cls
echo.
echo ╔════════════════════════════════════════════╗
echo ║   Очистка NuGet кэша - Выберите пакет     ║
echo ╚════════════════════════════════════════════╝
echo.
echo   МОИ ПАКЕТЫ (PetFamily):
echo   [1] PetFamily.SharedKernel.Domain
echo   [2] PetFamily.SharedKernel.Application
echo   [3] PetFamily.SharedKernel.WebApi
echo   [4] PetFamily.SharedKernel.Infrastructure
echo   [5] PetFamily.SharedKernel.Contracts
echo.
echo   ПОПУЛЯРНЫЕ ПАКЕТЫ:
echo   [6] MassTransit (все версии)
echo   [7] Microsoft.EntityFrameworkCore (все версии)
echo   [8] OpenTelemetry (все версии)
echo   [9] Swashbuckle.AspNetCore
echo  [10] Serilog (все версии)
echo.
echo   МАССОВЫЕ ОПЕРАЦИИ:
echo  [11] Очистить кэш ВСЕХ PetFamily пакетов
echo  [12] Очистить ВСЕ локальные источники
echo  [13] Очистить All ^+ Restore всех решений
echo.
echo   [0] Выход
echo.

set /p choice="Выберите номер [0-13]: "

if "%choice%"=="0" goto exit
if "%choice%"=="1" goto clear_domain
if "%choice%"=="2" goto clear_application
if "%choice%"=="3" goto clear_webapi
if "%choice%"=="4" goto clear_infrastructure
if "%choice%"=="5" goto clear_contracts
if "%choice%"=="6" goto clear_masstransit
if "%choice%"=="7" goto clear_efcore
if "%choice%"=="8" goto clear_opentelemetry
if "%choice%"=="9" goto clear_swashbuckle
if "%choice%"=="10" goto clear_serilog
if "%choice%"=="11" goto clear_all_petfamily
if "%choice%"=="12" goto clear_all_sources
if "%choice%"=="13" goto clear_and_restore
goto invalid_choice

:clear_domain
call :clear_package "petfamily.sharedkernel.domain" "PetFamily.SharedKernel.Domain"
goto menu

:clear_application
call :clear_package "petfamily.sharedkernel.application" "PetFamily.SharedKernel.Application"
goto menu

:clear_webapi
call :clear_package "petfamily.sharedkernel.webapi" "PetFamily.SharedKernel.WebApi"
goto menu

:clear_infrastructure
call :clear_package "petfamily.sharedkernel.infrastructure" "PetFamily.SharedKernel.Infrastructure"
goto menu

:clear_contracts
call :clear_package "petfamily.sharedkernel.contracts" "PetFamily.SharedKernel.Contracts"
goto menu

:clear_masstransit
call :clear_package "masstransit*" "MassTransit"
goto menu

:clear_efcore
call :clear_package "microsoft.entityframeworkcore*" "Microsoft.EntityFrameworkCore"
goto menu

:clear_opentelemetry
call :clear_package "opentelemetry*" "OpenTelemetry"
goto menu

:clear_swashbuckle
call :clear_package "swashbuckle.aspnetcore*" "Swashbuckle.AspNetCore"
goto menu

:clear_serilog
call :clear_package "serilog*" "Serilog"
goto menu

:clear_all_petfamily
cls
echo.
echo 🗑️  Очистка кэша ВСЕХ PetFamily пакетов...
echo.
call :clear_package "petfamily.*" "PetFamily.*"
goto menu

:clear_all_sources
cls
echo.
echo 🧹 Полная очистка всех NuGet источников...
echo.
dotnet nuget locals all --clear
echo.
echo ✓ Все источники очищены!
echo.
pause
goto menu

:clear_and_restore
cls
echo.
echo 🚀 Полная очистка и восстановление...
echo.
echo [1/2] Очистка всех источников...
dotnet nuget locals all --clear
echo.
echo [2/2] Восстановление всех решений...
cd /d "%~dp0"
dotnet restore Solution\VolunteerManagement\VolunteerManagement.slnx --force --no-cache --verbosity minimal
if errorlevel 1 (
    echo.
    echo ❌ Ошибка при восстановлении!
    echo.
) else (
    echo.
    echo ✅ Успешно восстановлено!
    echo.
)
pause
goto menu

:clear_package
setlocal
set "package_name=%~1"
set "display_name=%~2"

echo.
echo 🗑️  Очистка кэша: %display_name%
echo.

set "cache_dir=%USERPROFILE%\.nuget\packages\%package_name%"

if "%package_name:*=%"=="*" (
    echo ⏳ Поиск папок с маской "%package_name%"...
    for /d %%D in ("%USERPROFILE%\.nuget\packages\%package_name%") do (
        if exist "%%D" (
            echo   Удаление: %%D
            rmdir /s /q "%%D" 2>nul
        )
    )
) else (
    if exist "%cache_dir%" (
        echo   Удаление: %cache_dir%
        rmdir /s /q "%cache_dir%" 2>nul
    ) else (
        echo   ℹ️  Кэш не найден
    )
)

echo.
echo ✅ Кэш очищен для: %display_name%
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
