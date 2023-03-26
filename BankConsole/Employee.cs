namespace BankConsole;

public class Employee : User, IPerson
{
    public string Department
    {
        get;
        set;
    }
    public Employee(int ID, string Name, string Email, decimal Balance, string Department) : base(ID, Name, Email, Balance)
    {
        this.Department = Department;
        setBalance(Balance);
    }
    public Employee()
    {

    }
    public override void setBalance(decimal amount)
    {
        base.setBalance(amount);

        if (Department.Equals("IT"))
        {
            balance += (amount * 0.05m);
        }


    }
    public override string ShowDate()
    {
        return base.ShowDate() + $",Departamento: {this.Department}";
    }

}