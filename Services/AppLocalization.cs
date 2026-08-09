using System.Globalization;
using KabyliaTaste.Models;

namespace KabyliaTaste.Services
{
    public static class AppLocalization
    {
        private static readonly Dictionary<string, string> French = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Login"] = "Connexion",
            ["Username"] = "Nom d'utilisateur",
            ["Password"] = "Mot de passe",
            ["Please enter username and password."] = "Veuillez saisir le nom d'utilisateur et le mot de passe.",
            ["Invalid username or password."] = "Nom d'utilisateur ou mot de passe invalide.",
            ["Trial remaining"] = "Essai restant",
            ["day"] = "jour",
            ["days"] = "jours",
            ["Microsoft Store license is not valid."] = "La licence Microsoft Store n'est pas valide.",
            ["Microsoft Store license active."] = "Licence Microsoft Store active.",
            ["Good morning"] = "Bonjour",
            ["Good afternoon"] = "Bon après-midi",
            ["Good evening"] = "Bonsoir",
            ["You're logged in as:"] = "Vous êtes connecté en tant que :",
            ["admin"] = "Administrateur",
            ["user"] = "Utilisateur",
            ["Products"] = "Produits",
            ["Sales"] = "Ventes",
            ["Stats"] = "Statistiques",
            ["Expenses"] = "Dépenses",
            ["Invoices"] = "Factures",
            ["Settings"] = "Paramètres",
            ["Profile"] = "Profil",
            ["Store"] = "Boutique",
            ["Google Drive"] = "Google Drive",
            ["Backup"] = "Sauvegarde",
            ["Buyer"] = "Client",
            ["Period"] = "Période",
            ["Clear Filters"] = "Effacer les filtres",
            ["Print Report"] = "Imprimer le rapport",
            ["Day"] = "Jour",
            ["Week"] = "Semaine",
            ["Month"] = "Mois",
            ["Year"] = "Année",
            ["Show Buy Price"] = "Afficher le prix d'achat",
            ["Delete Sale"] = "Supprimer la vente",
            ["Update Sale"] = "Mettre à jour la vente",
            ["Add Sale"] = "Ajouter",
            ["Add Expense"] = "Ajouter",
            ["Update Expense"] = "Mettre à jour",
            ["Delete Expense"] = "Supprimer",
            ["Description"] = "Description",
            ["Category"] = "Catégorie",
            ["Select All"] = "Tout sélectionner",
            ["Page"] = "Page",
            ["Languages"] = "Langues",
            ["Language"] = "Langue",
            ["English"] = "Anglais",
            ["French"] = "Français",
            ["Save Preference"] = "Enregistrer",
            ["Currency"] = "Devise",
            ["Product Units"] = "Unités de produit",
            ["Current Password"] = "Mot de passe actuel",
            ["New Password"] = "Nouveau mot de passe",
            ["Confirm New Password"] = "Confirmer le nouveau mot de passe",
            ["Change Password"] = "Modifier le mot de passe",
            ["Change Username"] = "Modifier le nom d'utilisateur",
            ["Store Name"] = "Nom de la boutique",
            ["Save Name"] = "Enregistrer",
            ["Change Logo"] = "Changer le logo",
            ["Client ID"] = "ID client",
            ["Client Secret"] = "Secret client",
            ["Folder ID or Name"] = "ID ou nom du dossier",
            ["Refresh Token"] = "Jeton d'actualisation",
            ["Save Google Drive Config"] = "Enregistrer la config Google Drive",
            ["Generate Refresh Token"] = "Générer le jeton d'actualisation",
            ["Setup Help"] = "Aide à la configuration",
            ["Open Console"] = "Ouvrir la console Google Cloud",
            ["Download DB Backup"] = "Télécharger la sauvegarde",
            ["Upload / Restore DB Backup"] = "Téléverser / restaurer la sauvegarde",
            ["Upload to Google Drive"] = "Téléverser vers Google Drive",
            ["Download from Google Drive"] = "Télécharger depuis Google Drive",
            ["Download creates a local copy of the SQLite database. Restore replaces the current local database file."] = "Le téléchargement crée une copie locale de la base de données SQLite. La restauration remplace le fichier local actuel de la base de données.",
            ["Add"] = "Ajouter",
            ["Update"] = "Modifier",
            ["Delete"] = "Supprimer",
            ["Update Sale"] = "Modifier",
            ["Delete Sale"] = "Supprimer",
            ["Update Expense"] = "Modifier",
            ["Delete Expense"] = "Supprimer",
            ["Previous"] = "Précédent",
            ["Next"] = "Suivant",
            ["Clear"] = "Rafraîchir",
            ["Units Sold"] = "Unités vendues",
            ["Revenue"] = "Chiffre d'affaires",
            ["Cost"] = "Coût",
            ["Profit"] = "Bénéfice",
            ["Total Profit"] = "Bénéfice total",
            ["Hour"] = "Heure",
            ["Name"] = "Nom",
            ["Buy Price"] = "Prix d'achat",
            ["Sell Price"] = "Prix de vente",
            ["Quantity"] = "Quantité",
            ["Unit"] = "Unité",
            ["Search"] = "Rechercher",
            ["Select a product to update."] = "Sélectionnez un produit à mettre à jour.",
            ["Select a product to delete."] = "Sélectionnez un produit à supprimer.",
            ["A product with this name already exists."] = "Un produit avec ce nom existe déjà.",
            ["Name is required."] = "Le nom est obligatoire.",
            ["Confirm"] = "Confirmer",
            ["Are you sure you want to delete the selected product?"] = "Voulez-vous vraiment supprimer le produit sélectionné ?",
            ["Information"] = "Information",
            ["Validation"] = "Validation",
            ["Duplicate"] = "Doublon",
            ["Success"] = "Succès",
            ["Select a sale to update."] = "Sélectionnez une vente à mettre à jour.",
            ["Select a product."] = "Sélectionnez un produit.",
            ["Enter a buyer name to enable invoice printing."] = "Saisissez un nom d'acheteur pour activer l'impression de la facture.",
            ["Select at least one sale to include in the invoice."] = "Sélectionnez au moins une vente à inclure dans la facture.",
            ["Sale recorded successfully."] = "Vente enregistrée avec succès.",
            ["Not enough stock. Available:"] = "Stock insuffisant. Disponible :",
            ["Stock Error"] = "Erreur de stock",
            ["Select an expense to delete."] = "Sélectionnez une dépense à supprimer.",
            ["Are you sure you want to delete the selected expense?"] = "Voulez-vous vraiment supprimer la dépense sélectionnée ?",
            ["Select an invoice to preview."] = "Sélectionnez une facture à prévisualiser.",
            ["Select a valid invoice to preview."] = "Sélectionnez une facture valide à prévisualiser.",
            ["The selected invoice could not be found."] = "La facture sélectionnée est introuvable.",
            ["Status"] = "Statut",
            ["No"] = "Non",
            ["Yes"] = "Oui",
            ["Partially Paid"] = "Partiellement payée",
            ["Partially Paid Filter"] = "Partiellement payée",
            ["Invoice"] = "Facture",
            ["Date"] = "Date",
            ["Client"] = "Client",
            ["Invoice Details"] = "Détails de la facture",
            ["Total"] = "Total",
            ["Paid"] = "Payé",
            ["Due"] = "Reste dû",
            ["Product"] = "Produit",
            ["Qty"] = "Qté",
            ["Unit Price"] = "Prix unitaire",
            ["Grand Total"] = "Total général",
            ["Gross"] = "Brut",
            ["Debt"] = "Dette",
            ["Collected"] = "Collecté",
            ["Net Profit"] = "Bénéfice net",
            ["Sales Statistics Report"] = "Rapport des statistiques des ventes",
            ["Generated"] = "Généré",
            ["Filters"] = "Filtres",
            ["None"] = "Aucun",
            ["Units"] = "Unités",
            ["Thank you for your purchase!"] = "Merci pour votre achat !",
            ["Invoice #"] = "Facture n°",
            ["Login - "] = "Connexion - ",
            ["Logout"] = "Déconnexion",
            ["Amiar Store Manager"] = "Amiar Store Manager",
            ["Select Logo Image"] = "Sélectionner une image de logo",
            ["Image Files"] = "Fichiers image",
            ["Store name cannot be empty."] = "Le nom de la boutique ne peut pas être vide.",
            ["Store name saved."] = "Nom de la boutique enregistré.",
            ["Logo updated."] = "Logo mis à jour.",
            ["Username updated successfully."] = "Nom d'utilisateur mis à jour avec succès.",
            ["Username cannot be empty."] = "Le nom d'utilisateur ne peut pas être vide.",
            ["Password changed successfully."] = "Mot de passe modifié avec succès.",
            ["Please fill in all password fields."] = "Veuillez remplir tous les champs du mot de passe.",
            ["New password and confirmation do not match."] = "Le nouveau mot de passe et sa confirmation ne correspondent pas.",
            ["Unit name is required."] = "Le nom de l'unité est obligatoire.",
            ["This unit already exists."] = "Cette unité existe déjà.",
            ["Select a unit to update."] = "Sélectionnez une unité à mettre à jour.",
            ["Select a unit to delete."] = "Sélectionnez une unité à supprimer.",
            ["This unit is used by one or more products and cannot be deleted."] = "Cette unité est utilisée par un ou plusieurs produits et ne peut pas être supprimée.",
            ["Delete this unit?"] = "Supprimer cette unité ?",
            ["Prepare database backup..."] = "Préparation de la sauvegarde de la base de données...",
            ["Help"] = "Aide",
            ["Documentation"] = "Documentation",
            ["About Us"] = "À propos",
            ["Contact"] = "Contact",
            ["Bug Reporting"] = "Signalement de bugs",
            ["Software Version"] = "Version du logiciel",
            ["Open Documentation"] = "Ouvrir la documentation",
            ["Report an issue"] = "Signaler un problème",
            ["Email Us"] = "Nous écrire",
            ["Email address copied to clipboard."] = "Adresse e-mail copiée dans le presse-papiers.",
            ["Use the main tabs to manage products, sales, stats, expenses, invoices, and settings. This Help area provides quick reference information for the application."] = "Utilisez les onglets principaux pour gérer les produits, les ventes, les statistiques, les dépenses, les factures et les paramètres. Cette zone d'aide fournit des informations de référence rapide sur l'application.",
            ["Amiar Store Manager is a store management application for daily operations, sales tracking, and backups. It is designed to keep core store workflows in one place."] = "Amiar Store Manager est une application de gestion de magasin pour les opérations quotidiennes, le suivi des ventes et les sauvegardes. Elle est conçue pour regrouper les principales tâches du magasin en un seul endroit.",
            ["Email us for product questions and support requests. Click the email below to copy it."] = "Envoyez-nous un e-mail pour les questions sur le produit et les demandes d'assistance. Cliquez sur l'adresse ci-dessous pour la copier.",
            ["If you find a bug, open the issue tracker and include the steps to reproduce it, expected behavior, and screenshots if available."] = "Si vous trouvez un bug, ouvrez le suivi des problèmes et incluez les étapes pour le reproduire, le comportement attendu et des captures d'écran si possible.",
            ["Amiar Software builds practical business software focused on store operations, sales tracking, backups, and day-to-day management. Amiar Store Manager is designed to keep core workflows organized and easy to use."] = "Amiar Software développe des logiciels pratiques pour les entreprises, axés sur la gestion des magasins, le suivi des ventes, les sauvegardes et la gestion quotidienne. Amiar Store Manager est conçu pour garder les tâches principales organisées et faciles à utiliser.",
            ["App Version:"] = "Version de l'application :",
            ["Runtime Version:"] = "Version du runtime :",
        };

