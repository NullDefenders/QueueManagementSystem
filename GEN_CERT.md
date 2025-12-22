# Установка и настройка сертификата с помощью mkcert

## 1. Скачивание mkcert
Скачайте последнюю версию mkcert с официального репозитория:
[https://github.com/FiloSottile/mkcert/releases]

После скачивания переименуйте файл в **mkcert.exe**

## 2. Установка локального CA
Запустите команду для установки локального центра сертификации (CA):

    mkcert -install

## 3. Создание сертификатов для доменов

    mkcert -cert-file _wildcard.otus.localhost.pem -key-file _wildcard.otus.localhost-key.pem "*.otus.localhost" otus.localhost

После выполнения команды появятся два файла:

    _wildcard.otus.localhost.pem — сертификат
    _wildcard.otus.localhost-key.pem — закрытый ключ

## 4. Размещение сертификатов в Nginx

Поместите созданные файлы в папку Nginx:

    Nginx\certs\