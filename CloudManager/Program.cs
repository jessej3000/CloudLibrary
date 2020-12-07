using System;
using CloudLibrary;

namespace CloudManager
{
    /// <summary>
    /// Main console program that consumes the Cloud Library
    /// </summary>
    class MainClass
    {
        public static void Main(string[] args)
        {
            var command = string.Empty;

            // Initialize Infra Controller
            string basePath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            var infraControl = new InfraControl(basePath);

            while (command != "exit")
            {
                Console.Clear();
                Console.WriteLine("Enter your choice:");
                Console.WriteLine("1 - Provider");
                Console.WriteLine("2 - Environments");
                Console.WriteLine("3 - Virtual Machines");
                Console.WriteLine("4 - Database");

                command = Console.ReadLine();
                switch (command)
                {
                    case "1": // Provider
                        {
                            command = ProviderSelection();
                            ProviderAction(infraControl, command);
                        };
                        break; // Environments
                    case "2":
                        {
                            command = EnvironmentSelection();
                            EnvironmentAction(infraControl, command);
                        };
                        break; // Virtual Machines
                    case "3":
                        {
                            command = VirtualMachineSelection();
                            VirtualMachineAction(infraControl, command);
                        };
                        break;
                    case "4": // Database
                        {
                            command = DatabaseSelection();
                            DatabaseAction(infraControl, command);
                        };
                        break;
                    default:
                        Console.WriteLine("Not a command");
                        break;

                }
                command = string.Empty;
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            Console.WriteLine("Bye!");
        }

        // Which functions
        private static string WhichDatabase()
        {
            var command = string.Empty;
            while ((command.ToLower() != "m") &&
                    (command.ToLower() != "s") &&
                    (command.ToLower() != "x"))
            {
                Console.Clear();
                Console.WriteLine("Select Database");
                Console.WriteLine("M - MySQL");
                Console.WriteLine("S - SQL");
                Console.WriteLine("X - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }
        private static string WhichEnvironment()
        {
            var command = string.Empty;
            while ((command.ToLower() != "u") &&
                    (command.ToLower() != "s") &&
                    (command.ToLower() != "p") &&
                    (command.ToLower() != "x"))
            {
                Console.Clear();
                Console.WriteLine("Select Environment");
                Console.WriteLine("U - UAT");
                Console.WriteLine("S - Staging");
                Console.WriteLine("P - Production");
                Console.WriteLine("X - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }

        // Selection functions
        private static string DatabaseSelection()
        {
            var command = string.Empty;
            while ((command != "1") &&
                (command != "2") &&
                (command != "3") &&
                (command != "4"))
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("1 - Add Database");
                Console.WriteLine("2 - List all Database");
                Console.WriteLine("3 - Remove Database");
                Console.WriteLine("4 - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }
        private static string VirtualMachineSelection()
        {
            var command = string.Empty;
            while ((command != "1") &&
                (command != "2") &&
                (command != "3") &&
                (command != "4"))
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("1 - Add Virtual Machine");
                Console.WriteLine("2 - List all Virtual Machines");
                Console.WriteLine("3 - Remove Virtual Machine");
                Console.WriteLine("4 - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }
        private static string EnvironmentSelection()
        {
            var command = string.Empty;
            while ((command != "1") &&
                (command != "2") &&
                (command != "3") &&
                (command != "4"))
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("1 - Create Environment");
                Console.WriteLine("2 - List all Environments");
                Console.WriteLine("3 - Remove Environment");
                Console.WriteLine("4 - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }
        private static string ProviderSelection()
        {
            var command = string.Empty;
            while ((command != "1") &&
                (command != "2") &&
                (command != "3") &&
                (command != "4"))
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("1 - Create Provider");
                Console.WriteLine("2 - List all Providers");
                Console.WriteLine("3 - Remove Provider");
                Console.WriteLine("4 - Cancel");

                command = Console.ReadLine();
                Console.Clear();
            }
            return command;
        }

        // Action functions
        private static void DatabaseAction(InfraControl IC, string action)
        {
            var input = string.Empty;
            var provider = string.Empty;
            InfraGlobal.EnvironmentTypes environment = InfraGlobal.EnvironmentTypes._;
            switch (action)
            {
                case "1": // Add database
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then find envronment
                            // Ask for environment type
                            environment = SelectEnvironment();
                            if (environment == 0) return; // Cancelled
                            if (environment > 0)
                            {
                                if (IC.EnvironmentExist(provider, environment))
                                //if (IC.CloudServices.Find(x => x.Name == provider)
                                //    .Environments.Exists(y => y.Type == environment))
                                {
                                    var ID = GetEnvironmentID();
                                    if (IC.EnvironmentExistByID(provider, environment, ID))
                                    {
                                        InfraGlobal.DatabaseTypes database = SelectDatabase();
                                        if (database == 0) return; // Cancelled
                                        if (database > 0)
                                        {
                                            // Add database
                                            IC.CreateDatabase(provider, environment, ID, database);
                                        }
                                    }
                                    return;
                                }
                                Console.WriteLine("Environment {0:G} does not exist.", environment);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "2": // List all database
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {
                            InfraGlobal.EnvironmentTypes environ = SelectEnvironment();
                            if (IC.EnvironmentExist(provider, environ))
                            {
                                var ID = GetEnvironmentID();
                                if (IC.EnvironmentExistByID(provider, environ, ID))
                                {
                                    IC.ShowDatabases(provider, environ, ID);
                                    return;
                                }
                            }
                            Console.WriteLine("Environment does not exist!");
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist!", provider);
                    };
                    break;
                case "3": // Remove database
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then find envronment
                            // Ask for environment type
                            environment = SelectEnvironment();
                            if (environment == 0) return; // Cancelled
                            if (environment > 0)
                            {
                                if (IC.EnvironmentExist(provider, environment))
                                //if (IC.CloudServices.Find(x => x.Name == provider)
                                //    .Environments.Exists(y => y.Type == environment))
                                {
                                    var ID = GetEnvironmentID();
                                    if (IC.EnvironmentExistByID(provider, environment, ID))
                                    {
                                        InfraGlobal.DatabaseTypes database = SelectDatabase();
                                        if (database == 0) return; // Cancelled
                                        if (database > 0)
                                        {
                                            // Delete database
                                            IC.DeleteDatabase(provider, environment, ID, database);
                                        }
                                    }
                                    return;
                                }
                                Console.WriteLine("Environment {0:G} does not exist.", environment);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "4":
                    {
                        Console.WriteLine("Action cancelled.");
                    };
                    break;
                default: return;
            }
        }
        private static void VirtualMachineAction(InfraControl IC, string action)
        {
            var input = string.Empty;
            var provider = string.Empty;
            InfraGlobal.EnvironmentTypes environment = InfraGlobal.EnvironmentTypes._;
            switch (action)
            {
                case "1": // Add virtual machine
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then find envronment
                            // Ask for environment type
                            environment = SelectEnvironment();
                            if (environment == 0) return; // Cancelled
                            if (environment > 0)
                            {
                                if (IC.EnvironmentExist(provider, environment))
                                //if (IC.CloudServices.Find(x => x.Name == provider)
                                //    .Environments.Exists(y => y.Type == environment))
                                {
                                    // Ask for VM details
                                    var ID = GetEnvironmentID();
                                    if (IC.EnvironmentExistByID(provider, environment, ID))
                                    {
                                        var os = SelectOS();
                                        if (os == "x") return;
                                        var cpu = SetValue();
                                        if (cpu < 1) return;
                                        var memory = SetValue();
                                        if (memory < 1) return;

                                        InfraGlobal.OSPlatform finalOs = InfraGlobal.OSPlatform._;
                                        switch (os)
                                        {
                                            case "l":
                                                finalOs = InfraGlobal.OSPlatform.Linux;
                                                break;
                                            case "w":
                                                finalOs = InfraGlobal.OSPlatform.Windows;
                                                break;
                                        }

                                        // Create virtual machine
                                        IC.CloudServices.Find(x => x.Name == provider)
                                            .Environments.Find(y => y.Type == environment)
                                            .AddVirtualMachine(finalOs, cpu, memory);

                                        return;
                                    }
                                    Console.WriteLine("Environment ID {0:G} does not exist.", ID);
                                }
                                Console.WriteLine("Environment {0:G} does not exist.", environment);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "2": // List all virtual machines
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {
                            InfraGlobal.EnvironmentTypes environ = SelectEnvironment();
                            if (IC.EnvironmentExist(provider, environ))
                            {
                                var ID = GetEnvironmentID();
                                if (IC.EnvironmentExistByID(provider, environ, ID))
                                {
                                    IC.ShowVirtualMachines(provider, environ, ID);
                                }
                                return;
                            }
                            Console.WriteLine("Environment does not exist!");
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist!", provider);
                    };
                    break;
                case "3": // Remove virtual machine
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then find envronment
                            // Ask for environment type
                            environment = SelectEnvironment();
                            if (environment == 0) return; // Cancelled
                            if (environment > 0)
                            {
                                if (IC.EnvironmentExist(provider, environment))
                                //if (IC.CloudServices.Find(x => x.Name == provider)
                                //    .Environments.Exists(y => y.Type == environment))
                                {
                                    // Ask for VM details
                                    var ID = GetEnvironmentID();
                                    if (IC.EnvironmentExistByID(provider, environment, ID))
                                    {
                                        var vmID = GetVMID();
                                        if (IC.VirtualMachineExist(provider, environment, ID, vmID))
                                        {
                                            IC.DeleteVirtualMachine(provider, environment, ID, vmID);
                                        }
                                        Console.WriteLine("VM ID {0:G} does not exist.", vmID);
                                        return;
                                    }
                                }
                                Console.WriteLine("Environment {0:G} does not exist.", environment);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "4":
                    {
                        Console.WriteLine("Action cancelled.");
                    };
                    break;
                default: return;
            }
        }
        private static void EnvironmentAction(InfraControl IC, string action)
        {
            var input = string.Empty;
            var provider = string.Empty;
            switch (action)
            {
                case "1": // Create environmont
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then create environment
                            // Ask for environment type
                            input = WhichEnvironment();
                            if ((input.ToLower() == "u") ||
                                (input.ToLower() == "s") ||
                                (input.ToLower() == "p"))
                            {
                                CloudLibrary.InfraGlobal.EnvironmentTypes Env = 0;
                                switch (input.ToLower())
                                {
                                    case "u": Env = CloudLibrary.InfraGlobal.EnvironmentTypes.UAT;
                                        break;
                                    case "s": Env = CloudLibrary.InfraGlobal.EnvironmentTypes.Staging;
                                        break;
                                    case "p": Env = CloudLibrary.InfraGlobal.EnvironmentTypes.Production;
                                        break;
                                }
                                IC.CloudServices.Find(x => x.Name == provider).CreateEnvironment(Env);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "2": // List all environments
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {
                            IC.ShowEnvironments(provider);
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist!", provider);
                    };
                    break;
                case "3": // Remove environment
                    {
                        provider = GetProviderName();
                        if (IC.ProviderExist(provider))
                        {// Provider exist then create environment
                            // Ask for environment type
                            input = WhichEnvironment();
                            if ((input.ToLower() == "u") ||
                                (input.ToLower() == "s") ||
                                (input.ToLower() == "p"))
                            {
                                CloudLibrary.InfraGlobal.EnvironmentTypes Env = 0;
                                switch (input.ToLower())
                                {
                                    case "u":
                                        Env = CloudLibrary.InfraGlobal.EnvironmentTypes.UAT;
                                        break;
                                    case "s":
                                        Env = CloudLibrary.InfraGlobal.EnvironmentTypes.Staging;
                                        break;
                                    case "p":
                                        Env = CloudLibrary.InfraGlobal.EnvironmentTypes.Production;
                                        break;
                                }
                                IC.DeleteEnvironment(provider, Env);
                            }
                            return;
                        }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "4":
                    {
                        Console.WriteLine("Action cancelled.");
                    };
                    break;
                default: return;
            }
        }
        private static void ProviderAction(InfraControl IC, string action)
        {
            var input = string.Empty;
            switch (action)
            {
                case "1": // Create provider
                    {
                        Console.WriteLine("Enter provider name:");
                        input = Console.ReadLine();
                        IC.CreateProvider(input);
                    };
                    break;
                case "2": // List all providers
                    {
                        IC.ShowProviders();
                    };
                    break;
                case "3":
                    {
                        var provider = GetProviderName();
                        if (IC.ProviderExist(provider)) {
                            IC.DeleteProvider(provider);
                           return;
                         }
                        Console.WriteLine("Provider {0:G} does not exist.", provider);
                    };
                    break;
                case "4":
                    {
                        Console.WriteLine("Action cancelled.");
                    };
                    break;
                default: return;
            }
        }


        // Helper functions
        private static InfraGlobal.DatabaseTypes SelectDatabase()
        {
            var environment = WhichDatabase();

            InfraGlobal.DatabaseTypes result = InfraGlobal.DatabaseTypes._;
            switch (environment)
            {
                case "m":
                    result = InfraGlobal.DatabaseTypes.MySQL;
                    break;
                case "s":
                    result = InfraGlobal.DatabaseTypes.SQL;
                    break;
            }
            return result;
        }
        private static InfraGlobal.EnvironmentTypes SelectEnvironment()
        {
            var environment = WhichEnvironment();

            InfraGlobal.EnvironmentTypes result = InfraGlobal.EnvironmentTypes._;
            switch(environment)
            {
                case "u": result = InfraGlobal.EnvironmentTypes.UAT;
                    break;
                case "s": result = InfraGlobal.EnvironmentTypes.Staging;
                    break;
                case "p": result = InfraGlobal.EnvironmentTypes.Production;
                    break;
            }
            return result;
        }
        private static string SelectOS()
        {
            var os = string.Empty;
            while ((os.ToLower() != "x") &&
                (os.ToLower() != "l") &&
                (os.ToLower() != "w"))
            {
                Console.WriteLine("Select Operating System:");
                Console.WriteLine("L - Linux");
                Console.WriteLine("W - Windows");
                Console.WriteLine("X - Cancel");
                os = Console.ReadLine();
            }
            return os;
        }
        private static float SetValue()
        {
            float cpu = 0;
            var input = string.Empty;
            while (!(float.TryParse(input, out cpu)))
            {
                Console.WriteLine("Enter a valid value (ex. 2.3, 1.4) or X to cancel:");
                input = Console.ReadLine();
            }
            return cpu;
        }
        private static string GetProviderName()
        {
            Console.WriteLine("Enter provider name:");
            var provider = Console.ReadLine();
            return provider;
        }
        private static string GetEnvironmentID()
        {
            Console.WriteLine("Enter environment ID:");
            var ID = Console.ReadLine();
            return ID;
        }
        private static string GetVMID()
        {
            Console.WriteLine("Enter VM ID:");
            var ID = Console.ReadLine();
            return ID;
        }

    }
}
