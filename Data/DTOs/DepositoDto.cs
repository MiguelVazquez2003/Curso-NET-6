using BankAPI.Data.DTOs;
namespace BankAPI.Data.DTOs;

   public class DepositDto
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public int? TransactionType { get; set; }

    public bool IsValid()
    {
        if (TransactionType == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


