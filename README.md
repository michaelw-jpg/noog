# Noog

## Översikt

- Projekt gjort av Michael, Matheus, Oliver och Simon som en del av ett skolprojekt.
- Programmet består av ett primärt monorepo i GitHub, där vår backend och det mesta av Frontend utgår ifrån. Utöver det finns ett React repo https://github.com/ikariLain/NoogReact och en microservice i express.js https://github.com/ikariLain/Noog-Express-Microservice 
- Översiktlig beskrivning: En kollaborationsplattform där man kan skapa konto, skapa projektgrupper, bjuda in andra, ha möten/samtal, skapa AI-gemererade sammanfattningar av det som sagts under möten och se dessa sammansfattningar i sin projektgrupp.

## Arkitektur

- Backend: Asp.Net Web Api, Auth med Identity, .NET8, OpenAI(Summary Generation), AssemblyAI (Transcripts).
- Frontend (1): MVC Asp.Net, ViewComponents, CSS, Razor, Layered _Layouts.
- Frontend (2): React Vite Typescript, StreamIO (samtal).
- Microservice: Express.js, StreamIO (Call Crud).

## Övrigt:

Länk till driftsatt startpunkt: https://noogmvc-dagbgyecakdcecem.swedencentral-01.azurewebsites.net/ 
