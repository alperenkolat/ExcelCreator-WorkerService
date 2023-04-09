# Excel Creator Worker Service

## About This Project

Performing excel creation process from tables in web application via RabbitMQ in Worker Services.
### Used Technologies
* .Net (6.0)
* SignalR
* RabbitMQ
* ClosedXml
* Entity Framework
* JavaScript
* MSSQL
* SweetAlert


## project architecture
![image](https://user-images.githubusercontent.com/73352908/230746992-1731f237-07be-43a9-814e-57eefaa7de73.png)

### Installation

1. Clone the repo
   ```sh
     git clone https://github.com/alperenkolat/ExcelCreator-WorkerService.git
   ```
   
2. Delete the Migrations folder from the project
3. Set the url of your MS SQL Server and RabbitMQ from the appsetting.cs file
4. Go to the project location from the terminal and run the following commands in order. Instead of the migration name, you can write the name you want. (Ex. dotnet ef migrations add InititialCreate)
5. Add migrations
   ```
      dotnet ef migrations add migrationName
   ```
6. Update database
   ```
     dotnet ef database update
   ```
## Acknowledgments
Resources I used while developing the project

[.NET Dooc](https://learn.microsoft.com/en-us/dotnet/)

[SignaR Cdn](https://cdnjs.com/libraries/aspnet-signalr)

[RabbitMQ](https://www.rabbitmq.com/)

[Sweetalert](https://sweetalert2.github.io/)

[EF Core](https://www.entityframeworktutorial.net/)

[Udmey Course-Fatih Çakıroğlu](https://www.udemy.com/share/102rNc3@t65bWcgZIRLE1qpFaUUpYJLvQkWHpzQnJmTMSEq27Bs66TmS71n1Bg7fpdQLf3-5fQ==/)
