
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace Products.Service;

public class Notification
{

    public static bool EmailSend { get; set; }

    public static DateTime SendDate { get; set; }

    public static int NotificationNo(params int[] numbers)
    {
        int total = 0;
        foreach (int number in numbers)
        {
            total += number;
        }
        return total;
    }

    public static async Task EmailNotification(string to, string subject, string body)
    {
        MailboxAddress from = new MailboxAddress("Inventory System Reminder", "no-reply@panda.pizza");
        var msg = new MimeMessage();
        msg.From.Add(from);
        msg.To.Add(new MailboxAddress("", to));
        msg.Subject = subject;

        msg.Body = new TextPart("html")
        {
            Text = @"This message is generated automatically, there are some products need your immidiate attention. See details below<br><br>" + "<ul>" + body + "</ul>"
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.mailtrap.io", 587, false);
            await client.AuthenticateAsync("a7b2ef3a42b718", "e255e86efac9ca");
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}