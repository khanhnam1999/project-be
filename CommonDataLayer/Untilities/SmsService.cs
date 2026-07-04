using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Mail;

public class SmsService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "nam.lh.1950@aptechlearning.edu.vn";   // email gửi đi
    private readonly string _smtpPass = "smth tjmn elhu brec";      // app password (không dùng mật khẩu Gmail trực tiếp)
    private readonly IMemoryCache _cache;

    public SmsService(IMemoryCache cache)
    {
        _cache = cache;
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser, "Apartment Management App"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }

    public string GenerateAndSaveOtp(string email)
    {
        var otp = GenerateOtp(6);

        // Lưu OTP vào cache với thời gian sống 5 phút
        _cache.Set($"OTP_{email}", otp, TimeSpan.FromMinutes(5));
        string subject = "Xác nhận tài khoản";
        string message = $"Mã OTP của bạn là: {otp}. OTP sẽ hết hạn sau 5 phút.";
        SendEmailAsync(email, subject, message);

        return otp;
    }

    public bool VerifyOtp(string email, string inputOtp)
    {
        if (_cache.TryGetValue($"OTP_{email}", out string cachedOtp))
        {
            return cachedOtp == inputOtp;
        }
        return false;
    }

    private string GenerateOtp(int length = 6)
    {
        var random = new Random();
        return string.Concat(Enumerable.Range(0, length)
            .Select(_ => random.Next(0, 10).ToString()));
    }
}
