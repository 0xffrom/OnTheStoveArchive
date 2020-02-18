using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Objects;

namespace RecipeLibrary
{
    public partial class GetData
    {
        public RecipeFull RecipeFull;
        public List<RecipeShort> RecipeShorts { get; private set; } = new List<RecipeShort>();
        public bool IsCompleted = false;
        private const int CountOfSites = 2;

        private void Parser_OnNewData(object arg, RecipeFull recipeFull)
        {
            RecipeFull = recipeFull;
            IsCompleted = true;
        }

        private int _countOfSites = CountOfSites;
        private static readonly Random Rng = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);

        private void Parser_OnNewData(object arg, RecipeShort[] list)
        {
            _countOfSites--;

            foreach (var item in list)
                RecipeShorts.Add(item);

            if (_countOfSites != 0) return;

            // Random Sort
            RecipeShorts = RecipeShorts.Select(i => new {I = i, sort = Rng.Next()}).OrderBy(i => i.sort)
                .Select(i => i.I).ToList();

            IsCompleted = true;
        }
    }
}