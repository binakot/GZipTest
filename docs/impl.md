# :page_with_curl: Описание реализации

## Анализ задачи

> Написать программу на C#, предназначенную для сжатия и расжатия файлов с помощью System.IO.Compression.GzipStream.

Средства разработки: C#, .NET 3.5, VS 2017 Community edition with ReSharper Ultimate.
 
> Параметры программы, имена исходного и результирующего файлов задаются в командной строке следующим образом:
> * для архивации: GZipTest.exe compress [имя исходного файла] [имя архива]
> * для разархивации: GZipTest.exe decompress [имя архива] [имя распакованного файла]

Пишем консольное приложение.

> В случае успеха программа возвращает 0, при ошибке 1.

Используем метод [Environment.Exit(int exitCode)](https://msdn.microsoft.com/en-us/library/system.environment.exit(v=vs.90).aspx)

> Программа должна эффективно распараллеливать и синхронизировать задачи в многопроцессорной среде и уметь обрабатывать файлы, 
размер которых превышает объем доступной оперативной памяти.

Многопоточное приложение. Работа с одним ресурсом (файлом) из разных потоков. Оптимальное использование памяти.

> Код должен корректно обрабатывать все исключения, 
а при работе с потоками допускается использовать только стандартные классы и библиотеки из .Net 3.5 
(исключая ThreadPool, BackgroundWorker, TPL). Ожидается реализация с использованием Thread-ов.

В главном потоке обрабатываем управляемые исключения с помощью [try-catch-finally](https://msdn.microsoft.com/en-us/library/dszsf989.aspx).

Исключения в рабочих потоках ловим с помощью события [AppDomain.UnhandledException](https://msdn.microsoft.com/en-us/library/system.appdomain.unhandledexception(v=vs.90).aspx).

Отказываемся от [ThreadPool](https://msdn.microsoft.com/en-us/library/system.threading.threadpool(v=vs.90).aspx), 
[BackgroundWorker](https://msdn.microsoft.com/en-us/library/system.componentmodel.backgroundworker(v=vs.90).aspx), 
[TPL](https://msdn.microsoft.com/en-us/library/dd460717(v=vs.110).aspx).

Остаются только [Thread](https://msdn.microsoft.com/en-us/library/system.threading.thread(v=vs.90).aspx)
и примитивы синхронизации [volatile](https://msdn.microsoft.com/en-us/library/x13ttww7(v=vs.90).aspx), 
[lock](https://msdn.microsoft.com/en-us/library/c5kehkcz(v=vs.90).aspx),
[Interlocked](https://msdn.microsoft.com/en-us/library/system.threading.interlocked(v=vs.90).aspx),
[EventWaitHandle](https://msdn.microsoft.com/en-us/library/system.threading.eventwaithandle(v=vs.90).aspx),
[ManualResetEvent](https://msdn.microsoft.com/en-us/library/system.threading.manualresetevent(v=vs.90).aspx),
[AutoResetEvent](https://msdn.microsoft.com/en-us/library/system.threading.autoresetevent(v=vs.90).aspx),
[ReaderWriterLock](https://msdn.microsoft.com/en-us/library/system.threading.readerwriterlock(v=vs.90).aspx),
[ReaderWriterLockSlim](https://msdn.microsoft.com/en-us/library/system.threading.readerwriterlockslim(v=vs.90).aspx),
[Monitor](https://msdn.microsoft.com/en-us/library/system.threading.monitor(v=vs.90).aspx),
[Mutex](https://msdn.microsoft.com/en-us/library/system.threading.mutex(v=vs.90).aspx),
[Semaphore](https://msdn.microsoft.com/en-us/library/system.threading.semaphore(v=vs.90).aspx).

> Код программы должен следовать принципам ООП и ООД (читаемость, разбиение на классы и тд).

Держим в голове парадигмы [ООП](https://en.wikipedia.org/wiki/Object-oriented_programming) 
и принципы [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself) и [KISS](https://en.wikipedia.org/wiki/KISS_principle).

> Алгоритм работы программы необходимо описать словами.

Пишем данный документ.

> Исходники необходимо прислать вместе с проектом Visual Studio.

Отправить ссылку на данный репозиторий, либо отправить архив (если :octocat: не подходит).

> Дополнительным плюсом будет возможность корректной остановки программы по Ctrl-C.

Используем событие [Console.CancelKeyPress](https://msdn.microsoft.com/en-us/library/system.console.cancelkeypress(v=vs.90).aspx).

## Решение

[Версия v0.1 (неэффективная схема распараллеливания)](release-0.1.md)

[Версия v0.2 (текущая)](release-0.2.md)

