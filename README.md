# DT191G Projekt - ProjectApp
Detta repo innehåller en applikation med en tillhörande SQLite-databas för administration av olika projekt som jag har skapat inom ramen för min utbildning till webbutvecklare. 

Applikationen har konstruerats med ramverken ASP.NET Core och EF Core, designmönstret MVC och funktionaliteten Identity som används för hantering av autentisering och användaridentiteter i applikationen.

Applikationen har stöd för CRUD och innehåller funktionalitet för uppladdning och radering av bilder. Därtill har ett API som möjliggör utläsning av data från databasen från externa källor adderats till applikationen genom en kontroller. 

API:et kan konsumeras genom en GET-förfrågan till routen `/api/project`.