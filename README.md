# DrVideoLibrary
Web app to manage a DVD collection. Search in some free API's.

## Projects directory
This version is developed with a Blazor WebAssembly like a Client app, and Azure Functions like a API. Following Clean Architecture.

* Domain:
    - DrVideoLibrary.Entities
* Application:
    - DrVideoLibrary.Backend.ApplicationBusinessRules
* Interfaces: 
    - DrVideoLibrary.Backend.InterfaceAdapters
    - DrVideoLibrary.Backend.PushNotifications
    - DrVideoLibrary.Backend.Repositories
    - DrVideoLibrary.Backend.Storage
    - DrVideoLibrary.Razor
* Frameworks:
    - DrVideoLibrary.Cosmos.DbContext
    - DrVideoLibrary.Api
    - DrVideoLibrary.Client

### Projects can change with you own implementation
* All Frameworks
* DrVideoLibrary.Backend.PushNotifications
* DrVideoLibrary.Backend.Storage
* DrVideoLibrary.Backend.Repositories

The most common can change is the API (you can use a NET Core Minimal Api easy), and the DbContext (you can use Entity Framework with SQL Server for example). This is the most common to change. Any other it's already good to use like is it the implementation, but also you can change while implement the inferfaces from DrVideoLibrary.Entities, DrVideoLibrary.Backend.ApplicationBusinessRules and DrVideoLibrary.Backend.Repositories (if you not will full change the repositories as well).

All implementations follow Options Pattern. So it's easy set your own database conection string and private api keys for the external APIs.

## Api keys
If you see some API keys is for testing purpose. Please don't use the keys is exposed here.

### Get your own API's here
This is the services it's implemented with free API key. All they are implemented in the project DrVideoLibrary.Backend.Repositories. You can still change to use your own.

* [Search movies only in english](https://www.omdbapi.com/)
* [Search movies in any lange](https://www.themoviedb.org/) here the code use to search with spanish titles
* [Translate texts](https://www.deepl.com/es/translator)

## Translations
System is prepare to use localizatoin for the text. You can colaborate and add other langs to the Reources.

# Conclusion
This it's a simple exaple project how to use Clean Architecture and ready to use with NET MAUI. I am glad if somebody fork this repository and implement the MAUI client, or any other client.
