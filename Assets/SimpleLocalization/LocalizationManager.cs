using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.SimpleLocalization
{
    /// <summary>
    /// Localization manager.
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>
        /// Fired when localization changed.
        /// </summary>
        public static event Action LocalizationChanged = () => { };

        public static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new Dictionary<string, Dictionary<string, string>>();
        private static string _language = "English";

        /// <summary>
        /// Get or set language.
        /// </summary>
        public static string Language
        {
            get { return _language; }
            set { _language = Application.platform == RuntimePlatform.WebGLPlayer ? "English" : value; LocalizationChanged(); }
        }

        /// <summary>
        /// Set default language.
        /// </summary>
        public static void AutoLanguage()
        {
            Language = "English";
        }

        /// <summary>
        /// Read localization spreadsheets.
        /// </summary>
        public static void Read(string path = "Localization")
        {
            try
            {
                if (Dictionary.Count > 0) return;

                var textAssets = Resources.LoadAll<TextAsset>(path);

                foreach (var textAsset in textAssets)
                {
                    var text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
                    var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                    foreach (Match match in matches)
                    {
                        text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
                    }

                    var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();

                    for (var i = 1; i < languages.Count; i++)
                    {
                        if (!Dictionary.ContainsKey(languages[i]))
                        {
                            Dictionary.Add(languages[i], new Dictionary<string, string>());
                        }
                    }

                    for (var i = 1; i < lines.Length; i++)
                    {
                        var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[comma]", ",").Replace("[newline]", "\n").Replace("[quotes]", "\"")).ToList();
                        var key = columns[0];

                        if (key == "") continue;

                        for (var j = 1; j < languages.Count; j++)
                        {
                            Dictionary[languages[j]].Add(key, columns[j]);
                        }
                    }
                }

                //ENGLISH LANGUAGE

                Dictionary["English"].Add("Knockdown", "Knockdown");
                Dictionary["English"].Add("Bottles", "Bottles");
                Dictionary["English"].Add("Play", "Play");
                Dictionary["English"].Add("Select world", "Select world");
                Dictionary["English"].Add("Forest", "Forest");
                Dictionary["English"].Add("Ancient", "Ancient");
                Dictionary["English"].Add("Mettle", "Mettle");
                Dictionary["English"].Add("Desert", "Desert");
                Dictionary["English"].Add("Snowy", "Snowy");
                Dictionary["English"].Add("Level Cleared", "Level Cleared");
                Dictionary["English"].Add("Level Failed", "Level Failed");
                Dictionary["English"].Add("Do not hit this bottle", "Do not hit this bottle");
                Dictionary["English"].Add("Pause", "Pause");
                Dictionary["English"].Add("Achieve _ stars in forest world to unlock this world", "Achieve _ stars in forest world to unlock this world");
                Dictionary["English"].Add("Achieve _ stars in ancient world to unlock this world", "Achieve _ stars in ancient world to unlock this world");
                Dictionary["English"].Add("Achieve _ stars in mettle world to unlock this world", "Achieve _ stars in mettle world to unlock this world");
                Dictionary["English"].Add("Achieve _ stars in desert world to unlock this world", "Achieve _ stars in desert world to unlock this world");
                Dictionary["English"].Add("Achieve _ stars in snow world to unlock this world", "Achieve _ stars in snow world to unlock this world");
                Dictionary["English"].Add("Exit", "Exit");
                Dictionary["English"].Add("ARE YOU SURE..?", "ARE YOU SURE..?");
                Dictionary["English"].Add("Loading...", "Loading...");
                Dictionary["English"].Add("StorePanel", "StorePanel");
                Dictionary["English"].Add("Select", "Select");
                Dictionary["English"].Add("Spin wheel", "Spin wheel");
                Dictionary["English"].Add("Spin", "Spin");
                Dictionary["English"].Add("WATCH VIDEO TO SPIN", "WATCH VIDEO TO SPIN");
                Dictionary["English"].Add("No Free Spins Available! Try Tomorrow", "No Free Spins Available! Try Tomorrow");
                Dictionary["English"].Add("Get 1000 coins", "Get 1000 coins");
                Dictionary["English"].Add("Menu", "Menu");
                Dictionary["English"].Add("New game", "New game");
                Dictionary["English"].Add("Continue", "Continue");
                Dictionary["English"].Add("Settings", "Settings");
                Dictionary["English"].Add("Write a review", "Write a review");
                Dictionary["English"].Add("Difficulty", "Difficulty");
                Dictionary["English"].Add("Easy", "Easy");
                Dictionary["English"].Add("Medium", "Medium");
                Dictionary["English"].Add("Hard", "Hard");
                Dictionary["English"].Add("Effects volume", "Effects volume");
                Dictionary["English"].Add("Music volume", "Music volume");
                Dictionary["English"].Add("Play time: {0:N1} h.", "Play time: {0:N1} h.");
                Dictionary["English"].Add("SKIP", "SKIP");
                Dictionary["English"].Add("premium without ads", "premium without ads");
                Dictionary["English"].Add("100% Add free", "100% Add free");
                Dictionary["English"].Add("skip video", "skip video");
                Dictionary["English"].Add("loading assets...", "loading assets...");
                Dictionary["English"].Add("loading player data…", "loading player data…");
                Dictionary["English"].Add("loading scene...", "loading scene...");
                Dictionary["English"].Add("starting…", "starting…");
                Dictionary["English"].Add("PLAY", "PLAY");
                Dictionary["English"].Add("SHOP", "SHOP");
                Dictionary["English"].Add("RATE US", "RATE US");
                Dictionary["English"].Add("SELECTED", "SELECTED");
                Dictionary["English"].Add("OK", "OK");
                Dictionary["English"].Add("Go Premium without Ads", "Go Premium without Ads");
                Dictionary["English"].Add("Demo levels completed Purchase full version to continue", "Demo levels completed Purchase full version to continue");
                Dictionary["English"].Add("Connection Lost!", "Connection Lost");
                Dictionary["English"].Add("Unable to load the game.", "Unable to load the game.");
                Dictionary["English"].Add("Please check your internet connection.", "Please check your internet connection.");


                //ARABIC LANGUAGE

                Dictionary["Arabic"].Add("Knockdown", "ضربه قاضيه");
                Dictionary["Arabic"].Add("Bottles", "زجاجات");
                Dictionary["Arabic"].Add("Play", "لعب");
                Dictionary["Arabic"].Add("Select world", "حدد العالم");
                Dictionary["Arabic"].Add("Forest", "غابة");
                Dictionary["Arabic"].Add("Ancient", "قديم");
                Dictionary["Arabic"].Add("Mettle", "همه");
                Dictionary["Arabic"].Add("Desert", "صحراء");
                Dictionary["Arabic"].Add("Snowy", "ثلجي");
                Dictionary["Arabic"].Add("Level Cleared", "تم مسح المستوى");
                Dictionary["Arabic"].Add("Level Failed", "فشل المستوى");
                Dictionary["Arabic"].Add("Do not hit this bottle", "لا تضرب هذه الزجاجة");
                Dictionary["Arabic"].Add("Pause", " توقف");
                Dictionary["Arabic"].Add("Achieve _ stars in forest world to unlock this world ", " احصل على _ نجوم في عالم الغابة لفتح هذا العالم");
                Dictionary["Arabic"].Add("Achieve _ stars in ancient world to unlock this world ", "احصل على _ نجوم في العالم القديم لفتح هذا العالم");
                Dictionary["Arabic"].Add("Achieve _ stars in mettle world to unlock this world  ", "احصل على _ نجوم في عالم الشجاعة لفتح هذا العالم");
                Dictionary["Arabic"].Add("Achieve _ stars in desert world to unlock this world", " احصل على _ نجوم في عالم الصحراء لفتح هذا العالم");
                Dictionary["Arabic"].Add("Achieve _ stars in snow world to unlock this world", " احصل على _ نجوم في عالم الثلج لفتح هذا العالم");
                Dictionary["Arabic"].Add("Exit", "مخرج");
                Dictionary["Arabic"].Add("ARE YOU SURE..?", "هل أنت متأكد..؟");
                Dictionary["Arabic"].Add("Loading...", "تحميل...");
                Dictionary["Arabic"].Add("StorePanel", "لوحة التخزين");
                Dictionary["Arabic"].Add("Select", "اختار");
                Dictionary["Arabic"].Add("Spin wheel", "عجلة الدوران");
                Dictionary["Arabic"].Add("Spin", "ردن");
                Dictionary["Arabic"].Add("WATCH VIDEO TO SPIN" , " شاهد الفيديو للدوران");
                Dictionary["Arabic"].Add("No Free Spins Available! Try Tomorrow", "لا توجد دورات مجانية متاحة! جرب الغد");
                Dictionary["Arabic"].Add("Get 1000 coins", "احصل على 1000 قطعة نقدية");
                Dictionary["Arabic"].Add("Menu", "قائمة");
                Dictionary["Arabic"].Add("New game", "لعبة جديدة");
                Dictionary["Arabic"].Add("Continue", "استمر");
                Dictionary["Arabic"].Add("Settings", "اعدادات");
                Dictionary["Arabic"].Add("Write a review", "اكتب تعليقا");
                Dictionary["Arabic"].Add("Settings", "اعدادات");
                Dictionary["Arabic"].Add("Difficulty", "صعوبة");
                Dictionary["Arabic"].Add("Easy", "سهل");
                Dictionary["Arabic"].Add("Medium", "متوسط");
                Dictionary["Arabic"].Add("Hard", "شاق");
                Dictionary["Arabic"].Add("Effects volume", "حجم التأثيرات");
                Dictionary["Arabic"].Add("Music volume", "حجم الموسيقى");
                Dictionary["Arabic"].Add("Play time: {0:N1} h.", "وقت اللعب: {0:N1} ساعة.");
                Dictionary["Arabic"].Add("SKIP", "تخطي");
                Dictionary["Arabic"].Add("premium without ads", "بريميوم بدون اعلانات");
                Dictionary["Arabic"].Add("100% Add free", "100٪ إضافة مجانية");
                Dictionary["Arabic"].Add("skip video", "تخطي الفيديو");
                Dictionary["Arabic"].Add("loading assets...", "تحميل الأصول...");
                Dictionary["Arabic"].Add("loading player data…", "تحميل بيانات المشغل...");
                Dictionary["Arabic"].Add("loading scene...", "مشهد التحميل ...");
                Dictionary["Arabic"].Add("starting…", "بدء...");
                Dictionary["Arabic"].Add("PLAY", "لعب");
                Dictionary["Arabic"].Add("SHOP", "دكان");
                Dictionary["Arabic"].Add("RATE US", "قيمنا");
                Dictionary["Arabic"].Add("SELECTED", "المحدد");
                Dictionary["Arabic"].Add("OK", "موافق");
                Dictionary["Arabic"].Add("Go Premium without Ads ", "احصل على النسخة المميزة بدون إعلانات");
                Dictionary["Arabic"].Add("Demo levels completed Purchase full version to continue", "تم إكمال مستويات العرض، اشترِ النسخة الكاملة للمتابعة");
                Dictionary["Arabic"].Add("Connection Lost!", "فقد الاتصال!");
                Dictionary["Arabic"].Add("Unable to load the game.", "غير قادر على تحميل اللعبة.");
                Dictionary["Arabic"].Add("Please check your internet connection.", "يرجى التحقق من اتصالك بالإنترنت.");


                //SPANISH LANGUAGE

                Dictionary["Spanish"].Add("Knockdown", "Derribar");
                Dictionary["Spanish"].Add("Bottles", "Botellas");
                Dictionary["Spanish"].Add("Play", "Jugar");
                Dictionary["Spanish"].Add("Select world", "Seleccionar mundo");
                Dictionary["Spanish"].Add("Forest", "Bosque");
                Dictionary["Spanish"].Add("Ancient", "Antiguo");
                Dictionary["Spanish"].Add("Mettle", "Coraje");
                Dictionary["Spanish"].Add("Desert", "Desierto");
                Dictionary["Spanish"].Add("Snowy", "Nevado");
                Dictionary["Spanish"].Add("Level Cleared", "Nivel Completado");
                Dictionary["Spanish"].Add("Level Failed", "Nivel Fallido");
                Dictionary["Spanish"].Add("Do not hit this bottle", "No golpees esta botella");
                Dictionary["Spanish"].Add("Pause", "Pausa");
                Dictionary["Spanish"].Add("Achieve _ stars in forest world to unlock this world", "Logra _ estrellas en el mundo del bosque para desbloquear este mundo");
                Dictionary["Spanish"].Add("Achieve _ stars in ancient world to unlock this world", "Logra _ estrellas en el mundo antiguo para desbloquear este mundo");
                Dictionary["Spanish"].Add("Achieve _ stars in mettle world to unlock this world", "Logra _ estrellas en el mundo del coraje para desbloquear este mundo");
                Dictionary["Spanish"].Add("Achieve _ stars in desert world to unlock this world", "Logra _ estrellas en el mundo del desierto para desbloquear este mundo");
                Dictionary["Spanish"].Add("Achieve _ stars in snow world to unlock this world", "Logra _ estrellas en el mundo nevado para desbloquear este mundo");
                Dictionary["Spanish"].Add("Exit", "Salir");
                Dictionary["Spanish"].Add("ARE YOU SURE..?", "¿ESTÁS SEGURO..?");
                Dictionary["Spanish"].Add("Loading...", "Cargando...");
                Dictionary["Spanish"].Add("StorePanel", "Panel de la tienda");
                Dictionary["Spanish"].Add("Select", "Seleccionar");
                Dictionary["Spanish"].Add("Spin wheel", "Girar rueda");
                Dictionary["Spanish"].Add("Spin", "Girar");
                Dictionary["Spanish"].Add("WATCH VIDEO TO SPIN", "VER VIDEO PARA GIRAR");
                Dictionary["Spanish"].Add("No Free Spins Available! Try Tomorrow", "¡No hay giros gratis disponibles! Inténtalo mañana");
                Dictionary["Spanish"].Add("Get 1000 coins", "Obtén 1000 monedas");
                Dictionary["Spanish"].Add("Menu", "Menú");
                Dictionary["Spanish"].Add("New game", "Nuevo juego");
                Dictionary["Spanish"].Add("Continue", "Continuar");
                Dictionary["Spanish"].Add("Settings", "Configuración");
                Dictionary["Spanish"].Add("Write a review", "Escribir una reseña");
                Dictionary["Spanish"].Add("Difficulty", "Dificultad");
                Dictionary["Spanish"].Add("Easy", "Fácil");
                Dictionary["Spanish"].Add("Medium", "Medio");
                Dictionary["Spanish"].Add("Hard", "Difícil");
                Dictionary["Spanish"].Add("Effects volume", "Volumen de efectos");
                Dictionary["Spanish"].Add("Music volume", "Volumen de música");
                Dictionary["Spanish"].Add("Play time: {0:N1} h.", "Tiempo de juego: {0:N1} h.");
                Dictionary["Spanish"].Add("SKIP", "SALTAR");
                Dictionary["Spanish"].Add("premium without ads", "Premium sin anuncios");
                Dictionary["Spanish"].Add("100% Add free", "100% sin anuncios");
                Dictionary["Spanish"].Add("skip video", "saltar video");
                Dictionary["Spanish"].Add("loading assets...", "cargando recursos...");
                Dictionary["Spanish"].Add("loading player data…", "cargando datos del jugador...");
                Dictionary["Spanish"].Add("loading scene...", "cargando escena...");
                Dictionary["Spanish"].Add("starting…", "iniciando...");
                Dictionary["Spanish"].Add("PLAY", "JUGAR");
                Dictionary["Spanish"].Add("SHOP", "TIENDA");
                Dictionary["Spanish"].Add("RATE US", "CALIFÍCANOS");
                Dictionary["Spanish"].Add("SELECTED", "SELECCIONADO");
                Dictionary["Spanish"].Add("OK", "OK");
                Dictionary["Spanish"].Add("Go Premium without Ads", "Hazte Premium sin anuncios");
                Dictionary["Spanish"].Add("Demo levels completed Purchase full version to continue", "Niveles de demostración completados, compra la versión completa para continuar");
                Dictionary["Spanish"].Add("Connection Lost!", "¡Conexión Perdida!");
                Dictionary["Spanish"].Add("Unable to load the game.", "No se puede cargar el juego.");
                Dictionary["Spanish"].Add("Please check your internet connection.", "Por favor, revisa tu conexión a internet.");


                //GERMAN LANGUAGE

                Dictionary["German"].Add("Knockdown", "Abbruch");
                Dictionary["German"].Add("Bottles", "Flaschen");
                Dictionary["German"].Add("Play", "Spielen");
                Dictionary["German"].Add("Select world", "Welt auswählen");
                Dictionary["German"].Add("Forest", "Wald");
                Dictionary["German"].Add("Ancient", "Antik");
                Dictionary["German"].Add("Mettle", "Mut");
                Dictionary["German"].Add("Desert", "Wüste");
                Dictionary["German"].Add("Snowy", "Schnee");
                Dictionary["German"].Add("Level Cleared", "Level abgeschlossen");
                Dictionary["German"].Add("Level Failed", "Level fehlgeschlagen");
                Dictionary["German"].Add("Do not hit this bottle", "Schlage diese Flasche nicht");
                Dictionary["German"].Add("Pause", "Pause");
                Dictionary["German"].Add("Achieve _ stars in forest world to unlock this world", "Erreiche _ Sterne im Waldwelt, um diese Welt freizuschalten");
                Dictionary["German"].Add("Achieve _ stars in ancient world to unlock this world", "Erreiche _ Sterne im antiken Welt, um diese Welt freizuschalten");
                Dictionary["German"].Add("Achieve _ stars in mettle world to unlock this world", "Erreiche _ Sterne im Mutwelt, um diese Welt freizuschalten");
                Dictionary["German"].Add("Achieve _ stars in desert world to unlock this world", "Erreiche _ Sterne im Wüstenwelt, um diese Welt freizuschalten");
                Dictionary["German"].Add("Achieve _ stars in snow world to unlock this world", "Erreiche _ Sterne im Schneewelt, um diese Welt freizuschalten");
                Dictionary["German"].Add("Exit", "Verlassen");
                Dictionary["German"].Add("ARE YOU SURE..?", "BIST DU SICHER..?");
                Dictionary["German"].Add("Loading...", "Laden...");
                Dictionary["German"].Add("StorePanel", "Speicherpanel");
                Dictionary["German"].Add("Select", "Auswählen");
                Dictionary["German"].Add("Spin wheel", "Rad drehen");
                Dictionary["German"].Add("Spin", "Drehen");
                Dictionary["German"].Add("WATCH VIDEO TO SPIN", "VIDEO ANSEHEN ZUM DREHEN");
                Dictionary["German"].Add("No Free Spins Available! Try Tomorrow", "Keine kostenlosen Drehungen verfügbar! Versuche es morgen");
                Dictionary["German"].Add("Get 1000 coins", "Erhalte 1000 Münzen");
                Dictionary["German"].Add("Menu", "Menü");
                Dictionary["German"].Add("New game", "Neues Spiel");
                Dictionary["German"].Add("Continue", "Fortsetzen");
                Dictionary["German"].Add("Settings", "Einstellungen");
                Dictionary["German"].Add("Write a review", "Bewertung schreiben");
                Dictionary["German"].Add("Difficulty", "Schwierigkeit");
                Dictionary["German"].Add("Easy", "Einfach");
                Dictionary["German"].Add("Medium", "Mittel");
                Dictionary["German"].Add("Hard", "Schwer");
                Dictionary["German"].Add("Effects volume", "Effektlautstärke");
                Dictionary["German"].Add("Music volume", "Musiklautstärke");
                Dictionary["German"].Add("Play time: {0:N1} h.", "Spielzeit: {0:N1} h.");
                Dictionary["German"].Add("SKIP", "ÜBERSPRINGEN");
                Dictionary["German"].Add("premium without ads", "Premium ohne Werbung");
                Dictionary["German"].Add("100% Add free", "100% werbefrei");
                Dictionary["German"].Add("skip video", "Video überspringen");
                Dictionary["German"].Add("loading assets...", "Ressourcen laden...");
                Dictionary["German"].Add("loading player data…", "Spielerdaten laden...");
                Dictionary["German"].Add("loading scene...", "Szene laden...");
                Dictionary["German"].Add("starting…", "Starten...");
                Dictionary["German"].Add("PLAY", "SPIELEN");
                Dictionary["German"].Add("SHOP", "GESCHÄFT");
                Dictionary["German"].Add("RATE US", "BEWERTEN SIE UNS");
                Dictionary["German"].Add("SELECTED", "AUSGEWÄHLT");
                Dictionary["German"].Add("OK", "OK");
                Dictionary["German"].Add("Go Premium without Ads", "Werde Premium ohne Werbung");
                Dictionary["German"].Add("Demo levels completed Purchase full version to continue", "Demo-Level abgeschlossen, kaufen Sie die Vollversion, um fortzufahren");
                Dictionary["German"].Add("Connection Lost!", "Verbindung verloren!");
                Dictionary["German"].Add("Unable to load the game.", "Spiel kann nicht geladen werden.");
                Dictionary["German"].Add("Please check your internet connection.", "Bitte überprüfe deine Internetverbindung.");

                //PORTUGUESE LANGUAGE

                Dictionary["Portuguese"].Add("Knockdown", "Derrubar");
                Dictionary["Portuguese"].Add("Bottles", "Garrafas");
                Dictionary["Portuguese"].Add("Play", "Jogar");
                Dictionary["Portuguese"].Add("Select world", "Selecionar mundo");
                Dictionary["Portuguese"].Add("Forest", "Floresta");
                Dictionary["Portuguese"].Add("Ancient", "Antigo");
                Dictionary["Portuguese"].Add("Mettle", "Coragem");
                Dictionary["Portuguese"].Add("Desert", "Deserto");
                Dictionary["Portuguese"].Add("Snowy", "Nevado");
                Dictionary["Portuguese"].Add("Level Cleared", "Nível Completado");
                Dictionary["Portuguese"].Add("Level Failed", "Nível Falhou");
                Dictionary["Portuguese"].Add("Do not hit this bottle", "Não bata nesta garrafa");
                Dictionary["Portuguese"].Add("Pause", "Pausa");
                Dictionary["Portuguese"].Add("Achieve _ stars in forest world to unlock this world", "Alcançar _ estrelas no mundo da floresta para desbloquear este mundo");
                Dictionary["Portuguese"].Add("Achieve _ stars in ancient world to unlock this world", "Alcançar _ estrelas no mundo antigo para desbloquear este mundo");
                Dictionary["Portuguese"].Add("Achieve _ stars in mettle world to unlock this world", "Alcançar _ estrelas no mundo da coragem para desbloquear este mundo");
                Dictionary["Portuguese"].Add("Achieve _ stars in desert world to unlock this world", "Alcançar _ estrelas no mundo do deserto para desbloquear este mundo");
                Dictionary["Portuguese"].Add("Achieve _ stars in snow world to unlock this world", "Alcançar _ estrelas no mundo da neve para desbloquear este mundo");
                Dictionary["Portuguese"].Add("Exit", "Sair");
                Dictionary["Portuguese"].Add("ARE YOU SURE..?", "VOCÊ TEM CERTEZA..?");
                Dictionary["Portuguese"].Add("Loading...", "Carregando...");
                Dictionary["Portuguese"].Add("StorePanel", "Painel da loja");
                Dictionary["Portuguese"].Add("Select", "Selecionar");
                Dictionary["Portuguese"].Add("Spin wheel", "Girar roda");
                Dictionary["Portuguese"].Add("Spin", "Girar");
                Dictionary["Portuguese"].Add("WATCH VIDEO TO SPIN", "ASSISTIR VÍDEO PARA GIRAR");
                Dictionary["Portuguese"].Add("No Free Spins Available! Try Tomorrow", "Sem giros grátis disponíveis! Tente amanhã");
                Dictionary["Portuguese"].Add("Get 1000 coins", "Obter 1000 moedas");
                Dictionary["Portuguese"].Add("Menu", "Menu");
                Dictionary["Portuguese"].Add("New game", "Novo jogo");
                Dictionary["Portuguese"].Add("Continue", "Continuar");
                Dictionary["Portuguese"].Add("Settings", "Configurações");
                Dictionary["Portuguese"].Add("Write a review", "Escrever uma resenha");
                Dictionary["Portuguese"].Add("Difficulty", "Dificuldade");
                Dictionary["Portuguese"].Add("Easy", "Fácil");
                Dictionary["Portuguese"].Add("Medium", "Médio");
                Dictionary["Portuguese"].Add("Hard", "Difícil");
                Dictionary["Portuguese"].Add("Effects volume", "Volume dos efeitos");
                Dictionary["Portuguese"].Add("Music volume", "Volume da música");
                Dictionary["Portuguese"].Add("Play time: {0:N1} h.", "Tempo de jogo: {0:N1} h.");
                Dictionary["Portuguese"].Add("SKIP", "PULAR");
                Dictionary["Portuguese"].Add("premium without ads", "premium sem anúncios");
                Dictionary["Portuguese"].Add("100% Add free", "100% sem anúncios");
                Dictionary["Portuguese"].Add("skip video", "pular vídeo");
                Dictionary["Portuguese"].Add("loading assets...", "carregando recursos...");
                Dictionary["Portuguese"].Add("loading player data…", "carregando dados do jogador...");
                Dictionary["Portuguese"].Add("loading scene...", "carregando cena...");
                Dictionary["Portuguese"].Add("starting…", "iniciando...");
                Dictionary["Portuguese"].Add("PLAY", "JOGAR");
                Dictionary["Portuguese"].Add("SHOP", "LOJA");
                Dictionary["Portuguese"].Add("RATE US", "AVALIE-NOS");
                Dictionary["Portuguese"].Add("SELECTED", "SELECIONADO");
                Dictionary["Portuguese"].Add("OK", "OK");
                Dictionary["Portuguese"].Add("Go Premium without Ads", "Torne-se Premium sem anúncios");
                Dictionary["Portuguese"].Add("Demo levels completed Purchase full version to continue", "Níveis de demonstração concluídos, compre a versão completa para continuar");
                Dictionary["Portuguese"].Add("Connection Lost!", "Conexão Perdida!");
                Dictionary["Portuguese"].Add("Unable to load the game.", "Incapaz de carregar o jogo.");
                Dictionary["Portuguese"].Add("Please check your internet connection.", "Por favor, verifique sua conexão com a internet.");


                //POLISH LANGUAGE

                Dictionary["Polish"].Add("Knockdown", "Obalić");
                Dictionary["Polish"].Add("Bottles", "Butelki");
                Dictionary["Polish"].Add("Play", "Graj");
                Dictionary["Polish"].Add("Select world", "Wybierz świat");
                Dictionary["Polish"].Add("Forest", "Las");
                Dictionary["Polish"].Add("Ancient", "Starożytny");
                Dictionary["Polish"].Add("Mettle", "Hart ducha");
                Dictionary["Polish"].Add("Desert", "Pustynia");
                Dictionary["Polish"].Add("Snowy", "Zaśnieżony");
                Dictionary["Polish"].Add("Level Cleared", "Poziom ukończony");
                Dictionary["Polish"].Add("Level Failed", "Poziom nieudany");
                Dictionary["Polish"].Add("Do not hit this bottle", "Nie uderzaj tej butelki");
                Dictionary["Polish"].Add("Pause", "Pauza");
                Dictionary["Polish"].Add("Achieve _ stars in forest world to unlock this world", "Zdobyj _ gwiazdek w świecie lasu, aby odblokować ten świat");
                Dictionary["Polish"].Add("Achieve _ stars in ancient world to unlock this world", "Zdobyj _ gwiazdek w starożytnym świecie, aby odblokować ten świat");
                Dictionary["Polish"].Add("Achieve _ stars in mettle world to unlock this world", "Zdobyj _ gwiazdek w świecie hartu ducha, aby odblokować ten świat");
                Dictionary["Polish"].Add("Achieve _ stars in desert world to unlock this world", "Zdobyj _ gwiazdek w świecie pustyni, aby odblokować ten świat");
                Dictionary["Polish"].Add("Achieve _ stars in snow world to unlock this world", "Zdobyj _ gwiazdek w zaśnieżonym świecie, aby odblokować ten świat");
                Dictionary["Polish"].Add("Exit", "Wyjście");
                Dictionary["Polish"].Add("ARE YOU SURE..?", "CZY JESTEŚ PEWIEN..?");
                Dictionary["Polish"].Add("Loading...", "Ładowanie...");
                Dictionary["Polish"].Add("StorePanel", "Panel sklepu");
                Dictionary["Polish"].Add("Select", "Wybierz");
                Dictionary["Polish"].Add("Spin wheel", "Kręć kołem");
                Dictionary["Polish"].Add("Spin", "Kręć");
                Dictionary["Polish"].Add("WATCH VIDEO TO SPIN", "OGLĄDAJ WIDEO, ABY KRĘCIĆ");
                Dictionary["Polish"].Add("No Free Spins Available! Try Tomorrow", "Brak darmowych obrotów! Spróbuj jutro");
                Dictionary["Polish"].Add("Get 1000 coins", "Zdobądź 1000 monet");
                Dictionary["Polish"].Add("Menu", "Menu");
                Dictionary["Polish"].Add("New game", "Nowa gra");
                Dictionary["Polish"].Add("Continue", "Kontynuuj");
                Dictionary["Polish"].Add("Settings", "Ustawienia");
                Dictionary["Polish"].Add("Write a review", "Napisz recenzję");
                Dictionary["Polish"].Add("Difficulty", "Trudność");
                Dictionary["Polish"].Add("Easy", "Łatwy");
                Dictionary["Polish"].Add("Medium", "Średni");
                Dictionary["Polish"].Add("Hard", "Trudny");
                Dictionary["Polish"].Add("Effects volume", "Głośność efektów");
                Dictionary["Polish"].Add("Music volume", "Głośność muzyki");
                Dictionary["Polish"].Add("Play time: {0:N1} h.", "Czas gry: {0:N1} h.");
                Dictionary["Polish"].Add("SKIP", "POMIŃ");
                Dictionary["Polish"].Add("premium without ads", "premium bez reklam");
                Dictionary["Polish"].Add("100% Add free", "100% bez reklam");
                Dictionary["Polish"].Add("skip video", "pomiń wideo");
                Dictionary["Polish"].Add("loading assets...", "ładowanie zasobów...");
                Dictionary["Polish"].Add("loading player data…", "ładowanie danych gracza...");
                Dictionary["Polish"].Add("loading scene...", "ładowanie sceny...");
                Dictionary["Polish"].Add("starting…", "rozpoczynanie...");
                Dictionary["Polish"].Add("PLAY", "GRAJ");
                Dictionary["Polish"].Add("SHOP", "SKLEP");
                Dictionary["Polish"].Add("RATE US", "OCEN NAS");
                Dictionary["Polish"].Add("SELECTED", "WYBRANY");
                Dictionary["Polish"].Add("OK", "OK");
                Dictionary["Polish"].Add("Go Premium without Ads", "Przejdź na Premium bez reklam");
                Dictionary["Polish"].Add("Demo levels completed Purchase full version to continue", "Poziomy demonstracyjne ukończone Kup pełną wersję, aby kontynuować");
                Dictionary["Polish"].Add("Connection Lost!", "Utracono połączenie!");
                Dictionary["Polish"].Add("Unable to load the game.", "Nie można załadować gry.");
                Dictionary["Polish"].Add("Please check your internet connection.", "Proszę sprawdź swoje połączenie internetowe.");


                //FRENCH LANGUAGE

                Dictionary["French"].Add("Knockdown", "Renverser");
                Dictionary["French"].Add("Bottles", "Bouteilles");
                Dictionary["French"].Add("Play", "Jouer");
                Dictionary["French"].Add("Select world", "Sélectionner le monde");
                Dictionary["French"].Add("Forest", "Forêt");
                Dictionary["French"].Add("Ancient", "Ancien");
                Dictionary["French"].Add("Mettle", "Courage");
                Dictionary["French"].Add("Desert", "Désert");
                Dictionary["French"].Add("Snowy", "Enneigé");
                Dictionary["French"].Add("Level Cleared", "Niveau Complété");
                Dictionary["French"].Add("Level Failed", "Niveau Échoué");
                Dictionary["French"].Add("Do not hit this bottle", "Ne frappez pas cette bouteille");
                Dictionary["French"].Add("Pause", "Pause");
                Dictionary["French"].Add("Achieve _ stars in forest world to unlock this world", "Obtenez _ étoiles dans le monde de la forêt pour débloquer ce monde");
                Dictionary["French"].Add("Achieve _ stars in ancient world to unlock this world", "Obtenez _ étoiles dans le monde ancien pour débloquer ce monde");
                Dictionary["French"].Add("Achieve _ stars in mettle world to unlock this world", "Obtenez _ étoiles dans le monde du courage pour débloquer ce monde");
                Dictionary["French"].Add("Achieve _ stars in desert world to unlock this world", "Obtenez _ étoiles dans le monde du désert pour débloquer ce monde");
                Dictionary["French"].Add("Achieve _ stars in snow world to unlock this world", "Obtenez _ étoiles dans le monde enneigé pour débloquer ce monde");
                Dictionary["French"].Add("Exit", "Sortie");
                Dictionary["French"].Add("ARE YOU SURE..?", "ÊTES-VOUS SÛR..?");
                Dictionary["French"].Add("Loading...", "Chargement...");
                Dictionary["French"].Add("StorePanel", "Panneau de la boutique");
                Dictionary["French"].Add("Select", "Sélectionner");
                Dictionary["French"].Add("Spin wheel", "Tourner la roue");
                Dictionary["French"].Add("Spin", "Tourner");
                Dictionary["French"].Add("WATCH VIDEO TO SPIN", "REGARDER LA VIDÉO POUR TOURNER");
                Dictionary["French"].Add("No Free Spins Available! Try Tomorrow", "Pas de tours gratuits disponibles! Essayez demain");
                Dictionary["French"].Add("Get 1000 coins", "Obtenez 1000 pièces");
                Dictionary["French"].Add("Menu", "Menu");
                Dictionary["French"].Add("New game", "Nouveau jeu");
                Dictionary["French"].Add("Continue", "Continuer");
                Dictionary["French"].Add("Settings", "Paramètres");
                Dictionary["French"].Add("Write a review", "Écrire un avis");
                Dictionary["French"].Add("Difficulty", "Difficulté");
                Dictionary["French"].Add("Easy", "Facile");
                Dictionary["French"].Add("Medium", "Moyen");
                Dictionary["French"].Add("Hard", "Difficile");
                Dictionary["French"].Add("Effects volume", "Volume des effets");
                Dictionary["French"].Add("Music volume", "Volume de la musique");
                Dictionary["French"].Add("Play time: {0:N1} h.", "Temps de jeu : {0:N1} h.");
                Dictionary["French"].Add("SKIP", "PASSER");
                Dictionary["French"].Add("premium without ads", "premium sans annonces");
                Dictionary["French"].Add("100% Add free", "100% sans annonces");
                Dictionary["French"].Add("skip video", "passer la vidéo");
                Dictionary["French"].Add("loading assets...", "chargement des ressources...");
                Dictionary["French"].Add("loading player data…", "chargement des données du joueur...");
                Dictionary["French"].Add("loading scene...", "chargement de la scène...");
                Dictionary["French"].Add("starting…", "démarrage...");
                Dictionary["French"].Add("PLAY", "JOUER");
                Dictionary["French"].Add("SHOP", "BOUTIQUE");
                Dictionary["French"].Add("RATE US", "ÉVALUEZ-NOUS");
                Dictionary["French"].Add("SELECTED", "SÉLECTIONNÉ");
                Dictionary["French"].Add("OK", "OK");
                Dictionary["French"].Add("Go Premium without Ads", "Passez à Premium sans annonces");
                Dictionary["French"].Add("Demo levels completed Purchase full version to continue", "Niveaux de démonstration complétés, achetez la version complète pour continuer");
                Dictionary["French"].Add("Connection Lost!", "Connexion perdue!");
                Dictionary["French"].Add("Unable to load the game.", "Impossible de charger le jeu.");
                Dictionary["French"].Add("Please check your internet connection.", "Veuillez vérifier votre connexion Internet.");


                //DUTCH LANGUAGE

                Dictionary["Dutch"].Add("Knockdown", "Neerslag");
                Dictionary["Dutch"].Add("Bottles", "Flessen");
                Dictionary["Dutch"].Add("Play", "Spelen");
                Dictionary["Dutch"].Add("Select world", "Selecteer wereld");
                Dictionary["Dutch"].Add("Forest", "Bos");
                Dictionary["Dutch"].Add("Ancient", "Oud");
                Dictionary["Dutch"].Add("Mettle", "Moed");
                Dictionary["Dutch"].Add("Desert", "Woestijn");
                Dictionary["Dutch"].Add("Snowy", "Sneeuw");
                Dictionary["Dutch"].Add("Level Cleared", "Niveau Geslaagd");
                Dictionary["Dutch"].Add("Level Failed", "Niveau Mislukt");
                Dictionary["Dutch"].Add("Do not hit this bottle", "Raak deze fles niet");
                Dictionary["Dutch"].Add("Pause", "Pauze");
                Dictionary["Dutch"].Add("Achieve _ stars in forest world to unlock this world", "Behaal _ sterren in het boswereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].Add("Achieve _ stars in ancient world to unlock this world", "Behaal _ sterren in de oude wereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].Add("Achieve _ stars in mettle world to unlock this world", "Behaal _ sterren in de moedwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].Add("Achieve _ stars in desert world to unlock this world", "Behaal _ sterren in de woestijnwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].Add("Achieve _ stars in snow world to unlock this world", "Behaal _ sterren in de sneeuwwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].Add("Exit", "Afsluiten");
                Dictionary["Dutch"].Add("ARE YOU SURE..?", "WEET JE HET ZEKER..?");
                Dictionary["Dutch"].Add("Loading...", "Laden...");
                Dictionary["Dutch"].Add("StorePanel", "Winkelpaneel");
                Dictionary["Dutch"].Add("Select", "Selecteren");
                Dictionary["Dutch"].Add("Spin wheel", "Wiel draaien");
                Dictionary["Dutch"].Add("Spin", "Draaien");
                Dictionary["Dutch"].Add("WATCH VIDEO TO SPIN", "BEKIJK VIDEO OM TE DRAAIEN");
                Dictionary["Dutch"].Add("No Free Spins Available! Try Tomorrow", "Geen gratis spins beschikbaar! Probeer het morgen");
                Dictionary["Dutch"].Add("Get 1000 coins", "Krijg 1000 munten");
                Dictionary["Dutch"].Add("Menu", "Menu");
                Dictionary["Dutch"].Add("New game", "Nieuw spel");
                Dictionary["Dutch"].Add("Continue", "Doorgaan");
                Dictionary["Dutch"].Add("Settings", "Instellingen");
                Dictionary["Dutch"].Add("Write a review", "Schrijf een recensie");
                Dictionary["Dutch"].Add("Difficulty", "Moeilijkheidsgraad");
                Dictionary["Dutch"].Add("Easy", "Gemakkelijk");
                Dictionary["Dutch"].Add("Medium", "Gemiddeld");
                Dictionary["Dutch"].Add("Hard", "Moeilijk");
                Dictionary["Dutch"].Add("Effects volume", "Effecten volume");
                Dictionary["Dutch"].Add("Music volume", "Muziek volume");
                Dictionary["Dutch"].Add("Play time: {0:N1} h.", "Speeltijd: {0:N1} uur.");
                Dictionary["Dutch"].Add("SKIP", "OVERSLAAN");
                Dictionary["Dutch"].Add("premium without ads", "premium zonder advertenties");
                Dictionary["Dutch"].Add("100% Add free", "100% advertentievrij");
                Dictionary["Dutch"].Add("skip video", "video overslaan");
                Dictionary["Dutch"].Add("loading assets...", "middelen laden...");
                Dictionary["Dutch"].Add("loading player data…", "spelergegevens laden...");
                Dictionary["Dutch"].Add("loading scene...", "scène laden...");
                Dictionary["Dutch"].Add("starting…", "starten...");
                Dictionary["Dutch"].Add("PLAY", "SPELEN");
                Dictionary["Dutch"].Add("SHOP", "WINKEL");
                Dictionary["Dutch"].Add("RATE US", "BEOORDEEL ONS");
                Dictionary["Dutch"].Add("SELECTED", "GESELECTEERD");
                Dictionary["Dutch"].Add("OK", "OK");
                Dictionary["Dutch"].Add("Go Premium without Ads", "Word Premium zonder advertenties");
                Dictionary["Dutch"].Add("Demo levels completed Purchase full version to continue", "Demolevels voltooid Koop de volledige versie om door te gaan");
                Dictionary["Dutch"].Add("Connection Lost!", "Verbinding Verloren!");
                Dictionary["Dutch"].Add("Unable to load the game.", "Kan het spel niet laden.");
                Dictionary["Dutch"].Add("Please check your internet connection.", "Controleer je internetverbinding.");


                //TURKISH LANGUAGE

                Dictionary["Turkish"].Add("Knockdown", "Yıkılmak");
                Dictionary["Turkish"].Add("Bottles", "Şişeler");
                Dictionary["Turkish"].Add("Play", "Oynamak");
                Dictionary["Turkish"].Add("Select world", "Dünyayı seç");
                Dictionary["Turkish"].Add("Forest", "Orman");
                Dictionary["Turkish"].Add("Ancient", "Antik");
                Dictionary["Turkish"].Add("Mettle", "Cesaret");
                Dictionary["Turkish"].Add("Desert", "Çöl");
                Dictionary["Turkish"].Add("Snowy", "Karlı");
                Dictionary["Turkish"].Add("Level Cleared", "Seviye Geçildi");
                Dictionary["Turkish"].Add("Level Failed", "Seviye Başarısız");
                Dictionary["Turkish"].Add("Do not hit this bottle", "Bu şişeye vurma");
                Dictionary["Turkish"].Add("Pause", "Duraklat");
                Dictionary["Turkish"].Add("Achieve _ stars in forest world to unlock this world", "Bu dünyayı açmak için orman dünyasında _ yıldız elde et");
                Dictionary["Turkish"].Add("Achieve _ stars in ancient world to unlock this world", "Bu dünyayı açmak için antik dünyada _ yıldız elde et");
                Dictionary["Turkish"].Add("Achieve _ stars in mettle world to unlock this world", "Bu dünyayı açmak için cesaret dünyasında _ yıldız elde et");
                Dictionary["Turkish"].Add("Achieve _ stars in desert world to unlock this world", "Bu dünyayı açmak için çöl dünyasında _ yıldız elde et");
                Dictionary["Turkish"].Add("Achieve _ stars in snow world to unlock this world", "Bu dünyayı açmak için karlı dünyada _ yıldız elde et");
                Dictionary["Turkish"].Add("Exit", "Çıkış");
                Dictionary["Turkish"].Add("ARE YOU SURE..?", "EMİN MİSİN..?");
                Dictionary["Turkish"].Add("Loading...", "Yükleniyor...");
                Dictionary["Turkish"].Add("StorePanel", "Mağaza Paneli");
                Dictionary["Turkish"].Add("Select", "Seç");
                Dictionary["Turkish"].Add("Spin wheel", "Çarkı döndür");
                Dictionary["Turkish"].Add("Spin", "Döndür");
                Dictionary["Turkish"].Add("WATCH VIDEO TO SPIN", "DÖNDÜRMEK İÇİN VİDEO İZLE");
                Dictionary["Turkish"].Add("No Free Spins Available! Try Tomorrow", "Ücretsiz dönüş yok! Yarın tekrar deneyin");
                Dictionary["Turkish"].Add("Get 1000 coins", "1000 jeton al");
                Dictionary["Turkish"].Add("Menu", "Menü");
                Dictionary["Turkish"].Add("New game", "Yeni oyun");
                Dictionary["Turkish"].Add("Continue", "Devam et");
                Dictionary["Turkish"].Add("Settings", "Ayarlar");
                Dictionary["Turkish"].Add("Write a review", "İnceleme yaz");
                Dictionary["Turkish"].Add("Difficulty", "Zorluk");
                Dictionary["Turkish"].Add("Easy", "Kolay");
                Dictionary["Turkish"].Add("Medium", "Orta");
                Dictionary["Turkish"].Add("Hard", "Zor");
                Dictionary["Turkish"].Add("Effects volume", "Efekt sesi");
                Dictionary["Turkish"].Add("Music volume", "Müzik sesi");
                Dictionary["Turkish"].Add("Play time: {0:N1} h.", "Oynama süresi: {0:N1} sa.");
                Dictionary["Turkish"].Add("SKIP", "GEÇ");
                Dictionary["Turkish"].Add("premium without ads", "reklamsız premium");
                Dictionary["Turkish"].Add("100% Add free", "100% reklamsız");
                Dictionary["Turkish"].Add("skip video", "videoyu geç");
                Dictionary["Turkish"].Add("loading assets...", "varlıklar yükleniyor...");
                Dictionary["Turkish"].Add("loading player data…", "oyuncu verileri yükleniyor...");
                Dictionary["Turkish"].Add("loading scene...", "sahne yükleniyor...");
                Dictionary["Turkish"].Add("starting…", "başlıyor...");
                Dictionary["Turkish"].Add("PLAY", "OYNAMAK");
                Dictionary["Turkish"].Add("SHOP", "DÜKKAN");
                Dictionary["Turkish"].Add("RATE US", "BİZİ OYLA");
                Dictionary["Turkish"].Add("SELECTED", "SEÇİLDİ");
                Dictionary["Turkish"].Add("OK", "TAMAM");
                Dictionary["Turkish"].Add("Go Premium without Ads", "Reklamsız Premium'a geç");
                Dictionary["Turkish"].Add("Demo levels completed Purchase full version to continue", "Demo seviyeleri tamamlandı, devam etmek için tam sürümü satın alın");
                Dictionary["Turkish"].Add("Connection Lost!", "Bağlantı Kaybedildi!");
                Dictionary["Turkish"].Add("Unable to load the game.", "Oyunu yükleyemiyor.");
                Dictionary["Turkish"].Add("Please check your internet connection.", "Lütfen internet bağlantınızı kontrol edin.");



                //ITALIAN LANGUAGE

                Dictionary["Italian"].Add("Knockdown", "Abbattimento");
                Dictionary["Italian"].Add("Bottles", "Bottiglie");
                Dictionary["Italian"].Add("Play", "Giocare");
                Dictionary["Italian"].Add("Select world", "Seleziona il mondo");
                Dictionary["Italian"].Add("Forest", "Foresta");
                Dictionary["Italian"].Add("Ancient", "Antico");
                Dictionary["Italian"].Add("Mettle", "Coraggio");
                Dictionary["Italian"].Add("Desert", "Deserto");
                Dictionary["Italian"].Add("Snowy", "Nevoso");
                Dictionary["Italian"].Add("Level Cleared", "Livello Superato");
                Dictionary["Italian"].Add("Level Failed", "Livello Fallito");
                Dictionary["Italian"].Add("Do not hit this bottle", "Non colpire questa bottiglia");
                Dictionary["Italian"].Add("Pause", "Pausa");
                Dictionary["Italian"].Add("Achieve _ stars in forest world to unlock this world", "Raggiungi _ stelle nel mondo della foresta per sbloccare questo mondo");
                Dictionary["Italian"].Add("Achieve _ stars in ancient world to unlock this world", "Raggiungi _ stelle nel mondo antico per sbloccare questo mondo");
                Dictionary["Italian"].Add("Achieve _ stars in mettle world to unlock this world", "Raggiungi _ stelle nel mondo del coraggio per sbloccare questo mondo");
                Dictionary["Italian"].Add("Achieve _ stars in desert world to unlock this world", "Raggiungi _ stelle nel mondo del deserto per sbloccare questo mondo");
                Dictionary["Italian"].Add("Achieve _ stars in snow world to unlock this world", "Raggiungi _ stelle nel mondo nevoso per sbloccare questo mondo");
                Dictionary["Italian"].Add("Exit", "Uscita");
                Dictionary["Italian"].Add("ARE YOU SURE..?", "SEI SICURO..?");
                Dictionary["Italian"].Add("Loading...", "Caricamento...");
                Dictionary["Italian"].Add("StorePanel", "Pannello del negozio");
                Dictionary["Italian"].Add("Select", "Selezionare");
                Dictionary["Italian"].Add("Spin wheel", "Gira la ruota");
                Dictionary["Italian"].Add("Spin", "Gira");
                Dictionary["Italian"].Add("WATCH VIDEO TO SPIN", "GUARDA IL VIDEO PER GIRARE");
                Dictionary["Italian"].Add("No Free Spins Available! Try Tomorrow", "Nessun giro gratuito disponibile! Prova domani");
                Dictionary["Italian"].Add("Get 1000 coins", "Ottieni 1000 monete");
                Dictionary["Italian"].Add("Menu", "Menu");
                Dictionary["Italian"].Add("New game", "Nuovo gioco");
                Dictionary["Italian"].Add("Continue", "Continua");
                Dictionary["Italian"].Add("Settings", "Impostazioni");
                Dictionary["Italian"].Add("Write a review", "Scrivi una recensione");
                Dictionary["Italian"].Add("Difficulty", "Difficoltà");
                Dictionary["Italian"].Add("Easy", "Facile");
                Dictionary["Italian"].Add("Medium", "Medio");
                Dictionary["Italian"].Add("Hard", "Difficile");
                Dictionary["Italian"].Add("Effects volume", "Volume effetti");
                Dictionary["Italian"].Add("Music volume", "Volume della musica");
                Dictionary["Italian"].Add("Play time: {0:N1} h.", "Tempo di gioco: {0:N1} h.");
                Dictionary["Italian"].Add("SKIP", "SALTA");
                Dictionary["Italian"].Add("premium without ads", "premium senza annunci");
                Dictionary["Italian"].Add("100% Add free", "100% senza annunci");
                Dictionary["Italian"].Add("skip video", "salta video");
                Dictionary["Italian"].Add("loading assets...", "caricamento risorse...");
                Dictionary["Italian"].Add("loading player data…", "caricamento dati giocatore...");
                Dictionary["Italian"].Add("loading scene...", "caricamento scena...");
                Dictionary["Italian"].Add("starting…", "avvio...");
                Dictionary["Italian"].Add("PLAY", "GIOCARE");
                Dictionary["Italian"].Add("SHOP", "NEGOZIO");
                Dictionary["Italian"].Add("RATE US", "VALUTACI");
                Dictionary["Italian"].Add("SELECTED", "SELEZIONATO");
                Dictionary["Italian"].Add("OK", "OK");
                Dictionary["Italian"].Add("Go Premium without Ads", "Passa a Premium senza annunci");
                Dictionary["Italian"].Add("Demo levels completed Purchase full version to continue", "Livelli dimostrativi completati Acquista la versione completa per continuare");
                Dictionary["Italian"].Add("Connection Lost!", "Connessione persa!");
                Dictionary["Italian"].Add("Unable to load the game.", "Impossibile caricare il gioco.");
                Dictionary["Italian"].Add("Please check your internet connection.", "Si prega di controllare la connessione a Internet.");


                //RUSSIAN LANGUAGE

                Dictionary["Russian"].Add("Knockdown", "Нокаут");
                Dictionary["Russian"].Add("Bottles", "Бутылки");
                Dictionary["Russian"].Add("Play", "Играть");
                Dictionary["Russian"].Add("Select world", "Выбрать мир");
                Dictionary["Russian"].Add("Forest", "Лес");
                Dictionary["Russian"].Add("Ancient", "Древний");
                Dictionary["Russian"].Add("Mettle", "Мужество");
                Dictionary["Russian"].Add("Desert", "Пустыня");
                Dictionary["Russian"].Add("Snowy", "Снежный");
                Dictionary["Russian"].Add("Level Cleared", "Уровень пройден");
                Dictionary["Russian"].Add("Level Failed", "Уровень не пройден");
                Dictionary["Russian"].Add("Do not hit this bottle", "Не бейте эту бутылку");
                Dictionary["Russian"].Add("Pause", "Пауза");
                Dictionary["Russian"].Add("Achieve _ stars in forest world to unlock this world", "Достигните _ звёзд в лесном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].Add("Achieve _ stars in ancient world to unlock this world", "Достигните _ звёзд в древнем мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].Add("Achieve _ stars in mettle world to unlock this world", "Достигните _ звёзд в мире мужества, чтобы разблокировать этот мир");
                Dictionary["Russian"].Add("Achieve _ stars in desert world to unlock this world", "Достигните _ звёзд в пустынном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].Add("Achieve _ stars in snow world to unlock this world", "Достигните _ звёзд в снежном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].Add("Exit", "Выход");
                Dictionary["Russian"].Add("ARE YOU SURE..?", "ВЫ УВЕРЕНЫ..?");
                Dictionary["Russian"].Add("Loading...", "Загрузка...");
                Dictionary["Russian"].Add("StorePanel", "Панель магазина");
                Dictionary["Russian"].Add("Select", "Выбрать");
                Dictionary["Russian"].Add("Spin wheel", "Крутить колесо");
                Dictionary["Russian"].Add("Spin", "Крутить");
                Dictionary["Russian"].Add("WATCH VIDEO TO SPIN", "СМОТРЕТЬ ВИДЕО ЧТОБЫ КРУТИТЬ");
                Dictionary["Russian"].Add("No Free Spins Available! Try Tomorrow", "Бесплатных вращений нет! Попробуйте завтра");
                Dictionary["Russian"].Add("Get 1000 coins", "Получить 1000 монет");
                Dictionary["Russian"].Add("Menu", "Меню");
                Dictionary["Russian"].Add("New game", "Новая игра");
                Dictionary["Russian"].Add("Continue", "Продолжить");
                Dictionary["Russian"].Add("Settings", "Настройки");
                Dictionary["Russian"].Add("Write a review", "Написать отзыв");
                Dictionary["Russian"].Add("Difficulty", "Сложность");
                Dictionary["Russian"].Add("Easy", "Легко");
                Dictionary["Russian"].Add("Medium", "Средне");
                Dictionary["Russian"].Add("Hard", "Трудно");
                Dictionary["Russian"].Add("Effects volume", "Громкость эффектов");
                Dictionary["Russian"].Add("Music volume", "Громкость музыки");
                Dictionary["Russian"].Add("Play time: {0:N1} h.", "Время игры: {0:N1} ч.");
                Dictionary["Russian"].Add("SKIP", "ПРОПУСТИТЬ");
                Dictionary["Russian"].Add("premium without ads", "премиум без рекламы");
                Dictionary["Russian"].Add("100% Add free", "100% без рекламы");
                Dictionary["Russian"].Add("skip video", "пропустить видео");
                Dictionary["Russian"].Add("loading assets...", "загрузка ресурсов...");
                Dictionary["Russian"].Add("loading player data…", "загрузка данных игрока...");
                Dictionary["Russian"].Add("loading scene...", "загрузка сцены...");
                Dictionary["Russian"].Add("starting…", "запуск...");
                Dictionary["Russian"].Add("PLAY", "ИГРАТЬ");
                Dictionary["Russian"].Add("SHOP", "МАГАЗИН");
                Dictionary["Russian"].Add("RATE US", "ОЦЕНИТЕ НАС");
                Dictionary["Russian"].Add("SELECTED", "ВЫБРАНО");
                Dictionary["Russian"].Add("OK", "ОК");
                Dictionary["Russian"].Add("Go Premium without Ads", "Перейдите на Премиум без рекламы");
                Dictionary["Russian"].Add("Demo levels completed Purchase full version to continue", "Демо-уровни завершены Купите полную версию чтобы продолжить");
                Dictionary["Russian"].Add("Connection Lost!", "Соединение потеряно!");
                Dictionary["Russian"].Add("Unable to load the game.", "Не удалось загрузить игру.");
                Dictionary["Russian"].Add("Please check your internet connection.", "Пожалуйста, проверьте ваше интернет-соединение.");


                //JAPANESE LANGUAGE


                Dictionary["Japanese"].Add("Knockdown", "ノックダウン");
                Dictionary["Japanese"].Add("Bottles", "ボトル");
                Dictionary["Japanese"].Add("Play", "プレイ");
                Dictionary["Japanese"].Add("Select world", "世界を選択");
                Dictionary["Japanese"].Add("Forest", "森");
                Dictionary["Japanese"].Add("Ancient", "古代");
                Dictionary["Japanese"].Add("Mettle", "勇気");
                Dictionary["Japanese"].Add("Desert", "砂漠");
                Dictionary["Japanese"].Add("Snowy", "雪");
                Dictionary["Japanese"].Add("Level Cleared", "レベルクリア");
                Dictionary["Japanese"].Add("Level Failed", "レベル失敗");
                Dictionary["Japanese"].Add("Do not hit this bottle", "このボトルを叩かないでください");
                Dictionary["Japanese"].Add("Pause", "一時停止");
                Dictionary["Japanese"].Add("Achieve _ stars in forest world to unlock this world", "この世界をアンロックするために、森の世界で _ つの星を達成してください");
                Dictionary["Japanese"].Add("Achieve _ stars in ancient world to unlock this world", "この世界をアンロックするために、古代の世界で _ つの星を達成してください");
                Dictionary["Japanese"].Add("Achieve _ stars in mettle world to unlock this world", "この世界をアンロックするために、勇気の世界で _ つの星を達成してください");
                Dictionary["Japanese"].Add("Achieve _ stars in desert world to unlock this world", "この世界をアンロックするために、砂漠の世界で _ つの星を達成してください");
                Dictionary["Japanese"].Add("Achieve _ stars in snow world to unlock this world", "この世界をアンロックするために、雪の世界で _ つの星を達成してください");
                Dictionary["Japanese"].Add("Exit", "出口");
                Dictionary["Japanese"].Add("ARE YOU SURE..?", "本当に..?");
                Dictionary["Japanese"].Add("Loading...", "読み込み中...");
                Dictionary["Japanese"].Add("StorePanel", "ストアパネル");
                Dictionary["Japanese"].Add("Select", "選択");
                Dictionary["Japanese"].Add("Spin wheel", "ホイールを回す");
                Dictionary["Japanese"].Add("Spin", "回す");
                Dictionary["Japanese"].Add("WATCH VIDEO TO SPIN", "回すためにビデオを見る");
                Dictionary["Japanese"].Add("No Free Spins Available! Try Tomorrow", "無料のスピンはありません！ 明日もう一度お試しください");
                Dictionary["Japanese"].Add("Get 1000 coins", "1000枚のコインを取得する");
                Dictionary["Japanese"].Add("Menu", "メニュー");
                Dictionary["Japanese"].Add("New game", "新しいゲーム");
                Dictionary["Japanese"].Add("Continue", "続ける");
                Dictionary["Japanese"].Add("Settings", "設定");
                Dictionary["Japanese"].Add("Write a review", "レビューを書く");
                Dictionary["Japanese"].Add("Difficulty", "難易度");
                Dictionary["Japanese"].Add("Easy", "簡単");
                Dictionary["Japanese"].Add("Medium", "中程度");
                Dictionary["Japanese"].Add("Hard", "難しい");
                Dictionary["Japanese"].Add("Effects volume", "エフェクトの音量");
                Dictionary["Japanese"].Add("Music volume", "音楽の音量");
                Dictionary["Japanese"].Add("Play time: {0:N1} h.", "プレイ時間: {0:N1} 時間。");
                Dictionary["Japanese"].Add("SKIP", "スキップ");
                Dictionary["Japanese"].Add("premium without ads", "広告なしのプレミアム");
                Dictionary["Japanese"].Add("100% Add free", "100% 広告なし");
                Dictionary["Japanese"].Add("skip video", "ビデオをスキップ");
                Dictionary["Japanese"].Add("loading assets...", "アセットの読み込み中...");
                Dictionary["Japanese"].Add("loading player data…", "プレイヤーデータの読み込み中...");
                Dictionary["Japanese"].Add("loading scene...", "シーンの読み込み中...");
                Dictionary["Japanese"].Add("starting…", "開始...");
                Dictionary["Japanese"].Add("PLAY", "プレイ");
                Dictionary["Japanese"].Add("SHOP", "ショップ");
                Dictionary["Japanese"].Add("RATE US", "評価する");
                Dictionary["Japanese"].Add("SELECTED", "選択された");
                Dictionary["Japanese"].Add("OK", "OK");
                Dictionary["Japanese"].Add("Go Premium without Ads", "広告なしのプレミアムに移行");
                Dictionary["Japanese"].Add("Demo levels completed Purchase full version to continue", "デモレベルが完了しました 続行するには完全版を購入してください");
                Dictionary["Japanese"].Add("Connection Lost!", "接続が失われました！");
                Dictionary["Japanese"].Add("Unable to load the game.", "ゲームを読み込めません。");
                Dictionary["Japanese"].Add("Please check your internet connection.", "インターネット接続を確認してください。");


                //CHINESE LANGUAGE

                Dictionary["Chinese"].Add("Knockdown", "击倒");
                Dictionary["Chinese"].Add("Bottles", "瓶子");
                Dictionary["Chinese"].Add("Play", "玩");
                Dictionary["Chinese"].Add("Select world", "选择世界");
                Dictionary["Chinese"].Add("Forest", "森林");
                Dictionary["Chinese"].Add("Ancient", "古老");
                Dictionary["Chinese"].Add("Mettle", "勇气");
                Dictionary["Chinese"].Add("Desert", "沙漠");
                Dictionary["Chinese"].Add("Snowy", "多雪的");
                Dictionary["Chinese"].Add("Level Cleared", "关卡完成");
                Dictionary["Chinese"].Add("Level Failed", "关卡失败");
                Dictionary["Chinese"].Add("Do not hit this bottle", "不要打这个瓶子");
                Dictionary["Chinese"].Add("Pause", "暂停");
                Dictionary["Chinese"].Add("Achieve _ stars in forest world to unlock this world", "在森林世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].Add("Achieve _ stars in ancient world to unlock this world", "在古老世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].Add("Achieve _ stars in mettle world to unlock this world", "在勇气世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].Add("Achieve _ stars in desert world to unlock this world", "在沙漠世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].Add("Achieve _ stars in snow world to unlock this world", "在多雪的世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].Add("Exit", "退出");
                Dictionary["Chinese"].Add("ARE YOU SURE..?", "你确定吗..?");
                Dictionary["Chinese"].Add("Loading...", "加载中...");
                Dictionary["Chinese"].Add("StorePanel", "商店面板");
                Dictionary["Chinese"].Add("Select", "选择");
                Dictionary["Chinese"].Add("Spin wheel", "旋转轮盘");
                Dictionary["Chinese"].Add("Spin", "旋转");
                Dictionary["Chinese"].Add("WATCH VIDEO TO SPIN", "观看视频以旋转");
                Dictionary["Chinese"].Add("No Free Spins Available! Try Tomorrow", "没有免费的旋转！ 请明天再试");
                Dictionary["Chinese"].Add("Get 1000 coins", "获得1000金币");
                Dictionary["Chinese"].Add("Menu", "菜单");
                Dictionary["Chinese"].Add("New game", "新游戏");
                Dictionary["Chinese"].Add("Continue", "继续");
                Dictionary["Chinese"].Add("Settings", "设置");
                Dictionary["Chinese"].Add("Write a review", "写评论");
                Dictionary["Chinese"].Add("Difficulty", "难度");
                Dictionary["Chinese"].Add("Easy", "简单");
                Dictionary["Chinese"].Add("Medium", "中等");
                Dictionary["Chinese"].Add("Hard", "难");
                Dictionary["Chinese"].Add("Effects volume", "效果音量");
                Dictionary["Chinese"].Add("Music volume", "音乐音量");
                Dictionary["Chinese"].Add("Play time: {0:N1} h.", "游戏时间：{0:N1}小时。");
                Dictionary["Chinese"].Add("SKIP", "跳过");
                Dictionary["Chinese"].Add("premium without ads", "无广告高级版");
                Dictionary["Chinese"].Add("100% Add free", "100%无广告");
                Dictionary["Chinese"].Add("skip video", "跳过视频");
                Dictionary["Chinese"].Add("loading assets...", "加载资源...");
                Dictionary["Chinese"].Add("loading player data…", "加载玩家数据...");
                Dictionary["Chinese"].Add("loading scene...", "加载场景...");
                Dictionary["Chinese"].Add("starting…", "开始...");
                Dictionary["Chinese"].Add("PLAY", "玩");
                Dictionary["Chinese"].Add("SHOP", "商店");
                Dictionary["Chinese"].Add("RATE US", "评价我们");
                Dictionary["Chinese"].Add("SELECTED", "选择");
                Dictionary["Chinese"].Add("OK", "好");
                Dictionary["Chinese"].Add("Go Premium without Ads", "无广告高级版");
                Dictionary["Chinese"].Add("Demo levels completed Purchase full version to continue", "演示关卡完成 购买完整版以继续");
                Dictionary["Chinese"].Add("Connection Lost!", "连接丢失！");
                Dictionary["Chinese"].Add("Unable to load the game.", "无法加载游戏。");
                Dictionary["Chinese"].Add("Please check your internet connection.", "请检查您的互联网连接。");




                AutoLanguage();
            }
            catch (Exception exp)
            {
                try
                {
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                    var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                    var lineNumber = stackFrame.GetFileLineNumber();
                    string errorline = "Line:" + lineNumber;
                    if (lineNumber == 0)
                    {
                        int index = exp.ToString().IndexOf("at");
                        int length = exp.ToString().Substring(index).Length;
                        if (length > 99)
                        {
                            errorline = "Line:" + exp.ToString().Substring(index, 100);
                        }
                        else
                        {
                            errorline = "Line2:" + exp.ToString().Substring(index);
                        }
                    }
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "LM_Read", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
        }

        public static bool HasKey(string localizationKey)
        {
            return Dictionary[Language].ContainsKey(localizationKey);
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey)
        {
            try
            {
                if (Dictionary.Count == 0)
                {
                    Read();
                }

                //if (!Dictionary.ContainsKey(Language)) throw new KeyNotFoundException("Language not found: " + Language);
                if (!Dictionary.ContainsKey(Language)) Language = "English";

                //if (!Dictionary[Language].ContainsKey(localizationKey)) throw new KeyNotFoundException("Translation not found: " + localizationKey);

                if (Language == "English") return Dictionary[Language][localizationKey];

                var missed = !Dictionary[Language].ContainsKey(localizationKey) || string.IsNullOrEmpty(Dictionary[Language][localizationKey]);

                if (missed)
                {
                    Debug.LogWarningFormat("Translation not found: {localizationKey} ({0}).", Language);

                    return Dictionary["English"].ContainsKey(localizationKey) ? Dictionary["English"][localizationKey] : localizationKey;
                }

                return Dictionary[Language][localizationKey];
            }
            catch (Exception exp)
            {
                try
                {
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                    var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                    var lineNumber = stackFrame.GetFileLineNumber();
                    string errorline = "Line:" + lineNumber;
                    if (lineNumber == 0)
                    {
                        int index = exp.ToString().IndexOf("at");
                        int length = exp.ToString().Substring(index).Length;
                        if (length > 99)
                        {
                            errorline = "Line:" + exp.ToString().Substring(index, 100);
                        }
                        else
                        {
                            errorline = "Line2:" + exp.ToString().Substring(index);
                        }
                    }
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "LM_localise", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
                return null;
            }
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }

        public static string GetChars()
        {
            try
            {
                var asset = Resources.Load<TextAsset>("Localization/Common");

                if (asset == null) return "";

                var chars = new List<char>();

                foreach (var s in "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZАаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~Ξ ") if (!chars.Contains(s)) chars.Add(s);
                foreach (var s in asset.text)
                {
                    if (!chars.Contains(char.ToLower(s))) chars.Add(char.ToLower(s));
                    if (!chars.Contains(char.ToUpper(s))) chars.Add(char.ToUpper(s));
                }

                chars.Sort();

                var text = new System.Text.StringBuilder();

                foreach (var s in chars) text.Append(s);

                return text.ToString();
            }
            catch (Exception exp)
            {
                try
                {
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                    var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                    var lineNumber = stackFrame.GetFileLineNumber();
                    string errorline = "Line:" + lineNumber;
                    if (lineNumber == 0)
                    {
                        int index = exp.ToString().IndexOf("at");
                        int length = exp.ToString().Substring(index).Length;
                        if (length > 99)
                        {
                            errorline = "Line:" + exp.ToString().Substring(index, 100);
                        }
                        else
                        {
                            errorline = "Line2:" + exp.ToString().Substring(index);
                        }
                    }
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "LM_GetChars", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
                return null;
            }
        }

        private static string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }
    }
}