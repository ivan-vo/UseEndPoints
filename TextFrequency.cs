using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace endpoints
{
    public class TextFrequensy
    {
        Dictionary<string, int> words;
        private string text;
        public TextFrequensy(string text)
        {
            this.text = text;
        }
        public Dictionary<string, int> GetFrequensy()
        {
            Dictionary<string, int> frequensy = new Dictionary<string, int>();
            string[] words = Regex.Matches(text, "\\w+")
                .OfType<Match>()
                .Select(m => m.Value)
                .ToArray();
            foreach(var word in words)
            {  
                if(!frequensy.Keys.Contains(word.ToLower()))
                {   
                    frequensy.Add(word.ToLower(), 1);
                }
                else
                {
                    frequensy[word.ToLower()]++;
                }
            }
            return frequensy;
        }
        public string NumberOfUniquUords()
        {
            int count = 0;  
            foreach(var word in GetFrequensy())
            {
                if(word.Value == 1)
                {   
                    count++;
                }
            }
            return count.ToString();
        }
        public string TheMostCommonWord()
        {
            string theMostCommonWord = null;
            foreach(var word in GetFrequensy())
            {
                if(theMostCommonWord == null) 
                {
                    theMostCommonWord = word.Key;
                }
                else if(word.Value > GetFrequensy()[theMostCommonWord])
                {   
                    theMostCommonWord = word.Key;
                }
            }
            return theMostCommonWord;

        }
    }
}
