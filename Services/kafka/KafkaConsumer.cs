using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Services.kafka
{
    public class KafkaConsumer : BackgroundService
    { 
        private ConsumerConfig _config;
        public KafkaConsumer(ConsumerConfig config)
        {
            _config = config;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => Start(stoppingToken));
            return Task.CompletedTask;
        }

        private void Start(CancellationToken stoppingToken)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };
            using (var c = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                var topics = new List<string>() { "user-topic", "product-topic" };
                c.Subscribe(topics);
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = c.Consume(cts.Token);
                    var data = "";
                    switch (cr.Topic)
                    {
                        case "user-topic":
                            data = cr.Value;
                            break;
                        case "product-topic":
                            data = cr.Value;
                            break;
                        default:
                            break;
                    }
                }
            }


        }
    }

}

