using Biblioteka.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Controllers
{
    public class AppController : Controller
    {
        public ViewResult View(object obj,MessageType messageType,string text)
        {
            AddMessage(messageType, text);
            return base.View(obj);
        }
        public ViewResult View(string viewName,object obj, MessageType messageType, string text)
        {
            AddMessage(messageType, text);
            return base.View(viewName,obj);
        }
        private void AddMessage(MessageType messageType,string text)
        {
            if (messageType == MessageType.Success)
            {
                ViewData["success-msg"] = text;
            }
        }
    }
}
