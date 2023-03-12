# Тестовое задание на должность Стажер backend C# SENSE CAPITAL

Автор: Антонов Михаил [Резюме](https://spb.hh.ru/resume/904ea23cff09dfa64c0039ed1f454548316d4c)

## Описание

Для разработки сайта и мобильного приложения для игры в крестики нолики 3x3 для двух игроков требуется реализовать web api. Игра проходит по обычным правилам.

Платформа dotNet

Любая БД, допустимо просто использование файлов

## Запуск

### Запуск в режиме разработки

Для запуска прилоения в режиме разработки выполните

```
docker compose up -d
```

Приложение запустится на *localhost:5001*

## Тесты

Для запуска юнит тестов выполните

```
dotnet test --filter TestCategory=Unit
```

Для запуска интеграционных тестов необходимо сначала поднять тербуемые сервисы:

```
docker compose up -d redis

dotnet test --filter TestCategory=Functional
```

## Как пришел к результату

1. Выделил будущую архитектуру проекта (создал решения).
2. Написал доменный слой игры - классы **Player**, **Board**, **Game**.
3. Написал тесты для домена.
4. Написал слой инфраструктуры.
5. Написал тесты для инфраструктуры.
6. Написал слой приложения.
7. Написал тесты для слоя приложения.