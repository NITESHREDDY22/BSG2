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
        public static void Read()
        {
            try
            {

                //if (Dictionary.Count > 0) 
                //    return;
                /*
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
                            Dictionary.TryAdd(languages[i], new Dictionary<string, string>());
                        }
                    }

                    for (var i = 1; i < lines.Length; i++)
                    {
                        var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[comma]", ",").Replace("[newline]", "\n").Replace("[quotes]", "\"")).ToList();
                        var key = columns[0];

                        if (key == "") continue;

                        for (var j = 1; j < languages.Count; j++)
                        {
                            Dictionary[languages[j]].TryAdd(key, columns[j]);
                        }
                    }
                }
                */

                //ENGLISH LANGUAGE
              
                AddLanguageKeys("English");
                AddLanguageKeys("Arabic");
                AddLanguageKeys("Spanish");
                AddLanguageKeys("German");
                AddLanguageKeys("French");
                AddLanguageKeys("Dutch");
                AddLanguageKeys("Polish");
                AddLanguageKeys("Japanese");
                AddLanguageKeys("Chinese");
                AddLanguageKeys("Italian");
                AddLanguageKeys("Portuguese");
                AddLanguageKeys("Turkish");
                AddLanguageKeys("Russian");


                Dictionary["English"].TryAdd("Knockdown", "Knockdown");
                Dictionary["English"].TryAdd("Bottles", "Bottles");
                Dictionary["English"].TryAdd("Play", "Play");
                Dictionary["English"].TryAdd("Select world", "Select world");
                Dictionary["English"].TryAdd("Forest", "Forest");
                Dictionary["English"].TryAdd("Ancient", "Ancient");
                Dictionary["English"].TryAdd("Mettle", "Mettle");
                Dictionary["English"].TryAdd("Desert", "Desert");
                Dictionary["English"].TryAdd("Snowy", "Snowy");
                Dictionary["English"].TryAdd("WinPanel.Text", "Level Cleared");
                Dictionary["English"].TryAdd("FailPanel.Text", "Level Failed");
                Dictionary["English"].TryAdd("TutorialText", "Do not hit this bottle");
                Dictionary["English"].TryAdd("PausePanel.Text", "Pause");
                Dictionary["English"].TryAdd("Achieve _ stars in forest world to unlock this world", "Achieve _ stars in forest world to unlock this world");
                Dictionary["English"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Achieve _ stars in ancient world to unlock this world");
                Dictionary["English"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Achieve _ stars in mettle world to unlock this world");
                Dictionary["English"].TryAdd("Achieve _ stars in desert world to unlock this world", "Achieve _ stars in desert world to unlock this world");
                Dictionary["English"].TryAdd("Achieve _ stars in snow world to unlock this world", "Achieve _ stars in snow world to unlock this world");
                Dictionary["English"].TryAdd("Exit.Title", "Exit");
                Dictionary["English"].TryAdd("Exit.ConfirmText", "ARE YOU SURE..?");
                Dictionary["English"].TryAdd("HidePanel.Text", "Loading...");
                Dictionary["English"].TryAdd("StorePanel.text", "StorePanel");
                Dictionary["English"].TryAdd("Select", "Select");
                Dictionary["English"].TryAdd("SpinWheel.text", "Spin wheel");
                Dictionary["English"].TryAdd("Spin", "Spin");
                Dictionary["English"].TryAdd("Watchvideo.Text", "WATCH VIDEO\r\n TO SPIN");
                Dictionary["English"].TryAdd("Nospin.text", "No Free Spins Available! Try Tomorrow");
                Dictionary["English"].TryAdd("GetCoins", "GetCoins");
                Dictionary["English"].TryAdd("Menu", "Menu");
                Dictionary["English"].TryAdd("New game", "New game");
                Dictionary["English"].TryAdd("Continue", "Continue");
                Dictionary["English"].TryAdd("Settings", "Settings");
                Dictionary["English"].TryAdd("Write a review", "Write a review");
                Dictionary["English"].TryAdd("Difficulty", "Difficulty");
                Dictionary["English"].TryAdd("Easy", "Easy");
                Dictionary["English"].TryAdd("Medium", "Medium");
                Dictionary["English"].TryAdd("Hard", "Hard");
                Dictionary["English"].TryAdd("Effects volume", "Effects volume");
                Dictionary["English"].TryAdd("Music volume", "Music volume");
                Dictionary["English"].TryAdd("Play time: {0:N1} h.", "Play time: {0:N1} h.");
                Dictionary["English"].TryAdd("SKIP", "SKIP");
                Dictionary["English"].TryAdd("premium without ads", "premium without ads");
                Dictionary["English"].TryAdd("100% Add free", "100% Add free");
                Dictionary["English"].TryAdd("skip video", "skip video");
                Dictionary["English"].TryAdd("loading assets...", "loading assets...");
                Dictionary["English"].TryAdd("loading player data…", "loading player data…");
                Dictionary["English"].TryAdd("loading scene...", "loading scene...");
                Dictionary["English"].TryAdd("starting…", "starting…");
                Dictionary["English"].TryAdd("PLAY", "PLAY");
                Dictionary["English"].TryAdd("SHOP", "SHOP");
                Dictionary["English"].TryAdd("RATE US", "RATE US");
                Dictionary["English"].TryAdd("SELECTED", "SELECTED");
                Dictionary["English"].TryAdd("OK", "OK");
                Dictionary["English"].TryAdd("Go Premium without Ads", "Go Premium without Ads");
                Dictionary["English"].TryAdd("Demo levels completed Purchase full version to continue", "Demo levels completed Purchase full version to continue");
                Dictionary["English"].TryAdd("Connection Lost!", "Connection Lost");
                Dictionary["English"].TryAdd("Unable to load the game.", "Unable to load the game.");
                Dictionary["English"].TryAdd("Please check your internet connection.", "Please check your internet connection.");
                Dictionary["English"].TryAdd("Try again", "Try again");



                //ARABIC LANGUAGE

                Dictionary["Arabic"].TryAdd("Knockdown", "ضربه قاضيه");
                Dictionary["Arabic"].TryAdd("Bottles", "زجاجات");
                Dictionary["Arabic"].TryAdd("Play", "لعب");
                Dictionary["Arabic"].TryAdd("Select world", "حدد العالم");
                Dictionary["Arabic"].TryAdd("Forest", "غابة");
                Dictionary["Arabic"].TryAdd("Ancient", "قديم");
                Dictionary["Arabic"].TryAdd("Mettle", "همه");
                Dictionary["Arabic"].TryAdd("Desert", "صحراء");
                Dictionary["Arabic"].TryAdd("Snowy", "ثلجي");
                Dictionary["Arabic"].TryAdd("WinPanel.Text", "تم مسح المستوى");
                Dictionary["Arabic"].TryAdd("FailPanel.Text", "فشل المستوى");
                Dictionary["Arabic"].TryAdd("TutorialText", "لا تضرب هذه الزجاجة");
                Dictionary["Arabic"].TryAdd("PausePanel.Text", " توقف");
                Dictionary["Arabic"].TryAdd("Achieve _ stars in forest world to unlock this world ", " احصل على _ نجوم في عالم الغابة لفتح هذا العالم");
                Dictionary["Arabic"].TryAdd("Achieve _ stars in ancient world to unlock this world ", "احصل على _ نجوم في العالم القديم لفتح هذا العالم");
                Dictionary["Arabic"].TryAdd("Achieve _ stars in mettle world to unlock this world  ", "احصل على _ نجوم في عالم الشجاعة لفتح هذا العالم");
                Dictionary["Arabic"].TryAdd("Achieve _ stars in desert world to unlock this world", " احصل على _ نجوم في عالم الصحراء لفتح هذا العالم");
                Dictionary["Arabic"].TryAdd("Achieve _ stars in snow world to unlock this world", " احصل على _ نجوم في عالم الثلج لفتح هذا العالم");
                Dictionary["Arabic"].TryAdd("Exit.Title", "مخرج");
                Dictionary["Arabic"].TryAdd("Exit.ConfirmText", "هل أنت متأكد..؟");
                Dictionary["Arabic"].TryAdd("HidePanel.Text", "تحميل...");
                Dictionary["Arabic"].TryAdd("StorePanel.text", "لوحة التخزين");
                Dictionary["Arabic"].TryAdd("Select", "اختار");
                Dictionary["Arabic"].TryAdd("SpinWheel.text", "عجلة الدوران");
                Dictionary["Arabic"].TryAdd("Spin", "ردن");
                Dictionary["Arabic"].TryAdd("Watchvideo.Text" , " شاهد الفيديو للدوران");
                Dictionary["Arabic"].TryAdd("Nospin.text", "لا توجد دورات مجانية متاحة! جرب الغد");
                Dictionary["Arabic"].TryAdd("GetCoins", "احصل على 1000 قطعة نقدية");
                Dictionary["Arabic"].TryAdd("Menu", "قائمة");
                Dictionary["Arabic"].TryAdd("New game", "لعبة جديدة");
                Dictionary["Arabic"].TryAdd("Continue", "استمر");
                Dictionary["Arabic"].TryAdd("Settings", "اعدادات");
                Dictionary["Arabic"].TryAdd("Write a review", "اكتب تعليقا");
                Dictionary["Arabic"].TryAdd("Settings", "اعدادات");
                Dictionary["Arabic"].TryAdd("Difficulty", "صعوبة");
                Dictionary["Arabic"].TryAdd("Easy", "سهل");
                Dictionary["Arabic"].TryAdd("Medium", "متوسط");
                Dictionary["Arabic"].TryAdd("Hard", "شاق");
                Dictionary["Arabic"].TryAdd("Effects volume", "حجم التأثيرات");
                Dictionary["Arabic"].TryAdd("Music volume", "حجم الموسيقى");
                Dictionary["Arabic"].TryAdd("Play time: {0:N1} h.", "وقت اللعب: {0:N1} ساعة.");
                Dictionary["Arabic"].TryAdd("SKIP", "تخطي");
                Dictionary["Arabic"].TryAdd("premium without ads", "بريميوم بدون اعلانات");
                Dictionary["Arabic"].TryAdd("100% Add free", "100٪ إضافة مجانية");
                Dictionary["Arabic"].TryAdd("skip video", "تخطي الفيديو");
                Dictionary["Arabic"].TryAdd("loading assets...", "تحميل الأصول...");
                Dictionary["Arabic"].TryAdd("loading player data…", "تحميل بيانات المشغل...");
                Dictionary["Arabic"].TryAdd("loading scene...", "مشهد التحميل ...");
                Dictionary["Arabic"].TryAdd("starting…", "بدء...");
                Dictionary["Arabic"].TryAdd("PLAY", "لعب");
                Dictionary["Arabic"].TryAdd("SHOP", "دكان");
                Dictionary["Arabic"].TryAdd("RATE US", "قيمنا");
                Dictionary["Arabic"].TryAdd("SELECTED", "المحدد");
                Dictionary["Arabic"].TryAdd("OK", "موافق");
                Dictionary["Arabic"].TryAdd("Go Premium without Ads ", "احصل على النسخة المميزة بدون إعلانات");
                Dictionary["Arabic"].TryAdd("Demo levels completed Purchase full version to continue", "تم إكمال مستويات العرض، اشترِ النسخة الكاملة للمتابعة");
                Dictionary["Arabic"].TryAdd("Connection Lost!", "فقد الاتصال!");
                Dictionary["Arabic"].TryAdd("Unable to load the game.", "غير قادر على تحميل اللعبة.");
                Dictionary["Arabic"].TryAdd("Please check your internet connection.", "يرجى التحقق من اتصالك بالإنترنت.");
                Dictionary["Arabic"].TryAdd("Try again", "حاول ثانية");


                //SPANISH LANGUAGE

                Dictionary["Spanish"].TryAdd("Knockdown", "Derribar");
                Dictionary["Spanish"].TryAdd("Bottles", "Botellas");
                Dictionary["Spanish"].TryAdd("Play", "Jugar");
                Dictionary["Spanish"].TryAdd("Select world", "Seleccionar mundo");
                Dictionary["Spanish"].TryAdd("Forest", "Bosque");
                Dictionary["Spanish"].TryAdd("Ancient", "Antiguo");
                Dictionary["Spanish"].TryAdd("Mettle", "Coraje");
                Dictionary["Spanish"].TryAdd("Desert", "Desierto");
                Dictionary["Spanish"].TryAdd("Snowy", "Nevado");
                Dictionary["Spanish"].TryAdd("WinPanel.Text", "Nivel Completado");
                Dictionary["Spanish"].TryAdd("FailPanel.Text", "Nivel Fallido");
                Dictionary["Spanish"].TryAdd("TutorialText", "No golpees esta botella");
                Dictionary["Spanish"].TryAdd("PausePanel.Text", "Pausa");
                Dictionary["Spanish"].TryAdd("Achieve _ stars in forest world to unlock this world", "Logra _ estrellas en el mundo del bosque para desbloquear este mundo");
                Dictionary["Spanish"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Logra _ estrellas en el mundo antiguo para desbloquear este mundo");
                Dictionary["Spanish"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Logra _ estrellas en el mundo del coraje para desbloquear este mundo");
                Dictionary["Spanish"].TryAdd("Achieve _ stars in desert world to unlock this world", "Logra _ estrellas en el mundo del desierto para desbloquear este mundo");
                Dictionary["Spanish"].TryAdd("Achieve _ stars in snow world to unlock this world", "Logra _ estrellas en el mundo nevado para desbloquear este mundo");
                Dictionary["Spanish"].TryAdd("Exit.Title", "Salir");
                Dictionary["Spanish"].TryAdd("Exit.ConfirmText", "¿ESTÁS SEGURO..?");
                Dictionary["Spanish"].TryAdd("HidePanel.Text", "Cargando...");
                Dictionary["Spanish"].TryAdd("StorePanel.text", "Panel de la tienda");
                Dictionary["Spanish"].TryAdd("Select", "Seleccionar");
                Dictionary["Spanish"].TryAdd("SpinWheel.text", "Girar rueda");
                Dictionary["Spanish"].TryAdd("Spin", "Girar");
                Dictionary["Spanish"].TryAdd("Watchvideo.Text", "VER VIDEO PARA GIRAR");
                Dictionary["Spanish"].TryAdd("Nospin.text", "¡No hay giros gratis disponibles! Inténtalo mañana");
                Dictionary["Spanish"].TryAdd("GetCoins", "Obtén 1000 monedas");
                Dictionary["Spanish"].TryAdd("Menu", "Menú");
                Dictionary["Spanish"].TryAdd("New game", "Nuevo juego");
                Dictionary["Spanish"].TryAdd("Continue", "Continuar");
                Dictionary["Spanish"].TryAdd("Settings", "Configuración");
                Dictionary["Spanish"].TryAdd("Write a review", "Escribir una reseña");
                Dictionary["Spanish"].TryAdd("Difficulty", "Dificultad");
                Dictionary["Spanish"].TryAdd("Easy", "Fácil");
                Dictionary["Spanish"].TryAdd("Medium", "Medio");
                Dictionary["Spanish"].TryAdd("Hard", "Difícil");
                Dictionary["Spanish"].TryAdd("Effects volume", "Volumen de efectos");
                Dictionary["Spanish"].TryAdd("Music volume", "Volumen de música");
                Dictionary["Spanish"].TryAdd("Play time: {0:N1} h.", "Tiempo de juego: {0:N1} h.");
                Dictionary["Spanish"].TryAdd("SKIP", "SALTAR");
                Dictionary["Spanish"].TryAdd("premium without ads", "Premium sin anuncios");
                Dictionary["Spanish"].TryAdd("100% Add free", "100% sin anuncios");
                Dictionary["Spanish"].TryAdd("skip video", "saltar video");
                Dictionary["Spanish"].TryAdd("loading assets...", "cargando recursos...");
                Dictionary["Spanish"].TryAdd("loading player data…", "cargando datos del jugador...");
                Dictionary["Spanish"].TryAdd("loading scene...", "cargando escena...");
                Dictionary["Spanish"].TryAdd("starting…", "iniciando...");
                Dictionary["Spanish"].TryAdd("PLAY", "JUGAR");
                Dictionary["Spanish"].TryAdd("SHOP", "TIENDA");
                Dictionary["Spanish"].TryAdd("RATE US", "CALIFÍCANOS");
                Dictionary["Spanish"].TryAdd("SELECTED", "SELECCIONADO");
                Dictionary["Spanish"].TryAdd("OK", "OK");
                Dictionary["Spanish"].TryAdd("Go Premium without Ads", "Hazte Premium sin anuncios");
                Dictionary["Spanish"].TryAdd("Demo levels completed Purchase full version to continue", "Niveles de demostración completados, compra la versión completa para continuar");
                Dictionary["Spanish"].TryAdd("Connection Lost!", "¡Conexión Perdida!");
                Dictionary["Spanish"].TryAdd("Unable to load the game.", "No se puede cargar el juego.");
                Dictionary["Spanish"].TryAdd("Please check your internet connection.", "Por favor, revisa tu conexión a internet.");
                Dictionary["Spanish"].TryAdd("Try again", "intentar otra vez");


                //GERMAN LANGUAGE

                Dictionary["German"].TryAdd("Knockdown", "Abbruch");
                Dictionary["German"].TryAdd("Bottles", "Flaschen");
                Dictionary["German"].TryAdd("Play", "Spielen");
                Dictionary["German"].TryAdd("Select world", "Welt auswählen");
                Dictionary["German"].TryAdd("Forest", "Wald");
                Dictionary["German"].TryAdd("Ancient", "Antik");
                Dictionary["German"].TryAdd("Mettle", "Mut");
                Dictionary["German"].TryAdd("Desert", "Wüste");
                Dictionary["German"].TryAdd("Snowy", "Schnee");
                Dictionary["German"].TryAdd("WinPanel.Text", "Level abgeschlossen");
                Dictionary["German"].TryAdd("FailPanel.Text", "Level fehlgeschlagen");
                Dictionary["German"].TryAdd("TutorialText", "Schlage diese Flasche nicht");
                Dictionary["German"].TryAdd("PausePanel.Text", "Pause");
                Dictionary["German"].TryAdd("Achieve _ stars in forest world to unlock this world", "Erreiche _ Sterne im Waldwelt, um diese Welt freizuschalten");
                Dictionary["German"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Erreiche _ Sterne im antiken Welt, um diese Welt freizuschalten");
                Dictionary["German"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Erreiche _ Sterne im Mutwelt, um diese Welt freizuschalten");
                Dictionary["German"].TryAdd("Achieve _ stars in desert world to unlock this world", "Erreiche _ Sterne im Wüstenwelt, um diese Welt freizuschalten");
                Dictionary["German"].TryAdd("Achieve _ stars in snow world to unlock this world", "Erreiche _ Sterne im Schneewelt, um diese Welt freizuschalten");
                Dictionary["German"].TryAdd("Exit.Title", "Verlassen");
                Dictionary["German"].TryAdd("Exit.ConfirmText", "BIST DU SICHER..?");
                Dictionary["German"].TryAdd("HidePanel.Text", "Laden...");
                Dictionary["German"].TryAdd("StorePanel.text", "Speicherpanel");
                Dictionary["German"].TryAdd("Select", "Auswählen");
                Dictionary["German"].TryAdd("SpinWheel.text", "Rad drehen");
                Dictionary["German"].TryAdd("Spin", "Drehen");
                Dictionary["German"].TryAdd("Watchvideo.Text", "VIDEO ANSEHEN ZUM DREHEN");
                Dictionary["German"].TryAdd("Nospin.text", "Keine kostenlosen Drehungen verfügbar! Versuche es morgen");
                Dictionary["German"].TryAdd("GetCoins", "Erhalte 1000 Münzen");
                Dictionary["German"].TryAdd("Menu", "Menü");
                Dictionary["German"].TryAdd("New game", "Neues Spiel");
                Dictionary["German"].TryAdd("Continue", "Fortsetzen");
                Dictionary["German"].TryAdd("Settings", "Einstellungen");
                Dictionary["German"].TryAdd("Write a review", "Bewertung schreiben");
                Dictionary["German"].TryAdd("Difficulty", "Schwierigkeit");
                Dictionary["German"].TryAdd("Easy", "Einfach");
                Dictionary["German"].TryAdd("Medium", "Mittel");
                Dictionary["German"].TryAdd("Hard", "Schwer");
                Dictionary["German"].TryAdd("Effects volume", "Effektlautstärke");
                Dictionary["German"].TryAdd("Music volume", "Musiklautstärke");
                Dictionary["German"].TryAdd("Play time: {0:N1} h.", "Spielzeit: {0:N1} h.");
                Dictionary["German"].TryAdd("SKIP", "ÜBERSPRINGEN");
                Dictionary["German"].TryAdd("premium without ads", "Premium ohne Werbung");
                Dictionary["German"].TryAdd("100% Add free", "100% werbefrei");
                Dictionary["German"].TryAdd("skip video", "Video überspringen");
                Dictionary["German"].TryAdd("loading assets...", "Ressourcen laden...");
                Dictionary["German"].TryAdd("loading player data…", "Spielerdaten laden...");
                Dictionary["German"].TryAdd("loading scene...", "Szene laden...");
                Dictionary["German"].TryAdd("starting…", "Starten...");
                Dictionary["German"].TryAdd("PLAY", "SPIELEN");
                Dictionary["German"].TryAdd("SHOP", "GESCHÄFT");
                Dictionary["German"].TryAdd("RATE US", "BEWERTEN SIE UNS");
                Dictionary["German"].TryAdd("SELECTED", "AUSGEWÄHLT");
                Dictionary["German"].TryAdd("OK", "OK");
                Dictionary["German"].TryAdd("Go Premium without Ads", "Werde Premium ohne Werbung");
                Dictionary["German"].TryAdd("Demo levels completed Purchase full version to continue", "Demo-Level abgeschlossen, kaufen Sie die Vollversion, um fortzufahren");
                Dictionary["German"].TryAdd("Connection Lost!", "Verbindung verloren!");
                Dictionary["German"].TryAdd("Unable to load the game.", "Spiel kann nicht geladen werden.");
                Dictionary["German"].TryAdd("Please check your internet connection.", "Bitte überprüfe deine Internetverbindung.");
                Dictionary["German"].TryAdd("Try again", "versuchen Sie es erneut");

                //PORTUGUESE LANGUAGE

                Dictionary["Portuguese"].TryAdd("Knockdown", "Derrubar");
                Dictionary["Portuguese"].TryAdd("Bottles", "Garrafas");
                Dictionary["Portuguese"].TryAdd("Play", "Jogar");
                Dictionary["Portuguese"].TryAdd("Select world", "Selecionar mundo");
                Dictionary["Portuguese"].TryAdd("Forest", "Floresta");
                Dictionary["Portuguese"].TryAdd("Ancient", "Antigo");
                Dictionary["Portuguese"].TryAdd("Mettle", "Coragem");
                Dictionary["Portuguese"].TryAdd("Desert", "Deserto");
                Dictionary["Portuguese"].TryAdd("Snowy", "Nevado");
                Dictionary["Portuguese"].TryAdd("WinPanel.Text", "Nível Completado");
                Dictionary["Portuguese"].TryAdd("FailPanel.Text", "Nível Falhou");
                Dictionary["Portuguese"].TryAdd("TutorialText", "Não bata nesta garrafa");
                Dictionary["Portuguese"].TryAdd("PausePanel.Text", "Pausa");
                Dictionary["Portuguese"].TryAdd("Achieve _ stars in forest world to unlock this world", "Alcançar _ estrelas no mundo da floresta para desbloquear este mundo");
                Dictionary["Portuguese"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Alcançar _ estrelas no mundo antigo para desbloquear este mundo");
                Dictionary["Portuguese"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Alcançar _ estrelas no mundo da coragem para desbloquear este mundo");
                Dictionary["Portuguese"].TryAdd("Achieve _ stars in desert world to unlock this world", "Alcançar _ estrelas no mundo do deserto para desbloquear este mundo");
                Dictionary["Portuguese"].TryAdd("Achieve _ stars in snow world to unlock this world", "Alcançar _ estrelas no mundo da neve para desbloquear este mundo");
                Dictionary["Portuguese"].TryAdd("Exit.Title", "Sair");
                Dictionary["Portuguese"].TryAdd("Exit.ConfirmText", "VOCÊ TEM CERTEZA..?");
                Dictionary["Portuguese"].TryAdd("HidePanel.Text", "Carregando...");
                Dictionary["Portuguese"].TryAdd("StorePanel.text", "Painel da loja");
                Dictionary["Portuguese"].TryAdd("Select", "Selecionar");
                Dictionary["Portuguese"].TryAdd("SpinWheel.text", "Girar roda");
                Dictionary["Portuguese"].TryAdd("Spin", "Girar");
                Dictionary["Portuguese"].TryAdd("Watchvideo.Text", "ASSISTIR VÍDEO PARA GIRAR");
                Dictionary["Portuguese"].TryAdd("Nospin.text", "Sem giros grátis disponíveis! Tente amanhã");
                Dictionary["Portuguese"].TryAdd("GetCoins", "Obter 1000 moedas");
                Dictionary["Portuguese"].TryAdd("Menu", "Menu");
                Dictionary["Portuguese"].TryAdd("New game", "Novo jogo");
                Dictionary["Portuguese"].TryAdd("Continue", "Continuar");
                Dictionary["Portuguese"].TryAdd("Settings", "Configurações");
                Dictionary["Portuguese"].TryAdd("Write a review", "Escrever uma resenha");
                Dictionary["Portuguese"].TryAdd("Difficulty", "Dificuldade");
                Dictionary["Portuguese"].TryAdd("Easy", "Fácil");
                Dictionary["Portuguese"].TryAdd("Medium", "Médio");
                Dictionary["Portuguese"].TryAdd("Hard", "Difícil");
                Dictionary["Portuguese"].TryAdd("Effects volume", "Volume dos efeitos");
                Dictionary["Portuguese"].TryAdd("Music volume", "Volume da música");
                Dictionary["Portuguese"].TryAdd("Play time: {0:N1} h.", "Tempo de jogo: {0:N1} h.");
                Dictionary["Portuguese"].TryAdd("SKIP", "PULAR");
                Dictionary["Portuguese"].TryAdd("premium without ads", "premium sem anúncios");
                Dictionary["Portuguese"].TryAdd("100% Add free", "100% sem anúncios");
                Dictionary["Portuguese"].TryAdd("skip video", "pular vídeo");
                Dictionary["Portuguese"].TryAdd("loading assets...", "carregando recursos...");
                Dictionary["Portuguese"].TryAdd("loading player data…", "carregando dados do jogador...");
                Dictionary["Portuguese"].TryAdd("loading scene...", "carregando cena...");
                Dictionary["Portuguese"].TryAdd("starting…", "iniciando...");
                Dictionary["Portuguese"].TryAdd("PLAY", "JOGAR");
                Dictionary["Portuguese"].TryAdd("SHOP", "LOJA");
                Dictionary["Portuguese"].TryAdd("RATE US", "AVALIE-NOS");
                Dictionary["Portuguese"].TryAdd("SELECTED", "SELECIONADO");
                Dictionary["Portuguese"].TryAdd("OK", "OK");
                Dictionary["Portuguese"].TryAdd("Go Premium without Ads", "Torne-se Premium sem anúncios");
                Dictionary["Portuguese"].TryAdd("Demo levels completed Purchase full version to continue", "Níveis de demonstração concluídos, compre a versão completa para continuar");
                Dictionary["Portuguese"].TryAdd("Connection Lost!", "Conexão Perdida!");
                Dictionary["Portuguese"].TryAdd("Unable to load the game.", "Incapaz de carregar o jogo.");
                Dictionary["Portuguese"].TryAdd("Please check your internet connection.", "Por favor, verifique sua conexão com a internet.");
                Dictionary["Portuguese"].TryAdd("Try again", "tente novamente");


                //POLISH LANGUAGE

                Dictionary["Polish"].TryAdd("Knockdown", "Obalić");
                Dictionary["Polish"].TryAdd("Bottles", "Butelki");
                Dictionary["Polish"].TryAdd("Play", "Graj");
                Dictionary["Polish"].TryAdd("Select world", "Wybierz świat");
                Dictionary["Polish"].TryAdd("Forest", "Las");
                Dictionary["Polish"].TryAdd("Ancient", "Starożytny");
                Dictionary["Polish"].TryAdd("Mettle", "Hart ducha");
                Dictionary["Polish"].TryAdd("Desert", "Pustynia");
                Dictionary["Polish"].TryAdd("Snowy", "Zaśnieżony");
                Dictionary["Polish"].TryAdd("WinPanel.Text", "Poziom ukończony");
                Dictionary["Polish"].TryAdd("FailPanel.Text", "Poziom nieudany");
                Dictionary["Polish"].TryAdd("TutorialText", "Nie uderzaj tej butelki");
                Dictionary["Polish"].TryAdd("PausePanel.Text", "Pauza");
                Dictionary["Polish"].TryAdd("Achieve _ stars in forest world to unlock this world", "Zdobyj _ gwiazdek w świecie lasu, aby odblokować ten świat");
                Dictionary["Polish"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Zdobyj _ gwiazdek w starożytnym świecie, aby odblokować ten świat");
                Dictionary["Polish"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Zdobyj _ gwiazdek w świecie hartu ducha, aby odblokować ten świat");
                Dictionary["Polish"].TryAdd("Achieve _ stars in desert world to unlock this world", "Zdobyj _ gwiazdek w świecie pustyni, aby odblokować ten świat");
                Dictionary["Polish"].TryAdd("Achieve _ stars in snow world to unlock this world", "Zdobyj _ gwiazdek w zaśnieżonym świecie, aby odblokować ten świat");
                Dictionary["Polish"].TryAdd("Exit.Title", "Wyjście");
                Dictionary["Polish"].TryAdd("Exit.ConfirmText", "CZY JESTEŚ PEWIEN..?");
                Dictionary["Polish"].TryAdd("HidePanel.Text", "Ładowanie...");
                Dictionary["Polish"].TryAdd("StorePanel.text", "Panel sklepu");
                Dictionary["Polish"].TryAdd("Select", "Wybierz");
                Dictionary["Polish"].TryAdd("SpinWheel.text", "Kręć kołem");
                Dictionary["Polish"].TryAdd("Spin", "Kręć");
                Dictionary["Polish"].TryAdd("Watchvideo.Text", "OGLĄDAJ WIDEO, ABY KRĘCIĆ");
                Dictionary["Polish"].TryAdd("Nospin.text", "Brak darmowych obrotów! Spróbuj jutro");
                Dictionary["Polish"].TryAdd("GetCoins", "Zdobądź 1000 monet");
                Dictionary["Polish"].TryAdd("Menu", "Menu");
                Dictionary["Polish"].TryAdd("New game", "Nowa gra");
                Dictionary["Polish"].TryAdd("Continue", "Kontynuuj");
                Dictionary["Polish"].TryAdd("Settings", "Ustawienia");
                Dictionary["Polish"].TryAdd("Write a review", "Napisz recenzję");
                Dictionary["Polish"].TryAdd("Difficulty", "Trudność");
                Dictionary["Polish"].TryAdd("Easy", "Łatwy");
                Dictionary["Polish"].TryAdd("Medium", "Średni");
                Dictionary["Polish"].TryAdd("Hard", "Trudny");
                Dictionary["Polish"].TryAdd("Effects volume", "Głośność efektów");
                Dictionary["Polish"].TryAdd("Music volume", "Głośność muzyki");
                Dictionary["Polish"].TryAdd("Play time: {0:N1} h.", "Czas gry: {0:N1} h.");
                Dictionary["Polish"].TryAdd("SKIP", "POMIŃ");
                Dictionary["Polish"].TryAdd("premium without ads", "premium bez reklam");
                Dictionary["Polish"].TryAdd("100% Add free", "100% bez reklam");
                Dictionary["Polish"].TryAdd("skip video", "pomiń wideo");
                Dictionary["Polish"].TryAdd("loading assets...", "ładowanie zasobów...");
                Dictionary["Polish"].TryAdd("loading player data…", "ładowanie danych gracza...");
                Dictionary["Polish"].TryAdd("loading scene...", "ładowanie sceny...");
                Dictionary["Polish"].TryAdd("starting…", "rozpoczynanie...");
                Dictionary["Polish"].TryAdd("PLAY", "GRAJ");
                Dictionary["Polish"].TryAdd("SHOP", "SKLEP");
                Dictionary["Polish"].TryAdd("RATE US", "OCEN NAS");
                Dictionary["Polish"].TryAdd("SELECTED", "WYBRANY");
                Dictionary["Polish"].TryAdd("OK", "OK");
                Dictionary["Polish"].TryAdd("Go Premium without Ads", "Przejdź na Premium bez reklam");
                Dictionary["Polish"].TryAdd("Demo levels completed Purchase full version to continue", "Poziomy demonstracyjne ukończone Kup pełną wersję, aby kontynuować");
                Dictionary["Polish"].TryAdd("Connection Lost!", "Utracono połączenie!");
                Dictionary["Polish"].TryAdd("Unable to load the game.", "Nie można załadować gry.");
                Dictionary["Polish"].TryAdd("Please check your internet connection.", "Proszę sprawdź swoje połączenie internetowe.");
                Dictionary["Polish"].TryAdd("Try again", "spróbuj ponownie");


                //FRENCH LANGUAGE

                Dictionary["French"].TryAdd("Knockdown", "Renverser");
                Dictionary["French"].TryAdd("Bottles", "Bouteilles");
                Dictionary["French"].TryAdd("Play", "Jouer");
                Dictionary["French"].TryAdd("Select world", "Sélectionner le monde");
                Dictionary["French"].TryAdd("Forest", "Forêt");
                Dictionary["French"].TryAdd("Ancient", "Ancien");
                Dictionary["French"].TryAdd("Mettle", "Courage");
                Dictionary["French"].TryAdd("Desert", "Désert");
                Dictionary["French"].TryAdd("Snowy", "Enneigé");
                Dictionary["French"].TryAdd("WinPanel.Text", "Niveau Complété");
                Dictionary["French"].TryAdd("FailPanel.Text", "Niveau Échoué");
                Dictionary["French"].TryAdd("TutorialText", "Ne frappez pas cette bouteille");
                Dictionary["French"].TryAdd("PausePanel.Text", "Pause");
                Dictionary["French"].TryAdd("Achieve _ stars in forest world to unlock this world", "Obtenez _ étoiles dans le monde de la forêt pour débloquer ce monde");
                Dictionary["French"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Obtenez _ étoiles dans le monde ancien pour débloquer ce monde");
                Dictionary["French"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Obtenez _ étoiles dans le monde du courage pour débloquer ce monde");
                Dictionary["French"].TryAdd("Achieve _ stars in desert world to unlock this world", "Obtenez _ étoiles dans le monde du désert pour débloquer ce monde");
                Dictionary["French"].TryAdd("Achieve _ stars in snow world to unlock this world", "Obtenez _ étoiles dans le monde enneigé pour débloquer ce monde");
                Dictionary["French"].TryAdd("Exit.Title", "Sortie");
                Dictionary["French"].TryAdd("Exit.ConfirmText", "ÊTES-VOUS SÛR..?");
                Dictionary["French"].TryAdd("HidePanel.Text", "Chargement...");
                Dictionary["French"].TryAdd("StorePanel.text", "Panneau de la boutique");
                Dictionary["French"].TryAdd("Select", "Sélectionner");
                Dictionary["French"].TryAdd("SpinWheel.text", "Tourner la roue");
                Dictionary["French"].TryAdd("Spin", "Tourner");
                Dictionary["French"].TryAdd("Watchvideo.Text", "REGARDER LA VIDÉO POUR TOURNER");
                Dictionary["French"].TryAdd("Nospin.text", "Pas de tours gratuits disponibles! Essayez demain");
                Dictionary["French"].TryAdd("GetCoins", "Obtenez 1000 pièces");
                Dictionary["French"].TryAdd("Menu", "Menu");
                Dictionary["French"].TryAdd("New game", "Nouveau jeu");
                Dictionary["French"].TryAdd("Continue", "Continuer");
                Dictionary["French"].TryAdd("Settings", "Paramètres");
                Dictionary["French"].TryAdd("Write a review", "Écrire un avis");
                Dictionary["French"].TryAdd("Difficulty", "Difficulté");
                Dictionary["French"].TryAdd("Easy", "Facile");
                Dictionary["French"].TryAdd("Medium", "Moyen");
                Dictionary["French"].TryAdd("Hard", "Difficile");
                Dictionary["French"].TryAdd("Effects volume", "Volume des effets");
                Dictionary["French"].TryAdd("Music volume", "Volume de la musique");
                Dictionary["French"].TryAdd("Play time: {0:N1} h.", "Temps de jeu : {0:N1} h.");
                Dictionary["French"].TryAdd("SKIP", "PASSER");
                Dictionary["French"].TryAdd("premium without ads", "premium sans annonces");
                Dictionary["French"].TryAdd("100% Add free", "100% sans annonces");
                Dictionary["French"].TryAdd("skip video", "passer la vidéo");
                Dictionary["French"].TryAdd("loading assets...", "chargement des ressources...");
                Dictionary["French"].TryAdd("loading player data…", "chargement des données du joueur...");
                Dictionary["French"].TryAdd("loading scene...", "chargement de la scène...");
                Dictionary["French"].TryAdd("starting…", "démarrage...");
                Dictionary["French"].TryAdd("PLAY", "JOUER");
                Dictionary["French"].TryAdd("SHOP", "BOUTIQUE");
                Dictionary["French"].TryAdd("RATE US", "ÉVALUEZ-NOUS");
                Dictionary["French"].TryAdd("SELECTED", "SÉLECTIONNÉ");
                Dictionary["French"].TryAdd("OK", "OK");
                Dictionary["French"].TryAdd("Go Premium without Ads", "Passez à Premium sans annonces");
                Dictionary["French"].TryAdd("Demo levels completed Purchase full version to continue", "Niveaux de démonstration complétés, achetez la version complète pour continuer");
                Dictionary["French"].TryAdd("Connection Lost!", "Connexion perdue!");
                Dictionary["French"].TryAdd("Unable to load the game.", "Impossible de charger le jeu.");
                Dictionary["French"].TryAdd("Please check your internet connection.", "Veuillez vérifier votre connexion Internet.");
                Dictionary["French"].TryAdd("Try again", "essayer à nouveau");


                //DUTCH LANGUAGE

                Dictionary["Dutch"].TryAdd("Knockdown", "Neerslag");
                Dictionary["Dutch"].TryAdd("Bottles", "Flessen");
                Dictionary["Dutch"].TryAdd("Play", "Spelen");
                Dictionary["Dutch"].TryAdd("Select world", "Selecteer wereld");
                Dictionary["Dutch"].TryAdd("Forest", "Bos");
                Dictionary["Dutch"].TryAdd("Ancient", "Oud");
                Dictionary["Dutch"].TryAdd("Mettle", "Moed");
                Dictionary["Dutch"].TryAdd("Desert", "Woestijn");
                Dictionary["Dutch"].TryAdd("Snowy", "Sneeuw");
                Dictionary["Dutch"].TryAdd("WinPanel.Text", "Niveau Geslaagd");
                Dictionary["Dutch"].TryAdd("FailPanel.Text", "Niveau Mislukt");
                Dictionary["Dutch"].TryAdd("TutorialText", "Raak deze fles niet");
                Dictionary["Dutch"].TryAdd("PausePanel.Text", "Pauze");
                Dictionary["Dutch"].TryAdd("Achieve _ stars in forest world to unlock this world", "Behaal _ sterren in het boswereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Behaal _ sterren in de oude wereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Behaal _ sterren in de moedwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].TryAdd("Achieve _ stars in desert world to unlock this world", "Behaal _ sterren in de woestijnwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].TryAdd("Achieve _ stars in snow world to unlock this world", "Behaal _ sterren in de sneeuwwereld om deze wereld te ontgrendelen");
                Dictionary["Dutch"].TryAdd("Exit.Title", "Afsluiten");
                Dictionary["Dutch"].TryAdd("Exit.ConfirmText", "WEET JE HET ZEKER..?");
                Dictionary["Dutch"].TryAdd("HidePanel.Text", "Laden...");
                Dictionary["Dutch"].TryAdd("StorePanel.text", "Winkelpaneel");
                Dictionary["Dutch"].TryAdd("Select", "Selecteren");
                Dictionary["Dutch"].TryAdd("SpinWheel.text", "Wiel draaien");
                Dictionary["Dutch"].TryAdd("Spin", "Draaien");
                Dictionary["Dutch"].TryAdd("Watchvideo.Text", "BEKIJK VIDEO OM TE DRAAIEN");
                Dictionary["Dutch"].TryAdd("Nospin.text", "Geen gratis spins beschikbaar! Probeer het morgen");
                Dictionary["Dutch"].TryAdd("GetCoins", "Krijg 1000 munten");
                Dictionary["Dutch"].TryAdd("Menu", "Menu");
                Dictionary["Dutch"].TryAdd("New game", "Nieuw spel");
                Dictionary["Dutch"].TryAdd("Continue", "Doorgaan");
                Dictionary["Dutch"].TryAdd("Settings", "Instellingen");
                Dictionary["Dutch"].TryAdd("Write a review", "Schrijf een recensie");
                Dictionary["Dutch"].TryAdd("Difficulty", "Moeilijkheidsgraad");
                Dictionary["Dutch"].TryAdd("Easy", "Gemakkelijk");
                Dictionary["Dutch"].TryAdd("Medium", "Gemiddeld");
                Dictionary["Dutch"].TryAdd("Hard", "Moeilijk");
                Dictionary["Dutch"].TryAdd("Effects volume", "Effecten volume");
                Dictionary["Dutch"].TryAdd("Music volume", "Muziek volume");
                Dictionary["Dutch"].TryAdd("Play time: {0:N1} h.", "Speeltijd: {0:N1} uur.");
                Dictionary["Dutch"].TryAdd("SKIP", "OVERSLAAN");
                Dictionary["Dutch"].TryAdd("premium without ads", "premium zonder advertenties");
                Dictionary["Dutch"].TryAdd("100% Add free", "100% advertentievrij");
                Dictionary["Dutch"].TryAdd("skip video", "video overslaan");
                Dictionary["Dutch"].TryAdd("loading assets...", "middelen laden...");
                Dictionary["Dutch"].TryAdd("loading player data…", "spelergegevens laden...");
                Dictionary["Dutch"].TryAdd("loading scene...", "scène laden...");
                Dictionary["Dutch"].TryAdd("starting…", "starten...");
                Dictionary["Dutch"].TryAdd("PLAY", "SPELEN");
                Dictionary["Dutch"].TryAdd("SHOP", "WINKEL");
                Dictionary["Dutch"].TryAdd("RATE US", "BEOORDEEL ONS");
                Dictionary["Dutch"].TryAdd("SELECTED", "GESELECTEERD");
                Dictionary["Dutch"].TryAdd("OK", "OK");
                Dictionary["Dutch"].TryAdd("Go Premium without Ads", "Word Premium zonder advertenties");
                Dictionary["Dutch"].TryAdd("Demo levels completed Purchase full version to continue", "Demolevels voltooid Koop de volledige versie om door te gaan");
                Dictionary["Dutch"].TryAdd("Connection Lost!", "Verbinding Verloren!");
                Dictionary["Dutch"].TryAdd("Unable to load the game.", "Kan het spel niet laden.");
                Dictionary["Dutch"].TryAdd("Please check your internet connection.", "Controleer je internetverbinding.");
                Dictionary["Dutch"].TryAdd("Try again", "probeer het opnieuw");


                //TURKISH LANGUAGE

                Dictionary["Turkish"].TryAdd("Knockdown", "Yıkılmak");
                Dictionary["Turkish"].TryAdd("Bottles", "Şişeler");
                Dictionary["Turkish"].TryAdd("Play", "Oynamak");
                Dictionary["Turkish"].TryAdd("Select world", "Dünyayı seç");
                Dictionary["Turkish"].TryAdd("Forest", "Orman");
                Dictionary["Turkish"].TryAdd("Ancient", "Antik");
                Dictionary["Turkish"].TryAdd("Mettle", "Cesaret");
                Dictionary["Turkish"].TryAdd("Desert", "Çöl");
                Dictionary["Turkish"].TryAdd("Snowy", "Karlı");
                Dictionary["Turkish"].TryAdd("WinPanel.Text", "Seviye Geçildi");
                Dictionary["Turkish"].TryAdd("FailPanel.Text", "Seviye Başarısız");
                Dictionary["Turkish"].TryAdd("TutorialText", "Bu şişeye vurma");
                Dictionary["Turkish"].TryAdd("PausePanel.Text", "Duraklat");
                Dictionary["Turkish"].TryAdd("Achieve _ stars in forest world to unlock this world", "Bu dünyayı açmak için orman dünyasında _ yıldız elde et");
                Dictionary["Turkish"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Bu dünyayı açmak için antik dünyada _ yıldız elde et");
                Dictionary["Turkish"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Bu dünyayı açmak için cesaret dünyasında _ yıldız elde et");
                Dictionary["Turkish"].TryAdd("Achieve _ stars in desert world to unlock this world", "Bu dünyayı açmak için çöl dünyasında _ yıldız elde et");
                Dictionary["Turkish"].TryAdd("Achieve _ stars in snow world to unlock this world", "Bu dünyayı açmak için karlı dünyada _ yıldız elde et");
                Dictionary["Turkish"].TryAdd("Exit.Title", "Çıkış");
                Dictionary["Turkish"].TryAdd("Exit.ConfirmText", "EMİN MİSİN..?");
                Dictionary["Turkish"].TryAdd("HidePanel.Text", "Yükleniyor...");
                Dictionary["Turkish"].TryAdd("StorePanel.text", "Mağaza Paneli");
                Dictionary["Turkish"].TryAdd("Select", "Seç");
                Dictionary["Turkish"].TryAdd("SpinWheel.text", "Çarkı döndür");
                Dictionary["Turkish"].TryAdd("Spin", "Döndür");
                Dictionary["Turkish"].TryAdd("Watchvideo.Text", "DÖNDÜRMEK İÇİN VİDEO İZLE");
                Dictionary["Turkish"].TryAdd("Nospin.text", "Ücretsiz dönüş yok! Yarın tekrar deneyin");
                Dictionary["Turkish"].TryAdd("GetCoins", "1000 jeton al");
                Dictionary["Turkish"].TryAdd("Menu", "Menü");
                Dictionary["Turkish"].TryAdd("New game", "Yeni oyun");
                Dictionary["Turkish"].TryAdd("Continue", "Devam et");
                Dictionary["Turkish"].TryAdd("Settings", "Ayarlar");
                Dictionary["Turkish"].TryAdd("Write a review", "İnceleme yaz");
                Dictionary["Turkish"].TryAdd("Difficulty", "Zorluk");
                Dictionary["Turkish"].TryAdd("Easy", "Kolay");
                Dictionary["Turkish"].TryAdd("Medium", "Orta");
                Dictionary["Turkish"].TryAdd("Hard", "Zor");
                Dictionary["Turkish"].TryAdd("Effects volume", "Efekt sesi");
                Dictionary["Turkish"].TryAdd("Music volume", "Müzik sesi");
                Dictionary["Turkish"].TryAdd("Play time: {0:N1} h.", "Oynama süresi: {0:N1} sa.");
                Dictionary["Turkish"].TryAdd("SKIP", "GEÇ");
                Dictionary["Turkish"].TryAdd("premium without ads", "reklamsız premium");
                Dictionary["Turkish"].TryAdd("100% Add free", "100% reklamsız");
                Dictionary["Turkish"].TryAdd("skip video", "videoyu geç");
                Dictionary["Turkish"].TryAdd("loading assets...", "varlıklar yükleniyor...");
                Dictionary["Turkish"].TryAdd("loading player data…", "oyuncu verileri yükleniyor...");
                Dictionary["Turkish"].TryAdd("loading scene...", "sahne yükleniyor...");
                Dictionary["Turkish"].TryAdd("starting…", "başlıyor...");
                Dictionary["Turkish"].TryAdd("PLAY", "OYNAMAK");
                Dictionary["Turkish"].TryAdd("SHOP", "DÜKKAN");
                Dictionary["Turkish"].TryAdd("RATE US", "BİZİ OYLA");
                Dictionary["Turkish"].TryAdd("SELECTED", "SEÇİLDİ");
                Dictionary["Turkish"].TryAdd("OK", "TAMAM");
                Dictionary["Turkish"].TryAdd("Go Premium without Ads", "Reklamsız Premium'a geç");
                Dictionary["Turkish"].TryAdd("Demo levels completed Purchase full version to continue", "Demo seviyeleri tamamlandı, devam etmek için tam sürümü satın alın");
                Dictionary["Turkish"].TryAdd("Connection Lost!", "Bağlantı Kaybedildi!");
                Dictionary["Turkish"].TryAdd("Unable to load the game.", "Oyunu yükleyemiyor.");
                Dictionary["Turkish"].TryAdd("Please check your internet connection.", "Lütfen internet bağlantınızı kontrol edin.");
                Dictionary["Turkish"].TryAdd("Try again", "tekrar deneyin");



                //ITALIAN LANGUAGE

                Dictionary["Italian"].TryAdd("Knockdown", "Abbattimento");
                Dictionary["Italian"].TryAdd("Bottles", "Bottiglie");
                Dictionary["Italian"].TryAdd("Play", "Giocare");
                Dictionary["Italian"].TryAdd("Select world", "Seleziona il mondo");
                Dictionary["Italian"].TryAdd("Forest", "Foresta");
                Dictionary["Italian"].TryAdd("Ancient", "Antico");
                Dictionary["Italian"].TryAdd("Mettle", "Coraggio");
                Dictionary["Italian"].TryAdd("Desert", "Deserto");
                Dictionary["Italian"].TryAdd("Snowy", "Nevoso");
                Dictionary["Italian"].TryAdd("WinPanel.Text", "Livello Superato");
                Dictionary["Italian"].TryAdd("FailPanel.Text", "Livello Fallito");
                Dictionary["Italian"].TryAdd("TutorialText", "Non colpire questa bottiglia");
                Dictionary["Italian"].TryAdd("PausePanel.Text", "Pausa");
                Dictionary["Italian"].TryAdd("Achieve _ stars in forest world to unlock this world", "Raggiungi _ stelle nel mondo della foresta per sbloccare questo mondo");
                Dictionary["Italian"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Raggiungi _ stelle nel mondo antico per sbloccare questo mondo");
                Dictionary["Italian"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Raggiungi _ stelle nel mondo del coraggio per sbloccare questo mondo");
                Dictionary["Italian"].TryAdd("Achieve _ stars in desert world to unlock this world", "Raggiungi _ stelle nel mondo del deserto per sbloccare questo mondo");
                Dictionary["Italian"].TryAdd("Achieve _ stars in snow world to unlock this world", "Raggiungi _ stelle nel mondo nevoso per sbloccare questo mondo");
                Dictionary["Italian"].TryAdd("Exit.Title", "Uscita");
                Dictionary["Italian"].TryAdd("Exit.ConfirmText", "SEI SICURO..?");
                Dictionary["Italian"].TryAdd("HidePanel.Text", "Caricamento...");
                Dictionary["Italian"].TryAdd("StorePanel.text", "Pannello del negozio");
                Dictionary["Italian"].TryAdd("Select", "Selezionare");
                Dictionary["Italian"].TryAdd("SpinWheel.text", "Gira la ruota");
                Dictionary["Italian"].TryAdd("Spin", "Gira");
                Dictionary["Italian"].TryAdd("Watchvideo.Text", "GUARDA IL VIDEO PER GIRARE");
                Dictionary["Italian"].TryAdd("Nospin.text", "Nessun giro gratuito disponibile! Prova domani");
                Dictionary["Italian"].TryAdd("GetCoins", "Ottieni 1000 monete");
                Dictionary["Italian"].TryAdd("Menu", "Menu");
                Dictionary["Italian"].TryAdd("New game", "Nuovo gioco");
                Dictionary["Italian"].TryAdd("Continue", "Continua");
                Dictionary["Italian"].TryAdd("Settings", "Impostazioni");
                Dictionary["Italian"].TryAdd("Write a review", "Scrivi una recensione");
                Dictionary["Italian"].TryAdd("Difficulty", "Difficoltà");
                Dictionary["Italian"].TryAdd("Easy", "Facile");
                Dictionary["Italian"].TryAdd("Medium", "Medio");
                Dictionary["Italian"].TryAdd("Hard", "Difficile");
                Dictionary["Italian"].TryAdd("Effects volume", "Volume effetti");
                Dictionary["Italian"].TryAdd("Music volume", "Volume della musica");
                Dictionary["Italian"].TryAdd("Play time: {0:N1} h.", "Tempo di gioco: {0:N1} h.");
                Dictionary["Italian"].TryAdd("SKIP", "SALTA");
                Dictionary["Italian"].TryAdd("premium without ads", "premium senza annunci");
                Dictionary["Italian"].TryAdd("100% Add free", "100% senza annunci");
                Dictionary["Italian"].TryAdd("skip video", "salta video");
                Dictionary["Italian"].TryAdd("loading assets...", "caricamento risorse...");
                Dictionary["Italian"].TryAdd("loading player data…", "caricamento dati giocatore...");
                Dictionary["Italian"].TryAdd("loading scene...", "caricamento scena...");
                Dictionary["Italian"].TryAdd("starting…", "avvio...");
                Dictionary["Italian"].TryAdd("PLAY", "GIOCARE");
                Dictionary["Italian"].TryAdd("SHOP", "NEGOZIO");
                Dictionary["Italian"].TryAdd("RATE US", "VALUTACI");
                Dictionary["Italian"].TryAdd("SELECTED", "SELEZIONATO");
                Dictionary["Italian"].TryAdd("OK", "OK");
                Dictionary["Italian"].TryAdd("Go Premium without Ads", "Passa a Premium senza annunci");
                Dictionary["Italian"].TryAdd("Demo levels completed Purchase full version to continue", "Livelli dimostrativi completati Acquista la versione completa per continuare");
                Dictionary["Italian"].TryAdd("Connection Lost!", "Connessione persa!");
                Dictionary["Italian"].TryAdd("Unable to load the game.", "Impossibile caricare il gioco.");
                Dictionary["Italian"].TryAdd("Please check your internet connection.", "Si prega di controllare la connessione a Internet.");
                Dictionary["Italian"].TryAdd("Try again", "riprova");


                //RUSSIAN LANGUAGE

                Dictionary["Russian"].TryAdd("Knockdown", "Нокаут");
                Dictionary["Russian"].TryAdd("Bottles", "Бутылки");
                Dictionary["Russian"].TryAdd("Play", "Играть");
                Dictionary["Russian"].TryAdd("Select world", "Выбрать мир");
                Dictionary["Russian"].TryAdd("Forest", "Лес");
                Dictionary["Russian"].TryAdd("Ancient", "Древний");
                Dictionary["Russian"].TryAdd("Mettle", "Мужество");
                Dictionary["Russian"].TryAdd("Desert", "Пустыня");
                Dictionary["Russian"].TryAdd("Snowy", "Снежный");
                Dictionary["Russian"].TryAdd("WinPanel.Text", "Уровень пройден");
                Dictionary["Russian"].TryAdd("FailPanel.Text", "Уровень не пройден");
                Dictionary["Russian"].TryAdd("TutorialText", "Не бейте эту бутылку");
                Dictionary["Russian"].TryAdd("PausePanel.Text", "Пауза");
                Dictionary["Russian"].TryAdd("Achieve _ stars in forest world to unlock this world", "Достигните _ звёзд в лесном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].TryAdd("Achieve _ stars in ancient world to unlock this world", "Достигните _ звёзд в древнем мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].TryAdd("Achieve _ stars in mettle world to unlock this world", "Достигните _ звёзд в мире мужества, чтобы разблокировать этот мир");
                Dictionary["Russian"].TryAdd("Achieve _ stars in desert world to unlock this world", "Достигните _ звёзд в пустынном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].TryAdd("Achieve _ stars in snow world to unlock this world", "Достигните _ звёзд в снежном мире, чтобы разблокировать этот мир");
                Dictionary["Russian"].TryAdd("Exit.Title", "Выход");
                Dictionary["Russian"].TryAdd("Exit.ConfirmText", "ВЫ УВЕРЕНЫ..?");
                Dictionary["Russian"].TryAdd("HidePanel.Text", "Загрузка...");
                Dictionary["Russian"].TryAdd("StorePanel.text", "Панель магазина");
                Dictionary["Russian"].TryAdd("Select", "Выбрать");
                Dictionary["Russian"].TryAdd("SpinWheel.text", "Крутить колесо");
                Dictionary["Russian"].TryAdd("Spin", "Крутить");
                Dictionary["Russian"].TryAdd("Watchvideo.Text", "СМОТРЕТЬ ВИДЕО ЧТОБЫ КРУТИТЬ");
                Dictionary["Russian"].TryAdd("Nospin.text", "Бесплатных вращений нет! Попробуйте завтра");
                Dictionary["Russian"].TryAdd("GetCoins", "Получить 1000 монет");
                Dictionary["Russian"].TryAdd("Menu", "Меню");
                Dictionary["Russian"].TryAdd("New game", "Новая игра");
                Dictionary["Russian"].TryAdd("Continue", "Продолжить");
                Dictionary["Russian"].TryAdd("Settings", "Настройки");
                Dictionary["Russian"].TryAdd("Write a review", "Написать отзыв");
                Dictionary["Russian"].TryAdd("Difficulty", "Сложность");
                Dictionary["Russian"].TryAdd("Easy", "Легко");
                Dictionary["Russian"].TryAdd("Medium", "Средне");
                Dictionary["Russian"].TryAdd("Hard", "Трудно");
                Dictionary["Russian"].TryAdd("Effects volume", "Громкость эффектов");
                Dictionary["Russian"].TryAdd("Music volume", "Громкость музыки");
                Dictionary["Russian"].TryAdd("Play time: {0:N1} h.", "Время игры: {0:N1} ч.");
                Dictionary["Russian"].TryAdd("SKIP", "ПРОПУСТИТЬ");
                Dictionary["Russian"].TryAdd("premium without ads", "премиум без рекламы");
                Dictionary["Russian"].TryAdd("100% Add free", "100% без рекламы");
                Dictionary["Russian"].TryAdd("skip video", "пропустить видео");
                Dictionary["Russian"].TryAdd("loading assets...", "загрузка ресурсов...");
                Dictionary["Russian"].TryAdd("loading player data…", "загрузка данных игрока...");
                Dictionary["Russian"].TryAdd("loading scene...", "загрузка сцены...");
                Dictionary["Russian"].TryAdd("starting…", "запуск...");
                Dictionary["Russian"].TryAdd("PLAY", "ИГРАТЬ");
                Dictionary["Russian"].TryAdd("SHOP", "МАГАЗИН");
                Dictionary["Russian"].TryAdd("RATE US", "ОЦЕНИТЕ НАС");
                Dictionary["Russian"].TryAdd("SELECTED", "ВЫБРАНО");
                Dictionary["Russian"].TryAdd("OK", "ОК");
                Dictionary["Russian"].TryAdd("Go Premium without Ads", "Перейдите на Премиум без рекламы");
                Dictionary["Russian"].TryAdd("Demo levels completed Purchase full version to continue", "Демо-уровни завершены Купите полную версию чтобы продолжить");
                Dictionary["Russian"].TryAdd("Connection Lost!", "Соединение потеряно!");
                Dictionary["Russian"].TryAdd("Unable to load the game.", "Не удалось загрузить игру.");
                Dictionary["Russian"].TryAdd("Please check your internet connection.", "Пожалуйста, проверьте ваше интернет-соединение.");
                Dictionary["Russian"].TryAdd("Try again", "Попробуйте еще раз");


                //JAPANESE LANGUAGE


                Dictionary["Japanese"].TryAdd("Knockdown", "ノックダウン");
                Dictionary["Japanese"].TryAdd("Bottles", "ボトル");
                Dictionary["Japanese"].TryAdd("Play", "プレイ");
                Dictionary["Japanese"].TryAdd("Select world", "世界を選択");
                Dictionary["Japanese"].TryAdd("Forest", "森");
                Dictionary["Japanese"].TryAdd("Ancient", "古代");
                Dictionary["Japanese"].TryAdd("Mettle", "勇気");
                Dictionary["Japanese"].TryAdd("Desert", "砂漠");
                Dictionary["Japanese"].TryAdd("Snowy", "雪");
                Dictionary["Japanese"].TryAdd("WinPanel.Text", "レベルクリア");
                Dictionary["Japanese"].TryAdd("FailPanel.Text", "レベル失敗");
                Dictionary["Japanese"].TryAdd("TutorialText", "このボトルを叩かないでください");
                Dictionary["Japanese"].TryAdd("PausePanel.Text", "一時停止");
                Dictionary["Japanese"].TryAdd("Achieve _ stars in forest world to unlock this world", "この世界をアンロックするために、森の世界で _ つの星を達成してください");
                Dictionary["Japanese"].TryAdd("Achieve _ stars in ancient world to unlock this world", "この世界をアンロックするために、古代の世界で _ つの星を達成してください");
                Dictionary["Japanese"].TryAdd("Achieve _ stars in mettle world to unlock this world", "この世界をアンロックするために、勇気の世界で _ つの星を達成してください");
                Dictionary["Japanese"].TryAdd("Achieve _ stars in desert world to unlock this world", "この世界をアンロックするために、砂漠の世界で _ つの星を達成してください");
                Dictionary["Japanese"].TryAdd("Achieve _ stars in snow world to unlock this world", "この世界をアンロックするために、雪の世界で _ つの星を達成してください");
                Dictionary["Japanese"].TryAdd("Exit.Title", "出口");
                Dictionary["Japanese"].TryAdd("Exit.ConfirmText", "本当に..?");
                Dictionary["Japanese"].TryAdd("HidePanel.Text", "読み込み中...");
                Dictionary["Japanese"].TryAdd("StorePanel.text", "ストアパネル");
                Dictionary["Japanese"].TryAdd("Select", "選択");
                Dictionary["Japanese"].TryAdd("SpinWheel.text", "ホイールを回す");
                Dictionary["Japanese"].TryAdd("Spin", "回す");
                Dictionary["Japanese"].TryAdd("Watchvideo.Text", "回すためにビデオを見る");
                Dictionary["Japanese"].TryAdd("Nospin.text", "無料のスピンはありません！ 明日もう一度お試しください");
                Dictionary["Japanese"].TryAdd("GetCoins", "1000枚のコインを取得する");
                Dictionary["Japanese"].TryAdd("Menu", "メニュー");
                Dictionary["Japanese"].TryAdd("New game", "新しいゲーム");
                Dictionary["Japanese"].TryAdd("Continue", "続ける");
                Dictionary["Japanese"].TryAdd("Settings", "設定");
                Dictionary["Japanese"].TryAdd("Write a review", "レビューを書く");
                Dictionary["Japanese"].TryAdd("Difficulty", "難易度");
                Dictionary["Japanese"].TryAdd("Easy", "簡単");
                Dictionary["Japanese"].TryAdd("Medium", "中程度");
                Dictionary["Japanese"].TryAdd("Hard", "難しい");
                Dictionary["Japanese"].TryAdd("Effects volume", "エフェクトの音量");
                Dictionary["Japanese"].TryAdd("Music volume", "音楽の音量");
                Dictionary["Japanese"].TryAdd("Play time: {0:N1} h.", "プレイ時間: {0:N1} 時間。");
                Dictionary["Japanese"].TryAdd("SKIP", "スキップ");
                Dictionary["Japanese"].TryAdd("premium without ads", "広告なしのプレミアム");
                Dictionary["Japanese"].TryAdd("100% Add free", "100% 広告なし");
                Dictionary["Japanese"].TryAdd("skip video", "ビデオをスキップ");
                Dictionary["Japanese"].TryAdd("loading assets...", "アセットの読み込み中...");
                Dictionary["Japanese"].TryAdd("loading player data…", "プレイヤーデータの読み込み中...");
                Dictionary["Japanese"].TryAdd("loading scene...", "シーンの読み込み中...");
                Dictionary["Japanese"].TryAdd("starting…", "開始...");
                Dictionary["Japanese"].TryAdd("PLAY", "プレイ");
                Dictionary["Japanese"].TryAdd("SHOP", "ショップ");
                Dictionary["Japanese"].TryAdd("RATE US", "評価する");
                Dictionary["Japanese"].TryAdd("SELECTED", "選択された");
                Dictionary["Japanese"].TryAdd("OK", "OK");
                Dictionary["Japanese"].TryAdd("Go Premium without Ads", "広告なしのプレミアムに移行");
                Dictionary["Japanese"].TryAdd("Demo levels completed Purchase full version to continue", "デモレベルが完了しました 続行するには完全版を購入してください");
                Dictionary["Japanese"].TryAdd("Connection Lost!", "接続が失われました！");
                Dictionary["Japanese"].TryAdd("Unable to load the game.", "ゲームを読み込めません。");
                Dictionary["Japanese"].TryAdd("Please check your internet connection.", "インターネット接続を確認してください。");
                Dictionary["Japanese"].TryAdd("Try again", "もう一度やり直してください");


                //CHINESE LANGUAGE

                Dictionary["Chinese"].TryAdd("Knockdown", "击倒");
                Dictionary["Chinese"].TryAdd("Bottles", "瓶子");
                Dictionary["Chinese"].TryAdd("Play", "玩");
                Dictionary["Chinese"].TryAdd("Select world", "选择世界");
                Dictionary["Chinese"].TryAdd("Forest", "森林");
                Dictionary["Chinese"].TryAdd("Ancient", "古老");
                Dictionary["Chinese"].TryAdd("Mettle", "勇气");
                Dictionary["Chinese"].TryAdd("Desert", "沙漠");
                Dictionary["Chinese"].TryAdd("Snowy", "多雪的");
                Dictionary["Chinese"].TryAdd("WinPanel.Text", "关卡完成");
                Dictionary["Chinese"].TryAdd("FailPanel.Text", "关卡失败");
                Dictionary["Chinese"].TryAdd("TutorialText", "不要打这个瓶子");
                Dictionary["Chinese"].TryAdd("PausePanel.Text", "暂停");
                Dictionary["Chinese"].TryAdd("Achieve _ stars in forest world to unlock this world", "在森林世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].TryAdd("Achieve _ stars in ancient world to unlock this world", "在古老世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].TryAdd("Achieve _ stars in mettle world to unlock this world", "在勇气世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].TryAdd("Achieve _ stars in desert world to unlock this world", "在沙漠世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].TryAdd("Achieve _ stars in snow world to unlock this world", "在多雪的世界中获得 _ 颗星星以解锁这个世界");
                Dictionary["Chinese"].TryAdd("Exit.Title", "退出");
                Dictionary["Chinese"].TryAdd("Exit.ConfirmText", "你确定吗..?");
                Dictionary["Chinese"].TryAdd("HidePanel.Text", "加载中...");
                Dictionary["Chinese"].TryAdd("StorePanel.text", "商店面板");
                Dictionary["Chinese"].TryAdd("Select", "选择");
                Dictionary["Chinese"].TryAdd("SpinWheel.text", "旋转轮盘");
                Dictionary["Chinese"].TryAdd("Spin", "旋转");
                Dictionary["Chinese"].TryAdd("Watchvideo.Text", "观看视频以旋转");
                Dictionary["Chinese"].TryAdd("Nospin.text", "没有免费的旋转！ 请明天再试");
                Dictionary["Chinese"].TryAdd("GetCoins", "获得1000金币");
                Dictionary["Chinese"].TryAdd("Menu", "菜单");
                Dictionary["Chinese"].TryAdd("New game", "新游戏");
                Dictionary["Chinese"].TryAdd("Continue", "继续");
                Dictionary["Chinese"].TryAdd("Settings", "设置");
                Dictionary["Chinese"].TryAdd("Write a review", "写评论");
                Dictionary["Chinese"].TryAdd("Difficulty", "难度");
                Dictionary["Chinese"].TryAdd("Easy", "简单");
                Dictionary["Chinese"].TryAdd("Medium", "中等");
                Dictionary["Chinese"].TryAdd("Hard", "难");
                Dictionary["Chinese"].TryAdd("Effects volume", "效果音量");
                Dictionary["Chinese"].TryAdd("Music volume", "音乐音量");
                Dictionary["Chinese"].TryAdd("Play time: {0:N1} h.", "游戏时间：{0:N1}小时。");
                Dictionary["Chinese"].TryAdd("SKIP", "跳过");
                Dictionary["Chinese"].TryAdd("premium without ads", "无广告高级版");
                Dictionary["Chinese"].TryAdd("100% Add free", "100%无广告");
                Dictionary["Chinese"].TryAdd("skip video", "跳过视频");
                Dictionary["Chinese"].TryAdd("loading assets...", "加载资源...");
                Dictionary["Chinese"].TryAdd("loading player data…", "加载玩家数据...");
                Dictionary["Chinese"].TryAdd("loading scene...", "加载场景...");
                Dictionary["Chinese"].TryAdd("starting…", "开始...");
                Dictionary["Chinese"].TryAdd("PLAY", "玩");
                Dictionary["Chinese"].TryAdd("SHOP", "商店");
                Dictionary["Chinese"].TryAdd("RATE US", "评价我们");
                Dictionary["Chinese"].TryAdd("SELECTED", "选择");
                Dictionary["Chinese"].TryAdd("OK", "好");
                Dictionary["Chinese"].TryAdd("Go Premium without Ads", "无广告高级版");
                Dictionary["Chinese"].TryAdd("Demo levels completed Purchase full version to continue", "演示关卡完成 购买完整版以继续");
                Dictionary["Chinese"].TryAdd("Connection Lost!", "连接丢失！");
                Dictionary["Chinese"].TryAdd("Unable to load the game.", "无法加载游戏。");
                Dictionary["Chinese"].TryAdd("Please check your internet connection.", "请检查您的互联网连接。");
                Dictionary["Chinese"].TryAdd("Try again", "再试一次");




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
                    Debug.LogError(e.Message);
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
                  //  Read();
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

        private static void AddLanguageKeys(string language)
        {
            if (!Dictionary.ContainsKey(language))
            {
                Dictionary.TryAdd(language, new Dictionary<string, string>());
            }
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