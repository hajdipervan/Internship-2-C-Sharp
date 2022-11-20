using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace C_homework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var choice = 8;
            var playersDictionary = new Dictionary<string, (string position, int rating)>
            {
                {"Luka Modrić", ("MF", 88) },
                {"Marcelo Brozović", ("MF", 86) },
                {"Mateo Kovačić", ("MF", 84)},
                {"Ivan Perišić", ("MF", 84) },
                {"Andrej Kramarić", ("FW", 82) },
                {"Ivan Rakitić", ("MF", 82) },
                {"Joško Gvardiol", ("DF", 81) },
                {"Mario Pašalić", ("MF", 81) },
                {"Lovro Majer", ("MF", 80) },
                {"Dominik Livaković", ("GK", 80) },
                {"Ante Rebić", ("FW", 80) },
                {"Josip Brekalo", ("MF", 79) },
                {"Borna Sosa", ("DF", 78)},
                {"Nikola Vlašić", ("MF", 78) },
                {"Duje Ćaleta Car", ("DF", 78) },
                {"Dejan Lovren", ("DF", 78) },
                {"Mislav Oršić", ("FW", 77) },
                {"Marko Livaja", ("FW", 77) },
                {"Domagoj Vida", ("DF", 76) },
                {"Ante Budimir", ("FW", 76) },
            };
            
            var scorers = new List<string>();
            var scorersPlusGoals = new Dictionary<string, int>();
            var allResults = new Dictionary<(string, string), (int, int)>
            {
                {("Hrvatska","Maroko"), (0,0) },
                {("Belgija","Kanada"), (0,0) },
                {("Belgija", "Maroko"), (0,0) },
                {("Hrvatska", "Kanada"), (0,0) },
                {("Hrvatska", "Belgija"), (0,0) },
                {("Kanada", "Maroko"), (0,0) }
            };

            var tableGroup = new Dictionary<string, (int, int)>
            {
                {"Hrvatska", (0, 0)},
                {"Kanada", (0,0) },
                {"Belgija", (0,0) },
                {"Maroko",  (0,0) }
            };
            scorers.Clear();
            while (choice != 1 || choice != 2 || choice != 3 || choice != 4 || choice != 5)
            {
                Console.WriteLine("1 - Odradi trening \n2 - Odigraj utakmicu \n3 - Statistika \n4 - Kontrola igraca \n0 - Izlaz iz aplikacije");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice == 1)
                    {
                        //var playersDictionaryNew = new Dictionary<string, (string postition, int rating)> { };
                        PlayersNew(playersDictionary);
                        Console.WriteLine("Uspjesno odraden trening!");
                        choice = 8;

                    }
                    else if (choice == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("Odabrana opcija: odigraj utakmicu");
                        Random rnd = new Random();
                        for (int j = 0; j < 3; j++) //nabavljam rezultate mene protiv ostale 3 ekipe, mijenjam sebi rating
                        {
                            int ourTeam = 0;
                            int opponents = 0;
                            var bestPlayers = new List<string>();
                            bestPlayers = BestPlayersByRating(playersDictionary);
                            //0-pobjeda, 1-poraz, 2-izjednaceno
                            //stavljeno maks 10 golova
                            int winOrLose = rnd.Next(0, 3);
                            if (winOrLose == 0)
                            {
                                ourTeam = rnd.Next(1, 11);
                                opponents = rnd.Next(0, ourTeam);
                                tableGroup["Hrvatska"] = (tableGroup["Hrvatska"].Item1 + 3, tableGroup["Hrvatska"].Item2);
                            }
                            else if (winOrLose == 1)
                            {
                                ourTeam = rnd.Next(0, 10);
                                opponents = rnd.Next(ourTeam, 11);
                                if (j == 0)
                                    tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 3, tableGroup["Maroko"].Item2);
                                else if (j == 1)
                                    tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 3, tableGroup["Kanada"].Item2);
                                else
                                    tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 3, tableGroup["Belgija"].Item2);

                            }
                            else
                            {
                                ourTeam = rnd.Next(0, 11);
                                opponents = ourTeam;
                                tableGroup["Hrvatska"] = (tableGroup["Hrvatska"].Item1 + 1, tableGroup["Hrvatska"].Item2);
                                if (j == 0)
                                    tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 1, tableGroup["Maroko"].Item2);
                                else if (j == 1)
                                    tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 1, tableGroup["Kanada"].Item2);
                                else
                                    tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 1, tableGroup["Belgija"].Item2);
                            }

                            for (int i = 0; i < ourTeam; i++)
                            {
                                int scorersNumber = rnd.Next(0, 11);
                                scorers.Add(bestPlayers[scorersNumber]);
                            }

                            ChangeRatings(scorers, bestPlayers, playersDictionary, winOrLose);
                            ChangeTableResultsCroatia(allResults, ourTeam, opponents, j);

                        }

                        for (int j = 0; j < 3; j++) //biram random rezultate preostale 3 ekipe (Maroko, Belgija, Kanada) (kad odigraju medusobno tocno odredenin redoslijedon)
                        {
                            int first = rnd.Next(0, 11);
                            int second = rnd.Next(0, 11);
                            ChangeTableResultsOthers(allResults, first, second, j, tableGroup);
                        }
                        Console.WriteLine("Utakmica je uspjesno odigrana");
                        choice = 8;
                    }
                    else if (choice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Odabrana je opcija statistika");
                        var choice3Taskn = -1;
                        while (choice3Taskn < 0 || choice3Taskn > 1)
                        {
                            Console.WriteLine("Odaberite: \n1 - Ispis svih igraca \n0 - Povratak na pocetni izbornik");
                            try
                            {
                                choice3Taskn = int.Parse(Console.ReadLine());
                                if (choice3Taskn == 1)
                                {
                                    var choice3Task = -1;
                                    Console.Clear();
                                    while (choice3Task < 0 || choice3Task > 10)
                                    {
                                        Console.WriteLine("Odaberite 1 do 11");
                                        Console.WriteLine("1 - Ispis svih igraca onako kako su spremljeni \n2 - Ispis po ratingu uzlazno \n3 - Ispis po ratingu silazno \n4 - Ispis igraca po imenu i prezimenu" +
                                             "\n5 - Ispis igraca po ratingu \n6 - Ispis igraca po poziciji \n7 - Ispis trenutnih prvih 11 igraca \n8 - Ispis strijelaca i koliko golova imaju \n9 - Ispis svih rezultata ekipe" +
                                             "\n10 - Ispis rezultata svih ekipa \n11 - Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika \n0 - Povratak na pocetni izbornik");
                                        try
                                        {
                                            choice3Task = int.Parse(Console.ReadLine());
                                            if (choice3Task == 1)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis svih igraca onako kako su spremljeni");
                                                foreach (var pl in playersDictionary)
                                                {
                                                    Console.WriteLine(pl.Key, pl.Value.position, pl.Value.rating);
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 2)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis po ratingu uzlazno");
                                                foreach (KeyValuePair<string, (string, int)> pl in playersDictionary.OrderBy(key => key.Value.rating))
                                                {
                                                    Console.WriteLine(pl.Key, pl.Value.Item1, pl.Value.Item2);
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 3)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis po ratingu silazno");
                                                foreach (KeyValuePair<string, (string, int)> pl in playersDictionary.OrderByDescending(key => key.Value.rating))
                                                {
                                                    Console.WriteLine(pl.Key, pl.Value.Item1, pl.Value.Item2);
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 4)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis igraca po imenu i prezimenu");
                                                string insert = "";
                                                Console.WriteLine("Unesite rating za ispis: ");
                                                insert = Console.ReadLine();
                                                foreach (var pl in playersDictionary)
                                                {
                                                    if (insert == pl.Key)
                                                    {
                                                        Console.WriteLine(pl.Key, pl.Value.position, pl.Value.rating);
                                                    }
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 5)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis igraca po ratingu");
                                                int rat = -1;
                                                Console.WriteLine("Unesite rating za ispis: ");
                                                while (rat == -1)
                                                {
                                                    try
                                                    {
                                                        rat = int.Parse(Console.ReadLine());
                                                        foreach (var pl in playersDictionary)
                                                        {
                                                            if (rat == pl.Value.rating)
                                                            {
                                                                Console.WriteLine(pl.Key, pl.Value.position, pl.Value.rating);
                                                            }
                                                        }
                                                        choice = 8;
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("Nije unesen integer. Ponovite unos!");
                                                        rat = -1;
                                                    }
                                                }
                                            }
                                            else if (choice3Task == 6)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis igraca po poziciji");
                                                string pos = "";
                                                Console.WriteLine("Unesite position za ispis: ");
                                                pos = Console.ReadLine();
                                                foreach (var pl in playersDictionary)
                                                {
                                                    if (pos == pl.Value.position)
                                                    {
                                                        Console.WriteLine(pl.Key, pl.Value.position, pl.Value.rating);
                                                    }
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 7)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis trenutnih prvih 11 igraca");
                                                List<string> bestPlayers = new List<string>();
                                                bestPlayers = BestPlayersByRating(playersDictionary);
                                                foreach (var pl in bestPlayers)
                                                    Console.WriteLine(pl);
                                                choice = 8;
                                            }
                                            else if (choice3Task == 8)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis strijelaca i koliko golova imaju");
                                                foreach (var pl in playersDictionary)
                                                {
                                                    int counter = 0;
                                                    foreach (var scorer in scorers)
                                                        if (pl.Key == scorer)
                                                            counter++;
                                                    if (counter != 0)
                                                        scorersPlusGoals.Add(pl.Key, counter);
                                                }
                                                foreach (var scorer in scorersPlusGoals)
                                                    Console.WriteLine($"Strijelac: {scorer.Key}, golovi: {scorer.Value} ");
                                                choice = 8;
                                            }
                                            else if (choice3Task == 9)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis svih rezultata ekipe");
                                                foreach (var games in allResults)
                                                {
                                                    if (games.Key.Item1 == "Hrvatska")
                                                        Console.WriteLine($"{games.Key.Item1}\t{games.Value.Item1}-{games.Value.Item2}\t{games.Key.Item2}");
                                                    else if (games.Key.Item2 == "Hrvatska")
                                                        Console.WriteLine($"{games.Key.Item1}\t{games.Value.Item1}-{games.Value.Item2}\t{games.Key.Item2}");
                                                }
                                                choice = 8;

                                            }
                                            else if (choice3Task == 10)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis rezultata svih ekipa");
                                                foreach (var games in allResults)
                                                    Console.WriteLine($"{games.Key.Item1}\t{games.Value.Item1}-{games.Value.Item2}\t{games.Key.Item2}");
                                                choice = 8;
                                            }
                                            else if (choice3Task == 11)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabrana je opcija-Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika");
                                                ChangeTableGroup(allResults, tableGroup);
                                                Dictionary<string, (int, int)> tableGroupSorted = SortingTableGroup(tableGroup);
                                                int place = 0;
                                                Console.WriteLine("Mjesto na tablici, ime ekipe, broj bodova, gol razlika ");
                                                foreach (var gr in tableGroupSorted)
                                                {
                                                    place++;
                                                    Console.WriteLine($"{place}., {gr.Key}, {gr.Value.Item1}, {gr.Value.Item2}");
                                                }
                                                choice = 8;
                                            }
                                            else if (choice3Task == 0)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Odabran je povratak na pocetni izbornik");
                                                choice = 8;
                                            }
                                            else
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Nije unesen broj od 0 do 11. Ponovite unos");
                                                choice3Task = -1;
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Nije unesen integer. Ponovite unos");
                                            choice3Task = -1;
                                        }
                                    }
                                }
                                else if (choice3Taskn == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Odabran je povratak na pocetni izbornik");
                                    choice = 8;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Pogresan unos");
                                    choice3Taskn = -1;
                                }
                            }
                            catch
                            {
                                Console.Clear();
                                Console.WriteLine("Nije unesen integer. Ponovite unos!");
                                choice3Taskn = -1;
                            }
                        }
                    }
                    else if (choice == 4)
                    {
                        Console.Clear();
                        Console.WriteLine("Odabrana je opcija kontrola igraca");
                        int choice4Task = -1;
                        while (choice4Task < 0 || choice4Task > 10)
                        {
                            Console.WriteLine("Odaberi 1-3");
                            Console.WriteLine("1 - Unos novog igrača \n2 - Brisanje igrača \n3 - Uređivanje igrača \n0 - Povratak na pocetni izbornik");
                            int.TryParse(Console.ReadLine(), out choice4Task);
                            if (choice4Task == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Odabrali ste unos novog igraca");
                                Console.WriteLine("Za povratak na pocetni izbornik unesite 0");
                                if (playersDictionary.Count < 27)
                                {
                                    string newNameSurname = "";
                                    while (newNameSurname == "" || playersDictionary.ContainsKey(newNameSurname) == false)
                                    {
                                        Console.WriteLine("Unesite ime i prezime za novog igraca");
                                        newNameSurname = Console.ReadLine();
                                        if (playersDictionary.ContainsKey(newNameSurname) || newNameSurname == "")
                                        {
                                            Console.Clear();
                                            Console.WriteLine("U ekipi vec postoji igrac s tim imenom i prezimenom ili je unos prazan");
                                            choice4Task = -1;
                                            break;
                                        }
                                        else if (newNameSurname == "0")
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Odabrali ste povratak na pocetni izbornik");
                                            choice = 8;
                                            break;
                                        }
                                        else
                                        {
                                            string positionForNewPlayer = "";
                                            while (positionForNewPlayer != "MF" || positionForNewPlayer != "GK" || positionForNewPlayer == "DF" || positionForNewPlayer == "FW")
                                            {
                                                Console.WriteLine("Unesite poziciju za igraca: MF, DF, GK, FW");
                                                positionForNewPlayer = Console.ReadLine();
                                                if (positionForNewPlayer.ToUpper() == "MF" || positionForNewPlayer.ToUpper() == "GK" || positionForNewPlayer.ToUpper() == "DF" || positionForNewPlayer.ToUpper() == "FW")
                                                {
                                                    var answer = "";
                                                    Random rnd = new Random();
                                                    int ratingForNewPlayer = rnd.Next(0, 101);
                                                    Console.WriteLine($"Jesi li siguran da zelis unijeti {newNameSurname} na poziciji {positionForNewPlayer.ToUpper()}? Unesite y/n ");
                                                    answer = Console.ReadLine();
                                                    if (answer.ToLower() == "y")
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine($"Uspjesno ste unijeli novog igraca-{newNameSurname}!");
                                                        playersDictionary.Add(newNameSurname, (positionForNewPlayer.ToUpper(), ratingForNewPlayer));
                                                        choice4Task = -1;
                                                        break;
                                                    }
                                                    else if (answer.ToLower() == "n")
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Odustali ste od unosa novog igraca");
                                                        choice4Task = -1;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Nepoznat unos!");
                                                        choice4Task = -1;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("Pogresan unos pozicije!");
                                                    choice4Task = -1;
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Nazalost, u ekipi vec ima 26 igraca!");
                                    choice4Task = -1;
                                }
                            }
                            else if (choice4Task == 2)
                            {
                                Console.Clear();
                                Console.WriteLine("Odabrali ste brisanje igraca");
                                Console.WriteLine("Za povratak na pocetni izbornik unesite 0");
                                string nameForRemoving = "";
                                Console.WriteLine("Unesite ime i prezime igraca za brisanje");
                                nameForRemoving = Console.ReadLine();
                                if (playersDictionary.ContainsKey(nameForRemoving))
                                {
                                    var answer = "";
                                    Console.WriteLine($"Zelite li sigurno izbrisati igraca {nameForRemoving} iz ekipe? Unesite y/n");
                                    answer = Console.ReadLine();
                                    if (answer.ToLower() == "y")
                                    {
                                        Console.Clear();
                                        playersDictionary.Remove(nameForRemoving);
                                        Console.WriteLine("Igrac izbrisan iz ekipe");
                                        choice = 8;
                                    }
                                    else if (answer.ToLower() == "n")
                                    {
                                        Console.Clear();
                                        Console.WriteLine($"Odustali ste od brisanja igraca {nameForRemoving} iz ekipe");
                                        choice4Task = -1;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Nepoznat unos");
                                        choice4Task = -1;
                                    }
                                }
                                else if (nameForRemoving == "0")
                                {
                                    Console.Clear();
                                    Console.WriteLine("Odrabali ste povratak na pocetni izbornik");
                                    choice = 8;
                                }
                                else
                                {
                                    Console.WriteLine("U ekipi nema igraca s tim imenom i prezimenom");
                                    choice4Task = -1;
                                }
                            }
                            else if (choice4Task == 3)
                            {
                                Console.Clear();
                                Console.WriteLine("Odabrali ste uredivanje igraca");
                                int choice4Tasknew = -1;
                                while (choice4Tasknew < 0 || choice4Tasknew > 4)
                                {
                                    Console.WriteLine("1 - Uredi ime i prezime \n2 - Uredi poziciju igraca (GK, DF, FW, MF) \n3 - Uredi rating igraca (od 1 do 100)" +
                                        " \n0 - Povratak na pocetni izbornik");
                                    int.TryParse(Console.ReadLine(), out choice4Tasknew);
                                    if (choice4Tasknew == 1)
                                    {
                                        Console.Clear();
                                        string nameOfPlayer = "";
                                        Console.WriteLine("Odabrali ste uredivanje imena i prezimena igraca");
                                        Console.WriteLine("Unesite ime i prezime igraca koje zelite urediti \nZa povratak na pocetni izbornik unesite 0");
                                        nameOfPlayer = Console.ReadLine();
                                        if (nameOfPlayer != "0")
                                        {
                                            while (nameOfPlayer != "")
                                            {
                                                if (playersDictionary.ContainsKey(nameOfPlayer))
                                                {
                                                    string pos = playersDictionary[nameOfPlayer].position;
                                                    int rat = playersDictionary[nameOfPlayer].rating;
                                                    Console.WriteLine($"Unesite novo ime za igraca {nameOfPlayer}");
                                                    string newName = Console.ReadLine();
                                                    var answer = "";
                                                    Console.WriteLine($"Zelite li sigurno zamijeniti ime igraca {nameOfPlayer} sa {newName}? Unesite y/n");
                                                    answer = Console.ReadLine();
                                                    if (answer.ToLower() == "y")
                                                    {
                                                        Console.Clear();
                                                        playersDictionary.Remove(nameOfPlayer);
                                                        playersDictionary[newName] = (pos, rat);
                                                        Console.WriteLine("Novo ime za igraca je uspjesno uneseno");
                                                        choice4Tasknew = -1;
                                                        break;
                                                    }
                                                    else if (answer.ToLower() == "n")
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Odustali ste od uredivanja imena igraca");
                                                        choice4Task = -1;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Nepoznat unos");
                                                        choice4Task = -1;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("Ne postoji igrac s tim imenom");
                                                    choice4Tasknew = -1;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Odabran je povratak na pocetni izbornik");
                                            choice = 8;
                                        }
                                    }
                                    else if (choice4Tasknew == 2)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Odabrali ste uredivanje pozicije igraca (DF, GK, FW, MF)");
                                        Console.WriteLine("Unesite ime igraca ciju pozicije zelite promijeniti \nZa povratak na pocetni izbornik unesite 0");
                                        string nameOfPlayer = Console.ReadLine();
                                        if (nameOfPlayer != "0")
                                        {
                                            if (playersDictionary.ContainsKey(nameOfPlayer))
                                            {
                                                string newPos = "";
                                                while (newPos != "GK" || newPos != "FW" || newPos != "DF" || newPos != "MF")
                                                {
                                                    Console.WriteLine($"Unesite novu poziciju za igraca {nameOfPlayer}");
                                                    newPos = Console.ReadLine();
                                                    if (newPos.ToUpper() == "GK" || newPos.ToUpper() == "FW" || newPos.ToUpper() == "DF" || newPos.ToUpper() == "MF")
                                                    {
                                                        var answer = "";
                                                        Console.WriteLine($"Jeste li sigurni da zelite promijeniti poziciju igraca {nameOfPlayer} u {newPos}? Unesite y/n");
                                                        answer = Console.ReadLine();
                                                        if (answer.ToLower() == "y")
                                                        {
                                                            Console.Clear();
                                                            playersDictionary[nameOfPlayer] = (newPos, playersDictionary[nameOfPlayer].rating);
                                                            Console.WriteLine($"Unijeli ste novu poziciju {newPos} za igraca {nameOfPlayer}");
                                                            choice = 8;
                                                            break;
                                                        }
                                                        else if (answer == "n")
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Odustali ste od uredivanja pozicije za igraca {nameOfPlayer}");
                                                            choice4Task = -1;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine("Nepoznat unos");
                                                            choice4Task = -1;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Niste unijeli jednu od 4 ponudene opcije za poziciju");
                                                        choice4Tasknew = -1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Ne postoji igrac u ekipi s tim imenom i prezimenom");
                                                choice4Tasknew = -1;
                                            }
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Odabrali ste povratak na pocetni izbornik");
                                            choice = 8;
                                        }
                                    }
                                    else if (choice4Tasknew == 3)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Odabrali ste uredivanje rating igraca (od 0 do 100)");
                                        Console.WriteLine("Unesite ime igraca ciji rating zelite promijeniti \nZa povratak na pocetni izbornik unesite 0");
                                        string nameOfPlayer = Console.ReadLine();
                                        if (nameOfPlayer != "0")
                                        {
                                            if (playersDictionary.ContainsKey(nameOfPlayer))
                                            {
                                                int newRat = -1;
                                                while (newRat < 0 || newRat > 100)
                                                {
                                                    Console.WriteLine($"Unesite novi rating za igraca {nameOfPlayer}");
                                                    try
                                                    {
                                                        newRat = int.Parse(Console.ReadLine());
                                                        if (newRat > 0 && newRat < 101)
                                                        {
                                                            var answer = "";
                                                            Console.WriteLine($"Jeste li sigurni da zelite promijeniti rating igracu {nameOfPlayer}? Uneiste y/n");
                                                            answer = Console.ReadLine();
                                                            if (answer.ToLower() == "y")
                                                            {
                                                                Console.Clear();
                                                                playersDictionary[nameOfPlayer] = (playersDictionary[nameOfPlayer].position, newRat);
                                                                Console.WriteLine($"Unijeli ste novi rating {newRat} za igraca {nameOfPlayer}");
                                                                choice = 8;
                                                            }
                                                            else if (answer.ToLower() == "n")
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine("Odustali ste od uredivanja ratinga igraca");
                                                                choice4Task = -1;
                                                            }
                                                            else
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine("Nepoznat unos");
                                                                choice4Task = -1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Uneseni rating za promjenu ne nalazi se izmedu 0 i 100");
                                                            choice4Tasknew = -1;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("Nije unesen integer. Ponovite unos!");
                                                        newRat = -1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.Clear();

                                                Console.WriteLine("Ne postoji igrac u ekipi s tim imenom i prezimenom");
                                                choice4Tasknew = -1;
                                            }
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Odabrali ste povratak na pocetni izbornik");
                                            choice = 8;
                                        }
                                    }
                                    else if (choice4Tasknew == 0)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Odabran je povratak na pocetni izbornik");
                                        choice = 8;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Pogresan unos za odabir uredivanja igraca");
                                        choice4Tasknew = -1;
                                    }
                                }
                            }
                            else if (choice4Task == 0)
                            {
                                Console.Clear();
                                Console.WriteLine("Odabran je povratak na pocetni izbornik");
                                choice = 8;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Pogresan unos za opciju kontrole igraca ");
                                choice4Task = -1;
                            }
                        }
                    }
                    else if (choice == 0)
                    {
                        //Console.WriteLine("Odabrali ste izlaz iz aplikacije");
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Pogresan unos za odabir unutar aplikacije");
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nije unesen integer. Ponovite unos!");
                    choice = 8;
                }
            }

            //fje
            static void PlayersNew(Dictionary<string, (string position, int rating)> playersDictionary)
            {
                Random r = new Random();
                Console.Clear();
                foreach (var player in playersDictionary.ToList())
                {
                    int randomNumber = r.Next(0, 2);
                    int randomForPercent = r.Next(0, 5);
                    if (randomNumber == 0)
                    {
                        //snizava se
                        int number = (int)(player.Value.Item2 - (randomForPercent * 0.01 * player.Value.Item2));
                        Console.WriteLine($"Igracu {player.Key} rating je bio {player.Value.rating}, a sad je {number}");
                        playersDictionary[player.Key] = (player.Value.position, number);
                    }
                    else
                    {
                        //povecava se
                        int number = (int)(player.Value.Item2 + (randomForPercent * 0.01 * player.Value.Item2));
                        Console.WriteLine($"Igracu {player.Key} rating je bio {player.Value.rating}, a sad je {number}");
                        playersDictionary[player.Key] = (player.Value.position, number);

                    }
                }
            }

            static List<string> BestPlayersByRating(Dictionary<string, (string position, int rating)> playersDictionary)
            {
                int gk = 0;
                int df = 0;
                int mf = 0;
                int fw = 0;
                var bestPlayers = new List<string>();
                foreach (KeyValuePair<string, (string,int)> pl in playersDictionary.OrderByDescending(key => key.Value.rating))
                {
                    if (pl.Value.Item1 == "GK" && gk!=1)
                    {
                        bestPlayers.Add(pl.Key);
                        gk++;
                    }
                    else if(pl.Value.Item1=="DF" && df != 4)
                    {
                        bestPlayers.Add(pl.Key);
                        df++;
                    }
                    else if (pl.Value.Item1 == "MF" && mf != 3)
                    {
                        bestPlayers.Add(pl.Key);
                        mf++;
                    }
                    else if (pl.Value.Item1 == "FW" && fw != 3)
                    {
                        bestPlayers.Add(pl.Key);
                        fw++;
                    }
                }

                return bestPlayers;

            }
            static void ChangeRatings(List<string> scorerslist, List<string> bestPlayers, Dictionary<string, (string position, int rating)> playersDictionary, int winOrLose)
            {
                foreach (var player in playersDictionary.ToList())
                {
                    int newRating = 0;
                    if (scorerslist.Contains(player.Key))
                    {
                        newRating = (int)(player.Value.rating * 1.05);
                        playersDictionary[player.Key] = (player.Value.position, newRating);
                    }

                    if (winOrLose == 0)
                    {
                        //pobjeda
                        if (bestPlayers.Contains(player.Key))
                        {
                            newRating = (int)(player.Value.rating * 1.02);
                            playersDictionary[player.Key] = (player.Value.position, newRating);
                        }
                    }
                    else if (winOrLose == 1)
                    {
                        if (bestPlayers.Contains(player.Key))
                        {
                            newRating = (int)(player.Value.rating * 0.98);
                            playersDictionary[player.Key] = (player.Value.position, newRating);
                        }
                    }
                    else
                    {
                        //izjednacene ekipe, rating ostaje nepromijenjen!
                    }
                }
            }

            static void ChangeTableResultsCroatia(Dictionary<(string, string), (int, int)> allResults, int ourTeam, int opponents, int order)
            {
                if (order == 0)
                    allResults[("Hrvatska", "Maroko")] = (ourTeam, opponents);
                else if (order == 1)
                    allResults[("Hrvatska", "Kanada")] = (ourTeam, opponents);
                else
                    allResults[("Hrvatska", "Belgija")] = (ourTeam, opponents);

            }

            static void ChangeTableResultsOthers(Dictionary<(string, string), (int, int)> allResults, int first, int second, int order, Dictionary<string, (int, int)> tableGroup)
            {
                //pobjeda plus 3, poraz 0, izjednaceno 1 bod
                if (order == 0)
                {
                    allResults[("Belgija", "Kanada")] = (first, second);
                    if (first > second)
                    {
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 3, tableGroup["Belgija"].Item2);
                    }
                    else if(second> first)
                    {
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 3, tableGroup["Kanada"].Item2);
                    }
                    else
                    {
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 1, tableGroup["Belgija"].Item2);
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 1, tableGroup["Kanada"].Item2);

                    }
                }
                else if (order == 1)
                {
                    allResults[("Belgija", "Maroko")] = (first, second);
                    if (first > second)
                    {
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 3, tableGroup["Belgija"].Item2);
                    }
                    else if (second > first)
                    {
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 3, tableGroup["Maroko"].Item2);
                    }
                    else
                    {
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1 + 1, tableGroup["Belgija"].Item2);
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 1, tableGroup["Maroko"].Item2);
                    }
                }
                else
                {
                    allResults[("Kanada", "Maroko")] = (first, second);
                    if (first > second)
                    {
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 3, tableGroup["Kanada"].Item2);
                    }
                    else if (second > first)
                    {
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 3, tableGroup["Maroko"].Item2);
                    }
                    else
                    {
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1 + 1, tableGroup["Kanada"].Item2);
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1 + 1, tableGroup["Maroko"].Item2);
                    }
                }
            }

            static void ChangeTableGroup(Dictionary<(string, string), (int, int)> allResults, Dictionary<string, (int, int)> tableGroup)
            {
                foreach(var group in allResults) //za hrv
                {
                    if (group.Key.Item1 == "Hrvatska")
                        tableGroup["Hrvatska"] = (tableGroup["Hrvatska"].Item1, group.Value.Item1 - group.Value.Item2);
                    else if(group.Key.Item2=="Hrvatska")
                        tableGroup["Hrvatska"] = (tableGroup["Hrvatska"].Item1, group.Value.Item2 - group.Value.Item1);
                }
                foreach (var group in allResults) //za Maroko
                {
                    if (group.Key.Item1 == "Maroko")
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1, group.Value.Item1 - group.Value.Item2);
                    else if (group.Key.Item2 == "Maroko")
                        tableGroup["Maroko"] = (tableGroup["Maroko"].Item1, group.Value.Item2 - group.Value.Item1);
                }
                foreach (var group in allResults) //za Belgiju
                {
                    if (group.Key.Item1 == "Belgija")
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1, group.Value.Item1 - group.Value.Item2);
                    else if (group.Key.Item2 == "Belgija")
                        tableGroup["Belgija"] = (tableGroup["Belgija"].Item1, group.Value.Item2 - group.Value.Item1);
                }
                foreach (var group in allResults) //za Kanadu
                {
                    if (group.Key.Item1 == "Kanada")
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1, group.Value.Item1 - group.Value.Item2);
                    else if (group.Key.Item2 == "Kanada")
                        tableGroup["Kanada"] = (tableGroup["Kanada"].Item1, group.Value.Item2 - group.Value.Item1);
                }
            }

            static Dictionary<string, (int, int)> SortingTableGroup(Dictionary<string, (int, int)> tableGroup)
            {
                Dictionary<string, (int, int)> tableGroupSorted = new Dictionary<string, (int, int)>();
                foreach (KeyValuePair<string, (int, int)> gr in tableGroup.OrderByDescending(key => key.Value.Item1))
                    tableGroupSorted.Add(gr.Key, (gr.Value.Item1, gr.Value.Item2));
                return tableGroupSorted;

            }
        }
    }
}
