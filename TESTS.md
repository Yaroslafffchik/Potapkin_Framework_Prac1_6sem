# Примеры тестирования API

## Тест 1: Получить все треки
```bash
curl http://localhost:8080/api/tracks
```

**Ожидаемый результат:** Статус 200, список из 3 треков

**Проверка логов:** Должны появиться записи с RequestId, временем выполнения

**Ответ**:
```
[{"id":1,"title":"Bohemian Rhapsody","artist":"Queen","album":"A Night at the Opera","duration":354},{"id":2,"title":"Stairway to Heaven","artist":"Led Zeppelin","album":"Led Zeppelin IV","duration":482},{"id":3,"title":"Hotel California","artist":"Eagles","album":"Hotel California","duration":391}]
```

## Тест 2: Получить трек по ID
```bash
curl http://localhost:8080/api/tracks/1
```

**Ожидаемый результат:** Статус 200, трек "Bohemian Rhapsody"

**Ответ:***


## Тест 3: Фильтрация по исполнителю
```bash
curl "http://localhost:8080/api/tracks?artist=Queen"
```

**Ожидаемый результат:** Статус 200, только треки Queen

**Ответ:**
```
[{"id":1,"title":"Bohemian Rhapsody","artist":"Queen","album":"A Night at the Opera","duration":354}]% 
```

## Тест 4: Создать новый трек (успешно)
```bash
curl -X POST http://localhost:8080/api/tracks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Imagine",
    "artist": "John Lennon",
    "album": "Imagine",
    "duration": 183
  }'
```

**Ожидаемый результат:** Статус 201, созданный трек с ID=4

**Ответ:**
```
{"id":4,"title":"Imagine","artist":"John Lennon","album":"Imagine","duration":183}%    
```

## Тест 5: Валидация - пустое название
```bash
curl -X POST http://localhost:8080/api/tracks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "",
    "artist": "Test",
    "album": "Test",
    "duration": 100
  }'
```

**Ожидаемый результат:** Статус 400, errorCode=VALIDATION_ERROR, сообщение "Название трека не может быть пустым"

**Ответ:**
```
{"errorCode":"VALIDATION_ERROR","message":"\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435 \u0442\u0440\u0435\u043A\u0430 \u043D\u0435 \u043C\u043E\u0436\u0435\u0442 \u0431\u044B\u0442\u044C \u043F\u0443\u0441\u0442\u044B\u043C","requestId":"af0cd380-4067-4ca5-bac3-7bee762de73f"}%  
```
Комментарий: проблема кодировки, некорректное отображение русского языка

## Тест 6: Валидация - отрицательная длительность
```bash
curl -X POST http://localhost:8080/api/tracks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test",
    "artist": "Test",
    "album": "Test",
    "duration": -10
  }'
```

**Ожидаемый результат:** Статус 400, errorCode=VALIDATION_ERROR, сообщение "Длительность трека должна быть больше 0"

**Ответ:**
```
{"errorCode":"VALIDATION_ERROR","message":"\u0414\u043B\u0438\u0442\u0435\u043B\u044C\u043D\u043E\u0441\u0442\u044C \u0442\u0440\u0435\u043A\u0430 \u0434\u043E\u043B\u0436\u043D\u0430 \u0431\u044B\u0442\u044C \u0431\u043E\u043B\u044C\u0448\u0435 0","requestId":"0d3f8e61-9466-4623-8e29-4384a921f7f4"}%                   
```
Комментарий: проблема кодировки, некорректное отображение русского языка

## Тест 7: Валидация - пустой исполнитель
```bash
curl -X POST http://localhost:8080/api/tracks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Song",
    "artist": "",
    "album": "Test Album",
    "duration": 100
  }'
```

**Ожидаемый результат:** Статус 400, errorCode=VALIDATION_ERROR, сообщение "Исполнитель не может быть пустым"

**Ответ:**
```
{"errorCode":"VALIDATION_ERROR","message":"\u0418\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C \u043D\u0435 \u043C\u043E\u0436\u0435\u0442 \u0431\u044B\u0442\u044C \u043F\u0443\u0441\u0442\u044B\u043C","requestId":"7a17ab1c-d6f7-40ce-b94e-da3ce0b7ba20"}%     
```
Комментарий: проблема кодировки, некорректное отображение русского языка

## Тест 8: Трек не найден
```bash
curl http://localhost:8080/api/tracks/999
```

**Ожидаемый результат:** Статус 404, errorCode=NOT_FOUND, сообщение "Трек с ID 999 не найден"

**Ответ:**
```
{"errorCode":"NOT_FOUND","message":"\u0422\u0440\u0435\u043A \u0441 ID 999 \u043D\u0435 \u043D\u0430\u0439\u0434\u0435\u043D","requestId":"02c991be-14ad-4654-84d6-e6db89a4f414"}%    
```
Комментарий: проблема кодировки, некорректное отображение русского языка

## Тест 9: Проверка фильтрации (регистронезависимость)
```bash
curl "http://localhost:8080/api/tracks?artist=queen"
```

**Ожидаемый результат:** Статус 200, трек Queen (несмотря на строчные буквы)

**Ответ:**
```
[{"id":1,"title":"Bohemian Rhapsody","artist":"Queen","album":"A Night at the Opera","duration":354}]%
```

## Тест 10: Проверка наличия RequestId в ошибках
```bash
curl -v http://localhost:8080/api/tracks/999 2>&1 | grep requestId
```

**Ожидаемый результат:** В теле ответа должно быть поле requestId с UUID

**Ответ:**
```
{"errorCode":"NOT_FOUND","message":"\u0422\u0440\u0435\u043A \u0441 ID 999 \u043D\u0435 \u043D\u0430\u0439\u0434\u0435\u043D","requestId":"130dc50a-1acc-40e6-9430-842a86bb5a87"}
```
Комментарий: проблема кодировки, некорректное отображение русского языка


