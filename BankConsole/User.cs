using Newtonsoft.Json;
namespace BankConsole; //nombre del proyecto

public class User : IPerson
{
    /*agregar anotacion porque no podemos serializar atributos  privados */

    [JsonProperty]
    protected int ID
    {
        get;
        set;
    }
    [JsonProperty]
    protected string Name
    {
        get;
        set;
    }
    [JsonProperty]
    protected string Email
    {
        get;
        set;
    }
    [JsonProperty]
    protected decimal balance
    {
        get;
        set;

    }
    [JsonProperty]
    protected DateTime RegisterDate
    {
        get;
        set;
    }
    public User()
    {

    }
    public User(int ID, string Name, string Email, decimal Balance)
    {
        this.ID = ID;
        this.Name = Name;
        this.Email = Email;
        this.RegisterDate = DateTime.Now;
    }
    public DateTime getRegisterDate()
    {
        return RegisterDate;
    }
    public int GetID()
    {
        return ID;
    }
    public virtual void setBalance(decimal amount)
    {
        decimal quantity = 0;
        if (amount < 0)
        {
            quantity = 0;
        }
        else
        {
            quantity = amount;
        }
        this.balance += quantity;
    }
    /*virtual es para sobreescribir metodos*/
    public virtual string ShowDate()
    {

        return $"ID:{this.ID},Nombre: {this.Name},Correo: {this.Email},Saldo: {this.balance},Fecha de registro: {this.RegisterDate.ToShortDateString()}";
    }
    public string ShowDate(string initialMessage)
    {

        return $"{initialMessage}->Nombre: {this.Name},Correo: {this.Email},Saldo: {this.balance},Fecha de registro: {this.RegisterDate}";
    }

    public string GetName()
    {
        return Name;
    }

    public string GetCountry()
    {
        return "Mexico";
    }
}