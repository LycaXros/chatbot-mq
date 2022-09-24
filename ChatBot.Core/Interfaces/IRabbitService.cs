
namespace ChatBot.Core.Interfaces
{
    public interface IRabbitService
    {
        void Consume<T>(string queue, Action<T> execute);
        void Dispose();
        void Produce<T>(string queue, T message);
    }
}