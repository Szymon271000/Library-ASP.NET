using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class ChapterNotFoundException : Exception
    {
        public ChapterNotFoundException(int chapterId) : base($"Chapter {chapterId} not found")
        {
        }
    }
}
