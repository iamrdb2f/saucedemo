﻿using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using sauceDemo.Base;
using sauceDemo.Pages;

namespace sauceDemo.Tests
{
    [Parallelizable]
    public class ShopTests
    {
        private IPage _page;
        private LoginPage _loginPage;
        private InventoryPage _inventoryPage;

        [SetUp]
        public async Task Setup()
        {
            PlaywrightDriver playwrightDriver = new PlaywrightDriver();
            _page = await playwrightDriver.InitalizePlaywright();
            _loginPage = new LoginPage(_page);
            await _loginPage.GotoAsync();
            await _loginPage.LoginAsync(Constants.STANDARD_USER, Constants.GENERIC_PASSWORD);
            _inventoryPage = new InventoryPage(_page);
        }

        [Test, Category("Shop")]
        public async Task AddItemsToShop_CompletePurchsse_ShouldShowsConfirmationPageAsync()
        {
            //Arrange
            int total = 2;
            await _inventoryPage.AddItemsAsync(total);
            //Act
            CartPage cartPage = new CartPage(_page);
            await _inventoryPage.ShopingCartIcon.ClickAsync();
            await cartPage.ClickCheckoutAsync();
            CheckoutStep1Page checkoutStep1 = new CheckoutStep1Page(_page);
            await checkoutStep1.SetFirstNameAsync("Abigail");
            await checkoutStep1.SetLastNameAsync("Armijo");
            await checkoutStep1.SetPostalCodeAsync("27140");
            await checkoutStep1.ClickContinueAsync();
            CheckoutStep2Page checkoutStep2 = new CheckoutStep2Page(_page);
            //Assert
            Assert.AreEqual(total, checkoutStep2.ItemsInShoppingCart);
            for (int i = 0; i < total; i++)
            {
                checkoutStep2.CheckCartItem(_inventoryPage.ItemsName[i]);
            }
            await checkoutStep2.CickFinishAsync();
            //Additional assert to check complete
            CheckoutCompletePage checkoutComplete = new CheckoutCompletePage(_page);
            Assert.AreEqual("THANK YOU FOR YOUR ORDER", checkoutComplete.Thanks);
        }
    }
}