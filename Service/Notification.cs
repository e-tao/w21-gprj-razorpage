
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace Products.Service;

public class Notification
{
    public static int NotificationNo(params int[] numbers)
    {
        int total = 0;
        foreach (int number in numbers)
        {
            total += number;
        }
        return total;
    }
}