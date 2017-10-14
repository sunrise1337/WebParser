Test task from Konstantin Deulin for Netpeak software

Ќеобходимо распарсить одну страницу https://netpeaksoftware.com/ и HTTP-ответа сервера, сделав следующее:

Ќастроить парсинг таких параметров, как: Title, Description, Server Response (код ответа сервера), Response Time (врем€ до получени€ первого бита), заголовки h1, изображени€ и все AHREF-ссылки (внутренние и внешние).
ƒать возможность вставл€ть дл€ парсинга любой URL.
Ќастроить удобный просмотр полученных данных.
“ребовани€:

WPF Framework проект или Xamarin.Mac проект.
¬ыполн€ть запросы к серверу использу€ класс System.Net.HttpWebRequest.
ѕарсить HTML библиотекой HtmlAgilityPack.
Ќастроить многопоточный парсинг средствами класса System.Threading.ThreadPool.
»нверси€ управлени€ (IoC), использу€ библиотеку Autofac.
ѕродуманный и юзабильный интерфейс.