﻿using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace sauceDemo
{
    /// <summary>
    /// Basse Page with common functions to all pages
    /// </summary>
    public class BasePage
    {
        protected IPage Page;

        private string _burgerMenuId = "#react-burger-menu-btn";
        private string _logoutMenuItem = "#logout_sidebar_link";
        private string _shoppingCartBadge = "span.shopping_cart_badge";

        public BasePage(IPage page)
        {
            this.Page = page;
        }

        /// <summary>
        /// Top Shoping cart icon 
        /// </summary>
        public IElementHandle ShopingCartIcon => Page.QuerySelectorAsync(_shoppingCartBadge).Result;

        /// <summary>
        /// Total the items in the shopping cart
        /// </summary>
        public int ItemsInShoppingCart
        {
            get
            {
               var element = Page.QuerySelectorAsync(_shoppingCartBadge).Result;
               if (element!=null)
                    return int.Parse(element.TextContentAsync().Result);
               else
                    return 0;
            }
        }

        /// <summary>
        /// Take screenshot
        /// </summary>
        /// <param name="name">Name of the image to save</param>
        /// <remarks>Can be optional for some oncloud </remarks>
        /// <returns></returns>
        public async Task TakeScreenShootAsync(string name)
        {
            var screenImage = System.IO.Path.Combine(Initialize.TestContext.DeploymentDirectory, name + "-" + Guid.NewGuid().ToString() + ".png");
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenImage, FullPage = true});

            if (!System.IO.File.Exists(screenImage))
            {
                throw new ArgumentException("Error", nameof(screenImage));
            }
            Initialize.TestContext.AddResultFile(screenImage);  
        }

        /// <summary>
        /// Click in the hamburger menu
        /// </summary>
        /// <returns></returns>
        public async Task ClickMenu() => await Page.ClickAsync(_burgerMenuId);

        public async Task Logout()
        {
            await ClickMenu();
            await Page.ClickAsync(_logoutMenuItem);
        }

    }
}