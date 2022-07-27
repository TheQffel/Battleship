 # [ENG] Battleship - a strategy game for two people.
 ## Demo: http://91.246.104.18/ -> Battleship (WAN)
 ### Run (ASP): Battleship.exe --urls "http://*:5000"
 ### Run (NodeJS): npm run start (then "Y")
 **Requires a database to run. The hostname, username, password and table must be set in the * appsettings.json * file. The database in mysql should look like this:**
 
    CREATE TABLE `Battleship_Games` (` GameId` bigint (20) NOT NULL, `PlayerA` tinytext NOT NULL,` PlayerB` tinytext NOT NULL, `Turn` tinyint (1) NOT NULL,` BoardA` longtext NOT NULL, `BoardB` longtext NOT NULL);
    CREATE TABLE `Battleship_Users` (` Username` text NOT NULL, `Password` text NOT NULL);

 To start the game you need to log in or create an account. The login and password are stored in the form of cookies. After registration, select a game from the list or start a new one. To do this, enter your opponent's nickname. Then, place 6, 5, 4, 3, 2 cells long ships in that order, 1 of each. Ships must not touch each other and must be kept in a straight line. After both players have placed their ships, the player who starts the game "shoots" into the opponent's square. If he hit, the game continues, if not, it is the opponent's turn. The game continues until one player loses all of his ships.

 > In this app, empty space is marked in blue, ships are marked in green, misses are marked in yellow, and shots on target are marked in red.


 # [PL] Okręty (ang. Battleship) – gra strategiczna dla dwóch osób.
 ## Demo: http://91.246.104.18/ -> Battleship (WAN)
 ### Uruchamianie (ASP): Battleship.exe --urls "http://*:5000"
 ### Uruchamianie (NodeJS): npm run start (potem "Y")
 **Do uruchomienia potrzebna jest baza danych. Adres hosta, użytkownik, hasło i tabelę należy ustawić w pliku *appsettings.json*. Baza danych w mysql powinna wyglądać następująco:**
 
    CREATE TABLE `Battleship_Games` (`GameId` bigint(20) NOT NULL, `PlayerA` tinytext NOT NULL, `PlayerB` tinytext NOT NULL, `Turn` tinyint(1) NOT NULL, `BoardA` longtext NOT NULL, `BoardB` longtext NOT NULL);
    CREATE TABLE `Battleship_Users` (`Username` text NOT NULL, `Password` text NOT NULL);

 Aby rozpocząć grę należy zalogować się lub założyć konto. Login i hasło przechowywane jest w postaci plików cookie. Po rejestracji należy wybrać z listy grę lub rozpocząć nową. Aby to zrobić, należy wpisać nickname przeciwnika. Następnie należy rozłożyć statki o długości 6, 5, 4, 3, 2, w tej kolejności, każdego po 1 sztuce. Statki nie mogą się stykać i muszą być ułożone w prostej linii. Po ułożeniu statków przez obydwu graczy, gracz który zaczyna grę "oddaje strzał" na pole przeciwnika. Jeżeli trafił, gra dalej, jeżeli nie, kolej przechodzi na przeciwnika. Gra toczy się do momentu gdy jeden z graczy straci wszystkie swoje statki.

 > W tej aplikacji puste pole oznaczone jest na niebiesko, statki oznaczone są na zielono, strzały chybione na żółto, a strzały celne na czerwono.