        public static string CurrentLanguageCode { get; private set; } = "en";

        public static void SetLanguage(string? languageCode)
        {
            CurrentLanguageCode = Normalize(languageCode);
            var culture = CurrentLanguageCode == "fr"
                ? CultureInfo.GetCultureInfo("fr-FR")
                : CultureInfo.GetCultureInfo("en-US");

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        public static string T(string key)
        {
            if (CurrentLanguageCode != "fr")
                return key;

            return French.TryGetValue(key, out var translated) ? translated : key;
        }

        public static string GetGreeting(DateTime now)
        {
            return now.Hour < 12 ? T("Good morning") : now.Hour < 18 ? T("Good afternoon") : T("Good evening");
        }

        public static string GetRoleLabel(bool isAdmin) => isAdmin ? T("admin") : T("user");

        public static string GetInvoiceStatusText(InvoicePaymentStatus status)
        {
            return status switch
            {
                InvoicePaymentStatus.Yes => T("Yes"),
                InvoicePaymentStatus.PartiallyPaid => CurrentLanguageCode == "fr" ? T("Partially Paid") : "PP",
                _ => T("No")
            };
        }

        public static string GetInvoiceStatusDisplayText(InvoicePaymentStatus status)
        {
            return status switch
            {
                InvoicePaymentStatus.Yes => CurrentLanguageCode == "fr" ? "Payée" : "Paid",
                InvoicePaymentStatus.PartiallyPaid => T("Partially Paid"),
                _ => CurrentLanguageCode == "fr" ? "Non payée" : "Unpaid"
            };
        }

        public static bool TryParseInvoiceStatus(string? text, out InvoicePaymentStatus status)
        {
            var normalized = text?.Trim();
            if (string.IsNullOrEmpty(normalized))
            {
                status = InvoicePaymentStatus.No;
                return false;
            }

            if (string.Equals(normalized, T("Yes"), StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalized, "Yes", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalized, "Oui", StringComparison.OrdinalIgnoreCase))
            {
                status = InvoicePaymentStatus.Yes;
                return true;
            }

            if (string.Equals(normalized, T("Partially Paid"), StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalized, "PP", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalized, "Partiel", StringComparison.OrdinalIgnoreCase))
            {
                status = InvoicePaymentStatus.PartiallyPaid;
                return true;
            }

            status = InvoicePaymentStatus.No;
            return string.Equals(normalized, T("No"), StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(normalized, "No", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(normalized, "Non", StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeInvoiceStatusFilter(InvoicePaymentStatus status)
        {
            return status switch
            {
                InvoicePaymentStatus.Yes => T("Yes"),
                InvoicePaymentStatus.PartiallyPaid => CurrentLanguageCode == "fr" ? "Partiel" : "PP",
                _ => T("No")
            };
        }

        private static string Normalize(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                return "en";

            return languageCode.StartsWith("fr", StringComparison.OrdinalIgnoreCase) ? "fr" : "en";
        }
    }
}
