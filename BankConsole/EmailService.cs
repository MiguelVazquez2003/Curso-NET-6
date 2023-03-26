using MailKit.Net.Smtp;
using MimeKit;
namespace BankConsole;

public static class EmailService
{
    public static void SendMail()
    {
        /*para el mensaje que queremos enviar
        */
        var message = new MimeMessage();
        /*Remitente*/
        message.From.Add(new MailboxAddress("Miguel Vazquez", "polliyo67@gmail.com"));
        /*Destinatario*/
        message.To.Add(new MailboxAddress("Admin", "marcadostalker4@gmail.com"));
        /*Asunto*/
        message.Subject = "BankConsole: Usuarios nuevos";
        /*Cuerpo del mensaje*/
        message.Body = new TextPart("plain")
        {
            Text = getEmailText()
        };
        /*enviar el correo*/

        /*definir un nuevo objeto de SmtpClient*/
        using (var client = new SmtpClient())
        {
            /*servidor del correo, puerto, y su usaremos SSL*/
            client.Connect("smtp.gmail.com", 587, false);
            /**/
            client.Authenticate("polliyo67@gmail.com", "izewpcddrgsicliv");
            client.Send(message);
            client.Disconnect(true);
        }

    }
    private static string getEmailText()
    {
        /*lista de objetos tipo user
        */
        List<User> newUsers = Storage.GetNewUsers();
        if (newUsers.Count == 0)
        {
            return "No hay usuarios nuevos";
        }
        string emailText = "Usuarios agregados hoy: \n";
        /*iterar */
        foreach (User user in newUsers)
        {
            emailText += "\t+ " + user.ShowDate() + "\n";
        }
        return emailText;
    }

}