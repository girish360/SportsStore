using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Binders {
    public class CartModelBinder : IModelBinder {
        private string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            Cart cart = (Cart)controllerContext.RequestContext.HttpContext.Session[sessionKey];
            if (cart == null) {
                cart = new Cart();
                controllerContext.RequestContext.HttpContext.Session[sessionKey] = cart;
            }
            return cart;
        }
    }
}