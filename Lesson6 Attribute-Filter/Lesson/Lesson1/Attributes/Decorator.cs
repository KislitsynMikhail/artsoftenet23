using System;

namespace Lesson1.Attributes;

public static class DecoratorVsAttribute
{
    public static void Example()
    {
        Notification push = new Push();
        push = new SendByServer1(push);
        Write(push);

        Notification sms = new Sms();
        sms = new SendByServer2(sms);
        Write(sms);
        
        Notification email = new Email();
        email = new SendByServer2(email);
        Write(email);
    }

    private static void Write(Notification notification)
    {
        Console.WriteLine($"Уведомление: {notification.Name}");
    }
}

abstract class Notification
{
    protected Notification(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

class Push : Notification
{
    public Push() : base("Push ")
    { }
}

class Sms : Notification
{
    public Sms() : base("Sms ")
    {
    }
}

class Email : Notification
{
    public Email() : base("Email ")
    {
    }
}

abstract class NotificationDecorator : Notification
{
    private Notification notification;

    protected NotificationDecorator(string notificationName, Notification notification) 
        : base(notificationName)
    {
        this.notification = notification;
    }
}

class SendByServer1 : NotificationDecorator
{
    public SendByServer1(Notification p) : base(p.Name + "sending by server 1", p)
    {
    }
}

class SendByServer2 : NotificationDecorator
{
    public SendByServer2(Notification p) : base(p.Name + "sending by server 2", p)
    {
    }
}