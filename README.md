# Appointo

> Appointo is a website where hairdressers have a profile and customers can book appointments.

## Vragen voor feedback

- Pas ik bij de endpoints de best practices overal toe?
- Swagger neemt enkel bij hairdressers alle codes over, bij appointments en treatmnets niet. Ik neem aan dat dat zichzelf zal oplossen van zodra ik appointments en treatments in een eigen controller kan zetten?

## Requirements backend Web IV

> De nodige screenshots staan onder de informatie van het document.

#### De readme

- [x] Printscreen van de API zoals weergegeven in swagger. Per endpoint een printscreen van de parameters en de responses

- [x] Printscreen van het klassendiagram van de domeinlaag (toont de klassen met properties en methodes (inclusief de datatypes) en de associaties)(meer op https://fmoralesdev.com/2019/05/16/generate-class-diagram-vs2019-net-core/)

- [x] Opsomming van de instellingen die nodig zijn om je backend project lokaal te runnen, indien nodig

- [x] Opsomming van de eventuele extra’s

- [x] Voorbereiding feedback moment :

  - dit document waarin je aanvinkt wat je reeds hebt gerealiseerd

  - Opsomming van de vragen die je hebt over je backend project en waarover je feedback wenst

### Domein laag

- [x] Het domein bevat minstens 2 geassocieerde klassen

- [x] Klassen bevatten toestand en gedrag

- [x] Klassendiagram is aangemaakt, toont de properties, methodes en de associaties

### Data laag

- [x] DataContext is aangemaakt

- [x] Mapping is geïmplementeerd (In DataContext zoals in Recipe REST API voorbeeld, of a.d.h.v. Mapper klassen)

- [x] Databank wordt geseed met data (In DataContext zoals in Recipe REST API voorbeeld, of via initializer)

### Controller

- [x] Minstens 1 controller met endpoints voor de CRUD operaties

- [x] De endpoints zijn gedefinieerd volgens de best practices

- [x] Enkel de benodigde data wordt uitgewisseld (DTO’s indien nodig)

### Swagger

- [x] De documentatie is opgesteld

## Domain Class Diagram

![DCD](https://i.imgur.com/973BOTv.png)

## API

### Overview

![SwaggerAccountOverview](https://i.imgur.com/4Mh0vdI.png)
![SwaggerHairdresserOverview](https://i.imgur.com/eJrOClJ.png)
![SwaggerManageOverview](https://i.imgur.com/BH40qsm.png)
