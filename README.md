# notifier.net/gnotifier.net
Notifier of new emails.  
Уведомитель о приходе новых писем.  
### Уведомитель позволяет
- получать уведомления о новых письмах в любых ваших папках, которые у вас есть в почтовой службе и даже в папке Спам, если вы считаете, что туда могут попадать важные письма;
- получать стандартные системные уведомления с выбранной вами переодичностью;
- открывать в вашем web-обозревате вашу почту.
### Общие сведения
Уведомитель собран в двух вариантах с обычной иконкой и с новой иконкой почты gmail.  

Каждый вариант имеет свое имя запусконго exe-файла и свой ini-файл с настройками.  

Программа представлена с открытым исходным кодом, чтобы вы были уверены, что ваш пароль от почтового ящика никуда не пересылается и никуда не утекает, а только передается в зашифрованном виде методом TLS v1.2 для входа на IMAP-сервер вашего почтового провайдера. Если в файле настроек в параметре passw вы указали пароль так, как вы его получили в вашей почтовой службе, он сразу щифруется и перезаписывается в параметре passs, чтобы злоумышленник, случайно увидев ваш пароль не мог им воспользоваться. Если позволяет ваш почтовый провайдер, получите специальный пароль для приложений, который предоставляет доступ к IMAP-серверу только для чтения. Такой пароль предоставляется всеми известными почтовыми службами, если только у вас не своя корпоративная почтовая служба с ограниченными в плане безопасности возможностями.  

При чтении настроек в ini-файле, сначала просматривается первая позизия. Если это пустая строка или строка начинающаяся с ";", то вся строка игнорируется. Если строка начинается с "[", то строка воспринимается как название секции с параметрами. При первом обращении обычно секция [Folders] пустая. Если секция [Folders] пустая, то программа запрашивает все доступные вам папки и записывает их названия, так как они указаны на сервере, в секцию [Folders]. В этом случае сюда будут записаны и вспомогательные папки, проверять письма в которых не имеет смысла, в том числе папка "Корзина". После появления всех папкок откройте ini-файл с настройками и закомментируйте или удалите соответствующие строки с папками, которые проверять не нужно. Для обновления новых настроек вы можете воспользоваться в контекстном меню программы пунктом "Перезапуск".  

В параметре url вы указываете url вашего почтового провайдера для просмотра почты. Например: `url = https://mail.google.com/`. Данный url будет открываться в обозревателе по умолчанию при клике на уведомлении или клике на иконке в трее.  

Если вам необходимо проверять сразу 2 почтовых ящика, вы можете использовать оба варианта сборки. При желании вы можете проверять и большее количество ящиков, если каждый экземпляр программы поместите в отдельные папки.  

Чтобы уведомитель запускался при входе в ваш сеанс Windows, создайте ярлык/и на программу/ы и поместите их в папку для автозапуска программ, например: "C:\Users\kornienko\главное меню\Программы\Startup\". 
### Как скачать
Пока уведомитель не обретет популярность, Windows будет считать его потенциально "опасной" программой :-) Поэтому, подтвержаайте, если что, что вы доверяете этой программе. Чтобы скачать всё — тексты, экзешники и заготовки файлов с настройками нажмите зеленую кнопку `<> Code ▾`, а далее выбирите пункт `Download ZIP`.
### История версий
0.0.4.0. 26.02.2025 Внесена поправка для проверки почты каждую минуту (так часто проверять не желательно). В предыдущей версии для интервала 1 минута уведомления не появлялись.  
