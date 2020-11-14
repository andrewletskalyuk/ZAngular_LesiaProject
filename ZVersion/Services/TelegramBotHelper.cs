using BaseProjectDataContext;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZVersion.Services
{
    public class TelegramBotHelper
    {
        private const string text_1Appointments = "🕘 Хто на прийом сьогодні:";
        private const string text_2AppointmentPacient = "👩🏻‍⚕️‍ Детальна інфа, зі списку";
        private const string text_3Pacients = "🧑🏼‍🤝‍🧑🏼 Пацієнти за останній тиждень";
        private const string text_4Details = "📘 Детальна інфа про пацієнтів";
        private bool firstTime = true;
        private bool forSearch = false;
        private string _token;
        TelegramBotClient _client;
        private readonly SqLiteContextUsers _context;

        public TelegramBotHelper(string token, SqLiteContextUsers sqliteContext)
        {
            this._token = token;
            _context = sqliteContext;
        }

        internal void GetUpdates()
        {
            _client = new TelegramBotClient(_token);
            //після першого разу вирубаємо webHook
            _client.DeleteWebhookAsync();
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;
                while (true)
                {
                    try
                    {
                        var updates = _client.GetUpdatesAsync(offset).Result;
                        if (updates != null && updates.Count() > 0)
                        {
                            foreach (var update in updates)
                            {
                                //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                                processUpdate(update);
                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        private void processUpdate(Telegram.Bot.Types.Update update)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    if (firstTime)
                    {
                        _client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                        firstTime = !firstTime;
                    }
                    if (forSearch)
                    {
                        var surNameOfPacient = update.Message.Text;
                        var pacientsWithObj = _context.Pacients.Include(x => x.Objective);
                        GetDetailsAboutPacient(surNameOfPacient, update, pacientsWithObj);

                        //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                        forSearch = !forSearch;
                    }
                    string text = update.Message.Text;
                    switch (text)
                    {
                        case "/start":
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                        case text_1Appointments:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "На сьогодні записані(ий):");
                            GetDataFromDB(text, update);
                            // _client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                        case text_2AppointmentPacient:
                            //видаємо список для отримання детальної інформації про пацієнта
                            ListAppointments(text, update);

                            //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                        case text_3Pacients:
                            PacientsByLastWeek(text, update);
                            //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                        case text_4Details:
                            _client.SendTextMessageAsync(update.Message.Chat.Id
                                , "👩🏻‍⚕️ Введіть прізвище пацієнта для пошуку (прізвище може бути неповне, тоді результат буде у вигляді списку)");
                            forSearch = true; //це якщо підтверджуєтья пошук пацієнта
                            //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                        default:
                            //можливо цього не потрібно
                            //_client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
                            break;
                    }
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    //якщо отримали колбек - працюємо з даними для 
                    int number;
                    bool res = int.TryParse(update.CallbackQuery.Data, out number);
                    if (res)
                    {
                        var WhoOnAppToday = Int32.Parse(update.CallbackQuery.Data) - 1;
                        var TodayAppointments = _context.Appointments.Where(x => x.DateWhenAdded.Day == DateTime.Now.Day).ToArray();
                        if (TodayAppointments != null && TodayAppointments.Length > 0)
                        {
                            var ourAppointmentForAnswer = TodayAppointments[WhoOnAppToday];
                            //тут відсилаємо відповідь про людину, що записалася на прийом
                            SendDetailsAboutAppointment(update, ourAppointmentForAnswer);
                        }
                    }

                    break;
                default:
                    Console.WriteLine(update.Type + " not implement");
                    break;
            }
        }

        private void GetDetailsAboutPacient(string surNamePacient, Update update, IEnumerable<Pacient> _pacients)
        {
            //класичний запит, коли IEnumerable взяли з БД, а на стороні клієнта смалим пошук
            //а не навпаки
            var pacients = _pacients.ToList();
            var res = pacients.FindAll(x => x.Surname.Contains(surNamePacient));
            if (res != null && res.Count > 0)
            {
                if (res.Count == 1)
                {
                    var resPac = res[0];
                    _client.SendTextMessageAsync(update.Message.Chat.Id, "📰 ПІБ: "
                        + resPac.Surname + " " + resPac.Name + " " + resPac.Patronymic
                        + "  📅 Дата народження: " + resPac.Birthday.Year.ToString() + "/"
                        + resPac.Birthday.Month + "/"
                        + resPac.Birthday.Day
                        + " Анамнез: " + resPac.Anamnesis
                        + ", StatusLocalic: " + resPac.StatusLocalic
                        + ", Ріст(см): " + resPac.Objective.Height.ToString()
                        + ", Маса(кг): " + resPac.Objective.Weight
                        + ", IMT: " + resPac.Objective.IMT);
                }
                else
                {
                    int a = 1;
                    foreach (Pacient pac in res)
                    {
                        _client.SendTextMessageAsync(update.Message.Chat.Id, "___________");
                        _client.SendTextMessageAsync(update.Message.Chat.Id, a + ") 📰 ПІБ: "
                        + pac.Surname + " " + pac.Name + " " + pac.Patronymic
                        + "  📅 Дата народження: " + pac.Birthday.Year.ToString() + "/"
                        + pac.Birthday.Month + "/"
                        + pac.Birthday.Day
                        + " Анамнез: " + pac.Anamnesis
                        + ", StatusLocalic: " + pac.StatusLocalic
                        + ", Ріст(см): " + pac.Objective.Height.ToString()
                        + ", Маса(кг): " + pac.Objective.Weight
                        + ", IMT: " + pac.Objective.IMT);
                        Thread.Sleep(500);
                        a++;
                    }
                    a = 1;
                }
            }
            else
            {
                _client.SendTextMessageAsync(update.Message.Chat.Id, "🥺 пацінта(ів) з даним прізвищем в базі не знайдено!!!");
            }
        }

        private void PacientsByLastWeek(string text, Update update)
        {
            var lastWeek = DateTime.Now.Day - 7;
            var pacients = _context.Pacients.AsEnumerable().
                            Where(x => x.AddDay.DayOfYear > DateTime.Now.DayOfYear - 7);
            if (pacients != null && pacients.ToArray().Length > 0)
            {
                int a = 1;
                foreach (Pacient pacient in pacients)
                {
                    _client.SendTextMessageAsync(update.Message.Chat.Id, a + ") 🙂 ПІБ: "
                        + pacient.Surname + " "
                        + pacient.Name + " "
                        + pacient.Birthday.Year.ToString() + "/"
                        + pacient.Birthday.Month + "/"
                        + pacient.Birthday.Day);
                    a++;
                }
                a = 1;
                _client.SendTextMessageAsync(update.Message.Chat.Id, "", replyMarkup: GetButtons());
            }
        }

        //детальна інформація про людину, що записалася на  прийом
        private void SendDetailsAboutAppointment(Update update, Appointment ourAppointmentForAnswer)
        {
            _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "👀: " + ourAppointmentForAnswer.Name);
            _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "📱 -" + ourAppointmentForAnswer.Phone);
            _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "📧 - " + ourAppointmentForAnswer.Email);
            _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "🆔 - " + ourAppointmentForAnswer.Id.ToString());
            _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "", replyMarkup: GetButtons());
        }

        private IReplyMarkup GetInlineButtonsApp(long id, string name)
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton { Text = name + " - детальніше", CallbackData = id.ToString() });
        }

        private void ListAppointments(string text, Update update)
        {
            _client.SendTextMessageAsync(update.Message.Chat.Id, "Клікніть по імені пацієнта, для отримання детальної інформації");
            var date = DateTime.Today.Day;
            var resAppointments = _context.Appointments.Where(e => e.DateWhenAdded.Day == date).ToArray();
            if (resAppointments.Length > 0 && resAppointments != null)
            {

                long len = 1;
                foreach (var app in resAppointments)
                {
                    _client.SendTextMessageAsync(update.Message.Chat.Id, len + ")" + app.Name
                        + " записаний(-на) сьогодні на: "
                        + app.DateWhenAdded.TimeOfDay.ToString(),
                        replyMarkup: GetInlineButtonsApp(len, app.Name));
                    len++;
                }
                len = 0;
            }
            else
            {
                _client.SendTextMessageAsync(update.Message.Chat.Id, "Сьогодні записів на прийом немає");
            }
        }

        private IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        new KeyboardButton {
                            Text = text_1Appointments
                        },
                        new KeyboardButton {
                            Text = text_2AppointmentPacient
                        }
                    },
                    new List<KeyboardButton>
                    {
                        new KeyboardButton
                        {
                            Text = text_3Pacients
                        },
                        new KeyboardButton
                        {
                            Text = text_4Details
                        }
                    }
                },
                ResizeKeyboard = true
            };
        }

        private void GetDataFromDB(string text, Update update)
        {
            var date = DateTime.Today.Day;
            var resAppointments = _context.Appointments.Where(e => e.DateWhenAdded.Day == date).ToArray();
            if (resAppointments.Length > 0 && resAppointments != null)
            {

                int len = 1;
                foreach (var app in resAppointments)
                {
                    _client.SendTextMessageAsync(update.Message.Chat.Id, len + ")" + app.Name
                        + " записаний сьогодні на: "
                        + app.DateWhenAdded.TimeOfDay.ToString());
                    len++;
                }
                len = 0;
            }
            else
            {
                _client.SendTextMessageAsync(update.Message.Chat.Id, "🥺 Сьогодні на прийом немає");
            }
        }

        private List<Appointment> GetAppointmentsOnToday()
        {
            var date = DateTime.Today.Day;
            var resAppointments = _context.Appointments.Where(e => e.DateWhenAdded.Day == date).ToArray();
            return resAppointments.ToList().Count > 0 ? resAppointments.ToList() : null;
        }
    }
}
