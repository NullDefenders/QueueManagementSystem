@echo off
SET SCRIPT_DIR=%~dp0
cd /d %SCRIPT_DIR%

echo ==============================
echo Building QueueInformer
cd QueueInformer\QueueInformerFront
call npm ci
call npm run build

echo ==============================
echo Building GetTicket
cd ..\..\GetTicket\get-ticket-front
call npm ci
call npm run build

echo ==============================
echo Building WindowService
cd ..\..\WindowService\window-service-front
call npm ci
call npm run build

echo ==============================
echo Building PreRegistrationService
cd ..\..\PreRegistrationService\Frontend\PreRegistrationService
call npm ci
call npm run build

echo ==============================
echo All builds done!
pause
