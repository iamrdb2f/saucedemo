﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using sauceDemo.Components;

namespace sauceDemo.Pages;

public class CheckoutStep2Page : BasePage
{
   
    private Button _finishButton;
    public CartItems ListCartItems;

    public CheckoutStep2Page(IPage page) : base(page)
    {
        _finishButton =new Button(page, "data-test=finish");
        ListCartItems = new CartItems(this.Page);
    }

    /// <summary>
    /// Cart Items
    /// </summary>
    public List<CartItem> Items
    {
        get
        {
            return ListCartItems.Items;
        }
    }

    /// <summary>
    /// Click in finish button
    /// </summary>
    /// <returns></returns>
    public async Task CickFinishAsync()
    {
        await _finishButton.ClickAsync();
        await TakeScreenShootAsync("finishClick");
    }
}
