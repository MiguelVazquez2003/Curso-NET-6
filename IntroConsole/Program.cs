
Console.WriteLine("\t---------Banco CDIS-------");

/*Variable para elegir la opcion del menu*/
int entero;
/*Variable que guarda el numero de retiros para recorrerlos*/
int num_retiros = 0;
/*Variable que guarda el dinero de cada retiro en un arreglo*/
int[] dinero_retiros = new int[10];
/*variable auxiliar para validar el dinero ingresado*/
int aux_dinero_retiros = 0;
/*variable utilizada para determinar si es entero*/
int residuo = 0;
/*b1 representa los billetes de 500, b2 200, b3 100, b4 50 y b5 de 20. m1 monedas de 10, m2 de 5 y m3 de 1 peso.*/
/*b_totales representa los billetes totales y m_totales las monedas*/
int b1 = 0, b2 = 0, b3 = 0, b4 = 0, b5 = 0, m1 = 0, m2 = 0, m3 = 0, b_totales = 0, m_totales = 0;


/*usamos un ciclo while para que cuando el usuario ingrese cualquier numero diferente de 1 y 2, se termine el programa*/
while (true)
{
    Console.WriteLine("1.Ingresar la cantidad de retiros hechos por los usuarios.\n");

    Console.WriteLine("2.Revisar la cantidad entregada de billetes y monedas.\n");

    Console.WriteLine("\nIngresa la opcion: ");
    /*usamos int.Parse para convertir el string en entero*/
    entero = int.Parse(Console.ReadLine());



    /*usamos un switch, aunque tambien se puede usar una estructura if*/
    switch (entero)
    {

        case 1:
            Console.WriteLine("¿Cuantos retiros de hicieron?(maximo 10): ");

            num_retiros = int.Parse(Console.ReadLine());
            /*si el residuo es igual a 0, significa que es entero. por ejemplo: num_retiros=5. 5/1=5 con residuo 0.*/
            residuo = num_retiros % 1;

            if (residuo == 0 && num_retiros > 0 && num_retiros <= 10)
            {
                for (int i = 0; i < num_retiros; i++)
                {
                    Console.WriteLine("\nIngresa la cantidad del retiro {0}: ", i + 1);
                    /*usamos variable auxiliar para validar en el if que sea entera*/
                    aux_dinero_retiros = int.Parse(Console.ReadLine());

                    residuo = aux_dinero_retiros % 1;
                    if (residuo == 0 && aux_dinero_retiros <= 50000 && aux_dinero_retiros > 0)
                    {
                        /*si se cumple, asignamos el valor a la posicion del arreglo en la que estemos*/
                        dinero_retiros[i] = aux_dinero_retiros;

                    }
                    else
                    {
                        /*si no, restamos una posicion al contador ya que no se cumple.*/
                        Console.WriteLine("\n1.La cantidad de retiro debe ser como maximo 50,000.\n2.La cantidad debe ser entera y positiva.");
                        i--;
                    }


                }
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();


            }
            else
            {
                Console.WriteLine("\nEl numero de retiros debe ser entero,positivo y menor que 10.");
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();



            }


            break;
        case 2:
            /*recorremos el arreglo.*/
            for (int i = 0; i < num_retiros; i++)
            {
                /*en esta linea, convertimos a decimal dinero_retiros, para poder realizar la operacion
                dividimos entre el valor del billete y nos queda un decimal. 
                Con Math.Floor se redondea hacia abajo
                Con (int) lo volvemos a hacer entero.
                y despues restamos el dinero que ya se ha calculado.*/
                b1 = (int)Math.Floor((decimal)dinero_retiros[i] / 500);
                dinero_retiros[i] = dinero_retiros[i] - (b1 * 500);

                b2 = (int)Math.Floor((decimal)dinero_retiros[i] / 200);
                dinero_retiros[i] = dinero_retiros[i] - (b2 * 200);

                b3 = (int)Math.Floor((decimal)dinero_retiros[i] / 100);
                dinero_retiros[i] = dinero_retiros[i] - (b3 * 100);

                b4 = (int)Math.Floor((decimal)dinero_retiros[i] / 50);
                dinero_retiros[i] = dinero_retiros[i] - (b4 * 50);

                b5 = (int)Math.Floor((decimal)dinero_retiros[i] / 20);
                dinero_retiros[i] = dinero_retiros[i] - (b5 * 20);

                m1 = (int)Math.Floor((decimal)dinero_retiros[i] / 10);
                dinero_retiros[i] = dinero_retiros[i] - (m1 * 10);

                m2 = (int)Math.Floor((decimal)dinero_retiros[i] / 5);
                dinero_retiros[i] = dinero_retiros[i] - (m2 * 5);

                m3 = (int)Math.Floor((decimal)dinero_retiros[i] / 1);
                dinero_retiros[i] = dinero_retiros[i] - (m3 * 1);

                b_totales = b1 + b2 + b3 + b4 + b5;
                m_totales = m1 + m2 + m3;
                /*por ultimo, guardamos los billetes en una variable, aunque podriamos imprimir directamente tambien.*/
                Console.WriteLine("Retiro {0}", i + 1);
                Console.WriteLine("Billetes entregados: {0}", b_totales);
                Console.WriteLine("Monedas entregadas: {0}\n", m_totales);
            }
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();

            break;

        default:
            return;
    }
    Console.Clear();
}



