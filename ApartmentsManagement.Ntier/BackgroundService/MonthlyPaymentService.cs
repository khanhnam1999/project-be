using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.SignalR;

public class MonthlyPaymentService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    //private readonly IPaymentBL _paymentBL;
    //private readonly IHubContext<NotificationHub> _hubContext;
    //public MonthlyPaymentService(IPaymentBL paymentBL, IHubContext<NotificationHub> hubContext)
    //{
    //    _paymentBL = paymentBL;
    //    _hubContext = hubContext;
    //}

    public MonthlyPaymentService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;

            // Kiểm tra nếu là ngày mùng 1 và đúng 0h
            if (now.Day == 1 && now.Hour == 0)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var paymentBL = scope.ServiceProvider.GetRequiredService<IPaymentBL>();
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    try
                    {
                        List<Payment> payments = await paymentBL.GeneratePayments();
                        foreach (Payment payment in payments)
                        {
                            await hubContext.Clients.User(payment.ResidentId.ToString()).SendAsync("Payment", payment.PaymentId, payment.Title);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi tạo payment: {ex.Message}");
                    }
                }
                    // Logic tạo ticket payment
                    Console.WriteLine("Tạo ticket payment ngày mùng 1");

                // Chờ 1 phút để tránh chạy nhiều lần trong cùng giờ
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }


            // Kiểm tra mỗi phút
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}