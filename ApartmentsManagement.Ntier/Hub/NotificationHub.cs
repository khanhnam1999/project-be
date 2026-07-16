using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendToResident(Guid residentId, Guid paymentId, string message) 
    { 
        await Clients.User(residentId.ToString()).SendAsync("Payment", message);
    }
}
