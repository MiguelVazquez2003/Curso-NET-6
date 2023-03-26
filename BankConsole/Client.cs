namespace BankConsole;
/*cliente es hija de user*/

public class Client : User
{
    public char TaxRegime { get; set; }
    /*base es una palabra clave que hace referencia a la clase padre*/
    public Client(int ID, string Name, string Email, decimal Balance, char TaxRegime) : base(ID, Name, Email, Balance)
    {
        this.TaxRegime = TaxRegime;
        setBalance(Balance);
    }
    public Client()
    {

    }
    public override void setBalance(decimal amount)
    {
        base.setBalance(amount);
        if (TaxRegime.Equals('M'))
        {
            balance += (amount * 0.02m);
        }
    }
    public override string ShowDate()
    {
        return base.ShowDate() + $",Regimen Fiscal: {this.TaxRegime}";
    }

}