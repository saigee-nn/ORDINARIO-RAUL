namespace ORDINARIO_PROF_RAUL
{
    enum Especie
    {
        Perro,
        Gato,
        Capibara
    }

    enum Temperamento
    {
        Amable,
        Nervioso,
        Agresivo
    }

    interface IAcariciable
    {
        void ResponderACaricia();
    }

    class Mascota
    {
        private static int contadorId = 1;

        public string Id { get; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public Especie Especie { get; set; }
        public Temperamento Temperamento { get; set; }
        public Persona Dueño { get; set; }

        public Mascota(string nombre, int edad, Especie especie, Temperamento temperamento)
        {
            Id = $"{especie}-{contadorId}";
            contadorId++;

            Nombre = nombre;
            Edad = edad;
            Especie = especie;
            Temperamento = temperamento;
        }

        public void HacerRuido()
        {
            switch (Especie)
            {
                case Especie.Perro:
                    Console.WriteLine("Guau Guau");
                    break;
                case Especie.Gato:
                    Console.WriteLine("Miau Miau");
                    break;
                case Especie.Capibara:
                    Console.WriteLine("Cui Cui");
                    break;
                default:
                    Console.WriteLine("Sonido desconocido");
                    break;
            }
        }

        public void CambiarDueño(Persona nuevoDueño)
        {
            if (Dueño != null)
            {
                Console.WriteLine($"{ObtenerNombreEspecie()} {Nombre} ha cambiado su dueño a {nuevoDueño.Nombre}.");
            }

            Dueño = nuevoDueño;
            nuevoDueño.AgregarMascota(this);
        }

        public void MoverCola()
        {
            if (Especie == Especie.Perro)
            {
                Console.WriteLine("Mueve la cola felizmente");
            }
        }

        public void Gruñir()
        {
            if (Especie == Especie.Perro)
            {
                Console.WriteLine("Grrrrr");
            }
        }

        public void Ronronear()
        {
            if (Especie == Especie.Gato)
            {
                Console.WriteLine("Rrrrrrrr");
            }
        }

        public void Rasguñar()
        {
            if (Especie == Especie.Gato)
            {
                Console.WriteLine("¡Cuidado con esas garras!");
            }
        }

        public string ObtenerNombreEspecie()
        {
            return Enum.GetName(typeof(Especie), Especie);
        }
    }

    class Perro : Mascota, IAcariciable
    {
        public Perro(string nombre, int edad, Temperamento temperamento)
            : base(nombre, edad, Especie.Perro, temperamento)
        {
        }

        public void ResponderACaricia()
        {
            MoverCola();
        }
    }

    class Gato : Mascota, IAcariciable
    {
        public Gato(string nombre, int edad, Temperamento temperamento)
            : base(nombre, edad, Especie.Gato, temperamento)
        {
        }

        public void ResponderACaricia()
        {
            if (Temperamento == Temperamento.Amable || Temperamento == Temperamento.Nervioso)
            {
                Ronronear();
            }
            else if (Temperamento == Temperamento.Agresivo)
            {
                Rasguñar();
            }
        }
    }

    class Capibara : Mascota
    {
        public Capibara(string nombre, int edad)
            : base(nombre, edad, Especie.Capibara, Temperamento.Amable)
        {
        }
    }

    class Persona
    {
        private static int contadorId = 1;

        public string Id { get; }
        public string Nombre { get; set; }
        public List<Mascota> Mascotas { get; }

        public Persona(string nombre)
        {
            Id = $"Persona-{contadorId}";
            contadorId++;

            Nombre = nombre;
            Mascotas = new List<Mascota>();
        }

        public void AgregarMascota(Mascota mascota)
        {
            Mascotas.Add(mascota);
        }

        public Mascota ObtenerMascotaPorId(string id)
        {
            return Mascotas.Find(mascota => mascota.Id == id);
        }

        public List<Mascota> ObtenerMascotas()
        {
            return Mascotas;
        }

        public void Acariciar(Mascota mascota)
        {
            if (mascota is IAcariciable acariciable)
            {
                Console.WriteLine($"{Nombre} acaricia a {mascota.Nombre}.");
                acariciable.ResponderACaricia();
            }
            else
            {
                Console.WriteLine($"{Nombre} intenta acariciar a {mascota.Nombre}, pero no es posible.");
            }
        }

        public void AcariciarMascotas()
        {
            foreach (var mascota in Mascotas)
            {
                Acariciar(mascota);
            }
        }
    }

    class CentroMascotas
    {
        private List<Persona> personas;
        private List<Mascota> mascotas;

        public CentroMascotas()
        {
            personas = new List<Persona>();
            mascotas = new List<Mascota>();
        }

        public void RegistrarPersona(string nombre)
        {
            Persona persona = new Persona(nombre);
            personas.Add(persona);
            Console.WriteLine($"Se ha registrado a {persona.Nombre} en el centro.");
        }

        public void MostrarPersonas()
        {
            Console.WriteLine("Personas registradas en el centro:");
            foreach (var persona in personas)
            {
                Console.WriteLine($"ID: {persona.Id}, Nombre: {persona.Nombre}");
            }
        }

        public Persona BuscarPersonaPorNombre(string nombre)
        {
            return personas.Find(p => p.Nombre.Contains(nombre));
        }

        public void ExaminarPersona(string id)
        {
            Persona persona = personas.Find(p => p.Id == id);

            if (persona == null)
            {
                Console.WriteLine("No se encontró ninguna persona con ese ID.");
                Console.Write("¿Desea buscar por nombre? (Sí/No): ");
                string respuesta = Console.ReadLine();

                if (respuesta.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Ingrese el nombre a buscar: ");
                    string nombreBusqueda = Console.ReadLine();
                    BuscarPersonaPorNombreYExaminar(nombreBusqueda);
                }
                else
                {
                    Console.WriteLine("Regresando al menú anterior.");
                }
            }
            else
            {
                MostrarDatosPersona(persona);
            }
        }

        private void BuscarPersonaPorNombreYExaminar(string nombre)
        {
            List<Persona> personasEncontradas = personas.FindAll(p => p.Nombre.Contains(nombre));

            if (personasEncontradas.Count == 0)
            {
                Console.WriteLine("No se encontraron personas con ese nombre.");
                Console.Write("¿Desea intentarlo de nuevo? (Sí/No): ");
                string respuesta = Console.ReadLine();

                if (respuesta.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Ingrese el nombre a buscar: ");
                    string nombreBusqueda = Console.ReadLine();
                    BuscarPersonaPorNombreYExaminar(nombreBusqueda);
                }
                else
                {
                    Console.WriteLine("Regresando al menú anterior.");
                }
            }
            else if (personasEncontradas.Count == 1)
            {
                MostrarDatosPersona(personasEncontradas[0]);
            }
            else
            {
                Console.WriteLine("Se encontraron varias personas con ese nombre. Seleccione la persona por ID:");
                foreach (var persona in personasEncontradas)
                {
                    Console.WriteLine($"ID: {persona.Id}, Nombre: {persona.Nombre}");
                }

                Console.Write("Ingrese el ID de la persona a examinar: ");
                string idSeleccionado = Console.ReadLine();

                if (personasEncontradas.Exists(p => p.Id == idSeleccionado))
                {
                    ExaminarPersona(idSeleccionado);
                }
                else
                {
                    Console.WriteLine("ID no válido. Regresando al menú anterior.");
                }
            }
        }

        public void RegistrarMascota(string nombre, int edad, Especie especie, Temperamento temperamento)
        {
            Mascota mascota;

            switch (especie)
            {
                case Especie.Perro:
                    mascota = new Perro(nombre, edad, temperamento);
                    break;
                case Especie.Gato:
                    mascota = new Gato(nombre, edad, temperamento);
                    break;
                case Especie.Capibara:
                    mascota = new Capibara(nombre, edad);
                    break;
                default:
                    Console.WriteLine("Especie no válida.");
                    return;
            }

            mascotas.Add(mascota);

            Console.WriteLine($"Se ha registrado a la mascota {mascota.Nombre} en el centro.");

            Console.Write("¿Desea asignar un dueño a la mascota? (Sí/No): ");
            string respuestaAsignarDueño = Console.ReadLine();

            if (respuestaAsignarDueño.Equals("Sí", StringComparison.OrdinalIgnoreCase))
            {
                AsignarDueñoAMascota(mascota);
            }
            else
            {
                Console.WriteLine("Regresando al menú anterior.");
            }
        }

        private void AsignarDueñoAMascota(Mascota mascota)
        {
            Console.Write("¿Conoce el ID del dueño? (Sí/No): ");
            string respuestaConoceId = Console.ReadLine();

            if (respuestaConoceId.Equals("Sí", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Ingrese el ID del dueño: ");
                string idDueño = Console.ReadLine();
                Persona dueño = personas.Find(p => p.Id == idDueño);

                if (dueño != null)
                {
                    mascota.CambiarDueño(dueño);
                }
                else
                {
                    Console.WriteLine("No se encontró ninguna persona con ese ID.");
                    Console.Write("¿Desea buscar al dueño por nombre? (Sí/No): ");
                    string respuestaBuscarDueño = Console.ReadLine();

                    if (respuestaBuscarDueño.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                    {
                        BuscarDueñoPorNombreYAsignar(mascota);
                    }
                    else
                    {
                        Console.WriteLine("Regresando al menú anterior.");
                    }
                }
            }
            else
            {
                BuscarDueñoPorNombreYAsignar(mascota);
            }
        }

        private void BuscarDueñoPorNombreYAsignar(Mascota mascota)
        {
            Console.Write("Ingrese el nombre del dueño: ");
            string nombreDueño = Console.ReadLine();

            List<Persona> dueñosEncontrados = personas.FindAll(p => p.Nombre.Contains(nombreDueño));

            if (dueñosEncontrados.Count == 0)
            {
                Console.WriteLine("No se encontraron personas con ese nombre.");
                Console.Write("¿Desea intentarlo de nuevo? (Sí/No): ");
                string respuestaIntentarNuevamente = Console.ReadLine();

                if (respuestaIntentarNuevamente.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                {
                    BuscarDueñoPorNombreYAsignar(mascota);
                }
                else
                {
                    Console.WriteLine("Regresando al menú anterior.");
                }
            }
            else if (dueñosEncontrados.Count == 1)
            {
                Persona dueñoEncontrado = dueñosEncontrados[0];
                mascota.CambiarDueño(dueñoEncontrado);
            }
            else
            {
                Console.WriteLine("Se encontraron varias personas con ese nombre. Seleccione al dueño por ID:");
                foreach (var dueño in dueñosEncontrados)
                {
                    Console.WriteLine($"ID: {dueño.Id}, Nombre: {dueño.Nombre}");
                }

                Console.Write("Ingrese el ID del dueño seleccionado: ");
                string idDueñoSeleccionado = Console.ReadLine();

                if (dueñosEncontrados.Exists(d => d.Id == idDueñoSeleccionado))
                {
                    Persona dueñoSeleccionado = dueñosEncontrados.Find(d => d.Id == idDueñoSeleccionado);
                    mascota.CambiarDueño(dueñoSeleccionado);
                }
                else
                {
                    Console.WriteLine("ID no válido. Regresando al menú anterior.");
                }
            }
        }
        public void MostrarMascotasRegistradas()
        {
            Console.WriteLine("Mascotas registradas en el centro:");
            foreach (var mascota in mascotas)
            {
                Console.WriteLine($"ID: {mascota.Id}, Nombre: {mascota.Nombre}, Especie: {mascota.ObtenerNombreEspecie()}, Dueño: {mascota.Dueño?.Nombre ?? "Sin dueño"}");
            }
        }

        public void BuscarMascotasPorEspecie(Especie especie)
        {
            List<Mascota> mascotasPorEspecie = mascotas.FindAll(m => m.Especie == especie);

            if (mascotasPorEspecie.Count > 0)
            {
                Console.WriteLine($"Mascotas de la especie {especie}:");
                foreach (var mascota in mascotasPorEspecie)
                {
                    Console.WriteLine($"ID: {mascota.Id}, Nombre: {mascota.Nombre}, Dueño: {mascota.Dueño?.Nombre ?? "Sin dueño"}");
                }
            }
            else
            {
                Console.WriteLine($"No hay mascotas de la especie {especie} registradas en el centro.");
            }
        }

        public void BuscarMascotasPorNombre(string nombre)
        {
            List<Mascota> mascotasPorNombre = mascotas.FindAll(m => m.Nombre.Contains(nombre));

            if (mascotasPorNombre.Count > 0)
            {
                Console.WriteLine($"Mascotas con el nombre que contiene '{nombre}':");
                foreach (var mascota in mascotasPorNombre)
                {
                    Console.WriteLine($"ID: {mascota.Id}, Nombre: {mascota.Nombre}, Especie: {mascota.ObtenerNombreEspecie()}, Dueño: {mascota.Dueño?.Nombre ?? "Sin dueño"}");
                }
            }
            else
            {
                Console.WriteLine($"No hay mascotas con el nombre que contiene '{nombre}' registradas en el centro.");
            }
        }

        public void ExaminarMascota(string id)
        {
            Mascota mascota = mascotas.Find(m => m.Id == id);

            if (mascota == null)
            {
                Console.WriteLine("No se encontró ninguna mascota con ese ID.");
                Console.Write("¿Desea buscar por nombre? (Sí/No): ");
                string respuesta = Console.ReadLine();

                if (respuesta.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Ingrese el nombre a buscar: ");
                    string nombreBusqueda = Console.ReadLine();
                    BuscarMascotasPorNombreYExaminar(nombreBusqueda);
                }
                else
                {
                    Console.WriteLine("Regresando al menú anterior.");
                }
            }
            else
            {
                MostrarDatosMascota(mascota);
            }
        }

        private void BuscarMascotasPorNombreYExaminar(string nombre)
        {
            List<Mascota> mascotasEncontradas = mascotas.FindAll(m => m.Nombre.Contains(nombre));

            if (mascotasEncontradas.Count == 0)
            {
                Console.WriteLine("No se encontraron mascotas con ese nombre.");
                Console.Write("¿Desea intentarlo de nuevo? (Sí/No): ");
                string respuesta = Console.ReadLine();

                if (respuesta.Equals("Sí", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Ingrese el nombre a buscar: ");
                    string nombreBusqueda = Console.ReadLine();
                    BuscarMascotasPorNombreYExaminar(nombreBusqueda);
                }
                else
                {
                    Console.WriteLine("Regresando al menú anterior.");
                }
            }
            else if (mascotasEncontradas.Count == 1)
            {
                MostrarDatosMascota(mascotasEncontradas[0]);
            }
            else
            {
                Console.WriteLine("Se encontraron varias mascotas con ese nombre. Seleccione la mascota por ID:");
                foreach (var mascotaEncontrada in mascotasEncontradas)
                {
                    Console.WriteLine($"ID: {mascotaEncontrada.Id}, Nombre: {mascotaEncontrada.Nombre}, Especie: {mascotaEncontrada.ObtenerNombreEspecie()}, Dueño: {mascotaEncontrada.Dueño?.Nombre ?? "Sin dueño"}");
                }

                Console.Write("Ingrese el ID de la mascota a examinar: ");
                string idSeleccionado = Console.ReadLine();

                if (mascotasEncontradas.Exists(m => m.Id == idSeleccionado))
                {
                    ExaminarMascota(idSeleccionado);
                }
                else
                {
                    Console.WriteLine("ID no válido. Regresando al menú anterior.");
                }
            }
        }

        public void MostrarDatosPersona(Persona persona)
        {
            Console.WriteLine($"Datos de la persona {persona.Nombre}:");
            Console.WriteLine($"ID: {persona.Id}, Nombre: {persona.Nombre}");
            if (persona.Mascotas.Count > 0)
            {
                Console.WriteLine("Mascotas de la persona:");
                foreach (var mascota in persona.Mascotas)
                {
                    Console.WriteLine($"ID: {mascota.Id}, Nombre: {mascota.Nombre}, Especie: {mascota.ObtenerNombreEspecie()}");
                }
            }
            else
            {
                Console.WriteLine("Esta persona no tiene mascotas registradas.");
            }
        }

        public void MostrarDatosMascota(Mascota mascota)
        {
            Console.WriteLine($"Datos de la mascota {mascota.Nombre}:");
            Console.WriteLine($"ID: {mascota.Id}, Nombre: {mascota.Nombre}, Especie: {mascota.ObtenerNombreEspecie()}, Dueño: {mascota.Dueño?.Nombre ?? "Sin dueño"}");
        }

        public void Iniciar()
        {
            int opcion;

            do
            {
                Console.WriteLine("\nBienvenido al Centro de Adopciones y Bienestar Animal");
                Console.WriteLine("1 - Administración de Personas");
                Console.WriteLine("2 - Administración de Mascotas");
                Console.WriteLine("3 - Salir");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                switch (opcion)
                {
                    case 1:
                        AdministrarPersonas();
                        break;
                    case 2:
                        AdministrarMascotas();
                        break;
                    case 3:
                        Console.WriteLine("Saliendo del programa. ¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }

            } while (opcion != 3);
        }

        private void AdministrarPersonas()
        {
            int opcion;

            do
            {
                Console.WriteLine("\nAdministración de Personas");
                Console.WriteLine("1 - Registrar Persona");
                Console.WriteLine("2 - Mostrar Personas");
                Console.WriteLine("3 - Examinar Persona");
                Console.WriteLine("4 - Volver al menú anterior");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese el nombre de la persona: ");
                        string nombrePersona = Console.ReadLine();
                        RegistrarPersona(nombrePersona);
                        break;
                    case 2:
                        MostrarPersonas();
                        break;
                    case 3:
                        Console.Write("Ingrese el ID de la persona a examinar: ");
                        string idPersonaExaminar = Console.ReadLine();
                        ExaminarPersona(idPersonaExaminar);
                        break;
                    case 4:
                        Console.WriteLine("Regresando al menú anterior.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }

            } while (opcion != 4);
        }

        private void AdministrarMascotas()
        {
            int opcion;

            do
            {
                Console.WriteLine("\nAdministración de Mascotas");
                Console.WriteLine("1 - Registrar Mascota");
                Console.WriteLine("2 - Mostrar Mascotas Registradas");
                Console.WriteLine("3 - Buscar Mascotas por Especie");
                Console.WriteLine("4 - Buscar Mascotas por Nombre");
                Console.WriteLine("5 - Examinar Mascota");
                Console.WriteLine("6 - Volver al menú anterior");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese el nombre de la mascota: ");
                        string nombreMascota = Console.ReadLine();
                        Console.Write("Ingrese la edad de la mascota: ");
                        if (!int.TryParse(Console.ReadLine(), out int edadMascota))
                        {
                            Console.WriteLine("Edad no válida. Regresando al menú anterior.");
                            break;
                        }
                        Console.WriteLine("Seleccione la especie de la mascota:");
                        foreach (Especie especie in Enum.GetValues(typeof(Especie)))
                        {
                            Console.WriteLine($"{(int)especie} - {especie}");
                        }
                        Console.Write("Elija una opción: ");
                        if (!Enum.TryParse(Console.ReadLine(), out Especie especieMascota))
                        {
                            Console.WriteLine("Opción no válida. Regresando al menú anterior.");
                            break;
                        }
                        Console.WriteLine("Seleccione el temperamento de la mascota:");
                        foreach (Temperamento temperamento in Enum.GetValues(typeof(Temperamento)))
                        {
                            Console.WriteLine($"{(int)temperamento} - {temperamento}");
                        }
                        Console.Write("Elija una opción: ");
                        if (!Enum.TryParse(Console.ReadLine(), out Temperamento temperamentoMascota))
                        {
                            Console.WriteLine("Opción no válida. Regresando al menú anterior.");
                            break;
                        }
                        RegistrarMascota(nombreMascota, edadMascota, especieMascota, temperamentoMascota);
                        break;
                    case 2:
                        MostrarMascotasRegistradas();
                        break;
                    case 3:
                        Console.WriteLine("Seleccione la especie de mascotas a buscar:");
                        foreach (Especie especie in Enum.GetValues(typeof(Especie)))
                        {
                            Console.WriteLine($"{(int)especie} - {especie}");
                        }
                        Console.Write("Elija una opción: ");
                        if (!Enum.TryParse(Console.ReadLine(), out Especie especieBuscar))
                        {
                            Console.WriteLine("Opción no válida. Regresando al menú anterior.");
                            break;
                        }
                        BuscarMascotasPorEspecie(especieBuscar);
                        break;
                    case 4:
                        Console.Write("Ingrese el nombre de la mascota a buscar: ");
                        string nombreMascotaBuscar = Console.ReadLine();
                        BuscarMascotasPorNombre(nombreMascotaBuscar);
                        break;
                    case 5:
                        Console.Write("Ingrese el ID de la mascota a examinar: ");
                        string idMascotaExaminar = Console.ReadLine();
                        ExaminarMascota(idMascotaExaminar);
                        break;
                    case 6:
                        Console.WriteLine("Regresando al menú anterior.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                    }

            } while (opcion != 6);
        }
    }

    class Program
    {
        static void Main()
        {
            CentroMascotas centroMascotas = new CentroMascotas();
            centroMascotas.Iniciar();
        }
    }
}

   




