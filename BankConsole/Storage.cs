using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace BankConsole;

public static class Storage
{
    /*directorio base de la aplicacion*/
    static string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\users.json";


    public static void AddUser(User user)
    {
        string json = "";
        string usersInFile = "";

        if (File.Exists(filePath))
        {
            usersInFile = File.ReadAllText(filePath);
        }
        /*convertir json a objeto c#*/
        var listUsers = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if (listUsers == null)
        {
            listUsers = new List<object>();
        }
        listUsers.Add(user);
        /*la informacion de nuestro archivo json este identado*/
        JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        /**/
        json = JsonConvert.SerializeObject(listUsers, settings);
        /**/
        File.WriteAllText(filePath, json);

        /*SERIALIZAR:
        convertir objeto de c# a formato JSON*/
        /*json=JsonConvert.SerializeObject(user);
        /*este metodo crea un archivo en la ruta y la informacion de la cadena*/
        /*File.WriteAllText(filePath,json);*/
    }
    public static List<User> GetNewUsers()
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if (File.Exists(filePath))
        {
            usersInFile = File.ReadAllText(filePath);
        }
        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);
        if (listObjects == null)
        {
            return listUsers;
        }
        foreach (object obj in listObjects)
        {
            User newUser;
            /*el objeto user sea un nuevo tipo JObject
            representa un objeto json y puedo buscar una propiedad el objeto
            */
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
            {
                newUser = user.ToObject<Client>();
            }
            else
            {
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        var newUsersList = listUsers.Where(user => user.getRegisterDate().Date.Equals(DateTime.Today)).ToList();
        return newUsersList;
    }
    public static string DeleteUser(int ID)
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if (File.Exists(filePath))
        {
            usersInFile = File.ReadAllText(filePath);
        }
        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);
        if (listObjects == null)
        {
            return "There are no users in the file";
        }
        foreach (object obj in listObjects)
        {
            User newUser;
            /*el objeto user sea un nuevo tipo JObject
            representa un objeto json y puedo buscar una propiedad el objeto
            */
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
            {
                newUser = user.ToObject<Client>();
            }
            else
            {
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        /*single trae el unico registro que cumpla la condicion*/
        var userToDelete = listUsers.Where(user => user.GetID() == ID).Single();
        listUsers.Remove(userToDelete);

        JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        string json = JsonConvert.SerializeObject(listUsers, settings);
        File.WriteAllText(filePath, json);
        return "Sucess";


    }

    public static bool Validar_ID(int ID)
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if (File.Exists(filePath))
        {
            usersInFile = File.ReadAllText(filePath);
        }
        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);
        if (listObjects == null)
        {
            return false;
        }
        foreach (object obj in listObjects)
        {
            User newUser;
            /*el objeto user sea un nuevo tipo JObject
            representa un objeto json y puedo buscar una propiedad el objeto
            */
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
            {
                newUser = user.ToObject<Client>();
            }
            else
            {
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        /*single trae el unico registro que cumpla la condicion*/

        var ID_validar = listUsers.SingleOrDefault(user => user.GetID() == ID);
        if (ID_validar != null)
        {
            return false;
        }

        return true;

    }



}