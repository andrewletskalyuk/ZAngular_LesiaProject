using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectDataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;
using ZVersion.Services;

namespace ZVersion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqLiteContextUsers _context;
        public BotController(SqLiteContextUsers sqliteContext, IConfiguration configuration)
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
