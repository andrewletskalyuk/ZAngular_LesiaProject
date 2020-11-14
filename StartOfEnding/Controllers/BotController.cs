using ClassLibraryDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StartOfEnding.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StartOfEnding.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqliteContext _context;
        public BotController(SqliteContext sqliteContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = sqliteContext;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Update update)
        {
            try
            {
                var token = _configuration.GetSection("TelegramBot:token").Value;
                TelegramBotHelper botHelper = new TelegramBotHelper(token, _context);
                botHelper.GetUpdates();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok();
        }

    }
}
