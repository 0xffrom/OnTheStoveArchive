using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeLibrary.Objects;

namespace RecipeLibrary
{
    public partial class GetData
    {
        public RecipeFull RecipeFull;
        public List<RecipeShort> RecipeShorts { get; private set; } = new List<RecipeShort>();

        internal delegate Task IsCompletedDelegate();
        internal event IsCompletedDelegate IsCompleted;
        
        private const int CountOfSites = 3;

        private void Parser_OnNewData(object arg, RecipeFull recipeFull)
        {
            RecipeFull = recipeFull;
            IsCompleted?.Invoke();
        }

        
        // TODO: Доделать нормальное ожидание.
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

            IsCompleted?.Invoke();
        }
    }
}