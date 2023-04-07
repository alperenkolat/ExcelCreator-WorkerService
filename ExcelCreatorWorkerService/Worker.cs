using ClosedXML.Excel;
using ExcelCreatorWorkerService.Models;
using ExcelCreatorWorkerService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Data;
using System.Text;
using System.Text.Json;

namespace ExcelCreatorWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly RabbitMQClientService _rabbitMQClientService;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;
        public Worker(ILogger<Worker> logger, RabbitMQClientService rabbitMQClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitMQClientService = rabbitMQClientService;
            _serviceProvider = serviceProvider; 
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {


          _channel=  _rabbitMQClientService.Connect();

            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer=new  AsyncEventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            
            await Task.Delay(5000);


            var excelCreaterMessage = JsonSerializer.Deserialize<ExcelCreaterMessage>
                (Encoding.UTF8.GetString(@event.Body.ToArray()));

            using var ms=new MemoryStream();
            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(GetTable("Employees"));
            
            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);
            MultipartFormDataContent multipartFormDataContent = new();

            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", Guid.NewGuid().ToString() + ".xlsx");


            var baseUrl = "http://localhost:44308/api/files";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"{baseUrl}?fileID={excelCreaterMessage.FileID}", multipartFormDataContent);


                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"File(ID:{excelCreaterMessage.FileID })was created by suucessful.. ");
                    _channel.BasicAck(@event.DeliveryTag, false);
                }
            };
        } 

        private DataTable GetTable(string tableName)
        {

            List<Emp> employees;

            using(var scope = _serviceProvider.CreateScope())
            {

                var context = _serviceProvider.GetRequiredService<ExcelCreaterContext>();
                employees =context.Emps.ToList();
            }



            DataTable table = new DataTable
            {
                TableName = tableName,
            };

            table.Columns.Add("Empno", typeof(int));
            table.Columns.Add("Ename", typeof(string));
            table.Columns.Add("Job", typeof(string));
            table.Columns.Add("Sal", typeof(decimal));
            table.Columns.Add("Comm", typeof(decimal));
            employees.ForEach(x =>
            {

                table.Rows.Add(x.Empno, x.Ename, x.Job, x.Job, x.Comm);
            });
           
            return table;
        }
    }
}