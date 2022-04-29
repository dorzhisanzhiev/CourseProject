# CourseProject

Программа - шифратор и дешифратор шифра Виженера. Программа реализована в виде web-приложения с использованием стека технологий ASP.NET Razor Pages. 

Входящие и исходящие форматы файла: .txt и .docx. Знаки препинания, и прочие элементы, не относящиеся к алфавиту сообщения, не изменяются. В сообщении используется русский алфавит.

Программа: 
1) Позволяет выбрать файл формата .txt или .docx, считывая из него информацию и выводя её на экран пользователя.
2) Предоставляет возможность для зашифровывания или дешифровывания информации, полученной из файла или введенной пользователем с клавиатуры, с возможностью указать ключ. Результат шифрования выводится на экран пользователя.
3) Предоставляет возможность сохранить результат шифрования в отдельный файл формата .txt или .docx, с указанием его названия и директории для сохранения.
4) Обладает интерфейсом для взаимодействия с пользователем, а также меню для управления функциональными возможностями программы.

Основной функционал покрыт автоматическими Unit тестами, с использованием стандартных возможностей Unit Testing Framework MSTest.
