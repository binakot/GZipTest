# :page_facing_up: Техническое задание

Написать программу на `C#`, предназначенную для сжатия и расжатия файлов с помощью `System.IO.Compression.GzipStream`.

Параметры программы, имена исходного и результирующего файлов задаются в командной строке следующим образом:

* для архивации: `GZipTest.exe compress [имя исходного файла] [имя архива]`

* для разархивации: `GZipTest.exe decompress  [имя архива] [имя распакованного файла]`

В случае успеха программа возвращает `0`, при ошибке  `1`.

Программа должна эффективно распараллеливать и синхронизировать задачи в многопроцессорной среде и уметь обрабатывать файлы, размер которых превышает объем доступной оперативной памяти.

Код должен корректно обрабатывать все исключения, а при работе с потоками допускается использовать только стандартные классы и библиотеки из `.Net 3.5` (**исключая ThreadPool, BackgroundWorker, TPL**). Ожидается реализация с использованием **Thread-ов**.

Код программы должен следовать принципам ООП и ООД (читаемость, разбиение на классы и тд).

Алгоритм работы программы необходимо описать словами.

Исходники необходимо прислать вместе с проектом Visual Studio.

Дополнительным плюсом будет возможность корректной остановки программы по `Ctrl-C`.

# :bookmark_tabs: Критерии оценивания

Программа будет оцениваться по следующим критериям:

1. Работоспособность – проверяется на тестовых файлах с размерами `от 0 до 32 Gb`

2. Правильность выбора алгоритма с точки зрения эффективности – должен быть максимально загружен самый слабый компонент системы (диск/процессор)

3. Знание и умение использовать примитивы синхронизации – должны быть правильно выбраны примитивы для синхронизации потоков, доступа к данным

4. Проработка архитектуры – есть разбиение на классы по принципам ООП и ООД, не должно быть лишних классов, интерфейсов, методов и т.д.

5. Читабельность и понятность кода – код должен быть простым, аккуратным; алгоритм программы должен быть понятен без отладки

6. Грамотная обработка ошибок и нестандартных ситуаций – должна выводиться диагностическая информация, по которой должно быть понятно что произошло без отладки программы.

7. Правильное управления ресурсами – не должно быть утечек неуправляемых ресурсов, а также своевременное уничтожение управляемых ресурсов