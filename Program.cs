using System;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();
    static double meilleureNote = 0; 

    static void Main(string[] args)
    {
        while (true)
        {
            AfficherMessage("Choisissez 'F'-Facile 'D'-Difficile 'Q'-Quitter");
            string reponse = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (reponse == "Q")
            {
                Console.Clear();
                break; 
            }
            else if (reponse == "F")
            {
                JouerModeFacile();
            }
            // Tâche 7 : Lancement du Mode Difficile
            else if (reponse == "D")
            {
                JouerModeDifficile();
            }
            else
            {
                AfficherMessage("Choix 'F', 'D' ou 'Q'");
            }
        }
    }

    static void AfficherMessage(string message)
    {
        Console.WriteLine(message);
    }

    static double CalculerNote(int min, int max, int nombreEssais)
    {
        int etendue = max - min + 1;
        return ((double)(etendue - nombreEssais) / (max - min)) * 100;
    }

    // Utilisation des Tuples pour valider et extraire les bornes min et max
    static (bool Valide, int Min, int Max) AnalyserBornes(string saisie)
    {
        // Sépare la saisie (ex: "1 20" ou "1,20") en deux parties
        string[] parties = saisie.Split(new[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
        
        if (parties.Length >= 2 && int.TryParse(parties[0], out int min) && int.TryParse(parties[1], out int max))
        {
            return (true, min, max); // Le Tuple renvoie le succès et les deux nombres
        }
        return (false, 0, 0); // En cas d'erreur de saisie
    }

    static void JouerModeFacile()
    {
        int min = 1;
        int max = 10;
        
        while (true)
        {
            int nombreCache = random.Next(min, max + 1);
            List<int> essaisJoues = new List<int>(); 

            while (true)
            {
                AfficherMessage($"choisissez un nombre entre [{min}, {max}] ou 'Q'-Quitter");
                string saisie = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (saisie == "Q")
                {
                    Console.Clear();
                    AfficherMessage($"Meilleure note du joueur : {meilleureNote:F0}%");
                    return; 
                }

                if (!int.TryParse(saisie, out int proposition) || proposition < min || proposition > max)
                {
                    AfficherMessage($"réponse doit être comprise entre {min} et {max}");
                    continue; 
                }

                essaisJoues.Add(proposition);

                if (proposition == nombreCache)
                {
                    double notePartie = CalculerNote(min, max, essaisJoues.Count);
                    if (notePartie > meilleureNote) meilleureNote = notePartie;

                    AfficherMessage("vous avez gagné");
                    Console.Clear();
                    break; 
                }
                else
                {
                    string listeEssais = string.Join(", ", essaisJoues);
                    AfficherMessage($"vous avez perdu avec la liste des réponses déjà données : {listeEssais}");
                }
            }
        }
    }

    // Tâche 7 : Logique du mode Difficile
    static void JouerModeDifficile()
    {
        while (true) // Boucle pour demander les bornes (7-a)
        {
            AfficherMessage("Entrez les bornes min et max de l'intervalle (ex: 1 20), ou 'Q'-Quitter");
            string saisieBornes = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (saisieBornes == "Q")
            {
                Console.Clear();
                AfficherMessage($"Meilleure note du joueur : {meilleureNote:F0}%");
                return; // Retour au menu principal
            }

            // Récupération des données via le Tuple
            var bornes = AnalyserBornes(saisieBornes);

            // 7-d. Vérification des conditions : min < max et (max - min) > 10
            if (!bornes.Valide || bornes.Min >= bornes.Max || (bornes.Max - bornes.Min) <= 10)
            {
                AfficherMessage("Choisissez 2 nombres entiers et (max - min) doit être > à 10");
                continue; // Retour au début pour redemander les bornes (7-b)
            }

            int min = bornes.Min;
            int max = bornes.Max;
            
            // 7-e. Génération du nombre
            int nombreCache = random.Next(min, max + 1);
            int nombreEssais = 0;

            while (true) // Boucle pour deviner le nombre (7-f)
            {
                AfficherMessage($"choisissez un nombre entre [{min}, {max}] ou 'Q'-Quitter");
                string saisieJeu = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (saisieJeu == "Q")
                {
                    Console.Clear();
                    break; // Casse cette boucle pour retourner au choix des bornes (7-a)
                }

                if (!int.TryParse(saisieJeu, out int proposition) || proposition < min || proposition > max)
                {
                    AfficherMessage($"Choix doit être compris entre {min} et {max}");
                    continue; // Retour à 7-f
                }

                nombreEssais++;

                // 7-j. Victoire
                if (proposition == nombreCache)
                {
                    double notePartie = CalculerNote(min, max, nombreEssais);
                    if (notePartie > meilleureNote) meilleureNote = notePartie;

                    AfficherMessage("vous avez gagné");
                    Console.Clear();
                    break; // Retour au choix des bornes (7-a)
                }
                // 7-k. Défaite (pas de liste demandée dans les consignes du mode difficile)
                else
                {
                    AfficherMessage("vous avez perdu");
                }
            }
        }
    }
}