using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace PromotionsAPI.Utility
{
    public class PromotionEventPublisher : IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _startedSender;
        private readonly ServiceBusSender _endedSender;

        public PromotionEventPublisher(string connectionString)
        {
            _client            = new ServiceBusClient(connectionString);
            _startedSender     = _client.CreateSender("promotions-started-queue");
            _endedSender       = _client.CreateSender("promotions-ended-queue");
        }

        public async Task PublishPromotionStartedAsync(PromotionStartedEvent evt)
        {
            var json = JsonSerializer.Serialize(evt);
            var msg  = new ServiceBusMessage(json)
            {
                MessageId = Guid.NewGuid().ToString(),
                Subject   = nameof(PromotionStartedEvent)
            };
            msg.ApplicationProperties["eventType"] = "PromotionStarted";

            await _startedSender.SendMessageAsync(msg).ConfigureAwait(false);
        }

        public async Task PublishPromotionEndedAsync(PromotionEndedEvent evt)
        {
            var json = JsonSerializer.Serialize(evt);
            var msg  = new ServiceBusMessage(json)
            {
                MessageId = Guid.NewGuid().ToString(),
                Subject   = nameof(PromotionEndedEvent)
            };
            msg.ApplicationProperties["eventType"] = "PromotionEnded";

            await _endedSender.SendMessageAsync(msg).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await _startedSender.DisposeAsync().ConfigureAwait(false);
            await _endedSender.DisposeAsync().ConfigureAwait(false);
            await _client.DisposeAsync().ConfigureAwait(false);
        }
    }

    public record PromotionStartedEvent(
        int    Id,
        string Name,
        string Description,
        decimal Discount,
        DateTime StartDate,
        DateTime EndDate
    );

    public record PromotionEndedEvent(
        int    Id,
        string Name,
        DateTime EndDate
    );
}
