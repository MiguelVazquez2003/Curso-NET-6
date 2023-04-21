using BankAPI.Data;
using BankAPI.Data.BankModels;
namespace BankAPI.Services;

public class ClientService{
 private readonly BankContext _context;
    public ClientService(BankContext context)
    {
        _context = context;
    }

    public IEnumerable<Client> GetAll(){
        return _context.Clients.ToList();
    }
    public Client? GetById(int id){
        return _context.Clients.Find(id);
            }

    public Client Create(Client newClient){
        _context.Clients.Add(newClient);
        _context.SaveChanges();

        return newClient;

    }
    public void Update (int id,Client client){
        var existingClient=GetById(id);
        if(existingClient is not null){
            existingClient.Name=client.Name;
            existingClient.PhoneNumber=client.PhoneNumber;
            existingClient.Email=client.Email;

            _context.SaveChanges();
        }
    }

    public void Delete (int id){
        var clientToDelete=GetById(id);

        if(clientToDelete is not null){
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
        }
    }
}
