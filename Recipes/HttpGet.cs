﻿using System.Net;
using System.Net.Http;
using RecipesAndroid.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecipesAndroid
{
    public static class HttpGet
    {
        private static async Task<string> GetSource()
        {
            string currentUrl = "http://45.132.17.35/getPage?section=new";

            var client = new HttpClient();

            string source = string.Empty;

            var response = await client.GetAsync(currentUrl);

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }

        public static RecipeShort[] GetRecipes()
        {
            string source = GetSource().Result;

            List<RecipeShort> recipes = JsonConvert.DeserializeObject<List<RecipeShort>>(source);

            return recipes.ToArray();

        }
    }
}