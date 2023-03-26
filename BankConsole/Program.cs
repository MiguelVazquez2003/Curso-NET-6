using System.Text.RegularExpressions;
using BankConsole;

/*Client brayan=new Client(5,"Brayan","brayan67929@outlook.es",4000,'M');
Storage.AddUser(brayan);
Employee alan=new Employee(6,"Alan","ALAN@GMAIL.COM",4000,"IT");
Storage.AddUser(alan);*/

/*User usuario=new User(2,"Mike","miguel67929@outlook.es",5000);

usuario.setBalance(0);
Console.WriteLine(usuario.ShowDate("La informacion del usuario es la siguiente"));
Storage.AddUser(usuario);

User mike=new User(2,"Mike2","miguel56@outlook.es",2000);
Console.WriteLine(mike.ShowDate("La informacion del usuario es la siguiente"));
Storage.AddUser(mike);
//Console.WriteLine(mike.ShowDate()); */


/*si no le paso argumento a la aplicacion*/
if (args.Length == 0)
{
    EmailService.SendMail();
}
else
{
    showMenu();
}

void showMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opcion: ");
    Console.WriteLine("1 - Crear un usuario nuevo");
    Console.WriteLine("2 - Eliminar un usuario existente");
    Console.WriteLine("3 - Salir");

    int option = 0;

    do
    {
        string input = Console.ReadLine();
        if (!int.TryParse(input, out option))
        {
            Console.WriteLine("Debes ingresar un numero (1, 2 o 3)");

        }
        else if (option > 3)
        {
            Console.WriteLine("Debes ingresar un numero valido (1, 2 o 3)");

        }
    } while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;

    }
}

void CreateUser()
{   /*variables de validacion*/
    bool validar_decimal = false;
    bool email_validar = false;
    bool id_existente = false;
    bool validar = false;
    Console.Clear();
    Console.WriteLine("Ingresa la informacion del usuario:");


    Console.Write("ID: ");
    int ID;



    do
    {
        if (int.TryParse(Console.ReadLine(), out ID) && ID > 0)
        {
            id_existente = Storage.Validar_ID(ID); // validar que el ID exista.
            if (id_existente == false) //*si devuelve falso, el ID es diferente de null, o sea existe./
            {
                Console.WriteLine("El ID ya existe. Inténtalo de nuevo.");
                Console.Write("ID: ");
            }
            else
            {
                Console.WriteLine("El ID es válido.");
                validar = true;
            }
        }
        else
        {
            Console.WriteLine("El ID no es un entero positivo. Inténtalo de nuevo.");
            Console.Write("ID: ");
        }
    } while (!validar);


    Console.Write("Nombre: ");
    string name = Console.ReadLine();


    string email;
    do
    {
        Console.Write("Email: ");
        email = Console.ReadLine();
        email_validar = validarEmail(email);

        if (!email_validar)
        {
            Console.WriteLine("El email no tiene un formato valido.");
        }

    } while (!email_validar);

    decimal balance;
    Console.ReadKey();

    do
    {
        Console.Write("Saldo: ");
        string Saldo = Console.ReadLine();
        validar_decimal = decimal.TryParse(Saldo, out balance) && balance > 0;

        if (!validar_decimal)
        {
            Console.WriteLine("Debe ser un numero decimal positivo");
        }
    } while (!validar_decimal);



    char userType;

    User newUser;

    do
    {
        Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado: ");
        userType = Console.ReadLine()[0];

        if (userType != 'c' && userType != 'e')
        {
            Console.WriteLine("Debe ingresar 'c' o 'e'");
        }

    } while (userType != 'c' && userType != 'e');


    if (userType.Equals('c'))
    {
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }
    else
    {
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }


    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado");
    /*ver el mensaje de forma pausada*/
    Thread.Sleep(2000);
    showMenu();
}
void DeleteUser()
{
    bool validar = false;
    bool id_existente = true;
    Console.Clear();


    int ID; //= int.Parse(Console.ReadLine());
    do
    {
        Console.Write("Ingresa el ID del usuario a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out ID) && ID > 0)
        {
            id_existente = Storage.Validar_ID(ID); //si es falso, significa que existe ya que es!null.
            if (id_existente == false)
            {
                Console.WriteLine("El ID es válido.");
                validar = true;

            }
            else
            {
                Console.WriteLine("El ID no existe. Inténtalo de nuevo.");
                Console.Write("ID: ");
            }
        }
        else
        {
            Console.WriteLine("El ID no es un entero positivo. Inténtalo de nuevo.");
            Console.Write("ID: ");
        }
    } while (!validar);

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Sucess"))
    {
        Console.Write("Usuario eliminado. ");
        Thread.Sleep(2000);
        showMenu();
    }
}

bool validarEmail(string Email)
{
    /*expresion regular de un correo electronico*/
    string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    /*nombre_de_usuario@nombre_de_dominio.extension_de_dominio".*/
    // Verifica si el email cumple con el patrón de la expresión regular
    /*si coinciden las cadenas email y patron, devuelve true*/
    if (Regex.IsMatch(Email, patron))
    {
        return true;
    }
    else
    {
        return false;
    }
}


