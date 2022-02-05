using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.ResourceLocalization;

namespace WebMaze.Services.GuessTheNumber
{
    public enum GuessTheNumberGameDifficulty
    {
        [Display(Name = "Easy" , ResourceType = typeof (WebMaze.ResourceLocalization.GuessTheNumber))]
        Easy = 1,
        [Display(Name = "Medium", ResourceType = typeof(WebMaze.ResourceLocalization.GuessTheNumber))]
        Medium = 2,
        [Display(Name ="Hard", ResourceType = typeof(WebMaze.ResourceLocalization.GuessTheNumber))]
        Hard = 3
    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this GuessTheNumberGameDifficulty enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());

            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            return attributes != null && attributes.Length > 0
                    ? attributes[0].GetName()
                    : enumValue.ToString();
        }
    }

    //public static class DifficultyExt
    //{
    //    public static string AsDisplayString(this GuessTheNumberGameDifficulty difficulty)
    //    {
    //        switch (difficulty)
    //        {
    //            case GuessTheNumberGameDifficulty.Easy: return ResourceLocalization.GuessTheNumber.Easy;
    //            case GuessTheNumberGameDifficulty.Medium: return ResourceLocalization.GuessTheNumber.Medium;
    //            case GuessTheNumberGameDifficulty.Hard: return ResourceLocalization.GuessTheNumber.Hard;

    //            default: throw new ArgumentOutOfRangeException("difficulty");
    //        }
    //    }
    //}
}
