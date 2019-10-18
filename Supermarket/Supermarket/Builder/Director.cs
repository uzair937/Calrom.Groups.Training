﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Director
    {
        private readonly IFruitBuilder fruitBuilder;

        public Director(IFruitBuilder builder)
        {
            this.fruitBuilder = builder;
        }
        private static Director Instance = null;
        private static readonly object padlock = new object();
        public static Director getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new Director(new ConcreteBuilder());
                        }
                    }
                }
                return Instance;
            }
        }

        public void Construct(string input)
        {
            switch(input)
            {
                case "A":
                    fruitBuilder.BuildApple();
                    break;
                case "B":
                    fruitBuilder.BuildBanana();
                    break;
                case "O":
                    fruitBuilder.BuildOrange();
                    break;
                default:
                    break;
            }
        }

        public Fruit getFruit()
        {
            return fruitBuilder.GetFruit();
        }
    }
}
